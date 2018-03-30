/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('cityEditController', cityEditController);

    cityEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams'];

    function cityEditController(apiService, $scope, notificationService, $state, $stateParams) {
        $scope.city = {
            Telephone: "(+84)",
            Fax: "(+84)"
        }

        $scope.EditCity = EditCity;

        function EditCity() {
            apiService.put('/api/city/update', $scope.city,
                function (result) {
                    notificationService.displaySuccess(result.data.ID + ' đã được cập nhật.');
                    $state.go('cities');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }

        function loadCityDetail() {
            apiService.get('/api/city/getbyid/' + $stateParams.id, null, function (result) {
                $scope.city = result.data;
            }, function (error) {
                notificationService.displayError(error.data.Message);
            });
        }
        //function loadParentCategory() {
        //    apiService.get('/api/city/getallparents', null, function (result) {
        //        $scope.parentCategories = result.data;
        //    }, function () {
        //        console.log('Cannot get list parent');
        //    });
        //}

        //loadParentCategory();
        loadCityDetail();
    }
})(angular.module('BTS.cities'));