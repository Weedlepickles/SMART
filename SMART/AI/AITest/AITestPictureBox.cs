using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMART.AI.AITest
{
    class AITestPictureBox : PictureBox
    {
        public int AgentX, AgentY, TargetX, TargetY;
        Rectangle targetBox = new Rectangle();
        bool isDragging = false;
        Image cheese, mouse;
        public float Direction { get; set; }
        
        public AITestPictureBox()
            : base()
        {
            cheese = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\AI\\AITest\\cheese.png");
            mouse = Image.FromFile(System.Windows.Forms.Application.StartupPath + "\\AI\\AITest\\mouse.png");
        }

        public void InitializeState()
        {
            TargetX = this.Width / 4;
            TargetY = this.Height / 4;
            targetBox.Width = cheese.Width;
            targetBox.Height = cheese.Width;
            ResetAgentPos();
            Direction = 0f;
        }

        public void ResetAgentPos()
        {
            AgentX = this.Width / 2;
            AgentY = this.Height / 2;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            pe.Graphics.FillRectangle(Brushes.Wheat, 0, 0, Width, Height);

            // Draw target
            targetBox.Location = new Point(TargetX - (targetBox.Width / 2),TargetY - (targetBox.Height / 2));
            //pe.Graphics.FillRectangle(Brushes.Wheat, targetBox);
            pe.Graphics.DrawImage(cheese, targetBox);

            // Draw Agent
            //pe.Graphics.FillEllipse(Brushes.Tomato, AgentX-5, AgentY-5, 10, 10);
            Image tMouse = rotateImageUsi(new Bitmap(mouse, mouse.Width, mouse.Height), Direction);
            pe.Graphics.DrawImage(tMouse, AgentX - tMouse.Width / 2, AgentY - tMouse.Height / 2, tMouse.Width, tMouse.Height);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
 	         base.OnMouseDown(e);
             if (e.X >= targetBox.Left && e.X <= targetBox.Right &&
                 e.Y >= targetBox.Top && e.Y <= targetBox.Bottom)
             {
                 isDragging = true;
             }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isDragging = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isDragging)
            {
                if(e.X >= 0 && e.X <= this.Width &&
                    e.Y >= 0 && e.Y <= this.Height) {

                    TargetX = e.X;
                    TargetY = e.Y;
                    this.Invalidate();
                }
            }
        }

        private Bitmap rotateImageUsi(Bitmap bitmap, float angle)
        {
            Bitmap bm = new Bitmap(bitmap.Width, bitmap.Height);
            using (Graphics graphics = Graphics.FromImage(bm))
            {
                graphics.TranslateTransform((float)bitmap.Width / 2, (float)bitmap.Height / 2);
                graphics.RotateTransform(angle);
                graphics.TranslateTransform(-(float)bitmap.Width / 2, -(float)bitmap.Height / 2);
                graphics.DrawImage(bitmap, new Point(0, 0));
            }
            return bm;
        }
    }
}
