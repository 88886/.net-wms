using System;
using UserControl;

namespace BenQGuru.eMES.Common.DCT.Core
{
    /// <author>Laws Lu</author>
    /// <since>2006/04/14</since>
    /// <version>1.0.0</version>
    public abstract class BaseDCTAction
    {
        public BaseDCTAction()
        {
        }

        #region ����

        private ActionStatus _Status = ActionStatus.PrepareData;
        private FlowDirect _FlowDirect;
        private UserControl.Message _InitMesssage = null;
        private UserControl.Message _InputMessage = null;
        private UserControl.Message _OutMesssage = null;
        private UserControl.Message _LastPrompMesssage = null;

        private bool _IsTopAction = true;
        private int _CurrentSequence;
        private BaseDCTAction _NextAction = null;
        private BaseDCTAction _LastAction = null;

        private object _ObjectState = null;
        private int _RepetTimes;
        private bool _NeedCancel = false;
        private bool _NeedAuthorized = true;

        #endregion

        #region Property

        /// <summary>
        /// ״̬
        /// </summary>
        public virtual ActionStatus Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }
        }

        /// <summary>
        /// ���ݴ��ݷ���
        /// </summary>
        public virtual FlowDirect FlowDirect
        {
            get
            {
                return _FlowDirect;
            }
            set
            {
                _FlowDirect = value;
            }
        }

        /// <summary>
        /// ��ʼ����Ϣ
        /// </summary>
        public virtual UserControl.Message InitMessage
        {
            get
            {
                return _InitMesssage;
            }
            set
            {
                _InitMesssage = value;
            }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public virtual UserControl.Message InputMessage
        {
            get
            {
                return _InputMessage;
            }
            set
            {
                _InputMessage = value;
            }
        }

        /// <summary>
        /// �����Ϣ
        /// </summary>
        public virtual UserControl.Message OutMesssage
        {
            get
            {
                return _OutMesssage;
            }
            set
            {
                _OutMesssage = value;
            }
        }

        /// <summary>
        /// ���һ�ε���ʾ�����Ϣ
        /// </summary>
        public virtual UserControl.Message LastPrompMesssage
        {
            get
            {
                return _LastPrompMesssage;
            }
            set
            {
                _LastPrompMesssage = value;
            }
        }

        /// <summary>
        /// �Ƿ�Ϊ����Action
        /// </summary>
        public virtual bool IsTopAction
        {
            get
            {
                return _IsTopAction;
            }
            set
            {
                _IsTopAction = value;
            }
        }

        /// <summary>
        /// ��ǰִ�д���
        /// </summary>
        public virtual int CurrentSequence
        {
            get
            {
                return _CurrentSequence;
            }
            set
            {
                _CurrentSequence = value;
            }
        }

        /// <summary>
        /// ��һ��Action
        /// </summary>
        public virtual BaseDCTAction NextAction
        {
            get
            {
                return _NextAction;
            }
            set
            {
                _NextAction = value;
            }
        }

        /// <summary>
        /// ǰһAction��Ŀǰδʹ�ã�
        /// </summary>
        public virtual BaseDCTAction LastAction
        {
            get
            {
                return _LastAction;
            }
            set
            {
                _LastAction = value;
            }
        }

        /// <summary>
        /// ����Ķ���
        /// </summary>
        public virtual object ObjectState
        {
            get
            {
                return _ObjectState;
            }
            set
            {
                _ObjectState = value;
            }
        }

        /// <summary>
        /// �ظ�����
        /// </summary>
        public virtual int RepetTimes
        {
            get
            {
                return _RepetTimes;
            }
            set
            {
                _RepetTimes = value;
            }
        }

        /// <summary>
        /// �Ƿ���ҪCancel��������
        /// </summary>
        public virtual bool NeedCancel
        {
            get
            {
                return _NeedCancel;
            }
            set
            {
                _NeedCancel = value;
            }
        }

        /// <summary>
        /// �Ƿ�ֻ���ڵ�¼��ʹ��
        /// </summary>
        public virtual bool NeedAuthorized
        {
            get
            {
                return _NeedAuthorized;
            }
            set
            {
                _NeedAuthorized = value;
            }
        }

        #endregion

        #region Method

        public virtual Messages InitAction(object act)
        {
            this._Status = ActionStatus.PrepareData;
            FlowDirect = FlowDirect.WaitingInput;

            Messages msgs = new Messages();
            if (this.InitMessage != null)
                msgs.Add(this.InitMessage);
            if (this.OutMesssage != null)
                msgs.Add(this.OutMesssage);
            return msgs;
        }

        public virtual Messages PreAction(object act)
        {
            this._Status = ActionStatus.Working;
            FlowDirect = FlowDirect.WaitingInput;

            return null;
        }

        public virtual Messages Action(object act)
        {
            this._Status = ActionStatus.Pass;
            FlowDirect = FlowDirect.WaitingOutput;

            return null;
        }

        public virtual Messages AftAction(object act)
        {
            this._Status = ActionStatus.PrepareData;
            FlowDirect = FlowDirect.WaitingInput;

            return null;
        }

        public virtual Messages Do(object act)
        {
            Messages msgs = new Messages();

            if (this._Status == ActionStatus.Init)
            {
                msgs = InitAction(act);
            }
            else if (this._Status == ActionStatus.PrepareData)
            {
                msgs = PreAction(act);
            }
            else if (this._Status == ActionStatus.Working)
            {
                msgs = Action(act);
            }
            else if (this._Status == ActionStatus.Pass)
            {
                msgs = AftAction(act);
            }

            return msgs;
        }

        protected void ProcessBeforeReturn(ActionStatus newStatus, Messages msgs)
        {
            //����Status��FlowDirect
            this._Status = newStatus;
            switch (newStatus)
            {
                case ActionStatus.Init:
                    this.FlowDirect = FlowDirect.WaitingOutput;
                    break;

                case ActionStatus.PrepareData:
                    this.FlowDirect = FlowDirect.WaitingInput;
                    break;

                case ActionStatus.Working:
                    this.FlowDirect = FlowDirect.WaitingInput;
                    break;

                case ActionStatus.Pass:
                    this.FlowDirect = FlowDirect.WaitingOutput;
                    break;

                default:
                    this.FlowDirect = FlowDirect.WaitingOutput;
                    break;
            }

            //�����ص�Messages
            if (msgs != null && msgs.Count() > 0)
            {
                if (msgs.Objects(msgs.Count() - 1).Type == MessageType.Normal)
                    this.LastPrompMesssage = msgs.Objects(msgs.Count() - 1);

                if (msgs.Objects(msgs.Count() - 1).Type == MessageType.Error && this.LastPrompMesssage != null)
                    msgs.Add(this.LastPrompMesssage);
            }
        }

        #endregion
    }
}
