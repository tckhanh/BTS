/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('BTS.operators', ['BTS.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('operators', {
                url: "/operators",
                parent: 'base',
                templateUrl: "/app/components/operators/operatorListView.html",
                controller: "operatorListController"
            })
            .state('add_operator', {
                url: "/add_operator",
                parent: 'base',
                templateUrl: "/app/components/operators/operatorAddView.html",
                controller: "operatorAddController"
            })
	        .state('edit_operator', {
	            url: "/edit_operator/:id",
	            parent: 'base',
	            templateUrl: "/app/components/operators/operatorEditView.html",
	            controller: "operatorEditController"
	        });
    }
})();
