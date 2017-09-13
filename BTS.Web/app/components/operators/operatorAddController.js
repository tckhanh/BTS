/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('operatorAddController', operatorAddController);

    operatorAddController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function operatorAddController(apiService, $scope, notificationService, $state) {
        $scope.operator = {
            Telephone: "(+84)",
            Fax: "(+84)"
        }

        $scope.AddOperator = AddOperator;

        function AddOperator() {
            apiService.post('api/operator/create', $scope.operator,
                function (result) {
                    notificationService.displaySuccess(result.data.ID + ' đã được thêm mới.');
                    $state.go('operators');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }

        //function loadParentCategory() {
        //    apiService.get('api/operator/getallparents', null, function (result) {
        //        $scope.parentCategories = result.data;
        //    }, function () {
        //        console.log('Cannot get list parent');
        //    });
        //}

        //loadParentCategory();
    }

})(angular.module('BTS.operators'));