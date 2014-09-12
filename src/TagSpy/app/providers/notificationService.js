'use strict';
app.service('notificationService', function () {

    this.success = function (text) {
        toastr.success(text);
    }

    this.error = function (text, caption) {
        if (caption)
            toastr.error(text, caption);
        else
            toastr.error(text, "Error");
    }

    this.info = function (text) {
        toastr.info(text);
    }

    this.warning = function (text) {
        toastr.warning(text);
    }

});