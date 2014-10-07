function clientChanged(element, reportId, left, top, width, height, frameId, title) {
    var clientId = element.options[element.selectedIndex].value;

    //console.log("***Client changed, drawing the chart again: " + reportId + ", " + left + ", " + top + ", " + width + ", " + height + ", " + frameId);


    $(".reportPanel").map(function () {
        var _reportId = $(this).attr("id").replace("Container", "");
        var _left = $(this).css("left");
        var _top = $(this).css("top");
        var _width = $(this).css("width");
        var _height = $(this).css("height");
        var _frameId = $(this).attr("id").replace("Container", "dashboard");;
        //console.log("      Client changed, drawing the chart again: " + _reportId + ", " + _left + ", " + _top + ", " + _width + ", " + _height + ", " + _frameId);


        drawMyChartInWrapper(_reportId, _left, _top, _width, _height, _frameId, "", clientId);
    });
}



function drawMyChartInWrapper(reportId, left, top, width, height, frameId, title, clientId) {
    //console.log("*** Drawing chart: " + reportId + ", " + left + ", " + top + ", " + width + ", " + height + ", " + frameId + ", " + title + ", " + clientId);

    var dataTable = new window.google.visualization.DataTable();
    var requestUrl = '/Reports/Reports/GetCftReportJson?reportId=' + reportId;
    if (clientId > 0) {
        requestUrl += "&clientId=" + clientId;


    }

    var xmlhttp;
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    xmlhttp.onreadystatechange = function () {

        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var frame = document.getElementById(frameId);

            //if (frame != null)
            //console.log("      Frame element found, FrameId:" + frameId);
            //else
            //console.log("      Frame element no found, FrameId:" + frameId);

            var chartContainerWidth = Math.floor(width * 0.95);
            var filterWidth = Math.floor(width * 0.18);

            var responseText = xmlhttp.responseText;
            var responseObject = JSON.parse(responseText).Result;

            var haxisTitle = (responseObject.hAxis != null ? responseObject.hAxis : "");
            var vaxisTitle = (responseObject.hAxis != null ? responseObject.vAxis : "");

            var htmlOutput =
                  '<div style="z-index:200;"><h2 style="padding:0;margin:0;font-size:1.2em;font-family:Arial;font-weight:bold;">' + title + '</h2>';

            if (responseObject.Description != null && responseObject.Description.length > 0)
                htmlOutput += '<p style="padding:0;margin:0;font-size:0.8em;font-family:Arial;">' + responseObject.Description + '</p>';


            htmlOutput +=
                 '</div>' +
                '<div id="Frame' + reportId + '" class="chartContainer" style="width:' + chartContainerWidth + 'px;display:inline-block;vertical-align:top;padding:0;margin:0;">' +
                '</div>';




            var dashboardContainer = document.createElement("div");
            dashboardContainer.className = "dashboardContainer";

            dashboardContainer.innerHTML =
                  '<div id="Container' + reportId + '" class="reportPanel" style="position:fixed;display:inline-block;width:' + width + 'px;height:' + height + 'px;left:' + left + 'px;top:' + top + 'px;vertical-align:top;">' +
                        htmlOutput + '</div>';







            if (responseObject != null && responseObject.GoogleDataTableColumns != null) {
                for (var columnIndex = 0; columnIndex < responseObject.GoogleDataTableColumns.length; columnIndex++) {

                    dataTable.addColumn(responseObject.GoogleDataTableColumns[columnIndex].Type, responseObject.GoogleDataTableColumns[columnIndex].Label);

                }

            }



            if (responseObject != null && responseObject.GoogleDataTableRows != null) {
                dataTable.addRows(responseObject.GoogleDataTableRows.length);


                for (var rowIndex = 0; rowIndex < responseObject.GoogleDataTableRows.length; rowIndex++) {
                    for (columnIndex = 0; columnIndex < responseObject.GoogleDataTableColumns.length; columnIndex++) {
                        switch (responseObject.GoogleDataTableColumns[columnIndex].Type) {
                            case "string":
                                dataTable.setCell(rowIndex, columnIndex, responseObject.GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].StringValue);
                                break;

                            case "integer":
                                dataTable.setCell(rowIndex, columnIndex, responseObject.GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].IntegerValue);
                                break;

                            case "number":
                                dataTable.setCell(rowIndex, columnIndex, responseObject.GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].IntegerValue);
                                break;

                            case "date":
                                dataTable.setCell(rowIndex, columnIndex, new Date(parseInt(responseObject.GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].DateTimeValue.replace("/Date(", "").replace(")/", ""), 10)));
                                break;

                            case "datetime":
                                dataTable.setCell(rowIndex, columnIndex, responseObject.GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].DateTimeValue);
                                break;

                            default:
                                break;
                        }
                    }
                }


            }
            var categoryPickers = [];


            if (responseObject != null && responseObject.Filters != null) {
                //console.log("      Chart has " + responseObject.Filters.length + " filter(s)");

                var filtersElement = null;
                var filtersUL = null;
                if (document.getElementById("filtersUL") == null) {
                    filtersUL = document.createElement("ul");
                    filtersUL.setAttribute("id", "filtersUL");
                    if (frame != null)
                        frame.appendChild(filtersUL);
                }
                else
                    filtersUL = document.getElementById("filtersUL");



                if (document.querySelectorAll(".filters") == 0) {
                    //console.log("      Could not find filters div");
                    filtersElement = document.createElement("div");
                    filtersElement.className = "filters";
                    //console.log("      Appending filters div");
                    frame.appendChild(filtersElement);

                }
                else {
                    //console.log("      Found filters div");
                    filtersElement = document.querySelectorAll(".filters")[0];
                }

                if (filtersElement != null)
                    filtersElement.appendChild(filtersUL);

                //filtersUL = document.getElementById("filtersUL");

                var filtersULHTML = "";


                for (var filterIndex = 0; filterIndex < responseObject.Filters.length; filterIndex++) {
                    var filter = responseObject.Filters[filterIndex];
                    var filterElementId = 'filter-' + filterIndex + '-' + reportId;
                    if (document.getElementById(filterElementId) == null) {
                        var filterLi = document.createElement("li");
                        filterLi.setAttribute("id", filterElementId);
                        //console.log("      FilterId: " + filterElementId);
                        filtersUL.appendChild(filterLi);



                        //console.log("      filterElementId: " + filterElementId);
                        var categoryPicker = new window.google.visualization.ControlWrapper({
                            'controlType': 'CategoryFilter',
                            'containerId': filterElementId,
                            'options': {
                                'filterColumnLabel': filter,
                                'ui': {
                                    'allowTyping': false,
                                    'allowMultiple': true,
                                    'selectedValuesLayout': 'belowStacked'
                                }
                            },
                            // Define an initial state, i.e. a set of metrics to be initially selected.
                            'state': {}
                        });
                        categoryPickers.push(categoryPicker);
                    }
                }

                if (document.querySelectorAll(".clientDropDown").length == 0) {
                    var clientsFilter = document.createElement("div");
                    clientsFilter.className = "clientsFilter";
                    if (responseObject.ClientList != null) {

                        var clientHtml =
                            '<div class="google-visualization-controls-categoryfilter"><label class="google-visualization-controls-label">Client</label>' +
                            '<select class="clientDropDown" name="ClientFilter" onchange="clientChanged(this,' + reportId + ',' + left + ',' + top + ',' + width + ',' + height + ',\'' + frameId + '\',\'' +
                            (title != null ? title : "") + '\');"><option value="-1">Choose a client...</option>';

                        //console.log("      DrawmyChartinWrapper Function: number of clients: " + responseObject.ClientList.length)
                        for (var clientIndex = 0; clientIndex < responseObject.ClientList.length; clientIndex++) {
                            clientHtml +=
                                '<option value="' + responseObject.ClientList[clientIndex].ClientID + '"' +
                                ((clientId == responseObject.ClientList[clientIndex].ClientID) ? " selected " : "") +
                                '>' + responseObject.ClientList[clientIndex].Name + '</option>';
                        }
                        clientHtml +=
                            '</select>';

                        clientsFilter.innerHTML = clientHtml;
                    }
                    frame.appendChild(clientsFilter);
                }
            }




            var dashboard = new window.google.visualization.Dashboard(dashboardContainer);


            var options = {
                colors: responseObject.Colours,
                vAxis: { title: responseObject.HAxis, titleTextStyle: { color: responseObject.HAxisColour } },
                hAxis: { title: responseObject.VAxis, titleTextStyle: { color: responseObject.VAxisColour } }
            };


            var chart = new window.google.visualization.ChartWrapper({
                'chartType': responseObject.ChartType,
                'containerId': 'Container' + reportId,
                dataTable: dataTable,
                options: options
            });

            if (categoryPickers.length > 0) {
                for (var i = 0; i < categoryPickers.length; i++) {
                    dashboard.bind(categoryPickers[i], chart);
                    console.log('      Filter is binded to the chart: ' + categoryPickers[i].containerId);
                }



            }
            else {
                //var chartContainer = document.createElement('div');
                //chartContainer.setAttribute("id", 'Frame' + reportId);  
                //frame.appendChild(chart);

            }






            if (frame != null) {
                frame.appendChild(dashboardContainer);
                //console.log("      Dashboard container is added to the frame");
            }

            if (categoryPickers.length > 0) {
                dashboard.draw(dataTable);
                //console.log("      Dashboard is drawn!");
            }
            else {
                //console.log("      No filters!");
                chart.draw(dataTable, options);
                //console.log("      No filters so chart is drawn!");
            }
        }
    }
    xmlhttp.open("GET", requestUrl, true);
    xmlhttp.send();
}

function drawMyChart(reportId, left, top, width, height, frameId, title, clientId) {
    var dataTable = new window.google.visualization.DataTable();
    var requestUrl = '/Reports/Reports/GetCftReportJson?reportId=' + reportId;
    if (clientId > 0) {
        requestUrl += "&clientId=" + clientId;


    }

    var xmlhttp;
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    xmlhttp.onreadystatechange = function () {

        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {


            var chartContainerWidth = Math.floor(width * 0.80);
            var filterWidth = Math.floor(width * 0.18);

            var responseText = xmlhttp.responseText;
            var responseObject = JSON.parse(responseText).Result;

            var haxisTitle = (responseObject.hAxis != null ? responseObject.hAxis : "");
            var vaxisTitle = (responseObject.hAxis != null ? responseObject.vAxis : "");

            var htmlOutput =
                  '<div style="z-index:200;"><h2 style="padding:0;margin:0;font-size:1.2em;font-family:Arial;font-weight:bold;">' + title + '</h2>';

            if (responseObject.Description != null && responseObject.Description.length > 0)
                htmlOutput += '<p style="padding:0;margin:0;font-size:0.8em;font-family:Arial;">' + responseObject.Description + '</p>';

            htmlOutput += '</div>';
            htmlOutput +=
                '<div id="Frame' + reportId + '" class="chartContainer" style="width:' + chartContainerWidth + 'px;display:inline-block;vertical-align:top;padding:0;margin:0;">' +
                '</div>' +
                '<div class="filters" style="width:' + filterWidth + 'px;display:inline-block;padding:20px 0 0 0;margin:0;vertical-align:top;">' +
                '<ul style="padding:0;margin:0;text-aling:left;">';

            if (responseObject.ClientList != null) {

                htmlOutput +=
                    '<li>' +
                    '<div class="google-visualization-controls-categoryfilter"><label class="google-visualization-controls-label">Client</label>' +
                    '<select class="clientDropDown" name="ClientFilter" onchange="clientChanged(this,' + reportId + ',' + left + ',' + top + ',' + width + ',' + height + ',\'' + frameId + '\',\'' +
                    (title != null ? title : "") + '\');"><option value="-1">Choose a client...</option>';

                for (var clientIndex = 0; clientIndex < responseObject.ClientList.length; clientIndex++) {
                    htmlOutput +=
                        '<option value="' + responseObject.ClientList[clientIndex].ClientID + '"' +
                        ((clientId == responseObject.ClientList[clientIndex].ClientID) ? " selected " : "") +
                        '>' + responseObject.ClientList[clientIndex].Name + '</option>';
                }
                htmlOutput +=
                    '</select></li>';


            }

            if (responseObject.Filters != null)
                for (var filterIndex = 0; filterIndex < responseObject.Filters.length; filterIndex++) {
                    htmlOutput +=
                        '<li id="filter' + frameId + reportId + '-' + filterIndex + '"></li>';
                }



            htmlOutput +=
                '</ul></div>';



            if (clientId != 0) {
                //var containerElement = "Container" + reportId; 
                //alert(document.getElementById(containerElement).innerHTML)
            }
            else {
                document.getElementsByTagName("body")[0].innerHTML +=
                        '<div id="Container' + reportId + '" class="reportPanel" style="position:fixed;display:inline-block;width:' + width + 'px;height:' + height + 'px;left:' + left + 'px;top:' + top + 'px;vertical-align:top;">' +
                        htmlOutput + '</div>';
            }

            if (responseObject != null && responseObject.GoogleDataTableColumns != null) {
                for (var columnIndex = 0; columnIndex < responseObject.GoogleDataTableColumns.length; columnIndex++) {

                    dataTable.addColumn(responseObject.GoogleDataTableColumns[columnIndex].Type, responseObject.GoogleDataTableColumns[columnIndex].Label);

                }

            }



            if (responseObject != null && responseObject.GoogleDataTableRows != null) {
                dataTable.addRows(responseObject.GoogleDataTableRows.length);

                for (var rowIndex = 0; rowIndex < responseObject.GoogleDataTableRows.length; rowIndex++) {
                    for (columnIndex = 0; columnIndex < responseObject.GoogleDataTableColumns.length; columnIndex++) {
                        switch (responseObject.GoogleDataTableColumns[columnIndex].Type) {
                            case "string":
                                dataTable.setCell(rowIndex, columnIndex, responseObject.GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].StringValue);
                                break;

                            case "integer":
                                dataTable.setCell(rowIndex, columnIndex, responseObject.GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].IntegerValue);
                                break;

                            case "number":
                                dataTable.setCell(rowIndex, columnIndex, responseObject.GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].IntegerValue);
                                break;

                            case "date":
                                dataTable.setCell(rowIndex, columnIndex, new Date(parseInt(responseObject.GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].DateTimeValue.replace("/Date(", "").replace(")/", ""), 10)));
                                break;

                            case "datetime":
                                dataTable.setCell(rowIndex, columnIndex, responseObject.GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].DateTimeValue);
                                break;

                            default:
                                break;
                        }
                    }
                }


            }
            var categoryPickers = [];


            if (responseObject != null && responseObject.Filters != null) {
                for (filterIndex = 0; filterIndex < responseObject.Filters.length; filterIndex++) {
                    var filter = responseObject.Filters[filterIndex];

                    var categoryPicker = new window.google.visualization.ControlWrapper({
                        'controlType': 'CategoryFilter',
                        'containerId': 'filter' + frameId + reportId + '-' + filterIndex,
                        'options': {
                            'filterColumnLabel': filter.Name,
                            'ui': {
                                'allowTyping': false,
                                'allowMultiple': true,
                                'selectedValuesLayout': 'belowStacked'
                            }
                        },
                        // Define an initial state, i.e. a set of metrics to be initially selected.
                        'state': {}
                    });
                    categoryPickers.push(categoryPicker);
                }
            }

            var frame = document.getElementById(frameId);

            if (frame == null) {
                document.getElementsByTagName("body")[0].innerHTML += "<div id='" + frameId + "'></div>";
                frame = document.getElementById(frameId);

            }

            var dashboard = new window.google.visualization.Dashboard(frame);

            var options = {
                colors: responseObject.Colours,
                vAxis: { title: responseObject.HAxis, titleTextStyle: { color: responseObject.HAxisColour } },
                hAxis: { title: responseObject.VAxis, titleTextStyle: { color: responseObject.VAxisColour } }
            };


            var chart = new window.google.visualization.ChartWrapper({
                'chartType': responseObject.ChartType,
                'containerId': 'Frame' + reportId,
                dataTable: dataTable,
                options: options
            });

            if (categoryPickers.length > 0) {
                for (var i = 0; i < categoryPickers.length; i++) {
                    dashboard.bind(categoryPickers[i], chart);
                }
                dashboard.draw(dataTable);
            }
            else {
                chart.draw(dataTable, options);
            }
        }
    }
    xmlhttp.open("GET", requestUrl, true);
    xmlhttp.send();
}

function DrawClientDropDown(element, ClientList) {
    if (ClientList != null) {

        var htmlOutput =
            '<div class="google-visualization-controls-categoryfilter"><label class="google-visualization-controls-label">Client</label>' +
            '<select class="clientDropDown" name="ClientFilter" onchange="clientChangedII(this);"><option value="-1">Choose a client...</option>';

        for (var clientIndex = 0; clientIndex < responseObject.ClientList.length; clientIndex++) {
            htmlOutput +=
                '<option value="' + responseObject.ClientList[clientIndex].ClientID + '"' +
                ((clientId == responseObject.ClientList[clientIndex].ClientID) ? " selected " : "") +
                '>' + responseObject.ClientList[clientIndex].Name + '</option>';
        }
        htmlOutput +=
            '</select>';

        $(element).html(htmlOutput);
    }
}
