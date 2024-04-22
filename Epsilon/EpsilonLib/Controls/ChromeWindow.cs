using EpsilonLib.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace EpsilonLib.Controls
{
    public class ChromeWindow : Window
    {
        public ICommand MinimizeCommand
        {
            get => (ICommand)GetValue(MinimizeCommandProperty);
            set => SetValue(MinimizeCommandProperty, value);
        }

        public static readonly DependencyProperty MinimizeCommandProperty =
            DependencyProperty.Register("MinimizeCommand", typeof(ICommand), typeof(ChromeWindow), new PropertyMetadata(null));

        public ICommand MaximizeRestoreCommand
        {
            get => (ICommand)GetValue(MaximizeRestoreCommandProperty);
            set => SetValue(MaximizeRestoreCommandProperty, value);
        }

        public static readonly DependencyProperty MaximizeRestoreCommandProperty =
            DependencyProperty.Register("MaximizeRestoreCommand", typeof(ICommand), typeof(ChromeWindow), new PropertyMetadata(null));

        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(ChromeWindow), new PropertyMetadata(null));





        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AccentColorProperty =
            DependencyProperty.Register("AccentColor", typeof(int), typeof(ChromeWindow), new PropertyMetadata(0));



        public bool IsMaximized
        {
            get { return (bool)GetValue(IsMaximizedProperty); }
            set { SetValue(IsMaximizedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMaximized.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMaximizedProperty =
            DependencyProperty.Register("IsMaximized", typeof(bool), typeof(ChromeWindow), new PropertyMetadata(false));



        public bool CanMinimize
        {
            get { return (bool)GetValue(CanMinimizeProperty); }
            set { SetValue(CanMinimizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanMinimize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanMinimizeProperty =
            DependencyProperty.Register("CanMinimize", typeof(bool), typeof(ChromeWindow), new PropertyMetadata(false));



        public ChromeWindow()
        {
            MinimizeCommand = new DelegateCommand(Minimize);
            MaximizeRestoreCommand = new DelegateCommand(MaximizeOrRestoreWindowState);
            CloseCommand = new DelegateCommand(Close);
        }

        private void Minimize()
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeOrRestoreWindowState()
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            IsMaximized = WindowState == WindowState.Maximized;
            CanMinimize = ResizeMode == ResizeMode.CanMinimize;

            base.OnStateChanged(e);

        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            this.Style = (Style)FindResource(typeof(ChromeWindow));

           // DwmDropShadow.DropShadow(this);
        }
    }
}
