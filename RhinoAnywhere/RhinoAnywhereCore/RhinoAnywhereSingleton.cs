using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoAnywhereCore
{
    public class RhinoAnywhereSingleton
    {
        // Static variable that holds the single instance of the class
        private static RhinoAnywhereSingleton _instance;

        // Object for locking to avoid thread issues
        private static readonly object _lock = new object();

        // Private constructor to prevent external instantiation
        private RhinoAnywhereSingleton()
        {
        }

        // Public static method to get the instance of the class
        public static RhinoAnywhereSingleton Instance
        {
            get
            {
                // Double-check locking to ensure thread safety
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RhinoAnywhereSingleton();
                        }
                    }
                }
                return _instance;
            }
        }
        // Rest of the class implementation

        // Event declaration
        public event EventHandler<InputEventArgs> InputEvent;

        public void RaiseMouseMoveEvent()
        {

        }

        // Method to raise the event
        public void RaiseInputEvent(string method, string action, double x, double y, double deltaX, double deltaY, string value)
        {
            // Create the data object
            InputData data = new InputData
            {
                Method = method,
                Action = action,
                X = x,
                Y = y,
                DeltaX = deltaX,
                DeltaY = deltaY,
                Value = value
            };

            // Create the event args
            InputEventArgs args = new InputEventArgs
            {
                Type = "input",
                Data = data
            };

            // Raise the event
            InputEvent?.Invoke(this, args);
        }
    }
}
