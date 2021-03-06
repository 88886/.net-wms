<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register TagPrefix="cc2" NameSpace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<%@ Page language="c#" Codebehind="FTSChangedItemQP2.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.WebQuery.FTSChangedItemQP2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FINNOInfoQP</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="<%=StyleSheet%>" rel=stylesheet>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form2" method="post" runat="server">
			<table height="100%" width="100%" id="Table1">
				<tr class="moduleTitle">
					<td style="HEIGHT: 19px">
						<table cellpadding="0" cellspacing="0">
							<tr>
								<td><img id="imgTitle" src="../skin/image/ico_arrow.gif" width="15" height="15">
								</td>
								<td><asp:label id="lblRepairChangeInfo" runat="server" CssClass="labeltopic">维修换料信息</asp:label></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table class="query" height="100%" width="100%" id="Table2">
							<TR>
								<td class="fieldNameLeft" noWrap>
									<asp:label id="lblModelQuery" runat="server">产品别</asp:label>
								</td>
								<td class="fieldValue">
									<asp:TextBox id="txtModelQuery" runat="server" CssClass="textbox" ReadOnly="True"></asp:TextBox>
								</td>
								<td class="fieldName" noWrap>
									<asp:label id="lblItemQuery" runat="server">产品</asp:label>
								</td>
								<td class="fieldValue">
									<asp:TextBox id="txtItemQuery" runat="server" CssClass="textbox" ReadOnly="True"></asp:TextBox>
								</td>
								<td class="fieldName" noWrap>
									<asp:label id="lblMOQuery" runat="server">工单</asp:label>
								</td>
								<td class="fieldValue">
									<asp:TextBox id="txtMoQuery" runat="server" CssClass="textbox" ReadOnly="True"></asp:TextBox>
								</td>
								<td class="fieldName" noWrap>
									<asp:label id="lblSNQuery" runat="server">序列号</asp:label>
								</td>
								<td class="fieldValue">
									<asp:TextBox id="txtSnQuery" runat="server" CssClass="textbox" ReadOnly="True" Width="165px"></asp:TextBox>
								</td>
							</TR>
							<TR>
								<td class="fieldNameLeft" noWrap>
									<asp:label id="lblTSStateQuery" runat="server">维修状态</asp:label>
								</td>
								<td class="fieldValue">
									<asp:TextBox id="txtTsStateQuery" runat="server" CssClass="textbox" ReadOnly="True"></asp:TextBox>
								</td>
								<td class="fieldName" noWrap>
									<asp:label id="lblRepaireOperationQuery" runat="server">维修站</asp:label>
								</td>
								<td class="fieldValue">
									<asp:TextBox id="txtRepaireResourceQuery" runat="server" CssClass="textbox" ReadOnly="True"></asp:TextBox>
								</td>
								<td></td>
								<td></td>
								<td></td>
							</TR>
						</table>
					</td>
				</tr>
				<tr height="100%">
					<td class="fieldGrid">
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
							</igtbl:ultrawebgrid><DISPLAYLAYOUT TableLayout="Fixed" Name="webGrid" RowSelectorsDefault="No" CellPaddingDefault="4"
							AllowColSizingDefault="Free" BorderCollapseDefault="Separate" HeaderClickActionDefault="SortSingle" SelectTypeCellDefault="Single"
							SelectTypeRowDefault="Single" Version="2.00" RowHeightDefault="20px" AllowSortingDefault="Yes" StationaryMargins="Header" ColWidthDefault=""><ADDNEWBOX>
								<STYLE BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
								</STYLE>
							</ADDNEWBOX>
							<PAGER>
								<STYLE BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
								</STYLE>
							</PAGER>
							<HEADERSTYLEDEFAULT BackColor="#ABABAB" BorderStyle="Dashed" BorderWidth="1px" HorizontalAlign="Left"
								BorderColor="White" Font-Bold="True" Font-Size="12px">
								<BORDERDETAILS ColorLeft="White" ColorRight="White" WidthTop="1px" ColorBottom="White" WidthLeft="1px"
									ColorTop="White"></BORDERDETAILS>
							</HEADERSTYLEDEFAULT>
							<ROWSELECTORSTYLEDEFAULT BackColor="#EBEBEB"></ROWSELECTORSTYLEDEFAULT>
							<FRAMESTYLE Width="100%" Height="100%" BorderStyle="Groove" BorderWidth="0px" BorderColor="#ABABAB"
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
						</DISPLAYLAYOUT><BANDS>
							<IGTBL:ULTRAGRIDBAND></IGTBL:ULTRAGRIDBAND>
						</BANDS></td>
				</tr>
				<tr class="normal">
					<td>
						<table height="100%" cellPadding="0" width="100%" id="Table3">
							<tr>
								<TD class="smallImgButton"><INPUT class="gridExportButton" id="cmdGridExport" type="submit" value="  " name="cmdGridExport"
										runat="server"> |
								</TD>
								<TD>
									<cc1:pagersizeselector id="pagerSizeSelector" runat="server"></cc1:pagersizeselector>
								</TD>
								<td align="right">
									<cc1:PagerToolbar id="pagerToolBar" runat="server"></cc1:PagerToolbar>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr class="toolBar">
					<td>
						<table class="toolBar" id="Table4" width="100%">
							<tr>
								<td class="toolBar" align="center"><INPUT class="submitImgButton" id="cmdReturn" type="submit" value="返 回" name="cmdReturn"
										runat="server" onserverclick="cmdReturn_ServerClick"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
