<%@ Register TagPrefix="cc2" Namespace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>

<%@ Page Language="c#" CodeBehind="FSolderPasteMP.aspx.cs" AutoEventWireup="True"
    Inherits="BenQGuru.eMES.Web.SMT.FSolderPasteMP" %>

<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register TagPrefix="uc1" TagName="eMESDate" Src="~/UserControl/DateTime/DateTime/eMESDate.ascx" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>FSolderPasteMP</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="<%=StyleSheet%>" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table height="100%" width="100%" runat="server">
        <tr class="moduleTitle">
            <td>
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic"> ����Ǽ�</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="query" height="100%" width="100%">
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblSPIDQuery" runat="server"> �������</asp:Label>
                        </td>
                        <td class="fieldValue" nowrap>
                            <asp:TextBox ID="txtSPIDQuery" runat="server" CssClass="textbox" Width="130px"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" style="height: 26px" nowrap>
                            <asp:Label ID="lblPartNOQuery" runat="server">�������Ϻ�</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtPartNOQuery" runat="server" CssClass="textbox" Width="130px"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblLotSNQuery" runat="server">��������</asp:Label>
                        </td>
                        <td class="fieldValue" nowrap>
                            <asp:TextBox ID="txtLotNOQuery" runat="server" CssClass="textbox" Width="130px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblDateStartQuery" runat="server"> ����������</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox type="text" ID="txtFromProDateQuery" class='datepicker' runat="server"
                                Width="130px" />
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblZ" runat="server"> ��</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox type="text" ID="txtToProDateQuery" class='datepicker' runat="server"
                                Width="130px" />
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblSPStatusQuery" runat="server"> ����״̬</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:DropDownList ID="drpSPStatusQuery" runat="server" Width="120px" CssClass="textbox">
                            </asp:DropDownList>
                        </td>
                        <td class="fieldName">
                            <input class="submitImgButton" id="cmdQuery" type="submit" value="�� ѯ" name="btnQuery"
                                runat="server" onserverclick="cmdQuery_ServerClick">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height="100%">
            <td class="fieldGrid">
                <ig:WebDataGrid ID="gridWebGrid" runat="server" Width="100%">
                </ig:WebDataGrid>
            </td>
        </tr>
        <tr class="normal">
            <td>
                <table height="100%" cellpadding="0" width="100%">
                    <tr>
                        <td class="smallImgButton">
                            <input class="gridExportButton" id="cmdGridExport" type="submit" value="  " name="cmdCancel"
                                runat="server" onserverclick="cmdGridExport_ServerClick">
                        </td>
                        <td class="smallImgButton">
                            <input class="deleteButton" id="cmdDelete" type="submit" value="  " name="cmdDelete"
                                runat="server" onserverclick="cmdDelete_ServerClick">
                        </td>
                        <td>
                            <cc1:pagersizeselector id="pagerSizeSelector" runat="server">
                            </cc1:pagersizeselector>
                        </td>
                        <td align="right">
                            <cc1:pagertoolbar id="pagerToolBar" runat="server" PageSize="20">
                            </cc1:pagertoolbar>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="normal">
            <td>
                <table class="edit" height="100%" cellpadding="0" width="100%">
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblSPIDEdit" runat="server"> �������</asp:Label>
                        </td>
                        <td class="fieldValue" nowrap>
                            <asp:TextBox ID="txtSPIDEdit" runat="server" CssClass="require" Width="130px"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" style="height: 26px" nowrap>
                            <asp:Label ID="lblPartSNEdit" runat="server">�������Ϻ�</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtPartNOEdit" runat="server" CssClass="require" Width="130px"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblLotSNEdit" runat="server">��������</asp:Label>
                        </td>
                        <td class="fieldValue" nowrap>
                            <asp:TextBox ID="txtLotNOEdit" runat="server" CssClass="require" Width="130px"></asp:TextBox>
                        </td>
                        <td width="40%">
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblDateCode" runat="server"> ��������</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox type="text" ID="txtProDateEdit" class='datepicker' runat="server" Width="130px" />
                        </td>
                        <td colspan="5">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="toolBar">
            <td>
                <table class="toolBar">
                    <tr>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdAdd" type="submit" value="�� ��" name="cmdAdd"
                                runat="server" onserverclick="cmdAdd_ServerClick">
                        </td>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdSave" type="submit" value="�� ��" name="cmdSave"
                                runat="server" onserverclick="cmdSave_ServerClick">
                        </td>
                        <td>
                            <input class="submitImgButton" id="cmdCancel" type="submit" value="ȡ ��" name="cmdCancel"
                                runat="server" onserverclick="cmdCancel_ServerClick">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
