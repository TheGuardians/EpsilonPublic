using CacheEditor.Components.TagTree.Commands;
using EpsilonLib.Menus;
using EpsilonLib.Shell.Commands;


namespace CacheEditor.Components.TagTree
{
    public static class MenuDefinitions
    {
        public static object CopyMenuItemGroup = new object();
        public static object ViewMenuItemGroup = new object();

        [ExportMenuItem]
        public static MenuItemDefinition ContextMenu = new MenuItemDefinition();

        // Copy Group
        [ExportMenuItem]
        public static MenuItemDefinition CopyMenuItem = new CommandMenuItemDefinition<CopyCommand>(ContextMenu, CopyMenuItemGroup);

        // View Group
        [ExportMenuItem]
        public static MenuItemDefinition ViewMenu = new MenuItemDefinition(ContextMenu, ViewMenuItemGroup, "View");
        [ExportMenuItem]
        public static MenuItemDefinition ToggleFoldersViewMenuItem = new CommandMenuItemDefinition<ToggleFoldersViewCommand>(ViewMenu, null);
        [ExportMenuItem]
        public static MenuItemDefinition ToggleGroupsViewMenuItem = new CommandMenuItemDefinition<ToggleGroupsViewCommand>(ViewMenu, null);

        [ExportMenuItem]
        public static MenuItemDefinition GroupViewMenu = new MenuItemDefinition(ContextMenu, ViewMenuItemGroup, "Group View Options");
        [ExportMenuItem]
        public static MenuItemDefinition ToggleGroupNameViewMenuItem = new CommandMenuItemDefinition<ToggleGroupNameViewCommand>(GroupViewMenu, null);
        [ExportMenuItem]
        public static MenuItemDefinition ToggleGroupTagNameMenuItem = new CommandMenuItemDefinition<ToggleGroupTagNameViewCommand>(GroupViewMenu, null);
    }
}
