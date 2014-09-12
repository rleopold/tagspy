'use strict';
var app = angular.module('TagSpy');
app.controller('spyCtrl', spyCtrl);
spyCtrl.$inject = ['$scope', '$location', 'subscriptionService', 'signalRHubProxy'];

function spyCtrl ($scope, $location, subscriptionService, signalRHubProxy) {
    $scope.activeSub = subscriptionService.activeSubscription;
    $scope.medias = [];
    $scope.nextMedia = "0";
    $scope.selectedMedia = {};

    //set up our real time notifications
    var listener = signalRHubProxy('notifyHub');
    listener.on('newMediaAvailable', function (data) {
        if ($scope.nextMedia != "0" && data == $scope.activeSub.Object_Id) {
            subscriptionService.getLatestMedia($scope.activeSub, $scope.nextMedia)
            .success(function (result) {
                console.log('result:', result);
                if (result.Pagination && result.Data.length > 0) {
                    $scope.nextMedia = result.Pagination.next_min_id;
                    angular.forEach(result.Data, function (value, key) {
                        if (!hasDupes(value.Id)) {
                            $scope.medias.unshift(value);
                            $scope.medias.splice(18, $scope.medias.length - 18);
                        }
                        else
                            console.log("dupe:", value);
                    });
                }
            });
        }
    })
    listener.connection.start();
    if (!$scope.activeSub.Object_Id)
        $location.path('/manage'); //bail out if we don't have a sub to watch

    subscriptionService.getRecentMedia($scope.activeSub)
    .success(function (data) {
        console.log("recent:", data);
        $scope.nextMedia = data.Pagination.next_min_id;
        $scope.medias = data.Data;

    });


    $scope.clickedMedia = function (media) {
        $scope.selectedMedia = media;
    }

    $scope.$on("$destroy", function () {
        listener.off('newMediaAvailable');
        console.log('disconnected');
    });

    function hasDupes(id)
    {
        for(var i=0; i<$scope.medias.length;i++)
        {
            if ($scope.medias[i].Id === id)
                return true;
        }
        return false;
    }
}