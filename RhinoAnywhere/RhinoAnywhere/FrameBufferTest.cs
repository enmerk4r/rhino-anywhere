using System;
using Rhino;
using Rhino.Commands;

namespace RhinoAnywhere
{
    public class FrameBufferTest : Command
    {
        public FrameBufferTest()
        {
            Instance = this;
        }

        ///<summary>The only instance of the MyCommand command.</summary>
        public static FrameBufferTest Instance { get; private set; }

        public override string EnglishName => "FrameBufferTest";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            return Result.Success;
        }
    }
}