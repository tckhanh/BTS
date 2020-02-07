var statisticsController = {
    areaTabFirstClick: true,
    init: function () {
        statisticsController.registerEvent();
        statisticsController.loadGeneralTab();
    },
    registerEventDataTable: function () {
    },
    registerEvent: function () {
        $('#btnSearch').off('click').on('click', function () {
            if ($('#SelArea').val() != '') {
                statisticsController.loadAreaTab();
            }            
        });

        $("a[href='#areaTab']").on('shown.bs.tab', function (e) {
            if (statisticsController.areaTabFirstClick == true) {
                statisticsController.areaTabFirstClick = false;
                if ($('#SelArea').val() != '') {
                    statisticsController.loadAreaTab();
                }
            }            
        });
    },
    loadGeneralTab() {
        myChart.loadPieChart("/Statistics/StatCoupleCerByArea", "#generalTab-chart_StatCoupleCerByArea");
        //myChart.loadPieChart("/Home/StatBtsInProcess", "#generalTab-chart_StatBtsInProcess");
        myChart.loadBarChart("/Statistics/StatCerByArea", "#generalTab-chart_StatCerByArea", "", "Giấy CNKĐ", "Khu vực");

        myChart.loadBarGroupChart("/Statistics/StatExpiredCerByAreaYear", "#generalTab-chart_StatExpiredCerByAreaYear", "", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadBarGroupChart("/Statistics/StatCerByAreaYear", "#generalTab-chart_StatCerByAreaYear", "Năm", "Giấy CNKĐ", "Nhà mạng");

        myChart.loadPieChart("/Statistics/StatBtsByAreaBand", "#generalTab-chart_StatBtsByAreaBand");
        myChart.loadPieChart("/Statistics/StatBtsByAreaManufactory", "#generalTab-chart_StatBtsByAreaManufactory");
    },
    loadAreaTab: function () {
        myChart.loadLineChart("/Statistics/StatCerByOperatorCity", "#areaTab-chart_StatCerByOperatorCity", "Tỉnh/ Thành phố", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadLineChart("/Statistics/StatNearExpiredInYearCerByOperatorCity", "#areaTab-chart_StatNearExpiredInYearCerByOperatorCity", "Tỉnh/ Thành phố", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadLineChart("/Statistics/StatInYearCerByOperatorCity", "#areaTab-chart_StatInYearCerByOperatorCity", "Tỉnh/ Thành phố", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadLineChart("/Statistics/StatExpiredCerByOperatorCity", "#areaTab-chart_StatExpiredCerByOperatorCity", "Tỉnh/ Thành phố", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadLineChart("/Statistics/StatBtsByBandCity", "#areaTab-chart_StatBtsByBandCity", "Tỉnh/ Thành phố", "Số trạm BTS", "Băng tần");
        myChart.loadLineChart("/Statistics/StatBtsByOperatorCity", "#areaTab-chart_StatBtsByOperatorCity", "Tỉnh/ Thành phố", "Số trạm BTS", "Nhà mạng");
    }
}
statisticsController.init();