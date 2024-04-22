using CacheEditor;
using CacheEditor.TagEditing;
using EpsilonLib.Dialogs;
using EpsilonLib.Logging;
using EpsilonLib.Menus;
using EpsilonLib.Settings;
using Shared;
using Stylet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using TagStructEditor.Fields;
using TagTool.Tags;

namespace TagResourceEditorPlugin
{
    partial class TagResourceEditorViewModel : TagEditorPluginBase
    {
        private readonly IShell _shell;
        private ICacheFile _cacheFile;
        private TagResourceItem _activeItem;
        private IField _displayField;
        private ISettingsService _settings;

        public TagResourceEditorViewModel(IShell shell, ICacheFile cacheFile, ISettingsService settings)
        {
            _shell = shell;
            _cacheFile = cacheFile;
            _settings = settings;
        }

        public ICollection<TagResourceItem> Items { get; set; }
        public TagResourceItem ActiveItem
        {
            get => _activeItem;
            set
            {
                if (SetAndNotify(ref _activeItem, value))
                {
                    LoadResource(_activeItem.DefinitionType, _activeItem.Resource);
                }
            }
        }

        public IField DisplayField
        {
            get => _displayField;
            set => SetAndNotify(ref _displayField, value);
        }

        public Task LoadAsync(object definition)
        {
            Items = new BindableCollection<TagResourceItem>();
            foreach(var resourceReference in ResourceReferenceCollector.Collect(_cacheFile.Cache, definition as TagStructure))
            {
                var resourceDefinitionType = TagResourceUtils.GetTagResourceDefinitionType(_cacheFile.Cache, resourceReference);
                if (resourceDefinitionType == null)
                    continue;

                var displayName = $"{resourceDefinitionType.Name} ({GetDisplayableResourceId(resourceReference)})";
                Items.Add(new TagResourceItem() { DisplayName = displayName, DefinitionType = resourceDefinitionType, Resource = resourceReference });
            }

            return Task.CompletedTask;
        }

        private async void LoadResource(Type definitionType, TagResourceReference resourceReference)
        {
            using (var progress = _shell.CreateProgressScope())
            {
                try
                {
                    await LoadResourceBlocking(definitionType, resourceReference, progress);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());

                    var alert = new AlertDialogViewModel
                    {
                        AlertType = Alert.Error,
                        Message = "An exception occured while loading the resource:",
                        SubMessage = $"{ex}"
                    };
                    _shell.ShowDialog(alert);
                }
            }
        }

        private async Task LoadResourceBlocking(Type definitionType, TagResourceReference resourceReference, IProgressReporter progress)
        {
            var resourceCache = _cacheFile.Cache.ResourceCache;
            progress.Report("Deserializing resource definition...");
            var resourceDefinition = await Task.Run(() => TagResourceUtils.GetResourceDefinition(resourceCache, definitionType, resourceReference));

            progress.Report("Creating fields...");

            // bit of a hack
            var config = new TagStructEditor.Configuration() { };
            var settings = _settings.GetCollection("DefinitionEditor");
            config.DisplayFieldTypes = settings.Get("DisplayFieldTypes", false);
            config.DisplayFieldOffsets = settings.Get("DisplayFieldOffsets", false);
            config.CollapseBlocks = settings.Get("CollapseBlocks", false);

            DisplayField = await Task.Run(() =>
            {
                var tagFieldFactory = new FieldFactory(_cacheFile.Cache, new TagStructEditor.Common.TagList(_cacheFile.Cache), config);
                var field = tagFieldFactory.CreateStruct(resourceDefinition.GetType());
                field.Populate(resourceDefinition);
                return field;
            });
        }

        int GetDisplayableResourceId(TagResourceReference resourceReference)
        {
            if (resourceReference.HaloOnlinePageableResource != null)
            {
                return resourceReference.HaloOnlinePageableResource.Page.Index;
            }
            else
            {
                return resourceReference.Gen3ResourceID.Index;
            }
        }

        internal bool PopulateContextMenu(EMenu menu, IField field)
        {
            if (field == null)
                return false;

            field.PopulateContextMenu(menu);
            return true;
        }

        protected override void OnClose()
        {
            base.OnClose();
            _cacheFile = null;
            if(_activeItem != null)
                _activeItem.Resource = null;
            _activeItem = null;
            _displayField?.Dispose();
            Items?.Clear();
        }
    }
}
