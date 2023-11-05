using Org.BouncyCastle.Utilities;
using Rhino;
using Rhino.Commands;
using Rhino.Display;
using RhinoAnywhere.DataStructures;
using SIPSorcery.Media;
using SIPSorcery.Net;
using SIPSorceryMedia.Abstractions;
using SIPSorceryMedia.Encoders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace RhinoAnywhere
{


    public sealed partial class StartRhinoAnywhere : Command
    {
        private const int WEBSOCKET_PORT = 2337;

        private static uint DurationUnits { get; set; } = 1;

        public override string EnglishName => nameof(StartRhinoAnywhere);

        static private RTCPeerConnection Connection { get; set; }
        static private WebSocketServer SocketServer { get; set; }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            if (Connection is not null)
                Connection.Close("Because I said so");

            if (SocketServer is not null)
                SocketServer.Stop();

            RhinoApp.WriteLine("WebRTC Get Started");
            RhinoApp.WriteLine("Starting web socket server...");

            SocketServer = new WebSocketServer(IPAddress.Any, WEBSOCKET_PORT);
            SocketServer.AddWebSocketService<WebRTCWebSocketPeer>("/", (peer) => peer.CreatePeerConnection = () => CreatePeerConnection());
            SocketServer.Start();

            RhinoApp.WriteLine($"Waiting for web socket connections on {SocketServer.Address}:{SocketServer.Port}...");

            // Probably
            return Result.Success;
        }

        private readonly static VpxVideoEncoder Encoder;
        static StartRhinoAnywhere()
        {
            RegisterPipelineCall();
            Encoder = new VpxVideoEncoder();
        }

        private static void RegisterPipelineCall()
        {
            DisplayPipeline.DrawOverlay += DisplayPipeline_PostDrawObjects;
        }

        private static DateTime LastCall { get; set; } = DateTime.UtcNow;

        private static void DisplayPipeline_PostDrawObjects(object sender, DrawEventArgs e)
        {
            DurationUnits = (uint)DateTime.UtcNow.Subtract(LastCall).TotalMilliseconds;
            LastCall = DateTime.UtcNow;

            if (Connection is null)
                return;

            if (SocketServer is null)
                return;

            RhinoView activeView = RhinoDoc.ActiveDoc.Views.ActiveView;

            var size = activeView.Size;
            using (var bitmap = new Bitmap(size.Width, size.Height))
            {
                Bitmap LastBitMap = e.Display.FrameBuffer;
                SendBitmap(LastBitMap);
            }
        }


        private Task<RTCPeerConnection> CreatePeerConnection()
        {
            Connection = new RTCPeerConnection(null);

            var testPatternSource = new VideoTestPatternSource(Encoder);

            MediaStreamTrack videoTrack = new MediaStreamTrack(testPatternSource.GetVideoSourceFormats(), MediaStreamStatusEnum.SendOnly);
            Connection.addTrack(videoTrack);

            // testPatternSource.OnVideoSourceEncodedSample += connection.SendVideo;
            Connection.OnVideoFormatsNegotiated += (formats) => testPatternSource.SetVideoSourceFormat(formats.First());

            Connection.onconnectionstatechange += async (state) =>
            {
                Console.WriteLine($"Peer connection state change to {state}.");

                switch (state)
                {
                    case RTCPeerConnectionState.connected:
                        await testPatternSource.StartVideo();
                        break;
                    case RTCPeerConnectionState.failed:
                        Connection.Close("ice disconnection");
                        break;
                    case RTCPeerConnectionState.closed:
                        await testPatternSource.CloseVideo();
                        testPatternSource.Dispose();
                        break;
                }
            };

            Connection.createDataChannel("test");

            Connection.ondatachannel += (channel) =>
            {
                channel.onmessage += (test1, something, data) =>
                {
                    string json = System.Text.Encoding.UTF8.GetString(data);
                    var tst = JsonSerializer.Deserialize<JsonObject>(json);

                    string type = tst["type"].ToString();
                    Action<string> method = type switch
                    {
                        "command" => HandleCommand,
                        "input" => HandleClick,
                        "resize" => HandleResize,
                        _ => throw new NotImplementedException("No"),
                    };

                    method(json);
                };
            };

            return Task.FromResult(Connection);
        }

        private string lastcommand { get; set; }
        private void HandleCommand(string json)
        {
            var clickPacket = JsonSerializer.Deserialize<Packet<CommandData>>(json);
            lastcommand = clickPacket.data.command;
            RhinoApp.WriteLine(lastcommand);
            RhinoApp.Idle += RhinoApp_Idle;
        }

        private void RhinoApp_Idle(object sender, EventArgs e)
        {
            RhinoApp.Idle -= RhinoApp_Idle;
            RhinoApp.RunScript(lastcommand, true);
        }

        private void HandleClick(string json)
        {
            var clickPacket = JsonSerializer.Deserialize<Packet<MouseData>>(json);
            InputRecieved(clickPacket);
        }

        private void HandleResize(string json)
        {
            var viewportSize = JsonSerializer.Deserialize<ViewportSize>(json);
            RhinoDoc.ActiveDoc.Views.ActiveView.Size = new Size((int)viewportSize.Width, (int)viewportSize.Height);
        }

        private static void SendBitmap(Bitmap bitmap)
        {
            // DO NOT USE SENDFASTER

            var rect = new Rectangle(new Point(0, 0), new Size(bitmap.Width, bitmap.Height));
            var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr ptr = bitmapData.Scan0;
            int bytes = bitmapData.Stride * bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Connection.SendVideo(DurationUnits, Encoder.EncodeVideo(bitmap.Width, bitmap.Height, rgbValues, VideoPixelFormatsEnum.Bgra, VideoCodecsEnum.H265));
        }

        private void InputRecieved(Packet<MouseData> inputArgs)
        {
            if(inputArgs.type == "input")
            {

                string val = inputArgs.data.value;

                string left = "0";
                string right = "2";

                if (inputArgs.data.action == "up" && val == left)
                {
                    MouseController.MouseEvent(MouseController.MouseEventFlags.LeftUp);
                }
                else if (inputArgs.data.action == "down" && val == left)
                {
                    MouseController.MouseEvent(MouseController.MouseEventFlags.LeftDown);
                }
                else if (inputArgs.data.action == "down" && val == right)
                {
                    MouseController.MouseEvent(MouseController.MouseEventFlags.RightDown);
                }
                else if (inputArgs.data.action == "up" && val == right)
                {
                    MouseController.MouseEvent(MouseController.MouseEventFlags.RightUp);
                }
                else if (inputArgs.data.action == "move")
                {
                    double newX = inputArgs.data.x + inputArgs.data.deltax;
                    double newY = inputArgs.data.y + inputArgs.data.deltay;

                    var pt = DisplayController.WebViewToServerWindowCoordinate(newY, newX);
                    MouseController.SetCursorPosition((int)pt.X, (int)pt.Y);
                }
            }
        }
    }
}
