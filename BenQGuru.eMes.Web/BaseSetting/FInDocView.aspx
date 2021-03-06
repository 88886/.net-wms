<%@ Page Language="c#" CodeBehind="FInDocView.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.BaseSetting.FInDocView" %>

<%@ Register TagPrefix="cc2" Namespace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>FModelMP</title>
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
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic">文档查询</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="query" height="100%" width="100%">
                    <tr>
                        
                        
                          <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblTicketNoQuery" runat="server">单据号</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 120px">
                            <asp:TextBox ID="txtInvdocnoQuery" runat="server" CssClass="textbox" Width="120px"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblDocnameQuery" runat="server">文件名称</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 120px">
                            <asp:TextBox ID="txtDocnameQuery" runat="server" CssClass="textbox" Width="120px"></asp:TextBox>
                        </td>
                       
                    
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblDoctypeQuery" runat="server">文档类型</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 120px">
                            <asp:DropDownList ID="drpDoctypeQuery" runat="server" CssClass="select" Width="120px">
                            </asp:DropDownList>
                        </td>
                  
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblDirNameQuery" runat="server">目录名称</asp:Label>
                        </td>
                        <td class="fieldValue" nowrap>
                            <asp:DropDownList ID="drpDirNameQuery" runat="server" CssClass="select" Width="120px">
                            </asp:DropDownList>
                        </td>
                       <td class="fieldName" nowrap>
                            <asp:Label ID="lblPickNoQuery" runat="server">拣货任务令号</asp:Label>
                        </td>
                        <td class="fieldValue" nowrap>
                            <asp:TextBox ID="txtPickNoQuery" name="txtPickNoQuery" runat="server" CssClass="textbox"
                                Width="130px"></asp:TextBox>
                        </td>

                        <td class="fieldName">
                            <input class="submitImgButton" id="cmdQuery" type="submit" value="查 询" name="btnQuery"
                                runat="server" onserverclick="cmdQuery_ServerClick">
                        </td>
                    </tr>
                    
                       <tr style="display: none">
                    
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblMemoQuery" runat="server">备注</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 120px">
                            <asp:TextBox ID="txtMemoQuery" runat="server" CssClass="textbox" Width="120px"></asp:TextBox>
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
                            <input class="gridExportButton" id="cmdGridExport" type="submit" value="  " name="cmdCancel"
                                runat="server" onserverclick="cmdGridExport_ServerClick">
                            
                        </td>
                        <td>
                            <cc1:PagerSizeSelector ID="pagerSizeSelector" runat="server"></cc1:PagerSizeSelector>
                        </td>
                        <td align="right">
                            <cc1:PagerToolBar ID="pagerToolBar" runat="server" PageSize="20"></cc1:PagerToolBar>
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
                            <input class="submitImgButton" id="cmdReturn" type="submit" value="返 回" name="cmdReturn"
                                runat="server" onserverclick="cmdReturn_ServerClick" visible="false">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
