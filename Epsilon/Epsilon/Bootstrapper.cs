using Epsilon;
using Epsilon.Pages;
using EpsilonLib.Commands;
using EpsilonLib.Editors;
using EpsilonLib.Logging;
using EpsilonLib.Menus;
using EpsilonLib.Settings;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;


namespace WpfApp20
{
    public class Bootstrapper : MefBootstrapper<ShellViewModel>
    {
        private FileHistoryService _fileHistory;
        private IEditorService _editorService;
        private ISettingsCollection _settings;
        private string DefaultCachePath;
        private string DefaultPakPath;
        private double StartupPositionLeft;
        private double StartupPositionTop;
        private double StartupWidth;
        private double StartupHeight;
        private bool AlwaysOnTop;
        private string AccentColor;

        protected async override void Launch()
        {
            RegisterAdditionalLoggers();

            var startupTasks = new List<Task>();
            startupTasks.Add(_fileHistory.InitAsync());

            PrepareResources();

            await Task.WhenAll(startupTasks);

            App.Current.DispatcherUnhandledException += (o, args) =>
            {
                var dialog = new ExceptionDialog(args.Exception);
                dialog.Owner = App.Current.MainWindow;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if(dialog.ShowDialog() == false)
                    args.Handled = true;
            };

            var providers = _editorService.EditorProviders.ToList();

            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;

            base.Launch();

            PostLaunchInitShell();

            await OpenDefault(DefaultCachePath, providers.ElementAt(0));
            await OpenDefault(DefaultPakPath, providers.ElementAt(1));
        }

        private void RegisterAdditionalLoggers()
        {
            foreach (var logger in GetInstances<ILogHandler>())
                Logger.RegisterLogger(logger);
        }

        protected override void ConfigureIoC(CompositionBatch batch)
        {
            base.ConfigureIoC(batch);

            _fileHistory = new FileHistoryService(new XmlFileHistoryStore("filehistory.xml"));
            batch.AddExportedValue<IFileHistoryService>(_fileHistory);
        }

        protected override IEnumerable<Assembly> GetAssemblies()
        {
            var pluginManager = new PluginLoader();
            pluginManager.LoadPlugins();

            yield return Assembly.GetExecutingAssembly();
            yield return (typeof(IShell).Assembly); // EpsilonLib

            foreach (var file in pluginManager.Plugins)
                yield return file.Assembly;
        }

        private void PrepareResources()
        {
            foreach (var dict in GetInstances<ResourceDictionary>())
                App.Current.Resources.MergedDictionaries.Add(dict);

            _editorService = GetInstance<IEditorService>();
            _settings = GetInstance<ISettingsService>().GetCollection("General");
            DefaultCachePath = _settings.Get("DefaultTagCache", "");
            DefaultPakPath = _settings.Get("DefaultModPackage", "");
            AlwaysOnTop = _settings.Get("AlwaysOnTop", false);
            AccentColor = _settings.Get("AccentColor", "#007ACC");

            App.Current.Resources.Add(typeof(ICommandRegistry), GetInstance<ICommandRegistry>());
            App.Current.Resources.Add(typeof(IMenuFactory), GetInstance<IMenuFactory>());
            App.Current.Resources.Add(SystemParameters.MenuPopupAnimationKey, PopupAnimation.None);
            App.Current.Resources["AlwaysOnTop"] = AlwaysOnTop;
        }

        private void PostLaunchInitShell()
        {
            // better font rendering
            TextOptions.TextFormattingModeProperty.OverrideMetadata(typeof(Window),
               new FrameworkPropertyMetadata(TextFormattingMode.Display,
               FrameworkPropertyMetadataOptions.AffectsMeasure |
               FrameworkPropertyMetadataOptions.AffectsRender |
               FrameworkPropertyMetadataOptions.Inherits));

            double.TryParse(_settings.Get("StartupPositionLeft", ""), out StartupPositionLeft);
            double.TryParse(_settings.Get("StartupPositionTop", ""), out StartupPositionTop);
            if (StartupPositionLeft != 0 && StartupPositionTop != 0)
            {
                App.Current.MainWindow.Left = StartupPositionLeft;
                App.Current.MainWindow.Top = StartupPositionTop;
            }

            double.TryParse(_settings.Get("StartupWidth", ""), out StartupWidth);
            double.TryParse(_settings.Get("StartupHeight", ""), out StartupHeight);
            if (StartupWidth > 281 && StartupHeight > 500)
            {
                App.Current.MainWindow.Width = StartupWidth;
                App.Current.MainWindow.Height = StartupHeight;
            }

            InitAppearance();
        }

        private async Task OpenDefault(string path, IEditorProvider editorProvider)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            if(File.Exists(path))
                await _editorService.OpenFileWithEditorAsync(path, editorProvider.Id);
            else
                MessageBox.Show($"Startup cache or mod package could not be found at the following location:" + 
                        $"\n\n{path}", "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void InitAppearance()
        {
            App.Current.Resources["AccentColor"] = (Color)ColorConverter.ConvertFromString(AccentColor);

            var epsilonTheme = "Default";
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/Epsilon;component/Themes/" + epsilonTheme.ToString() + ".xaml", UriKind.Relative)
            });
        }
    }
}
