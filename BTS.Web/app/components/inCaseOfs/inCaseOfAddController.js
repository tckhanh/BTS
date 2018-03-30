/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('inCaseOfAddController', inCaseOfAddController);

    inCaseOfAddController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function inCaseOfAddController(apiService, $scope, notificationService, $state) {
        $scope.inCaseOf = {
            ID: "",
            Name: ""
        };

        $scope.AddInCaseOf = AddInCaseOf;

        function AddInCaseOf() {
            apiService.post('/api/inCaseOf/create', $scope.inCaseOf,
                function (result) {
                    notificationService.displaySuccess(result.data.ID + ' đã được thêm mới.');
                    $state.go('inCaseOfs');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
    }
})(angular.module('BTS.inCaseOfs'));