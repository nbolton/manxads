using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI.WebControls;
using System.Web;
using Image = System.Drawing.Image;
using System.Drawing.Drawing2D;

namespace ManxAds
{
    public class Imaging
    {
        public static void CropAndSave(
            Stream stream,
            string saveToPath,
            int targetWidth,
            int targetHeight)
        {
            Image source = Image.FromStream(stream);
            int sourceWidth = source.Width;
            int sourceHeight = source.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = -1;
            int destY = -1;

            float percent = 0;
            float percentWidth = 0;
            float percentHeight = 0;

            // Calculate relational width/height percentages.
            percentWidth = (float)targetWidth / (float)sourceWidth;
            percentHeight = (float)targetHeight / (float)sourceHeight;

            if (percentHeight < percentWidth)
            {
                // Move horizontally to top edge.
                percent = percentWidth;
                destY += (int)((targetHeight - (sourceHeight * percent)) / 2);
            }
            else
            {
                // Move vertically to left edge.
                percent = percentHeight;
                destX += (int)((targetWidth - (sourceWidth * percent)) / 2);
            }

            // Resize width and height proportionally.
            int destWidth = (int)(sourceWidth * percent);
            int destHeight = (int)(sourceHeight * percent);

            // Create a base to draw image onto.
            Bitmap result = new Bitmap(
                targetWidth, targetHeight,
                PixelFormat.Format32bppArgb);

            // Change resolution of base to match source.
            result.SetResolution(
                source.HorizontalResolution,
                source.VerticalResolution);

            // Get graphics handler from base image.
            Graphics graphics = Graphics.FromImage(result);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw source onto base using graphics handler.
            graphics.DrawImage(source,
                new Rectangle(destX, destY, destWidth + 2, destHeight + 2),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            if (File.Exists(saveToPath))
            {
                // Delete to force cache refresh.
                File.Delete(saveToPath);
            }

            result.Save(saveToPath);
            result.Dispose();
            graphics.Dispose();
        }

        public static void ShrinkAndSave(
            Stream stream,
            string saveToPath,
            int maximumWidth,
            int maximumHeight)
        {
            // Check whether the file is really a JPEG by opening it.
            Image.GetThumbnailImageAbort callBack =
                new Image.GetThumbnailImageAbort(ThumbnailCallback);

            Image source = Image.FromStream(stream);
            int targetWidth = 0;
            int targetHeight = 0;
            float scaleFactor;

            if (source.Width > source.Height)
            {
                // Shrink to fit height.
                scaleFactor = (float)source.Height / (float)maximumHeight;
                targetHeight = maximumHeight;
                targetWidth = (int)(source.Width / scaleFactor);

                if (targetWidth > maximumWidth)
                {
                    // Shrink to fit width.
                    scaleFactor = (float)source.Width / (float)maximumWidth;
                    targetWidth = maximumWidth;
                    targetHeight = (int)(source.Height / scaleFactor);
                }
            }
            else
            {
                // Shrink to fit width.
                scaleFactor = (float)source.Width / (float)maximumWidth;
                targetWidth = maximumWidth;
                targetHeight = (int)(source.Height / scaleFactor);

                if (targetHeight > maximumHeight)
                {
                    // Shrink to fit width.
                    scaleFactor = (float)source.Height / (float)maximumHeight;
                    targetHeight = maximumHeight;
                    targetWidth = (int)(source.Width / scaleFactor);
                }
            }

            /*Image thumbnail = source.GetThumbnailImage(
                thumbWidth, thumbHeight, callBack, IntPtr.Zero);*/

            // Create a base to draw image onto.
            Bitmap result = new Bitmap(
                targetWidth, targetHeight,
                PixelFormat.Format32bppArgb);

            // Change resolution of base to match source.
            result.SetResolution(
                source.HorizontalResolution,
                source.VerticalResolution);

            // Get graphics handler from base image.
            Graphics graphics = Graphics.FromImage(result);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            graphics.DrawImage(source,
                new Rectangle(0, 0, targetWidth, targetHeight),
                new Rectangle(0, 0, source.Width, source.Height),
                GraphicsUnit.Pixel);

            if (File.Exists(saveToPath))
            {
                // Delete to force cache refresh.
                File.Delete(saveToPath);
            }

            /*thumbnail.Save(saveToPath);
            thumbnail.Dispose();*/

            result.Save(saveToPath);
            result.Dispose();
            graphics.Dispose();
        }

        protected static bool ThumbnailCallback()
        {
            return true;
        }
    }
}