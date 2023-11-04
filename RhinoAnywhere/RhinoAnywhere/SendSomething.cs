using Org.BouncyCastle.Utilities;
using Rhino;
using Rhino.Commands;
using Rhino.Display;
using SIPSorcery.Media;
using SIPSorcery.Net;
using SIPSorceryMedia.Abstractions;
using SIPSorceryMedia.Encoders;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
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

        public struct ClickPacket
        {
            public string type { get; set; }
            public ClickData data { get; set; }
        }

        public struct ClickData
        {
            public string method { get; set; }
            public string action { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int deltax { get; set; }
            public int deltay { get; set; }
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
                    var clickPacket = JsonSerializer.Deserialize<ClickPacket>(json);
                    ;

                    RhinoApp.WriteLine($"Got x:{clickPacket.data.x} y:{clickPacket.data.y} from client");
                    //InputRecieved(clickPacket);
                };
            };

            return Task.FromResult(connection);
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

        private void InputRecieved(ClickPacket inputArgs)
        {
            if(inputArgs.type == "input")
            {
                if (inputArgs.data.method == "leftup")
                {
                    MouseController.MouseEvent(MouseController.MouseEventFlags.LeftUp);
                }
                else if (inputArgs.data.method == "leftdown")
                {
                    MouseController.MouseEvent(MouseController.MouseEventFlags.LeftDown);
                }
                else if (inputArgs.data.method == "rightdown")
                {
                    MouseController.MouseEvent(MouseController.MouseEventFlags.RightDown);
                }
                else if (inputArgs.data.method == "rightup")
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
