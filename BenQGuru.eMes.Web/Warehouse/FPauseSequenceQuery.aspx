<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FPauseSequenceQuery.aspx.cs"
    Inherits="BenQGuru.eMES.Web.WarehouseWeb.FPauseSequenceQuery" %>

<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>FPauseSequenceQuery</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="<%=StyleSheet%>" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table1" height="100%" width="100%" runat="server">
        <tr class="moduleTitle">
            <td>
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic">ͣ�����кŲ�ѯ</asp:Label>
            </td>
        </tr>
        <tr>
            <td style="display: none">
                <table class="query" height="100%" width="100%">
                    <tr>
                        <td class="fieldValue" style="display: none">
                            <asp:TextBox ID="txtStorageCodeQuery" runat="server" Width="150px" ReadOnly="True"
                                CssClass="textbox"></asp:TextBox>
                            <asp:TextBox ID="txtStackCodeQuery" runat="server" Width="150px" ReadOnly="True"
                                CssClass="textbox"></asp:TextBox>
                            <asp:TextBox ID="txtPalletCodeQuery" runat="server" Width="150px" ReadOnly="True"
                                CssClass="textbox"></asp:TextBox>
                            <asp:TextBox ID="txtItemCodeQuery" runat="server" Width="150px" ReadOnly="True" CssClass="textbox"></asp:TextBox>
                            <asp:TextBox ID="txtPauseCodeQuery" runat="server" Width="150px" ReadOnly="True"
                                CssClass="textbox"></asp:TextBox>
                            <asp:TextBox ID="txtCancelPauseQuery" runat="server" Width="150px" ReadOnly="True"
                                CssClass="textbox"></asp:TextBox>
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
        <tr class="toolBar">
            <td>
                <table class="toolBar">
                    <tr>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdReturn" type="submit" value="�� ��" name="cmdReturn"
                                runat="server" onserverclick="cmdReturn_ServerClick">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
