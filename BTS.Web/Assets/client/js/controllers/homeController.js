var homeController = {
    init: function () {
        homeController.registerEvent();

        myChart.loadPieChart("/Home/StatCerByOperator", "#chart_StatCerByOperator");
        //myChart.loadPieChart("/Home/StatBtsInProcess", "#chart_StatBtsInProcess");
        myChart.loadBarChart("/Home/StatCerByOperatorArea", "#chart_StatCerByOperatorArea", "Khu vực", "Giấy CNKĐ", "Nhà mạng");

        myChart.loadBarChart("/Home/StatExpiredInYearCerByOperatorArea", "#chart_StatExpiredInYearCerByOperatorArea", "Khu vực", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadBarChart("/Home/StatBtsByOperatorArea", "#chart_StatBtsByOperatorArea", "Khu vực", "Số BTS", "Nhà mạng");

        myChart.loadStackedBarChart("/Home/StatCerByOperatorCity", "#chart_StatCerByOperatorCity", "Tỉnh/ Thành phố", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadBarChart("/Home/StatCerByOperatorYear", "#chart_StatCerByOperatorYear", "Năm", "Giấy CNKĐ", "Nhà mạng");
        
        myChart.loadPieChart("/Home/StatBtsByBand", "#chart_StatBtsByBand");
        myChart.loadPieChart("/Home/StatBtsByManufactory", "#chart_StatBtsByManufactory");
        
        myChart.loadStackedBarChart("/Home/StatBtsByBandCity", "#chart_StatBtsByBandCity", "Tỉnh/ Thành phố", "Số trạm BTS", "Băng tần");
        myChart.loadStackedBarChart("/Home/StatBtsByOperatorCity", "#chart_StatBtsByOperatorCity", "Tỉnh/ Thành phố", "Số trạm BTS", "Nhà mạng");
    },
    registerEventDataTable: function () {
    },
    registerEvent: function () {
    },
}
homeController.init();