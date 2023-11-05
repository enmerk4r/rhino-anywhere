namespace RhinoAnywhere.DataStructures
{
    public struct Packet<T>
    {
        public string type { get; set; }
        public T data { get; set; }
    }
}
