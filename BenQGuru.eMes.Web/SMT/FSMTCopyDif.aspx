<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics.WebUI.UltraWebGrid.v3.1, Version=3.1.20042.26, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page language="c#" Codebehind="FSMTCopyDif.aspx.cs" AutoEventWireup="false" Inherits="BenQGuru.eMES.Web.SMT.FSMTCopyDif" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FSMTCopyDif</title>
		<base target="_self">
		<meta http-equiv="pragma" content="no-cache">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="<%=StyleSheet%>" rel=stylesheet>
		<script language="javascript">
			
			function Close()
			{
				returnValue = 'false';
				window.close();
			}
			function FClose()
			{
				returnValue = 'true';
				window.close();
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table height="100%" width="100%">
				<tr class="moduleTitle" valign=top>
					<td>
						<table  cellPadding="0" width="100%">
							<tr>
								<td><asp:label id="lblCopyAlert" runat="server" CssClass="labeltopic"></asp:label></td>
							</tr>
							<tr class="moduleTitle" valign=top height=30px>
								<td><asp:label id="Label1" runat="server" CssClass="labeltopic">(��ѡ��ʾ����)</asp:label></td>
							</tr>
						</table>
					</td>
				</tr>
				
				<tr style="DISPLAY: none" valign=top>
					<td class="fieldGrid"><igtbl:ultrawebgrid id="gridWebGrid" runat="server" Width="100%" Height="100%"><DISPLAYLAYOUT TableLayout="Fixed" Name="webGrid" RowSelectorsDefault="No" CellPaddingDefault="4"
								AllowColSizingDefault="Free" BorderCollapseDefault="Separate" HeaderClickActionDefault="SortSingle" SelectTypeCellDefault="Single" SelectTypeRowDefault="Single" Version="2.00" RowHeightDefault="20px"
								AllowSortingDefault="Yes" StationaryMargins="Header" ColWidthDefault=""><ADDNEWBOX>
									<STYLE BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px"></STYLE>
								</ADDNEWBOX>
								<PAGER>
									<STYLE BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px"></STYLE>
								</PAGER>
								<HEADERSTYLEDEFAULT BackColor="#ABABAB" BorderStyle="Dashed" BorderWidth="1px" HorizontalAlign="Left"
									BorderColor="White" Font-Bold="True" Font-Size="12px">
									<BORDERDETAILS ColorLeft="White" ColorRight="White" WidthTop="1px" ColorBottom="White" WidthLeft="1px"
										ColorTop="White"></BORDERDETAILS>
								</HEADERSTYLEDEFAULT>
								<ROWSELECTORSTYLEDEFAULT BackColor="#EBEBEB"></ROWSELECTORSTYLEDEFAULT>
								<FRAMESTYLE Height="100%" Width="100%" BorderStyle="Groove" BorderWidth="0px" BorderColor="#ABABAB"
									Font-Size="12px" Font-Names="Verdana"></FRAMESTYLE>
								<FOOTERSTYLEDEFAULT BackColor="LightGray" BorderStyle="Groove" BorderWidth="0px">
									<BORDERDETAILS ColorLeft="White" WidthTop="1px" WidthLeft="1px" ColorTop="White"></BORDERDETAILS>
								</FOOTERSTYLEDEFAULT>
								<ACTIVATIONOBJECT BorderStyle="Dotted"></ACTIVATIONOBJECT>
								<EDITCELLSTYLEDEFAULT BorderStyle="None" BorderWidth="1px" BorderColor="Black" VerticalAlign="Middle">
									<PADDING Bottom="1px"></PADDING>
								</EDITCELLSTYLEDEFAULT>
								<ROWALTERNATESTYLEDEFAULT BackColor="White"></ROWALTERNATESTYLEDEFAULT>
								<ROWSTYLEDEFAULT BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" BorderColor="#D7D8D9"
									VerticalAlign="Middle">
									<PADDING Left="3px"></PADDING>
									<BORDERDETAILS WidthTop="0px" WidthLeft="0px"></BORDERDETAILS>
								</ROWSTYLEDEFAULT>
								<IMAGEURLS ImageDirectory="/ig_common/WebGrid3/"></IMAGEURLS>
							</DISPLAYLAYOUT>
							<BANDS>
								<igtbl:UltraGridBand></igtbl:UltraGridBand>
							</BANDS>
						</igtbl:ultrawebgrid></td>
				</tr>
				<tr class="normal" style="DISPLAY: none" valign=top>
					<td>
						<table height="100%" cellPadding="0" width="100%">
							<tr>
								<td><FONT face="����"><asp:checkbox id="chbSelectAll" runat="server" Width="124px" Text="ȫѡ"></asp:checkbox></FONT></td>
								<td width="100%"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr class="toolBar"  valign=top>
					<td>
						<table class="query">
							<tr vAlign="top">
								<td class="fieldName" noWrap colspan=""><asp:checkboxlist id="chklstOPControlEdit" runat="server" Width="100%" RepeatColumns="3" RepeatDirection="Horizontal"></asp:checkboxlist></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr valign=top><td></td></tr>
				<tr class="toolBar" valign=baseline height="20%">
					<td>
						<table class="toolBar">
							<tr>
								<td class="toolBar"><INPUT class="submitImgButton" id="cmdCopy" type="submit" value="�� ��" name="cmdCopy" runat="server"></td>
								<td><INPUT class="submitImgButton" id="cmdReturn" type="button" value="�� ��" name="cmdReturn" onclick="Close();"
										runat="server"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
