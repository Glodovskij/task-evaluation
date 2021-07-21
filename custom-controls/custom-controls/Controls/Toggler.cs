using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace custom_controls.Controls
{
    public class Toggler : Control
    {
        public bool ToggleState
        {
            get { return (bool)GetValue(ToggleStateProperty); }
            set { SetValue(ToggleStateProperty, value); }
        }

        public static readonly DependencyProperty ToggleStateProperty =
            DependencyProperty.Register("ToggleState", typeof(bool), typeof(Toggler), new PropertyMetadata(false));

        static Toggler()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Toggler), new FrameworkPropertyMetadata(typeof(Toggler)));
        }

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
