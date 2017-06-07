<%@ Page language="c#" Codebehind="FOQCLRR2.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.WebQuery.FOQCLRR2" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="uc1" TagName="eMesTime" Src="~/UserControl/DateTime/DateTime/eMesTime.ascx" %>
<%@ Register TagPrefix="uc1" TagName="eMesDate" Src="~/UserControl/DateTime/DateTime/eMesDate.ascx" %>
<%@ Register TagPrefix="cc2" NameSpace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FOQCLRR2</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="<%=StyleSheet%>" rel=stylesheet>
		<script language="javascript">
			function onCheckBoxChange(cb)
			{
				if(cb.id == "cbdetail")
				{
					document.all.cbsample.checked = false;
				}
				else if(cb.id == "cbsample")
				{
					document.all.cbdetail.checked = false;
				}
			}
		</script>
	</HEAD>
	<body scroll="yes" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table id="Table1" height="100%" width="100%">
				<tr class="moduleTitle">
					<td>
						<table cellpadding="0" cellspacing="0">
							<tr>
								<td><img id="imgTitle" src="../skin/image/ico_arrow.gif" width="15" height="15">
								</td>
								<td><asp:label id="lblLRRTitle" runat="server" CssClass="Title2">OQC LRR</asp:label></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table class="query" id="Table2" height="100%" width="100%">
							<TR>
								<td class="fieldNameLeft" noWrap><asp:label id="lblItemCodeQuery" runat="server">��Ʒ����</asp:label></td>
								<td class="fieldValue"><cc2:selectabletextbox id="txtConditionItem" runat="server" Type="item" Width="90px"></cc2:selectabletextbox></td>
								<td class="fieldName" noWrap><asp:label id="lblModelCodeQuery" runat="server">��Ʒ�����</asp:label></td>
								<td class="fieldValue"><cc2:selectabletextbox id="txtConditionModel" runat="server" Type="model" Width="90px"></cc2:selectabletextbox></td>
								<td class="fieldName" noWrap><asp:label id="lblDateGroup" runat="server">ͳ�Ʒ�Χ</asp:label></td>
								<td class="fieldValue"><asp:dropdownlist id="drpDateGroup" runat="server" Width="120">
										<asp:ListItem Value="MDATE">��</asp:ListItem>
										<asp:ListItem Value="WEEK">��</asp:ListItem>
										<asp:ListItem Value="MONTH">��</asp:ListItem>
									</asp:dropdownlist></td>
								<td></td>
							</TR>
							<tr>
								<td class="fieldName" noWrap><asp:label id="lblOQCBegindate" runat="server">�����ʼ����</asp:label></td>
								<TD class="fieldValue" noWrap>
									<TABLE cellSpacing="0" cellPadding="0" border="0">
										<TR>
											<TD><uc1:emesdate id="txtOQCBeginDate" runat="server" CssClass="require" width="80"></uc1:emesdate></TD>
											<TD><uc1:emestime id="txtOQCBeginTime" runat="server" CssClass="require" width="60"></uc1:emestime></TD>
										</TR>
									</TABLE>
								</TD>
								<td class="fieldName" noWrap><asp:label id="lblOQCEnddt" runat="server">����������</asp:label></td>
								<TD class="fieldValue" noWrap>
									<TABLE cellSpacing="0" cellPadding="0" border="0">
										<TR>
											<TD><uc1:emesdate id="txtOQCEndDate" runat="server" CssClass="require" width="80"></uc1:emesdate></TD>
											<TD><uc1:emestime id="txtOQCEndTime" runat="server" CssClass="require" width="60"></uc1:emestime></TD>
										</TR>
									</TABLE>
								</TD>
								<td class="fieldName" noWrap><asp:label id="lblTypeQuery" runat="server">ͳ������</asp:label></td>
								<td class="fieldValue"><asp:dropdownlist id="drpType" runat="server" Width="120">
										<asp:ListItem Value="NORMAL">һ���ͼ�</asp:ListItem>
										<asp:ListItem Value="RELOT">�ؼ�</asp:ListItem>
										<asp:ListItem Value="REWORKLOT">�����ͼ�</asp:ListItem>
										<asp:ListItem Value="TRYLOT">�������ͼ�</asp:ListItem>
									</asp:dropdownlist></td>
								<td></td>
								<td class="fieldName"><INPUT class="submitImgButton" id="cmdQuery" type="submit" value="�� ѯ" name="btnQuery"
										runat="server">
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr height="150">
					<td class="fieldGrid" vAlign="top"><DISPLAYLAYOUT ColWidthDefault="" StationaryMargins="Header" AllowSortingDefault="Yes" RowHeightDefault="20px"
							Version="2.00" SelectTypeRowDefault="Single" SelectTypeCellDefault="Single" HeaderClickActionDefault="SortSingle" BorderCollapseDefault="Separate"
							AllowColSizingDefault="Free" CellPaddingDefault="4" RowSelectorsDefault="No" Name="webGrid" TableLayout="Fixed">
							<igtbl:ultrawebgrid id="gridWebGrid" runat="server" Width="100%" Height="100%">
								<DisplayLayout ColWidthDefault="" StationaryMargins="Header" AllowSortingDefault="Yes" RowHeightDefault="20px"
									Version="2.00" SelectTypeRowDefault="Single" SelectTypeCellDefault="Single" HeaderClickActionDefault="SortSingle"
									BorderCollapseDefault="Separate" AllowColSizingDefault="Free" CellPaddingDefault="4" RowSelectorsDefault="No"
									Name="webGrid" TableLayout="Fixed">
									<AddNewBox>
										<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">

<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White">
</BorderDetails>

										</Style>
									</AddNewBox>
									<Pager>
										<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">

<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White">
</BorderDetails>

										</Style>
									</Pager>
									<HeaderStyleDefault BorderWidth="1px" Font-Size="12px" Font-Bold="True" BorderColor="White" BorderStyle="Dashed"
										HorizontalAlign="Left" BackColor="#ABABAB">
										<BorderDetails ColorTop="White" WidthLeft="1px" ColorBottom="White" WidthTop="1px" ColorRight="White"
											ColorLeft="White"></BorderDetails>
									</HeaderStyleDefault>
									<RowSelectorStyleDefault BackColor="#EBEBEB"></RowSelectorStyleDefault>
									<FrameStyle Width="100%" BorderWidth="0px" Font-Size="12px" Font-Names="Verdana" BorderColor="#ABABAB"
										BorderStyle="Groove" Height="100%"></FrameStyle>
									<FooterStyleDefault BorderWidth="0px" BorderStyle="Groove" BackColor="LightGray">
										<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
									</FooterStyleDefault>
									<ActivationObject BorderStyle="Dotted"></ActivationObject>
									<EditCellStyleDefault VerticalAlign="Middle" BorderWidth="1px" BorderColor="Black" BorderStyle="None">
										<Padding Bottom="1px"></Padding>
									</EditCellStyleDefault>
									<RowAlternateStyleDefault BackColor="White"></RowAlternateStyleDefault>
									<RowStyleDefault VerticalAlign="Middle" BorderWidth="1px" BorderColor="#D7D8D9" BorderStyle="Solid"
										HorizontalAlign="Left">
										<Padding Left="3px"></Padding>
										<BorderDetails WidthLeft="0px" WidthTop="0px"></BorderDetails>
									</RowStyleDefault>
									<ImageUrls ImageDirectory="/ig_common/WebGrid3/"></ImageUrls>
								</DisplayLayout>
								<Bands>
									<igtbl:UltraGridBand></igtbl:UltraGridBand>
								</Bands>
							</igtbl:ultrawebgrid>
							<ADDNEWBOX>
								<STYLE BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
								</STYLE>
							</ADDNEWBOX>
							<PAGER>
								<STYLE BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
								</STYLE>
							</PAGER>
							<HEADERSTYLEDEFAULT BorderWidth="1px" BorderStyle="Dashed" BackColor="#ABABAB" Font-Size="12px" Font-Bold="True"
								BorderColor="White" HorizontalAlign="Left">
								<BORDERDETAILS ColorTop="White" WidthLeft="1px" ColorBottom="White" WidthTop="1px" ColorRight="White"
									ColorLeft="White"></BORDERDETAILS>
							</HEADERSTYLEDEFAULT>
							<ROWSELECTORSTYLEDEFAULT BackColor="#EBEBEB"></ROWSELECTORSTYLEDEFAULT>
							<FRAMESTYLE Width="100%" Height="100%" BorderWidth="0px" BorderStyle="Groove" Font-Size="12px"
								BorderColor="#ABABAB" Font-Names="Verdana"></FRAMESTYLE>
							<FOOTERSTYLEDEFAULT BorderWidth="0px" BorderStyle="Groove" BackColor="LightGray">
								<BORDERDETAILS ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BORDERDETAILS>
							</FOOTERSTYLEDEFAULT>
							<ACTIVATIONOBJECT BorderStyle="Dotted"></ACTIVATIONOBJECT>
							<EDITCELLSTYLEDEFAULT BorderWidth="1px" BorderStyle="None" BorderColor="Black" VerticalAlign="Middle">
								<PADDING Bottom="1px"></PADDING>
							</EDITCELLSTYLEDEFAULT>
							<ROWALTERNATESTYLEDEFAULT BackColor="White"></ROWALTERNATESTYLEDEFAULT>
							<ROWSTYLEDEFAULT BorderWidth="1px" BorderStyle="Solid" BorderColor="#D7D8D9" HorizontalAlign="Left"
								VerticalAlign="Middle">
								<PADDING Left="3px"></PADDING>
								<BORDERDETAILS WidthLeft="0px" WidthTop="0px"></BORDERDETAILS>
							</ROWSTYLEDEFAULT>
							<IMAGEURLS ImageDirectory="/ig_common/WebGrid3/"></IMAGEURLS>
						</DISPLAYLAYOUT><BANDS><IGTBL:ULTRAGRIDBAND></IGTBL:ULTRAGRIDBAND>
						</BANDS></td>
				</tr>
				<tr class="normal">
					<td>
						<table id="Table3" height="100%" cellPadding="0" width="100%">
							<tr>
								<TD class="smallImgButton"><INPUT class="gridExportButton" id="cmdGridExport" type="submit" value="  " name="cmdGridExport"
										runat="server"> |
								</TD>
								<TD width="140"><cc1:pagersizeselector id="pagerSizeSelector" runat="server"></cc1:pagersizeselector></TD>
								<td align="right">
									<cc1:PagerToolbar id="pagerToolBar" runat="server"></cc1:PagerToolbar>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr style="DISPLAY:none">
					<td>
						<table>
							<tr>
								<td colSpan="8"><cc1:owcpivottable id="OWCPivotTable1" runat="server"></cc1:owcpivottable></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table>
							<tr>
								<td colspan="8">
									<cc1:OWCChartSpace id="OWCChartSpace1" runat="server"></cc1:OWCChartSpace>
								</td>
								<td valign="top">
									<table border="0">
										<tr>
											<td class="fieldNameLeft"><asp:Label Runat="server" ID="lblLotTotalCount" Font-Bold="True">���LOT�ܼ�</asp:Label></td>
											<td><asp:Label Runat="server" ID="lblLotTotalCountValue"></asp:Label></td>
										</tr>
										<tr>
											<td class="fieldNameLeft"><asp:Label Runat="server" ID="lblLotRejectCount" Font-Bold="True">����LOT�ܼ�</asp:Label></td>
											<td><asp:Label Runat="server" ID="lblLotRejectCountValue"></asp:Label></td>
										</tr>
										<tr>
											<td class="fieldNameLeft"><asp:Label Runat="server" ID="lblLRR" Font-Bold="True">LRR ����</asp:Label></td>
											<td><asp:Label Runat="server" ID="lblLRRValue"></asp:Label></td>
										</tr>
										<tr>
											<td class="fieldNameLeft"><asp:Label Runat="server" ID="lblLotSampleCount" Font-Bold="True">�����ܼ�</asp:Label></td>
											<td><asp:Label Runat="server" ID="lblLotSampleCountValue"></asp:Label></td>
										</tr>
										<tr>
											<td class="fieldNameLeft"><asp:Label Runat="server" ID="lblLotSampleNGCount" Font-Bold="True">�����ܼ�</asp:Label></td>
											<td><asp:Label Runat="server" ID="lblLotSampleNGCountValue"></asp:Label></td>
										</tr>
										<tr>
											<td class="fieldNameLeft"><asp:Label Runat="server" ID="lblDPPM" Font-Bold="True">DPPM ����</asp:Label></td>
											<td><asp:Label Runat="server" ID="lblDPPMValue"></asp:Label></td>
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
</HTML>
