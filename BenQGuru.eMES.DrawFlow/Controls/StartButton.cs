/***********************************************************************
 * Module:  StartButton.cs
 * Author:  Administrator
 * Purpose: Definition of the Class Flow.Controls.StartButton
 ***********************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using BenQGuru.eMES.DrawFlow.Data;

namespace BenQGuru.eMES.DrawFlow.Controls
{
    /// <summary>
    /// </summary>
    public class StartButton : FunctionButton
    {
        /// <summary>
        /// </summary>
        public override void DrawButton()
        {
            DrawUtility.DrawStart(this);
        }

        public StartButton()
        {
            this.BackColor = Color.Green;
            Radius = 6 * DrawUtility.a;
        }

        public StartButton(Color backColor, int radius)
        {
            this.BackColor = backColor;
            this.radius = radius;
        }

        /// <summary>
        /// </summary>
        protected override void CreateContextMenu()
        {
            ProcessName = "��ʼ";
            ContextMenu cm = new ContextMenu();

            MenuItem miUp = new MenuItem();
            miUp.Text = "�������ϼ�ͷ";
            miUp.Click += new EventHandler(miUp_Click);
            cm.MenuItems.Add(miUp);

            MenuItem miDown = new MenuItem();
            miDown.Text = "�������¼�ͷ";
            miDown.Click += new EventHandler(miDown_Click);
            cm.MenuItems.Add(miDown);

            MenuItem miLeft = new MenuItem();
            miLeft.Text = "���������ͷ";
            miLeft.Click += new EventHandler(miLeft_Click);
            cm.MenuItems.Add(miLeft);

            MenuItem miRight = new MenuItem();
            miRight.Text = "�������Ҽ�ͷ";
            miRight.Click += new EventHandler(miRight_Click);
            cm.MenuItems.Add(miRight);

            MenuItem miRandom = new MenuItem();
            miRandom.Text = "��������Ƕȼ�ͷ";
            miRandom.Click += new EventHandler(miRandom_Click);
            cm.MenuItems.Add(miRandom);

            this.ContextMenu = cm;

        }

        protected void miUp_Click(object sender, EventArgs e)
        {
            AddOutArrow(-90);
        }

        protected void miDown_Click(object sender, EventArgs e)
        {
            AddOutArrow(90);
        }

        protected void miRight_Click(object sender, EventArgs e)
        {
            AddOutArrow(0);
        }

        protected void miLeft_Click(object sender, EventArgs e)
        {
            AddOutArrow(180);
        }

        protected void miRandom_Click(object sender, EventArgs e)
        {
            AddOutArrow((new Random()).Next(0, 360));
        }

        public override FlowButton AddOutArrow(double degree)
        {
            StraightArrowButton rab = new StraightArrowButton(SystemColors.InactiveCaption, degree, this);
            this.Parent.Controls.Add(rab);
            rab.DrawButton();
            OutFlows.Add(rab);
            rab.FromProcesses.Add(this);
            rab.SetLinkControls();
            return rab;
        }

        private int radius;

        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }


        /// <summary>
        /// ��ȡԲ����㰴ť�ͳ�����ͷ�ĽӴ��㣬��Ϊ��ͷ��ʵ��β������
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public override Point GetNextArrowTail(double degree)
        {
            Point arrowPoint = DrawUtility.GetCyclePoint(100, 0, degree);
            int x = (this.Left + this.Right) / 2 + arrowPoint.X * Radius / 100;
            int y = (this.Top + this.Bottom) / 2 + arrowPoint.Y * Radius / 100;
            return new Point(x, y);
        }

        public void setProcessName(string value)
        {
            if (value != null)
            {
                ProcessName = value;
            }
        }
    }
}