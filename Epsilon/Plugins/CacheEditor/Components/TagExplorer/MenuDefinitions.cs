using CacheEditor.Components.TagExplorer.Commands;
using CacheEditor.Components.TagTree.Commands;
using EpsilonLib.Menus;
using EpsilonLib.Shell.Commands;

namespace CacheEditor.Components.TagExplorer
{
    public static class MenuDefinitions
    {
        public static object CopyMenuItemGroup = new object();
        public static object ViewMenuItemGroup = new object();
        public static object EditMenuItemGroup = new object();
        public static object ExtractImportMenuItemGroup = new object();

        [ExportMenuItem]
        public static MenuItemDefinition ContextMenu = new MenuItemDefinition();

        // Copy Group
        [ExportMenuItem]
        public static MenuItemDefinition CopyMenuItem = new CommandMenuItemDefinition<CopyCommand>(ContextMenu, CopyMenuItemGroup);
        [ExportMenuItem]
        public static MenuItemDefinition CopyTagNameItem = new CommandMenuItemDefinition<CopyTagNameCommand>(ContextMenu, CopyMenuItemGroup);
        [ExportMenuItem]
        public static MenuItemDefinition CopyTagIndexItem = new CommandMenuItemDefinition<CopyTagIndexCommand>(ContextMenu, CopyMenuItemGroup);

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

        // Edit Group
        [ExportMenuItem]
        public static MenuItemDefinition RenameTagMenuItem = new CommandMenuItemDefinition<RenameTagCommand>(ContextMenu, EditMenuItemGroup);
        [ExportMenuItem]
        public static MenuItemDefinition DuplicateTagMenuItem = new CommandMenuItemDefinition<DuplicateTagCommand>(ContextMenu, EditMenuItemGroup);    
        [ExportMenuItem]
        public static MenuItemDefinition DeleteTagMenuItem = new CommandMenuItemDefinition<DeleteTagCommand>(ContextMenu, EditMenuItemGroup);


        // Extract/Import Group
        [ExportMenuItem]
        public static MenuItemDefinition ExtractBitmapMenuItem = new CommandMenuItemDefinition<ExtractBitmapCommand>(ContextMenu, ExtractImportMenuItemGroup);
        [ExportMenuItem]
        public static MenuItemDefinition ExportModeJMSMenuItem = new CommandMenuItemDefinition<ExportModeJMSCommand>(ContextMenu, ExtractImportMenuItemGroup);
        [ExportMenuItem]
        public static MenuItemDefinition ExportCollJMSMenuItem = new CommandMenuItemDefinition<ExportCollJMSCommand>(ContextMenu, ExtractImportMenuItemGroup);
        [ExportMenuItem]
        public static MenuItemDefinition ExportPhmoJMSMenuItem = new CommandMenuItemDefinition<ExportPhmoJMSCommand>(ContextMenu, ExtractImportMenuItemGroup);
        [ExportMenuItem]
        public static MenuItemDefinition ExtractSoundMenuItem = new CommandMenuItemDefinition<ExtractSoundCommand>(ContextMenu, ExtractImportMenuItemGroup);
        [ExportMenuItem]
        public static MenuItemDefinition ExtractTagMenuItem = new CommandMenuItemDefinition<ExtractTagCommand>(ContextMenu, ExtractImportMenuItemGroup);
        [ExportMenuItem]
        public static MenuItemDefinition ImportTagMenuItem = new CommandMenuItemDefinition<ImportTagCommand>(ContextMenu, ExtractImportMenuItemGroup);
    }
}
