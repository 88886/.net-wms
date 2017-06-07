using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using BenQGuru.eMES.DrawFlow.Data;

namespace BenQGuru.eMES.DrawFlow.Controls
{
   /// <summary>
   /// </summary>
    public class EndButton : FunctionButton
    {
        /// <summary>
        /// </summary>
        public override void DrawButton()
        {
            DrawUtility.DrawEnd(this);
        }

        /// <summary>
        /// </summary>
        protected override void CreateContextMenu()
        {
            ProcessName = "����";
        }

        public EndButton()
        {
            this.BackColor = Color.Black;
            this.radius= 6 * DrawUtility.a;
            this.inArrowDegree = 0;
            this.inPoint = new Point();
        }

        public EndButton(Color backColor, double inAorrowDegree, Point inPoint)
        {
            this.BackColor = backColor;
            this.inArrowDegree = inAorrowDegree;
            this.inPoint = inPoint;
            this.radius = 6 * DrawUtility.a;
        }

        public EndButton(Color backColor, double inAorrowDegree, Point inPoint,int radius)
        {
            this.BackColor = backColor;
            this.inArrowDegree = inAorrowDegree;
            this.inPoint = inPoint;
            this.radius = radius;
        }

        private double inArrowDegree;

        /// <summary>
        /// ָ���Լ��ļ�ͷ����
        /// </summary>
        public double InArrowDegree
        {
            get { return inArrowDegree; }
            set { inArrowDegree = value; }
        }

        private Point inPoint;

        /// <summary>
        /// ָ���Լ��ļ�ͷ��ͷ������
        /// </summary>
        public Point InPoint
        {
            get { return inPoint; }
            set { inPoint = value; }
        }

        private int radius;

        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        } 
    }
}