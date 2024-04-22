using CacheEditor;
using CacheEditor.TagEditing;
using CacheEditor.TagEditing.Messages;
using EpsilonLib.Dialogs;
using Shared;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TagTool.Cache;
using TagTool.Scripting;
using TagTool.Scripting.Compiler;
using TagTool.Tags.Definitions;

namespace BlamScriptEditorPlugin
{
    class ScriptTagEditorViewModel : TagEditorPluginBase
    {
        private readonly IShell _shell;
        private ICacheFile _cacheFile;
        private Scenario _definition;
        private string _scriptSourceCode;

        public string ScriptSourceCode
        {
            get => _scriptSourceCode;
            set => SetAndNotify(ref _scriptSourceCode, value);
        }

        public ScriptTagEditorViewModel(IShell shell, ICacheFile cacheFile, Scenario definition)
        {
            _shell = shell;
            _cacheFile = cacheFile;
            _definition = definition;
        }

        protected override async void OnMessage(object sender, object message)
        {
            if(message is DefinitionDataChangedEvent e)
            {
                _definition = (Scenario)e.NewData;
                Dispatcher.CurrentDispatcher.InvokeAsync(() => DecompileAsync());
            }
        }

        public async Task LoadAsync()
        {
            await DecompileAsync();
        }

        private async Task DecompileAsync()
        {
            try
            {
                var decompiler = new ScriptDecompiler(_cacheFile.Cache, _definition);
                ScriptSourceCode = await Task.Run(() =>
                {
                    using (var writer = new StringWriter())
                    {
                        decompiler.DecompileScripts(writer);
                        return writer.ToString();
                    }
                });
            }
            catch (Exception ex)
            {
                ScriptSourceCode = ex.ToString();
                //MessageBox.Show($"An exception occured while decompiling\n{ex}");
            }
        }

        public async void SaveAndCompile()
        {
            try
            {
                using (var progress = _shell.CreateProgressScope())
                {
                    progress.Report("Compiling script...");
                    var cache = _cacheFile.Cache;
                    await CompileSourceCode(cache, _definition, ScriptSourceCode);
                    progress.Report("Script Compiled", true, 1);
                    await Task.Delay(TimeSpan.FromSeconds(2.5));
                }
            }
            catch (Exception ex)
            {
                var error = new AlertDialogViewModel
                {
                    AlertType = Alert.Error,
                    Message = $"An exception occured while attempting to compile script\n{ex}"
                };
                _shell.ShowDialog(error);
            }
        }

        private async Task CompileSourceCode(GameCache cache, Scenario definition, string sourceCode)
        {
            var file = new FileInfo("tmp.txt");
            try
            {
                ScriptCompiler scriptCompiler = new ScriptCompiler(_cacheFile.Cache, _definition);

                using (var fs = new StreamWriter(file.OpenWrite()))
                    await fs.WriteAsync(sourceCode);

                await Task.Run(() => scriptCompiler.CompileFile(file));

                PostMessage(this, new DefinitionDataChangedEvent(definition));
            }
            finally
            {
                if (file.Exists)
                    file.Delete();
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            _cacheFile = null;
            _definition = null;
        }
    }
}
