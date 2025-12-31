using System;
using System.Drawing;
using System.Drawing.Imaging;
using Accord.Imaging;
using Accord.Imaging.Filters;

namespace MediaSymbolRecognizer
{
    public class ImageProcessor
    {
        public Bitmap ProcessCameraImage(Bitmap input)
        {
            try
            {
                Bitmap gray = Grayscale(input);

                Bitmap binary = Binarize(gray, GetAdaptiveThreshold(gray));

                Rectangle boundingBox = FindBoundingBox(binary);

                Bitmap cropped;
                if (boundingBox.Width > 10 && boundingBox.Height > 10)
                {
                    cropped = CropImage(binary, boundingBox);
                }
                else
                {
                    cropped = binary;
                }

                Bitmap resized = ResizeImage(cropped, 100, 100);

                return resized;
            }
            catch (Exception)
            {
                return new Bitmap(100, 100);
            }
        }

        public Bitmap Grayscale(Bitmap input)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);

            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    Color color = input.GetPixel(x, y);
                    int gray = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
                    output.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }

            return output;
        }

        public Bitmap Binarize(Bitmap input, int threshold)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);

            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    Color color = input.GetPixel(x, y);
                    int value = (color.R < threshold) ? 0 : 255;
                    output.SetPixel(x, y, Color.FromArgb(value, value, value));
                }
            }

            return output;
        }

        public int GetAdaptiveThreshold(Bitmap gray)
        {
            long sum = 0;
            int count = 0;

            for (int y = 0; y < gray.Height; y += 2)
            {
                for (int x = 0; x < gray.Width; x += 2)
                {
                    sum += gray.GetPixel(x, y).R;
                    count++;
                }
            }

            int average = (int)(sum / Math.Max(1, count));
            return Math.Max(100, Math.Min(200, average - 30));
        }

        public Rectangle FindBoundingBox(Bitmap binary)
        {
            int minX = binary.Width;
            int minY = binary.Height;
            int maxX = 0;
            int maxY = 0;
            bool found = false;

            for (int y = 0; y < binary.Height; y++)
            {
                for (int x = 0; x < binary.Width; x++)
                {
                    Color pixel = binary.GetPixel(x, y);
                    if (pixel.R < 128) 
                    {
                        found = true;
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }
            }

            if (!found)
                return new Rectangle(0, 0, binary.Width, binary.Height);

            int padding = 10;
            minX = Math.Max(0, minX - padding);
            minY = Math.Max(0, minY - padding);
            maxX = Math.Min(binary.Width - 1, maxX + padding);
            maxY = Math.Min(binary.Height - 1, maxY + padding);

            int width = maxX - minX + 1;
            int height = maxY - minY + 1;

            if (width <= 0 || height <= 0)
                return new Rectangle(0, 0, binary.Width, binary.Height);

            return new Rectangle(minX, minY, width, height);
        }

        public Bitmap CropImage(Bitmap input, Rectangle rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0 ||
                rect.X < 0 || rect.Y < 0 ||
                rect.X + rect.Width > input.Width ||
                rect.Y + rect.Height > input.Height)
            {
                return new Bitmap(1, 1);
            }

            Bitmap output = new Bitmap(rect.Width, rect.Height);

            using (Graphics g = Graphics.FromImage(output))
            {
                g.DrawImage(input, 0, 0, rect, GraphicsUnit.Pixel);
            }

            return output;
        }

        public Bitmap ResizeImage(Bitmap input, int width, int height)
        {
            Bitmap output = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(output))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(input, 0, 0, width, height);
            }

            return output;
        }

        public Bitmap ResizeForNetwork(Bitmap input)
        {
            return ResizeImage(input, 20, 20);
        }
    }
}