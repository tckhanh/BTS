$(function () {
    checkMessage();
});

var checkMessage = function () {
    var error = $('#error').val();
    var success = $('#success').val();
    var info = $('#info').val();
    var warning = $('#warning').val();

    if (success != null) {
        displayMessage(success, 'success');
        return true;
    }
    if (info != null) {
        displayMessage(info, 'info');
        return true;
    }
    if (warning != null) {
        displayMessage(warning, 'warning');
        return true;
    }
    if (error != null) {
        displayMessage(error, 'error');
        return false;
    }
};

var displayMessage = function (message, msgType) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-right",
        "onClick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "8000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    toastr[msgType](message);
};