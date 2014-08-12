/// <reference path="_references.ts" />

module Application {
    'use strict';

    var app = angular.module("app", ["ngRoute"]);

    app.config(function ($routeProvider) {
        $routeProvider
            .when('/', {
                templateUrl: 'home/index.html'
            })
    });
}
