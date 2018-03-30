/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('inCaseOfListController', inCaseOfListController);

    inCaseOfListController.$inject = ['$scope', '$location', 'apiService', 'notificationService', '$ngBootbox', '$filter'];
    function inCaseOfListController($scope, $location, apiService, notificationService, $ngBootbox, $filter) {
        $scope.inCaseOfs = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.keyword = '';
        $scope.isAll = false;

        $scope.search = search;

        $scope.deleteInCaseOf = deleteInCaseOf;

        $scope.selectAll = selectAll;

        $scope.deleteMultiple = deleteMultiple;

        $scope.$watch("inCaseOfs", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        $scope.getInCaseOfs = getInCaseOfs;

        $scope.getInCaseOfs();

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedInCaseOfs: JSON.stringify(listId)
                }
            }
            apiService.del('/api/inCaseOf/deletemulti', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                search();
            }, function (error) {
                notificationService.displayError('Xóa không thành công');
            });
        }

        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.inCaseOfs, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.inCaseOfs, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        function deleteInCaseOf(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/inCaseOf/delete', config, function () {
                    notificationService.displaySuccess('Xóa thành công');
                    search();
                }, function () {
                    notificationService.displayError('Xóa không thành công');
                })
            });
        }

        function search() {
            getInCaseOfs();
        }

        function getInCaseOfs(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 10
                }
            }
            apiService.get('/api/inCaseOf/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                }
                //else {
                //    notificationService.displaySuccess('Đã tìm thấy ' + result.data.TotalCount + ' bản ghi.');
                //}
                $scope.inCaseOfs = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function (response) {
                console.log('Load InCaseOfs failed.');
                if (response.status == "401") {
                    notificationService.displayError("Bạn chưa được cấp quyền để thực hiện");
                    $location.path('/admin');
                }
                else {
                    notificationService.displayError(response.data.Message);
                }
            });
        }
    }
})(angular.module('BTS.inCaseOfs'));