using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EpsilonLib.Shell.TreeModels
{
    public class TreeModel : PropertyChangedBase, ITreeViewEventSink
    {
        private ICollection<ITreeNode> _nodes;
        private ITreeNode _selectedNode;

        public event EventHandler<TreeNodeEventArgs> NodeDoubleClicked;
        public event EventHandler<TreeNodeEventArgs> NodeSelected;

        public ICollection<ITreeNode> Nodes
        {
            get => _nodes ?? (_nodes = new ObservableCollection<ITreeNode>());
            set => SetAndNotify(ref _nodes, value);
        }

        public ITreeNode SelectedNode
        {
            get => _selectedNode;
            set
            {
                var oldSelectedNode = _selectedNode;
                if (SetAndNotify(ref _selectedNode, value))
                {
                    if(oldSelectedNode != null)
                        oldSelectedNode.IsSelected = false;

                    if(_selectedNode != null)
                        _selectedNode.IsSelected = true;
                }  
            }
        }

        public IEnumerable<ITreeNode> FindNodesWithTag(object tag)
        {
            IEnumerable<ITreeNode> FindNodesWithTag(IEnumerable<ITreeNode> roots)
            {
                foreach(var node in roots)
                {
                    if (node.Tag == tag)
                        yield return node;

                    if (node.Children != null)
                    {
                        foreach (var child in FindNodesWithTag(node.Children))
                            yield return child;
                    }
                }
            }

            return FindNodesWithTag(_nodes);
        }


        #region ITreeEventSink Members
        object ITreeViewEventSink.Source { get; set; }
        void ITreeViewEventSink.NodeDoubleClicked(TreeNodeEventArgs e)
        {
            //OnNodeDoubleClicked(e);
        }
        void ITreeViewEventSink.NodeSelected(TreeNodeEventArgs e) => OnNodeSelected(e);
        #endregion

        protected virtual void OnNodeDoubleClicked(TreeNodeEventArgs e)  => NodeDoubleClicked?.Invoke(this, e);
        protected virtual void OnNodeSelected(TreeNodeEventArgs e)
        {
            SelectedNode = e.Node;
            NodeSelected?.Invoke(this, e);

            if (Mouse.RightButton != MouseButtonState.Pressed)
                NodeDoubleClicked?.Invoke(this, e);
        }

        public void SimulateDoubleClick(TreeNodeEventArgs e)
        {
            NodeDoubleClicked?.Invoke(this, e);
        }
    }
}
