using EpsilonLib.Options;
using EpsilonLib.Shell.TreeModels;
using Stylet;
using System;
using System.Linq;
using System.Windows.Media;
using System.Windows;
using Xceed.Wpf.AvalonDock.Themes;
using EpsilonLib.Settings;

namespace Epsilon.Options
{
    class OptionsViewModel : Conductor<IOptionsPage>.Collection.OneActive
    {
        public OptionsPageTreeViewModel CategoryTree { get; }

        private IOptionsService _optionsService;

        public OptionsViewModel(IOptionsService optionsService)
        {
            _optionsService = optionsService;
            DisplayName = "Options";
            CategoryTree = new OptionsPageTreeViewModel(optionsService.OptionPages);
            CategoryTree.NodeSelected += CategoryTree_NodeSelected;

            foreach (var node in CategoryTree.Nodes.Where(x => x.Children != null))
                node.IsExpanded = true;

            CategoryTree.SelectedNode = CategoryTree.Nodes.FirstOrDefault();
        }

        private void CategoryTree_NodeSelected(object sender, TreeNodeEventArgs e)
        {
            if(e.Node is OptionsTreeNode node)
            {
                ActiveItem = node.Page;
                if(node.Children.Count > 0)
                    node.IsExpanded = true;
            }
        }

        public void Apply()
        {
            foreach(var page in _optionsService.OptionPages)
            {
                if(page.IsDirty)
                    page.Apply();

                page.IsDirty = false;
            }

            RequestClose(true);
        }

        public void Cancel()
        {
            GeneralOptionsViewModel general = (GeneralOptionsViewModel)_optionsService.OptionPages.First();
            general.RevertAppearance();

            RequestClose(false);
        }
    }
}
