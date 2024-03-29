﻿// ImageListView - A listview control for image files
// Copyright (C) 2009 Ozgur Ozcitak
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Ozgur Ozcitak (ozcitak@yahoo.com)

using System;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Text;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Contains utility functions.
    /// </summary>
    public static class Utility
    {
        #region Text Utilities
        /// <summary>
        /// Formats the given file size as a human readable string.
        /// </summary>
        /// <param name="size">File size in bytes.</param>
        public static string FormatSize(long size)
        {
            double mod = 1024;
            double sized = size;

            // string[] units = new string[] { "B", "KiB", "MiB", "GiB", "TiB", "PiB" };
            string[] units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            int i;
            for (i = 0; sized > mod; i++)
            {
                sized /= mod;
            }

            return string.Format("{0} {1}", Math.Round(sized, 2), units[i]);
        }
        #endregion

        #region Graphics Utilities
        /// <summary>
        /// Checks the stream header if it matches with
        /// any of the supported image file types.
        /// </summary>
        /// <param name="stream">An open stream pointing to an image file.</param>
        /// <returns>true if the stream is an image file (BMP, TIFF, PNG, GIF, JPEG, WMF, EMF, ICO, CUR);
        /// false otherwise.</returns>
        internal static bool IsImage(Stream stream)
        {
            // Sniff some bytes from the start of the stream
            // and check against magic numbers of supported 
            // image file formats
            byte[] header = new byte[8];
            stream.Seek(0, SeekOrigin.Begin);
            if (stream.Read(header, 0, header.Length) != header.Length)
                return false;

            // BMP
            string bmpHeader = Encoding.ASCII.GetString(header, 0, 2);
            if (bmpHeader == "BM") // BM - Windows bitmap
                return true;
            else if (bmpHeader == "BA") // BA - Bitmap array
                return true;
            else if (bmpHeader == "CI") // CI - Color Icon
                return true;
            else if (bmpHeader == "CP") // CP - Color Pointer
                return true;
            else if (bmpHeader == "IC") // IC - Icon
                return true;
            else if (bmpHeader == "PT") // PI - Pointer
                return true;

            // TIFF
            string tiffHeader = Encoding.ASCII.GetString(header, 0, 4);
            if (tiffHeader == "MM\x00\x2a") // Big-endian
                return true;
            else if (tiffHeader == "II\x2a\x00") // Little-endian
                return true;

            // PNG
            if (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47 &&
                header[4] == 0x0D && header[5] == 0x0A && header[6] == 0x1A && header[7] == 0x0A)
                return true;

            // GIF
            string gifHeader = Encoding.ASCII.GetString(header, 0, 4);
            if (gifHeader == "GIF8")
                return true;

            // JPEG
            if (header[0] == 0xFF && header[1] == 0xD8)
                return true;

            // WMF
            if (header[0] == 0xD7 && header[1] == 0xCD && header[2] == 0xC6 && header[3] == 0x9A)
                return true;

            // EMF
            if (header[0] == 0x01 && header[1] == 0x00 && header[2] == 0x00 && header[3] == 0x00)
                return true;

            // Windows Icons
            if (header[0] == 0x00 && header[1] == 0x00 && header[2] == 0x01 && header[3] == 0x00) // ICO
                return true;
            else if (header[0] == 0x00 && header[1] == 0x00 && header[2] == 0x02 && header[3] == 0x00) // CUR
                return true;

            return false;
        }
        /// <summary>
        /// Draws the given caption and text inside the given rectangle.
        /// </summary>
        internal static int DrawStringPair(Graphics g, Rectangle r, string caption, string text, Font font, Brush captionBrush, Brush textBrush)
        {
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags = StringFormatFlags.NoWrap;

                SizeF szc = g.MeasureString(caption, font, r.Size, sf);
                int y = (int)szc.Height;
                if (szc.Width > r.Width) szc.Width = r.Width;
                Rectangle txrect = new Rectangle(r.Location, Size.Ceiling(szc));
                g.DrawString(caption, font, captionBrush, txrect, sf);
                txrect.X += txrect.Width;
                txrect.Width = r.Width;
                if (txrect.X < r.Right)
                {
                    SizeF szt = g.MeasureString(text, font, r.Size, sf);
                    y = Math.Max(y, (int)szt.Height);
                    txrect = Rectangle.Intersect(r, txrect);
                    g.DrawString(text, font, textBrush, txrect, sf);
                }

                return y;
            }
        }
        /// <summary>
        /// Gets the scaled size of an image required to fit
        /// in to the given size keeping the image aspect ratio.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <param name="fit">The size to fit in to.</param>
        /// <returns>New image size.</returns>
        internal static Size GetSizedImageBounds(Image image, Size fit)
        {
            float f = System.Math.Max((float)image.Width / (float)fit.Width, (float)image.Height / (float)fit.Height);
            if (f < 1.0f) f = 1.0f; // Do not upsize small images
            int width = (int)System.Math.Round((float)image.Width / f);
            int height = (int)System.Math.Round((float)image.Height / f);
            return new Size(width, height);
        }
        /// <summary>
        /// Gets the bounding rectangle of an image required to fit
        /// in to the given rectangle keeping the image aspect ratio.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <param name="fit">The rectangle to fit in to.</param>
        /// <param name="hAlign">Horizontal image aligment in percent.</param>
        /// <param name="vAlign">Vertical image aligment in percent.</param>
        /// <returns>New image size.</returns>
        public static Rectangle GetSizedImageBounds(Image image, Rectangle fit, float hAlign, float vAlign)
        {
            if (hAlign < 0 || hAlign > 100.0f)
                throw new ArgumentException("hAlign must be between 0.0 and 100.0 (inclusive).", "hAlign");
            if (vAlign < 0 || vAlign > 100.0f)
                throw new ArgumentException("vAlign must be between 0.0 and 100.0 (inclusive).", "vAlign");
            Size scaled = GetSizedImageBounds(image, fit.Size);
            int x = fit.Left + (int)(hAlign / 100.0f * (float)(fit.Width - scaled.Width));
            int y = fit.Top + (int)(vAlign / 100.0f * (float)(fit.Height - scaled.Height));

            return new Rectangle(x, y, scaled.Width, scaled.Height);
        }
        /// <summary>
        /// Gets the bounding rectangle of an image required to fit
        /// in to the given rectangle keeping the image aspect ratio.
        /// The image will be centered in the fit box.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <param name="fit">The rectangle to fit in to.</param>
        /// <returns>New image size.</returns>
        public static Rectangle GetSizedImageBounds(Image image, Rectangle fit)
        {
            return GetSizedImageBounds(image, fit, 50.0f, 50.0f);
        }
        /// <summary>
        /// Gets a path representing a rounded rectangle.
        /// </summary>
        private static GraphicsPath GetRoundedRectanglePath(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(x + radius, y, x + width - radius, y);
            if (radius > 0)
                path.AddArc(x + width - 2 * radius, y, 2 * radius, 2 * radius, 270.0f, 90.0f);
            path.AddLine(x + width, y + radius, x + width, y + height - radius);
            if (radius > 0)
                path.AddArc(x + width - 2 * radius, y + height - 2 * radius, 2 * radius, 2 * radius, 0.0f, 90.0f);
            path.AddLine(x + width - radius, y + height, x + radius, y + height);
            if (radius > 0)
                path.AddArc(x, y + height - 2 * radius, 2 * radius, 2 * radius, 90.0f, 90.0f);
            path.AddLine(x, y + height - radius, x, y + radius);
            if (radius > 0)
                path.AddArc(x, y, 2 * radius, 2 * radius, 180.0f, 90.0f);
            return path;
        }
        /// <summary>
        /// Fills the interior of a rounded rectangle.
        /// </summary>
        public static void FillRoundedRectangle(System.Drawing.Graphics graphics, Brush brush, int x, int y, int width, int height, int radius)
        {
            using (GraphicsPath path = GetRoundedRectanglePath(x, y, width, height, radius))
            {
                graphics.FillPath(brush, path);
            }
        }
        /// <summary>
        /// Fills the interior of a rounded rectangle.
        /// </summary>
        public static void FillRoundedRectangle(System.Drawing.Graphics graphics, Brush brush, float x, float y, float width, float height, float radius)
        {
            FillRoundedRectangle(graphics, brush, (int)x, (int)y, (int)width, (int)height, (int)radius);
        }
        /// <summary>
        /// Fills the interior of a rounded rectangle.
        /// </summary>
        public static void FillRoundedRectangle(System.Drawing.Graphics graphics, Brush brush, Rectangle rect, int radius)
        {
            FillRoundedRectangle(graphics, brush, rect.Left, rect.Top, rect.Width, rect.Height, radius);
        }
        /// <summary>
        /// Fills the interior of a rounded rectangle.
        /// </summary>
        public static void FillRoundedRectangle(System.Drawing.Graphics graphics, Brush brush, RectangleF rect, float radius)
        {
            FillRoundedRectangle(graphics, brush, (int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height, (int)radius);
        }
        /// <summary>
        /// Draws the outline of a rounded rectangle.
        /// </summary>
        public static void DrawRoundedRectangle(System.Drawing.Graphics graphics, Pen pen, int x, int y, int width, int height, int radius)
        {
            using (GraphicsPath path = GetRoundedRectanglePath(x, y, width, height, radius))
            {
                graphics.DrawPath(pen, path);
            }
        }
        /// <summary>
        /// Draws the outline of a rounded rectangle.
        /// </summary>
        public static void DrawRoundedRectangle(System.Drawing.Graphics graphics, Pen pen, float x, float y, float width, float height, float radius)
        {
            DrawRoundedRectangle(graphics, pen, (int)x, (int)y, (int)width, (int)height, (int)radius);
        }
        /// <summary>
        /// Draws the outline of a rounded rectangle.
        /// </summary>
        public static void DrawRoundedRectangle(System.Drawing.Graphics graphics, Pen pen, Rectangle rect, int radius)
        {
            DrawRoundedRectangle(graphics, pen, rect.Left, rect.Top, rect.Width, rect.Height, radius);
        }
        /// <summary>
        /// Draws the outline of a rounded rectangle.
        /// </summary>
        public static void DrawRoundedRectangle(System.Drawing.Graphics graphics, Pen pen, RectangleF rect, float radius)
        {
            DrawRoundedRectangle(graphics, pen, (int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height, (int)radius);
        }
        #endregion

        #region Tuples
        /// <summary>
        /// Represents a factory class for creating tuples.
        /// </summary>
        public static class Tuple
        {
            /// <summary>
            /// Creates a new 1-tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the only component of the tuple.</typeparam>
            /// <param name="item1">The value of the only component of the tuple.</param>
            /// <returns>A 1-tuple whose value is (<paramref name="item1"/>).</returns>
            public static Tuple<T1> Create<T1>(T1 item1)
            {
                return new Tuple<T1>(item1);
            }
            /// <summary>
            /// Creates a new 2-tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
            /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
            /// <param name="item1">The value of the first component of the tuple.</param>
            /// <param name="item2">The value of the second component of the tuple.</param>
            /// <returns>A 2-tuple whose value is (<paramref name="item1"/>, <paramref name="item2"/>).</returns>
            public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
            {
                return new Tuple<T1, T2>(item1, item2);
            }
            /// <summary>
            /// Creates a new 3-tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
            /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
            /// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
            /// <param name="item1">The value of the first component of the tuple.</param>
            /// <param name="item2">The value of the second component of the tuple.</param>
            /// <param name="item3">The value of the third component of the tuple.</param>
            /// <returns>A 3-tuple whose value is (<paramref name="item1"/>, <paramref name="item2"/>, <paramref name="item3"/>).</returns>
            public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
            {
                return new Tuple<T1, T2, T3>(item1, item2, item3);
            }
        }
        /// <summary>
        /// Represents a tuple with one element.
        /// </summary>
        public class Tuple<T1> 
        {
            private T1 mItem1;

            /// <summary>
            /// Gets the value of the first component.
            /// </summary>
            public T1 Item1 { get { return mItem1; } }

            /// <summary>
            /// Initializes a new instance of the <see cref="Tuple&lt;T1&gt;"/> class.
            /// </summary>
            /// <param name="item1">The value of the first component of the tuple.</param>
            public Tuple(T1 item1)
            {
                mItem1 = item1;
            }
        }
        /// <summary>
        /// Represents a tuple with two elements.
        /// </summary>
        public class Tuple<T1, T2> : Tuple<T1>
        {
            private T2 mItem2;

            /// <summary>
            /// Gets the value of the second component.
            /// </summary>
            public T2 Item2 { get { return mItem2; } }

            /// <summary>
            /// Initializes a new instance of the <see cref="Tuple&lt;T1, T2&gt;"/> class.
            /// </summary>
            /// <param name="item1">The value of the first component of the tuple.</param>
            /// <param name="item2">The value of the second component of the tuple.</param>
            public Tuple(T1 item1, T2 item2)
                : base(item1)
            {
                mItem2 = item2;
            }
        }
        /// <summary>
        /// Represents a tuple with three elements.
        /// </summary>
        public class Tuple<T1, T2, T3> : Tuple<T1, T2>
        {
            private T3 mItem3;

            /// <summary>
            /// Gets the value of the third component.
            /// </summary>
            public T3 Item3 { get { return mItem3; } }

            /// <summary>
            /// Initializes a new instance of the <see cref="Tuple&lt;T1, T2, T3&gt;"/> class.
            /// </summary>
            /// <param name="item1">The value of the first component of the tuple.</param>
            /// <param name="item2">The value of the second component of the tuple.</param>
            /// <param name="item3">The value of the third component of the tuple.</param>
            public Tuple(T1 item1, T2 item2, T3 item3)
                : base(item1, item2)
            {
                mItem3 = item3;
            }
        }
        #endregion
    }
}
