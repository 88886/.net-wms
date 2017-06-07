<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Page language="c#" Codebehind="FmaterialRation.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.BaseSetting.FmaterialRation" %>
<%@ Register TagPrefix="cc2" Namespace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<%@ Register TagPrefix="uc1" TagName="eMESDate" Src="~/UserControl/DateTime/DateTime/eMESDate.ascx" %>
<%@ Register TagPrefix="uc1" TagName="eMESTime" Src="~/UserControl/DateTime/DateTime/eMESTime.ascx" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FmaterialRation</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="<%=StyleSheet%>" rel=stylesheet>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table height="100%" width="100%">
				<tr class="moduleTitle">
					<td><asp:label id="lblTitle" runat="server" CssClass="labeltopic">����</asp:label></td>
				</tr>
				<tr>
					<td>
						<table class="query" height="100%" width="100%">
							<tr>
							<tr>
								<td class="fieldName" noWrap><asp:label id="lblPlanDateFrom" runat="server">�ƻ����ڴ�</asp:label></td>
								<td class="fieldValue"><uc1:eMESDate id="datePlanDateFromQuery" CssClass="textbox" width="130" runat="server"></uc1:eMESDate>
								</td>
								<td class="fieldName" noWrap><asp:label id="lblTo" runat="server">��</asp:label></td>
								<td class="fieldValue"><uc1:eMESDate id="datePlanDateToQuery" CssClass="textbox" width="130" runat="server"></uc1:eMESDate></td>
								<td class="fieldNameLeft" noWrap style="width: 67px"><asp:label id="lblBigSSCodeGroup" runat="server">����</asp:label></td>
								<td class="fieldValue">
								<cc2:SelectableTextBox ID="txtBigSSCodeGroupQuery" runat="server" Type="bigline" Readonly="false"
                                    CanKeyIn="true" Width="130px"></cc2:SelectableTextBox>
                                    </td>
								</tr>
								<td class="fieldNameLeft" noWrap><asp:label id="lblMOIDQuery" runat="server">��������</asp:label></td>
								<td class="fieldValue"><asp:TextBox id="txtMoQuery" runat="server" CssClass="textbox"></asp:TextBox></td>
								<td class="fieldNameLeft" noWrap><asp:label id="lblActionStatusQuery" runat="server">ִ��״̬</asp:label></td>
								<td class="fieldValue"><asp:DropDownList id="drpActionStatusQuery" runat="server" CssClass="textbox"  width="130" OnLoad="drpActionStatusQuery_Load"></asp:DropDownList></td>
								<td class="fieldNameLeft" noWrap style="width: 67px"><asp:label id="lblMaterialStatusQuery" runat="server">����״̬</asp:label></td>
								<td class="fieldValue"><asp:DropDownList id="drpMaterialStatusQuery" runat="server" CssClass="textbox"  width="130" OnLoad="drpMaterialStatusQuery_Load"></asp:DropDownList></td>
								<td class="fieldName"><INPUT class="submitImgButton" id="cmdQuery" type="submit" value="�� ѯ" name="btnQuery"
										runat="server"></td>
							</tr>
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
						</igtbl:ultrawebgrid></td>
				</tr>
				<tr class="normal">
					<td>
						<table height="100%" cellPadding="0" width="100%">
							<tr>
								<td><asp:checkbox id="chbSelectAll" runat="server" Text="ȫѡ" AutoPostBack="True"></asp:checkbox></td>
								<TD class="smallImgButton"><INPUT class="gridExportButton" id="cmdGridExport" type="submit" value="  " name="cmdGridExport"
										runat="server"> |
								</TD>
								
								<TD><cc1:pagersizeselector id="pagerSizeSelector" runat="server"></cc1:pagersizeselector></TD>
								<td align="right">
									<cc1:PagerToolbar id="pagerToolBar" runat="server"></cc1:PagerToolbar></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr class="normal">
					<td>
						<table class="edit" height="100%" cellPadding="0" width="100%">
							<tr>
								<td class="fieldNameLeft" noWrap style="height: 29px"><asp:label id="lblDate" runat="server">����</asp:label></td>
								<td class="fieldValue" style="height: 29px"><asp:TextBox id="txtdateDateEdit" CssClass="require" runat="server"  width="130" ReadOnly=true ></asp:TextBox></td>
								<td class="fieldName" noWrap style="height: 29px"><asp:label id="lblBigSSCodeGroupRequired" runat="server">����</asp:label></td>
								<td class="fieldValue" style="height: 29px"><asp:TextBox id="txtBigSSCodeGroupEdit" runat="server" Width="130px" CssClass="require"  ReadOnly=true></asp:TextBox></td>
								<td class="fieldNameLeft" style="height: 29px"><asp:label id="lblMOCodeGroup" runat="server">����</asp:label></td>
								<td class="fieldValue" style="height: 29px"><asp:TextBox id="txtMOCodeGroupEdit" runat="server" Width="130px" CssClass="require" ReadOnly=true></asp:TextBox></td>								
							</tr>
							<tr>
								<td class="fieldNameLeft" noWrap><asp:label id="lblMOSeqEdit" runat="server">�������</asp:label></td>
								<td class="fieldValue"><asp:TextBox id="txtMOSeqEdit" CssClass="require" runat="server" width="130px" ReadOnly=true></asp:TextBox></td>
								<td class="fieldName" noWrap><asp:label id="lblPromiseTimeEdit" runat="server">��ŵ����ʱ��</asp:label></td>
								<td class="fieldValue"><uc1:emestime id="timePromiseTimeEdit" CssClass="textbox" width="130" runat="server"></uc1:emestime>
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
								<INPUT class="submitImgButton" id="cmdOKSuit" type="submit" value="��������ȷ��" name="cmdOKSuit" runat="server" onserverclick="cmdOKSuit_ServerClick"></td>
								<td class="toolBar"><INPUT class="submitImgButton" id="cmdCancelSuit" type="submit" value="ȡ������" name="cmdCancelSuit" runat="server" onserverclick="cmdCancelSuit_ServerClick"></td>
								<td class="toolBar"><INPUT class="submitImgButton" id="cmdSave" type="submit" value="�� ��" name="cmdSave" runat="server"></td>
								<td><INPUT class="submitImgButton" id="cmdCancel" type="submit" value="ȡ ��" name="cmdCancel"
										runat="server"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>

