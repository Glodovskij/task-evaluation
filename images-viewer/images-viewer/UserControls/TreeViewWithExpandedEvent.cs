using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace images_viewer.UserControls
{
    class TreeViewWithExpandedEvent : TreeView
    {
        public ICommand OnExpanded
        {
            get { return (ICommand)GetValue(OnExpandedProperty); }
            set { SetValue(OnExpandedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnExpanded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnExpandedProperty =
            DependencyProperty.Register("OnExpanded", typeof(ICommand), typeof(TreeViewWithExpandedEvent), new PropertyMetadata(null));


    }
}
