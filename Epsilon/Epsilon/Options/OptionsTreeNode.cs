using EpsilonLib.Options;
using EpsilonLib.Shell.TreeModels;
using Stylet;
using System.Collections.Generic;

namespace Epsilon.Options
{
    class OptionsTreeNode : StandardTreeNode
    {
        public IOptionsPage Page { get; set; }

        public OptionsTreeNode(string displayName, IOptionsPage page, IEnumerable<OptionsTreeNode> children)
        {
            Text = displayName;
            Page = page;
            if(children != null)
                Children = new BindableCollection<ITreeNode>(children);
        }
    }
}
