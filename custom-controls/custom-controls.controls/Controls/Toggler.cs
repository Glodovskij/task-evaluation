using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace custom_controls.controls
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

            Thumb thumb = GetTemplateChild("PART_Toggler") as Thumb;

            thumb.DragDelta += Thumb_DragDelta;
            thumb.DragCompleted += Thumb_DragCompleted;

            grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var thumb = sender as Thumb;
            var offset = VisualTreeHelper.GetOffset(thumb);

            if(offset.X != 20 && offset.X != 0)
            {
                if(ToggleState)
                {
                    Canvas.SetLeft(thumb, 20);
                }
                else
                {
                    Canvas.SetLeft(thumb, 0);
                }
            }
           
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            var offset = VisualTreeHelper.GetOffset(thumb);

            var left = offset.X + e.HorizontalChange;
            if (left <= 20 && left >= 0)
            {
                Canvas.SetLeft(thumb, left);
            }
            if (left == 0)
            {
                ToggleState = false;
                VisualStateManager.GoToState(this, "Zero", false);
            }
            else if (left == 20)
            {
                ToggleState = true;
                VisualStateManager.GoToState(this, "One", false);
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToggleState = !ToggleState;
            if (ToggleState)
            {
                VisualStateManager.GoToState(this, "One", false);
                Thumb thumb = GetTemplateChild("PART_Toggler") as Thumb;
                Canvas.SetLeft(thumb, 20);
            }
            else
            {
                VisualStateManager.GoToState(this, "Zero", false);
                Thumb thumb = GetTemplateChild("PART_Toggler") as Thumb;
                Canvas.SetLeft(thumb, 0);
            }
        }
    }
}
