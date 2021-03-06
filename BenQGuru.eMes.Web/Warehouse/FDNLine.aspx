<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FDNLine.aspx.cs" Inherits="BenQGuru.eMES.Web.WarehouseWeb.FDNLine" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register TagPrefix="cc2" Namespace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>FDNLine</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="<%=StyleSheet%>" rel="stylesheet">
</head>
<script language="javascript">
    function CheckNumber() {
        var dnQuantity = document.getElementById("txtDNQuantityEdit");
        var dnQuantityValue = dnQuantity.value;

        for (var i = 0; i < dnQuantityValue.length; i++) {
            if (isNaN(parseInt(dnQuantityValue.substr(i, 1)))) {
                alert("计划数量必须为数字格式, 并且大于零!");
                document.getElementById("txtDNQuantityEdit").focus();
                return false;
            }
        }

        return true;

    }
</script>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table height="100%" width="100%" runat="server">
        <tr class="moduleTitle">
            <td>
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic">单据行项维护</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="query" height="100%" width="100%">
                    <tr>
                        <td class="fieldNameLeft" nowrap>
                            <asp:Label ID="lblDNNOQuery" runat="server">单据号码</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox ID="txtDNNOQuery" runat="server" Width="150px" ReadOnly="True" CssClass="textbox"></asp:TextBox>
                        </td>
                        <td nowrap width="100%">
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
                            <asp:Label ID="lblItemCodeEdit" runat="server">产品代码</asp:Label>
                        </td>
                        <td class="fieldValue" style="height: 26px">
                            <cc2:selectabletextbox id="txtItemCodeEdit" runat="server" CssClass="require" Width="150px"
                                AutoPostBack="true" Type="singleitem">
                            </cc2:selectabletextbox>
                        </td>
                        <td class="fieldNameLeft" style="height: 26px" nowrap>
                            <asp:Label ID="lblItemDescpritionEdit" runat="server">产品描述</asp:Label>
                        </td>
                        <td class="fieldValue" style="height: 26px">
                            <asp:TextBox ID="txtItemDescriptionEdit" runat="server" CssClass="textbox" Width="260px"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" style="height: 26px" nowrap>
                            <asp:Label ID="lblOrderNoEdit" runat="server">合同号</asp:Label>
                        </td>
                        <td class="fieldValue" style="height: 26px">
                            <asp:TextBox ID="txtOrderNoEdit" runat="server" CssClass="textbox" Width="130px"></asp:TextBox>
                        </td>
                        <td colspan="1" style="width: 100%">
                        </td>
                    </tr>
                    <tr>
                        <%--<td class="fieldNameLeft" noWrap>
								    <asp:label id="lblItemGradeEdit" runat="server">产品档次</asp:label></td>
								<td class="fieldValue"  style="WIDTH: 159px">
									<asp:DropDownList ID="drpItemGradeEdit" runat="server" CssClass="textbox" Width="130px" DESIGNTIMEDRAGDROP="94" OnLoad="drpItemGradeEdit_Load"></asp:DropDownList></TD>--%>
                        <td class="fieldNameLeft" style="height: 26px" nowrap>
                            <asp:Label ID="lblStorageTypeEdit" runat="server">库别</asp:Label>
                        </td>
                        <td class="fieldValue" style="height: 26px">
                            <asp:DropDownList ID="drpStorageCodeEdit" runat="server" CssClass="require" Width="130px"
                                DESIGNTIMEDRAGDROP="94" OnLoad="drpStorageCodeEdit_Load">
                            </asp:DropDownList>
                        </td>
                        <td class="fieldNameLeft" style="height: 26px" nowrap>
                            <asp:Label ID="lblDNQuantityEdit" runat="server">计划数量</asp:Label>
                        </td>
                        <td class="fieldValue" style="height: 26px">
                            <asp:TextBox ID="txtDNQuantityEdit" runat="server" CssClass="require" Width="130px"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" nowrap>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                        </td>
                        <td style="width: 100%">
                        </td>
                    </tr>
                    <tr>
                        <td class="fieldNameLeft" style="height: 26px" nowrap>
                            <asp:Label ID="lblMOCodeEdit" runat="server">工单</asp:Label>
                        </td>
                        <td class="fieldValue" style="height: 26px">
                            <asp:TextBox ID="txtMOCodeEdit" runat="server" CssClass="textbox" Width="260px"></asp:TextBox>
                        </td>
                        <td class="fieldNameLeft" style="height: 26px" nowrap>
                            <asp:Label ID="lblReworkMOEdit" runat="server">返工工单</asp:Label>
                        </td>
                        <td class="fieldValue" style="height: 26px">
                            <asp:TextBox ID="txtReworkMOEdit" runat="server" CssClass="textbox" Width="260px"></asp:TextBox>
                        </td>
                        <td class="fieldValue" style="display: none">
                            <asp:TextBox ID="txtDnLine" runat="server" CssClass="textbox"></asp:TextBox>
                        </td>
                        <td class="fieldValue" style="display: none">
                            <asp:TextBox ID="txtRealQuantity" runat="server" CssClass="textbox"></asp:TextBox>
                        </td>
                        <td colspan="2" style="width: 100%">
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
                            <input class="submitImgButton" id="cmdAdd" type="submit" value="新 增" name="cmdSelect"
                                runat="server" onclick="return CheckNumber();">
                        </td>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdSave" type="submit" value="保存" name="cmdSave"
                                runat="server" onclick="return CheckNumber();">
                        </td>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdCancel" type="submit" value="取 消" name="cmdCancel"
                                runat="server">
                        </td>
                        <td class="toolBar">
                            <input class="submitImgButton" id="cmdReturn" type="submit"
                                value="返 回" name="cmdReturn" runat="server" onserverclick="cmdReturn_ServerClick">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
