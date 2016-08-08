'use strict';

var app = angular.module('myApp.controllers', []);

app.controller('employeeListCtrl', ['pagingParams', 'employeesResource', 'employeesGetParams', function (pagingParams, employeesResource, employeesGetParams) {
    var vm = this;
    // ref to "global" pagingParams in service
    vm.pagingParams = pagingParams;
    vm.pageSizeOptions = [5,10,20,50];

    vm.pageSizeChanged = function () {
        vm.pagingParams.currentPage = 1;
        loadPage();
    };

    vm.search = function () {
        vm.pagingParams.currentPage = 1;
        loadPage();
    };

    vm.searchClear = function () {
        vm.pagingParams.currentPage = 1;
        vm.pagingParams.search = '';
        loadPage();
    };

    vm.sort = function (sortBy) {
        if (sortBy == vm.pagingParams.sortBy) {
            vm.pagingParams.reverse = !vm.pagingParams.reverse;
        } else {
            vm.pagingParams.sortBy = sortBy;
            vm.pagingParams.reverse = false;
        }
        vm.pagingParams.currentPage = 1;
        loadPage();
    };

    vm.pageChanged = function () {
        loadPage();
    };

    function loadPage() {
        vm.employees = null;

        employeesGetParams.GetEmployees(pagingParams)
        .success(function (result) {
            vm.totalItems = result.totalItems;
            vm.employees = result.data;
        });
    }

    vm.employeeDelete = function (employee) {
        var emp = employeesResource.get({ id: employee.id });
        emp.$delete({id: employee.id}, function (success) {
            //remove deleted record from array
            var index = vm.employees.indexOf(employee);
            vm.employees.splice(index, 1)
        });
    }

    loadPage();
}]);

app.controller('employeeEditCtrl', ['employee', 'positionsResource', '$state',
    function (employee, positionsResource, $state) {
        var vm = this;
        vm.employee = employee;

        //make copy for reset form by Cancel button
        vm.originalEmp = angular.copy(employee);

        // get Positions for dropdown list
        positionsResource.query(function (data) {
            vm.positions = data;
        });

        vm.submit = function () {
            vm.message = '';
            if (!vm.empForm.$pristine) {
                if (vm.employee.id && vm.employee.id != 0) { //PUT
                    vm.employee.$update(
                        //success
                        function (data) {
                            vm.originalEmp = angular.copy(data);
                            vm.message = 'Save Complete';
                            $state.transitionTo('home');
                        },
                        //failure
                        function (response) {
                            vm.message = 'Error: ';
                            vm.message = response.statusText + '\r\n';
                            if (response.data.modelState) {
                                for (var key in response.data.modelState) {
                                    if (response.data.modelState.hasOwnProperty(key)) {
                                        vm.message += response.data.modelState[key] + "\r\n";
                                    }
                                }
                            }
                            if (response.data.exceptionMessage)
                                vm.message += response.data.exceptionMessage;
                        });
                } else { //POST
                    vm.employee.$save(
                        //success
                        function (data) {
                            vm.originalEmp = angular.copy(data);
                            vm.message = 'Save Complete';
                            $state.transitionTo('home');
                        },
                        //failure
                        function (response) {
                            vm.message = 'Error: ';
                            vm.message = response.statusText + '\r\n';
                            if (response.data.modelState) {
                                for (var key in response.data.modelState) {
                                    if (response.data.modelState.hasOwnProperty(key)) {
                                        vm.message += response.data.modelState[key] + "\r\n";
                                    }
                                }
                            }
                            if (response.data.exceptionMessage)
                                vm.message += response.data.exceptionMessage;
                        });
                }
            } else {
                vm.message = "No Changes";
                $state.transitionTo('home');
            }
        };

        vm.cancel = function () {
            vm.empForm.$setPristine();
            vm.employee = angular.copy(vm.originalEmp);
            vm.message = '';
            $state.transitionTo('home');
        }
    }]);
