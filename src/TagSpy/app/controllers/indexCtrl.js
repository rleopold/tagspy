'use strict';
var app = angular.module('TagSpy');
app.controller('indexCtrl', indexCtrl);
indexCtrl.$inject = ['$scope', '$location', 'authService'];

function indexCtrl ($scope, $location, authService) {

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    $scope.authentication = authService.authentication;

}
