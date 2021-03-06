<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register TagPrefix="cc2" Namespace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>

<%@ Page Language="c#" CodeBehind="FEQPUseInfo.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.BaseSetting.FEQPUseInfo" %>

<%@ Register TagPrefix="uc1" TagName="eMESDate" Src="~/UserControl/DateTime/DateTime/eMESDate.ascx" %>
<%@ Register TagPrefix="uc2" TagName="eMESTime" Src="~/UserControl/DateTime/DateTime/eMESTime.ascx" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>FEQPOEE</title>
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
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic">设备使用情况维护</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="query" height="100%" width="100%">
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblEQPIDQuery" runat="server">设备编码</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <cc2:SelectableTextBox ID="txtEQPIDQuery" runat="server" Type="Equiment" 
                                CanKeyIn="true" CssClass="textbox"></cc2:SelectableTextBox>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblDateUseQuery" runat="server">使用日期</asp:Label>
                        </td>
                        <td >
                               <asp:TextBox type="text" id="txtDateUseBeginQuery"  class='datepicker' runat="server"  Width="130px"/>
                        </td>
                        <td>
                            ~
                        </td>
                        <td >
                           <asp:TextBox type="text" id="txtDateUseEndQuery"  class='datepicker' runat="server"  Width="130px"/>
                        </td>
                        <td nowrap width="40%">
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
                        <td class="smallImgButton">
                            <input class="deleteButton" id="cmdDelete" type="submit" value="  " name="cmdDelete"
                                runat="server">
                        </td>
                        <td>
                            <cc1:PagerSizeSelector ID="pagerSizeSelector" runat="server"></cc1:PagerSizeSelector>
                        </td>
                        <td align="right">
                            <cc1:PagerToolBar ID="pagerToolBar" runat="server"></cc1:PagerToolBar>
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
                            <asp:Label ID="lblPEQPIDEdit" runat="server">设备编码</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <cc2:SelectableTextBox ID="txtEQPIDEdit" runat="server" Type="singEqpID" Width="100px"
                                CanKeyIn="true" CssClass="require"></cc2:SelectableTextBox>
    
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblDateUseEdit" runat="server">使用日期</asp:Label>
                        </td>
                        <td class="fieldValue">
                           <asp:TextBox type="text" id="txtDateUseEdit"  class='datepicker' runat="server"  Width="130px"/>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblSTOPDurationEdit" runat="server">停机时长(分钟)</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtSTOPDurationEdit" runat="server" Width="150px" CssClass="require"
                                MaxLength="6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblOnTimeEdit" runat="server">开机时间</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <uc2:emestime id="txtOnTimeEdit" runat="server" CssClass="require" width="130">
                            </uc2:emestime>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblOffTimeEdit" runat="server">关机时间</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <uc2:emestime id="txtOffTimeEdit" runat="server" CssClass="require" width="130">
                            </uc2:emestime>
                        </td>
                        <td class="fieldName" nowrap>
                        </td>
                        <td class="fieldValue">
                        </td>
                    </tr>
            </table>
    </td> </tr>
    <tr class="toolBar">
        <td>
            <table class="toolBar">
                <tr>
                    <td class="toolBar">
                        <input class="submitImgButton" id="cmdAdd" type="submit" value="新 增" name="cmdAdd"
                            runat="server">
                    </td>
                    <td class="toolBar">
                        <input class="submitImgButton" id="cmdSave" type="submit" value="保 存" name="cmdSave"
                            runat="server">
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
