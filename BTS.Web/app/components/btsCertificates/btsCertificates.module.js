/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('BTS.btsCertificates', ['BTS.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('btsCertificates', {
            url: "/btsCertificates",
            templateUrl: "/app/components/btsCertificates/btsCertificateListView.html",
            controller: "btsCertificateListController"
        }).state('btsCertificate_add', {
            url: "/btsCertificate_add",
            templateUrl: "/app/components/btsCertificates/btsCertificateAddView.html",
            controller: "btsCertificateAddController"
        });
    }
})();
