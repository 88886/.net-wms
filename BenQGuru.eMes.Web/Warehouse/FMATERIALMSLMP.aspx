﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FMATERIALMSLMP.aspx.cs"
    Inherits="BenQGuru.eMES.Web.WarehouseWeb.FMATERIALMSLMP" %>

<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register TagPrefix="cc2" Namespace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>FMATERIALMSLMP</title>
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
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic">物料湿敏等级维护</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="query" height="100%" width="100%">
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblItemIDQuery" runat="server">物料代码</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <cc2:selectabletextbox id="txtConditionItem" runat="server" Width="90px" Type="material"
                                readonly="false" cankeyin="true">
                            </cc2:selectabletextbox>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblItemNQuery" runat="server">物料名称</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox ID="txtItemNQuery" runat="server" Width="130px" CssClass="textbox"></asp:TextBox>
                        </td>
                        <td nowrap width="100%">
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblMaterialDescQuery" runat="server">物料描述</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox ID="txtMaterialDescQuery" runat="server" Width="130px" CssClass="textbox"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblMSDLevelQuery" runat="server">湿敏等级</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <cc2:selectabletextbox id="txtMSDLevelQuery" runat="server" Width="90px" Type="msdlevel"
                                readonly="false" cankeyin="true">
                            </cc2:selectabletextbox>
                        </td>
                        <td nowrap width="100%">
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblMSDLevelDescQuery" runat="server">湿敏等级描述</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox ID="txtMSDLevelDescQuery" runat="server" Width="130px" CssClass="textbox"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="fieldName">
                            <input class="submitImgButton" id="cmdQuery" type="submit" value="查 询" name="btnQuery"
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
                        <td class="smallImgButton">
                            <input class="gridExportButton" id="cmdGridExport" type="submit" value="  " name="cmdGridExport"
                                runat="server">
                        </td>
                        <td class="smallImgButton">
                            <input class="deleteButton" id="cmdDelete" type="submit" value="  " name="cmdDelete"
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
        <tr class="normal">
            <td>
                <table class="edit" height="100%" cellpadding="0" width="100%">
                    <tr>
                        <td class="fieldNameLeft" style="height: 26px" nowrap>
                            <asp:Label ID="lblItemIDEdit" runat="server">物料代码</asp:Label>
                        </td>
                        <td class="fieldValue" style="height: 26px">
                            <cc2:selectabletextbox id="txtItemIDEdit" runat="server" Type="singlematerial">
                            </cc2:selectabletextbox>
                        </td>
                        <td class="fieldNameLeft" style="height: 26px" nowrap>
                            <asp:Label ID="lblMSDLevelEdit" runat="server">湿敏等级</asp:Label>
                        </td>
                        <td class="fieldValue" style="height: 26px">
                            <cc2:selectabletextbox id="txtMSDLevelEdit" runat="server" Type="singlemsdlevel">
                            </cc2:selectabletextbox>
                        </td>
                        <td style="width: 100%">
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
                            <input class="submitImgButton" id="cmdAdd" type="submit" value="新 增" name="cmdAdd"
                                runat="server">
                        </td>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdSave" type="submit" value="保存" name="cmdSave"
                                runat="server" />
                        </td>
                        <td>
                            <input class="submitImgButton" id="cmdCancel" type="submit" value="取 消" name="cmdCancel"
                                runat="server">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
