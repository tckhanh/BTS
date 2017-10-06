/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('operatorListController', operatorListController);

    operatorListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];
    function operatorListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.operators = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.keyword = '';
        $scope.isAll = false;

        $scope.search = search;

        $scope.deleteOperator = deleteOperator;

        $scope.selectAll = selectAll;

        $scope.deleteMultiple = deleteMultiple;

        $scope.$watch("operators", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        $scope.getOperators = getOperators;        

        $scope.getOperators();



        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedOperators: JSON.stringify(listId)
                }
            }
            apiService.del('/api/operator/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }       

        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.operators, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.operators, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }


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
                $scope.operators = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load Operators failed.');
            });
        }

    }

})(angular.module('BTS.operators'));