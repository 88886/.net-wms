<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FExecuteASNMP.aspx.cs" Inherits="BenQGuru.eMES.Web.WarehouseWeb.FExecuteASNMP" %>

<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register TagPrefix="cc2" Namespace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<%@ Register TagPrefix="uc1" TagName="eMESDate" Src="~/UserControl/DateTime/DateTime/eMESDate.ascx" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>FExecuteASNMP</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="<%=StyleSheet%>" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table id="table1" height="100%" width="100%" runat="server">
        <tr class="moduleTitle">
            <td>
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic">仓库执行入库指令</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="query" height="100%" width="100%">
                    <tr>
                         <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblStorageInASNQuery" runat="server">入库指令号</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox ID="txtStorageInASNQuery" runat="server" Width="150px" DESIGNTIMEDRAGDROP="257"
                                CssClass="textbox"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblStorageInTypeQuery" runat="server">入库类型</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:DropDownList ID="drpStorageInTypeQuery" runat="server" CssClass="textbox" Width="150px"
                                DESIGNTIMEDRAGDROP="94">
                            </asp:DropDownList>
                        </td>
                         <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblInvNoQuery" runat="server">SAP单据号</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox ID="txtInvNoQuery" runat="server" Width="150px" DESIGNTIMEDRAGDROP="257"
                                CssClass="textbox"></asp:TextBox>
                        </td>
                        
                           <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblStackListQuery" runat="server">库位</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:DropDownList ID="drpStackListQuery" runat="server" CssClass="textbox" Width="150px"
                                DESIGNTIMEDRAGDROP="94">
                            </asp:DropDownList>
                        </td>

                    </tr>

                    <tr>
                         <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblStatusQuery" runat="server">状态</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:DropDownList ID="drpStatusQuery" runat="server" CssClass="textbox" Width="120px"
                                DESIGNTIMEDRAGDROP="94">
                            </asp:DropDownList>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblCBDateQuery" runat="server">创建日期开始</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                             <asp:TextBox type="text" id="txtCBDateQuery"  class='datepicker' runat="server"  Width="130px"/>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblCEDateQuery" runat="server">创建日期结束</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                             <asp:TextBox type="text" id="txtCEDateQuery"  class='datepicker' runat="server"  Width="130px"/>
                        </td>
                       <td colspan="2" nowrap  width="100%">
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
                         <input class="submitImgButton" id="cmdInitial" type="submit" value="取消下发" name="cmdInitial"
                                runat="server"  onclick="GetSelectRowGUIDS('gridWebGrid')"  onserverclick="cmdInitial_ServerClick" />
                                </td>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdInitialCheck" type="submit" value="初检" name="cmdInitialCheck"
                                runat="server"   onclick="GetSelectRowGUIDS('gridWebGrid')" onserverclick="cmdInitialCheck_ServerClick" />
                        </td>
                        <td>
                            <input class="submitImgButton" id="cmdApplyIQC" type="submit" value="申请IQC" name="cmdApplyIQC"
                                   runat="server"  onclick="GetSelectRowGUIDS('gridWebGrid')"  onserverclick="cmdApplyIQC_ServerClick" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

    </table>
    </form>
</body>
</html>
