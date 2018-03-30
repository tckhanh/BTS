/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('inCaseOfEditController', inCaseOfEditController);

    inCaseOfEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams'];

    function inCaseOfEditController(apiService, $scope, notificationService, $state, $stateParams) {
        $scope.inCaseOf = {
            Telephone: "(+84)",
            Fax: "(+84)"
        }

        $scope.EditInCaseOf = EditInCaseOf;

        function EditInCaseOf() {
            apiService.put('/api/inCaseOf/update', $scope.inCaseOf,
                function (result) {
                    notificationService.displaySuccess(result.data.ID + ' đã được cập nhật.');
                    $state.go('inCaseOfs');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }

        function loadInCaseOfDetail() {
            apiService.get('/api/inCaseOf/getbyid/' + $stateParams.id, null, function (result) {
                $scope.inCaseOf = result.data;
            }, function (error) {
                notificationService.displayError(error.data.Message);
            });
        }
        
        loadInCaseOfDetail();
    }
})(angular.module('BTS.inCaseOfs'));