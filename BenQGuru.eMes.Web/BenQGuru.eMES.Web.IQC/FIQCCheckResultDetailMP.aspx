<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FIQCCheckResultDetailMP.aspx.cs"
    Inherits="BenQGuru.eMES.Web.IQC.FIQCCheckResultDetailMP" %>

<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register TagPrefix="cc2" Namespace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>IQCDetailMP</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="<%=StyleSheet%>" rel="stylesheet">
    <style type="text/css">
        .style2
        {
            height: 26px;
            width: 70px;
        }
        .style3
        {
            height: 46px;
            width: 70px;
        }
        .style4
        {
            height: 26px;
            width: 42px;
        }
        .style5
        {
            height: 46px;
            width: 42px;
        }
        .textbox
        {
            margin-right: 0px;
        }
        .style6
        {
            width: 112px;
        }
        .style7
        {
            height: 46px;
        }
    </style>
    <script src="../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script language="javascript">
        //        $(function () {
        //            $("#gridWebGrid2").height(60);
        //        });
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:Label ID="lblIQCNo" runat="server" Visible="false">�ͼ쵥��</asp:Label>
    <asp:TextBox ID="txtIQCNo" runat="server" CssClass="require" ReadOnly="true" Width="130px"
        Visible="false"></asp:TextBox>
    <table id="TableCheck" height="100%" width="100%" runat="server">
        <tr class="moduleTitle">
            <td>
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic">IQC��ϸ��Ϣά��</asp:Label>
            </td>
        </tr>
        <%--<tr>
                <td>
                    <table class="query" height="100%" width="100%" >
                        <tr>
                            <td class="fieldNameLeft" style="height: 26px" nowrap>
                                <asp:Label ID="lblIQCNo" runat="server" >�ͼ쵥��</asp:Label></td>
                            <td class="fieldValue" style="height: 26px">
                                <asp:TextBox ID="txtIQCNo" runat="server" CssClass="require" ReadOnly="true" Width="130px" ></asp:TextBox></td>
                            <td class="fieldName" style="height: 26px" nowrap>
                            </td>
                            <td nowrap width="100%">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
        <tr height="100%">
            <td class="fieldGrid">
                <ig:WebDataGrid ID="gridWebGrid" runat="server" Width="100%">
                </ig:WebDataGrid>
            </td>
        </tr>
        <tr class="normal">
            <td>
                <table cellpadding="0" style="height: 100%; width: 100%">
                    <tr>
                        <td class="smallImgButton">
                            <input class="gridExportButton" id="cmdGridExport" type="submit" value="  " name="cmdCancel"
                                runat="server" onserverclick="cmdGridExport_ServerClick">
                        </td>
                        <td>
                            <cc1:PagerSizeSelector ID="pagerSizeSelector" runat="server">
                            </cc1:PagerSizeSelector>
                        </td>
                        <td align="right">
                            <cc1:PagerToolBar ID="pagerToolBar" runat="server">
                            </cc1:PagerToolBar>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="normal">
            <td>
                <table class="edit" cellpadding="0" width="100%" id="GridTable2" style="height: 260px;">
                    <tr>
                        <td style="width: 80%;">
                            <table class="edit" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="fieldName" nowrap style="height: 26px">
                                        <asp:Label ID="lblIQCLineNoEdit" runat="server">�ͼ쵥�к�</asp:Label>
                                    </td>
                                    <td class="fieldValue" style="height: 26px">
                                        <asp:TextBox ID="txtIQCLineNoEdit" runat="server" Width="110px" CssClass="require"
                                            Enabled="false"></asp:TextBox>
                                    </td>
                                    <td class="fieldName" nowrap>
                                        <asp:Label ID="lblAcceptStyleEdit" runat="server">���ܷ�ʽ</asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:DropDownList ID="drpAcceptStyleEdit" runat="server" Width="110px" CssClass="textbox"
                                            OnLoad="drpAcceptStyleEdit_Load">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="fieldName" nowrap>
                                        <asp:Label ID="lblPIC" runat="server">PICȷ��</asp:Label>
                                    </td>
                                    <td class="fieldValue">
                                        <asp:TextBox ID="txtPICEdit" runat="server" Width="110px" CssClass="textbox"></asp:TextBox>
                                    </td>
                                    <td class="fieldName" nowrap>
                                        <asp:Label ID="lblAQLLevel" runat="server">AQL��׼</asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:DropDownList ID="drpAQLLevel" runat="server" Width="110px" CssClass="textbox"
                                            OnLoad="drpAQLLevel_Load" OnSelectedIndexChanged="drpAQLLevel_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 26px" valign="middle">
                                        <asp:RadioButtonList ID="rblCheckType" runat="server" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="rblCheckType_SelectedIndexChanged" AutoPostBack="true" style="float:left;">
                                        </asp:RadioButtonList>
                                        <asp:RadioButtonList ID="rblPass" runat="server" RepeatDirection="Horizontal" style="float:left;margin-left:130px;">
                                        </asp:RadioButtonList>
                                    </td>
                                    <td class="fieldName" nowrap style="height: 26px">
                                        <asp:Label ID="lblAdviseSampleQty" runat="server">��������</asp:Label>
                                    </td>
                                    <td class="fieldValue" style="height: 26px">
                                        <asp:TextBox ID="txtAdviseSampleQty" runat="server" Width="110px" CssClass="textbox"></asp:TextBox>
                                    </td>
                                    <td class="fieldName" nowrap>
                                        <asp:Label ID="lblSampleQty" runat="server">������</asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:TextBox ID="txtSampleQtyEdit" runat="server" Width="110px" CssClass="textbox"></asp:TextBox>
                                    </td>
                                    <td class="fieldName" nowrap>
                                        <asp:Label ID="lblNGQty" runat="server">������</asp:Label>
                                    </td>
                                    <td class="fieldValue">
                                        <asp:TextBox ID="txtNGQtyEdit" runat="server" Width="110px" CssClass="textbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style7">
                                        <asp:Label ID="lblNGDescEdit" runat="server">���ϸ�����</asp:Label>
                                    </td>
                                    <td class="style7" style="width:330px;">
                                        <asp:TextBox ID="txtNGDescEdit" runat="server" Width="300px" CssClass="textbox" Height="30px"
                                            MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td class="fieldGrid" colspan="6" rowspan="4">
                                        <ig:WebDataGrid ID="gridWebGridF" runat="server" Width="99%" Height="170px">
                                        </ig:WebDataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style3">
                                        <asp:Label ID="lblDescription" runat="server">��ע</asp:Label>
                                    </td>
                                    <td class="style5">
                                        <asp:TextBox ID="txtDescriptionEdit" runat="server" Width="300px" CssClass="textbox"
                                            Height="30px" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style7">
                                        <asp:Label ID="lblAction" runat="server">�����Դ�ʩ˵��</asp:Label>
                                    </td>
                                    <td class="style7">
                                        <asp:TextBox ID="txtActionEidt" runat="server" Width="300px" CssClass="textbox" Height="30px"
                                            MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="fieldName" style="height: 26px" nowrap>
                                        <asp:Label ID="lblCheckGroupEdit" runat="server">�����Ŀ��</asp:Label>
                                    </td>
                                    <td class="fieldValue" style="height: 26px">
                                        <cc2:selectabletextbox id="txtCheckGroupCodeEdit" runat="server" Width="120px" cankeyin="true"
                                            CssClass="require" Type="singleoqccheckgroup" ontextchanged="txtCheckGroupEdit_TextChanged"
                                            AutoPostBack="true">
                                        </cc2:selectabletextbox>
                                    </td>
                                </tr>
                                <tr class="toolBar">
                                    <td colspan="6">
                                        <table class="toolBar">
                                            <tr>
                                                <td class="toolBar" align="center">
                                                    <input class="submitImgButton" id="cmdGoodChecked" type="submit" value="�����ϸ�" name="cmdGoodChecked"
                                                        runat="server" onserverclick="cmdGoodChecked_ServerClick" visible="false">
                                                </td>
                                                <td class="toolBar" align="center">
                                                    <input class="submitImgButton" id="Submit1" type="submit" value="" name="cmdSave1"
                                                        runat="server" visible="false">
                                                </td>
                                                <td class="toolBar" align="center">
                                                    <input class="submitImgButton" id="Submit2" type="submit" value="" name="cmdSave2"
                                                        runat="server" visible="false">
                                                </td>
                                                <td class="toolBar" align="center">
                                                    <input class="submitImgButton" id="Submit3" type="submit" value="" name="cmdSave3"
                                                        runat="server" visible="false">
                                                </td>
                                                <td class="toolBar" align="center">
                                                    <input class="submitImgButton" id="Submit4" type="submit" value="" name="cmdSave4"
                                                        runat="server" visible="false">
                                                </td>
                                                <td class="toolBar" align="center">
                                                    <input class="submitImgButton" id="Submit5" type="submit" value="" name="cmdSave5"
                                                        runat="server" visible="false">
                                                </td>
                                                <td class="toolBar" align="center">
                                                    <input class="submitImgButton" id="cmdSave" type="submit" value="�� ��" name="cmdSave"
                                                        runat="server" >
                                                </td>
                                                <td class="toolBar" align="center">
                                                    <input class="submitImgButton" id="cmdCancel" type="submit" value="ȡ ��" name="cmdCancel"
                                                        runat="server" onserverclick="cmdCancel_ServerClick">
                                                </td>
                                                <td class="toolBar" align="center">
                                                    <input class="submitImgButton" id="cmdReturn" type="submit" value="�� ��" name="cmdReturn"
                                                        runat="server" onserverclick="cmdReturn_ServerClick">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table class="edit" cellpadding="0" style="height: 276px;">
                                <tr>
                                    <td align="left" class="style6">
                                        <asp:Label ID="lblRegressToAccept" runat="server">�ò�����</asp:Label>
                                    </td>
                                    <td nowrap width="100%">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style6" nowrap>
                                        <asp:Label ID="lblRegressToAcceptNUmber" runat="server">�ò���������</asp:Label>
                                    </td>
                                    <td class="fieldValue" style="padding-right:5px;">
                                        <asp:TextBox ID="txtRegressToAcceptNUmberEdit" runat="server" Width="180px" CssClass="textbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style6" nowrap>
                                        <asp:Label ID="lblRegressToAcceptNo" runat="server">�ò����յ�����</asp:Label>
                                    </td>
                                    <td class="fieldValue" style="padding-right:5px;">
                                        <asp:TextBox ID="txtRegressToAcceptNoEdit" runat="server" Width="180px" CssClass="textbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style6" nowrap style="height: 136px;">
                                        <asp:Label ID="lblRegressToAcceptCaption" runat="server">�ò�����˵��</asp:Label>
                                    </td>
                                    <td class="fieldValue" style="padding-right:5px;">
                                        <asp:TextBox ID="txtRegressToAcceptCaptionEdit" runat="server" Width="180px" Height="80px"
                                            TextMode="MultiLine" CssClass="textbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style6" nowrap>
                                        <asp:Label ID="Label1" runat="server" Visible="false">�ò�����</asp:Label>
                                    </td>
                                    <td class="toolBar" align="left">
                                        <input class="submitImgButton" id="cmdRegressToAcceptButton" type="submit" value="�ò�����"
                                            name="cmdRegressToAccept" runat="server" onserverclick="cmdRegressToAcceptButton_ServerClick">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
