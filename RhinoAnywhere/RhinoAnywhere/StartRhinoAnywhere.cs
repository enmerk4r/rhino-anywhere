﻿using Rhino;
using Rhino.Commands;
using Rhino.Display;
using System;
using System.Drawing;

namespace RhinoAnywhere
{
  public sealed partial class StartRhinoAnywhere : Command
  {
    private static Server Server { get; set; }

    public override string EnglishName => "StartRhinoAnywhere";

    protected override Result RunCommand(RhinoDoc doc, RunMode mode)
    {
      Server?.Dispose();
      var random = new Random();
      var portNumber = (int)((random.NextDouble() * 50) + 2330);
      Server = new Server(portNumber);
      return Result.Success;
    }

    static StartRhinoAnywhere()
    {
      RegisterPipelineCall();
    }

    private static void RegisterPipelineCall()
    {
      DisplayPipeline.DrawOverlay += DisplayPipeline_PostDrawObjects;
    }

    private static DateTime LastCall { get; set; } = DateTime.UtcNow;

    private static void DisplayPipeline_PostDrawObjects(object sender, DrawEventArgs e)
    {
      if (Server is null)
        return;

      Server.DurationUnits = (uint)DateTime.UtcNow.Subtract(LastCall).TotalMilliseconds;
      LastCall = DateTime.UtcNow;

      Bitmap LastBitMap = e.Display.FrameBuffer;
      Server?.SendBitmap(LastBitMap);
    }
  }
}
