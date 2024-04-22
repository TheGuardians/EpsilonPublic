using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EpsilonLib.Menus
{
    // Simple class that acts as a builder for a menu with automatic separator insertion.
    // Simply add submenus and groups to the menu, and then when finished call BuildMenu()
    // to get a MenuItem that can be inserted into a Menu or ContextMenu
    public class EMenu
    {
        private readonly string GroupKey = "";
        private readonly string Text = "";
        private readonly ICommand Command;
        private readonly string Shortcut = null;
        private readonly string ToolTip = null;
        private readonly string IconPath = null;
        private readonly List<EMenu> Children = new List<EMenu>();
        private bool IsDisabled = false;

        public EMenu()
        {

        }

        private EMenu(string text)
        {
            Text = text;
        }

        public EMenu(string groupKey, string text, ICommand command, string tooltip, string shortcut, bool disabled)
        {
            GroupKey = groupKey;
            Text = text;
            Command = command;
            ToolTip = tooltip;
            Shortcut = shortcut;
            IsDisabled = disabled;   
        }

        public bool HasGroup(string groupKey)
        {
            return Children.Any(x => x.GroupKey == groupKey);
        }

        public bool HasSubmenu(string text)
        {
            return Children.Any(x => x.Text == text);
        }

        public EMenu Submenu(string text)
        {
            var menu = Children.FirstOrDefault(x => x.Text == text);
            if(menu == null)
            {
                Children.Add(menu = new EMenu(text));
            }
            return menu;
        }

        public MenuGroup Group(string name)
        {
            return new MenuGroup(name, this);
        }

        public void Add(EMenu item)
        {
            if (Children.Any(x => x.Text == item.Text))
                throw new ArgumentException($"Item \"{item.Text}\" has already added!");

            Children.Add(item.Clone());
        }

        public EMenu Add(string text, ICommand command = null, string tooltip = null, string shortcut = null, bool disabled = false)
        {
            Children.Add(new EMenu("", text, command, tooltip, shortcut, disabled));
            return this;
        }


        public EMenu Clone()
        {
            var newMenu = new EMenu(GroupKey, Text, Command, ToolTip, Shortcut, IsDisabled);
            foreach (var child in Children)
                newMenu.Children.Add(child.Clone());
            return newMenu;
        }


        public MenuItem BuildMenu()
        {
            var built = new MenuItem();
            built.Header = Text;
            built.Command = Command;
            built.ToolTip = string.IsNullOrEmpty(ToolTip) ? null : ToolTip;
            built.InputGestureText = Shortcut;
            built.IsEnabled = !IsDisabled;

            if (!string.IsNullOrEmpty(IconPath))
                built.Icon = new Image { Source = new BitmapImage(new Uri(IconPath, UriKind.Relative)) };

            var builtSubitems = new List<object>();

            IGrouping<string, EMenu> prevGroup = null;
            foreach (var group in Children.GroupBy(x => x.GroupKey))
            {
                if (prevGroup != null && prevGroup.Count() != 0)
                    builtSubitems.Add(new Separator());

                prevGroup = group;

                foreach (EMenu item in group)
                    builtSubitems.Add(item.BuildMenu());
            }

            foreach (object subitem in builtSubitems)
                built.Items.Add(subitem);

            return built;
        }

        public void PopulateTopLevelMenu(EMenu input, ItemCollection output)
        {
            var built = input.BuildMenu();

            // because of the way ItemCollection works we have to detach the items first :/
            var items = built.Items.OfType<object>().ToList();
            built.Items.Clear();

            items.ForEach((x) => output.Add(x));
        }

        public class MenuGroup
        {
            private EMenu Menu;
            private string GroupKey;

            public MenuGroup(string group, EMenu menu)
            {
                GroupKey = group;
                Menu = menu;
            }

            public MenuGroup Add(string text, ICommand command = null, string tooltip = null, string shortcut = null, bool disabled = false)
            {
                Menu.Add(new EMenu(GroupKey, text, command, tooltip, shortcut, disabled));
                return this;
            }

            public MenuGroup Add(EMenu menu)
            {
                Menu.Add(menu.Clone());
                return this;
            }

            public EMenu Submenu(string text)
            {
                return Menu.Submenu(text);
            }
        }
    }
}

