var svc = angular.module("services", []);

svc.service('signalRProxy', ["$rootScope", function ($rootScope) {
    var connection = $.hubConnection(path);
    var service = this;
    function proxyFactory(hubName, done, fail) {
        var proxy = service.connection.createHubProxy(hubName);
        service.connection.start().done(
            function () {
                $rootScope.$apply(function () {
                    if (done) {
                        done();
                    }
                });
            }).fail(function () {
                $rootScope.$apply(function () {
                    if (fail) {
                        fail();
                    }
                });
            });

        return {
            refresh: function (method, callback, interval) {
                bind = this;
                this.on(method, function (result) {
                    callback.call(this, result);
                    setTimeout(bind.on, interval, method, this);
                });
            },
            on: function (method, callback) {
                proxy.on(method, function () {
                    var args = (arguments.length === 1 ? [arguments[0]] : Array.apply(null, arguments));
                    $rootScope.$apply(function () {
                        console.log(args);
                        if (callback) {
                            callback.apply(null, args);
                        }
                    })
                });
            },

            invoke: function (method, done, fail, args) {
                var arguments = [method];
                for (var arg in args) {
                    if (args.hasOwnProperty(arg)) {
                        arguments.push(args[arg]);
                    }
                }
                console.log(arguments);
                proxy.invoke.apply(proxy, arguments).done(
                    function (result) {
                        $rootScope.$apply(
                            function () {
                                if (done) {
                                    done(result);
                                }
                            }
                        );
                    }
                ).fail(
                    function (error) {
                        $rootScope.$apply(
                            function () {
                                if (fail) {
                                    fail(error);
                                }
                            }
                        );
                    }
                );
            }
        }
    }

    return proxyFactory;
}]);

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