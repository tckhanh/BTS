/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('operatorListController', operatorListController);

    operatorListController.$inject = ['$scope','apiService'];
    function operatorListController($scope, apiService) {
        $scope.operators = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.keyword = '';

        $scope.search = search;

        function search() {
            getOperators();
        }

        $scope.getOperators = getOperators;

        function getOperators(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 2
                }
            }
            apiService.get('/api/Operator/getall', config, function (result) {
                $scope.operators = result.data.items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load Operators failed.');
            });
        }

        $scope.getOperators();
    }

})(angular.module('BTS.operators'));