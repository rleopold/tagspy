'use strict';
app.service('subscriptionService', ['$http', function ($http) {

    var subUrl = '/api/subscription';
    var mediaUrl = 'api/recentmedia';

    this.getSubscriptions = function () {
        return $http.get(subUrl);
    }

    this.createSubscription = function (data) {
        return $http.post(subUrl, data);
    }

    this.getRecentMedia = function (subscription) {
        return $http.get(mediaUrl + '/?type=' + subscription.Type + '&object_id=' + subscription.Object_Id);
    }

    this.getLatestMedia = function (subscription, min_id) {
        console.log('request:', mediaUrl + '/?type=' + subscription.Type + '&object_id=' + subscription.Object_Id + '&minMediaId=' + min_id);
        return $http.get(mediaUrl + '/?type=' + subscription.Type + '&object_id=' + subscription.Object_Id + '&minMediaId=' + min_id);
    }

    this.deleteSubscription = function (id) {
        return $http.delete(subUrl + '/' + id);
    }

    this.activeSubscription = {};

}]);