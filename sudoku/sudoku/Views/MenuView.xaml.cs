using System.Windows;
using System.Windows.Controls;

namespace sudoku.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public bool IsGameCanBeContinued
        {
            get { return (bool)GetValue(IsGameCanBeContinuedProperty); }
            set { SetValue(IsGameCanBeContinuedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsGameCanBeContinued.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsGameCanBeContinuedProperty =
            DependencyProperty.Register("IsGameCanBeContinued", typeof(bool), typeof(MenuView), new PropertyMetadata(false));


        public MenuView()
        {
            InitializeComponent();
        }
    }
}
