using EpsilonLib.Menus;
using System;
using System.Windows;
using System.Windows.Controls;


namespace EpsilonLib.Behaviors
{
    public class ContextMenuBehavior
    {
        public static object MenuStyleKey = new object();

        public static readonly DependencyProperty MenuProperty = 
            DependencyProperty.RegisterAttached("Menu", typeof(MenuItemDefinition), typeof(ContextMenuBehavior), new PropertyMetadata(OnMenuChanged));
        public static readonly DependencyProperty MenuSelectorProperty =
           DependencyProperty.RegisterAttached("MenuSelector", typeof(IContextMenuSelector), typeof(ContextMenuBehavior), new PropertyMetadata(OnMenuSelectorChanged));

        public static MenuItemDefinition GetMenu(DependencyObject obj) => (MenuItemDefinition)obj.GetValue(MenuProperty);
        public static void SetMenu(DependencyObject obj, MenuItemDefinition value) => obj.SetValue(MenuProperty, value);
        public static IContextMenuSelector GetMenuSelector(DependencyObject obj) => (IContextMenuSelector)obj.GetValue(MenuSelectorProperty);
        public static void SetMenuSelector(DependencyObject obj, IContextMenuSelector value) => obj.SetValue(MenuSelectorProperty, value);

        private static void OnMenuSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;

            element.ContextMenuOpening -= Element_ContextMenuOpening;

            if (e.NewValue != null)
                element.ContextMenuOpening += Element_ContextMenuOpening;
        }

        private static void OnMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;

            element.ContextMenuOpening -= Element_ContextMenuOpening;
            element.ContextMenuOpening += Element_ContextMenuOpening;
        }

        private static void Element_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var host = sender as FrameworkElement;
            var menu = GetMenuSelector(host)?.SelectMenu(e.OriginalSource as FrameworkElement, e) ?? GetMenu(sender as DependencyObject);
            if (menu == null)
                return;

            RunContextMenu(e.OriginalSource as FrameworkElement, menu);
        }

        private static void RunContextMenu(FrameworkElement host, MenuItemDefinition definition)
        {
            var menuFactory = (IMenuFactory)host.TryFindResource(typeof(IMenuFactory));
            if (menuFactory == null)
                throw new InvalidOperationException("No menu factory found in application resources!");

            var menu = menuFactory.GetMenu(definition);
            var ctxMenu = new ContextMenu();
            ctxMenu.Style = (Style)host.FindResource(MenuStyleKey);
            ctxMenu.ItemsSource = menu.Children;
            ctxMenu.IsOpen = true;
        }
    }
}
