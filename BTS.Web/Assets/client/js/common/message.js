$(function () {
    checkMessage();
});

var checkMessage = function () {
    var error = $('#error').val();
    var success = $('#success').val();
    var info = $('#info').val();
    var warning = $('#warning').val();

    if (success != null && success != "") {
        displayMessage(success, 'success');
        return true;
    }
    if (info != null && info != "") {
        displayMessage(info, 'info');
        return true;
    }
    if (warning != null && warning != "") {
        displayMessage(warning, 'warning');
        return true;
    }
    if (error != null && error != "") {
        displayMessage(error, 'error');
        return false;
    }
};

var displayMessage = function (message, msgType) {
    //toastr.options = {
    //    "closeButton": true,
    //    "debug": false,
    //    "positionClass": "toast-top-right",
    //    "onClick": null,
    //    "showDuration": "300",
    //    "hideDuration": "1000",
    //    "timeOut": "8000",
    //    "extendedTimeOut": "1000",
    //    "showEasing": "swing",
    //    "hideEasing": "linear",
    //    "showMethod": "fadeIn",
    //    "hideMethod": "fadeOut"
    //};
    //toastr[msgType](message);
    $.notify(message, {
        className: msgType,
        clickToHide: false
    });
};