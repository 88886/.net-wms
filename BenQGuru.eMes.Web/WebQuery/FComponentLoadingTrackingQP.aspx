<%@ Page Language="c#" CodeBehind="FComponentLoadingTrackingQP.aspx.cs" AutoEventWireup="True"
    Inherits="BenQGuru.eMES.Web.WebQuery.FComponentLoadingTrackingQP" %>

<%@ Register TagPrefix="uc1" TagName="eMESDate" Src="~/UserControl/DateTime/DateTime/eMESDate.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>FComponentLoadingTrackingQP</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="<%=StyleSheet%>" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table height="100%" width="100%" runat="server">
        <tr class="moduleTitle">
            <td>
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic">����׷�ٹ���</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="query" height="100%" width="100%">
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblInsideItemCodeQuery" runat="server">�Ϻ�</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtInsideItemCodeQuery" runat="server" CssClass="require" Width="110px"></asp:TextBox>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblSupplierCodeQuery" runat="server">���̴���</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtVendorCodeQuery" runat="server" CssClass="textbox" Width="110px"></asp:TextBox>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblSupplierItemQuery" runat="server">�����Ϻ�</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtVendorItemCodeQuery" runat="server" CssClass="textbox" Width="110px"></asp:TextBox>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblLotNo" runat="server">��������</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtLotNo" runat="server" CssClass="textbox" Width="110px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblDateCode" runat="server">��������</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtDateCode" runat="server" CssClass="textbox" Width="110px"></asp:TextBox>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblINNO" runat="server">�����Ϻ�</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtINNO" runat="server" CssClass="textbox" Width="110px"></asp:TextBox>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblKeypartsStart" runat="server">�����Ϻſ�ʼ</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtKeypartsStart" runat="server" CssClass="textbox" Width="110px"></asp:TextBox>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblKeypartsEnd" runat="server">�����ϺŽ���</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtKeypartsEnd" runat="server" CssClass="textbox" Width="110px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblStartDateQuery" runat="server">��ʼ����</asp:Label>
                        </td>
                        <td class="fieldValue">
                        <asp:TextBox  id="dateStartDateQuery"  class='datepicker' runat="server"  Width="110px"/>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblEndDateQuery" runat="server">��������</asp:Label>
                        </td>
                        <td class="fieldValue">
                        <asp:TextBox  id="dateEndDateQuery"  class='datepicker' runat="server"  Width="110px"/>
                        </td>
                        <td>
                        </td>
                        <td nowrap>
                            <asp:CheckBox ID="chbSplitTransition" runat="server" Text="�������ת��"></asp:CheckBox>
                        </td>
                        <td>
                        </td>
                        <td class="fieldName">
                            <input class="submitImgButton" id="cmdQuery" type="submit" value="�� ѯ" name="btnQuery"
                                runat="server">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height="100%">
            <td class="fieldGrid">
                <ig:webdatagrid id="gridWebGrid" runat="server" width="100%">
                </ig:webdatagrid>
            </td>
        </tr>
        <tr class="normal">
            <td>
                <table height="100%" cellpadding="0" width="100%">
                    <tr>
                        <td class="smallImgButton">
                            <input class="gridExportButton" id="cmdGridExport" type="submit" value="  " name="cmdGridExport"
                                runat="server">
                            
                        </td>
                        <td>
                            <cc1:pagersizeselector id="pagerSizeSelector" runat="server">
                            </cc1:pagersizeselector>
                        </td>
                        <td align="right">
                            <cc1:PagerToolbar id="pagerToolBar" runat="server">
                            </cc1:PagerToolbar>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
