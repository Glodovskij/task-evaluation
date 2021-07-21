using System;
using System.Collections.Generic;
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

namespace custom_controls.Controls
{
    public class Toggler : Control
    {
        static Toggler()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Toggler), new FrameworkPropertyMetadata(typeof(Toggler)));
        }

        public bool ToggleState
        {
            get { return (bool)GetValue(ToggleStateProperty); }
            set { SetValue(ToggleStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleStateProperty =
            DependencyProperty.Register("ToggleState", typeof(bool), typeof(Toggler), new PropertyMetadata(false));



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Grid grid = GetTemplateChild("PART_ToggleButtonView") as Grid;

            grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToggleState = !ToggleState;
            if(ToggleState)
            {
                VisualStateManager.GoToState(this, "One", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Zero", false);
            }
        }
    }
}
