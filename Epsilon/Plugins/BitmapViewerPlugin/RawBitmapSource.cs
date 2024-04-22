using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BitmapViewerPlugin
{
    public unsafe class RawBitmapSource : BitmapSource
    {
        private byte[] _buffer;
        private int _pixelWidth;
        private int _pixelHeight;
        public bool channelR = true;
        public bool channelG = true;
        public bool channelB = true;
        public bool channelA = true;
        public bool linkColorChannels;


        public RawBitmapSource(byte[] buffer, int pixelWidth)
        {
            this._buffer = buffer;
            this._pixelWidth = pixelWidth;
            this._pixelHeight = buffer.Length / (4 * pixelWidth);
        }

        unsafe public override void CopyPixels(Int32Rect sourceRect, Array pixels, int stride, int offset)
        {
            fixed (byte* source = _buffer, destination = (byte[])pixels)
            {
                byte* dstPtr = destination + offset;

                for (int y = sourceRect.Y; y < sourceRect.Y + sourceRect.Height; y++)
                {
                    for (int x = sourceRect.X; x < sourceRect.X + sourceRect.Width; x++)
                    {
                        byte* srcPtr = source + stride * y + 4 * x;

                        //if (linkColorChannels)
                        //{
                        //    var singlecolor = (byte)(channelB ? (*(srcPtr + 0)) : 0);
                        //
                        //    *(dstPtr+1) = *(dstPtr+2) = *(dstPtr + 3) = singlecolor;
                        //    *(dstPtr+4) = (byte)(channelA ? (*(srcPtr + 3)) : 255);
                        //    dstPtr += 4;
                        //    continue;
                        //}

                        *(dstPtr++) = (byte)(channelB ? (*(srcPtr + 0)) : 0);
                        *(dstPtr++) = (byte)(channelG ? (*(srcPtr + 1)) : 0);
                        *(dstPtr++) = (byte)(channelR ? (*(srcPtr + 2)) : 0);
                        //*(dstPtr++) = 255; // ignore alpha for now until the decoders are fixed.
                        *(dstPtr++) = (byte)(channelA ? (*(srcPtr + 3)) : 255);
                    }
                }
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new RawBitmapSource(_buffer, _pixelWidth);
        }

        public override event EventHandler<DownloadProgressEventArgs> DownloadProgress;
        public override event EventHandler DownloadCompleted;
        public override event EventHandler<ExceptionEventArgs> DownloadFailed;
        public override event EventHandler<ExceptionEventArgs> DecodeFailed;

        public override double DpiX
        {
            get { return 96; }
        }

        public override double DpiY
        {
            get { return 96; }
        }

        public override System.Windows.Media.PixelFormat Format
        {
            get { return PixelFormats.Bgra32; }
        }

        public override int PixelWidth
        {
            get { return _pixelWidth; }
        }

        public override int PixelHeight
        {
            get { return _pixelHeight; }
        }

        public override double Width
        {
            get { return _pixelWidth; }
        }

        public override double Height
        {
            get { return _pixelHeight; }
        }
    }
}
