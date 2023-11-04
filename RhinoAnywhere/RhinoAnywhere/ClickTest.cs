using System;
using Rhino;
using Rhino.Commands;

namespace RhinoAnywhere
{
    public class ClickTest : Command
    {
        public ClickTest()
        {
            Instance = this;
        }

        ///<summary>The only instance of the MyCommand command.</summary>
        public static ClickTest Instance { get; private set; }

        public override string EnglishName => "ClickTest";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            return Result.Success;
        }
    }
}