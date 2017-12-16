using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Math;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        Graphics myGraphics;
        Bitmap bitmap;
        bool dragging = false;
        Point firstPoint;
        bool isline = true;

        public Form1()
        {
            InitializeComponent();
            var bounds = Screen.PrimaryScreen.Bounds;
            bitmap = new Bitmap(bounds.Width, bounds.Height);
            myGraphics = Graphics.FromImage(bitmap);
            this.pictureBox1.Image = bitmap;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            firstPoint = this.PointToClient(MousePosition);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            firstPoint = Point.Empty;
            this.bitmap = new Bitmap(this.pictureBox1.Image);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                var bitmap = this.bitmap.Clone() as Bitmap;
                myGraphics = Graphics.FromImage(bitmap);
                Point pp = this.PointToClient(Cursor.Position);

                if (isline)
                    myGraphics.DrawLine(Pens.Red, firstPoint, pp);
                else
                {
                    var p = Point.Subtract(firstPoint, new Size(pp));
                    double radious = Sqrt(Pow(p.X, 2) + Pow(p.Y, 2));
                    Point Base = Point.Subtract(firstPoint, new Size((int)radious, (int)radious));
                    Rectangle r = new Rectangle(Base, new Size(2 * (int)radious, 2 * (int)radious));
                    myGraphics.DrawEllipse(Pens.Red, r);
                }

                this.pictureBox1.Image = bitmap;
            }
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            if (this.Width <= this.MinimumSize.Width || this.Size == this.bitmap.Size)
                return;

            this.Controls.Remove(pictureBox1);
            pictureBox1 = new PictureBox();
            adjustPB();
            this.Controls.Add(this.pictureBox1);

            //var bounds = Screen.PrimaryScreen.Bounds;
            //var tmpBitmap = this.bitmap;
            //this.bitmap = new Bitmap(bounds.Width, bounds.Height);
            //Graphics.FromImage(this.bitmap).DrawImage(tmpBitmap, new PointF(0, 0));
            this.pictureBox1.Image = this.bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = dialog.FileName;
                this.bitmap.Save(fileName);
            }
        }

        void adjustPB()
        {
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = this.Size;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.pictureBox1.Resize += new System.EventHandler(this.pictureBox1_Resize);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isline = !isline;
        }
    }
}
