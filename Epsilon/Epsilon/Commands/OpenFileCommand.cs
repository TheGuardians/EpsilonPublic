using EpsilonLib.Commands;
using EpsilonLib.Editors;
using EpsilonLib.Logging;
using EpsilonLib.Shell.Commands;
using Microsoft.Win32;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;

namespace Epsilon.Commands
{
    [ExportCommandHandler]
    class OpenFileCommandHandler : ICommandHandler<OpenFileCommand>
    {
        private IEditorService _editorService;
        private IFileHistoryService _fileHistory;

        [Import]
        public Lazy<IShell> Shell { get; set; }

        [ImportingConstructor]
        public OpenFileCommandHandler(IEditorService editorService, IFileHistoryService fileHistory)
        {
            _editorService = editorService;
            _fileHistory = fileHistory;
        }

        public async void ExecuteCommand(Command command)
        {
            var filters = new List<string>()
            {
                "Tag Cache (*.dat;*.map;*.pak)|*.dat;*.map;*.pak"
            };
            var editorProviders = _editorService.EditorProviders.ToList();
            foreach (var provider in editorProviders)
            {
                var glob = string.Join(";", provider.FileExtensions.Select(ext => $"*{ext}").ToArray());
                filters.Add($"{provider.DisplayName} ({glob})|{glob}");
            }
            var filterString = string.Join("|", filters.ToArray());

            var dialog = new OpenFileDialog
            {
                Filter = filterString
            };

            if (dialog.ShowDialog() == false)
                return;

            var editorProviderIndex = dialog.FilterIndex - 2;

            if (editorProviderIndex < 0)
            {
                var ext = Path.GetExtension(dialog.FileName);
                switch (ext)
                {
                    case ".dat":
                    case ".map":
                        editorProviderIndex = 0;
                        break;
                    case ".pak":
                        editorProviderIndex = 1;
                        break;
                    default:
                        return;
                }
            }

            var editorProvider = editorProviders[editorProviderIndex];

            Logger.Info($"Opening file '{dialog.FileName}' using the editor provided by '{editorProvider.GetType().Name}'");

            try
            {
                await _editorService.OpenFileWithEditorAsync(dialog.FileName, editorProvider.Id);
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occurred while opening the file. Check the log for details.", "Error", MessageBoxButton.OK,  MessageBoxImage.Error);
                Logger.Error(ex.ToString());
            }
        }

        public void UpdateCommand(Command command)
        {

        }
    }
}
