/***********************************************************************
 * Module:  ProcessButton.cs
 * Author:  Administrator
 * Purpose: Definition of the Class Flow.Controls.ProcessButton
 ***********************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using BenQGuru.eMES.DrawFlow.Data;

namespace BenQGuru.eMES.DrawFlow.Controls
{
   /// <summary>
   /// ���̰�ť
   /// </summary>
    public class ProcessButton : FunctionButton
    {
        /// <summary>
        /// </summary>
        public override void DrawButton()
        {
            DrawUtility.DrawProcess(this);
            SetTooltip();
            
        }

        ToolTip toolTip1 = new ToolTip();


        private void SetTooltip()
        {
            // Set up the delays for the ToolTip. 
            toolTip1.AutoPopDelay = 500;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active. 
            //toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox. 
            toolTip1.SetToolTip(this, "");
            //toolTip1.ToolTipIcon = ToolTipIcon.Info;

            this.MouseHover += new EventHandler(ProcessButton_MouseHover);
            this.MouseLeave += new EventHandler(ProcessButton_MouseLeave);
        }

        void ProcessButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(this.ProcessID, this, 10000);
        }

        private void ProcessButton_MouseLeave(object sender, EventArgs e)
        {
            //toolTip1.Hide(this);
            ToolStripDropDown t = new ToolStripDropDown();
        }

        public ProcessButton()
        {
            this.BackColor = SystemColors.Control;
        }

        public ProcessButton(Color backColor,double inAorrowDegree,Point inPoint)
        {
            this.BackColor = backColor;
            this.InArrowDegree = inAorrowDegree;
            this.InPoint = inPoint;         
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
 
        /// <summary>
        /// </summary>
        protected override void CreateContextMenu()
        {
            if (ContextMenu == null)
                ContextMenu = new ContextMenu();

            MenuItem miUp = new MenuItem();
            miUp.Text = "�������ϼ�ͷ";
            miUp.Click += new EventHandler(miUp_Click);
            ContextMenu.MenuItems.Add(miUp);

            MenuItem miDown = new MenuItem();
            miDown.Text = "�������¼�ͷ";
            miDown.Click += new EventHandler(miDown_Click);
            ContextMenu.MenuItems.Add(miDown);

            MenuItem miLeft = new MenuItem();
            miLeft.Text = "���������ͷ";
            miLeft.Click += new EventHandler(miLeft_Click);
            ContextMenu.MenuItems.Add(miLeft);

            MenuItem miRight = new MenuItem();
            miRight.Text = "�������Ҽ�ͷ";
            miRight.Click += new EventHandler(miRight_Click);
            ContextMenu.MenuItems.Add(miRight);

            MenuItem miRandom = new MenuItem();
            miRandom.Text = "��������Ƕȼ�ͷ";
            miRandom.Click += new EventHandler(miRandom_Click);
            ContextMenu.MenuItems.Add(miRandom);


            //MenuItem miTS = new MenuItem();
            //miTS.Text = "����ά������";
            //miTS.Click += new EventHandler(miTS_Click);
            //ContextMenu.MenuItems.Add(miTS);

            MenuItem miJump = new MenuItem();
            miJump.Text = "������ת����";
            miJump.Click += new EventHandler(miJump_Click);
            ContextMenu.MenuItems.Add(miJump);

            //MenuItem miDefine = new MenuItem();
            //miDefine.Text = "�����Զ�������";
            //miDefine.Click += new EventHandler(miDefine_Click);
            //ContextMenu.MenuItems.Add(miDefine);


        }

        /// <summary>
        /// </summary>
        private void miDefine_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.Cross;
                SelectProcessForm spf = new SelectProcessForm(this.Parent);
                if (spf.ShowDialog() == DialogResult.OK)
                {
                    ProcessButton pb = spf.SelectedButton;
                    DefineArrowButton dab;
                    if (pb.Left > this.Left)
                    {
                        Point startPoint = new Point(this.Left + this.Width, this.Top);
                        Point endPoint = new Point(pb.Left, pb.Top + pb.Height);
                        dab = new DefineArrowButton(startPoint, endPoint);
                        //�����Ϸ���
                        dab.Left = this.Left + this.Width;
                        dab.Top = this.Top - (startPoint.Y - endPoint.Y);
                    }
                    else
                    {
                        Point startPoint = new Point(this.Left, this.Top);
                        Point endPoint = new Point(pb.Left + pb.Width, pb.Top + pb.Height);
                        dab = new DefineArrowButton(startPoint, endPoint);
                        //�����Ϸ���
                        dab.Left = endPoint.X;
                        dab.Top = endPoint.Y;
                    }

                    this.Parent.Controls.Add(dab);
                    OutFlows.Add(dab);
                    dab.FromProcesses.Add(this);
                    dab.ToProcesses.Add(pb);
                }
                //��ȡĿ���
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }

        /// <summary>
        /// </summary>
        private void miJump_Click(object sender, EventArgs e)
        {
            //ѡ��Ŀ���
            ProcessButton pbEnd = null;
            SelectProcessForm spf = new SelectProcessForm(this.Parent);
            if (spf.ShowDialog() == DialogResult.OK)
            {
                pbEnd = spf.SelectedButton;
                int length = 0;
                if (pbEnd.Left > this.Left)
                {
                    length = Math.Abs(pbEnd.Left - this.Left - DrawUtility.a);
                }
                else
                {
                    length = Math.Abs(this.Left - pbEnd.Left - DrawUtility.a);
                }

                bool isBack = pbEnd.Left < this.Left;
                JumpArrowButton jab = new JumpArrowButton(isBack, length);
                DrawUtility.DrawJumpArrow(jab, null, length, isBack);
                if (pbEnd.Left > this.Left)
                {
                    jab.Left = this.Left + this.Width / 2 - DrawUtility.a;
                    jab.Top = this.Top - 12 * DrawUtility.a;
                }
                else
                {
                    jab.Left = pbEnd.Left + pbEnd.Width / 2 - DrawUtility.a;
                    jab.Top = this.Top - 17 * DrawUtility.a;
                }
                this.Parent.Controls.Add(jab);
                OutFlows.Add(jab);
                jab.FromProcesses.Add(this);
                jab.ToProcesses.Add(pbEnd);
            }
        }

        /// <summary>
        /// </summary>
        private void miTS_Click(object sender, EventArgs e)
        {
            UpDownArrowButton udb = new UpDownArrowButton();
            udb.Left = this.Left + (int)((DrawUtility.rw + DrawUtility.ah - 2) / 2 * DrawUtility.a);
            udb.Top = this.Top + this.Height - 4;
            this.Parent.Controls.Add(udb);
            OutFlows.Add(udb);
            udb.FromProcesses.Add(this);
            udb.SetLinkControls();
        }

     
        public override FlowButton AddOutArrow(double outdegree)
        {
            //�����ͷ�����غ� 
            if (((this.InArrowDegree- outdegree) % 360 + 360) % 360 == 180)
            {
                MessageBox.Show("�����ͷ�����غ�");
                return null;
            }

            StraightArrowButton rab = new StraightArrowButton(SystemColors.InactiveCaption, outdegree, this);
            this.Parent.Controls.Add(rab);
            rab.DrawButton();
            OutFlows.Add(rab);
            rab.FromProcesses.Add(this);
            rab.SetLinkControls();
            return rab;
        }

        /// <summary>
        /// ��ȡ���ΰ�ť�ͳ�����ͷ�ĽӴ��㣬��Ϊ��ͷ��ʵ��β������
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public override Point GetNextArrowTail(double outdegree)
        {      
            int x = 0;
            int y = 0;
            double degree = (outdegree % 360 + 360) % 360;
            if (degree <= 45 || degree > 315)
            {
                //�Ӵ���Ϊ���ΰ�ť���Ҳ��е�
                x = this.Right;
                y = (this.Top + this.Bottom) / 2;
            }
            else if (degree > 45 && degree <= 135)
            {
                //�Ӵ���Ϊ���ΰ�ť���²��е�
                x = (this.Left + this.Right) / 2;
                y = this.Bottom;
            }
            else if (degree > 135 && degree <= 225)
            {
                //�Ӵ���Ϊ���ΰ�ť������е�
                x = this.Left;
                y = (this.Top + this.Bottom) / 2;
            }
            else if (degree > 225 && degree <= 315)
            {
                //�Ӵ���Ϊ���ΰ�ť���ϲ��е�
                x = (this.Left + this.Right) / 2;
                y = this.Top;
            }

            return new Point(x, y);
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
    }
}