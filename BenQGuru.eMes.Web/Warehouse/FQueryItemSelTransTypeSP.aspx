<%@ Page language="c#" Codebehind="FQueryItemSelTransTypeSP.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.WarehouseWeb.FQueryItemSelTransTypeSP" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>��������ѡ��</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="<%=StyleSheet%>" rel=stylesheet>
		<base target="_self">
		<script language="javascript">
				function ReturnValue()
				{
					var obj = new Object();
					scode = document.getElementById("txtSelectedItemCode").value;
					sname = document.getElementById("txtSelectedItemName").value;
					obj.code = scode;
					obj.name = sname;
					window.returnValue = obj;
					window.close();
				}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table width="100%" height="100%">
				<tr class="moduleTitle">
					<td><asp:label id="lblTitle" runat="server" CssClass="labeltopic"> ��������ѡ��</asp:label></td>
				</tr>
				<tr>
					<td align="center" valign="top">
						<table>
							<tr>
								<td>
									<asp:CheckBoxList Runat="server" ID="listTransType"></asp:CheckBoxList>
									<input type="hidden" runat="server" id="txtSelectedItemCode">
									<input type="hidden" runat="server" id="txtSelectedItemName">
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr class="toolBar">
					<td>
						<table class="toolBar">
							<tr>
								<td class="toolBar"><INPUT class="submitImgButton" id="cmdSave" type="submit" value="�� ��" name="cmdSave" runat="server" onserverclick="cmdSave_ServerClick"></td>
								<td><INPUT class="submitImgButton" id="cmdCancel" type="submit" value="ȡ ��" name="cmdCancel"
										runat="server" onclick="window.close();return false;"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
