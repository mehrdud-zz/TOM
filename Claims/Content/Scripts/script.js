var CLIPBOARD = "";

function ShowHide(sender, target) {

    var currentState = $("#" + target).css('visibility');

    if (currentState != null && currentState == 'hidden') {
        $("#" + target).css('visibility', 'visible');
    }
    else {
        $("#" + target).css('visibility', 'hidden');
    }
}

function VoidLink() {
    return false;
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function SelectAllRows(element) {
    var rowSelectors = document.getElementsByName("rowSelector");

    var checked = $(element).is(":checked");


    for (var i = 0; i < rowSelectors.length; i++) {

        if (checked) {
            $(rowSelectors[i]).prop('checked', true);
        }
        else {
            $(rowSelectors[i]).removeAttr("checked");
        }
    }

}


function GetRandomString(length) {
    var randomId = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var k = 0; k < length; k++)
        randomId += possible.charAt(Math.floor(Math.random() * possible.length));

    return randomId;
}



function CustomiseThisPage() {
    $("div.reportPanel").addClass("panel panel-primary");
    $("div.reportPanel-heading").addClass("Panel-heading");

    $("div.reportPanel").draggable();
    $("div.reportPanel").resizable();

    var customisePanel =
        '<div class="customisePanel">' +
        '<div style="display:inline-block;height:100%;width:100%;vertical-align:middle;magin: 0 auto;"><input type="button" value="Add new report" onclick="AddNewFrame();"/>' +
        ' <input type="button" value="Save changes!" onclick="SavePage();" />' +
        '</div></div>';
    $("#content").append(customisePanel);

    $("a[name=closePanelLink]").css("display", "inline-block");
}

function closePanel(panelId) {
    $("#" + panelId).remove();
}


function ShowReportTemplate(frameId, reportSelectId) {
    var reportId = $("#" + reportSelectId).val();
    var newPanel =

    '<div id="' + frameId + reportId + '" class="chartContainer"  >' +
    '<h3>Second step:</h3><p>Drag and resize this panel, press "Save changes" to see the report</p>' +
    '</div>' +
    '<input type="hidden" name="reportID" id="Report' + frameId + '" value="' + reportId + '" />';

    $("#" + frameId).html(newPanel);
    $("#" + frameId).draggable();
    $("#" + frameId).resizable();

}



function AddNewFrame() {
    var randomId = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var k = 0; k < 20; k++)
        randomId += possible.charAt(Math.floor(Math.random() * possible.length));

    var frameId = "Frame" + randomId;

    var newPanel =
    '<div id="' + frameId + '" class="reportPanel panel panel-primary" style="padding:5px; display:inline-block;position:absolute;left:100px;top:150px;background-color:#f1f1f1;width:320px;height:240px;border: 1px solid #428bca;">' +
    '<div  style="width:100%;height:100%;display:inline-block;"></div>' +
    '<input type="hidden" name="reportID" id="Report' + frameId + '" value="0" />' +
    '</div>';

    $("#content").append(newPanel);
    $.get("/Reports/Reports/GetReportTemplates", {}, function (data) {
        var claimTemplateId = "";
        $.each(data.Result, function () {
            claimTemplateId += '<option value="' + this.ReportID + '">' + this.Name + '</option>';
        });
        claimTemplateId =
            '<h3>First step:</h3><p>Select the report you want to see:&nbsp;<select id="Select' +
            frameId + '" onchange="ShowReportTemplate(\'' + frameId + '\',\'Select' + frameId + '\');"><option>(Select a report)</option>' +
            claimTemplateId + '</select></p>';

        $("#" + frameId).html(claimTemplateId);
    });
}


function SavePage() {
    var reportDetails = [];

    $("div.reportPanel").map(function () {
        var frameId = $(this).attr("id");
        var reportId = $("#" + frameId + " input").val();
        var width = $(this).width();
        var height = $(this).height();

        if (reportId != null) {
            reportDetails.push("{\"ReportID\":" + reportId + ",\"Width\":" + width + ",\"Height\":" + height + ",\"Left\":" + $(this).position().left + ",\"Top\":" + $(this).position().top
        + ",\"FrameID\":\"" + frameId + "\"}");

        }
    }).get();




    var jsonString = "{\"PageElementDetails\":[" + reportDetails.join(',') + "],\"PageURL\":\"" + window.location + "\"}";
    var obj1 = jQuery.parseJSON(jsonString);

    $.ajax({
        type: 'POST',
        url: "/Customise/SavePage",
        data: { jsonOfLog: JSON.stringify(obj1) },
        dataType: "json",
        traditional: true,
        success: function () {
            window.location.reload();
        }
    });
}




function GetQueryStringValue(queryPart) {
    var result = {}, keyValuePairs = location.search.slice(1).split('&');

    if (keyValuePairs != null)
        for (var i = 0; i < keyValuePairs.length; i++) {
            if (keyValuePairs[i] != null) {
                var keyValuePair = keyValuePairs[i].split('=');
                result[keyValuePair[0]] = keyValuePair[1] || '';
            }
        }
    return result[queryPart];
}

function GetPageElements() {
    var reportId = GetQueryStringValue("ReportID");
    var requestUrl = "/Customise/GetPageElementListByUsernameandPageUrl?pageUrl=" + window.location;
    if (reportId != null)
        requestUrl += '&reportId=' + reportId;

    $.get(requestUrl, function (data) {
        for (var i = 0; i < data.Result.length; i++) {
            var html = '<iframe src="/SharePointIntegration/SharePointIntegration/SharePointDashboard?ReportID=' + data.Result[i].ReportId + '" width="' + data.Result[i].Width + '" height="' + data.Result[i].Width + '"></iframe>';
            drawMyChart(data.Result[i].ReportId, data.Result[i].Left, data.Result[i].Top, data.Result[i].Width, data.Result[i].Height, data.Result[i].FrameId, data.Result[i].Title, 0);
        }
    });
}



function DrawDashboard() { 
    var dashboardId = GetQueryStringValue("DashboardId");
    ////console.log("DashboardId: " + dashboardId);
    if (dashboardId != null && dashboardId != "") {
        var requestUrl = "/Customise/GetPageElementListByDashboardId?dashboardId=" + dashboardId;
        
        var dashboardElement = document.createElement("div");
        var dashboardElementId = "dashboard" + dashboardId;
        
        $(dashboardElement).attr("id", dashboardElementId);
        $(dashboardElement).addClass("dashboard");
         
        ////console.log("dashboardElement: " + dashboardElement);
        $("#main").append(dashboardElement);

        $.get(requestUrl, function (data) {
            for (var i = 0; i < data.Result.length; i++) {
                ////console.log("Function DrawDashboard: Calling drawMyChartInWrapper " + dashboardId);
                drawMyChartInWrapper(data.Result[i].ReportId, data.Result[i].Left, data.Result[i].Top, data.Result[i].Width, data.Result[i].Height, dashboardElementId, data.Result[i].Title, 0, false);
            }
        });

        var filters = document.createElement('div');
        DrawClientDropDown(filters);
    }
}



function DrawDashboardII() {
    var dashboardId = GetQueryStringValue("DashboardId");
    ////console.log("DashboardId: " + dashboardId);
    if (dashboardId != null && dashboardId != "") {
        var requestUrl = "/Customise/GetPageElementListByDashboardId?dashboardId=" + dashboardId;

        var dashboardElement = document.createElement("div");
        var dashboardElementId = "dashboard" + dashboardId;

        $(dashboardElement).attr("id", dashboardElementId);
        $(dashboardElement).addClass("dashboard");


        ////console.log("dashboardElement: " + dashboardElement);
        $("#main").append(dashboardElement);

        $.get(requestUrl, function (data) {
            for (var i = 0; i < data.Result.length; i++) {
                ////console.log("Function DrawDashboard: Calling drawMyChartInWrapper " + dashboardId);
                drawMyChartInWrapper(data.Result[i].ReportId, data.Result[i].Left, data.Result[i].Top, data.Result[i].Width, data.Result[i].Height, dashboardElementd, data.Result[i].Title, 0);
            }
        });

        var filters = document.createElement('div');
        DrawClientDropDown(filters);
    }
}



function changeWindowLocation(newLocation) {
    window.location = newLocation;
}


$(document).ready(function () {

    // top nav
    var area = '@ViewContext.RouteData.DataTokens["area"]';
    var controller = '@HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString()';

    $("ul.nav li").each(function () {
        var url = $(this).find("a").attr("href");
        if (url.indexOf("/" + area + "/" + controller + "/") == 0)
            $(this).addClass("active");
    });


});

//if (google != null) {
//    google.setOnLoadCallback(GetPageElements);
//    //google.setOnLoadCallback(DrawDashboard);
//}