if (!window.console) var console = {
    log: function () {

    }
};

function GetRandomStringOOP(length) {
    var randomId = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var k = 0; k < length; k++)
        randomId += possible.charAt(Math.floor(Math.random() * possible.length));

    return randomId;
}


function GetQueryStringValueOOP(queryPart) {
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

function CFTReport(reportId, cftDashboard, top, left, width, height) {
    console.log("CFTReport->Constructor");
    this.ReportId = reportId;
    this.Left = left;
    this.Top = top;
    this.Width = width;
    this.Height = height;
    this.DashboardElementId = 0;
    this.Title = "";
    this.Description = "";
    this.ClientId = 0;
    this.Filters = [];
    this.ChartType = "";
    this.ChartContainer = null;
    this.Dashboard = cftDashboard;
    this.DataIsLoaded = false;
    this.ChartContainerId = "";
    this.vAxisTitle = "";
    this.vAxixColour = "";
    this.hAxisTitle = "";
    this.hAxixColour = "";
    this.DataTable = new window.google.visualization.DataTable();
    this.ClientsList = [];
    this.TitleElement = null;
    this.DescriptionElement = null;

    this.LoadReport = function (cftReport) {
        var requestUrl = '/Reports/Reports/GetCftReportJson?reportId=' + this.ReportId;
        if (this.ClientId > 0) {
            requestUrl += "&clientId=" + ClientId;
        }

        $.ajax({
            url: requestUrl,
            type: "GET",
            context: this,
            success: cftReport.ReportLoaded,
            error: function (xhr, ajaxOptions, thrownError) {
                //alert(xhr.status);
                //alert(thrownError);
            }
        });
    }

    this.ReportLoaded = function (data) {

        this.DashboardElementId = data.Result.DashboardElementId;
        this.Title = data.Result.Title;
        this.Description = data.Result.Description;
        this.ClientId = data.Result.ClientId;
        this.Filters = data.Result.Filters;
        this.ChartType = data.Result.ChartType;
        this.vAxisTitle = data.Result.vAxisTitle;
        this.vAxixColour = data.Result.vAxixColour;
        this.hAxisTitle = data.Result.hAxisTitle;
        this.hAxixColour = data.Result.hAxixColour;

        console.log("CFTReport->LoadData: ReportId:" + this.ReportId);

        var GoogleDataTableColumns = data.Result.GoogleDataTableColumns;
        var GoogleDataTableRows = data.Result.GoogleDataTableRows;

        for (var columnIndex = 0; columnIndex < GoogleDataTableColumns.length; columnIndex++) {
            this.DataTable.addColumn(GoogleDataTableColumns[columnIndex].Type, GoogleDataTableColumns[columnIndex].Label);
            console.log("CFTReport->LoadData: Column added:" + GoogleDataTableColumns[columnIndex].Label);
        }

        this.DataTable.addRows(GoogleDataTableRows.length);

        for (var rowIndex = 0; rowIndex < GoogleDataTableRows.length; rowIndex++) {
            for (columnIndex = 0; columnIndex < GoogleDataTableColumns.length; columnIndex++) {
                switch (GoogleDataTableColumns[columnIndex].Type) {
                    case "string":
                        this.DataTable.setCell(rowIndex, columnIndex, GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].StringValue);
                        break;

                    case "integer":
                        this.DataTable.setCell(rowIndex, columnIndex, GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].IntegerValue);
                        break;

                    case "number":
                        this.DataTable.setCell(rowIndex, columnIndex, GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].IntegerValue);
                        break;

                    case "date":
                        this.DataTable.setCell(rowIndex, columnIndex, new Date(parseInt(GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].DateTimeValue.replace("/Date(", "").replace(")/", ""), 10)));
                        break;

                    case "datetime":
                        this.DataTable.setCell(rowIndex, columnIndex, GoogleDataTableRows[rowIndex].GoogleDataRecords[columnIndex].DateTimeValue);
                        break;

                    default:
                        break;
                }
            }
        }
        console.log("CFTReport->LoadData: Rows added:" + GoogleDataTableRows.length);
        this.ChartType = data.Result.ChartType;
        console.log("CFTReport->LoadData: ChartType:" + this.ChartType);
        console.log("CFTReport->LoadData: Number of Columns:" + this.DataTable.getNumberOfColumns());
        console.log("CFTReport->LoadData: Number of Rows:" + this.DataTable.getNumberOfRows());
        this.ClientList = data.Result.ClientList;
        this.Title = data.Result.Name;
        this.DataIsLoaded = true;


        if (this.TitleElement != null)
            this.TitleElement.innerText = this.Title;

        if (this.DescriptionElement != null && data.Result.Description != null && data.Result.Description != "")
            this.DescriptionElement.innerText = data.Result.Description;

        if (this.Dashboard != null)
            this.Dashboard.LoadedAlert(this);
        else
            alert("No dashboards!" + this.ReportId);

    }

    this.DrawChart = function () {
        if (this.ChartContainer != null) {
            console.log("CFTReport->LoadData:ChartContainer: " + this.ChartContainer.getAttribute("id"));
            var chart = new window.google.visualization.ChartWrapper({
                'chartType': this.ChartType,
                'containerId': this.ChartContainer.getAttribute("id"),
                dataTable: this.DataTable
            });


        }
    }


    this.ReloadReportData = function (cftReport, clientId) {
        var requestUrl = '/Reports/Reports/GetCftReportJson?reportId=' + this.ReportId + "&clientId=" + clientId;

        $.ajax({
            url: requestUrl,
            type: "GET",
            context: this,
            success: cftReport.ReportLoaded,
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
}


function CFTFilter(fieldId, name) {
    this.FieldId = fieldId;
    this.Name = name;
}

function CFTDashboard(dashboardId, ParentId, reportId) {
    console.log("CFTDashboard->Constructor");
    this.Reports = new Array(0);
    this.ReportsURL = "/Customise/GetPageElementListByDashboardId?dashboardId=";
    this.DashboardId = (dashboardId != null) ? dashboardId : 0;
    this.Filters = [];
    this.DashboardElement = null;
    this.ChartElement = null;


    this.ClientElement = null;
    this.ParentId = ParentId;
    this.ReportId = reportId;

    this.LoadDashboard = function (dashboard) {
        console.log("CFTDashboard->LoadDashboard");
        if (this.DashboardId >= 0) {
            console.log("CFTDashboard->LoadDashboard: DashboardId" + this.DashboardId);
            var requestUrl = this.ReportsURL + this.DashboardId;
            if (this.ReportId > 0)
                requestUrl += "&reportId=" + this.ReportId;
            $.ajax({
                url: requestUrl,
                type: "GET",
                context: this,
                success: dashboard.DashboardDataReceived,
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }
    }

    this.DashboardDataReceived = function (data) {
        console.log("CFTDashboard->LoadReports");
        var dashboardData = data.Result;
        for (var i = 0; i < dashboardData.length; i++) {
            var cftReport = new CFTReport(dashboardData[i].ReportId, this, dashboardData[i].Top, dashboardData[i].Left, dashboardData[i].Width, dashboardData[i].Height);
            for (var j = 0; j < dashboardData[i].Filters.length; j++) {
                var filter = new CFTFilter(dashboardData[i].Filters[j].FieldId, dashboardData[i].Filters[j].Name);

                var isUnique = true;
                for (k = 0; k < this.Filters.length; k++) {
                    if (filter.Name == this.Filters[k].Name && filter.FieldId == this.Filters[k].FieldId)
                        isUnique = false;
                }

                if (isUnique) {
                    console.log("CFTDashboard->LoadReports: Filter added: " + dashboardData[i].Filters[j].FieldId + "-" + dashboardData[i].Filters[j].Name);
                    this.Filters.push(filter);
                }
                else {
                    console.log("CFTDashboard->LoadReports: Filter is not unique: " + dashboardData[i].Filters[j].FieldId + "-" + dashboardData[i].Filters[j].Name);
                }
            }
            this.Reports.push(cftReport);
        }
        console.log("CFTDashboard->LoadReports: Number of reports loaded: " + this.Reports.length);
        console.log("CFTDashboard->LoadReports: Number of filters loaded: " + this.Filters.length);
        this.DrawSkeleton();
        this.DrawReports();
    }

    this.DrawSkeleton = function (dashboard) {
        var dashboardContainer = document.createElement("div");
        dashboardContainer.className = "dashboardContainer";
        dashboardContainer.setAttribute("id", "Dashboard" + this.DashboardId + GetRandomStringOOP(5));


        var chartsContainer = document.createElement("div");
        chartsContainer.className = "chartsContainer";
        dashboardContainer.appendChild(chartsContainer);

        console.log("CFTDashboard->DrawSkeleton: Number of reports: " + this.Reports.length);
        for (var i = 0; i < this.Reports.length; i++) {
            var chartTitleContainer = document.createElement("div");
            chartTitleContainer.className = "chartTitle";

            var chartTitle = document.createElement("strong");
            this.Reports[i].TitleElement = chartTitle;
            chartTitleContainer.appendChild(chartTitle);


            var chartSubTitleContainer = document.createElement("p");
            this.Reports[i].DescriptionElement = chartSubTitleContainer;
            chartTitleContainer.appendChild(chartSubTitleContainer);



            var dashboard = document.createElement("div");
            dashboard.className = "dashboard";
            //var dashboardStyles = "top:" + this.Reports[i].Top + "px;left:" + this.Reports[i].Left + "px;position:relative;";
            //dashboard.setAttribute("style", dashboardStyles);
            dashboard.appendChild(chartTitleContainer);
            chartsContainer.appendChild(dashboard);

            var chart = document.createElement("div");
            chart.className = "chart";
            chart.setAttribute("ReportId", this.Reports[i].ReportId);
            chart.setAttribute("DashboardId", this.DashboardId);


            var chartId = "Chart" + this.Reports[i].ReportId + GetRandomStringOOP(5);
            chart.setAttribute("id", chartId);

            this.Reports[i].ChartContainerId = chartId;
            dashboard.appendChild(chart);

            this.Reports[i].DashboardElement = dashboard
            this.Reports[i].ChartElement = chart;
        }

        var filtersContainer = document.createElement("div");
        filtersContainer.className = "filtersContainer";
        dashboardContainer.appendChild(filtersContainer);

        var filtersList = document.createElement("ul");
        filtersList.className = "filtersList";
        filtersContainer.appendChild(filtersList);



        var clientFiltersListElement = document.createElement("li");
        clientFiltersListElement.className = "clientFilters filter";
        filtersList.appendChild(clientFiltersListElement);

        var cl1 = document.createElement("div");
        cl1.className = "google-visualization-controls-categoryfilter";
        clientFiltersListElement.appendChild(cl1);

        var clientLabel = document.createElement("label");
        clientLabel.className = "google-visualization-controls-label";
        clientLabel.innerText = "Clients";
        cl1.appendChild(clientLabel);



        var clientElement = document.createElement("select");
        clientElement.className = "clientDropDown";
        clientElement.onchange = function (event) {
            var clientId = this.options[this.selectedIndex].value;


            cftDashboard.RefreshCharts(clientId);
        };

        var c2 = document.createElement("div");
        c2.setAttribute("class", "charts-inline-block")
        c2.appendChild(clientElement);


        cl1.appendChild(c2);
        this.ClientElement = clientElement;

        for (var i = 0; i < this.Filters.length; i++) {
            var filter = document.createElement("li");
            filter.setAttribute("id", "Filter" + this.Filters[i].FieldId);
            filter.className = "filter";
            filtersList.appendChild(filter);
        }

        if (this.Filters.length == 0) {
            var filter = document.createElement("li");
            filter.setAttribute("id", "NoFilters");
            filter.setAttribute("style", "visibility:hidden;");
            filter.className = "filter";
            filtersList.appendChild(filter);
        }

        document.getElementById(this.ParentId).appendChild(dashboardContainer);
    }

    this.DrawReports = function () {
        for (i = 0; i < this.Reports.length; i++) {
            this.Reports[i].Dashboard = this;
            this.Reports[i].LoadReport(this.Reports[i]);
        }
    }

    this.LoadedAlert = function (report) {
        console.log("***LoadedAlert Called***");
        var allChartsAreLoaded = true;
        for (i = 0; i < this.Reports.length; i++)
            allChartsAreLoaded = allChartsAreLoaded && this.Reports[i].DataIsLoaded;

        if (allChartsAreLoaded) {
            console.log("***allChartsAreLoaded***");
            var filtersControls = [];
            for (i = 0; i < this.Filters.length; i++) {
                var filter = new window.google.visualization.ControlWrapper({
                    'controlType': 'CategoryFilter',
                    'containerId': "Filter" + this.Filters[i].FieldId,
                    'options': {
                        'filterColumnLabel': this.Filters[i].Name,
                        'ui': {
                            'allowTyping': false,
                            'allowMultiple': true,
                            'selectedValuesLayout': 'belowStacked'
                        }
                    },
                    // Define an initial state, i.e. a set of metrics to be initially selected.
                    'state': {}
                });

                filtersControls.push(filter);
            }


            for (i = 0; i < this.Reports.length; i++) {
                if (this.Reports[i].ClientList != null) {
                    for (var clientIndex = 0; clientIndex < this.Reports[i].ClientList.length; clientIndex++) {
                        var newClientId = this.Reports[i].ClientList[clientIndex].ClientID;
                        var isUniqueClient = true;
                        for (j = 0; j < this.ClientElement.options.length; j++) {
                            var currentValue = this.ClientElement.options[j].value;
                            if (currentValue == newClientId)
                                isUniqueClient = false;
                        }

                        if (isUniqueClient) {
                            var client = document.createElement("option");
                            client.setAttribute("value", this.Reports[i].ClientList[clientIndex].ClientID);
                            client.innerText = this.Reports[i].ClientList[clientIndex].Name;
                            this.ClientElement.appendChild(client);
                        }
                    }
                }



                console.log("dashbord: " + this.Reports[i].DashboardElement + ", ReportId: " + this.Reports[i].ReportId);
                var dashboard = new window.google.visualization.Dashboard(this.Reports[i].DashboardElement);


                console.log("Width:" + this.Reports[i].Width + ", Height: " + this.Reports[i].Height);
                console.log("Left:" + this.Reports[i].Left + ", Top: " + this.Reports[i].Top);
                var options = {
                    vAxis: { title: this.Reports[i].hAxisTitle, titleTextStyle: { color: this.Reports[i].hAxisColour } },
                    hAxis: { title: this.Reports[i].vAxisTitle, titleTextStyle: { color: this.Reports[i].vAxisColour } },
                    // chartArea: { top: this.Reports[i].Top, left: this.Reports[i].Left },
                    width: this.Reports[i].Width,
                    height: this.Reports[i].Height
                };

                var chart = new window.google.visualization.ChartWrapper({
                    'chartType': this.Reports[i].ChartType,
                    'containerId': this.Reports[i].ChartContainerId
                    , options: options
                });



                for (j = 0; j < this.Filters.length; j++) {
                    for (k = 0; k < this.Reports[i].Filters.length; k++) {
                        if (this.Filters[j].FieldId == this.Reports[i].Filters[k].FieldId) {
                            dashboard.bind(filtersControls[j], chart);

                            console.log("dashboard binding chart: '" + this.Reports[i].Title + "' and filter: '" + this.Filters[j].Name + "'");
                        }
                    }
                }


                this.Reports[i].DashboardElement.setAttribute(
                    "style",
                    "left:" + this.Reports[i].Left + "px;top:" + this.Reports[i].Top + "px;position:absolute;display:inline-block;");


                if (this.Filters.length == 0) {
                    var filter = new window.google.visualization.ControlWrapper({
                        'controlType': 'CategoryFilter',
                        'containerId': 'NoFilters',
                        'options': {
                            'filterColumnLabel': this.Reports[i].DataTable.getColumnLabel(0),
                            'ui': {
                                'allowTyping': false,
                                'allowMultiple': true,
                                'selectedValuesLayout': 'belowStacked'
                            }
                        },
                        // Define an initial state, i.e. a set of metrics to be initially selected.
                        'state': {}
                    });

                    dashboard.bind(filter,chart);
                }

               dashboard.draw(this.Reports[i].DataTable);
            }

        }
    }

    this.RefreshCharts = function (clientId) {
        for (i = 0; i < this.Reports.length; i++) {
            this.Reports[i].ReloadReportData(this.Reports[i], clientId);
        }
    }
}

