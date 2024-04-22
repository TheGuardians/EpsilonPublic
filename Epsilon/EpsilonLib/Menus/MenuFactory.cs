using EpsilonLib.Commands;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace EpsilonLib.Menus
{
    [Export(typeof(IMenuFactory))]
    public class MenuFactory : IMenuFactory
    {
        private readonly ICommandRegistry _commandRegistry;
        private readonly MenuItemDefinition[] _definitions;
        private Dictionary<object, MenuItemViewModel> _menuViewModels = new Dictionary<object, MenuItemViewModel>();

        [ImportingConstructor]
        public MenuFactory(
            [Import] ICommandRegistry commandRegistry,
            [ImportMany] MenuItemDefinition[] definitions)
        {
            _commandRegistry = commandRegistry;
            _definitions = definitions;
        }

        public MenuItemViewModel GetMenu(MenuItemDefinition definition)
        {
            if (_menuViewModels.TryGetValue(definition, out MenuItemViewModel menuViewModel))
                return menuViewModel;

            menuViewModel = new MenuItemViewModel() { Text = definition.Text };
            _menuViewModels.Add(definition, menuViewModel);

            if(definition.Text != null && definition.Text == "File")
            {

            }

            var children = _definitions
                .Where(x => x.Parent == definition)
                .InPreferedOrder();

            var groups = children.GroupBy(x => x.Group).ToList();

            for(int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];

                foreach (var child in group)
                {
                    var childViewModel = CreateChild(child);
                    menuViewModel.AddChild(childViewModel, group.Key);
                }

                if (i < groups.Count - 1)
                    menuViewModel.AddChild(MenuItemViewModel.Separator, group.Key);
            }

            return menuViewModel;
        }

        private MenuItemViewModel CreateChild(MenuItemDefinition child)
        {
            switch (child)
            {
               
                case CommandMenuItemDefinition item:
                    {
                        var command = _commandRegistry.GetCommand(item.CommandType);
                        return new CommandMenuItemViewModel(command);
                    }
                default:
                    return GetMenu(child);
            }
        }

        public IEnumerable<MenuItemViewModel> GetMenuBar(MenuBarDefinition definition)
        {
            var children = _definitions
                .Where(x => x.Parent == definition)
                .InPreferedOrder();

            foreach (var child in children)
                yield return GetMenu(child);
        }
    }

    static class Helpers
    {
        public static IEnumerable<MenuItemDefinition> InPreferedOrder(this IEnumerable<MenuItemDefinition> input)
        {
            var sorted = input.ToList<MenuItemDefinition>();
            for (int i = 0; i < sorted.Count; i++)
            {
                if (sorted[i].PlaceAfter != null)
                {
                    var source = sorted[i];
                    var target = source.PlaceAfter();
                    sorted.RemoveAt(i);
                    sorted.Insert(sorted.IndexOf(target) + 1, source);
                }
            }
            return sorted;
        }
    }
}
