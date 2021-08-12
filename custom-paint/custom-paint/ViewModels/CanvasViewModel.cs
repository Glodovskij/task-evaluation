using custom_paint.Commands;
using custom_paint.Utility;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace custom_paint.ViewModels
{
    public class CanvasViewModel : BaseViewModel
    {
        public ObservableCollection<Shape> Shapes { get; set; }

        public ObservableCollection<Shape> ShapessReadyToRedo { get; set; }

        private Shape _currentFigure;

        public Shape CurrentFigure
        {
            get { return _currentFigure; }
            set { _currentFigure = value; OnPropertyChanged(); }
        }

        private Color _selectedColor;

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; OnPropertyChanged(); }
        }

        private int _strokeThikness;

        public int StrokeThikness
        {
            get { return _strokeThikness; }
            set { _strokeThikness = value; OnPropertyChanged(); }
        }

        private Point FirstCoordinatePoint { get; set; }
        public bool IsErasing { get; private set; }

        public ICommand MouseMoveCommand { get; }
        public ICommand MouseDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }
        public ICommand ChoseFigureToDrawCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public CanvasViewModel()
        {
            MouseMoveCommand = new RelayCommand(MouseMoveCommandHandler);
            MouseLeftButtonUpCommand = new RelayCommand(LeftMouseButtonUpCommandHandler);
            MouseDownCommand = new RelayCommand(MouseDownCommandHandler);
            ChoseFigureToDrawCommand = new RelayCommand(ChoseFigureToDrawCommandHandler);
            UndoCommand = new RelayCommand(UndoCommandHandler);
            RedoCommand = new RelayCommand(RedoCommandHandler);

            ShapessReadyToRedo = new();
            Shapes = new();
            CurrentFigure = new Polyline();
            IsErasing = false;
            StrokeThikness = 1;

            SelectedColor = Colors.Black;
        }

        private void InitializeSelectedShape(MouseEventArgsWithCoordinates args = null)
        {
            if(IsErasing == true)
            {
                CurrentFigure = new Polyline();

                SolidColorBrush whiteBrush = new SolidColorBrush();
                whiteBrush.Color = Colors.White;

                CurrentFigure.Stroke = whiteBrush;
                CurrentFigure.StrokeThickness = StrokeThikness;

                (CurrentFigure as Polyline).Points  = new PointCollection();
                return;
            }
            if(CurrentFigure is Polyline)
            {
                CurrentFigure = new Polyline();

                SolidColorBrush blackBrush = new SolidColorBrush();
                blackBrush.Color = SelectedColor;

                CurrentFigure.Stroke = blackBrush;
                CurrentFigure.StrokeThickness = StrokeThikness;

                (CurrentFigure as Polyline).Points = new PointCollection();
            }
            if(CurrentFigure is Ellipse)
            {
                CurrentFigure = new Ellipse();
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                mySolidColorBrush.Color = SelectedColor;
                CurrentFigure.Fill = mySolidColorBrush;
                CurrentFigure.StrokeThickness = StrokeThikness;
                CurrentFigure.Stroke = Brushes.Black;


                CurrentFigure.Margin = new Thickness(args.Coordinates.X, args.Coordinates.Y, 0, 0);
                FirstCoordinatePoint = new Point(args.Coordinates.X, args.Coordinates.Y);
            }
            if(CurrentFigure is Rectangle)
            {
                CurrentFigure = new Rectangle();
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                mySolidColorBrush.Color = SelectedColor;
                CurrentFigure.Fill = mySolidColorBrush;
                CurrentFigure.StrokeThickness = StrokeThikness;
                CurrentFigure.Stroke = Brushes.Black;


                CurrentFigure.Margin = new Thickness(args.Coordinates.X, args.Coordinates.Y, 0, 0);
                FirstCoordinatePoint = new Point(args.Coordinates.X, args.Coordinates.Y);
            }
            if(CurrentFigure is Line)
            {
                CurrentFigure = new Line();
                CurrentFigure.StrokeThickness = StrokeThikness;
                CurrentFigure.Stroke = Brushes.Black;


                (CurrentFigure as Line).X1 = args.Coordinates.X;
                (CurrentFigure as Line).Y1 = args.Coordinates.Y;
            }
        }

        private void MouseDownCommandHandler(object obj)
        {
            InitializeSelectedShape(obj as MouseEventArgsWithCoordinates);
            Shapes.Add(CurrentFigure);
            ShapessReadyToRedo.Clear();
        }

        public void LeftMouseButtonUpCommandHandler(object obj)
        {
            CurrentFigure = Activator.CreateInstance(CurrentFigure.GetType()) as Shape;
        }

        private void MouseMoveCommandHandler(object obj)
        {
            DrawShape(obj as MouseEventArgsWithCoordinates);
        }

        private void DrawShape(MouseEventArgsWithCoordinates mouseArgs)
        {
            if (CurrentFigure is Polyline)
            {
                if (mouseArgs.MouseEventArgs.LeftButton == MouseButtonState.Pressed)
                {
                    if(mouseArgs.Coordinates.Y >= 0)
                        (CurrentFigure as Polyline).Points.Add(mouseArgs.Coordinates);
                }
            }
            if (CurrentFigure is Ellipse || CurrentFigure is Rectangle)
            {
                if (mouseArgs.MouseEventArgs.LeftButton == MouseButtonState.Pressed)
                {

                    if (mouseArgs.Coordinates.X > FirstCoordinatePoint.X)
                    {
                        CurrentFigure.RenderTransform = new ScaleTransform() { ScaleX = 1 };
                        CurrentFigure.Width = Math.Abs(FirstCoordinatePoint.X - mouseArgs.Coordinates.X);
                    }
                    if (mouseArgs.Coordinates.Y > FirstCoordinatePoint.Y)
                    {
                        CurrentFigure.RenderTransform = new ScaleTransform() { ScaleY = 1 };
                        CurrentFigure.Height = Math.Abs(FirstCoordinatePoint.Y - mouseArgs.Coordinates.Y);
                    }
                    if(mouseArgs.Coordinates.Y < FirstCoordinatePoint.Y)
                    {
                        CurrentFigure.RenderTransform = new ScaleTransform() { ScaleY = -1 };
                        CurrentFigure.Height = Math.Abs(FirstCoordinatePoint.Y - mouseArgs.Coordinates.Y);
                    }
                    if (mouseArgs.Coordinates.X < FirstCoordinatePoint.X)
                    {
                        CurrentFigure.RenderTransform = new ScaleTransform() { ScaleX = -1 };
                        CurrentFigure.Width = Math.Abs(FirstCoordinatePoint.X - mouseArgs.Coordinates.X);
                    }

                    if(mouseArgs.Coordinates.Y < FirstCoordinatePoint.Y && mouseArgs.Coordinates.X < FirstCoordinatePoint.X)
                    {
                        CurrentFigure.RenderTransform = new ScaleTransform() { ScaleX = -1, ScaleY = -1 };
                        CurrentFigure.Height = Math.Abs(FirstCoordinatePoint.Y - mouseArgs.Coordinates.Y);
                        CurrentFigure.Width = Math.Abs(FirstCoordinatePoint.X - mouseArgs.Coordinates.X);
                    }
                }
            }
            if(CurrentFigure is Line)
            {
                if (mouseArgs.MouseEventArgs.LeftButton == MouseButtonState.Pressed)
                {
                    (CurrentFigure as Line).X2 = mouseArgs.Coordinates.X;
                    (CurrentFigure as Line).Y2 = mouseArgs.Coordinates.Y;
                }
            }
        }

        private void ChoseFigureToDrawCommandHandler(object obj)
        {
            if((obj as Type).Name == "Eraser")
            {
                IsErasing = true;
            }
            else
            {
                IsErasing = false;
            }
            CurrentFigure = Activator.CreateInstance(obj as Type) as Shape;
        }

        private void UndoCommandHandler(object obj)
        {
            if (Shapes.Count > 0)
            {
                ShapessReadyToRedo.Add(Shapes.Last());
                Shapes.Remove(Shapes.Last());
            }
        }

        private void RedoCommandHandler(object obj)
        {
            if (ShapessReadyToRedo.Count > 0)
            {
                Shapes.Add(ShapessReadyToRedo.Last());
                ShapessReadyToRedo.Remove(ShapessReadyToRedo.Last());
            }
        }
    }
}
