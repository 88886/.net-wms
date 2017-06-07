<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCWhereConditions.ascx.cs" Inherits="BenQGuru.Web.ReportCenter.UserControls.UCWhereConditions" %>
<%@ Register TagPrefix="cc1" Namespace="BenQGuru.eMES.Web.Helper" Assembly="BenQGuru.eMES.WebUI.Helper" %>
<%@ Register TagPrefix="cc2" NameSpace="BenQGuru.eMES.Web.SelectQuery" Assembly="BenQGuru.eMES.Web.SelectQuery" %>
<%@ Register TagPrefix="cc3" TagName="eMESDate" Src="~/UserControl/DateTime/DateTime/eMESDate.ascx" %>

<asp:Panel ID="Panel1" runat="server">
    <div style="height:30px;clear:both;width:960px">
        <asp:Panel ID="PanelTitle" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:left;font-weight:bold;">
                <asp:Label ID="lblWhereConditions" runat="server" Text="��������"></asp:Label>
            </div>
        </asp:Panel>
    </div>  
    <div style="clear:both;width:960px">
        <asp:Panel ID="PanelControls" runat="server">            
            <asp:Table ID="TableControls" runat="server"></asp:Table>
        </asp:Panel>
    </div>     
</asp:Panel>  
<asp:Panel ID="PanelWhereConditions" runat="server">      
    <!--һ����DIV���в���,һ�з�4���ؼ���ÿ��240px���ܿ��960px-->
    <div style="height:25px;clear:both;width:960px">
        <!--һ���е�ÿ���ؼ�����label������ؼ�������Panel������װ��������Ƴ������-->
        <asp:Panel ID="PanelGoodSemiGoodWhere" runat="server" >            
            <!--����ؼ���DIV�����֣��������ص�ǰ��Ŀؼ����ÿؼ����Զ�����ǰ��ؼ���λ��-->
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblGoodSemiGoodWhere" runat="server" Text="��Ʒ/���Ʒ"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">                
                <asp:DropDownList ID="ddlGoodSemiGoodWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelItemCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblItemCodeWhere" runat="server" Text="��Ʒ����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtItemCodeWhere" runat="server" Type="item" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>
         <asp:Panel ID="PanelMaterialCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblMaterialCodeWhere" runat="server" Text="���ϴ���"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtMaterialCodeWhere" runat="server" Type="material" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelMaterialModelCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblMaterialModelCodeWhere" runat="server" Text="��������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">                
                <cc2:selectabletextbox id="txtMaterialModelCodeWhere" runat="server" Type="mmodelcode" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>
    </div>
    <div style="height:25px;clear:both;width:960px">
        <asp:Panel ID="PanelMOCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblMOCodeWhere" runat="server" Text="����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtMOCodeWhere" runat="server" Type="mo" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelMOTypeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblMOTypeWhere" runat="server" Text="��������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlMOTypeWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelOrderNoWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblOrderNoWhere" runat="server" Text="��ͬ��"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">                
                <asp:TextBox ID="txtOrderNoWhere" runat="server" Width="131px" MaxLength="40"></asp:TextBox>
            </div>
        </asp:Panel> 
        <asp:Panel ID="PanelMOBOMVersionWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblMOBOMVersionWhere" runat="server" Text="BOM�汾"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlMOBOMVersionWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelMaterialMachineTypeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblMaterialMachineTypeWhere" runat="server" Text="������о"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtMaterialMachineTypeWhere" runat="server" Type="mmachinetype" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelLotNoWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblLotNoWhere" runat="server" Text="����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtLotNoWhere" runat="server" Type="lot" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>       
    </div>    
    <div style="height:25px;clear:both;width:960px">
        <asp:Panel ID="PanelBigSSCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblBigSSCodeWhere" runat="server" Text="����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtBigSSCodeWhere" runat="server" Type="bigline" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelSegCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblSegCodeWhere" runat="server" Text="����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtSegCodeWhere" runat="server" Type="segment" Readonly="false" CanKeyIn="true" AutoPostBack="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelSSCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblSSCodeWhere" runat="server" Text="������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox4SS id="txtSSCodeWhere" runat="server" Type="stepsequence" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox4SS>
            </div>
        </asp:Panel>                
    </div>
    <div style="height:25px;clear:both;width:960px">
        <asp:Panel ID="PanelOPCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblOPCodeWhere" runat="server" Text="����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtOPCodeWhere" runat="server" Type="operation" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelResCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblResCodeWhere" runat="server" Text="��Դ"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtResCodeWhere" runat="server" Type="resource" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>        
        <asp:Panel ID="PanelShiftCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblShiftCodeWhere" runat="server" Text="���"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlShiftCodeWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelCrewCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblCrewCodeWhere" runat="server" Text="����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlCrewCodeWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel>                
    </div>    
    <div style="height:25px;clear:both;width:960px">
        <asp:Panel ID="PanelStartDateWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblStartDateWhere" runat="server" Text="��ʼ����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <input type="text" id="datStartDateWhere"  class='datepicker' runat="server"  style="width:130px;"/>
            </div>
        </asp:Panel>                
    </div>    
    <div style="height:25px;clear:both;width:960px">
        <asp:Panel ID="PanelEndDateWhere" runat="server">              
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblEndDateWhere" runat="server" Text="��������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                 <input type="text" id="datEndDateWhere"  class='datepicker' runat="server" style="width:130px;" />
            </div>
        </asp:Panel>    
    </div>    
    <div style="height:25px;clear:both;width:960px">
        <asp:Panel ID="PanelFirstClassWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblFirstClassWhere" runat="server" Text="һ������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlFirstClassWhere" runat="server" Width="131px" AutoPostBack="true"></asp:DropDownList>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelSecondClassWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblSecondClassWhere" runat="server" Text="��������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlSecondClassWhere" runat="server" Width="131px" AutoPostBack="true"></asp:DropDownList>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelThirdClassWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblThirdClassWhere" runat="server" Text="��������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlThirdClassWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel>                       
    </div>  
    <div style="height:25px;clear:both;width:960px">
        <asp:Panel ID="PanelInputOututWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblInputOututWhere" runat="server" Text="Ͷ��/����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlInputOututWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelMOMemoWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblMOMemoWhere" runat="server" Text="������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtMOMemoWhere" runat="server" Type="momemo" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelNewMassWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblNewMassWhere" runat="server" Text="��Ʒ/����Ʒ"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlNewMassWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel>        
        <asp:Panel ID="PanelMaterialExportImportWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblMaterialExportImportWhere" runat="server" Text="����/����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlMaterialExportImportWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel> 
       <asp:Panel ID="PanelProductionTypeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblProductionTypeWhere" runat="server" Text="��������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtProductionTypeWhere" runat="server" Type="productiontype" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelOQCLotTypeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblOQCLotTypeWhere" runat="server" Text="������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtOQCLotTypeWhere" runat="server" Type="oqclottype" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelErrorCauseGroupCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblErrorCauseGroupCodeWhere" runat="server" Text="������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtErrorCauseGroupCodeWhere" runat="server" Type="errorcausegroup" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>    
        <asp:Panel ID="PanelDutyCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblDutyCodeWhere" runat="server" Text="���α�"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtDutyCodeWhere" runat="server" Type="duty" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>   
        <asp:Panel ID="PanelExceptionCodeWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblExceptionCodeWhere" runat="server" Text="�쳣�¼�����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtExceptionCodeWhere" runat="server" Type="exceptioncode" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>       
        <asp:Panel ID="PanelExceptionFlagWhere" runat="server">            
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:CheckBox id="chbExceptionFlagWhere" runat="server" Text="����������ʧ" Checked="true"></asp:CheckBox>
            </div>
        </asp:Panel>   
        <asp:Panel ID="PanelIncludeDroppedMaterialWhere" runat="server">            
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:CheckBox id="chbIncludeDroppedMaterialWhere" runat="server" Text="�����Ѳ���¼" Checked="true"></asp:CheckBox>
            </div>
        </asp:Panel>          
        <asp:Panel ID="PanelInspectorWhere" runat="server">            
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblInspectorWhere" runat="server" Text="��Ա"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtInspectorWhere" runat="server" Type="user" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>     
        <asp:Panel ID="PanelVendorCodeWhere" runat="server">        
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblVendorCodeWhere" runat="server" Text="��Ӧ�̴���"></asp:Label>&nbsp;
            </div>
             <div style="border:0px;float:left;width:160px;text-align:left;">
                <cc2:selectabletextbox id="txtVendorCodeWhere" runat="server" Type="vendor" Readonly="false" CanKeyIn="true" Width="131px"></cc2:selectabletextbox>
            </div>
        </asp:Panel>  
        <asp:Panel ID="PanelIQCItemTypeWhere" runat="server"> 
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblIQCItemTypeWhere" runat="server" Text="IQC����������"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlIQCItemTypeWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel>  
        <asp:Panel ID="PanelIQCLineItemTypeWhere" runat="server"> 
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblIQCLineItemTypeWhere" runat="server" Text="����Ŀ����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlIQCLineItemTypeWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel> 
        <asp:Panel ID="PanelRoHSWhere" runat="server"> 
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblRoHSWhere" runat="server" Text="���/�����RoHS"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlRoHSWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel>   
         <asp:Panel ID="PanelConcessionWhere" runat="server"> 
            <div style="border:0px;float:left;width:75px;height:22px;line-height:22px;text-align:right;">
                <asp:Label ID="lblConcessionWhere" runat="server" Text="�ò�����"></asp:Label>&nbsp;
            </div>
            <div style="border:0px;float:left;width:160px;text-align:left;">
                <asp:DropDownList ID="ddlConcessionWhere" runat="server" Width="131px"></asp:DropDownList>
            </div>
        </asp:Panel>                
    </div>           
</asp:Panel>
