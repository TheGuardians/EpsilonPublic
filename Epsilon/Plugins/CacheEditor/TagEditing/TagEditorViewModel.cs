using EpsilonLib.Commands;
using EpsilonLib.Core;
using EpsilonLib.Logging;
using EpsilonLib.Shell;
using EpsilonLib.Shell.TreeModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using TagTool.Cache;

namespace CacheEditor
{
    class TagEditorViewModel : Conductor<TagEditorPluginTabViewModel>.Collection.OneActive, ITagEditorPluginClient
    {
        private static readonly object LastOpenedTabKey = new object();
        private bool _pluginsLoaded = false;
        private ICacheEditingService _cacheEditingService;
        public IObservableCollection<TagEditorPluginTabViewModel> Documents => Items;

        public CachedTag Tag;
        public string FullName { get; set; }

        public ICommand CloseCommand { get; set; }
        public ICommand CopyTagNameCommand { get; set; }
        public ICommand CopyTagIndexCommand { get; set; }
        public ICommand TagTreeDeselect { get; set; }

        public TagEditorViewModel(ICacheEditingService cacheEditingService, TagEditorContext context)
        {
            _cacheEditingService = cacheEditingService;
            Tag = context.Instance;
            DisplayName = $"{Path.GetFileName(Tag.Name)}.{Tag.Group.Tag}";
            FullName = $"{Tag.Name}.{Tag.Group.Tag}";

            CloseCommand = new DelegateCommand(Close);
            CopyTagNameCommand = new DelegateCommand(() => ClipboardEx.SetTextSafe($"{Tag}"));
            CopyTagIndexCommand = new DelegateCommand(() => ClipboardEx.SetTextSafe($"0x{Tag.Index:X08}"));
            TagTreeDeselect = new DelegateCommand(() => (context.CacheEditor.TagTree as TreeModel).SelectedNode = null);

            LoadPlugins(context);
        }

        private async void LoadPlugins(TagEditorContext context)
        {
            foreach(var provider in _cacheEditingService.TagEditorPlugins)
            {
                if (!provider.ValidForTag(context.CacheEditor.CacheFile, context.Instance))
                    continue;

                var futurePlugin = LoadPluginAsync(context, provider);
                Items.Add(new TagEditorPluginTabViewModel(futurePlugin) { DisplayName = provider.DisplayName });
            }

            // if a tab was opened previously and we have a tab with the same name, active that one.
            // otherwse just activate the first.
            var sessionStore = GlobalServiceProvider.GetService<ISessionStore>();
            sessionStore.TryGetItem(LastOpenedTabKey, out string lastOpenedTab);
            var tabToActivate = Items.FirstOrDefault(x => x.DisplayName == lastOpenedTab) ?? Items.FirstOrDefault();

            ActiveItem = tabToActivate;
            _pluginsLoaded = true;
        }

        private async Task<ITagEditorPlugin> LoadPluginAsync(TagEditorContext context, ITagEditorPluginProvider provider)
        {
            try
            {
                var plugin = await provider.CreateAsync(context);
                plugin.Client = this;
                return plugin;
            }
            catch (Exception ex)
            {
                Logger.Error($"failed to load tag editor plugin '{provider.DisplayName}'. Exception: {ex}");
                throw;
            }
        }

        public void Close()
        {
            RequestClose();
            TagTreeDeselect.Execute(null);
        }

        protected override void OnClose()
        {
            base.OnClose();
            foreach (var document in Documents)
                ((IScreen)document).Close();
            Documents.Clear();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }


        public override void ActivateItem(TagEditorPluginTabViewModel item)
        {
            base.ActivateItem(item);

            if (item != null && _pluginsLoaded)
            {
                // store the last opened tab name
                var sessionStore = GlobalServiceProvider.GetService<ISessionStore>();
                sessionStore.StoreItem(LastOpenedTabKey, item.DisplayName);
            }
        }

        async void ITagEditorPluginClient.PostMessage(object sender, object message)
        {
            foreach(var tab in Items)
            {
                var plugin = await tab.LoadTask;

                if (plugin != sender)
                    plugin.OnMessage(sender, message);
            }
        }

        public override string ToString() => Tag.ToString();
    }
}
