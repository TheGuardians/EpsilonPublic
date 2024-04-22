using EpsilonLib.Themes;
using Stylet;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EpsilonLib.Shell.TreeModels
{
    public class StandardTreeNode : PropertyChangedBase, ITreeNode
    {
        private object _tag;
        private bool _isSelected;
        private bool _isExpanded;
        private IList<ITreeNode> _children;
        private string _text;
        private string _altText;
        private ColorHint _textColor;

        public object Tag
        {
            get => _tag;
            set => SetAndNotify(ref _tag, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetAndNotify(ref _isSelected, value);
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetAndNotify(ref _isExpanded, value);
        }

        public IList<ITreeNode> Children
        {
            get => _children ?? (_children = new ObservableCollection<ITreeNode>());
            set => SetAndNotify(ref _children, value);
        }

        public virtual string Text
        {
            get => _text;
            set => SetAndNotify(ref _text, value);
        }

        public virtual string AltText
        {
            get => _altText;
            set => SetAndNotify(ref _altText, value);
        }

        public virtual ColorHint TextColor
        {
            get => _textColor;
            set => SetAndNotify(ref _textColor, value);
        }

        IEnumerable<ITreeNode> ITreeNode.Children => _children;
    }
}
