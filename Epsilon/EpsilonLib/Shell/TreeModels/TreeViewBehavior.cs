using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EpsilonLib.Shell.TreeModels
{
    public static class TreeViewBehavior
    {
        public static bool GetBringIntoViewWhenSelected(TreeViewItem treeViewItem)
        {
            return (bool)treeViewItem.GetValue(BringIntoViewWhenSelectedProperty);
        }

        public static void SetBringIntoViewWhenSelected(TreeViewItem treeViewItem, bool value)
        {
            treeViewItem.SetValue(BringIntoViewWhenSelectedProperty, value);
        }

        public static readonly DependencyProperty BringIntoViewWhenSelectedProperty =
            DependencyProperty.RegisterAttached("BringIntoViewWhenSelected", typeof(bool),
            typeof(TreeViewBehavior), new UIPropertyMetadata(false, OnBringIntoViewWhenSelectedChanged));

        static void OnBringIntoViewWhenSelectedChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem item = depObj as TreeViewItem;
            if (item == null)
                return;

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
                item.BringIntoView();
        }

        public static object GetModel(DependencyObject obj)
        {
            return (object)obj.GetValue(ModelProperty);
        }

        public static void SetModel(DependencyObject obj, object value)
        {
            obj.SetValue(ModelProperty, value);
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.RegisterAttached("Model", typeof(object), typeof(TreeViewBehavior), new PropertyMetadata(OnModelChanged));

        private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeView = (TreeView)d;

            if(e.OldValue is ITreeViewEventSink oldSink)
            {
                if (oldSink.Source is IDisposable disposable)
                    disposable.Dispose();
                oldSink.Source = null;
            }

            if (e.NewValue is ITreeViewEventSink newSink)
                newSink.Source = new TreeViewModelBinding(treeView, newSink);
        }

        class TreeViewModelBinding : IDisposable
        {
            private readonly TreeView _treeView;
            private readonly ITreeViewEventSink _sink;
            private Dictionary<RoutedEvent, RoutedEventHandler> _attachedHandlers;

            public TreeViewModelBinding(TreeView treeView, ITreeViewEventSink sink)
            {
                _treeView = treeView;
                _sink = sink;
                _attachedHandlers = new Dictionary<RoutedEvent, RoutedEventHandler>();

                BindingOperations.SetBinding(_treeView, TreeView.ItemsSourceProperty,
                    new Binding(nameof(TreeModel.Nodes)) { Source = _sink });

                AttachHandler(TreeViewItem.MouseDoubleClickEvent, CreateNodeEventHandler(_sink.NodeDoubleClicked));
                AttachHandler(TreeViewItem.SelectedEvent, CreateNodeEventHandler(_sink.NodeSelected));
            }

            public void Dispose()
            {
                BindingOperations.ClearBinding(_treeView, TreeView.ItemsSourceProperty);

                foreach (var pair in _attachedHandlers.ToList())
                    DetachHandler(pair.Key, pair.Value);
            }

            private void DetachHandler(RoutedEvent e, RoutedEventHandler handler)
            {
                _treeView.RemoveHandler(e, handler);
                _attachedHandlers.Remove(e);
            }

            private void AttachHandler(RoutedEvent e, RoutedEventHandler handler)
            {
                _attachedHandlers.Add(e, handler);
                _treeView.AddHandler(e, handler);
            }

            private RoutedEventHandler CreateNodeEventHandler(Action<TreeNodeEventArgs> handler)
            {
                return new RoutedEventHandler((sender, e) =>
                {
                    var treeViewItem = (e.OriginalSource as DependencyObject).FindAncestors<TreeViewItem>().FirstOrDefault();
                    if(treeViewItem != null && treeViewItem.IsMouseOver)
                        handler(new TreeNodeEventArgs(treeViewItem.DataContext as ITreeNode));
                });
            }
        }
    }
}
