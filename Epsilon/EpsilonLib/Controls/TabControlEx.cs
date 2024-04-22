using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace EpsilonLib.Controls
{
    [TemplatePart(Name = "PART_ItemsHolder", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_SelectedContentHost", Type = typeof(ContentPresenter))]
    public class TabControlEx : TabControl
    {
        private Panel _itemsHolderPanel;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _itemsHolderPanel = CreateItemsHolder();

            var contentPresenter = GetTemplateChild("PART_SelectedContentHost");
            var border = VisualTreeHelper.GetParent(contentPresenter) as Border;
            border.Child = _itemsHolderPanel;

            UpdateSelectedItem();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_itemsHolderPanel == null)
                return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    _itemsHolderPanel.Children.Clear();
                    break;

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            ContentPresenter cp = FindChildContentPresenter(item);
                            if (cp != null)
                                _itemsHolderPanel.Children.Remove(cp);
                        }
                    }

                    // Don't do anything with new items because we don't want to
                    // create visuals that aren't being shown

                    UpdateSelectedItem();
                    break;

                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException("Replace not implemented yet");
            }
        }

        private void UpdateSelectedItem()
        {
            if (_itemsHolderPanel == null)
                return;

            // Generate a ContentPresenter if necessary
            TabItem item = GetSelectedTabItem();
            if (item != null)
                CreateChildContentPresenter(item);

            // show the right child
            foreach (ContentPresenter child in _itemsHolderPanel.Children)
                child.Visibility = ((child.Tag as TabItem).IsSelected) ? Visibility.Visible : Visibility.Collapsed;
        }

        private ContentPresenter CreateChildContentPresenter(object item)
        {
            if (item == null)
                return null;

            ContentPresenter cp = FindChildContentPresenter(item);

            if (cp != null)
                return cp;

            cp = new ContentPresenter();
            cp.Content = (item is TabItem) ? (item as TabItem).Content : item;
            cp.ContentTemplate = this.SelectedContentTemplate;
            cp.ContentTemplateSelector = this.SelectedContentTemplateSelector;
            cp.ContentStringFormat = this.SelectedContentStringFormat;
            cp.Visibility = Visibility.Collapsed;
            cp.Tag = (item is TabItem) ? item : (this.ItemContainerGenerator.ContainerFromItem(item));
            _itemsHolderPanel.Children.Add(cp);

            return cp;
        }

        private ContentPresenter FindChildContentPresenter(object data)
        {
            if (data is TabItem tabItem)
                data = tabItem.Content;

            if (data == null)
                return null;

            if (_itemsHolderPanel == null)
                return null;

            foreach (ContentPresenter cp in _itemsHolderPanel.Children)
            {
                if (cp.Content == data)
                    return cp;
            }

            return null;
        }

        private Grid CreateItemsHolder()
        {
            var grid = new Grid();
            Binding binding = new Binding(PaddingProperty.Name);
            binding.Source = this;
            grid.SetBinding(Grid.MarginProperty, binding);

            binding = new Binding(SnapsToDevicePixelsProperty.Name);
            binding.Source = this;
            grid.SetBinding(Grid.SnapsToDevicePixelsProperty, binding);

            return grid;
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            UpdateSelectedItem();
        }

        protected TabItem GetSelectedTabItem()
        {
            if (SelectedItem == null)
                return null;

            var tabItem = SelectedItem as TabItem;
            if(tabItem == null)
                tabItem = ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;

            return tabItem;
        }
    }
}
