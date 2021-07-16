using images_viewer.Domain.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace images_viewer.Views
{
    public class GalleryObjectTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && item is GalleryObject)
            {
                var window = Application.Current.MainWindow;
                if (item is Folder)
                {
                    return window.FindResource("FolderTemplate") as DataTemplate;
                }
                else
                {
                    return window.FindResource("PictureTemplate") as DataTemplate;
                }
            }
            return null;
        }
    }
}
