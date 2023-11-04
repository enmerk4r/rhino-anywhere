using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoAnywhereCore
{
    public class InputEventArgs : EventArgs
    {
        public string Type { get; set; }
        public InputData Data { get; set; }
    }

    public class InputData
    {
        public string Method { get; set; }
        public string Action { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int DeltaX { get; set; }
        public int DeltaY { get; set; }
        public string Value { get; set; }
    }
}
