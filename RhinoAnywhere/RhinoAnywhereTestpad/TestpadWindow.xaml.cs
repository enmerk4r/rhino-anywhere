//using RhinoAnywhereCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RhinoAnywhereTestpad
{
    /// <summary>
    /// Interaction logic for TestpadWindow.xaml
    /// </summary>
    public partial class TestpadWindow : Window
    {
        //RhinoAnywhereSingleton _rhinoAnywhereSingleton;

        public TestpadWindow()
        {
            InitializeComponent();
            //_rhinoAnywhereSingleton = RhinoAnywhereSingleton.Instance;
        }

        private Point _previousPosition; // Field to store the previous mouse position

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            var currentPosition = e.GetPosition(this);

            // Calculate the delta
            var deltaX = currentPosition.X - _previousPosition.X;
            var deltaY = currentPosition.Y - _previousPosition.Y;

            // Update the previous position for the next move event
            _previousPosition = currentPosition;

            // Output the mouse position to the console or use it as needed
            Trace.WriteLine($"Mouse Move: X = {currentPosition.X}, Y = {currentPosition.Y}, DeltaX = {deltaX}, DeltaY = {deltaY}");

            // Raise the InputEvent with the mouse move data
            //_rhinoAnywhereSingleton.RaiseInputEvent(
            //    method: "mouse",
            //    action: "move",
            //    x: currentPosition.X,
            //    y: currentPosition.Y,
            //    deltaX: deltaX,
            //    deltaY: deltaY,
            //    value: "" // value is not applicable for a mouse move
            //);
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Output the mouse down event to the console or use it as needed
            Trace.WriteLine("Mouse Button Down");
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Output the mouse up event to the console or use it as needed
            Trace.WriteLine("Mouse Button Up");
        }
    }
}
