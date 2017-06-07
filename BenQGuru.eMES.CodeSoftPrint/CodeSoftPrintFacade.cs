using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BenQGuru.eMES.Common;
using BenQGuru.eMES.Common.Domain;
using BenQGuru.eMES.Common.DomainDataProvider;
using BenQGuru.eMES.Domain.Material;
using BenQGuru.eMES.Web.Helper;
using UserControl;
using BenQGuru.eMES.BaseSetting;

namespace BenQGuru.eMES.CodeSoftPrint
{
    public class CodeSoftPrintFacade : MarshalByRefObject
    {
        #region dataprovider
        private IDomainDataProvider _domainDataProvider = null;
        private FacadeHelper _helper = null;

        public CodeSoftPrintFacade(IDomainDataProvider domainDataProvider)
        {
            this._domainDataProvider = domainDataProvider;
            this._helper = new FacadeHelper(DataProvider);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public CodeSoftPrintFacade()
        {
            this._helper = new FacadeHelper(DataProvider);
        }

        protected IDomainDataProvider DataProvider
        {
            get
            {
                if (_domainDataProvider == null)
                {
                    _domainDataProvider = DomainDataProviderManager.DomainDataProvider();
                }
                return _domainDataProvider;
            }
        }
        #endregion

        #region Print

        private bool _IsBatchPrint = true;
        private string _DataDescFileName = "Label.dsc";
        protected CodeSoftPrintData _CodeSoftPrintData = new CodeSoftPrintData();
        protected CodeSoftFacade _CodeSoftFacade = new CodeSoftFacade();

        public string DataDescFileName
        {
            get { return _DataDescFileName; }
            set { _DataDescFileName = value; }
        }

        public UserControl.Messages Print(string printer, string templatePath, MKeyPart mKeyPart, List<MKeyPartDetail> mKeyPartDetailList, List<string> reserveInfo)
        {
            UserControl.Messages messages = new UserControl.Messages();

            try
            {
                try
                {
                    this.PrePrint();
                    _CodeSoftFacade.OpenTemplate(printer, templatePath);
                }
                catch (System.Exception ex)
                {
                    messages.Add(new UserControl.Message(UserControl.MessageType.Error, ex.Message));
                    return messages;
                }

                //������ӡǰ�����ı��ļ�
                string strBatchDataFile = string.Empty;
                if (_IsBatchPrint)
                {
                    strBatchDataFile = CreateFile();
                }

                for (int i = 0; i < mKeyPartDetailList.Count; i++)
                {
                    LabelPrintVars labelPrintVars = new LabelPrintVars();

                    string[] vars = new string[0];

                    if (messages.IsSuccess())
                    {
                        try
                        {
                            //Ҫ����Codesoft�����飬�ֶ�˳�����޸�
                            vars = this.GetPrintVars(mKeyPart, mKeyPartDetailList[i], reserveInfo);

                            //������ӡǰ��д�ļ�
                            if (_IsBatchPrint)
                            {
                                string[] printVars = ProcessVars(vars, labelPrintVars);
                                WriteFile(strBatchDataFile, printVars);
                            }
                            //ֱ�Ӵ�ӡ
                            else
                            {
                                _CodeSoftFacade.LabelPrintVars = labelPrintVars;
                                _CodeSoftFacade.Print(vars);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            messages.Add(new UserControl.Message(UserControl.MessageType.Error, ex.Message));
                            return messages;
                        }
                    }
                }

                //������ӡ
                if (_IsBatchPrint)
                {
                    try
                    {
                        _CodeSoftFacade.Print(strBatchDataFile, GetDataDescPath(_DataDescFileName));
                    }
                    catch (System.Exception ex)
                    {
                        messages.Add(new UserControl.Message(UserControl.MessageType.Error, ex.Message));
                        return messages;
                    }
                }

                messages.Add(new UserControl.Message(UserControl.MessageType.Success, "$Success_Print_Label"));
            }
            finally
            {
            }

            return messages;
        }

        //bighai.wang 2009/03/03 �������ϱ�ǩ��ӡ

        public UserControl.Messages PrintMaterialLot(string printer, List<string> materialLot)
        {
            UserControl.Messages messages = new UserControl.Messages();
            string varName = string.Empty;
            string templatePath = string.Empty;
            string fileName = string.Empty;

            try
            {
                try
                {
                    SystemSettingFacade parameter = new SystemSettingFacade();
                     fileName = parameter.GetGetParameterFileName("TemplateFileName", "MATERIALTYPE");
                     //templatePath = System.Environment.CurrentDirectory+"\\" + fileName;
                     varName = parameter.GetGetParameterFileName("VarName", "MATERIALTYPE");

                    this.PrePrint();
                    _CodeSoftFacade.OpenTemplateMaterialLot(printer, fileName, varName);
                }
                catch (System.Exception ex)
                {
                    messages.Add(new UserControl.Message(UserControl.MessageType.Error, ex.Message));
                    return messages;
                }

                //������ӡǰ�����ı��ļ�
                string strBatchDataFile = string.Empty;
                if (_IsBatchPrint)
                {
                    strBatchDataFile = CreateFile();
                }

                for (int i = 0; i < materialLot.Count; i++)
                {
                    LabelPrintVars labelPrintVars = new LabelPrintVars();

                    string[] vars = new string[0];

                    if (messages.IsSuccess())
                    {
                        try
                        {
                            //Ҫ����Codesoft�����飬�ֶ�˳�����޸�
                            vars = this.GetPrintVarsMaterialLot(materialLot[i]);

                            //������ӡǰ��д�ļ�
                            if (_IsBatchPrint)
                            {
                                string[] printVars = ProcessVars(vars, labelPrintVars);
                                WriteFile(strBatchDataFile, printVars);
                            }
                            //ֱ�Ӵ�ӡ
                            //else
                            //{
                                _CodeSoftFacade.LabelPrintVars = labelPrintVars;
                                _CodeSoftFacade.Print(vars, varName, fileName);
                            //}
                        }
                        catch (System.Exception ex)
                        {
                            messages.Add(new UserControl.Message(UserControl.MessageType.Error, ex.Message));
                            return messages;
                        }
                    }
                }

                //messages.Add(new UserControl.Message(UserControl.MessageType.Success, "$Success_Print_Label"));
            }
            finally
            {
            }

            return messages;
        }

        public virtual void PrePrint()
        {
        }

        //ȡ����ӡ��Ҫ����ģ������ݣ���������Ӧ�ֱ�ʵ���������
        public string[] GetPrintVars(string rCard, string mCode, string mName, string machineType, string qty, string dateCode)
        {
            string[] returnValue = new string[6];
            for (int i = 0; i < returnValue.Length; i++)
            {
                returnValue[i] = string.Empty;
            }

            returnValue[0] = rCard;
            returnValue[1] = mCode;
            returnValue[2] = mName;
            returnValue[3] = machineType;
            returnValue[4] = qty;
            returnValue[5] = dateCode;
            return returnValue;
        }

        //ȡ����ӡ��Ҫ����ģ������ݣ���������Ӧ�ֱ�ʵ���������
        protected string[] GetPrintVars(MKeyPart mKeyPart, MKeyPartDetail mKeyPartDetail, List<string> reserveInfo)
        {
            if (mKeyPart == null || mKeyPartDetail == null)
                return null;

            int count = (reserveInfo == null) ? 0 : reserveInfo.Count;
            string[] returnValue = new string[20 + count];
            for (int i = 0; i < returnValue.Length; i++)
            {
                returnValue[i] = string.Empty;
            }

            returnValue[1] = mKeyPart.MItemCode;
            returnValue[2] = mKeyPart.LotNO;
            returnValue[3] = mKeyPart.PCBA;
            returnValue[4] = mKeyPart.BIOS;
            returnValue[5] = mKeyPart.Version;
            returnValue[6] = mKeyPart.VendorItemCode;
            returnValue[7] = mKeyPart.VendorCode;
            returnValue[8] = mKeyPart.DateCode;
            returnValue[9] = mKeyPart.MoCode;
            returnValue[10] = mKeyPart.MITEMNAME;
            returnValue[11] = mKeyPartDetail.SerialNo;

            for (int i = 0; i < count; i++)
            {
                returnValue[20 + i] = reserveInfo[i];
            }

            return returnValue;
        }

        //bighai.wang 2009/03/03 �������ϱ�ǩ��ӡ
        protected string[] GetPrintVarsMaterialLot(string materialLot)
        {
            if (materialLot == null)
                return null;


            string[] returnValue = new string[1];

            returnValue[0] = materialLot;

            return returnValue;
        }

        //ȡ����ӡ��Ҫ����ģ������ݣ���������Ӧ�ֱ�ʵ���������
        //#3
        protected string GetPrintVars_No3()
        {
            return _CodeSoftPrintData.No3Seq;
        }

        //ȡ����ӡ��Ҫ����ģ������ݣ���������Ӧ�ֱ�ʵ���������
        //#2
        protected string GetPrintVars_No2()
        {
            return _CodeSoftPrintData.No2Seq;
        }

        /// <summary>
        /// ����������ӡ�����ݣ��Ա�ǩΪ��λ
        /// </summary>
        /// <param name="vars"></param>
        /// <param name="labelPrintVars"></param>
        /// <returns></returns>
        public string[] ProcessVars(string[] vars, LabelPrintVars labelPrintVars)
        {
            //��ȡ�������
            int intMaxSeq = vars.Length - 1;
            for (int i = 0; i < labelPrintVars.LabelVars_No2.Length; i++)
            {
                int intSeq = int.Parse(labelPrintVars.LabelVars_No2[i].Substring(3));
                if (intMaxSeq < intSeq) { intMaxSeq = intSeq; }
            }
            for (int i = 0; i < labelPrintVars.LabelVars_No3.Length; i++)
            {
                int intSeq = int.Parse(labelPrintVars.LabelVars_No3[i].Substring(3));
                if (intMaxSeq < intSeq) { intMaxSeq = intSeq; }
            }

            //****��ֵ******
            string[] results = new string[intMaxSeq + 1];
            for (int i = 0; i <= intMaxSeq; i++)
            { results[i] = string.Empty; }

            //vars
            for (int i = 0; i < vars.Length; i++)
            {
                results[i] = vars[i];
            }
            //���Ŵ�ӡ��ֵ
            for (int i = 0; i < labelPrintVars.LabelVars_No2.Length; i++)
            {
                int intSeq = int.Parse(labelPrintVars.LabelVars_No2[i].Substring(3));
                results[intSeq] = labelPrintVars.LabelValues_No2[i];
            }
            for (int i = 0; i < labelPrintVars.LabelVars_No3.Length; i++)
            {
                int intSeq = int.Parse(labelPrintVars.LabelVars_No3[i].Substring(3));
                results[intSeq] = labelPrintVars.LabelValues_No3[i];
            }

            return results;
        }

        public string CreateFile()
        {
            string strFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "LabelPrint\\Temp\\");
            if (System.IO.Directory.Exists(strFile) == false)
            {
                System.IO.Directory.CreateDirectory(strFile);
            }

            //��ʱĿ¼�еľ��ļ��ʵ�ɾ��һЩ
            DirectoryInfo dir = new DirectoryInfo(strFile);
            FileInfo[] files = dir.GetFiles();
            if (files != null && files.Length > 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].CreationTime.AddDays(30) < DateTime.Now)
                    {
                        try
                        {
                            files[i].Attributes &= ~FileAttributes.ReadOnly;
                            files[i].Delete();
                        }
                        catch { }
                    }
                }
            }

            strFile += "LabelPrint_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + DateTime.Now.Millisecond + "_Data.txt";
            StreamWriter writer = new StreamWriter(strFile);
            writer.Close();
            return strFile;
        }

        public void WriteFile(string strFile, string[] vars)
        {
            string strOutput = string.Empty;
            for (int i = 0; i < vars.Length; i++)
            { strOutput += "\"" + vars[i] + "\","; }
            if (strOutput.Length > 0) { strOutput = strOutput.Substring(0, strOutput.Length - 1); }

            StreamWriter writer = new StreamWriter(strFile, true, Encoding.Default);
            writer.WriteLine(strOutput);
            writer.Close();
        }

        public string GetDataDescPath(string fileName)
        {
            string strFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "") + "\\";
            strFile += fileName;
            return strFile;
        }


        #region ���ݴ�ӡģ��ı��������д�ӡ(�������������ӡ)
        public UserControl.Messages Print(string printer, string templatePath, List<System.Collections.Specialized.StringDictionary> valueLists)
        {
            UserControl.Messages messages = new UserControl.Messages();
            //Add By Leo @2013-12-31 for ���ǩ��ӡ֧�� 
            var templates = templatePath.Split('#');

            try
            {
                PrePrint();
                if (templates.Length > 1)                 //���ű�ǩ��ӡ
                {
                    try
                    {
                        foreach (var item in templates)
                        {
                            _CodeSoftFacade.OpenTemplate(printer, item.Trim().TrimEnd('#'));
                            _CodeSoftFacade.PrintWithOutReleaseCom(valueLists);
                        }
                    }
                    catch (Exception ex)
                    {
                        messages.Add(new UserControl.Message(MessageType.Error, ex.Message));
                    }
                    finally
                    {
                        //�ͷŶ˿�
                        _CodeSoftFacade.ReleaseCom();
                    }
                }
                else
                {
                    _CodeSoftFacade.OpenTemplate(printer, templatePath);
                    _CodeSoftFacade.Print(valueLists);
                }

                messages.Add(new UserControl.Message(UserControl.MessageType.Success, "$Success_Print_Label"));
            }
            catch (System.Exception ex)
            {
                messages.Add(new UserControl.Message(UserControl.MessageType.Error, ex.Message));
                return messages;
            }
            finally
            {
                _CodeSoftFacade.ReleaseCom();
            }

            return messages;
        }
        #endregion

        #endregion
    }
}
