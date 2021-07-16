using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace images_viewer.Views
{
    /// <summary>
    /// Interaction logic for PhotoViewerView.xaml
    /// </summary>
    public partial class PhotoViewerView : UserControl
    {
        public PhotoViewerView()
        {
            InitializeComponent();
            this.photosListView.Focus();
        }

        private void photosListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Keyboard.Focus(sender as IInputElement);
            this.photosListView.ScrollIntoView(sender);
        }
    }
}
