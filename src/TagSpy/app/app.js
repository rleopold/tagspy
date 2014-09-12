'use strict';

var app = angular.module('TagSpy', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'google-maps']);

app.constant('tagspySettings', {
     apiServiceBaseUri: window.location.origin + '/',   
});

app.config(['$routeProvider', function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeCtrl",
        templateUrl: "/app/partials/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginCtrl",
        controllerAs: "vm",
        templateUrl: "/app/partials/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupCtrl",
        templateUrl: "/app/partials/signup.html"
    });

    $routeProvider.when('/create', {
        templateUrl: 'app/partials/create.html',
        controller: 'createCtrl'
    });

    $routeProvider.when('/manage', {
        templateUrl: 'app/partials/manage.html',
        controller: 'manageCtrl'
    });

    $routeProvider.when('/spy', {
        templateUrl: 'app/partials/spy.html',
        controller: 'spyCtrl'
    });

    $routeProvider.otherwise({ redirectTo: '/home' });
}]);

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);
