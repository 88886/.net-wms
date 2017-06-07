<%@ Page language="c#" Codebehind="FSMTImport2.aspx.cs" AutoEventWireup="false" Inherits="BenQGuru.eMES.Web.SMT.FSMTImport2" %>
<%@ Register TagPrefix="cc2" NameSpace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics.WebUI.UltraWebGrid.v3.1, Version=3.1.20042.26, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics.WebUI.UltraWebNavigator.v3.2, Version=3.2.20042.26, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MPageSmtLoading</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="<%=StyleSheet%>" rel=stylesheet>
		<script language="jscript">
			//���Ƶļ��
			function CheckIfCopy()
			{
				if(event.keyCode==13)return false;
				var txtmocode = document.getElementById('txtMoQuery:_ctl0');
				if(txtmocode.value == '' || txtmocode.value == '0')
				{
					alert("��ѡ���ά������");
					return false;
				}
				if(document.all.drpMOCopySourceQuery.value == '' || document.all.drpMOCopySourceQuery.value == '0')
				{
					alert("��ѡ����Դ����");
					return false;
				}
			}
			function CheckImport()
			{
				var txtmocode = document.getElementById('txtMoQuery:_ctl0');
				if(txtmocode.value == '' || txtmocode.value == '0')
				{
					alert("��ѡ���ά������");
					return false;
				}
				if( ''==document.all.fileExcel.value )
				{
					alert("��ѡ��Ҫ�����վ���ļ�");
					return false;
				}
			}
			
			function CheckCompare()
			{
				if("comparesourcetype_excel" == document.all.hidComparesource.value)
				{
					if( ''==document.all.FileMOItem.value )
					{
						alert("��ѡ��Ҫ�ȶԵ������嵥");
						return false;
					}
				}
				//�����嵥����mocode �� opcode��ȡ
				if( ''==document.all.fileExcel.value )
				{
					alert("��ѡ��Ҫ�ȶԵ�վ���ļ�");
					return false;
				}
				SetImportBtnEnable();	//�������
				document.all.hidifImportUse.value = 'true';
			}
			
			function OnCheckBom()
			{
				if(document.all.CheckboxBOM.checked)
				{
					document.all.compareBomTR.style.display = 'block';
					document.all.compareSourceTR.style.display = 'block';
				}
				else
				{
					document.all.compareBomTR.style.display = 'none';
					document.all.compareSourceTR.style.display = 'none';
				}
				BOMCheckBoxChange();
				OnMOBOMSourceChange();
			}
			
			function OnMOBOMSourceChange()
			{
				if("comparesourcetype_db" == document.all.hidComparesource.value)
				{
					document.all.trDB.style.display = 'block';
					document.all.trExcel.style.display = 'none';
				}
				else if("comparesourcetype_excel" == document.all.hidComparesource.value)
				{
					document.all.trDB.style.display = 'none';
					document.all.trExcel.style.display = 'block';
				}
				else
				{
					document.all.trDB.style.display = 'block';
					document.all.trExcel.style.display = 'none';
				}
			}
		
			//�ȶ�BOM��CheckBox�ı�
			function BOMCheckBoxChange()
			{
				if(document.all.CheckboxBOM.checked == true)
				{
					SetBOMBtnEnable()		 //�ȶ�BOM����
					if(document.all.hidifImportUse.value!='true')
					{SetImportBtnDisabled();  }//���벻����
				}
				else
				{
					SetBOMBtnDisabled();	//�ȶ�BOM������
					SetImportBtnEnable();	//�������
				}
			}
			
			
			//�ȶ�BOM��ť����
			//���ñȶ�BOM����
			function SetBOMBtnEnable()
			{
				document.all.cmdCompare.disabled = false;
				SetImportBtnEnable();
			}
			//���ñȶ�BOM������
			function SetBOMBtnDisabled()
			{
				document.all.cmdCompare.disabled = true;
				SetImportBtnEnable();
			}
			
			//���밴ť����
			//���õ�����ÿ���
			function SetImportBtnEnable()
			{
				document.all.cmdImport.disabled = false;
			}
			//���õ��벻����
			function SetImportBtnDisabled()
			{
				document.all.cmdImport.disabled = true;
			}
			
			
			//����ֻ��
			function SetReadOnly(thischeckbox)
			{
				var txtID = GetTxtID(thischeckbox);
				if(''!=txtID){
					document.getElementById(txtID).value ='';
					document.getElementById(txtID).readOnly = true;
				}
				
			}
			//ȡ��ֻ��
			function CancleReadOnly(thischeckbox)
			{
				var txtID = GetTxtID(thischeckbox);
				if(''!=txtID){document.getElementById(txtID).readOnly = false;}
			}
			
			function OnCheck(thischeckbox)
			{
				if(true == thischeckbox.checked){ 
					CancleReadOnly(thischeckbox);
				}
				else{
					SetReadOnly(thischeckbox);
				}
			}
			
			function GetTxtID(thischeckbox)
			{
				var returnTxtID="";
				switch(thischeckbox.id)
				{
					//case "chbFeederEdit":
					//	returnTxtID = "txtFeederEdit";
					//	break;
					case "chbSupplierItemEdit":
						returnTxtID = "txtSupplierItemEdit";
						break;
					case "chbLotNOEdit":
						returnTxtID = "txtLotNOEdit";
						break;
					case "chbSupplyCodeEdit":
						returnTxtID = "txtSupplyCodeEdit";
						break;
					case "chbDateCodeEdit":
						returnTxtID = "txtDateCodeEdit";
						break;
					case "chbPCBAEdit":
						returnTxtID = "txtPCBAEdit";
						break;
					case "chbBIOSEdit":
						returnTxtID = "txtBIOSEdit";
						break;
					case "chbVersionEdit":
						returnTxtID = "txtVersionEdit";
						break;
						
				}
				return returnTxtID;
			}
			
			function SetTreeChecked(ifchecked)
			{
				var tree = igtree_getTreeById('treeWebTree');
				var nodes = tree.getNodes();
				for(var i=0;i<nodes.length;i++)
				{
					nodes[i].setChecked(ifchecked);
				}
			}
			
			function OnchbExportByResCheck()
			{
				if(document.all.chbExportByRes.checked)
				{
					SetTreeChecked(false);
				}
				else
				{
					SetTreeChecked(true);
				}
			}
			
			function popCopyPage()
			{
				var RValue = window.showModalDialog("FSMTCopyDif.aspx","","scroll:0;status:0;help:0;resizable:1;dialogWidth:420px;dialogHeight:300px");
				if(RValue!= null && RValue=='true')
				{
					//�˴�ˢ��ҳ��,�������ؿؼ���click�¼�,refleshҳ��
					document.all.cmdFreshTree.click();
				}
				return false;
			}
			
			function popDifferentBomPage()
			{
				var RValue = window.showModalDialog("FSMTBOMCompareDif.aspx","","scroll:0;status:0;help:0;resizable:1;dialogWidth:1024px;dialogHeight:768px");
				//document.all.cmdFreshTree.click();
				return false;
			}
			function popErrorPage()
			{
				window.showModalDialog("FSMTImportError.aspx","","scroll:0;status:0;help:0;resizable:1;dialogWidth:800px;dialogHeight:600px");
				return false;
			}
			/*
			function popSelectrPage()
			{
				//var returnMOValue = window.showModalDialog("FSMTSelectMO.aspx","","scroll:0;status:0;help:0;resizable:1;dialogWidth:900px;dialogHeight:600px");
				var returnMOValue = window.showModalDialog("FSMTSelectMO.aspx?mocode="+escape(document.all.txtMOCode.value),"","scroll:0;status:0;help:0;resizable:1;dialogWidth:900px;dialogHeight:600px");
				if(returnMOValue!=null && returnMOValue!='')
				{
					document.all.txtMOCode.value = returnMOValue;
					document.all.hidtxtMOCode.value = returnMOValue;
					//ˢ��ҳ��
					document.all.cmdchangeMO.click();
				}
				return false;
			}
			*/
			function L_popSelectrPage()
			{
				//ˢ��ҳ��
				document.all.cmdchangeMO.click();
				return false;
			
			}
			
			function Init()
			{
				OnCheckBom();
				var txtmocode = document.getElementById('txtMoQuery:_ctl0');
				txtmocode.focus();
			}
			function OnKeyPress()
			{
				if('txtMoQuery:_ctl0'== event.srcElement.name  && 13 == window.event.keyCode )
				{
					window.event.keyCode  = 0;
					var btnmo = document.getElementById('txtMoQuery:_ctl2');
					btnmo.click();
					L_popSelectrPage();
					
				}
				if(13 == window.event.keyCode){window.event.keyCode  = 0;}
				
			}
			
			
			
		</script>
	</HEAD>
	<body onkeypress="OnKeyPress()" bottomMargin="0" leftMargin="0" topMargin="0" scroll="yes"
		onload="Init()" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 0px; WIDTH: 100%; POSITION: absolute; TOP: 0px; HEIGHT: 100%"
				cellSpacing="0" cellPadding="0" width="300" border="0">
				<TBODY>
					<TR>
						<TD style="PADDING-LEFT: 8px; HEIGHT: 20px"><asp:label id="lblTitle" runat="server" CssClass="labeltopic">SMT���Ϸ���ά��</asp:label><asp:textbox id="txtOperationCode" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtResourceCode" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtItemCodeQuery" runat="server" Visible="False"></asp:textbox><asp:textbox id="txtRouteCode" runat="server" Visible="False"></asp:textbox></TD>
					</TR>
					<TR>
						<TD style="PADDING-LEFT: 8px; HEIGHT: 20px">
							<TABLE class="query" id="Table13" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TBODY>
									<tr>
						</TD>
						<td colSpan="8"></td>
					</TR>
					<TR>
						<td class="fieldNameLeft" noWrap><asp:label id="lblMoQuery" runat="server">��������</asp:label></td>
						<td class="fieldValue"><cc2:selectabletextbox id="txtMoQuery" runat="server" Width="120px" Type="mo"></cc2:selectabletextbox></td>
						<TD style="DISPLAY: none" noWrap><FONT face="����"><asp:dropdownlist id="drpMOCode" runat="server" Width="120px" AutoPostBack="True"></asp:dropdownlist></FONT></TD>
						<TD class="fieldName" noWrap><asp:label id="lblItemCodeQuery" runat="server">��Ʒ����</asp:label></TD>
						<TD noWrap><asp:textbox id="txtItemCode" runat="server" CssClass="textbox" Width="120px" ReadOnly="True"></asp:textbox></TD>
						<TD class="fieldName" noWrap><FONT face="����"><asp:label id="lblMOCopySourceQuery" runat="server"> ��Դ����</asp:label></FONT></TD>
						<TD noWrap><FONT face="����"><asp:dropdownlist id="drpMOCopySourceQuery" runat="server" Width="130px" AutoPostBack="True"></asp:dropdownlist></FONT></TD>
						<TD class="fieldName" noWrap><FONT face="����"><asp:label id="Label2" runat="server">ѡ���̨</asp:label></FONT></TD>
						<TD noWrap><FONT face="����"><asp:dropdownlist id="drpResource" runat="server" Width="130px"></asp:dropdownlist></FONT></TD>
						<TD class="fieldName" noWrap><FONT face="����"><INPUT class="submitImgButton" id="cmdCopy" type="submit" value="�� ��" name="cmdCopy" runat="server"></FONT></TD>
					</TR>
				</TBODY>
			</TABLE>
			</TD></TR>
			<TR>
				<TD style="PADDING-LEFT: 8px; HEIGHT: 20px">
					<TABLE class="query" cellSpacing="0" cellPadding="0" width="100%">
						<TR>
							<TD class="fieldValue">&nbsp;&nbsp;&nbsp;&nbsp;<INPUT id="CheckboxBOM" onclick="OnCheckBom()" type="checkbox" CHECKED name="CheckboxBOM"
									runat="server">�ȶ�BOM</TD>
							<td></td>
						</TR>
						<TR>
							<TD class="fieldName" noWrap><asp:label id="Label5" runat="server"> ѡ��վ�����ļ�</asp:label></TD>
							<TD class="fieldValue"><INPUT class="textStyle" id="fileExcel" type="file" size="100" name="fileExcel" runat="server"></TD>
							<TD class="fieldName"><FONT face="����"><INPUT class="submitImgButton" id="cmdImport" type="submit" value="�� ��" name="cmdImport"
										runat="server"></FONT></TD>
						</TR>
						<tr id="compareSourceTR">
							<td class="fieldName"><asp:label id="lblVisibleStyle" runat="server">�ȶԹ���BOM��Դ</asp:label></td>
							<td colSpan="2"><asp:radiobuttonlist id="rblMOBOMSourceSelect" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"></asp:radiobuttonlist></td>
						</tr>
						<TR id="compareBomTR">
							<td colSpan="2">
								<table>
									<tr id="trDB">
										<TD class="fieldName" noWrap><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="lblSapOpcode" runat="server">SAP�������</asp:label></FONT></TD>
										<TD noWrap><asp:textbox id="txtSapOPCode" runat="server" CssClass="textbox" Width="120px"></asp:textbox></TD>
									</tr>
									<tr id="trExcel">
										<TD class="fieldName" noWrap><FONT face="����">&nbsp;&nbsp; </FONT>
											<asp:label id="Label7" runat="server">���������嵥�ļ�</asp:label></TD>
										<TD class="fieldValue"><INPUT class="textStyle" id="FileMOItem" type="file" size="100" name="fileExcel" runat="server"></TD>
									</tr>
								</table>
							</td>
							<TD class="fieldName"><INPUT class="submitImgButton" id="cmdCompare" type="submit" value="�ȶ�BOM" name="cmdCompare"
									runat="server"></TD>
						</TR>
					</TABLE>
				</TD>
			</TR>
			<TR>
				<TD>
					<TABLE id="Table2" style="PADDING-RIGHT: 4px; PADDING-LEFT: 4px; PADDING-BOTTOM: 4px; WIDTH: 100%; PADDING-TOP: 4px; HEIGHT: 100%"
						cellSpacing="0" cellPadding="0" width="300" border="0">
						<TBODY>
							<TR>
								<TD style="PADDING-LEFT: 8px; WIDTH: 200px" vAlign="top" align="left">
									<TABLE class="fieldGrid" id="Table3" style="HEIGHT: 100%" cellSpacing="0" cellPadding="0"
										width="200" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE class="edit" id="Table9" height="100" cellSpacing="0" cellPadding="0" width="100%"
													border="0">
													<tr>
														<td class="fieldNameRight" style="HEIGHT: 20px"><FONT face="����">&nbsp; </FONT>
															<asp:label id="Label3" runat="server">��̨����</asp:label></td>
													</tr>
													<TR>
														<TD style="PADDING-RIGHT: 8px" vAlign="top"><ignav:ultrawebtree id="treeWebTree" runat="server" Width="100%" Font-Size="12px" Height="100%" Cursor="hand"
																WebTreeTarget="ClassicTree" ImageDirectory="/ig_Images2/" JavaScriptFilename="/ig_scripts2/ig_webtree2.js" DisabledClass="DisabledClass"
																CollapseImage="ig_treeMinus.gif" ExpandImage="ig_treePlus.gif" Indentation="20" HiliteClass="HiliteClass">
																<SelectedNodeStyle Cursor="Hand" ForeColor="White" BackColor="DarkBlue"></SelectedNodeStyle>
																<DisabledStyle ForeColor="LightGray"></DisabledStyle>
																<Levels>
																	<ignav:Level Index="0"></ignav:Level>
																	<ignav:Level Index="1"></ignav:Level>
																	<ignav:Level Index="2"></ignav:Level>
																	<ignav:Level Index="3"></ignav:Level>
																	<ignav:Level Index="4"></ignav:Level>
																	<ignav:Level Index="5"></ignav:Level>
																	<ignav:Level Index="6"></ignav:Level>
																</Levels>
																<Styles>
																	<ignav:Style Cursor="Hand" ForeColor="White" BackColor="DarkBlue" CssClass="HiliteClass"></ignav:Style>
																	<ignav:Style ForeColor="LightGray" CssClass="DisabledClass"></ignav:Style>
																</Styles>
															</ignav:ultrawebtree></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</TD>
								<TD>
									<TABLE id="Table5" style="WIDTH: 100%; HEIGHT: 100%" cellSpacing="0" cellPadding="0" width="300"
										border="0">
										<TBODY>
											<TR>
												<TD class="fieldGrid" vAlign="top" align="center" height="100%"><igtbl:ultrawebgrid id="gridWebGrid" runat="server" Width="100%" Height="100%">
														<DisplayLayout ColWidthDefault="" StationaryMargins="Header" AllowSortingDefault="Yes" RowHeightDefault="20px"
															Version="2.00" SelectTypeRowDefault="Single" SelectTypeCellDefault="Single" HeaderClickActionDefault="SortSingle"
															BorderCollapseDefault="Separate" AllowColSizingDefault="Free" CellPaddingDefault="4" RowSelectorsDefault="No"
															Name="gridWebGrid" TableLayout="Fixed">
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
														</DisplayLayout>
														<Bands>
															<igtbl:UltraGridBand></igtbl:UltraGridBand>
														</Bands>
													</igtbl:ultrawebgrid></TD>
											</TR>
											<TR height="20">
												<TD>
													<table id="Table11" height="100%" cellPadding="0" width="100%" border="0">
														<tr>
															<td><asp:checkbox id="chbSelectAll" runat="server" AutoPostBack="True" Text="ȫѡ"></asp:checkbox></td>
															<td style="DISPLAY: none"><asp:checkbox id="chbifImportCheck" runat="server" Text="" Checked></asp:checkbox></td>
															<TD class="smallImgButton" style="DISPLAY: none"><INPUT id="cmdReFlesh" type="submit" value="  " name="cmdReFlesh" runat="server">
																|</TD>
															<TD class="smallImgButton" style="DISPLAY: none"><INPUT class="gridExportButton" id="cmdGridExport" type="submit" value="  " name="cmdGridExport"
																	runat="server"> |</TD>
															<TD class="smallImgButton"><INPUT class="deleteButton" id="cmdDelete" type="submit" value="  " name="cmdDelete" runat="server">
																|
															</TD>
															<TD noWrap><cc1:pagersizeselector id="pagerSizeSelector" runat="server"></cc1:pagersizeselector></TD>
															<td noWrap align="right"><cc1:pagertoolbar id="pagerToolBar" runat="server"></cc1:pagertoolbar></td>
														</tr>
													</table>
												</TD>
											</TR>
											<TR>
												<TD vAlign="top">
													<TABLE class="query" id="Table7" style="WIDTH: 100%; HEIGHT: 100%" cellSpacing="0" cellPadding="0"
														width="300" border="0">
														<TR>
															<TD vAlign="top">
																<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
																	<TR>
																		<TD class="fieldNameLeft" style="HEIGHT: 36px"><asp:label id="lblStationEdit" runat="server"> վλ</asp:label></TD>
																		<TD></TD>
																		<TD class="fieldValue" style="HEIGHT: 36px"><asp:textbox id="txtStationEdit" runat="server" CssClass="textbox" Width="150px" ReadOnly DESIGNTIMEDRAGDROP="1285"></asp:textbox></TD>
																		<TD class="fieldName" style="HEIGHT: 36px"><asp:label id="lblItemCodeEdit" runat="server">�Ϻ�</asp:label></TD>
																		<TD></TD>
																		<TD class="fieldValue" style="HEIGHT: 36px" noWrap><asp:textbox id="txtItemCodeEdit" runat="server" CssClass="textbox" Width="150px" ReadOnly></asp:textbox></TD>
																	</TR>
																	<TR>
																		<TD class="fieldNameLeft" style="HEIGHT: 36px"><asp:label id="lblFeederEdit" runat="server" DESIGNTIMEDRAGDROP="975"> �ϼܹ�����</asp:label></TD>
																		<TD style="WIDTH: 30px"><INPUT id="chbFeederEdit" style="DISPLAY: none" type="checkbox" name="chbFeederEdit" runat="server"></TD>
																		<TD class="fieldValue" style="HEIGHT: 36px"><asp:textbox id="txtFeederEdit" runat="server" CssClass="textbox" Width="150px" ReadOnly DESIGNTIMEDRAGDROP="1285"></asp:textbox></TD>
																		<TD class="fieldName" style="HEIGHT: 36px"><asp:label id="lblSupplierItemEdit" runat="server"> �����Ϻ�</asp:label></TD>
																		<TD style="WIDTH: 30px"><INPUT id="chbSupplierItemEdit" onclick="OnCheck(this)" type="checkbox" name="chbSupplierItemEdit"
																				runat="server"></TD>
																		<TD class="fieldValue" style="HEIGHT: 36px"><asp:textbox id="txtSupplierItemEdit" runat="server" CssClass="textbox" Width="150px" ReadOnly></asp:textbox></TD>
																	</TR>
																	<TR>
																		<TD class="fieldNameLeft"><asp:label id="lblLotNOEdit" runat="server" DESIGNTIMEDRAGDROP="1282"> ��������</asp:label></TD>
																		<TD><INPUT id="chbLotNOEdit" onclick="OnCheck(this)" type="checkbox" name="chbLotNOEdit" runat="server"
																				DESIGNTIMEDRAGDROP="1284"></TD>
																		<TD class="fieldValue" style="HEIGHT: 36px"><asp:textbox id="txtLotNOEdit" runat="server" CssClass="textbox" Width="150px" ReadOnly DESIGNTIMEDRAGDROP="1285"></asp:textbox></TD>
																		<TD class="fieldName"><asp:label id="lblSupplyCodeEdit" runat="server" DESIGNTIMEDRAGDROP="1440"> ����</asp:label></TD>
																		<TD><INPUT id="chbSupplyCodeEdit" onclick="OnCheck(this)" type="checkbox" name="chbSupplyCodeEdit"
																				runat="server"></TD>
																		<TD class="fieldValue" style="HEIGHT: 36px"><asp:textbox id="txtSupplyCodeEdit" runat="server" CssClass="textbox" Width="150px" ReadOnly></asp:textbox></TD>
																	</TR>
																	<TR>
																		<TD class="fieldNameLeft"><asp:label id="lblDateCodeEdit" runat="server" DESIGNTIMEDRAGDROP="1912"> ��������</asp:label></TD>
																		<TD><INPUT id="chbDateCodeEdit" onclick="OnCheck(this)" type="checkbox" name="chbDateCodeEdit"
																				runat="server" DESIGNTIMEDRAGDROP="1914"></TD>
																		<TD class="fieldValue" style="HEIGHT: 36px"><asp:textbox id="txtDateCodeEdit" runat="server" CssClass="textbox" Width="150px" ReadOnly DESIGNTIMEDRAGDROP="1915"></asp:textbox></TD>
																		<TD class="fieldName"><asp:label id="lblPCBAEdit" runat="server" DESIGNTIMEDRAGDROP="1917"> PCBA�汾</asp:label></TD>
																		<TD><INPUT id="chbPCBAEdit" onclick="OnCheck(this)" type="checkbox" name="chbPCBAEdit" runat="server"
																				DESIGNTIMEDRAGDROP="1919"></TD>
																		<TD class="fieldValue" style="HEIGHT: 36px"><asp:textbox id="txtPCBAEdit" runat="server" CssClass="textbox" Width="150px" ReadOnly DESIGNTIMEDRAGDROP="1920"></asp:textbox></TD>
																	</TR>
																	<TR>
																		<TD class="fieldNameLeft"><asp:label id="lblBIOSEdit" runat="server"> BIOS�汾</asp:label></TD>
																		<TD><INPUT id="chbBIOSEdit" onclick="OnCheck(this)" type="checkbox" name="chbBIOSEdit" runat="server"></TD>
																		<TD class="fieldValue" style="HEIGHT: 36px"><asp:textbox id="txtBIOSEdit" runat="server" CssClass="textbox" Width="150px" ReadOnly></asp:textbox></TD>
																		<TD class="fieldName"><asp:label id="lblVersionEdit" runat="server"> ���ϰ汾</asp:label></TD>
																		<TD><INPUT id="chbVersionEdit" onclick="OnCheck(this)" readOnly type="checkbox" name="Checkbox8"
																				runat="server"></TD>
																		<TD class="fieldValue" style="HEIGHT: 36px"><asp:textbox id="txtVersionEdit" runat="server" CssClass="textbox" Width="150px"></asp:textbox></TD>
																	</TR>
																</TABLE>
															</TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
											<TR>
												<TD id="Table8" align="top">
													<TABLE class="toolBar" id="Table12" border="0">
														<TR>
															<TD style="DISPLAY: none" noWrap width="100"><nobr><INPUT id="chbExportByRes" onclick="OnchbExportByResCheck()" type="checkbox" name="chbExportByRes"
																		runat="server" DESIGNTIMEDRAGDROP="1919"><asp:label id="Label1" runat="server">ָ����̨����</asp:label>&nbsp;</nobr>
															</TD>
															<TD noWrap width="100"><nobr><INPUT id="chbIfContainFeeder" type="checkbox" CHECKED name="chbIfContainFeeder" runat="server"
																		DESIGNTIMEDRAGDROP="1919"><asp:label id="Label4" runat="server">��������Feeder</asp:label>&nbsp;</nobr>
															</TD>
															<td class="toolBar"><INPUT class="submitImgButton" id="cmdMOClose" type="submit" value="�� ��" name="cmdMOClose"
																	runat="server"></td>
															<TD class="toolBar" align="center"><INPUT class="submitImgButton" id="cmdSave" type="submit" value="�� ��" name="cmdSave" runat="server"></TD>
															<TD class="toolBar" align="center"><INPUT class="submitImgButton" id="cmdCancel" type="submit" value="ȡ ��" name="cmdCancel"
																	runat="server"></TD>
															<TD class="toolBar" style="DISPLAY: none"><INPUT class="submitImgButton" id="cmdRelease" type="submit" value="ɾ ��" name="cmdRelease"
																	runat="server"></TD>
															<TD class="toolBar"><FONT face="����"></FONT></TD>
															<td class="toolBar" style="DISPLAY: none"><INPUT id="hidifImportUse" type="hidden" value="false" name="hidifImportUse" runat="server"></td>
															<td class="toolBar" style="DISPLAY: none"><INPUT class="submitImgButton" id="cmdchangeMO" type="submit" value="����ѡ��ı�" name="cmdchangeMO"
																	runat="server"></td>
															<td class="toolBar" style="DISPLAY: none"><INPUT class="submitImgButton" id="cmdFreshTree" type="submit" value="ˢ�����ṹ" name="cmdFreshTree"
																	runat="server"></td>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</TBODY>
									</TABLE>
								</TD>
							</TR>
						</TBODY>
					</TABLE>
				</TD>
			</TR>
			</TBODY></TABLE></TD></TR></TBODY></TABLE><input id="hidComparesource" type="hidden" runat="server"><input id="hidtxtMOCode" type="hidden" name="hidtxtMOCode" runat="server">
		</form>
	</body>
</HTML>
