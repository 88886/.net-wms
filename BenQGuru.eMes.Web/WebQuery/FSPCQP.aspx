<%@ Page Language="c#" CodeBehind="FSPCQP.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.WebQuery.FSPCQP" %>

<%@ Register TagPrefix="cc2" Namespace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>FSPCQP</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="<%=StyleSheet%>" rel="stylesheet">
    <script src="../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script>
        $(document.all).find("chbAuto").click = function () {
            try {
                var date1 = Date.parse(document.getElementById("txtDateQuery_GuruDate").value.replace(/-/g, '/'));
                var d = new Date();
                var date2 = Date.parse(d.getFullYear() + "/" + (parseInt(d.getMonth()) + 1).toString() + "/" + d.getDate());
                if (date1 != date2)
                    document.all.chbAuto.checked = false;
            }
            catch (e) {

            }

        }
        $(document.all).find("img1").click = function () {
            calendar1(document.getElementById("txtDateQuery_GuruDate"), true);

            try {
                var date1 = Date.parse(document.getElementById("txtDateQuery_GuruDate").value.replace(/-/g, '/'));
                var d = new Date();
                var date2 = Date.parse(d.getFullYear() + "/" + (parseInt(d.getMonth()) + 1).toString() + "/" + d.getDate());
                if (date1 != date2) {
                    document.all.chbAuto.disabled = true;
                    document.all.chbAuto.checked = false;
                    document.all.txtMIQuery.disabled = true;
                }
                else {
                    document.all.chbAuto.disabled = false;
                    document.all.chbAuto.checked = true;
                    document.all.txtMIQuery.disabled = false;
                }
            }
            catch (e) {

            }
        }
        function valiateInput() {
            var _inputEmpty = " " + document.getElementById("_Error_Input_Empty").value;

            if ($("#txtItemCodeQuery").find("table").find("tbody").find("tr").find("td").find("input").val() == "") {
                alert(document.getElementById("lblItemCodeQuery").innerText + _inputEmpty); //alert("��Ʒ���� ȱ������");
                return false;
            }
            if ($("#txtDateQuery").val() == "") {
                alert(document.getElementById("lblSDateQuery").innerText + _inputEmpty); //alert("���� ȱ������");
                return false;
            }
            if ($("#drpTestNameQuery option:selected").length == 0) {
                alert(document.getElementById("lblTestNameQuery").innerText + _inputEmpty); 	//alert("������ ȱ������");
                return false;
            }
            if (document.getElementById("chbAuto").checked == true) {
                if (document.getElementById("txtMIQuery").value == "") {
                    alert(document.getElementById("lblMIQuery").innerText + _inputEmpty); //alert("���� ȱ������");
                    return false;
                }
                var mi = parseFloat(document.getElementById("txtMIQuery").value);
                if (isNaN(mi)) {
                    alert(document.getElementById("lblMIQuery").innerText + " " + document.getElementById("_Error_Number_Format_Error").value); //alert("���� ���ָ�ʽ�������")
                    return false;
                }
                if (mi <= 0) {
                    alert(document.getElementById("lblMIQuery").innerText + " " + document.getElementById("_Error_Number_TooLittle").value + " 0"); //alert("���� ������� 0");
                    return false;
                }
            }

            return true;
        }

        function DisplaySPC() {
            if (valiateInput()) {
                var spc = document.getElementById("SPC_Chart");
                spc.ChartType = $("#hidChartType").val();
                spc.ItemCode = $("#txtItemCodeQuery").find("input").eq(0).val();
                spc.SearchDateFrom = $("#txtDateQuery").val();
                spc.SearchDateTo = $("#txtDateQuery").val();
                if ($("#txtDateToQuery").val() != "")
                    spc.SearchDateTo = $("#txtDateToQuery").val();

                spc.Resource = $("#txtResQuery").find("input").eq(0).val();
                spc.TestItem = $("#drpTestNameQuery option:selected").val();
                spc.Condition = $("#drpConditionQuery option:selected").val();
                spc.TestResult = $("#drpTestResult option:selected").val();
                spc.AutoRefresh = $("#chbAuto").attr("checked") == "checked" ? "1" : "0";
                spc.AutoRefreshMins = $("#txtMIQuery").val();
                spc.Server = $("#_server").val();
                spc.InvokeSearch();
                //window.open(encodeURI("http://grd-joe-song/emes/WebQuery/GetSPCData.aspx?itemcode=" + document.getElementById("txtItemCodeQuery").value + "&date=" + document.getElementById("txtDateQuery_GuruDate").value + "&resource=" + document.getElementById("txtResQuery$ctl00").value + "&testitem=" + document.getElementById("drpTestNameQuery").value));
            }
        }

        function showInstallHelp() {
            if (document.getElementById("trSPC").style.display != "none") {
                document.getElementById("trSPC").style.display = "none";
                document.getElementById("trInstallTip").style.display = "";
            }
            else {
                document.getElementById("trSPC").style.display = "";
                document.getElementById("trInstallTip").style.display = "none";
            }
        }
       
    </script>
</head>
<body scroll="yes" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table height="100%" width="100%" runat="server">
        <tr class="moduleTitle">
            <td>
                <asp:Label ID="lblTitle" runat="server" CssClass="labeltopic">SPC��CPK����</asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 20px">
                <table class="query" width="100%" >
                    <tr>
                        <td nowrap class="fieldNameLeft">
                            <asp:Label ID="lblItemCodeQuery" runat="server">��Ʒ����</asp:Label>
                        </td>
                        <td nowrap>
                            <cc2:SelectableTextBox id="txtItemCodeQuery" runat="server" CssClass="require" AutoPostBack="True"
                                type="singleitem">
                            </cc2:SelectableTextBox>
                        </td>
                        <td nowrap class="fieldNameLeft">
                            <asp:Label ID="lblTestNameQuery" runat="server"> ������</asp:Label>
                        </td>
                        <td nowrap>
                            <asp:DropDownList ID="drpTestNameQuery" runat="server" CssClass="require" Width="100"
                                AutoPostBack="True" OnSelectedIndexChanged="drpTestNameQuery_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td nowrap class="fieldNameLeft">
                            <asp:Label ID="lblTestConditionQuery" runat="server"> ��������</asp:Label>
                        </td>
                        <td nowrap>
                            <asp:DropDownList ID="drpConditionQuery" runat="server" CssClass="require" Width="100">
                            </asp:DropDownList>
                        </td>
                        <td nowrap class="fieldNameLeft">
                            <asp:Label ID="lblRes" runat="server"> �� Դ</asp:Label>
                        </td>
                        <td nowrap>
                            <cc2:selectabletextbox id="txtResQuery" runat="server" CssClass="textbox" width="50"
                                Target="resource" CanKeyIn="true" Type="resource">
                            </cc2:selectabletextbox>
                        </td>
                        <td align="right" colspan="2">
                            <a href="#" onclick="showInstallHelp();return false;" style="color: blue">
                                <asp:Label ID="lblComponent" runat="server" Text="�����װ����"></asp:Label></a>
                        </td>
                        <td>
                            <input type="hidden" id="hidChartType" runat="server">
                        </td>
                    </tr>
                    <tr>
                        <td nowrap class="fieldNameLeft" >
                            <asp:Label ID="lblSDateQuery" runat="server"> ��ʼ����</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px;" >
                            <asp:TextBox type="text" ID="txtDateQuery" class='datepicker' runat="server" Width="130px" />
                        </td>
                        <td nowrap class="fieldNameLeft">
                            <asp:Label ID="lblEDateQuery" runat="server"> ��������</asp:Label>
                        </td>
                        <td class="fieldValue" style="width: 159px">
                            <asp:TextBox type="text" ID="txtDateToQuery" class='datepicker' runat="server" Width="130px" />
                        </td>
                        <td nowrap class="fieldNameLeft">
                            <asp:Label ID="lblTestResultQuery" runat="server"> ���Խ��</asp:Label>
                        </td>
                        <td nowrap>
                            <asp:DropDownList ID="drpTestResult" runat="server" Width="100">
                                <asp:ListItem Selected="True" Value="">����</asp:ListItem>
                                <asp:ListItem Value="GOOD">��Ʒ</asp:ListItem>
                                <asp:ListItem Value="NG">����Ʒ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td nowrap colspan="2">
                            <asp:CheckBox ID="chbAuto" runat="server" Text=" �Զ�ˢ��"></asp:CheckBox><asp:TextBox
                                ID="txtMIQuery" runat="server" CssClass="textbox" Width="24px">5</asp:TextBox><asp:Label
                                    ID="lblMinute" runat="server">����</asp:Label>
                        </td>
                        <td width="30%">
                        </td>
                        <td>
                            &nbsp;&nbsp;
                            <input class="submitImgButton" id="cmdQuery" type="button" value="�� ѯ" name="btnQuery"
                                runat="server" onclick="DisplaySPC();">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height="100%" id="trSPC" >
            <td class="fieldGrid">
       			<OBJECT id="SPC_Chart" codeBase="BenQGuru_eMES.CAB#version=1,0,0,11" classid="CLSID:18037CDC-C620-4ADF-A0FF-D29FB00C7EC3" 
							VIEWASTEXT>
							<PARAM NAME="_ExtentX" VALUE="25532">
							<PARAM NAME="_ExtentY" VALUE="12303">
						</OBJECT>
            </td>
        </tr>
        <tr height="100%" id="trInstallTip" valign="top" style="display: none">
            <td class="fieldGrid">
                <br>
                <b>
                    <asp:Label ID="lblActiveX" runat="server" Text=" ����޷��Զ���װActiveX�ؼ����볢�����½������:"></asp:Label></b>
                <ul>
                    <li>
                        <asp:Label runat="server" ID="lblCloseIE" Text="�ر�����IE��������ڣ������´򿪱�ҳ��"> </asp:Label>
                        <li>
                            <asp:Label runat="server" ID="lblReset" Text="��������ϵͳ�������´򿪱�ҳ��"></asp:Label>
                            <li>
                                <asp:Label runat="server" ID="lblSetActiveX" Text="ĳЩIE��������������ActiveX��������Ӧ����"> </asp:Label>
                                <li>
                                    <asp:Label runat="server" ID="lblExportGuru" Text="������¹��ȫ֤�飬�����´򿪱�ҳ�棬���"></asp:Label><a
                                        href="BenQGuruCertificate.zip" style="color: blue">
                                        <asp:Label runat="server" ID="lblHere" Text="����"></asp:Label>
                                    </a>
                                    <asp:Label runat="server" ID="lblDownGuru" Text="���ذ�ȫ֤��"> </asp:Label>
                                    <li>
                                        <asp:Label runat="server" ID="lblDownInstr" Text="���ذ�װ���򣬵��"></asp:Label><a href="BenQGuru_eMES_Setup.zip"
                                            style="color: blue">
                                            <asp:Label ID="lblHhere" runat="server" Text="����"></asp:Label>
                                        </a>
                                        <asp:Label ID="lblInserll" runat="server" Text="��װ����"></asp:Label>
                                        <li>
                                            <asp:Label ID="lblSystemRight" runat="server" Text="��ȷ������ϵͳ������"> </asp:Label>
                                            <li>
                                                <asp:Label ID="lblSystemReset" runat="server" Text="�п���ĳЩϵͳ�ļ����𻵣����°�װ����ϵͳ"></asp:Label>
                                            </li>
                </ul>
                <asp:Label ID="lblBackSPC" runat="server" Text="�ٴε��'�����װ����'���Իص�SPCͼ�����"> </asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
