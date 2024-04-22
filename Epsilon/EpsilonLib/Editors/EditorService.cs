using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EpsilonLib.Editors
{
    [Export(typeof(IEditorService))]
    class EditorService : IEditorService
    {
        private readonly Lazy<IShell> _shell;
        private readonly IEditorProvider[] _providers;
        private readonly IFileHistoryService _fileHistory;

        public IEnumerable<IEditorProvider> EditorProviders => _providers;

        [ImportingConstructor]
        public EditorService(
            [Import] Lazy<IShell> shell,
            [ImportMany]IEditorProvider[] providers,
            [Import] IFileHistoryService fileHistory)
        {
            _shell = shell;
            _providers = providers;
            _fileHistory = fileHistory;
        }

        IEditorProvider GetProvider(Guid id)
        {
            return _providers.FirstOrDefault(x => x.Id == id);
        }

        public async Task OpenFileWithEditorAsync(string filePath, Guid editorProviderId)
        {
            var shell = _shell.Value;

            using (var progress = shell.CreateProgressScope())
            {
                progress.Report($"Loading '{filePath}'...");
                var provider = GetProvider(editorProviderId);
                if (provider == null)
                    throw new NotSupportedException("Unable to find editor for this file type");

                await provider.OpenFileAsync(shell, filePath);
                await _fileHistory.RecordFileOpened(editorProviderId, filePath);
            }
        }

        public Task OpenFileAsync(string filePath)
        {
            var file = new FileInfo(filePath);
            foreach(var provider in EditorProviders)
            {
                if (provider.FileExtensions.Contains(file.Extension))
                    return OpenFileWithEditorAsync(filePath, provider.Id); 
            }

            throw new NotSupportedException();
        }
    }
}
