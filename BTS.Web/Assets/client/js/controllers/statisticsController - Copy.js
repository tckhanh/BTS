var statisticsController = {
    Tab2FirstClick: false,
    Tab3FirstClick: false,
    init: function () {
        statisticsController.registerEvent();
        statisticsController.loadTab1();
    },
    registerEventDataTable: function () {
    },
    registerEvent: function () {
        $('#btnSearch').off('click').on('click', function () {
            if ($('#SelArea').val() != '') {
                statisticsController.loadTab3();
            }            
        });

        $("a[href='#Tab2']").on('shown.bs.tab', function (e) {
            if (statisticsController.Tab2FirstClick == false) {
                statisticsController.Tab2FirstClick = true;
                statisticsController.loadTab2();
            }
        });

        $("a[href='#Tab3']").on('shown.bs.tab', function (e) {
            if (statisticsController.Tab3FirstClick == false) {
                statisticsController.Tab3FirstClick = true;
                if ($('#SelArea').val() != '') {
                    statisticsController.loadTab3();
                }
            }            
        });       
    },

    loadTab1: function()  {

        myChart.loadPieChart("/Statistics/StatCoupleCerByOperator", "#Tab1-chart_StatCoupleCerByOperator", true);
        //myChart.loadPieChart("/Statistics/StatBtsInProcess", "#Tab1-chart_StatBtsInProcess");
        myChart.loadBarChart("/Statistics/StatCerByOperator", "#Tab1-chart_StatCerByOperator", "", "Giấy CNKĐ", "Nhà mạng");

        myChart.loadBarGroupChart("/Statistics/StatExpiredCerByOperatorYear", "#Tab1-chart_StatExpiredCerByOperatorYear", "", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadBarGroupChart("/Statistics/StatCerByOperatorYear", "#Tab1-chart_StatCerByOperatorYear", "Năm", "Giấy CNKĐ", "Nhà mạng");

        myChart.loadPieChart("/Statistics/StatBtsByOperatorBand", "#Tab1-chart_StatBtsByOperatorBand", true);
        myChart.loadPieChart("/Statistics/StatBtsByOperatorManufactory", "#Tab1-chart_StatBtsByOperatorManufactory", true);
    },
    loadTab2: function()  {
        myChart.loadPieChart("/Statistics/StatCoupleCerByArea", "#Tab2-chart_StatCoupleCerByArea", true);
        //myChart.loadPieChart("/Home/StatBtsInProcess", "#Tab2-chart_StatBtsInProcess");
        myChart.loadBarChart("/Statistics/StatCerByArea", "#Tab2-chart_StatCerByArea", "", "Giấy CNKĐ", "Khu vực");

        myChart.loadBarGroupChart("/Statistics/StatExpiredCerByAreaYear", "#Tab2-chart_StatExpiredCerByAreaYear", "", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadBarGroupChart("/Statistics/StatCerByAreaYear", "#Tab2-chart_StatCerByAreaYear", "Năm", "Giấy CNKĐ", "Nhà mạng");

        myChart.loadPieChart("/Statistics/StatBtsByAreaBand", "#Tab2-chart_StatBtsByAreaBand",true);
        myChart.loadPieChart("/Statistics/StatBtsByAreaManufactory", "#Tab2-chart_StatBtsByAreaManufactory",true);
    },
    loadTab3: function () {
        statisticsController.SubTab3CurrentUnsignName = ""

        myChart.loadLineChart("/Statistics/StatCerByOperatorCity", "#Tab3-chart_StatCerByOperatorCity", "Tỉnh/ Thành phố", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadLineChart("/Statistics/StatNearExpiredInYearCerByOperatorCity", "#Tab3-chart_StatNearExpiredInYearCerByOperatorCity", "Tỉnh/ Thành phố", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadLineChart("/Statistics/StatInYearCerByOperatorCity", "#Tab3-chart_StatInYearCerByOperatorCity", "Tỉnh/ Thành phố", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadLineChart("/Statistics/StatExpiredCerByOperatorCity", "#Tab3-chart_StatExpiredCerByOperatorCity", "Tỉnh/ Thành phố", "Giấy CNKĐ", "Nhà mạng");
        myChart.loadLineChart("/Statistics/StatBtsByBandCity", "#Tab3-chart_StatBtsByBandCity", "Tỉnh/ Thành phố", "Số trạm BTS", "Băng tần");
        myChart.loadLineChart("/Statistics/StatBtsByOperatorCity", "#Tab3-chart_StatBtsByOperatorCity", "Tỉnh/ Thành phố", "Số trạm BTS", "Nhà mạng");
    }
}
statisticsController.init();