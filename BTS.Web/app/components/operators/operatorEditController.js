/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('operatorEditController', operatorEditController);

    operatorEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams'];

    function operatorEditController(apiService, $scope, notificationService, $state, $stateParams) {
        $scope.operator = {
            Telephone: "(+84)",
            Fax: "(+84)"
        }

        $scope.EditOperator = EditOperator;
        
        function EditOperator() {
            apiService.put('/api/operator/update', $scope.operator,
                function (result) {
                    notificationService.displaySuccess(result.data.ID + ' đã được cập nhật.');
                    $state.go('operators');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }

        function loadOperatorDetail() {
            apiService.get('/api/operator/getbyid/' + $stateParams.id, null, function (result) {
                $scope.operator = result.data;
            }, function (error) {
                notificationService.displayError(error.data.Message);
            });
        }
        //function loadParentCategory() {
        //    apiService.get('/api/operator/getallparents', null, function (result) {
        //        $scope.parentCategories = result.data;
        //    }, function () {
        //        console.log('Cannot get list parent');
        //    });
        //}

        //loadParentCategory();
        loadOperatorDetail();
    }

})(angular.module('BTS.operators'));