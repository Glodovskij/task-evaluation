using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace custom_controls.controls
{
    public class Toggler : Control
    {
        private double FalseXAxisCanvasCoordinate;
        private double TrueXAxisCanvasCoordinate;

        public bool ToggleState
        {
            get { return (bool)GetValue(ToggleStateProperty); }
            set { SetValue(ToggleStateProperty, value); }
        }

        public string OnState
        {
            get { return (string)GetValue(OnStateProperty); }
            set { SetValue(OnStateProperty, value); }
        }

        public static readonly DependencyProperty OnStateProperty =
            DependencyProperty.Register("OnState", typeof(string), typeof(Toggler), new PropertyMetadata(null));


        public string OffState
        {
            get { return (string)GetValue(OffStateProperty); }
            set { SetValue(OffStateProperty, value); }
        }

        public static readonly DependencyProperty OffStateProperty =
            DependencyProperty.Register("OffState", typeof(string), typeof(Toggler), new PropertyMetadata(null));


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

            Canvas canvas = GetTemplateChild("PART_TogglerCanvas") as Canvas;

            canvas.SizeChanged += Canvas_SizeChanged;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            Thumb thumb = GetTemplateChild("PART_Toggler") as Thumb;

            FalseXAxisCanvasCoordinate = 0;
            TrueXAxisCanvasCoordinate = canvas.ActualWidth - thumb.ActualWidth;

        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var thumb = sender as Thumb;
            var offset = VisualTreeHelper.GetOffset(thumb);

            if(offset.X != TrueXAxisCanvasCoordinate && offset.X != FalseXAxisCanvasCoordinate)
            {
                if(ToggleState)
                {
                    Canvas.SetLeft(thumb, TrueXAxisCanvasCoordinate);
                }
                else
                {
                    Canvas.SetLeft(thumb, FalseXAxisCanvasCoordinate);
                }
            }
           
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            var offset = VisualTreeHelper.GetOffset(thumb);

            var left = offset.X + e.HorizontalChange;
            if (left <= TrueXAxisCanvasCoordinate && left >= FalseXAxisCanvasCoordinate)
            {
                Canvas.SetLeft(thumb, left);
            }
            if (left == FalseXAxisCanvasCoordinate)
            {
                ToggleState = false;
                VisualStateManager.GoToState(this, "Zero", false);

            }
            else if (left == TrueXAxisCanvasCoordinate)
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
                Canvas.SetLeft(thumb, TrueXAxisCanvasCoordinate);
            }
            else
            {
                VisualStateManager.GoToState(this, "Zero", false);
                Thumb thumb = GetTemplateChild("PART_Toggler") as Thumb;
                Canvas.SetLeft(thumb, FalseXAxisCanvasCoordinate);
            }
        }
    }
}
