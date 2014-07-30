/// <reference path="_references.ts" />

module Application {
    'use strict';

    var app = angular.module("app", ["ngRoute", "ngResource", "ngSanitize", "angular-loading-bar", "ngAnimate", "security"]);

    app.config(function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    });

    app.config(function ($httpProvider, securityProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
        $httpProvider.interceptors.push('myHttpInterceptor');
        securityProvider.urls.login = '/api/auth/login';
    });

    app.run(function ($rootScope, security) {
        $rootScope.security = security;
    });

    app.config(function ($routeProvider, $locationProvider) {
        $locationProvider.html5Mode(true);
        $routeProvider
            .when('/', {
                templateUrl: 'home/index.html'
            })
    });
}