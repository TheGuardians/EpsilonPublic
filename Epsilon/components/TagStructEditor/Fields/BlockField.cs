using EpsilonLib.Logging;
using EpsilonLib.Menus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using TagStructEditor.Common;
using TagStructEditor.Helpers;
using TagTool.Commands.Editing;

namespace TagStructEditor.Fields
{
    public class BlockField : ValueField, IExpandable
    {
        private bool _isExpanded = true;
        private static ObservableNonGenericCollection CopiedBlocks = new ObservableNonGenericCollection();
        private static Type CopiedBlockType;
        private bool CanPaste = false;

        public BlockField(Type elementType, ValueFieldInfo info) : base(info)
        {
            ElementType = elementType;
            CanPaste = CopiedBlockType != null 
                && CopiedBlocks.Count > 0 
                && CopiedBlockType == ElementType;

            Block = new ObservableNonGenericCollection();
            Block.CollectionChanged += Block_CollectionChanged;
            AddCommand = new DelegateCommand(Add, () => !IsFixedSize);
            InsertCommand = new DelegateCommand(Insert, () => CurrentElementValid && !IsFixedSize);
            DeleteCommand = new DelegateCommand(Delete, () => CurrentElementValid && !IsFixedSize);
            DeleteAllCommand = new DelegateCommand(DeleteAll, () => CurrentElementValid && !IsFixedSize);
            DuplicateCommand = new DelegateCommand(Duplicate, () => CurrentElementValid && !IsFixedSize);
            ShiftUpCommand = new DelegateCommand(() => Shift(-1), () => CurrentElementValid && !IsFixedSize && CurrentIndex > 0);
            ShiftDownCommand = new DelegateCommand(() => Shift(1), () => CurrentElementValid && !IsFixedSize && CurrentIndex < Block.Count - 1);
            ExpandAllCommand = new DelegateCommand(ExpandChildren, () => CurrentElementValid);
            CollapseAllCommand = new DelegateCommand(CollapseChildren, () => CurrentElementValid);
            GotoIndexCommand = new DelegateCommand(GotoIndex, () => Block.Count > 0);

            CopyBlockCommand = new DelegateCommand(CopyBlock, () => CurrentElementValid && !IsFixedSize);
            CopyRangeCommand = new DelegateCommand(CopyRange, () => CurrentElementValid && !IsFixedSize);
            PasteBlocksAtEndCommand = new DelegateCommand(PasteBlocks, () => Block != null && !IsFixedSize && CanPaste);
        }


        public Type ElementType { get; set; }
        public IField Template { get; set; }
        public int CurrentIndex { get; set; } = -1;
        public ObservableNonGenericCollection Block { get; set; } = new ObservableNonGenericCollection();
        public bool IsFixedSize { get; set; }

        public DelegateCommand AddCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand DeleteAllCommand { get; set; }
        public DelegateCommand InsertCommand { get; set; }
        public DelegateCommand DuplicateCommand { get; set; }
        public DelegateCommand CopyBlockCommand { get; set; }
        public DelegateCommand CopyRangeCommand { get; set; }
        public DelegateCommand PasteBlocksAtEndCommand { get; set; }
        public DelegateCommand ShiftDownCommand { get; set; }
        public DelegateCommand ShiftUpCommand { get; set; }
        public DelegateCommand ExpandAllCommand { get; set; }
        public DelegateCommand CollapseAllCommand { get; set; }
        public DelegateCommand GotoIndexCommand { get; set; }

        bool CurrentElementValid => Block != null && CurrentIndex != -1;

        public bool IsExpanded
        {
            // only show as expanded if there is any element to display
            get => _isExpanded && CurrentElementValid;
            set
            {
                _isExpanded = value;
            }
        }

        public override void Accept(IFieldVisitor visitor)
        {
             visitor.Visit(this);
        }

        /// <summary>
        /// Called when the field should be populated with a value
        /// </summary>
        /// <param name="value">Value to populate with</param>
        protected override void OnPopulate(object value)
        {
            CurrentIndex = -1;
            // reset the current index so that the change handler gets invoked even if the value has not changed
            Block.BaseCollection = (IList)value;
        }

        private void Block_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // update the current index
            CurrentIndex = DetermineNextCurrentIndex(e);
            // commit the changes to the underlying field storage
            SetActualValue(Block.BaseCollection);

            NotifyCommandAvailability();
        }

        private int DetermineNextCurrentIndex(NotifyCollectionChangedEventArgs e)
        {
            // if the block is empty after the change, set the current index to -1
            if (Block.Count < 1) return -1;

            if (e.Action == NotifyCollectionChangedAction.Reset)
                return 0;
            else if (e.Action == NotifyCollectionChangedAction.Add)
                // set the current index to the index of the new element
                return e.NewStartingIndex;
            else if (e.Action == NotifyCollectionChangedAction.Remove)
                // set the current index to the previous element if any, otherwise leave it unchanged
                return CurrentIndex > 0 ? (CurrentIndex - 1) : CurrentIndex;
            else if (e.Action == NotifyCollectionChangedAction.Move)
                // set the current index to the shifted element index
                return CurrentIndex = e.NewStartingIndex;
            else
                throw new NotSupportedException();
        }

        private void NotifyCommandAvailability()
        {
            AddCommand.RaiseCanExecuteChanged();
            InsertCommand.RaiseCanExecuteChanged();
            DuplicateCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            DeleteAllCommand.RaiseCanExecuteChanged();
            ShiftDownCommand.RaiseCanExecuteChanged();
            ShiftUpCommand.RaiseCanExecuteChanged();
            CopyBlockCommand.RaiseCanExecuteChanged();
            CopyRangeCommand.RaiseCanExecuteChanged();
            PasteBlocksAtEndCommand.RaiseCanExecuteChanged();
        }

        public void OnCurrentIndexChanged()
        {
            if (CurrentIndex < 0 || CurrentIndex >= Block.Count)
                return;

            // Load the page
            Template.Populate(Block[CurrentIndex]);
        }

        private void Add()
        {
            Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", FieldHelper.GetFieldPath(Template.Parent), Logger.CommandEvent.CommandType.none, $"addblockelements {FieldHelper.GetFieldPath(Template.Parent)}");
            Block.Add(Utils.ActivateType(ElementType));            
        }

        private void Delete()
        {
            Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", FieldHelper.GetFieldPath(Template.Parent), Logger.CommandEvent.CommandType.none, $"removeblockelements {FieldHelper.GetFieldPath(Template.Parent)} {CurrentIndex} 1");
            Block.RemoveAt(CurrentIndex);           
        }

        private void DeleteAll()
        {
            Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", FieldHelper.GetFieldPath(Template.Parent), Logger.CommandEvent.CommandType.none, $"removeblockelements {FieldHelper.GetFieldPath(Template.Parent)} 0 *");
            Block.Clear();          
        }

        private void Insert()
        {
            Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", FieldHelper.GetFieldPath(Template.Parent), Logger.CommandEvent.CommandType.none, $"addblockelements {FieldHelper.GetFieldPath(Template.Parent)} 1 {CurrentIndex}");
            Block.Insert(CurrentIndex, Utils.ActivateType(ElementType));           
        }

        private void Duplicate()
        {
            Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", FieldHelper.GetFieldPath(Template.Parent), Logger.CommandEvent.CommandType.none, $"copyblockelements {FieldHelper.GetFieldPath(Template.Parent)} {CurrentIndex} 1");
            Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", FieldHelper.GetFieldPath(Template.Parent), Logger.CommandEvent.CommandType.none, $"pasteblockelements {FieldHelper.GetFieldPath(Template.Parent)}");
            Block.Add(Block[CurrentIndex].DeepCloneV2());
        }

        private void CopyBlock()
        {
            Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", FieldHelper.GetFieldPath(Template.Parent), Logger.CommandEvent.CommandType.none, $"copyblockelements {FieldHelper.GetFieldPath(Template.Parent)} {CurrentIndex} 1");
            CopiedBlocks.Clear();
            CopiedBlocks.Add(Block[CurrentIndex].DeepCloneV2());
            CopiedBlockType = ElementType;
            CanPaste = true;
            PasteBlocksAtEndCommand.RaiseCanExecuteChanged();
        }

        private void CopyRange()
        {
            Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", FieldHelper.GetFieldPath(Template.Parent), Logger.CommandEvent.CommandType.none, $"copyblockelements {FieldHelper.GetFieldPath(Template.Parent)} {CurrentIndex} *");
            int start = CurrentIndex;
            int end = Block.Count;
            int range = end - start;

            // todo: dialog box

            CopiedBlocks.Clear();

            for (int i = 0; i < range; i++)
            {
                if (start + i >= Block.Count)
                    break;

                CopiedBlocks.Add(Block[start + i].DeepCloneV2());
            }

            CopiedBlockType = ElementType;
            CanPaste = true;
            PasteBlocksAtEndCommand.RaiseCanExecuteChanged();
        }

        private void PasteBlocks()
        {
            if (CopiedBlocks == null || CopiedBlocks.Count == 0)
                return;

            Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", FieldHelper.GetFieldPath(Template.Parent), Logger.CommandEvent.CommandType.none, $"pasteblockelements {FieldHelper.GetFieldPath(Template.Parent)}");
            var newCurrentIndex = Block.Count;
            for (int i = 0; i < CopiedBlocks.Count; i++)
                Block.Add(CopiedBlocks[i]);

            //to-do: implement pasting at desired index.
            //insert changes collection during loop.
            //var tempblocks = new ObservableNonGenericCollection();
            //if (CurrentIndex < Block.Count - 1)
            //{ 
            //}

            CurrentIndex = newCurrentIndex;            
        }

        private void Shift(int direction)
        {
            Logger.LogCommand($"{Logger.ActiveTag.Name}.{Logger.ActiveTag.Group}", FieldHelper.GetFieldPath(Template.Parent), Logger.CommandEvent.CommandType.none, $"swapblockelements {FieldHelper.GetFieldPath(Template.Parent)} {CurrentIndex} {CurrentIndex + direction}");
            Block.Move(CurrentIndex, (CurrentIndex + direction));        
        }

        private void ExpandChildren()
        {
            FieldExpander.Expand(this, FieldExpander.ExpandTarget.All, FieldExpander.ExpandMode.Expand);
        }

        private void CollapseChildren()
        {
            FieldExpander.Expand(this, FieldExpander.ExpandTarget.All, FieldExpander.ExpandMode.Collapse);
        }

        private void GotoIndex()
        {
            var window = new GotoWindow();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if(window.ShowDialog() == true)
            {
                if(!int.TryParse(window.txtInput.Text, out int index) || (index < 0 || index >= Block.Count))
                {
                    SystemSounds.Hand.Play();
                    return;
                }

                CurrentIndex = index;
            }
        }

        protected override void OnPopulateContextMenu(EMenu menu)
        {
            menu.Group("TagBlock10")
                .Add("Go To Index", tooltip: "", command: GotoIndexCommand);
            menu.Group("TagBlock1")
                .Add("Add", tooltip: "Add a new element", command: AddCommand)
                .Add("Insert", tooltip: "Insert a new element at the current index", command: InsertCommand)
                .Add("Delete", tooltip: "Delete the element at the current index", command: DeleteCommand)
                .Add("Duplicate", tooltip: "Duplicate the element at the current index", command: DuplicateCommand);
            menu.Group("TagBlock2")
                .Add("Shift Up", tooltip: "Shift the current element up one", command: ShiftUpCommand)
                .Add("Shift Down", tooltip: "Shift the current element down one", command: ShiftDownCommand)
                .Add("Delete All", tooltip: "Delete all elements", command: DeleteAllCommand);
            menu.Group("TagBlock3")
                .Add("Copy Block", tooltip: "Copies a single block", command: CopyBlockCommand)
                .Add("Copy Range...", tooltip: "Copies the entire set of blocks (for now)", command: CopyRangeCommand)
                .Add("Paste Blocks At End", tooltip: "Append copied blocks to the end of the list", command: PasteBlocksAtEndCommand);
            menu.Group("TagBlock4")
                .Add("Collapse All", tooltip: "Collapse all children", command: CollapseAllCommand)
                .Add("Expand All", tooltip: "Expand all children", command: ExpandAllCommand);
        }
        public override void Dispose()
        {
            base.Dispose();
            Block.BaseCollection.Clear();
            Block = null;
            Template.Dispose();
        }
    }
}
