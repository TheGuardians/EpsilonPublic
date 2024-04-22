using Microsoft.Win32;
using System.IO;
using TagStructEditor.Common;

namespace TagStructEditor.Fields
{
    public class DataField : ValueField, IExpandable
    {
        public DataField(ValueFieldInfo info) : base(info)
        {
            ImportCommand = new DelegateCommand(Import);
            ExportCommand = new DelegateCommand(Export, () => Length > 0);
        }

        public byte[] Data { get; set; }
        public int Length => Data != null ? Data.Length : 0;
        public bool IsExpanded { get; set; }
        public DelegateCommand ImportCommand { get; set; }
        public DelegateCommand ExportCommand { get; set; }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            Data = (byte[])value;
        }

        public virtual void OnDataChanged()
        {
            SetActualValue(Data);

            RaisePropertyChanged(nameof(Length));
            ImportCommand.RaiseCanExecuteChanged();
            ExportCommand.RaiseCanExecuteChanged();        
        }

        private void Export()
        {
            var dlg = new SaveFileDialog()
            {
                Title = "Export Data",
                DefaultExt = ".bin",
                FileName = $"{Name}.bin",
            };

            if (dlg.ShowDialog() == true)
                File.WriteAllBytes(dlg.FileName, Data);
        }

        private void Import()
        {
            var dlg = new OpenFileDialog()
            {
                Title = "Import Data",
                DefaultExt = ".bin",
                FileName = $"{Name}.bin",
            };

            if (dlg.ShowDialog() == true)
                Data = File.ReadAllBytes(dlg.FileName);
        }
    }
}
