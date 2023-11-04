using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using RhinoAnywhereCore;
using System;
using System.Collections.Generic;

namespace RhinoAnywhere
{
    public class RhinoAnywhere : Command
    {
        //RhinoAnywhereSingleton _rhinoAnywhereServer;

        public RhinoAnywhere()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
            //_rhinoAnywhereServer = RhinoAnywhereSingleton.Instance;

            // Subscribe to the InputEvent
            //_rhinoAnywhereServer.InputEvent += RhinoAnywhereServer_InputEvent;
        }

        ///<summary>The only instance of this command.</summary>
        public static RhinoAnywhere Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "RhinoAnywhere";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            return Result.Success;
        }

        // Event handler method
        private void RhinoAnywhereServer_InputEvent(object sender, InputEventArgs e)
        {
            // Check if the type is "input" and the action is "move"
            if (e.Type == "input" && e.Data.Action == "move")
            {
                // Construct the summary message
                string summary = $"Move Event: Type = {e.Type}, Position = ({e.Data.X}, {e.Data.Y}), Delta = ({e.Data.DeltaX}, {e.Data.DeltaY})";

                // Print the summary message to the Rhino console
                RhinoApp.WriteLine(summary);
                //ManipulateViewport(e.Data.DeltaX, e.Data.DeltaY);
            }
        }

        private void ManipulateViewport(double viewportDeltaX, double viewportDeltaY)
        {
            // Implement the actual viewport manipulation logic here
            // This might involve panning, zooming, or rotating the view in Rhino
            //var activeViewport = RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport;
            // For example, to pan the view:
            
            //activeViewport.CameraUp *= viewportDeltaY;
            //activeViewport.CameraTarget *= viewportDeltaX;
            // Make sure to redraw the view to reflect the changes
            //RhinoDoc.ActiveDoc.Views.Redraw();
        }
    }
}
