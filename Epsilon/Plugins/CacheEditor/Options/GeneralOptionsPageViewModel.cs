using CacheEditor.Components.TagTree;
using EpsilonLib.Options;
using EpsilonLib.Settings;
using System.ComponentModel.Composition;

namespace CacheEditor.Options
{
    [Export(typeof(IOptionsPage))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class GeneralOptionsPageViewModel : OptionPageBase
    {
        private TagTreeViewMode _tagTreeViewMode;
        private TagTreeGroupDisplayMode _tagTreeGroupDisplayMode;
        private bool _showTagGroupAltNames;
        private bool _baseCacheWarnings;
        private ISettingsCollection _settings;

        [ImportingConstructor]
        public GeneralOptionsPageViewModel(ISettingsService settingsService) : base("Cache Editor", "Tag Browser")
        {
            _settings = settingsService.GetCollection(Settings.CollectionKey);
        }

        public TagTreeViewMode TagTreeViewMode
        {
            get => _tagTreeViewMode;
            set => SetOptionAndNotify(ref _tagTreeViewMode, value);
        }

        public TagTreeGroupDisplayMode TagTreeGroupDisplayMode
        {
            get => _tagTreeGroupDisplayMode;
            set => SetOptionAndNotify(ref _tagTreeGroupDisplayMode, value);
        }

        public bool ShowTagGroupAltNames
        {
            get => _showTagGroupAltNames;
            set => SetOptionAndNotify(ref _showTagGroupAltNames, value);
        }

        public bool BaseCacheWarnings
        {
            get => _baseCacheWarnings;
            set => SetOptionAndNotify(ref _baseCacheWarnings, value);
        }

        public override void Apply()
        {
            _settings.Set(Components.TagTree.Settings.TagTreeViewModeSetting.Key, TagTreeViewMode);
            _settings.Set(Components.TagTree.Settings.TagTreeGroupDisplaySetting.Key, TagTreeGroupDisplayMode);
            _settings.Set(Components.TagTree.Settings.ShowTagGroupAltNamesSetting.Key, ShowTagGroupAltNames);
            _settings.Set(Components.TagTree.Settings.BaseCacheWarningsSetting.Key, BaseCacheWarnings);
        }

        public override void Load()
        {
            TagTreeViewMode = _settings.Get<TagTreeViewMode>(Components.TagTree.Settings.TagTreeViewModeSetting);
            TagTreeGroupDisplayMode = _settings.Get<TagTreeGroupDisplayMode>(Components.TagTree.Settings.TagTreeGroupDisplaySetting);
            ShowTagGroupAltNames = _settings.Get<bool>(Components.TagTree.Settings.ShowTagGroupAltNamesSetting);
            BaseCacheWarnings = _settings.Get<bool>(Components.TagTree.Settings.BaseCacheWarningsSetting);
        }
    }
}
