using custom_paint_inccanvas.Commands;
using custom_paint_inccanvas.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace custom_paint_inccanvas.ViewModels
{
    public class CanvasViewModel : BaseViewModel
    {
        private StrokeCollection _strokeCollection;

        public StrokeCollection StrokeCollection
        {
            get { return _strokeCollection; }
            set { _strokeCollection = value; OnPropertyChanged(); }
        }

        private StrokeCollection _strokesReadyToRedo;

        public StrokeCollection StrokesReadyToRedo
        {
            get { return _strokesReadyToRedo; }
            set { _strokesReadyToRedo = value; OnPropertyChanged(); }
        }

        private Stroke CurrentDrawingStroke { get; set; }

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

        private InkCanvasEditingMode _editingMode;

        public InkCanvasEditingMode CanvasEditingMode
        {
            get { return _editingMode; }
            set { _editingMode = value;  OnPropertyChanged(); }
        }

        public ObservableCollection<bool> ButtonStates { get; set; }

        private Point FirstCoordinatePoint { get; set; }
        private bool IsErasing { get; set; }

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

            StrokeCollection = new();

            StrokesReadyToRedo = new();

            CurrentFigure = new Polyline();
            IsErasing = false;
            StrokeThikness = 1;

            SelectedColor = Colors.Black;

            CanvasEditingMode = InkCanvasEditingMode.None;

            ButtonStates = new ObservableCollection<bool>();

            for(int i = 0; i < 5; i++)
            {
                ButtonStates.Add(false);
            }

            ButtonStates[0] = true;
        }

        private void MouseDownCommandHandler(object obj)
        {
            Point initialCoord = (obj as MouseEventArgsWithCoordinates).Coordinates;
            FirstCoordinatePoint = new Point(initialCoord.X, initialCoord.Y);

            // Initialization of the first point for the Stroke.
            CurrentDrawingStroke = new Stroke(new StylusPointCollection() { new StylusPoint { X = FirstCoordinatePoint.X, Y = FirstCoordinatePoint.Y } })
            {
                DrawingAttributes = new DrawingAttributes()
                {
                    Color = SelectedColor,
                    FitToCurve = false,
                    Height = StrokeThikness,
                    IgnorePressure = true,
                    Width = StrokeThikness,
                    StylusTip = StylusTip.Rectangle
                }
            };

            if (IsErasing)
            {
                CurrentDrawingStroke.DrawingAttributes.Color = Colors.White;
            }

            StrokeCollection.Add(CurrentDrawingStroke);

            StrokesReadyToRedo.Clear();
        }

        public void LeftMouseButtonUpCommandHandler(object obj)
        {
            CurrentDrawingStroke = new Stroke(new StylusPointCollection() { new StylusPoint { X = FirstCoordinatePoint.X, Y = FirstCoordinatePoint.Y } });
            FirstCoordinatePoint = new Point(0, 0);
        }

        private void MouseMoveCommandHandler(object obj)
        {
            DrawShape(obj as MouseEventArgsWithCoordinates);
        }

        private void DrawShape(MouseEventArgsWithCoordinates mouseArgs)
        {
            if (mouseArgs.MouseEventArgs.LeftButton != MouseButtonState.Pressed)
                return;

            if (CurrentFigure is Polyline || CurrentFigure is Eraser)
            {
                CurrentDrawingStroke.StylusPoints.Add(new StylusPoint(mouseArgs.Coordinates.X, mouseArgs.Coordinates.Y));
            }
            if (CurrentFigure is Ellipse)
            {
                if (mouseArgs.MouseEventArgs.LeftButton == MouseButtonState.Pressed)
                {
                    Point endPoint = mouseArgs.Coordinates;
                    List<Point> ellipsePoints = GenerateEclipseGeometry(FirstCoordinatePoint, endPoint);
                    StylusPointCollection figurePointCollection = new StylusPointCollection(ellipsePoints);

                    CurrentDrawingStroke.StylusPoints = new StylusPointCollection(figurePointCollection);
                }
            }
            if (CurrentFigure is Rectangle)
            {
                Point endPoint = mouseArgs.Coordinates;
                Point initialPoint = FirstCoordinatePoint;
                List<Point> pointList = new List<Point>
                    {
                        new Point(initialPoint.X, initialPoint.Y),
                        new Point(endPoint.X, initialPoint.Y),
                        new Point(endPoint.X, endPoint.Y),
                        new Point(initialPoint.X, endPoint.Y),
                        new Point(initialPoint.X, initialPoint.Y),
                    };

                CurrentDrawingStroke.StylusPoints = new StylusPointCollection(pointList);
            }
            if (CurrentFigure is Line)
            {
                Point endPoint = mouseArgs.Coordinates;

                List<Point> pointList = new();

                pointList.Add(FirstCoordinatePoint);
                pointList.Add(endPoint);

                CurrentDrawingStroke.StylusPoints = new StylusPointCollection(pointList);
            }

        }

        private List<Point> GenerateEclipseGeometry(Point st, Point ed)
        {
            double a = 0.5 * (ed.X - st.X);
            double b = 0.5 * (ed.Y - st.Y);
            List<Point> pointList = new List<Point>();
            for (double r = 0; r <= 2 * Math.PI; r = r + 0.01)
            {
                pointList.Add(new Point(0.5 * (st.X + ed.X) + a * Math.Cos(r), 0.5 * (st.Y + ed.Y) + b * Math.Sin(r)));
            }
            return pointList;
        }


        private void ChoseFigureToDrawCommandHandler(object obj)
        {
            if ((obj as Type).Name == "Eraser")
            {
                IsErasing = true;
                CanvasEditingMode = InkCanvasEditingMode.EraseByPoint;
            }
            else
            {
                IsErasing = false;
                CanvasEditingMode = InkCanvasEditingMode.None;
            }
            CurrentFigure = Activator.CreateInstance(obj as Type) as Shape;

            for(int i = 0; i < ButtonStates.Count; i++)
            {
                ButtonStates[i] = false;
            }

            switch(CurrentFigure.GetType().Name)
            {
                case nameof(Polyline):
                    ButtonStates[0] = true;
                    break;
                case nameof(Rectangle):
                    ButtonStates[1] = true;
                    break;
                case nameof(Ellipse):
                    ButtonStates[2] = true;
                    break;
                case nameof(Line):
                    ButtonStates[3] = true;
                    break;
                case nameof(Eraser):
                    ButtonStates[4] = true;
                    break;
            }
        }

        private void UndoCommandHandler(object obj)
        {
            if (StrokeCollection.Count > 0)
            {
                StrokesReadyToRedo.Add(StrokeCollection.Last());
                StrokeCollection.Remove(StrokeCollection.Last());
            }
        }

        private void RedoCommandHandler(object obj)
        {
            if (StrokesReadyToRedo.Count > 0)
            {
                StrokeCollection.Add(StrokesReadyToRedo.Last());
                StrokesReadyToRedo.Remove(StrokesReadyToRedo.Last());
            }
        }
    }
}
