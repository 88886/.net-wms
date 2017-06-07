<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FAlertASNReceiveMP.aspx.cs" Inherits="BenQGuru.eMES.Web.Alert.FAlertASNReceiveMP" %>
<%@ Register TagPrefix="cc2" NameSpace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<head>
    <title>Ԥ������趨</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="C#" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="<%=StyleSheet%>" rel="stylesheet" />
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table style="height: 100%; width: 100%;">
            <tr class="moduleTitle">
                <td>
                    <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic">Ԥ������趨</asp:Label></td>
            </tr>
            <tr>
                <td>
                    <table class="query" style="height: 50%; width: 100%;">
                        <tr>
                            <td class="fieldNameLeft" nowrap="nowrap">
                                <asp:Label ID="lblAlertItemSequence" runat="server">Ԥ�����</asp:Label>
                            </td>
                            <td class="fieldValue" nowrap="nowrap" style="width:222px">
                                <asp:TextBox ID="txtAlertItemSequence" runat="server" CssClass="textbox" Width="150px" ReadOnly="true">
                                </asp:TextBox>
                            </td>
                            
                            <td class="fieldNameLeft" nowrap="nowrap">
                                <asp:Label ID="lblAlertItemDesc" runat="server">�������</asp:Label>
                            </td>
                            <td class="fieldValue" style="width: 159px">
                                <asp:TextBox ID="txtAlertItemDesc" runat="server" CssClass="textbox" Width="250px" ReadOnly="true">
                                </asp:TextBox>
                            </td>
                            
                            <td style="width:100%;">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            
            <tr>
                <td>
                    <table class="query" style="height: 50%; width: 100%;">
                        <tr>
                            <td nowrap="nowrap" colspan="5" style="text-align:left;font-weight:bold;">
                                <asp:Label ID="lblAlertLinePause" runat="server">ͣ��ʱ��Ԥ��</asp:Label>
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="fieldNameLeft" nowrap="nowrap">
                                <asp:Label ID="lblSSEdit" runat="server">���ߴ���</asp:Label></td>
                            <td class="fieldValue">
                                <cc2:selectabletextbox4SS id="txtSSCodeWhere" runat="server" Type="stepsequence" Readonly="false" CanKeyIn="false" Width="131px" CssClass="require"></cc2:selectabletextbox4SS>
                            </td>
                            
                            <td class="fieldNameLeft" nowrap="nowrap">
                                <asp:Label ID="lblAlertOPCode" runat="server">Ͷ�빤��</asp:Label></td>
                            <td class="fieldValue" nowrap="nowrap">
                                <asp:DropDownList ID="ddlOPCode" runat="server" CssClass="require" Width="150px">
                                </asp:DropDownList>
                            </td>
                            
                            <td style="width:100%;">
                            </td>
                        </tr>
                        <tr>
                            <td class="fieldNameLeft" nowrap="nowrap">
                                <asp:Label ID="lblAlertStandard" runat="server">Ԥ����׼</asp:Label>
                            </td>
                            <td class="fieldValue" colspan="4">
                                <asp:TextBox ID="txtAlertStandard" runat="server" CssClass="require" Width="131px">
                                </asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr>
                            <td class="fieldNameLeft" nowrap="nowrap">
                                <asp:Label ID="lblDealMethod" runat="server">���÷�ʽ</asp:Label></td>
                            <td class="fieldValue" colspan="4" nowrap="nowrap">
                                <asp:CheckBox id="chbGenerateNotice" runat="server" Checked="true" Text="����Ԥ��ͨ��"></asp:CheckBox>
                                <asp:CheckBox id="chbSendMail" runat="server" Text="�����ʼ�" Checked="false"></asp:CheckBox>
                                <input class="submitImgButton" id="cmdMailSetup" type="submit" value="�ʼ��趨" name="cmdMailSetup"
                                    runat="server" onserverclick="cmdMailSetup_ServerClick" />
                            </td>
                            
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr height="100%">
                <td></td>
            </tr>
            
            <tr class="toolBar">
                <td>
                    <table class="toolBar">
                        <tr>
                            <td class="toolBar">
                                <input class="submitImgButton" id="cmdSave" type="submit" value="�� ��" name="cmdSave"
                                    runat="server" onserverclick="cmdSave_ServerClick" /></td>
                            <td class="toolBar"><INPUT class="submitImgButton" id="cmdReturn" type="submit" value="�� ��" name="cmdReturn"
										runat="server" onserverclick="cmdReturn_ServerClick"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
