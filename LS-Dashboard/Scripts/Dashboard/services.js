var svc = angular.module("services", []);

// Service for modules, controllers, directives, services to easily connect to signalR hubs
svc.factory("signalRFactory", function ($rootScope, $http, $timeout) {
    var factory = {};
    factory.connection = $.connection;

    var startDoneFunctions = [];
    var debounce;

    factory.start = function (done) {
        factory.connection.hub.stop();

        if (done) {
            if (startDoneFunctions.indexOf(done) == -1) {
                startDoneFunctions.push(done);
            }
        }
        if (debounce) $timeout.cancel(debounce);
        debounce = $timeout(function () {
            factory.connection.hub.start().done(function () {
                for (var x = 0; x < startDoneFunctions.length; x++) {
                    startDoneFunctions[x]();
                }
            });
        }, 100);
    };

    return factory;
});