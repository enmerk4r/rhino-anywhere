namespace RhinoAnywhere
{


    public sealed partial class StartRhinoAnywhere
    {
        public struct MouseData
        {
            public string method { get; set; }
            public string action { get; set; }
            public double x { get; set; }
            public double y { get; set; }
            public double deltax { get; set; }
            public double deltay { get; set; }
            public string value { get; set; }
        }
    }
}
