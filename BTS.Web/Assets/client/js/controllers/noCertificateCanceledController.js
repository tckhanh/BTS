﻿
var noCertificatecanceledController = {
    emailRegex: /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0, 61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0, 61}[a-zA-Z0-9])?)*$/,
    certificateNum: $("#certificateNum"),
    canceledReason: $("#canceledReason"),
    canceledDate: $("#canceledDate"),
    tips: $(".validateTips"),
    form: function () {
        return noCertificatecanceledController.showDialog.find("form").on("submit", function (event) {
            event.preventDefault();
            cancelCertificate();
        });
    },
    // From http://www.whatwg.org/specs/web-apps/current-work/multipage/states-of-the-type-attribute.html#e-mail-state-%28type=email%29

    init: function () {
        noCertificatecanceledController.registerEvent();
    },

    showDialog: $("#dialog-form").dialog({
        autoOpen: false,
        height: 550,
        width: 450,
        modal: true,
        dialogClass: 'alert',
        resizable: false,
        buttons: {
            "send": {
                text: 'Thu hồi',
                class: 'btn btn-success',
                click: function () {
                    noCertificatecanceledController.cancelNoCertificate();
                }
            },
            "cancel": {
                text: 'Thoát',
                class: 'btn btn-info',
                click: function () {
                    noCertificatecanceledController.showDialog.dialog("close");
                }
            }
        },
        close: function () {
        }
    }),

    registerEvent: function () {
        $("#create-user").button().on("click", function () {
            noCertificatecanceledController.dialog.dialog("open");
        });


    },
    updateTips: function (t) {
        noCertificatecanceledController.tips
            .text(t)
            .addClass("ui-state-highlight");
        setTimeout(function () {
            noCertificatecanceledController.tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    },

    checkLength: function (o, n, min, max) {
        if (o.val().length > max || o.val().length < min) {
            o.addClass("ui-state-error");
            noCertificatecanceledController.updateTips("Length of " + n + " must be between " +
                min + " and " + max + ".");
            return false;
        } else {
            return true;
        }
    },

    checkRegexp: function (o, regexp, n) {
        if (!(regexp.test(o.val()))) {
            o.addClass("ui-state-error");
            noCertificatecanceledController.updateTips(n);
            return false;
        } else {
            return true;
        }
    },
    
    cancelNoCertificate: function () {
        var form = $("#__AjaxAntiForgeryForm")[0];
        var dataForm = new FormData(form);
        if ($("#noCertificateId").val() != null) {
            dataForm.append('noCertificateId', $("#noCertificateId").val());            
        }
        dataForm.append('canceledReason', $("#canceledReason").val());
        dataForm.append('canceledDate', $("#canceledDate").val());
        $.validator.unobtrusive.parse(form);
        if ($(form).valid()) {
            $('html').addClass('waiting');
            var ajaxConfig = {
                type: 'POST',
                url: '/NoCertificate/Cancel',
                data: dataForm,
                success: function (response) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Error") {
                        $.notify(response.message, "error");
                    } else if (response.status = "Success") {
                        // $("#firstTab").html(response.html);
                        addinController.refreshAddNewTab($(form).attr('data-restUrl'), true);
                        $('#MyDataTable').DataTable().ajax.reload();
                        $.notify(response.message, "success");
                    }
                    else {
                        $.notify(response.message, "error");
                    }
                    noCertificatecanceledController.showDialog.dialog("close");
                },
                error: function (response) {
                    $.notify(response.message, "error");
                    noCertificatecanceledController.showDialog.dialog("close");
                }
            }
            if ($(form).attr('enctype') == "multipart/form-data") {
                ajaxConfig["contentType"] = false;
                ajaxConfig["processData"] = false;
            }
            $.ajax(ajaxConfig);
        }
        return false;
    },
};

noCertificatecanceledController.init();
