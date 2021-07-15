using images_viewer.Domain.Behaviours.CommandParameters;
using images_viewer.Domain.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace images_viewer.Domain.Behaviours
{
    public static class TreeViewBehaviours
    {
        #region Expanding
        public static readonly DependencyProperty ExpandingBehaviourProperty =
       DependencyProperty.RegisterAttached("ExpandingBehaviour", typeof(ICommand), typeof(TreeViewBehaviours),
           new PropertyMetadata(OnExpandingBehaviourChanged));

        public static void SetExpandingBehaviour(DependencyObject o, ICommand value)
        {
            o.SetValue(ExpandingBehaviourProperty, value);
        }
        public static ICommand GetExpandingBehaviour(DependencyObject o)
        {
            return (ICommand)o.GetValue(ExpandingBehaviourProperty);
        }

        private static void OnExpandingBehaviourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem node = d as TreeViewItem;

            if (node != null)
            {
                ICommand command = e.NewValue as ICommand;
                if (command != null)
                {
                    node.Expanded += (s, a) =>
                    {
                        if (node.Tag != null)
                        {
                            command.Execute(new TreeViewItemCommandParameters
                            {
                                Header = (node.Header as TreeViewNode).Header,
                                Tag = node.Tag.ToString()
                            });
                        }
                        a.Handled = true;
                    };
                }
            }
        }
        #endregion

        #region SelectedItem
        public static readonly DependencyProperty SelectedItemProperty =
                       DependencyProperty.RegisterAttached("SelectedItem",
                                 typeof(object),
                                 typeof(TreeViewBehaviours),
                                 new UIPropertyMetadata(null, SelectedItemChanged));

        public static object GetSelectedItem(DependencyObject obj)
        {
            return (object)obj.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }



        private static void SelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is TreeView) || e.NewValue == null)
                return;

            var view = obj as TreeView;
            view.SelectedItemChanged += (sender, e2) => SetSelectedItem(view, e2.NewValue);
        }

        #endregion
    }
}

