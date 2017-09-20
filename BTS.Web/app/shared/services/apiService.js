/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function (app) {
    app.factory('apiService', apiService);

    apiService.$inject = ['$http', 'notificationService', 'authenticationService', '$state'];

    function apiService($http, notificationService, authenticationService, $state) {
        return {
            get: get,
            post: post,
            put: put,
            del: del
        }

        function get(url, params, success, failure) {
            authenticationService.setHeader();
            $http.get(url, params).then(function (result) {
                success(result);
            }, function (error) {
                console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayError('Authenticate is required.');
                    $state.go('login');
                }
                else if (failure != null) {
                    failure(error);
                }                
            });
        }

        function post(url, data, success, failure) {
            authenticationService.setHeader();
            $http.post(url, data).then(function (result) {
                success(result);
            }, function (error) {
                console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayError('Authenticate is required.');
                    $state.go('login');
                }
                else if (failure != null) {
                    failure(error);
                }
            });
        }

        function put(url, data, success, failure) {
            authenticationService.setHeader();
            $http.put(url, data).then(function (result) {
                success(result);
            }, function (error) {
                console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayError('Authenticate is required.');
                    $state.go('login');
                }
                else if (failure != null) {
                    failure(error);
                }
            });
        }

        function del(url, params, success, failure) {
            authenticationService.setHeader();
            $http.delete(url, params).then(function (result) {
                success(result);
            }, function (error) {
                console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayError('Authenticate is required.');
                    $state.go('login');
                }
                else if (failure != null) {
                    failure(error);
                }
            });
        }
    }
})(angular.module('BTS.common'));