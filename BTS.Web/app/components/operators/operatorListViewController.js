(function (app) {
    app.controller('operatorListController', operatorListController);

    operatorListController.$inject = ['$scope','apiService'];
    function operatorListController($scope, apiService) {
        $scope.operators = [];

        $scope.getOperators = getOperators;

        function getOperators() {
            apiService.get('/api/Operator/getall', null, function (result) {
                $scope.operators = result.data;
            }, function () {
                console.log('Load Operators failed.');
            });
        }

        $scope.getOperators();
    }

})(angular.module('BTS.operators'));