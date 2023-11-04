using System;
using Rhino;
using Rhino.Commands;

namespace RhinoAnywhere
{
    public class RhinoAnywhereTestpad : Command
    {
        public RhinoAnywhereTestpad()
        {
            Instance = this;
        }

        ///<summary>The only instance of the MyCommand command.</summary>
        public static RhinoAnywhereTestpad Instance { get; private set; }

        public override string EnglishName => "RhinoAnywhereTestpad";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("Start testpad");

            // TODO: complete command.
            return Result.Success;
        }
    }
}