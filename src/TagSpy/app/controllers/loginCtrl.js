'use strict';
var app = angular.module('TagSpy');
app.controller('loginCtrl', loginCtrl);
loginCtrl.$inject = ['$location', 'authService'];

function loginCtrl ($location, authService) {
    var vm = this;
    vm.loginData = {
        userName: "",
        password: "",
    };

    vm.message = "";

    vm.login = function () {

        authService.login(vm.loginData).then(function (response) {

            $location.path('/subscriptions');

        },
         function (err) {
             vm.message = err.error_description;
         });
    };

}