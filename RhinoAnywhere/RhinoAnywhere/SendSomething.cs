using Org.BouncyCastle.Utilities;
using Rhino;
using Rhino.Commands;
using SIPSorcery.Media;
using SIPSorcery.Net;
using SIPSorceryMedia.Abstractions;
using SIPSorceryMedia.Encoders;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace RhinoAnywhere
{
    public sealed class SendSomething : Command
    {
        private const int WEBSOCKET_PORT = 8081;
        private uint durationUnits => 1;

        public override string EnglishName => nameof(SendSomething);

        private RTCPeerConnection connection { get; set; }
        private WebSocketServer webSocketServer { get; set; }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            if (connection is not null)
                connection.Close("Because I said so");

            if (webSocketServer is not null)
                webSocketServer.Stop();

            RhinoApp.WriteLine("WebRTC Get Started");
            RhinoApp.WriteLine("Starting web socket server...");

            webSocketServer = new WebSocketServer(IPAddress.Any, WEBSOCKET_PORT);
            webSocketServer.AddWebSocketService<WebRTCWebSocketPeer>("/", (peer) => peer.CreatePeerConnection = () => CreatePeerConnection());
            webSocketServer.Start();

            RhinoApp.Idle += RhinoApp_Idle;

            RhinoApp.WriteLine($"Waiting for web socket connections on {webSocketServer.Address}:{webSocketServer.Port}...");

            // Probably
            return Result.Success;
        }

        private static byte[] myOtherBits { get; set; }
        private static byte[] myBits()
        {
            if (myOtherBits is not null)
                return myOtherBits;

            var h = 640;
            var w = 480;
            var bits = new byte[h * w * 4];
            for (int i = 0; i<h; i++)
            {
                for (int j = 0; j<w; j++)
                {
                    var pixIndex = (i * w + j) * 4;
                    bits[pixIndex + 0] = 0; // B
                    bits[pixIndex + 1] = 0; // G
                    bits[pixIndex + 2] = 255; // R
                    bits[pixIndex + 3] = 0; // A
                }
            }

            myOtherBits = bits;
            return myOtherBits;
        }

        private void RhinoApp_Idle(object sender, EventArgs e)
        {
            // RhinoApp.Idle -= RhinoApp_Idle;

            var encoder = new VpxVideoEncoder();
            connection.SendVideo(durationUnits, encoder.EncodeVideo(640, 480, myBits(), VideoPixelFormatsEnum.Bgra, VideoCodecsEnum.H264));
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

            RhinoApp.Idle += RhinoApp_Idle;

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

            connection.SendVideo(durationUnits, encoder.EncodeVideo(bitmap.Width, bitmap.Height, rgbValues, VideoPixelFormatsEnum.Rgb, VideoCodecsEnum.JPEG));
        }

    }
}
