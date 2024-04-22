using EpsilonLib.Commands;
using EpsilonLib.Editors;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace EpsilonLib.Shell.RecentFiles
{
    [ExportCommandHandler]
    class RecentFilesCommandHandler :
    ICommandHandler<RecentFilesCommandList>,
    ICommandListPopulator<RecentFilesCommandList>
    {
        private readonly IFileHistoryService _fileHistory;
        private readonly Lazy<IEditorService> _editorService;

        [ImportingConstructor]
        public RecentFilesCommandHandler(IFileHistoryService fileHistory, Lazy<IEditorService> editorService)
        {
            _fileHistory = fileHistory;
            _editorService = editorService;
        }

        public async void ExecuteCommand(Command command)
        {
            if (command.Tag is FileHistoryRecord record)
            {
                await _editorService.Value.OpenFileWithEditorAsync(record.FilePath, record.EditorProviderId);
            }
        }

        public void UpdateCommand(Command command)
        {

        }

        public IEnumerable<Command> PopulateCommandList(Command command)
        {
            if (_fileHistory.RecentlyOpened.Any())
            {
                int i = 0;
                while (i < 25 && i < _fileHistory.RecentlyOpened.Count())
                {
                    var record = _fileHistory.RecentlyOpened.ElementAt(i);
                    yield return new Command(command.Definition) { RequiresUpdate = false, DisplayText = record.FilePath, Tag = record };
                    i++;
                }
            }
            else
            {
                yield return new Command(command.Definition) { IsEnabled = false, DisplayText = "(empty)" };
            }
        }
    }
}
