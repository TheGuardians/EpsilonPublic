using EpsilonLib.Menus;
using EpsilonLib.Shell.Commands;
using EpsilonLib.Shell.RecentFiles;

namespace EpsilonLib.Shell
{
    public static class StandardMenus
    {
        public static object DefaultMenuItemGroup = new object();
        public static object FileOpenMenuItemGroup = new object();
        public static object FileRecentMenuItemGroup = new object();
        public static object FileExitMenuItemGroup = new object();
        public static object EditCopyPasteMenuItemGroup = new object();

        [ExportMenuBar]
        public static MenuBarDefinition MainMenu = new MenuBarDefinition();

        [ExportMenuItem]
        public static MenuItemDefinition FileMenu = new MenuItemDefinition(MainMenu, null, "File");
        [ExportMenuItem]
        public static MenuItemDefinition EditMenu = new MenuItemDefinition(MainMenu, null, "Edit");
        [ExportMenuItem]
        public static MenuItemDefinition ViewMenu = new MenuItemDefinition(MainMenu, null, "View");
        [ExportMenuItem]
        public static MenuItemDefinition ToolsMenu = new MenuItemDefinition(MainMenu, null, "Tools");
        [ExportMenuItem]
        public static MenuItemDefinition HelpMenu = new MenuItemDefinition(MainMenu, null, "Help");

        [ExportMenuItem]
        public static MenuItemDefinition FileNewMenu = new MenuItemDefinition(FileMenu, FileOpenMenuItemGroup, "New");

        [ExportMenuItem]
        public static MenuItemDefinition FileOpenMenu = new CommandMenuItemDefinition<OpenFileCommand>(FileMenu, null);

        //[ExportMenuItem]
        //public static MenuItemDefinition FileOpenMenuItem = new CommandMenuItemDefinition<OpenFileCommand>(FileOpenMenu, null);

        [ExportMenuItem]
        public static CommandMenuItemDefinition FileRecentFilesMenu = new CommandMenuItemDefinition<RecentFilesCommandList>(FileMenu, FileRecentMenuItemGroup);

        [ExportMenuItem]
        public static MenuItemDefinition FileExitMenuItem = new CommandMenuItemDefinition<ExitCommand>(FileMenu, FileExitMenuItemGroup);

        [ExportMenuItem]
        public static MenuItemDefinition EditCopyMenuItem = new CommandMenuItemDefinition<CopyCommand>(EditMenu, EditCopyPasteMenuItemGroup);
    }
}
