/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('homeController', homeController);

    homeController.$inject = ['authenticationService', '$location', 'notificationService'];

    function homeController(authenticationService, $location, notificationService) {
        if (!authenticationService.isAuthenticated()) {
            notificationService.displayError('Yêu cầu đăng nhập hệ thống.');
            $location.path('/login');
        }
    }

})(angular.module('BTS'));