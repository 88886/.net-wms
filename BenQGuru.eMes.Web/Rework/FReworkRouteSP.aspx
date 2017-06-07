<%@ Page Language="c#" CodeBehind="FReworkRouteSP.aspx.cs" AutoEventWireup="True"
    Inherits="BenQGuru.eMES.Web.Rework.FReworkRouteSP" %>

<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>FReworkRouteSP</title>
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
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic">����;��ά��</asp:Label>
                <asp:TextBox ID="txtItemCode" runat="server" Visible="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <table class="query" id="Table2" height="100%" width="100%">
                    <tr>
                        <td nowrap>
                            <asp:Label ID="lblReTypeQuery" runat="server">��������</asp:Label>
                        </td>
                        <td class="fieldName">
                            <asp:TextBox ID="txtReworkType" runat="server" CssClass="textbox" ReadOnly="True"
                                DESIGNTIMEDRAGDROP="272"></asp:TextBox>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblReworkStatus" runat="server">����״̬</asp:Label>
                        </td>
                        <td class="fieldName">
                            <asp:TextBox ID="txtReworkStatus" runat="server" CssClass="textbox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="fieldName" width="100%">
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
                <table id="Table3" height="100%" cellpadding="0" width="100%">
                    <tr>
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
        <tr class="normal">
            <td>
                <table class="edit" id="Table4" height="100%" cellpadding="0" width="100%">
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblReworkCode" runat="server">���󵥺�</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtReworkCode" runat="server" CssClass="textbox" ReadOnly="True"
                                DESIGNTIMEDRAGDROP="207"></asp:TextBox>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblRoute" runat="server">;��</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:DropDownList ID="drpRoute" runat="server" Width="150px" OnLoad="drpRoute_Load">
                            </asp:DropDownList>
                        </td>
                        <td width="100%">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="toolBar">
            <td>
                <table class="toolBar" id="Table5">
                    <tr>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdSave" type="submit" value="�� ��" name="cmdSave"
                                runat="server">
                        </td>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdCancel" type="submit" value="ȡ ��" name="cmdCancel"
                                runat="server">
                        </td>
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
