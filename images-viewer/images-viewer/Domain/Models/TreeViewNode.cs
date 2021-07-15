using System.Collections.Generic;

namespace images_viewer.Domain.Models
{
    public class TreeViewNode
    {
        public string Header { get; set; }
        public string Path { get; set; }
        public List<TreeViewNode> Nodes { get; set; }

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
