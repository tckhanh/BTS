/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('BTS.inCaseOfs', ['BTS.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('inCaseOfs', {
                url: "/inCaseOfs",
                parent: 'base',
                templateUrl: "/app/components/inCaseOfs/inCaseOfListView.html",
                controller: "inCaseOfListController"
            })
            .state('add_inCaseOf', {
                url: "/add_inCaseOf",
                parent: 'base',
                templateUrl: "/app/components/inCaseOfs/inCaseOfAddView.html",
                controller: "inCaseOfAddController"
            })
	        .state('edit_inCaseOf', {
	            url: "/edit_inCaseOf/:id",
	            parent: 'base',
	            templateUrl: "/app/components/inCaseOfs/inCaseOfEditView.html",
	            controller: "inCaseOfEditController"
	        });
    }
})();