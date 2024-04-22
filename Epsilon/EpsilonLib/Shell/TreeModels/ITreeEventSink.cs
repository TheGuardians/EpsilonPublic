using System;

namespace EpsilonLib.Shell.TreeModels
{
    public interface ITreeViewEventSink
    {
        object Source { get; set; }

        void NodeDoubleClicked(TreeNodeEventArgs e);
        void NodeSelected(TreeNodeEventArgs e);
    }

    public class TreeNodeEventArgs : EventArgs
    { 
        public ITreeNode Node { get; }

        public TreeNodeEventArgs(ITreeNode node)
        {
            Node = node;
        }
    }
}
