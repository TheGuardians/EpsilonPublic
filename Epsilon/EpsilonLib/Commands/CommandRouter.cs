using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EpsilonLib.Commands
{
    [Export(typeof(ICommandRouter))]
    class CommandRouter : ICommandRouter
    {
        private readonly Dictionary<Type, ICommandHandler> _globalHandlers;
            
        [ImportingConstructor]
        public CommandRouter(
            [ImportMany] ICommandTarget[] exportedCommandTargets)
        {
            _globalHandlers = new Dictionary<Type, ICommandHandler>();
            foreach (var target in exportedCommandTargets)
                RegisterGlobalHandlers(target);
        }

        private void RegisterGlobalHandlers(ICommandTarget target)
        {
            foreach(var type in GetHandlerInterfaces(target))
            {
                var commandType = type.GetGenericArguments()[0];

                if(!_globalHandlers.TryGetValue(commandType, out ICommandHandler handler))
                    handler = _globalHandlers[commandType] = CreateHandler(commandType, target);
                else
                {
                    if (handler.Target != target)
                        throw new InvalidOperationException("Only one global command handler allowed per command!");
                }
            }
        }

        public IEnumerable<Type> GetHandlerInterfaces(ICommandTarget target)
        {
            foreach(var type in target.GetType().GetInterfaces())
            {
                if (!type.IsGenericType)
                    continue;
                var genericType = type.GetGenericTypeDefinition();
                if (genericType != typeof(ICommandHandler<>) && genericType != typeof(ICommandListPopulator<>))
                    continue;

                yield return type;
            }
        }

        public ICommandHandler GetHandler(Command command)
        {
            ICommandHandler handler = FindCommandHandlerInVisualTree(command);
            if (handler != null)
                return handler;

            _globalHandlers.TryGetValue(command.Definition.GetType(), out handler);

            return handler;
        }

        private ICommandHandler CreateHandler(Type commandType, ICommandTarget target)
        {
            return (ICommandHandler)Activator.CreateInstance(typeof(CommandHandler<>).MakeGenericType(commandType), target);
        }

        private ICommandHandler FindCommandHandlerInVisualTree(Command command)
        {
            foreach(var window in Application.Current.Windows.OfType<Window>())
            {
                if (!window.IsActive)
                    continue;

                var element = (FocusManager.GetFocusedElement(window) ?? window) as DependencyObject;

                // walk up the visual tree from the focused element until looking for DataContext (ICommandTarget) that can handle this command
                while (element != null)
                {
                    if (element is FrameworkElement frameworkElement)
                    {
                        if (frameworkElement.DataContext is ICommandTarget target)
                        {
                            var commandType = command.Definition.GetType();
                            if (GetHandlerInterfaces(target).Any(type => type.GetGenericArguments()[0] == commandType))
                                return CreateHandler(commandType, target);
                        }
                    }

                    element = VisualTreeHelper.GetParent(element);
                }
            }

            return null;
        }
    }
}
