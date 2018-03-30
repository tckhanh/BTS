/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('cityAddController', cityAddController);

    cityAddController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function cityAddController(apiService, $scope, notificationService, $state) {
        $scope.city = {
            ID: "",
            Name: ""
        };

        $scope.AddCity = AddCity;

        function AddCity() {
            apiService.post('/api/city/create', $scope.city,
                function (result) {
                    notificationService.displaySuccess(result.data.ID + ' đã được thêm mới.');
                    $state.go('cities');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
    }
})(angular.module('BTS.cities'));