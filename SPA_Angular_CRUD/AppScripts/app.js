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
            .state('home.addEmployee', {
                url: '/add',
                onEnter: ["$state", "$uibModal", "employeesResource", function ($state, $uibModal, employeesResource) {
                    $uibModal.open({
                        animation: true,
                        size: "sm", 
                        templateUrl: "AppScripts/Views/employeeAddModal.html",
                        resolve: {
                            employee: function (employeesResource) { 
                                return new employeesResource(); }
                        },
                        controller: "employeeAddCtrl as vm"
                    }).result.finally(function () {
                        $state.go('home');
                    });
                }]
        });
            $routeProvider.otherwise('')
        }]);

    app.run(['$state', function ($state) {
        $state.go('home'); //make a transition to 'home' state when app starts
    }]);
}());