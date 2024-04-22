using EpsilonLib.Options;
using EpsilonLib.Shell.TreeModels;
using Stylet;
using System.Collections.Generic;
using System.Linq;

namespace Epsilon.Options
{
    class OptionsPageTreeViewModel : TreeModel
    {
        public OptionsPageTreeViewModel(IEnumerable<IOptionsPage> pages)
        {
            var pageModels = pages.Select(page => new OptionsTreeNode(page.DisplayName, page, default));

            Nodes = new BindableCollection<ITreeNode>(pageModels.GroupBy(page => page.Page.Category)
                .Select(group => new OptionsTreeNode(group.Key, group.FirstOrDefault()?.Page, group)));
        }
    }
}
