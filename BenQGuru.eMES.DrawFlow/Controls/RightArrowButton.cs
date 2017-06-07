/***********************************************************************
 * Module:  RightArrowButton.cs
 * Author:  Administrator
 * Purpose: Definition of the Class Flow.Controls.RightArrowButton
 ***********************************************************************/

using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using BenQGuru.eMES.DrawFlow.Data;

namespace BenQGuru.eMES.DrawFlow.Controls
{
   /// <summary>
   /// </summary>
    public class RightArrowButton : FlowButton
    {
        /// <summary>
        /// </summary>
        public override void DrawButton()
        {
            //this.ArrowHead = DrawUtility.DrawOneArrow(this, 1, degree, this.ArrowTail);
        }

        public RightArrowButton()
        {
            this.BackColor = Color.Blue;
            this.degree = 0;
        }

        public RightArrowButton(Color backColor,double degree,Point arrowTail)
        {
            this.BackColor = backColor;
            this.degree = degree;
            this.ArrowTail = arrowTail;
        }

        /// <summary>
        /// ������ʾ��ͷ����ֵΪˮƽ����˳ʱ����ת�Ķ���
        /// </summary>
        private double degree;

        /// <summary>
        /// ��ͷͷ������
        /// </summary>
        public Point ArrowHead;

        /// <summary>
        /// ��ͷβ������
        /// </summary>
        public Point ArrowTail;
        /// <summary>
        /// </summary>
        protected override void CreateContextMenu()
        {
            if (ContextMenu == null)
                ContextMenu = new ContextMenu();

            MenuItem miProcess = new MenuItem();
            miProcess.Text = "��������";
            miProcess.Click += new EventHandler(miProcess_Click);
            ContextMenu.MenuItems.Add(miProcess);

            MenuItem miEnd = new MenuItem();
            miEnd.Text = "����������";
            miEnd.Click += new EventHandler(miEnd_Click);
            ContextMenu.MenuItems.Add(miEnd);

        }

        /// <summary>
        /// </summary>
        protected override void SetLinkArea(RectangleCollection linkAreas)
        {
            int x = this.Left + this.Width - 4;
            int y = this.Top;
            int width = this.Width;
            int height = this.Height;
            Rectangle rect = new Rectangle(x, y, width, height);
            linkAreas.Add(rect);
        }

        /// <summary>
        /// </summary>
        private void miProcess_Click(object sender, EventArgs e)
        {
            //����Ѿ��еĲ�Ҫ�����ˡ�
            if (ToProcesses.Count > 0)
            {
                MessageBox.Show("Ŀ������Ѿ�����");
            }
            else
            {
                //���ұ�����һ���µĽ���
                ProcessButton pb = new ProcessButton();
                pb.Left = this.Right;
                pb.Top = this.Top - 2 * DrawUtility.a;
                this.Parent.Controls.Add(pb);
                pb.InFlows.Add(this);
                this.ToProcesses.Add(pb);
            }

        }

        public override FunctionButton AddOutProcess(string processName)
        {
            if (ToProcesses.Count > 0)
            {
                return null;
            }
            else
            {
                //���ұ�����һ���µĽ���
                ProcessButton pb = new ProcessButton();
                pb.ProcessName = processName;
                pb.Left = this.Right - this.Width / 2;
                pb.Top = this.Top - 2 * DrawUtility.a;
                this.Parent.Controls.Add(pb);
                pb.InFlows.Add(this);
                this.ToProcesses.Add(pb);
                return pb;
            }

        }
        /// <summary>
        /// </summary>
        private void miEnd_Click(object sender, EventArgs e)
        {
            if (ToProcesses.Count > 0)
            {
                MessageBox.Show("Ŀ������Ѿ�����");
            }

            EndButton eb = new EndButton();
            eb.Left = this.Left + this.Width;
            eb.Top = this.Top - 2 * DrawUtility.a;
            this.Parent.Controls.Add(eb);
            eb.InFlows.Add(this);
            this.ToProcesses.Add(eb);
        }

    }
}