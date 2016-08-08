(function () {
    'use strict';
    var app = angular.module('myApp', ['ui.router', 'ngMessages', 'ngRoute', 'ngAnimate',
        'ui.bootstrap', 'ui.bootstrap.tpls',
        'myApp.services', 'myApp.controllers']);

    app.config(
        ['$stateProvider', '$routeProvider', function ($stateProvider, $routeProvider) {
            $stateProvider.state('home', {
                url: '',
                templateUrl: 'AppScripts/Views/employeesListView.html',
                controller: 'employeeListCtrl as vm'
            })
            .state('employeeEdit', {
                url: '/employee/edit/:id',
                templateUrl: 'AppScripts/Views/employeeEditView.html',
                controller: 'employeeEditCtrl as vm',
                resolve: {
                    employeesResource: 'employeesResource',
                    employee: function (employeesResource, $stateParams) {
                        var id = $stateParams.id;
                        //find record
                        if (id && id != 0) {
                            return employeesResource.get({ id: id }).$promise;
                        }
                        //create record
                        return new employeesResource();
                    }
                }
            });
            $routeProvider.otherwise('')
        }]);

    app.run(['$state', function ($state) {
        $state.go('home'); //make a transition to 'home' state when app starts
    }]);
}());