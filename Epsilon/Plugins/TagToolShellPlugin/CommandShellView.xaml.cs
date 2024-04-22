using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace TagToolShellPlugin
{
    /// <summary>
    /// Interaction logic for CommandShellView.xaml
    /// </summary>

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CommandShellView : UserControl
    {
        private IDisposable _observable;

        public CommandShellView()
        {
            InitializeComponent();
            this.DataContextChanged += CommandShellView_DataContextChanged;
            this.Unloaded += CommandShellView_Unloaded;
            this.Loaded += CommandShellView_Loaded;
            outputText.RequestBringIntoView += OutputText_RequestBringIntoView;
            outputText.LayoutUpdated += OutputText_LayoutUpdated;
            outputText.IsVisibleChanged += OutputText_IsVisibleChanged;
            this.IsVisibleChanged += CommandShellView_IsVisibleChanged;
        }

        private void CommandShellView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue == true)
            {
                Dispatcher.InvokeAsync(() => { outputText.ScrollToEnd(); }, DispatcherPriority.ContextIdle);
            }
            
        }

        private void OutputText_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void OutputText_LayoutUpdated(object sender, EventArgs e)
        {
            
        }

        private void OutputText_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            
        }

        private void CommandShellView_Loaded(object sender, RoutedEventArgs e)
        {
            inputField.Focus();
        }


        private void CommandShellView_Unloaded(object sender, RoutedEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void CommandShellView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.DataContextChanged -= CommandShellView_DataContextChanged;

            if (_observable != null)
                _observable.Dispose();

            if (e.NewValue is CommandShellViewModel model)
            {
                model.Cleared += (s, _) => Dispatcher.Invoke(() => outputText.Clear());

                _observable = Observable.FromEventPattern<CommandShellOutputEventArgs>(
                    x => model.OutputReceived += x,
                    x => model.OutputReceived -= x)
                    .Buffer(TimeSpan.FromMilliseconds(25), 250)
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(buffer => PrintOutput(buffer.Select(o => o.EventArgs.OutputLine)));
            }
        }

        private void PrintOutput(IEnumerable<string> lines)
        {
            if (!lines.Any())
                return;

            var sb = new StringBuilder();
            foreach (var line in lines)
                sb.AppendLine(line);

            outputText.AppendText(sb.ToString());
 
            /*var lineCount = outputText.LineCount;
            int removeCount = 0;
            int n = 0;
            while (lineCount >= 100)
            {
                removeCount += outputText.GetLineLength(n);
                lineCount--;
                n++;
            }
            outputText.Text = outputText.Text.Remove(0, removeCount);
            */
            outputText.ScrollToEnd();
        }
    }
}
