using EpsilonLib.Editors;
using EpsilonLib.Utils;
using Shared;
using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CryingCatPlugin
{
    class CryingCatEditorViewModel : Screen, IEditor
    {
        private BitmapSource _imageSource;
        private float _angle;
        private bool _isSpiraling;
        private IDisposable _spiralTimer;

        public BitmapSource ImageSource
        {
            get => _imageSource;
            set => SetAndNotify(ref _imageSource, value);
        }
        public float Angle
        {
            get => _angle;
            set => SetAndNotify(ref _angle, value);
        }

        public bool IsSpiraling
        {
            get => _isSpiraling;
            set => SetAndNotify(ref _isSpiraling, value);
        }

        public CryingCatEditorViewModel(string filename, BitmapImage image)
        {
            DisplayName = filename;
            ImageSource = image;
        }

        public void Greyscale()
        {
            FormatConvertedBitmap newSource = new FormatConvertedBitmap();
            newSource.BeginInit();
            newSource.Source = ImageSource;
            newSource.DestinationFormat = PixelFormats.Gray32Float;
            newSource.EndInit();
            ImageSource = newSource;
        }

        public void Spiral()
        {
            if(IsSpiraling)
            {
                _spiralTimer.Dispose();
                IsSpiraling = false;
            }
            else
            {
                _spiralTimer = DispatcherEx.CreateTimer(TimeSpan.FromSeconds(1 / 60.0f), () =>
                {
                    Angle += 0.5f;
                });
                IsSpiraling = true;
            }
            
        }

        public void Rotate()
        {
            Angle += 90;
        }
    }

    [Export(typeof(IEditorProvider))]
    class CryingCatEditorProvider : IEditorProvider
    {
        public string DisplayName => "Cat Images";

        public Guid Id { get; } = new Guid("{024A8AD4-A8ED-431D-A23F-28086413B679}");

        public IReadOnlyList<string> FileExtensions => new[] { ".jpg", ".png" };

        public Task OpenFileAsync(IShell shell, string fileName)
        {
            var bitmap = new BitmapImage(new Uri(fileName));
            var doc = new CryingCatEditorViewModel(Path.GetFileName(fileName), bitmap);
            shell.ActiveDocument = doc;

            return Task.CompletedTask;
        }
    }
}
