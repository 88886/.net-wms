<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Page language="c#" Codebehind="FTransPrintSP.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.WarehouseWeb.FTransPrintSP" %>
<%@ Register TagPrefix="uc1" TagName="eMESTime" Src="~/UserControl/DateTime/DateTime/eMESTime.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="<%=StyleSheet%>" rel=stylesheet>
		<script language="JavaScript">   
			var HKEY_Root,HKEY_Path,HKEY_Key; 
			HKEY_Root="HKEY_CURRENT_USER"; 
			HKEY_Path="\\Software\\Microsoft\\Internet Explorer\\PageSetup\\"; 
			//������ҳ��ӡ��ҳüҳ��Ϊ�� 
			function PageSetup_Null() 
			{ 
				try 
				{ 
					var Wsh=new ActiveXObject("WScript.Shell"); 
					HKEY_Key="header"; 
					Wsh.RegWrite(HKEY_Root+HKEY_Path+HKEY_Key,""); 
					HKEY_Key="footer"; 
					Wsh.RegWrite(HKEY_Root+HKEY_Path+HKEY_Key,""); 
					HKEY_Key="margin_bottom"; 
					Wsh.RegWrite(HKEY_Root+HKEY_Path+HKEY_Key,"0.35630"); 
					HKEY_Key="margin_left"; 
					Wsh.RegWrite(HKEY_Root+HKEY_Path+HKEY_Key,"0.35630"); 
					HKEY_Key="margin_right"; 
					Wsh.RegWrite(HKEY_Root+HKEY_Path+HKEY_Key,"0.35630"); 
					HKEY_Key="margin_top"; 
					Wsh.RegWrite(HKEY_Root+HKEY_Path+HKEY_Key,"0.35630"); 
				} 
				catch(e){} 
			} 
		</script>
		<script language="javascript">
			function PrintDocument()
			{
				PageSetup_Null();
				document.all.WebBrowser.ExecWB(7,1);
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table height="100%" width="100%">
				<tr class="moduleTitle">
					<td align="center"><asp:label id="lblTitle" runat="server" CssClass="labeltopic" Font-Size="Large"> ���µ��ӹɷ����޹�˾</asp:label>
						<br>
						<asp:Label Runat="server" ID="lblSubTitle" CssClass="labeltopic" Font-Size="Large"></asp:Label>
					</td>
				</tr>
				<tr>
					<td>
						<table width="100%">
							<tr>
								<td width="50%" align="left"><asp:label id="lblPrintTime" runat="server"> ��ʱ�䣺</asp:label><asp:Label Runat="server" ID="txtPrintTime"></asp:Label>
								</td>
								<td width="50%" align="right"><asp:Label ID="lblTicketNo" Runat="server"></asp:Label>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table class="query" height="100%" width="100%">
							<tr>
								<td noWrap><asp:label id="lblFactoryFrom" runat="server"> ��Դ������</asp:label>
									<asp:Label Runat="server" ID="txtFactoryFrom"></asp:Label>
								</td>
								<td noWrap><asp:label id="lblFactoryTo" runat="server"> Ŀ�깤����</asp:label>
									<asp:Label Runat="server" ID="txtFactoryTo"></asp:Label>
								</td>
							</tr>
							<tr>
								<td noWrap><asp:label id="lblWarehouseFrom" runat="server"> ��Դ�ֿ⣺</asp:label>
									<asp:Label Runat="server" ID="txtWarehouseFrom"></asp:Label>
								</td>
								<td noWrap><asp:label id="lblWarehouseTo" runat="server"> Ŀ��ֿ⣺</asp:label>
									<asp:Label Runat="server" ID="txtWarehouseTo"></asp:Label>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr height="100%">
					<td class="fieldGrid" valign="top">
						<asp:DataGrid Runat="server" ID="gridWebGrid" Width="100%" AutoGenerateColumns="False" BorderColor="Black"
							BorderStyle="Solid" BorderWidth="1px" CellPadding="0">
							<AlternatingItemStyle Font-Size="11pt" Wrap="False"></AlternatingItemStyle>
							<ItemStyle Font-Size="11pt" Wrap="False" BorderWidth="1px" BorderStyle="Solid" BorderColor="Black"></ItemStyle>
							<HeaderStyle Font-Size="12px" Font-Bold="True" HorizontalAlign="Center" BackColor="#ABABAB"></HeaderStyle>
							<Columns>
								<asp:BoundColumn HeaderText="No.">
									<HeaderStyle Width="40px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="���ϱ��">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="��������">
									<HeaderStyle Width="150px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="����">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="��������">
									<HeaderStyle Width="60px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="ʵ������">
									<HeaderStyle Width="60px"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:DataGrid>
					</td>
				</tr>
				<tr class="normal">
					<td>
						<table class="edit" height="100%" cellPadding="0" width="100%">
							<tr>
								<td><asp:Label Runat="server" ID="lblCreateUser">�Ʊ��ˣ�</asp:Label><asp:Label Runat="server" ID="txtCreateUser"></asp:Label>
								</td>
								<td><asp:Label Runat="server" ID="lblConfirmUser">����ˣ�</asp:Label><asp:Label Runat="server" ID="txtConfirmUser"></asp:Label>
								</td>
								<td><asp:Label Runat="server" ID="lblSendUser">�����ˣ�</asp:Label><asp:Label Runat="server" ID="txtSendUser"></asp:Label>
								</td>
								<td><asp:Label Runat="server" ID="lblReceiveUser">�����ˣ�</asp:Label><asp:Label Runat="server" ID="txtReceiveUser"></asp:Label>
								</td>
								<td><asp:Label Runat="server" ID="lblRecordUser">��¼�ˣ�</asp:Label><asp:Label Runat="server" ID="txtRecordUser"></asp:Label>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<OBJECT id="WebBrowser" height="0" width="0" classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"
				VIEWASTEXT>
				<PARAM NAME="ExtentX" VALUE="26">
				<PARAM NAME="ExtentY" VALUE="26">
				<PARAM NAME="ViewMode" VALUE="0">
				<PARAM NAME="Offline" VALUE="0">
				<PARAM NAME="Silent" VALUE="0">
				<PARAM NAME="RegisterAsBrowser" VALUE="0">
				<PARAM NAME="RegisterAsDropTarget" VALUE="1">
				<PARAM NAME="AutoArrange" VALUE="0">
				<PARAM NAME="NoClientEdge" VALUE="0">
				<PARAM NAME="AlignLeft" VALUE="0">
				<PARAM NAME="NoWebView" VALUE="0">
				<PARAM NAME="HideFileNames" VALUE="0">
				<PARAM NAME="SingleClick" VALUE="0">
				<PARAM NAME="SingleSelection" VALUE="0">
				<PARAM NAME="NoFolders" VALUE="0">
				<PARAM NAME="Transparent" VALUE="0">
				<PARAM NAME="ViewID" VALUE="{0057D0E0-3573-11CF-AE69-08002B2E1262}">
				<PARAM NAME="Location" VALUE="res://C:\WINDOWS\System32\shdoclc.dll/navcancl.htm#res://E:\WINDOWS\System32\shdoclc.dll/navcancl.htm#res://C:\WINNT\system32\shdoclc.dll/dnserror.htm#http:///">
			</OBJECT>
		</form>
		<script language="javascript">
	PrintDocument();
		</script>
	</body>
</HTML>
