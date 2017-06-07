<%@ Page Language="c#" CodeBehind="FErrorCodeGroup2ErrorCodeAP.aspx.cs" AutoEventWireup="True"
    Inherits="BenQGuru.eMES.Web.BaseSetting.FErrorCodeGroup2ErrorCodeAP" %>

<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title></title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="<%=StyleSheet%>" rel="stylesheet">
    <base target="_self" />
    
</head>
<body topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table height="100%" width="100%" id="table1" runat="server">
        <tr class="moduleTitle">
            <td>
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic"> ѡ��������</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="query" height="100%" width="100%">
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblNGCodeQuery" runat="server"> ��������</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox ID="txtErrorCodeCodeQuery" runat="server" Width="150px" CssClass="textbox"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblSummaryObjectQuery" runat="server" Visible="False"> ����������</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox ID="txtErrorCodeGroupCodeQuery" runat="server" Width="150px" CssClass="textbox"
                                ReadOnly="True" Visible="False"></asp:TextBox>
                        </td>
                        <td nowrap width="100%">
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
                <ig:WebDataGrid ID="gridWebGrid" runat="server" Width="100%">
                </ig:WebDataGrid>
            </td>
        </tr>
        <tr class="normal">
            <td>
                <table height="100%" cellpadding="0" width="100%">
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
        <tr class="toolBar">
            <td>
                <table class="toolBar">
                    <tr>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdSave" type="submit" value="����" name="cmdSelect"
                                runat="server" >
                        </td>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdCancel" type="submit" value="ȡ��" name="cmdLeave"
                                runat="server" >
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
