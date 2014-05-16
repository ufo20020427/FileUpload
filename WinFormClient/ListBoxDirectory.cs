using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Model;

namespace WinFormClient
{
    public class ListBoxDirectory : ListBox 
    {
        private Color RowBackColorSel = Color.FromArgb(150, 200, 250);
        public ListBoxDirectory()
        {
            base.DrawMode = DrawMode.OwnerDrawFixed;
            base.ItemHeight = 20;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            LocalDirectory localDirectory = Items[e.Index] as LocalDirectory;    

            Brush myBrush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                myBrush = new SolidBrush(RowBackColorSel);
            }
            else
            {
                myBrush = new SolidBrush(Color.White);
            }

            e.Graphics.FillRectangle(myBrush, e.Bounds);
            e.DrawFocusRectangle();

            Image image = Image.FromFile("Images/LocalDirectory.ico");
            Graphics graphics = e.Graphics;
            Rectangle bounds = e.Bounds;           
            Rectangle imageRect = new Rectangle(bounds.X, bounds.Y, bounds.Height, bounds.Height);
            Rectangle textRect = new Rectangle(imageRect.Right, bounds.Y, bounds.Width - imageRect.Right, bounds.Height);

            if (image != null)
            {
                graphics.DrawImage(image, imageRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            }

            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(localDirectory.FileName, e.Font, new SolidBrush(e.ForeColor), textRect, stringFormat);

        }
    }
}
