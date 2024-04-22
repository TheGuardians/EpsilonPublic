using CacheEditor;
using System;
using TagTool.Bitmaps;
using TagTool.Bitmaps.Utils;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using BitmapDecoder = TagTool.Bitmaps.BitmapDecoder;
using BitmapGen2 = TagTool.Tags.Definitions.Gen2.Bitmap;

namespace BitmapViewerPlugin
{
    class BitmapExtractionHelper
    {
        private readonly ICacheFile _cacheFile;
        private readonly Bitmap _bitmapGroup;
        private readonly BitmapGen2 _bitmapGroupGen2;
        private readonly CachedTag _tag;

        public BitmapExtractionHelper(ICacheFile cacheFile, CachedTag instance, Bitmap definition)
        {
            _cacheFile = cacheFile;
            _tag = instance;
            _bitmapGroup = definition;
        }

        public BitmapExtractionHelper(ICacheFile cacheFile, CachedTag instance, BitmapGen2 definition)
        {
            _cacheFile = cacheFile;
            _tag = instance;
            _bitmapGroupGen2 = definition;
        }

        public ExtractedBitmap GetBitmapData(BaseBitmap baseBitmap, int bitmapIndex, int layerIndex, int mipLevel)
        {
            if (baseBitmap == null)
            {
                if(CacheVersionDetection.GetGeneration(_cacheFile.Cache.Version) == CacheGeneration.Second)
                {
                    baseBitmap = TagTool.Commands.Gen2.Bitmaps.BitmapConverterGen2.ExtractBitmap((GameCacheGen2)_cacheFile.Cache, _bitmapGroupGen2, bitmapIndex);
                }
                else
                    baseBitmap = BitmapExtractor.ExtractBitmap(_cacheFile.Cache, _bitmapGroup, bitmapIndex, _tag.Name, false);
            }
                

            byte[] mipData = GetMipmapData(baseBitmap, layerIndex, mipLevel, out int mipWidth, out int mipHeight);
            mipData = BitmapDecoder.DecodeBitmap(mipData, baseBitmap.Format, mipWidth, mipHeight);

            return new ExtractedBitmap()
            {
                BaseBitmap = baseBitmap,
                MipWidth = mipWidth,
                MipHeight = mipHeight,
                MipData = mipData,
            };
        }

        private static byte[] GetMipmapData(BaseBitmap bitmap, int layerIndex, int mipmapIndex, out int width, out int height)
        {
            var tempImage = new Bitmap.Image();
            tempImage.Width = (short)bitmap.Width;
            tempImage.Height = (short)bitmap.Height;
            tempImage.Depth = (sbyte)bitmap.Depth;
            tempImage.Type = bitmap.Type;
            tempImage.Format = bitmap.Format;
            tempImage.Flags = bitmap.Flags;

            width = BitmapUtilsPC.GetMipmapWidth(tempImage, mipmapIndex);
            height = BitmapUtilsPC.GetMipmapHeight(tempImage, mipmapIndex);

            int mipmapSize, mipmapOffset;
            if (bitmap.Type == BitmapType.Texture2D)
            {
                mipmapOffset = BitmapUtilsPC.GetTextureOffset(tempImage, mipmapIndex);
                mipmapSize = width * height * BitmapFormatUtils.GetBitsPerPixel(tempImage.Format) / 8;
            }
            else if (bitmap.Type == BitmapType.CubeMap)
            {
                mipmapOffset = BitmapUtilsPC.GetTextureCubemapOffset(tempImage, 0, 0, layerIndex, mipmapIndex);
                mipmapSize = width * height * BitmapFormatUtils.GetBitsPerPixel(tempImage.Format) / 8;
            }
            else if (bitmap.Type == BitmapType.Array)
            {
                mipmapOffset = BitmapUtilsPC.GetTextureArrayOffset(tempImage, 0, 0, layerIndex, mipmapIndex);
                mipmapSize = width * height * BitmapFormatUtils.GetBitsPerPixel(tempImage.Format) / 8;
            }
            else if (bitmap.Type == BitmapType.Texture3D)
            {
                mipmapOffset = BitmapUtilsPC.GetTexture3DOffset(tempImage, 0, 0, layerIndex, mipmapIndex);
                mipmapSize = width * height * BitmapFormatUtils.GetBitsPerPixel(tempImage.Format) / 8;
            }
            else
            {
                throw new NotSupportedException();
            }

            var mipmapData = new byte[mipmapSize];
            Array.Copy(bitmap.Data, mipmapOffset, mipmapData, 0, mipmapSize);
            return mipmapData;
        }

        public class ExtractedBitmap
        {
            public BaseBitmap BaseBitmap;
            public byte[] MipData;
            public int MipWidth;
            public int MipHeight;
        }
    }
}
