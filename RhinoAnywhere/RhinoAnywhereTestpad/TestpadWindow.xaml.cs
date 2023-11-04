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
        public TestpadWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            var position = e.GetPosition(this);
            // Output the mouse position to the console or use it as needed
            Trace.WriteLine($"Mouse Move: X = {position.X}, Y = {position.Y}");
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
