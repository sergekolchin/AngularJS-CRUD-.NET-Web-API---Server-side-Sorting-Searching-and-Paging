(function () {
    'use strict';

    angular.module('myApp.services', ['ngResource'])
        .factory('pagingParams', [function () {
            var pagingParams = {
                currentPage: 1,
                pageSize: 10,
                sortBy: "id",
                reverse: false,
                search: ''
            };
            return pagingParams;
        }])
        .factory('employeesGetParams', ['$http', function ($http) {
            var service = {};
            service.GetEmployees = function (pagingParams) {
                return $http.get('api/employees', { params: pagingParams });
            };
            return service;
        }])
    .factory('employeesResource', ['$resource', function ($resource) {
        return $resource('api/employees/:id', { id: '@id' }, {
            update: {
                method: 'PUT', params: { id: '@id' }
            }
        });
    }])
    .factory('positionsResource', ['$resource', function ($resource) {
        return $resource('api/positions/:id', { id: '@id' }, {
            update: {
                method: 'PUT', params: { id: '@id' }
            }
        });
    }]);
}());