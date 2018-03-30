/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('BTS.cities', ['BTS.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('cities', {
                url: "/cities",
                parent: 'base',
                templateUrl: "/app/components/cities/cityListView.html",
                controller: "cityListController"
            })
            .state('add_city', {
                url: "/add_city",
                parent: 'base',
                templateUrl: "/app/components/cities/cityAddView.html",
                controller: "cityAddController"
            })
	        .state('edit_city', {
	            url: "/edit_city/:id",
	            parent: 'base',
	            templateUrl: "/app/components/cities/cityEditView.html",
	            controller: "cityEditController"
	        });
    }
})();