<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCGroupConditions.ascx.cs" Inherits="BenQGuru.Web.ReportCenter.UserControls.UCGroupConditions" %>

<!-- script type="text/javascript">   
	function onCheckChange(thisControl)
	{
	    if (document.getElementById(thisControl).checked)
	    {
		    document.all.divCompareTypeGroup.style.display = "block";
		}
		else
	    {
	        document.all.divCompareTypeGroup.style.display = "none";
	    }
	}
	function loadCheckStatus(thisControl)
	{
	    if (document.getElementById(thisControl).checked)
	    {
		    document.all.divCompareTypeGroup.style.display = "block";
		}
		else
	    {
	        document.all.divCompareTypeGroup.style.display = "none";
	    }		    
	}
</script -->
		
<asp:Panel ID="PanelGroupConditions" runat="server">
    <div style="height:30px;clear:both;width:960px">
        <asp:Panel ID="PanelTitle" runat="server">            
            <div style="border:0px;float:left;width:80px;height:22px;line-height:22px;text-align:left;font-weight:bold;">
                <asp:Label ID="lblGroupConditions" runat="server" Text="ͳ��ά��"></asp:Label>
            </div>
        </asp:Panel>
    </div>        
    <div style="overflow:auto;height:auto;clear:both;width:960px">
        <asp:Panel ID="PanelByTime" runat="server" >    
            <div style="border:0px;float:left;width:80px;height:22px;line-height:22px;text-align:left;">
                <asp:Label ID="lblByTime" runat="server" Text="��ʱ��"></asp:Label>
            </div>            
            <div style="border:0px;float:left;width:400px;height:22px;text-align:left;">                
                <asp:radiobuttonlist id="rblByTimeTypeGroup" runat="server" RepeatDirection="Horizontal"></asp:radiobuttonlist>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelByCompareType" runat="server" >       
            <div style="border:0px;float:left;width:60px;height:22px;text-align:left;margin-top:3px;">                
                <asp:CheckBox id="chbCompareGroup" runat="server" Text="�Ƚ�" AutoPostBack="true"></asp:CheckBox> 
            </div>
            <div id="divCompareTypeGroup" style="border:0px;float:left;width:180px;height:22px;text-align:left;"> 
                <asp:radiobuttonlist id="rblCompareTypeGroup" runat="server" RepeatDirection="Horizontal"></asp:radiobuttonlist>
            </div>
            <div style="border:0px;float:left;width:40px;height:22px;text-align:left;">                
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelByCompleteType" runat="server" >              
            <div style="border:0px;float:left;width:180px;height:22px;text-align:left;">                
                <asp:radiobuttonlist id="rblCompleteTypeGroup" runat="server" RepeatDirection="Horizontal"></asp:radiobuttonlist>
            </div> 
        </asp:Panel> 
    </div>   
    <div style="overflow:auto;height:auto;clear:both;width:970px">
        <asp:Panel ID="PanelByConditions0" runat="server" >    
            <div style="border:0px;float:left;width:70px;height:25px;line-height:22px;text-align:left;">
                <asp:Label ID="lblByConditionsRequired" runat="server" Text="����������ѡ��"></asp:Label>
            </div>                        
            <div style="border:0px;float:left;width:880px;height:25px;line-height:22px;text-align:left;"> 
                <asp:CheckBox id="chbBigSSCodeGroupRequired" runat="server" Text="����"></asp:CheckBox>
                <asp:CheckBox id="chbSegCodeGroupRequired" runat="server" Text="����"></asp:CheckBox>
                <asp:CheckBox id="chbSSCodeGroupRequired" runat="server" Text="������"></asp:CheckBox>
                <asp:CheckBox id="chbOPCodeGroupRequired" runat="server" Text="����"></asp:CheckBox>
                <asp:CheckBox id="chbResCodeGroupRequired" runat="server" Text="��Դ"></asp:CheckBox>
            </div>
        </asp:Panel>    
    </div>    
    <div style="height:25px;clear:both;width:970px">
        <asp:Panel ID="PanelByConditions1" runat="server" >    
            <div style="border:0px;float:left;width:70px;height:22px;line-height:22px;text-align:left;">
                <asp:Label ID="lblByConditions" runat="server" Text="������"></asp:Label>
            </div>                
            <div style="border:0px;float:left;width:900px;height:auto;line-height:22px;text-align:left;">                
                <asp:CheckBox id="chbGoodSemiGoodGroup" runat="server" Text="��Ʒ/���Ʒ"></asp:CheckBox>
                <asp:CheckBox id="chbInspectorGroup" runat="server" Text="��Ա"></asp:CheckBox>
                <asp:CheckBox id="chbItemCodeGroup" runat="server" Text="��Ʒ����"></asp:CheckBox>
                <asp:CheckBox id="chbItemTypeGroup" runat="server" Text="��Ʒ��"></asp:CheckBox>
                <asp:CheckBox id="chbProjectName" runat="server" Text="��Ŀ����"></asp:CheckBox>
                <asp:CheckBox id="chbMaterialModelCodeGroup" runat="server" Text="��������"></asp:CheckBox>
                <asp:CheckBox id="chbMaterialMachineTypeGroup" runat="server" Text="������о"></asp:CheckBox>
                <asp:CheckBox id="chbMaterialExportImportGroup" runat="server" Text="����/����"></asp:CheckBox>   
                <asp:CheckBox id="chbLotNoGroup" runat="server" Text="����"></asp:CheckBox>    
                <asp:CheckBox id="chbProductionTypeGroup" runat="server" Text="��������"></asp:CheckBox> 
                <asp:CheckBox id="chbOQCLotTypeGroup" runat="server" Text="������"></asp:CheckBox>          
                <asp:CheckBox id="chbMOCodeGroup" runat="server" Text="����"></asp:CheckBox>
                <asp:CheckBox id="chbMOMemoGroup" runat="server" Text="������"></asp:CheckBox>
                <asp:CheckBox id="chbNewMassGroup" runat="server" Text="��Ʒ/����Ʒ"></asp:CheckBox>                
                <asp:CheckBox id="chbCrewCodeGroup" runat="server" Text="����"></asp:CheckBox>                
                <asp:CheckBox id="chbFirstClassGroup" runat="server" Text="һ������"></asp:CheckBox>
                <asp:CheckBox id="chbSecondClassGroup" runat="server" Text="��������"></asp:CheckBox>
                <asp:CheckBox id="chbThirdClassGroup" runat="server" Text="��������"></asp:CheckBox>
                <asp:CheckBox id="chbFacCodeGroup" runat="server" Text="����" AutoPostBack="true"></asp:CheckBox>
                <asp:Label ID="lblSplitter1" runat="server" Text="|" ForeColor="#999999"></asp:Label>
                <asp:CheckBox id="chbBigSSCodeGroup" runat="server" Text="����"></asp:CheckBox>
                <asp:Label ID="lblSplitter2" runat="server" Text="|" ForeColor="#999999"></asp:Label>
                <asp:CheckBox id="chbSegCodeGroup" runat="server" Text="����"></asp:CheckBox>
                <asp:CheckBox id="chbSSCodeGroup" runat="server" Text="������"></asp:CheckBox>
                <asp:Label ID="lblSplitter3" runat="server" Text="|" ForeColor="#999999"></asp:Label>
                <asp:CheckBox id="chbOPCodeGroup" runat="server" Text="����"></asp:CheckBox>
                <asp:CheckBox id="chbResCodeGroup" runat="server" Text="��Դ"></asp:CheckBox>
                <asp:CheckBox id="chbExceptionCodeGroup" runat="server" Text="�쳣�¼�����"></asp:CheckBox>
                <asp:CheckBox id="chbIQCItemTypeWhere" runat="server" Text="����������"></asp:CheckBox>
                <asp:CheckBox id="chbIQCLineItemTypeWhere" runat="server" Text="����Ŀ��������"></asp:CheckBox>                
                <asp:CheckBox id="chbRoHSWhere" runat="server" Text="���/�����RoHS"></asp:CheckBox>
                <asp:CheckBox id="chbConcessionWhere" runat="server" Text="�ò�����"></asp:CheckBox>
                <asp:CheckBox id="chbVendorCodeWhere" runat="server" Text="��Ӧ�̴���"></asp:CheckBox>
                <asp:CheckBox id="chbMaterialCodeGroup" runat="server" Text="���ϴ���"></asp:CheckBox> 
                <asp:radiobuttonlist id="rblExceptionOrDuty" runat="server" RepeatDirection="Horizontal"></asp:radiobuttonlist>
            </div>
        </asp:Panel> 
        <asp:Panel ID="PanelManHour" runat="server" >  
            <div style="border:0px;float:left;width:70px;height:22px;line-height:22px;text-align:left;">
            </div>    
            <div style="border:0px;float:left;width:160px;height:22px;text-align:left;margin-top:3px;">                
                <asp:CheckBox id="chbExcludeReworkOutput" runat="server" Text="ȥ����������" Checked="false"></asp:CheckBox> 
            </div>          
            <div style="border:0px;float:left;width:160px;height:22px;text-align:left;margin-top:3px;">                
                <asp:CheckBox id="chbExcludeLostManHour" runat="server" Text="ȥ����Ч��ʱ" Checked="false"></asp:CheckBox> 
            </div>
            <div style="border:0px;float:left;width:160px;height:22px;text-align:left;margin-top:3px;">                
                <asp:CheckBox id="chbIncludeIndirectManHour" runat="server" Text="�������������ʱ" Checked="true"></asp:CheckBox> 
            </div>
        </asp:Panel> 
    </div>
</asp:Panel>