var homeController = {
    init: function () {
        homeController.registerEvent();

        myChart.loadPieChart("/Home/StatCoupleCerByOperator", "#chart_StatCoupleCerByOperator", true);
        //myChart.loadPieChart("/Home/StatBtsInProcess", "#chart_StatBtsInProcess");
        myChart.loadBarChart("/Home/StatCerByOperator", "#chart_StatCerByOperator", "", "Giấy CNKĐ", "Nhà mạng");

        myChart.loadBarGroupChart("/Home/StatExpiredCerByOperatorYear", "#chart_StatExpiredCerByOperatorYear", "", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadBarGroupChart("/Home/StatCerByOperatorYear", "#chart_StatCerByOperatorYear", "Năm", "Giấy CNKĐ", "Nhà mạng");

        myChart.loadPieChart("/Home/StatBtsByOperatorBand", "#chart_StatBtsByOperatorBand", true);
        myChart.loadPieChart("/Home/StatBtsByOperatorManufactory", "#chart_StatBtsByOperatorManufactory", true);
    },
    registerEventDataTable: function () {
    },
    registerEvent: function () {
    },
}
homeController.init();