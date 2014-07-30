/// <reference path="../_references.ts"/>

module Application {

    var app = angular.module("app");

    app.factory("ErrorService", ($rootScope: ng.IRootScopeService, alertService : IAlertService)
        => new ErrorService($rootScope, alertService));

    export interface IErrorService {
        handle(data, status, headers, config);
    }

    export class ErrorService implements IErrorService {
        $rootScope: ng.IRootScopeService;
        alertService: IAlertService

        constructor (
            $rootScope: ng.IRootScopeService,
            alertService: IAlertService
            ) {
            this.$rootScope = $rootScope;
            this.alertService = alertService;
        }

        handle(data, status, headers, config) {
            var message = [];
            if (data.message) {
                message.push("<strong>" + data.message + "</strong>");
            }
            if (data.modelState) {
                angular.forEach(data.modelState, function (errors, key) {
                    message.push(errors);
                });
            }
            if (data.exceptionMessage) {
                message.push(data.exceptionMessage);
            }
            if (data.error_description) {
                message.push(data.error_description);
            }
            this.alertService.create('danger', message.join('<br/>'));
        }
    }

    export class ErrorHttpInterceptor {
        $q: ng.IQService;
        errorService: IErrorService

        constructor (
            errorService: IErrorService,
            $q: ng.IQService
            ) {
            this.errorService = errorService;
            this.$q = $q;
        }

        response(response) {
            return response;
        }

        responseError(response) {
            this.errorService.handle(response.data, response.status, response.headers, response.config);
            return this.$q.reject(response);
        }
    }
}

