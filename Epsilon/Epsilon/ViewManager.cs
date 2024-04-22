using EpsilonLib.Commands;
using EpsilonLib.Core;
using Stylet;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Epsilon
{
    class ViewManager : Stylet.ViewManager
    {
        public ViewManager(ViewManagerConfig config) : base(config)
        {
        }

        protected override Type LocateViewForModel(Type modelType)
        {
            var explicitAttribute = modelType
                .GetCustomAttributes(typeof(ExplicitViewAttribute), false)
                .Cast<ExplicitViewAttribute>().FirstOrDefault();

            if(explicitAttribute != null)
            {
                return explicitAttribute.ViewType;
            }

            return base.LocateViewForModel(modelType);
        }

        public override void InitializeView(UIElement view, Type viewType)
        {
            base.InitializeView(view, viewType);

            if (view is Window window)
                RegisterCommandShortcuts(window);
        }

        private static void RegisterCommandShortcuts(Window window)
        {
            window.InputBindings.Clear();
            var commandRegistry = (ICommandRegistry)window.FindResource(typeof(ICommandRegistry));
            foreach (var command in commandRegistry.GetCommands())
            {
                if (command.Definition.KeyShortcut == KeyShortcut.None)
                    continue;

                window.InputBindings.Add(new InputBinding(command, command.Definition.KeyShortcut.KeyGesture));
            }
        }
    }
}
