using System;
using System.ComponentModel.Composition;

namespace EpsilonLib.Menus
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ExportMenuItemAttribute : ExportAttribute
    {
        public ExportMenuItemAttribute() : base(typeof(MenuItemDefinition))
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ExportMenuBarAttribute : ExportAttribute
    {
        public ExportMenuBarAttribute() : base(typeof(MenuBarDefinition))
        {
        }
    }
}
