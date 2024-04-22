using EpsilonLib.Options;
using EpsilonLib.Settings;
using System.ComponentModel.Composition;

namespace DefinitionEditor.Options
{
    [Export(typeof(IOptionsPage))]
    class OptionsPageViewModel : OptionPageBase
    {
        private readonly ISettingsCollection _settings;

        private bool _displayFieldTypes;
        private bool _displayFieldOffsets;
        private bool _collapseBlocks;

        [ImportingConstructor]
        public OptionsPageViewModel(ISettingsService settingsService) : base("Cache Editor", "Definition Editor")
        {
            _settings = settingsService.GetCollection(Settings.CollectionKey);
        }

        public bool DisplayFieldTypes
        {
            get => _displayFieldTypes;
            set => SetOptionAndNotify(ref _displayFieldTypes, value);
        }

        public bool DisplayFieldOffsets
        {
            get => _displayFieldOffsets;
            set => SetOptionAndNotify(ref _displayFieldOffsets, value);
        }

        public bool CollapseBlocks
        {
            get => _collapseBlocks;
            set =>  SetOptionAndNotify(ref _collapseBlocks, value);
        }

        public override void Apply()
        {
            _settings.Set(Settings.DisplayFieldTypesSetting.Key, DisplayFieldTypes);
            _settings.Set(Settings.DisplayFieldOffsetsSetting.Key, DisplayFieldOffsets);
            _settings.Set(Settings.CollapseBlocksSetting.Key, CollapseBlocks);
        }

        public override void Load()
        {
            DisplayFieldTypes = _settings.Get(Settings.DisplayFieldTypesSetting.Key, (bool)Settings.DisplayFieldTypesSetting.DefaultValue);
            DisplayFieldOffsets = _settings.Get(Settings.DisplayFieldOffsetsSetting.Key, (bool)Settings.DisplayFieldOffsetsSetting.DefaultValue);
            CollapseBlocks = _settings.Get(Settings.CollapseBlocksSetting.Key, (bool)Settings.CollapseBlocksSetting.DefaultValue);
        }
    }
}
