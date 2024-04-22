using Stylet;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EpsilonLib.Menus
{
    public class MenuItemViewModel : PropertyChangedBase
    {
        public object GroupTag { get; set; }
        public MenuItemViewModel Parent { get; set; }
        public ICommand Command { get; set; }

        private bool _isVisible = true;
        private bool _isEnabled = true;
        private bool _isChecked;
        private string _text;
        private ObservableCollection<MenuItemViewModel> _children;
        private bool _isSubmenuOpen = false;
        private string _inputGestureText;

        public static MenuItemViewModel Separator => new MenuItemViewModel() { IsSeparator = true };

        public MenuItemViewModel()
        {
            _children = new ObservableCollection<MenuItemViewModel>();
        }

        public IList<MenuItemViewModel> Children => _children;

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (SetAndNotify(ref _isVisible, value))
                {
                    OnVisibilityChanged();
                }
            }
        }


        public bool IsSubmenuOpen
        {
            get => _isSubmenuOpen;
            set
            {
                if (SetAndNotify(ref _isSubmenuOpen, value))
                {
                    if (value)
                        OnSubmenuOpened();
                }
            }
        }

        public string InputGestureText
        {
            get => _inputGestureText;
            set => SetAndNotify(ref _inputGestureText, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetAndNotify(ref _isEnabled, value);
        }

        public bool IsChecked
        {
            get => _isChecked;
            set => SetAndNotify(ref _isChecked, value);
        }

        public string Text
        {
            get => _text;
            set => SetAndNotify(ref _text, value);
        }

        public bool IsSeparator
        {
            get;
            set;
        }

        public void AddChild(MenuItemViewModel child, object group)
        {
            child.Parent = this;
            child.GroupTag = group;
            _children.Add(child);
        }

        protected virtual void OnVisibilityChanged()
        {
            if (Parent != null)
                Parent.OnChildVisibilityChanged(this);
        }

        protected virtual void OnChildVisibilityChanged(MenuItemViewModel child)
        {
            if (child.IsSeparator)
                return;

            UpdateSeparatorVisibility();
            IsVisible = Children.Any(x => x.IsVisible);
        }

        private void UpdateSeparatorVisibility()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].IsSeparator)
                    Children[i].IsVisible = SeparatorShouldBeVisible(i);
            }
        }

        private bool SeparatorShouldBeVisible(int index)
        {
            bool visibleBefore = false;
            bool visibleAfter = false;

            for (int j = index - 1; j >= 0; j--)
            {
                if (Children[j].IsVisible)
                {
                    visibleBefore = !Children[j].IsSeparator;
                    break;
                }
            }

            if (!visibleBefore)
                return false;

            for (int j = index + 1; j < Children.Count; j++)
            {
                if (Children[j].IsVisible)
                {
                    visibleAfter = !Children[j].IsSeparator;
                    break;
                }
            }

            return visibleBefore && visibleAfter;
        }

        protected virtual void OnSubmenuOpened()
        {
            foreach (var child in Children)
                child.OnAboutToDisplay();

            OnAboutToDisplay();
        }

        public new void Refresh()
        {
            if (IsSeparator)
                return;

            if (Command == null)
            {
                foreach (var child in Children)
                    child.Refresh();

                IsVisible = Children.Count > 0 && Children.Any(x => x.IsVisible);
            }
            else
            {
                Command.CanExecute(null);
            }
        }

        protected virtual void OnAboutToDisplay()
        {

        }
    }
}
