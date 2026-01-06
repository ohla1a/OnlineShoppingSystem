using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OnlineShoppingManagementSystem
{
    [ToolboxItem(true)]
    public class RoundedButton : Button
    {
        private int _cornerRadius = 18;
        private Color _baseColor;
        private bool _isHovered;
        private float _hoverLightenFactor = 0.20f; // default to 20% lighter

        [Category("Appearance")]
        [Description("Corner radius for the button.")]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Amount to lighten the button on hover (0.0 - 1.0). Default is 0.20 (20%).")]
        public float HoverLightenFactor
        {
            get => _hoverLightenFactor;
            set
            {
                // clamp to [0.0f, 1.0f]
                _hoverLightenFactor = Math.Max(0f, Math.Min(1f, value));
            }
        }

        public RoundedButton()
        {
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            // keep base color updated unless we are currently hovered
            if (!_isHovered)
                _baseColor = BackColor;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            var rect = ClientRectangle;
            rect.Inflate(-1, -1);

            using (GraphicsPath path = CreateRoundedRectanglePath(rect, CornerRadius))
            using (SolidBrush brush = new SolidBrush(BackColor))
            using (Pen pen = new Pen(Color.FromArgb(120, Color.Black), 1f))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }

            // draw image if any (left)
            int imgOffset = 8;
            if (Image != null)
            {
                var imgRect = new Rectangle(imgOffset, (Height - Image.Height) / 2, Image.Width, Image.Height);
                g.DrawImage(Image, imgRect);
                imgOffset += Image.Width + 6;
            }

            // draw centered text
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(g, Text, Font,
                new Rectangle(0, 0, Width, Height),
                ForeColor, flags);

            if (Focused && ShowFocusCues)
            {
                ControlPaint.DrawFocusRectangle(g, ClientRectangle);
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

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            // use configurable lighten factor (default 0.20 = 20% lighter)
            BackColor = ControlPaint.Light(_baseColor, _hoverLightenFactor);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            BackColor = _baseColor;
            Invalidate();
        }
    }
}
