(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', '$location', 'apiService', 'notificationService', '$filter'];

    function revenueStatisticController($scope, $location, apiService, notificationService,$filter) {
        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['Doanh số', 'Lợi nhuận'];

        $scope.chartdata = [];
        function getStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: '01/01/2016',
                    toDate: '01/01/2017'
                }
            }
            apiService.get('api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                $scope.tabledata = response.data;
                var labels = [];
                var chartData = [];
                var revenues = [];
                var benefits = [];
                $.each(response.data, function (i, item) {
                    labels.push($filter('date')(item.Date,'dd/MM/yyyy'));
                    revenues.push(item.Revenues);
                    benefits.push(item.Benefit);
                });
                chartData.push(revenues);
                chartData.push(benefits);

                $scope.chartdata = chartData;
                $scope.labels = labels;
            }, function (response) {

                if (response.status == "401") {
                    notificationService.displayError("Bạn chưa được cấp quyền để thực hiện");
                    $location.path('/admin');
                }
                else {
                    notificationService.displayError(response.data.Message);
                }
            });
        }

        getStatistic();
    }

})(angular.module('BTS.statistics'));