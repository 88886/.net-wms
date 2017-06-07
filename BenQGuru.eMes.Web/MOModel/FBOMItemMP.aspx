<%@ Page Language="c#" CodeBehind="FBOMItemMP.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.MOModel.MBOMItem" %>

<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>MBOMItem</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="<%=StyleSheet%>" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table1" height="100%" width="100%" runat="server">
        <tr class="moduleTitle">
            <td>
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic"> �����嵥��Ʒ��Ϣ</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="query" height="100%" width="100%">
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblItemCode" runat="server"> ��Ʒ����</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtItemCode" runat="server" CssClass="textbox" Width="120px"></asp:TextBox>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblItemName" runat="server"> ��Ʒ����</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtItemName" runat="server" CssClass="textbox" Width="120px"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblItemType" runat="server"> ��Ʒ����</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtItemType" runat="server" CssClass="textbox" Width="120px"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblSBOMVersionQuery" runat="server"> SBOM Version </asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:DropDownList ID="DropdownlistSBOMVersionQuery" runat="server" CssClass="require"
                                Width="120px" OnSelectedIndexChanged="DropdownlistSBOMVersionQuery_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td nowrap width="100%">
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
        <tr class="toolBar">
            <td>
                <table class="toolBar">
                    <tr>
                        <td>
                            <input class="submitImgButton" id="cmdReturn" type="submit" value="����" name="cmdReturn"
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
