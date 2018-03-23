/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('rootController', rootController);

    rootController.$inject = ['$state', 'authData', 'loginService', '$injector', '$scope', 'authenticationService'];

    function rootController($state, authData, loginService, $injector, $scope, authenticationService) {
        $scope.logOut = function () {
            loginService.logOut();
            $state.go('login');
        }
        $scope.authentication = authData.authenticationData;
        $scope.Header = "/app/shared/views/Header.html";
        $scope.SideBar = "/app/shared/views/SideBar.html";
        $scope.Footer = "/app/shared/views/Footer.html";
        authenticationService.init();
        authenticationService.validateRequest().then(function (response) {
            if (response != null && response.data.error != undefined) {
                notificationService.displayError("Bạn cần đăng nhập vào hệ thống");
            }
            else {
                var stateService = $injector.get('$state');
                stateService.go('home');
            }
        })
    }
})(angular.module('BTS'));