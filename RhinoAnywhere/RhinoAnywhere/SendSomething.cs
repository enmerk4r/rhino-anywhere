using Org.BouncyCastle.Utilities;
using Rhino;
using Rhino.Commands;
using Rhino.Display;
using RhinoAnywhereCore;
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
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace RhinoAnywhere
{
    public sealed class SendSomething : Command
    {
        private const int WEBSOCKET_PORT = 8081;
        private uint durationUnits => 16;

        public override string EnglishName => nameof(SendSomething);

        private RTCPeerConnection connection { get; set; }
        private WebSocketServer webSocketServer { get; set; }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Rhino.Display.DisplayPipeline.DrawForeground += DisplayPipeline_PostDrawObjects;

            if (connection is not null)
                connection.Close("Because I said so");

            if (webSocketServer is not null)
                webSocketServer.Stop();

            RhinoApp.WriteLine("WebRTC Get Started");
            RhinoApp.WriteLine("Starting web socket server...");

            webSocketServer = new WebSocketServer(IPAddress.Any, WEBSOCKET_PORT);
            webSocketServer.AddWebSocketService<WebRTCWebSocketPeer>("/", (peer) => peer.CreatePeerConnection = () => CreatePeerConnection());
            webSocketServer.Start();

            RhinoApp.WriteLine($"Waiting for web socket connections on {webSocketServer.Address}:{webSocketServer.Port}...");

            // Probably
            return Result.Success;
        }

        private void DisplayPipeline_PostDrawObjects(object sender, DrawEventArgs e)
        {
            if (connection is null)
                return;

            if (webSocketServer is null)
                return;

            // TODO: complete command.
            RhinoView activeView = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;

            var encoder = new VpxVideoEncoder();
            var size = activeView.Size;
            using (var bitmap = new Bitmap(size.Width, size.Height))
            {
                Bitmap outputBitmap = e.Display.FrameBuffer;
                SendBitmap(outputBitmap, encoder);
            }
        }

        public struct Packet<T>
        {
            public string type { get; set; }
            public T data { get; set; }
        }

        public struct MouseData
        {
            public string method { get; set; }
            public string action { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int deltax { get; set; }
            public int deltay { get; set; }
            public string value { get; set; }
        }

        public struct CommandData
        {
            public string command { get; set; }
        }

        public struct ViewportSize
        {
            public double Width { get; set; }
            public double Height { get; set; }
        }


        private Task<RTCPeerConnection> CreatePeerConnection()
        {
            connection = new RTCPeerConnection(null);

            var encoder = new VpxVideoEncoder();
            var testPatternSource = new VideoTestPatternSource(encoder);

            MediaStreamTrack videoTrack = new MediaStreamTrack(testPatternSource.GetVideoSourceFormats(), MediaStreamStatusEnum.SendOnly);
            connection.addTrack(videoTrack);

            // testPatternSource.OnVideoSourceEncodedSample += connection.SendVideo;
            connection.OnVideoFormatsNegotiated += (formats) => testPatternSource.SetVideoSourceFormat(formats.First());

            connection.onconnectionstatechange += async (state) =>
            {
                Console.WriteLine($"Peer connection state change to {state}.");

                switch (state)
                {
                    case RTCPeerConnectionState.connected:
                        await testPatternSource.StartVideo();
                        break;
                    case RTCPeerConnectionState.failed:
                        connection.Close("ice disconnection");
                        break;
                    case RTCPeerConnectionState.closed:
                        await testPatternSource.CloseVideo();
                        testPatternSource.Dispose();
                        break;
                }
            };

            connection.createDataChannel("test");

            connection.ondatachannel += (channel) =>
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

            return Task.FromResult(connection);
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
            RhinoApp.WriteLine($"Got x:{clickPacket.data.x} y:{clickPacket.data.y} from client");
        }

        private void HandleResize(string json)
        {
            var viewportSize = JsonSerializer.Deserialize<ViewportSize>(json);
            RhinoDoc.ActiveDoc.Views.ActiveView.Size = new Size((int)viewportSize.Width, (int)viewportSize.Height);
        }

        private void SendBitmap(Bitmap bitmap, IVideoEncoder encoder)
        {
            // DO NOT USE SENDFASTER

            var rect = new Rectangle(new Point(0, 0), new Size(bitmap.Width, bitmap.Height));
            var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr ptr = bitmapData.Scan0;
            int bytes = bitmapData.Stride * bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            // connection.SendVideo(durationUnits, encoder.EncodeVideo(640, 480, myBits(), VideoPixelFormatsEnum.Bgra, VideoCodecsEnum.H264));
            connection.SendVideo(durationUnits, encoder.EncodeVideo(bitmap.Width, bitmap.Height, rgbValues, VideoPixelFormatsEnum.Bgra, VideoCodecsEnum.H264));
        }

        private void InputRecieved(Packet<MouseData> inputArgs)
        {
            if(inputArgs.type == "input")
            {
                int val = int.Parse(inputArgs.data.value);
                int left = 0;
                int right = 2;

                if (inputArgs.data.method == "up" && val == left)
                {
                    MouseController.MouseEvent(MouseController.MouseEventFlags.LeftUp);
                }
                else if (inputArgs.data.method == "down" && val == left)
                {
                    MouseController.MouseEvent(MouseController.MouseEventFlags.LeftDown);
                }
                else if (inputArgs.data.method == "down" && val == right)
                {
                    MouseController.MouseEvent(MouseController.MouseEventFlags.RightDown);
                }
                else if (inputArgs.data.method == "up" && val == right)
                {
                    MouseController.MouseEvent(MouseController.MouseEventFlags.RightUp);
                }
                else if (inputArgs.data.method == "move")
                {
                    int newX = inputArgs.data.x + inputArgs.data.deltax;
                    int newY = inputArgs.data.y + inputArgs.data.deltay;

                    MouseController.SetCursorPosition(newX, newY);
                }
            }
        }
    }
}
