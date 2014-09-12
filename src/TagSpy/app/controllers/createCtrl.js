'use strict';
var app = angular.module('TagSpy');
app.controller('createCtrl', createCtrl);
createCtrl.$inject = ['$scope', '$location', 'subscriptionService', 'notificationService'];

function createCtrl($scope, $location, subscriptionService, notificationService) {
    $scope.sub = {};
    $scope.sub.latitude = 32.7758;
    $scope.sub.longitude = -96.7967;
    $scope.sub.radius = 1000;           //set an ideal radius, does it make sense to let user choose this?
    $scope.sub.tag = "";
    $scope.sub.type = "place";
    $scope.isGeo = true;

    $scope.createSubscription = function (subscription) {
        if ($scope.isGeo) {
            subscription.type = "place";
        }
        else {
            subscription.type = "tag";
        }

        subscriptionService.createSubscription(subscription)
        .success(function (data) {
            notificationService.success("Subscription Created!");
            console.log(data); //what to do with this?
        })
        .error(function (data) {
            notificationService.error("Failed to Create Subscription");
            console.log(data);
        });
    }

    //google map integration!
    $scope.map = {
        clickedMarker: {
            id: 0
        },
        events: {
            click: function (mapModel, eventName, originalEventArgs) {
                var e = originalEventArgs[0];
                var lat = e.latLng.lat(),
                    lon = e.latLng.lng();
                $scope.sub.latitude = lat;
                $scope.sub.longitude = lon;

                $scope.map.clickedMarker = {
                    id: 0,
                    latitude: lat,
                    longitude: lon
                };
                //scope apply required because this event handler is outside of the angular domain
                $scope.$apply();
            }
        },
        center: {
            latitude: $scope.sub.latitude,
            longitude: $scope.sub.longitude
        },
        zoom: 8
    };

}