/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('operatorListController', operatorListController);

    operatorListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];
    function operatorListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.operators = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.keyword = '';

        $scope.search = search;

        $scope.deleteOperator = deleteOperator;

        function deleteOperator(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/operator/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }

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
            apiService.get('/api/operator/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                }
                //else {
                //    notificationService.displaySuccess('Đã tìm thấy ' + result.data.TotalCount + ' bản ghi.');
                //}
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