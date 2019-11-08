var recoveryController = {
    token: function () {
        var form = $('#__AjaxAntiForgeryForm');
        return $('input[name="__RequestVerificationToken"]', form).val();
    },

    init: function () {
        
    },
    registerEventDataTable: function () {
    },
    registerEvent: function () {
    },

    jQueryAjaxPost: function (form) {
        $.validator.unobtrusive.parse(form);
        if ($(form).valid()) {
            var ajaxConfig = {
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                success: function (response) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login";
                    } else if (response.status == "Error") {
                        $.notify(response.message, "error");
                    } else if (response.status == "Success") {
                        $.notify(response.message, "Success");
                    } else if (response.status == "Recovery") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login";
                    } else {
                        $.notify(response.message, "error");
                    }
                },
                error: function (response) {
                    $.notify(response.error, "error");
                }
            }
            if ($(form).attr('enctype') == "multipart/form-data") {
                ajaxConfig["contentType"] = false;
                ajaxConfig["processData"] = false;
            }
            $.ajax(ajaxConfig);
        }
        return false;
    }
};
recoveryController.init();