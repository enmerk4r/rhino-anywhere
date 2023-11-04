using System;
using Rhino;
using Rhino.Commands;
using RhinoAnywhereTestpad;

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

            TestpadWindow wnd = new TestpadWindow();
            wnd.Show();

            RhinoApp.WriteLine("Start testpad");

            // TODO: complete command.
            return Result.Success;
        }
    }
}