<%@ Page language="c#" Codebehind="FModelRouteOperationEP.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.MOModel.FModelRouteOperationEP" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FModelRouteOperationEP</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="<%=StyleSheet%>" rel=stylesheet>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table height="100%" width="100%">
				<!--this is for title-->
				<tr class="moduleTitle">
					<td><asp:label id="lblTitle" runat="server" CssClass="labeltopic"> ��Ʒ����ά��</asp:label></td>
				</tr>
				<!--this is for operation information update-->
				<tr class="normal">
					<td>
						<table class="edit" height="100%" cellPadding="0" width="100%">
							<tr>
								<TD class="fieldNameLeft" noWrap><asp:label id="lblItemOperationCodeEdit" runat="server">�������</asp:label></TD>
								<td class="fieldValue"><asp:textbox id="txtOperationCodeEdit" runat="server"></asp:textbox></td>
							</tr>
							<tr>
								<td class="fieldNameLeft" noWrap><asp:label id="lblCodeEdit" runat="server">���</asp:label></td>
								<td class="fieldValue" style="HEIGHT: 26px"><asp:textbox id="txtOperationsequenceEdit" runat="server"></asp:textbox></td>
								<td nowrap width="100%"></td>
							</tr>
							<tr>
								<td nowrap class="fieldNameLeft"><asp:checkbox id="chbOperationCheckEdit" runat="server" Width="100px" Text="������"></asp:checkbox></td>
								<td nowrap class="fieldValue"><asp:label id="lblOPCheckEdit" runat="server" Width="200px">���Ա�Ǹù����Ƿ���й�����</asp:label></td>
							</tr>
							<tr>
								<td nowrap class="fieldNameLeft"><asp:checkbox id="chbEditSPC" runat="server" Width="100px" Text="SPCͳ��"></asp:checkbox></td>
								<td nowrap class="fieldValue"><asp:label id="lblSPCEdit" runat="server" Width="200px">���Ա�Ǹù����Ƿ����SPCͳ��</asp:label></td>
								<td nowrap width="100%"></td>
							</tr>
							<tr>
								<td nowrap class="fieldNameLeft"><asp:checkbox id="chbCompLoadingEdit" runat="server" Width="100px" Text="���ϼ��"></asp:checkbox></td>
								<td nowrap class="fieldValue"><asp:label id="lblComponentLoadingEdit" runat="server" Width="200px">���Ա�Ǹù����Ƿ�Ϊ���Ϲ���</asp:label></td>
							</tr>
							<tr>
								<td nowrap class="fieldNameLeft"><asp:checkbox id="chbNGTestEdit" runat="server" Width="100px" Text="�����ж�"></asp:checkbox></td>
								<td nowrap class="fieldValue"><asp:label id="lblTestEdit" runat="server" Width="200px">���Ա�Ǹù����Ƿ�Ϊ���Թ���</asp:label></td>
								<td nowrap width="100%"></td>
							</tr>
							<tr>
								<td nowrap class="fieldNameLeft"><asp:checkbox id="chbRepairEdit" runat="server" Width="100px" Text="ά��"></asp:checkbox></td>
								<td nowrap class="fieldValue"><asp:label id="lblRepair" runat="server" Width="200px">���Ա�Ǹù����Ƿ�Ϊά�޹���</asp:label></td>
							</tr>
							<tr>
								<td nowrap class="fieldNameLeft"><asp:checkbox id="chbStartOpEdit" runat="server" Width="100px" Text="��ʼ����"></asp:checkbox></td>
								<td nowrap class="fieldValue"><asp:label id="lblStartOperationEdit" runat="server" Width="200px">���Ա�Ǹù����Ƿ�Ϊ��ʼ����</asp:label></td>
								<td nowrap width="100%"></td>
							</tr>
							<tr>
								<td nowrap class="fieldNameLeft"><asp:checkbox id="chbEndOpEdit" runat="server" Width="100px" Text="��������"></asp:checkbox></td>
								<td nowrap class="fieldValue"><asp:label id="lblCloseOperationEdit" runat="server" Width="200px">���Ա�Ǹù����Ƿ�Ϊ��������</asp:label></td>
								<td nowrap width="100%"></td>
							</tr>
							<tr>
								<td nowrap><asp:checkbox id="chbPackEdit" runat="server" Width="100px" Text="��װ"></asp:checkbox></td>
								<td nowrap class="fieldValue"><asp:label id="lblPackingEdit" runat="server" Width="200px">���Ա�Ǹù����Ƿ�Ϊ��װ����</asp:label></td>
								<td nowrap width="100%"></td>
							</tr>
							<tr>
								<td nowrap><asp:checkbox id="chbIDMergeEdit" runat="server" Text="���̿��ű任" AutoPostBack="True"></asp:checkbox></td>
							</tr>
							<tr>
								<td nowrap class="fieldValue"><asp:label id="lblMNIDMergeEdit" runat="server">���Ա�Ǹù������̿��Ƿ�任</asp:label></td>
								<td nowrap colspan="2"><asp:panel id="pnlMainEdit" runat="server">
										<TABLE height="100%" width="100%">
											<TR>
												<TD noWrap>
													<asp:label id="lblMergeTypeEdit" runat="server" CssClass="ASPLAbType"> ת������</asp:label></TD>
												<TD>
													<asp:dropdownlist id="drpMergeTypeEdit" runat="server" Width="150px" Height="18px"></asp:dropdownlist></TD>
												<TD noWrap>
													<asp:label id="lblMergeRule" runat="server" CssClass="ASPLAbType"> ת������</asp:label></TD>
												<TD noWrap>
													<asp:label id="lblnumeratorEdit" runat="server" CssClass="ASPLAbType" Width="20px">1</asp:label>
													<asp:label id="Label16" runat="server" CssClass="ASPLAbType" Width="20px">:</asp:label>
													<asp:textbox id="txtDenominatorEdit" runat="server" Width="20px"></asp:textbox></TD>
											</TR>
										</TABLE>
									</asp:panel></td>
								<td nowrap width="100%"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td></td>
				</tr>
				<tr class="toolBar">
					<td>
						<table class="toolBar">
							<tr>
								<td class="toolBar"><INPUT class="submitImgButton" id="cmdSave" type="submit" value="�� ��" name="cmdSave" runat="server" onserverclick="cmdSave_ServerClick"></td>
								<td><INPUT class="submitImgButton" id="cmdReturn" type="submit" value="����" name="cmdReturn"
										runat="server" onserverclick="cmdReturn_ServerClick"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
