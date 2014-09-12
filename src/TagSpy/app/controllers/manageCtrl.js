'use strict';
var app = angular.module('TagSpy');
app.controller('manageCtrl', manageCtrl);
manageCtrl.$inject = ['$scope', 'subscriptionService','$location', '$timeout' , 'signalRHubProxy'];

function manageCtrl($scope, subscriptionService, $location, $timeout, signalRHubProxy) {
    $scope.subscriptions = [];
    $scope.subActivity = {};
    subscriptionService.getSubscriptions()
        .success(function (data) {
            $scope.subscriptions = data;
            angular.forEach($scope.subscriptions, function (value, key) {
                $scope.subActivity[value.Object_Id] = false; // set up our activity watching
            });
            console.log($scope.subActivity);
        });

    //set up our real time notifications
    var listener = signalRHubProxy('notifyHub');
    listener.on('newMediaAvailable', function (data) {
        showSubActivity(data);
    });
    listener.connection.start();

    //functions for deleting subs
    $scope.deleteSubscriptions = function () {
        subscriptionService.deleteSubscription('all');
    }

    $scope.deleteSubscription = function (idx) {
        var tag = $scope.subscriptions[idx];
        subscriptionService.deleteSubscription(tag.Object_Id)
            .success(function () {
                $scope.subscriptions.splice(idx, 1);
            });
    }

    $scope.setActiveSubscription = function (subscription) {
        subscriptionService.activeSubscription = subscription;
        $location.path('/spy');
    }

    function showSubActivity(id) {
        $scope.subActivity[id] = true;
        $timeout(function () {
            $scope.subActivity[id] = false;
        }, 300);
    }

    $scope.$on("$destroy", function () {
        listener.off('newMediaAvailable');
        console.log('disconnected');
    });

}
