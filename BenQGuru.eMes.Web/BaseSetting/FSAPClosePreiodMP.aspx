<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register TagPrefix="cc2" Namespace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>

<%@ Page Language="c#" CodeBehind="FSAPClosePreiodMP.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.BaseSetting.FSAPClosePreiodMP" %>

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
                <asp:Label ID="lblSAPTitle" runat="server" CssClass="labeltopic">SAP��������ά��</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="query" height="100%" width="100%">
                    <tr>
                        
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lbldateFromQuery" runat="server">��������</asp:Label>
                        </td>
                        <td >
                               <asp:TextBox type="text" id="txtDateUseBeginQuery"  class='datepicker' runat="server"  Width="130px"/>
                        </td>
                        <td class="fieldName" nowrap="noWrap" style="height: 26px">
                            <asp:Label ID="lbldateToQuery" runat="server">��</asp:Label>
                        </td>
                        <td >
                           <asp:TextBox type="text" id="txtDateUseEndQuery"  class='datepicker' runat="server"  Width="130px"/>
                        </td>
                        <td nowrap width="40%"><asp:TextBox type="text" ID="txtserial" runat="server" Visible="false"></asp:TextBox>
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
                       
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblFromDateTimeEdit" runat="server">��ʼ����</asp:Label>
                        </td>
                        <td class="fieldValue">
                           <asp:TextBox type="text" id="txtDateUseEdit"  class='datepicker' runat="server"  Width="130px"/>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblOnTimeEdit" runat="server">��ʼʱ��</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <uc2:emestime id="txtOnTimeEdit" runat="server" CssClass="require" width="130">
                            </uc2:emestime>
                        </td>
                        <td class="fieldName" nowrap>
                            <asp:Label ID="lblEndDateTimeEdit" runat="server">��������</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <asp:TextBox type="text" ID="txtToDateEdit" class='datepicker' runat="server"
                                Width="130px" />
                        </td>
                         <td class="fieldName" nowrap>
                            <asp:Label ID="lblOffTimeEdit" runat="server">����ʱ��</asp:Label>
                        </td>
                        <td class="fieldValue">
                            <uc2:emestime id="txtOffTimeEdit" runat="server" CssClass="require" width="130">
                            </uc2:emestime>
                        </td>
                       
                    </tr>
                    
            </table>
    </td> </tr>
    <tr class="toolBar">
        <td>
            <table class="toolBar">
                <tr>
                    <td class="toolBar">
                        <input class="submitImgButton" id="cmdAdd" type="submit" value="�� ��" name="cmdAdd"
                            runat="server">
                    </td>
                    <td class="toolBar">
                        <input class="submitImgButton" id="cmdSave" type="submit" value="�� ��" name="cmdSave"
                            runat="server">
                    </td>
                    <td>
                        <input class="submitImgButton" id="cmdCancel" type="submit" value="ȡ ��" name="cmdCancel"
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
