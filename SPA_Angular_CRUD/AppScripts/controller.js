'use strict';

var app = angular.module('myApp.controllers', []);

app.controller('employeeListCtrl', ['empData', 'pagingParams', 'employeesResource',
    'positionsResource', 'employeesGetParams',
    function (empData, pagingParams, employeesResource,
        positionsResource, employeesGetParams) {
        var vm = this;
        vm.message = '';
        // ref to "global" pagingParams & empData in service
        vm.pagingParams = pagingParams;
        vm.employees = empData;

        vm.pageSizeOptions = [5, 10, 20, 50];
        //selected employee
        vm.selected = {};

        // get Positions for dropdown list
        positionsResource.query(function (data) {
            vm.positions = data;
        });

        vm.getTemplate = function (emp) {
            if (emp.id === vm.selected.id) return 'edit';
            else return 'display';
        }

        vm.editEmployee = function (emp) {
            vm.selected = angular.copy(emp);
        }

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

        vm.updateEmployee = function (employee) {
            employeesResource.update({ id: employee.id }, employee,
                            function (data) { //success
                                vm.selected = angular.copy(data);
                                vm.message = 'Save Complete';
                                vm.selected = {};
                            },
                            function (response) { //failure
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
        };

        vm.cancel = function () {
            //find employee by id
            var index = vm.employees.map(function (e) { return e.id; }).indexOf(vm.selected.id);
            if (index !== -1) {
                vm.employees[index] = vm.selected;
            }
            vm.selected = {};
            vm.message = '';
        }

        vm.deleteEmployee = function (employee) {
            employeesResource.delete({ id: employee.id }, function (success) {
                //remove deleted record from array
                var index = vm.employees.indexOf(employee);
                if (index !== -1) {
                    vm.employees.splice(index, 1)
                }
            });
        };

         loadPage();
    }]);

app.controller('employeeAddCtrl', ['empData', 'employee', 'positionsResource', '$uibModalInstance',
    function (empData, employee, positionsResource, $uibModalInstance) {
        var vm = this;
        vm.employee = employee;
        vm.employees = empData;

        //make copy for reset form by Cancel button
        vm.originalEmp = angular.copy(employee);

        // get Positions for dropdown list
        positionsResource.query(function (data) {
            vm.positions = data;
        });

        vm.submit = function () {
            vm.message = '';
            vm.employee.$save(
                //success
                function (data) {
                    vm.originalEmp = angular.copy(data);
                    vm.message = 'Save Complete';
                    vm.employees.push(data);
                    $uibModalInstance.close();
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
        };

        vm.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        }
    }]);
