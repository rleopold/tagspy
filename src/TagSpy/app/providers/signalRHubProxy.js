'use strict';

app.factory('signalRHubProxy', ['$rootScope', 'tagspySettings',
    function ($rootScope, tagspySettings) {
        function signalRHubProxyFactory(hubName, startOptions) {
            var connection = $.hubConnection(tagspySettings.apiServiceBaseUri);
            var proxy = connection.createHubProxy(hubName);

            return {
                on: function (eventName, callback) {
                    proxy.on(eventName, function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });

                },
                off: function (eventName, callback) {
                    proxy.off(eventName, function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
                },
                invoke: function (methodName, args, callback) {
                    proxy.invoke(methodName, args)
                        .done(function (result) {
                            $rootScope.$apply(function () {
                                if (callback) {
                                    callback(result);
                                }
                            });
                        });
                },
                connection: connection
            };
        };

        return signalRHubProxyFactory;
    }]);