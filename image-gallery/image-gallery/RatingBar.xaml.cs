using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace image_gallery
{
    /// <summary>
    /// Interaction logic for RatingBar.xaml
    /// </summary>
    public partial class RatingBar : StackPanel
    {
        public RatingBar()
        {
            InitializeComponent();
        }

        //public static readonly DependencyProperty CommandProperty =
        //DependencyProperty.Register(
        //"Command",
        //typeof(ICommand),
        //typeof(RatingBar));

        //public ICommand Command
        //{
        //    get
        //    {
        //        return (ICommand)GetValue(CommandProperty);
        //    }

        //    set
        //    {
        //        SetValue(CommandProperty, value);
        //    }
        //}


        public static readonly DependencyProperty RatingValueProperty = DependencyProperty.Register(
            "RatingValue",
            typeof(int),
            typeof(RatingBar),
            new PropertyMetadata(0, new PropertyChangedCallback(RatingValueChanged)));

        public int RatingValue
        {
            get
            {

                return (int)GetValue(RatingValueProperty);
            }
            set
            {
                if (value < 0)
                {
                    SetValue(RatingValueProperty, 0);
                }
                else if (value > 5)
                {
                    SetValue(RatingValueProperty, 5);
                }
                else
                {
                    SetValue(RatingValueProperty, value);
                }
            }
        }

        private static void RatingValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RatingBar parent = sender as RatingBar;
            int ratingValue = (int)e.NewValue;
            UIElementCollection children = parent.Children;

            ToggleButton button = null;
            for (int i = 0; i < ratingValue; i++)
            {
                button = children[i] as ToggleButton;
                button.IsChecked = true;
            }

            for (int i = ratingValue; i < children.Count; i++)
            {
                button = children[i] as ToggleButton;
                button.IsChecked = false;
            }
        }

        private void RatingButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            ToggleButton button = sender as ToggleButton;
            RatingValue = int.Parse((string)button.Tag);
        }
    }
}
