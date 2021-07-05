using image_gallery.Domain.ViewModels;
using System;
using System.Windows;

namespace image_gallery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            int width = (int)((System.Windows.Controls.Panel)Application.Current.MainWindow.Content).ActualWidth;

            (DataContext as PicturesSourceViewModel).ColumnAmount = width / 200;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
