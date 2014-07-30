/// <reference path="../_references.ts"/>

module Application {

    var app = angular.module("app");

    app.factory("alertService", ($rootScope: ng.IRootScopeService, $timeout : ng.ITimeoutService)
        => new AlertService($rootScope, $timeout));

    export interface IAlert {
        type: string;
        message: string;
        close: any;
        timer: ng.IPromise<any>;
    }

    export interface IAlertService {
        create(type :string, message: string, time?: number);
        close(alert: IAlert, index: number)
    }

    export class AlertService implements IAlertService {
        $timeout: ng.ITimeoutService;
        $rootScope: ng.IRootScopeService;
        alerts : IAlert[];

        constructor (
            $rootScope: ng.IRootScopeService,
            $timeout: ng.ITimeoutService
            ) {
            this.$rootScope = $rootScope;
            this.$timeout = $timeout;
            this.alerts = (<any>$rootScope).alerts = [];
        }

        create(type : string, message : string, time? : number) {
            var alert : IAlert = {
                type: type,
                message: message,
                close: Alert.close,
                timer: null
            };

            if (time != null) {
                alert.timer = this.$timeout(function () {
                    alerts.splice(alerts.indexOf(alert), 1);
                }, time);
            }
            alerts.push(alert);
        }

        close (alert : IAlert, index : number) {
            if (alert.timer != null) this.$timeout.cancel(alert.timer);
            alerts.splice(index, 1);
        }
    }
}