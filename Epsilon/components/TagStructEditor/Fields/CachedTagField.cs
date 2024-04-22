using EpsilonLib.Menus;
using EpsilonLib.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TagStructEditor.Common;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using static TagTool.Tags.Definitions.Bitmap.Sequence;

namespace TagStructEditor.Fields
{
    public class CachedTagField : ValueField
    {
        private readonly Func<CachedTag> _browseTagCallback;

        public CachedTagField(
            ValueFieldInfo info, 
            TagList tagList, 
            Action<CachedTag> openTagCallback, 
            Func<CachedTag> browseTagCallback) : base(info)
        {
            _browseTagCallback = browseTagCallback;

            Groups = (info.Attribute?.ValidTags == null) 
                ? tagList.Groups 
                : GetValidGroups(info, tagList);

            GotoCommand = new DelegateCommand(() => openTagCallback(SelectedInstance.Instance), () => SelectedInstance != null);
            NullCommand = new DelegateCommand(Null, () => SelectedGroup != null);
            BrowseCommand = new DelegateCommand(BrowseTag);
            CopyTagNameCommand = new DelegateCommand(CopyTagName, () => SelectedInstance != null);
            CopyTagIndexCommand = new DelegateCommand(CopyTagIndex, () => SelectedInstance != null);
            PasteTagNameCommand = new DelegateCommand(PasteTagName);
        }

        private IEnumerable<TagGroupItem> GetValidGroups(ValueFieldInfo info, TagList tagList )
        {
            var validTags = info.Attribute.ValidTags.ToList();
            var parentTags = new string[] { "obje", "devi", "unit", "item", "rm  " };
            var match = parentTags.FirstOrDefault(p => validTags.Contains(p));

            if (!string.IsNullOrEmpty(match))
            {
                validTags.Remove(match);

                switch(match)
                {
                    case "obje":
                        validTags.AddRange( new string[] { "argd", "armr", "bipd", "bloc", 
                            "cobj", "crea", "ctrl", "efsc", "eqip", "gint", "mach", "proj", "scen", 
                            "ssce", "term", "unit", "vehi", "weap"});
                        break;
                    case "rm  ":
                        validTags.AddRange(new string[] { "rmbk", "rmcs", "rmct", "rmd ",
                            "rmfl", "rmgl", "rmhg", "rmsh", "rmss", "rmtr", "rmw ", "rmzo" });
                        break;
                    case "devi":
                        validTags.AddRange(new string[] { "argd", "ctrl", "mach", "term" });
                        break;
                    case "unit":
                        validTags.AddRange(new string[] { "bipd", "gint", "vehi" });
                        break;
                    case "item":
                        validTags.AddRange(new string[] { "eqip", "weap" }) ;
                        break;
                }
            }

            return tagList.Groups.Where(x => validTags.Contains(x.TagAscii));
        }

        public IEnumerable<TagGroupItem> Groups { get; set; }
        public TagGroupItem SelectedGroup { get; set; }
        public TagInstanceItem SelectedInstance { get; set; }

        public bool SelectedGroupValid => SelectedGroup != null;
        public bool SelectedInstanceValid => SelectedInstance != null;

        public DelegateCommand GotoCommand { get; }
        public DelegateCommand NullCommand { get; }
        public DelegateCommand BrowseCommand { get; }
        public DelegateCommand CopyTagNameCommand { get; }
        public DelegateCommand CopyTagIndexCommand { get; }
        public DelegateCommand PasteTagNameCommand { get; }

        public override void Accept(IFieldVisitor visitor) => visitor.Visit(this);

        protected override void OnPopulate(object value)
        {
            var instance = (CachedTag)value;
            if (instance != null)
            {
                SelectCachedTag(instance);
            }
            else
            {
                SelectedGroup = null;
                SelectedInstance = null;
            }

            InvalidateCommands();
        }

        private void SelectCachedTag(CachedTag instance)
        {
            SelectedGroup = Groups.FirstOrDefault(item => item.Group == instance.Group);
            SelectedInstance = SelectedGroup?.Instances.FirstOrDefault(item => $"{item.Instance}" == $"{instance}");
        }

        public void OnSelectedInstanceChanged()
        {
            if (SelectedInstance != null || SelectedGroup == null)
                SetActualValue(SelectedInstance?.Instance);

            InvalidateCommands();
        }

        public void OnSelectedGroupChanged()
        {
            InvalidateCommands();
        }

        private void InvalidateCommands()
        {
            GotoCommand.RaiseCanExecuteChanged();
            NullCommand.RaiseCanExecuteChanged();
            BrowseCommand.RaiseCanExecuteChanged();
            CopyTagIndexCommand.RaiseCanExecuteChanged();
            CopyTagNameCommand.RaiseCanExecuteChanged();
            PasteTagNameCommand.RaiseCanExecuteChanged();
        }

        public void Null()
        {
            SelectedGroup = null;
        }

        private void BrowseTag()
        {
            var instance = _browseTagCallback();
            if (instance == null)
                return;

            var group = ValidateTagGroup(instance.Group.ToString());
            if (group != null)
            {
                SelectCachedTag(instance);
            }
        }

        private void CopyTagName()
        {
            ClipboardEx.SetTextSafe($"{SelectedInstance.Instance}");
        }

        private void CopyTagIndex()
        {
            ClipboardEx.SetTextSafe($"0x{SelectedInstance.Instance.Index:X08}");
        }

        private void PasteTagName()
        {
            string[] split = Clipboard.GetText().Split('.');
            if (split.Count() != 2)
                return;
            else
            {
                try
                {
                    var group = ValidateTagGroup(split.Last());
                    if (group != null)
                    {
                        SelectedGroup = group;
                        SelectedInstance = SelectedGroup.Instances.First(item => item.Name == split.First()) ?? SelectedInstance;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is Exception)
                        MessageBox.Show($"{Clipboard.GetText()} is not a valid tag name.", "Tag Not Found");
                    else
                        throw;
                }
            }
        }

        public TagGroupItem ValidateTagGroup(string input)
        {
            // check long form
            input = input.Trim();
            TagGroupItem group = Groups.FirstOrDefault(item =>
                ((TagTool.Cache.Gen3.TagGroupGen3)item.Group).Name == input);

            // check abbreviations
            if (group == null)
            {
                if (input.Length == 3)
                    input += " ";

                group = Groups.FirstOrDefault(item => item.TagAscii == input);
            }

            // alert if not found
            if (group == null)
            {
                var validgroups = string.Join(", ", 
                    Groups.Select(g => 
                    ((TagTool.Cache.Gen3.TagGroupGen3)g.Group).Name + $" ({g.TagAscii})"));

                if (Groups.Count() > 20)
                    validgroups = "unspecified";

                MessageBox.Show($"\'{input}\' is not a valid group for this tag reference."
                    + $"\n\nValid tag groups: {validgroups}"
                    , "Invalid Tag Group");
            }

            return group;
        }

        protected override void OnPopulateContextMenu(EMenu menu)
        {
            menu.Group("Copy")
                .Add("Copy Tag Name", command: CopyTagNameCommand)
                .Add("Copy Tag Index", command: CopyTagIndexCommand)
                .Add("Paste Tag Name", command: PasteTagNameCommand);

            menu.Group("CachedTag2")
                .Add("Open in a new tab", command: GotoCommand)
                .Add("Select a tag", tooltip: "Select a tag", command: BrowseCommand);

            menu.Group("CachedTag3")
                .Add("Null", tooltip: "Null this tag reference", command: NullCommand);
        }

        public override void Dispose()
        {
            base.Dispose();
            //foreach (var group in Groups)
            //    group.Instances.Clear();
            Groups = null;
            SelectedGroup = null;
            SelectedInstance = null;
        }
    }
}
