<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics35.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.1006, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Page Language="c#" CodeBehind="FStartPage.aspx.cs" AutoEventWireup="True" Inherits="BenQGuru.eMES.Web.FStartPage" %>

<%@ Register Assembly="Infragistics35.Web.v11.2, Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics35.Web.v11.2,Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics35.Web.v11.2,Version=11.2.20112.1019, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>--%>
<head>
    <title>
        <%=Title%>
    </title>
    <meta content="C#" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <link href="<%=StyleSheet%>" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>
   <%-- <script src="http://code.jquery.com/jquery-latest.js"></script>--%>
    <%-- <script src="Scripts/jquery-1.6.min.js" type="text/javascript"></script>--%>
    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="reveal/jquery.reveal.js" type="text/javascript"></script>
    <link href="reveal/reveal.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .Header
        {
            background-image: url(Skin/Image/Index_eMES_banner_middle.gif);
            background-repeat: repeat-x;
        }
        .Header_Banner
        {
            background-image: url(Skin/Image/Index_eMES_banner.gif);
            background-repeat: no-repeat;
            height: 53px;
            background-position: left;
            width: 100%;
        }
        .Header_Banner_Width
        {
            width: 250px;
        }
        html
        {
            overflow: hidden;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function BrowserType() {

            var Sys = {};

            var ua = navigator.userAgent.toLowerCase();

            var s;

            (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :

            (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :

            (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :

            (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :

            (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;

            return Sys;
        }

        //�������message�еĹؼ�������֮�󣬹ر�div����ֵ���㵽��Ӧ�Ŀؼ��ϡ�
        function SetFocus(idWithSharp) {
            $('#messageModal').trigger('reveal:close');
            if ($("#frmWorkSpace").contents().find(idWithSharp).html() == null ||
            $("#frmWorkSpace").contents().find(idWithSharp).is('span')) {
                $("#frmWorkSpace").contents().find("input[name^='" + idWithSharp.substring(1) + "']").first().focus();
                $("#frmWorkSpace").contents().find("input[name^='" + idWithSharp.substring(1) + "']").first().select();
            }
            else {

                $("#frmWorkSpace").contents().find(idWithSharp).focus();
                $("#frmWorkSpace").contents().find(idWithSharp).select();
            }
            return false;
        }

        window.onresize = function () {
            dyniframesize();
        }

        $(function () {

            //���ú�̨������������
            $("#UploadError").click(function () {
                $.ajax({
                    type: "POST",   //����WebServiceʹ��Post��ʽ����
                    contentType: "application/json", //WebService �᷵��Json����
                    url: "FStartPage.aspx/UploadError", //����WebService�ĵ�ַ�ͷ���������� ---- WsURL/������
                    data: "{msg:'" + $("#lblError").text() + "',innserMsg:'" + $("#lblErrorDetail").text() + "'}",
                    dataType: 'json',
                    success: function (result) {     //�ص�������result������ֵ
                        $('#errorModal').trigger('reveal:close');
                    }
                }).error(function (XmlHttpRequest, textStatus, errorThrown) {
                    alert("��" + XmlHttpRequest.responseText);
                });
                return false;
            });

            $('#errorModal').draggable({ cursor: "move", handle: ".head-reveal-modal" });
            $('#messageModal').draggable({ cursor: "move", handle: ".head-reveal-modal" });

        });

        //����������ʾ��
        function showErrorDialog(error, errorDetail) {

            if (BrowserType().ie == "6.0") {
                alert(error + "\n" + $("#detailDesc").text() + errorDetail);
                return;
            }

            $('#errorModal').reveal({
                animation: 'fade',                   //fade, fadeAndPop, none
                animationspeed: 500,                       //how fast animtions are
                closeonbackgroundclick: false,              //if you click background will modal close?
                dismissmodalclass: 'close-reveal-modal'    //the class of a button or element that will close an open modal
            });
            $('#errorModal').css("top", $(window).height() / 2 - $('#errorModal').outerHeight() / 2);

            $("#lblError").text(error);
            $("#lblErrorDetail").text($("#detailDesc").text() + errorDetail);

        }

        //������ͨ��Ϣ��ʾ��
        function showMessageDialog(msg) {

            if (BrowserType().ie == "6.0") {
                alert($("#temp").html(msg.replace("<br />", "\n")).text());
                return;
            }
            $('#messageModal').reveal({
                animation: 'fade',
                animationspeed: 500,
                closeonbackgroundclick: true,
                dismissmodalclass: 'close-reveal-modal'
            });
            $(".reveal-modal-bg").css("zoom", null);

            $('#messageModal').css("top", $(window).height() / 2 - $('#messageModal').outerHeight() / 2);

            $("#lblMessage").html(msg);

        }

        //�Զ�����
        $(function () {
            $("#autoHidden").click(function () {

                if ($("#trHead:hidden").length > 0) {
                    $("#trHead").show();
                    $("#trMenu").show();
                    $("#autoHidden").css("background-image", "url(Skin/Image/up1.gif)");
                    otherHeight = trHeadHeight + trMenuHeight + trNavHeight;
                    dyniframesize();

                }
                else {
                    $("#trHead").hide();
                    $("#trMenu").hide();
                    $("#autoHidden").css("background-image", "url(Skin/Image/down1.gif)");
                    otherHeight = trNavHeight;
                    dyniframesize();
                }
            });

        });

        var trHeadHeight, trMenuHeight, trNavHeight;
        var otherHeight; //��ҳ���ϳ���frmWorkSpace�����Ŀɼ��߶��ۺ�
        function MainFrameLoad(objFrame) {
            try
            {
                if(window.frmWorkSpace.window.location.pathname.indexOf('FIframePage.aspx')<0)
                    document.getElementById('frmNav').contentWindow.location = "FPageNavigator.aspx";

                trHeadHeight = $("#trHead").outerHeight();
                trMenuHeight = $("#trMenu").outerHeight();
                trNavHeight = $("#trNav").outerHeight();
                otherHeight = trHeadHeight + trMenuHeight + trNavHeight;

                dyniframesize();
            }
            catch(e) {
                window.history.back(-1);
            }
        }


        var secondHover;
        function reSetMenuCss() {
            //�����˵�λ��
            //$("#WebDataMenuSample").css("margin-left", "1px");
            $("#WebDataMenuSample").css("margin-top", "-3px");
            
            var tdWidth = $("#WebDataMenuSample").parent().width();
            var rootMenu = $("#WebDataMenuSample").children("div").first().children("ul");
       
            $("#WebDataMenuSample").children("div").width(tdWidth);
            //��Ŀ¼������ɫ����
            var rootBackColor = "rgb(175,215,237)"; //
            $("#WebDataMenuSample").css("background-image", "none");
            $("#WebDataMenuSample").css("background-color", rootBackColor);
            rootMenu.parent().css("background-color", rootBackColor);
            rootMenu.css("background-color", rootBackColor);
            rootMenu.children("li").css("background-color", rootBackColor);

            //���˵���������
            //rootMenu.children("li").children("a").css("font-weight", "bold");
            rootMenu.children("li").children("a").css("color", "black");

            //���˵��߶�����
            rootMenu.parent().css("height", "25px");
            rootMenu.css("height", "25px");
            rootMenu.children("li").css("height", "20px");
            rootMenu.children("li").css("padding-top", "5px");
            rootMenu.children("li").css("padding-bottom", "0px");
            rootMenu.children("li").children("div").css("margin-top", "-3px");
            //$("li").find("a").css("font-size", "11px");

            rootMenu.children("li").children("a").unload("click");
            //���ö����˵���ʽ
            var secondMenu = $("#WebDataMenuSample").children("div").first().children("ul").children("li").children("div").children("ul");
            //secondMenu.find("li").css("background-image", "none");
            secondMenu.parent().css("border-width", "0");
            secondMenu.find("li").css("background-color", "#EAF7FF");
            secondMenu.find("li").css("border", "1px solid #9AC6EB");

            if (BrowserType().ie == '9.0') {
                secondMenu.find("li").css("border-right", "2px solid #9AC6EB");
            }
            secondMenu.each(function () {
                $(this).children("li").first().css("margin-top", "1px");
                $(this).children("li").last().css("border-bottom", "2px solid #9AC6EB");

            });

            secondMenu.find("li").css("margin-bottom", "1px");
            secondMenu.find("li").css("color", "black");

            var thirdMenu = secondMenu.children("li").children("div").children("ul");

            secondMenu.children("li").hover(
                  function () {
                      $(this).css("background-color", "#77BAF1");
                      $(this).css("border-color", "#5093CA");
                      secondHover = $(this);
                  },
                  function () {
                      $(this).css("background-color", "#EAF7FF");
                      $(this).css("border-color", "#9AC6EB");
                  }
                );

            //���������˵���ʽ

            thirdMenu.parent().css("border-width", "0");
            //thirdMenu.find("li:last").css("border-bottom", "2px solid #9AC6EB");
            thirdMenu.find("li").hover(
                  function () {
                      $(this).css("background-color", "#77BAF1");
                      $(this).css("border-color", "#5093CA");

                      secondHover.css("background-color", "#77BAF1");
                      secondHover.css("border-color", "#5093CA");
                  },
                  function () {
                      $(this).css("background-color", "#EAF7FF");
                      $(this).css("border-color", "#9AC6EB");

                      secondHover.css("background-color", "#EAF7FF");
                      secondHover.css("border-color", "#9AC6EB");
                  }
                );


            //alert(mainWidth);
            //            if (BrowserType().ie || BrowserType().firefox) {
            //                //IE�ͻ��������Ӵ�֮����Ҫ�����˵����
            //                var mainWidth = parseInt($("#WebDataMenuSample").children("div").first().children("ul").first().css("width"));
            //                $("#WebDataMenuSample").children("div").first().children("ul").first().css("width", mainWidth * 1.05 + "px");
            //            }

            //            $("#WebDataMenuSample").children("div").first().children("ul").find("ul").children("li:even").css("background-color", "#CFE2F1"); //CFE2F1

        }

        var frameHeight;
        function dyniframesize() {

            //iframe�߶�����Ӧ
            var pTar = null;
            if (document.getElementById) {
                pTar = document.getElementById('frmWorkSpace');
            }
            else {
                eval("pTar = ' frmWorkSpace';");
            }


            if (pTar && !window.opera) {
                //begin resizing iframe 
                //pTar.style.display = "block"
                if (BrowserType().ie || BrowserType().firefox) {
                    pTar.height = $(window).height() - otherHeight - 1;
                }
                else if (pTar.contentDocument && pTar.contentDocument.body.offsetHeight) {
                    //ns6 syntax 
                    pTar.height = $(window).height() - otherHeight - 1;
                    //pTar.scrolling = "no";
                }
                else if (pTar.Document && pTar.Document.body.scrollHeight) {
                    //ie5+ syntax 
                    pTar.height = $(document).height() - otherHeight - 1;
                    //pTar.width =document.body.scrollWidth;
                }
            }

            //IE����Ҫ�ٴε����߶�
            if (pTar && !window.opera) {
                //begin resizing iframe 
                //pTar.style.display = "block"
                if (BrowserType().ie || BrowserType().firefox) {
                    pTar.height = $(window).height() - otherHeight - 1;

                }
                else if (pTar.contentDocument && pTar.contentDocument.body.offsetHeight) {
                    //ns6 syntax 
                    pTar.height = $(window).height() - otherHeight - 1;
                    //pTar.scrolling = "no";
                }
                else if (pTar.Document && pTar.Document.body.scrollHeight) {
                    //ie5+ syntax 
                    pTar.height = $(document).height() - otherHeight - 1;
                    //pTar.width =document.body.scrollWidth;
                }
            }
            //pTar.scrolling = "no";

            //�����Ҫ�������iframe�ĸ߶�
            if (BrowserType().firefox) {
                while ($(window).height() < $(document).height()) {
                    pTar.height--;
                }

            }
          
        }

        function test() {
        }

        function LogoutCheck() {
            var alterString = document.getElementById("lblLogout").value;
            if (confirm(alterString)) return true;
            else return false;
        }

        function GoToHomePage() {
            if (document.getElementById("inputIsPlatForm").value == "True") {
                $('#frmWorkSpace').attr('src', 'ReportView/FRptMaintainStartPageMP.aspx');
            }
            else {
                location.href = 'FStartPage.aspx';
                // $('#frmWorkSpace').attr('src', 'FIframePage.aspx');
            }
            document.getElementById('frmNav').contentWindow.location = "FPageNavigator.aspx";

        }

    </script>
    <script language="javascript" id="clientEventHandlersJS" type="text/javascript">
			var selectItem = null; 		//��ǰѡ����Ŀ
			<!--
			//��������
			function toolbarPassword_onclick() {
				window.showModalDialog("./FUserPassWordModifyMP.aspx","",showDialog(6));
			}
			
			//��������
			function toolbarReport_onclick()
			{
				window.document.location.href = "FReportMain.aspx";
			}
			
			//����ƽ̨
			function toolbarReportView_onclick()
			{
				//window.document.location.href = "FReportViewMain.aspx";
			}
            function LinkOrgList_Click()
			{		    
			    var newWindow = "./FOrgChangePage.aspx";
			    
			    //var result = window.open(newWindow);
			    var result = window.showModalDialog(newWindow,"",showDialog(6));
                //alert(result);
                document.getElementById("LinkOrgList").innerText = result;
                document.frames["frmWorkSpace"].location.replace(document.frames["frmWorkSpace"].location.href);
			}
           
//-->
    </script>
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout"
    scroll="no">
    <form id="Form1" method="post" runat="server">
    <ig:WebScriptManager ID="WebScriptManager1" runat="server">
    </ig:WebScriptManager>
    <table id="TableMain" style="width: 100%; height: 100%;" cellspacing="0" cellpadding="0">
        <tr id="trHead">
            <td>
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="width: 340px; height: 50px">
                            <img src="Skin/Image/Index_eMES_banner.gif" width="340" alt="" runat="server" id="ImageHead"
                                height="50" />
                        </td>
                        <td class="Header" align="right">
                            <table cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td align="center">
                                        <table style="display: none;">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblUserName_odl" runat="server" CssClass="welcome">Customer</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="lblWelcome" runat="server" CssClass="welcome"> ��ӭʹ�ñ�ϵͳ</asp:Label>
                                                    <asp:Label ID="lblOrganization" runat="server" CssClass="welcome"> ������֯Ϊ:</asp:Label>
                                                    <a runat="server" href="" style="text-decoration: underline; cursor: hand;" class="welcome"
                                                        onclick="return LinkOrgList_Click();" id="LinkOrgList_old" target="_parent">orgnization</a>
                                                    <iframe id="iframeDownload" name="iframeDownload" width="0" height="0"></iframe>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td style="color: White;">
                                                    Welcome,<asp:Label ID="lblUserName" runat="server" CssClass="welcome">Customer</asp:Label>
                                                    ( <a runat="server" href="" style="text-decoration: underline; cursor: pointer; color: White;"
                                                        class="welcome" onclick="return LinkOrgList_Click();" id="LinkOrgList" target="_parent">
                                                        orgnization</a>)
                                                </td>
                                                <td width="8" style="color: White">
                                                    &nbsp;&nbsp;|&nbsp;&nbsp;
                                                </td>
                                                <td style="color: White;">
                                                    <asp:Label ID="lblLocate" runat="server" Text="����ǰ��λ�ã�"></asp:Label>
                                                     <asp:Label ID="lblLocation" runat="server" Text="��ҵ����"></asp:Label>
                                                </td>
                                                <td width="4" style="color: White">
                                                    &nbsp;
                                                </td>
                                                <td width="25">
                                                    <a href="#" style="color: White" onclick="GoToHomePage();">
                                                        <asp:Label runat="server" ID="lblHome" Text="��ҳ"></asp:Label></a>
                                                </td>
                                                <td width="8" style="color: White">
                                                    &nbsp;&nbsp;|&nbsp;&nbsp;
                                                </td>
                                                <td width="50">
                                                    <asp:LinkButton runat="server" ID="linkBtnPlatForm" Text="����ƽ̨" OnClick="linkBtnPlatForm_OnClick"
                                                        ForeColor="White">
                                                    </asp:LinkButton>
                                                    <input id="inputIsPlatForm" type="hidden" value='<%= ViewState["IsReportCenter"]%>' />
                                                </td>
                                                <td width="8" style="color: White">
                                                    &nbsp;&nbsp;|&nbsp;&nbsp;
                                                </td>
                                                <td width="50">
                                                    <a href="#" style="color: White" onclick="toolbarPassword_onclick();return false;">
                                                        <asp:Label runat="server" ID="lblChangePWD" Text="��������"></asp:Label></a>
                                                </td>
                                                <td width="8" style="color: White">
                                                    &nbsp;&nbsp;|&nbsp;&nbsp;
                                                </td>
                                                <%-- <td width="30">
                                                    <a href="#" style="color: White">
                                                        <asp:Label runat="server" ID="lblHelp" Text="����"></asp:Label></a>
                                                </td>
                                                <td width="8" style="color: White">
                                                    &nbsp;&nbsp;|&nbsp;&nbsp;
                                                </td>--%>
                                                <td width="30">
                                                    <asp:LinkButton ID="lnkButtonLogout" Style="color: White" runat="server" OnClientClick="return LogoutCheck();"
                                                        Text="�ǳ�"></asp:LinkButton>
                                                </td>
                                                <td width="8">
                                                    <asp:TextBox ID="lblLogout" runat="server" Width="8" Visible="true" Style="display: none"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 90px; height: 50px">
                            <img src="Skin/Image/Index_eMES_logo.gif" width="90" alt="" runat="server" height="50"
                                id="ImageLogo" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 20px; vertical-align: top; display: none;">
                <ignav:UltraWebMenu ID="mainMenu" runat="server" CssClass="Control" Width="100%"
                    TopAligment="Center" ItemPaddingTop="1" TargetFrame="" TargetUrl="">
                    <ItemStyle Cursor="Hand" />
                    <IslandStyle CssClass="Island" Font-Size="9pt">
                    </IslandStyle>
                    <HoverItemStyle CssClass="Hover">
                    </HoverItemStyle>
                    <TopLevelHoverItemStyle CssClass="TopLevelHover">
                    </TopLevelHoverItemStyle>
                    <TopLevelLeafItemStyle CssClass="TopLevel">
                    </TopLevelLeafItemStyle>
                    <TopLevelParentItemStyle CssClass="TopLevel">
                    </TopLevelParentItemStyle>
                    <Levels>
                        <ignav:Level Index="0" />
                        <ignav:Level Index="1" LevelClass="SubLevel" LevelHoverClass="SubLevelHover" />
                        <ignav:Level Index="2" LevelClass="SubLevel" LevelHoverClass="SubLevelHover" />
                        <ignav:Level Index="3" LevelClass="SubLevel" LevelHoverClass="SubLevelHover" />
                    </Levels>
                </ignav:UltraWebMenu>
            </td>
        </tr>
        <tr id="trMenu">
            <td>
                <ig:WebDataMenu ID="WebDataMenuSample" runat="server" Width="100%" StyleSetName="Caribbean"
                    ClientEvents-Initialize="reSetMenuCss">
                    <GroupSettings Orientation="Horizontal" />
                </ig:WebDataMenu>
            </td>
        </tr>
        <tr id="trNav">
            <td style="height: 25px;">
                <iframe id="frmNav" style="width: 90%; border-top-style: none; border-right-style: none;
                    border-left-style: none; height: 100%; background-color: transparent; border-bottom-style: none;
                    float: left;" name="frmNav" src="FPageNavigator.aspx" frameborder="0" width="100%"
                    scrolling="no" height="100%"></iframe>
                <div id="autoHidden" style="float: right; background-image: url(Skin/Image/up1.gif);
                    background-repeat: no-repeat; width: 20px; height: 30px;">
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; height: 100%;">
                <iframe id="frmWorkSpace" style="padding-right: 8px; padding-left: 0px; width: 100%;
                    border: 0px solid red" name="frmWorkSpace" align="middle" marginwidth="0" marginheight="0"
                    src="FIframePage.aspx" frameborder="0" scrolling="auto" onload="MainFrameLoad(this);">
                </iframe>
            </td>
        </tr>
    </table>
    <asp:DropDownList ID="DDList1" runat="server" AutoPostBack="True" Visible="false">
        <asp:ListItem>Appletini</asp:ListItem>
        <asp:ListItem Selected="True">Caribbean</asp:ListItem>
        <asp:ListItem>Claymation</asp:ListItem>
        <asp:ListItem>Default</asp:ListItem>
        <asp:ListItem>ElectricBlue</asp:ListItem>
        <asp:ListItem>Harvest</asp:ListItem>
        <asp:ListItem>LucidDream</asp:ListItem>
        <asp:ListItem>Nautilus</asp:ListItem>
        <asp:ListItem>Office2007Black</asp:ListItem>
        <asp:ListItem>Office2007Blue</asp:ListItem>
        <asp:ListItem>Office2007Silver</asp:ListItem>
        <asp:ListItem>Pear</asp:ListItem>
        <asp:ListItem>RedPlanet</asp:ListItem>
        <asp:ListItem>RubberBlack</asp:ListItem>
        <asp:ListItem>Trendy</asp:ListItem>
        <asp:ListItem>Windows7</asp:ListItem>
        <asp:ListItem>Office2010Blue</asp:ListItem>
    </asp:DropDownList>
    <div id="test02" style="display: none">
        asdas</div>
    <div id="temp" style="display: none">
    </div>
    <div style="display: none" id="detailDesc">
        <% =this.languageComponent1.GetString("$PageControl_Detials") %>
    </div>
    <div id="errorModal" class="reveal-modal size3">
        <h1 id="lblError" style="color: Red; word-wrap: break-word; word-break: break-all;">
        </h1>
        <p id="lblErrorDetail">
        </p>
        <a href="" id="UploadError" style="color: Blue;">�ύ����</a>
        <div class="head-reveal-modal">
            <a class="title-reveal-modal" style="color: Red;">Error</a> <a class="close-reveal-modal">
                &#215;</a>
        </div>
    </div>
    <div id="messageModal" class="reveal-modal size2">
        <h3 id="lblMessage" style="word-wrap: break-word; word-break: break-all;">
        </h3>
        <div class="head-reveal-modal">
            <div class="title-reveal-modal" style="color: Blue;">
                Message</div>
            <div class="close-reveal-modal">
                &#215;</div>
        </div>
    </div>
    </form>
</body>
</html>
