using CacheEditor;
using CacheEditor.TagEditing;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using static BitmapViewerPlugin.BitmapExtractionHelper;
using BitmapGen2 = TagTool.Tags.Definitions.Gen2.Bitmap;

namespace BitmapViewerPlugin
{
    public class BitmapViewerViewModel : TagEditorPluginBase
    {
        public enum LoadStates
        {
            Success,
            Failed,
            Loading
        }

        private BitmapExtractionHelper _bitmapExtractor;
        private BaseBitmap _cachedBaseBitmap;
        private int _bitmapIndex;
        private int _layerIndex;
        private int _mipmapLevel;
        private BitmapSource _displayBitmap;
        private ObservableCollection<string> _bitmaps;
        private ObservableCollection<string> _layers;
        private ObservableCollection<string> _mipmapLevels;
        private string _format;
        private string _dimensions;
        private string _resourceSize;
        private CancellationTokenSource _loadCancelTokenSource = new CancellationTokenSource();
        private LoadStates _loadingState;
        private string _errorMessage;
        private bool _channelA = true;
        private bool _channelR = true;
        private bool _channelG = true;
        private bool _channelB = true;
        private bool _linkColorChannels;

        public BitmapViewerViewModel(ICacheFile cacheFile, CachedTag instance, Bitmap definition)
        {
            _bitmapExtractor = new BitmapExtractionHelper(cacheFile, instance, definition);

            PopulateBitmapList(definition);
            LoadBitmapInBackground();
        }

        public BitmapViewerViewModel(ICacheFile cacheFile, CachedTag instance, BitmapGen2 definition)
        {
            _bitmapExtractor = new BitmapExtractionHelper(cacheFile, instance, definition);

            PopulateBitmapList(definition);
            LoadBitmapInBackground();
        }

        public string Format
        {
            get => _format;
            set => SetAndNotify(ref _format, value);
        }

        public string Dimensions
        {
            get => _dimensions;
            set => SetAndNotify(ref _dimensions, value);
        }

        public string ResourceSize
        {
            get => _resourceSize;
            set => SetAndNotify(ref _resourceSize, value);
        }

        public ObservableCollection<string> Bitmaps
        {
            get => _bitmaps;
            set => SetAndNotify(ref _bitmaps, value);
        }

        public ObservableCollection<string> Layers
        {
            get => _layers;
            set => SetAndNotify(ref _layers, value);
        }

        public ObservableCollection<string> MipLevels
        {
            get => _mipmapLevels;
            set => SetAndNotify(ref _mipmapLevels, value);
        }

        public int BitmapIndex
        {
            get => _bitmapIndex;
            set
            {
                if (SetAndNotify(ref _bitmapIndex, value))
                {
                    _cachedBaseBitmap = null;
                    _layerIndex = 0;
                    _mipmapLevel = 0;
                    LoadBitmapInBackground();

                    NotifyOfPropertyChange(nameof(LayerIndex));
                    NotifyOfPropertyChange(nameof(MipLevel));
                }
            }
        }

        public int LayerIndex
        {
            get => _layerIndex;
            set
            {
                if (SetAndNotify(ref _layerIndex, value))
                    LoadBitmapInBackground();
            }
        }

        public int MipLevel
        {
            get => _mipmapLevel;
            set
            {
                if (SetAndNotify(ref _mipmapLevel, value))
                    LoadBitmapInBackground();
            }
        }

        public bool ChannelA
        {
            get => _channelA;
            set
            {
                if (SetAndNotify(ref _channelA, value))
                    LoadBitmapInBackground();
            }
        }

        public bool ChannelR
        {
            get => _channelR;
            set
            {
                if (SetAndNotify(ref _channelR, value))
                    LoadBitmapInBackground();
            }
        }

        public bool ChannelG
        {
            get => _channelG;
            set
            {
                if (SetAndNotify(ref _channelG, value))
                    LoadBitmapInBackground();
            }
        }

        public bool ChannelB
        {
            get => _channelB;
            set
            {
                if (SetAndNotify(ref _channelB, value))
                    LoadBitmapInBackground();
            }
        }

        public BitmapSource DisplayBitmap
        {
            get => _displayBitmap;
            set => SetAndNotify(ref _displayBitmap, value);
        }

        public LoadStates LoadingState
        {
            get => _loadingState;
            set
            {
                if (SetAndNotify(ref _loadingState, value))
                    NotifyOfPropertyChange(nameof(IsLoaded));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetAndNotify(ref _errorMessage, value);
        }

        public bool IsLoaded => _loadingState != LoadStates.Loading;

        private async void LoadBitmapInBackground()
        {
            _loadCancelTokenSource.Cancel();
            _loadCancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = _loadCancelTokenSource.Token;

            try
            {
                LoadingState = LoadStates.Loading;

                ExtractedBitmap data = await Task.Run(() =>
                    _bitmapExtractor.GetBitmapData(_cachedBaseBitmap, BitmapIndex, LayerIndex, MipLevel));

                if (!cancelToken.IsCancellationRequested)
                {
                    OnBitmapLoaded(data);
                    LoadingState = LoadStates.Success;
                }
            }
            catch (Exception ex) when (!cancelToken.IsCancellationRequested)
            {
                ErrorMessage = ex.ToString();
                LoadingState = LoadStates.Failed;
            }
        }

        private void OnBitmapLoaded(ExtractedBitmap bitmap)
        {
            _cachedBaseBitmap = bitmap.BaseBitmap;

            DisplayBitmap = new RawBitmapSource(bitmap.MipData, bitmap.MipWidth);
            Format = $"{_cachedBaseBitmap.Format}".ToUpper();
            Dimensions = $"{bitmap.MipWidth}x{bitmap.MipHeight}";
            ResourceSize = FormatResourceSize(bitmap.BaseBitmap.Data.Length);

            ((RawBitmapSource)DisplayBitmap).channelR = _channelR;
            ((RawBitmapSource)DisplayBitmap).channelG = _channelG;
            ((RawBitmapSource)DisplayBitmap).channelB = _channelB;
            ((RawBitmapSource)DisplayBitmap).channelA = _channelA;


            int layerCount = _cachedBaseBitmap.Type == BitmapType.CubeMap ? 6 : _cachedBaseBitmap.Depth;
            int mipLevelCount = _cachedBaseBitmap.MipMapCount;

            Layers = new ObservableCollection<string>(Enumerable.Range(0, layerCount).Select((_, i) => $"Layer: {i}"));
            MipLevels = new ObservableCollection<string>(Enumerable.Range(0, mipLevelCount).Select((_, i) => $"Level: {i}"));
        }

        private string FormatResourceSize(float length)
        {
            string units;

            if (length < 1024) // arbitrary
                units = "bytes";
            else
            {
                length /= 1024;
                units = "KB";

                if (length > 1024)
                {
                    length /= 1024;
                    units = "MB";
                }
            }

            length = (float)Math.Round(length, 1);
            return $"{length} {units}";
        }

        private void PopulateBitmapList(Bitmap definition)
        {
            Bitmaps = new ObservableCollection<string>(Enumerable.Range(0, definition.Images.Count).Select((_, i) => $"Bitmap: {i}"));
        }

        private void PopulateBitmapList(BitmapGen2 definition)
        {
            Bitmaps = new ObservableCollection<string>(Enumerable.Range(0, definition.Bitmaps.Count).Select((_, i) => $"Bitmap: {i}"));
        }
    }
}
