using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OnlineShoppingManagementSystem
{
    [ToolboxItem(true)]
    public class CardPanel : Panel
    {
        private int _cornerRadius = 24;
        private Color _leftColor = Color.FromArgb(150, 120, 85, 220);
        private Color _rightColor = Color.FromArgb(200, 240, 190, 200);

        [Category("Appearance")]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("Appearance")]
        public Color LeftColor
        {
            get => _leftColor;
            set { _leftColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color RightColor
        {
            get => _rightColor;
            set { _rightColor = value; Invalidate(); }
        }

        public CardPanel()
        {
            SetStyle(ControlStyles.UserPaint
                 | ControlStyles.ResizeRedraw
                 | ControlStyles.OptimizedDoubleBuffer
                 | ControlStyles.AllPaintingInWmPaint, true);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var bounds = ClientRectangle;
            bounds.Inflate(-6, -6);
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            using (GraphicsPath path = CreateRoundedRectanglePath(bounds, CornerRadius))
            {
                using (LinearGradientBrush lg = new LinearGradientBrush(bounds, LeftColor, RightColor, 315f))
                {
                    g.FillPath(lg, path);
                }

                Point[] triangle = new Point[]
                {
                    new Point(bounds.Left + bounds.Width/2, bounds.Top + bounds.Height),
                    new Point(bounds.Right, bounds.Top + bounds.Height),
                    new Point(bounds.Right, bounds.Top + bounds.Height/3)
                };
                using (SolidBrush cream = new SolidBrush(Color.FromArgb(240, 240, 235)))
                {
                    Region oldClip = g.Clip;
                    g.SetClip(path);
                    g.FillPolygon(cream, triangle);
                    g.Clip = oldClip;
                }

                using (Pen border = new Pen(Color.FromArgb(100, Color.White), 1f))
                {
                    g.DrawPath(border, path);
                }
            }
        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            if (d > rect.Width) d = rect.Width;
            if (d > rect.Height) d = rect.Height;
            path.StartFigure();
            path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
