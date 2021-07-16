using System;
using System.Collections.ObjectModel;
using System.IO;

namespace images_viewer.Domain.ViewModels
{
    public class TreeViewNode : Base
    {
        public string Header { get; set; }
        public string Path { get; set; }
        public ObservableCollection<TreeViewNode> Nodes { get; set; }

        public static event Action<string> Expanded;

        private bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { 
                _isExpanded = value;
                ExpandTreeView();
                OnPropertyChanged(); }
        }

        private void ExpandTreeView()
        {
            try
            {
                //LoadImagesFromFolder(treeViewItem.Tag.ToString());
                if(Path == null)
                {
                    return;
                }
                Expanded?.Invoke(Path);
                Nodes.Clear();
                foreach (var folder in Directory.GetDirectories(Path))
                {
                    TreeViewNode folderNode = new TreeViewNode()
                    {
                        Header = folder.Substring(folder.LastIndexOf("\\") + 1),
                        Path = folder
                    };

                    try
                    {
                        if (Directory.GetDirectories(folder).Length > 0)
                        {
                            folderNode.Nodes.Add(null);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        //Часть папок невозможно открыть из-за безопасности
                    }

                    Nodes.Add(folderNode);
                }
            }
            catch (UnauthorizedAccessException)
            {
                //Часть папок невозможно открыть из-за безопасности
            }
        }

        public TreeViewNode()
        {
            Nodes = new();
        }

        public static TreeViewNode GetNode(TreeViewNode root, string path)
        {
            if (root.Nodes != null)
                foreach (var child in root.Nodes)
                {
                    if (child.Path.Equals(path))
                        return child;

                    var node = GetNode(child, path);

                    if (node != null)
                        return node;
                }
            return null;
        }
    }
}
