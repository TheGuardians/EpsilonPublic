using EpsilonLib.Commands;
using EpsilonLib.Logging;
using EpsilonLib.Shell;
using EpsilonLib.Shell.TreeModels;
using Stylet;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;

namespace CacheEditor.Components.CommandLog
{
    class CommandLogViewModel : CacheEditorTool
    {
        public const string ToolName = "CacheEditor.CommandLog";
        private ICacheEditor _editor;

        public CommandLogViewModel(ICacheEditor editor)
        {
            _editor = editor;

            Name = ToolName;
            DisplayName = "Command Log";
            PreferredLocation = EpsilonLib.Shell.PaneLocation.Right;
            PreferredWidth = 450;
            IsVisible = true;
        }

        public class CommandLogControls
        {
            public ICommand ClearCommandLogCommand { get; set; }

            public CommandLogControls()
            {
                ClearCommandLogCommand = new DelegateCommand(() => Logger.ClearCommandLog());
            }
        }

        public override bool InitialAutoHidden => true;
    }
}
