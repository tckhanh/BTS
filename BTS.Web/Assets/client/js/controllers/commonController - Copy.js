$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

function ShowImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        };
        reader.readAsDataURL(imageUploader.files[0]);
    }
}

function jQueryAjaxPost(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var ajaxConfig = {
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            success: function (response) {
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else if (response.status = "Success") {
                    $("#firstTab").html(response.html);
                    refreshAddNewTab($(form).attr('data-restUrl'), true);
                    $.notify(response.message, "success");
                    if (typeof activatejQueryTable !== 'undefined' && $.isFunction(activatejQueryTable))
                        activatejQueryTable();
                }
                else {
                    $.notify(response.message, "error");
                }
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

function refreshAddNewTab(resetUrl, showViewTab) {
    $.ajax({
        type: 'GET',
        url: resetUrl,
        success: function (response) {
            if (response.status == "TimeOut") {
                $.notify(response.message, "warn");
                window.location.href = "/Account/Login"
            } else {
                $("#secondTab").html(response);
                $('ul.nav.nav-tabs a:eq(1)').html(' Thêm mới');
                if (showViewTab)
                    $('ul.nav.nav-tabs a:eq(0)').tab('show');
            }
        }
    });
}

function Edit(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            if (response.status == "TimeOut") {
                $.notify(response.message, "warn");
                window.location.href = "/Account/Login"
            } else {
                $("#secondTab").html(response);
                $('ul.nav.nav-tabs a:eq(1)').html('Cập nhật');
                $('ul.nav.nav-tabs a:eq(1)').tab('show');
            }
        }
    });
}

function GetData(url) {
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.status == "TimeOut") {
                $.notify(response.message, "warn");
                window.location.href = "/Account/Login"
            } else if (response.status == "Success") {
                return response.data;
            }
            else {
                //bootbox.alert(response.message);
                $.notify(response.message, {
                    className: "warn"
                });
                return "";
            }
        },
        error: function (err) {
            console.log(err);
            $.notify(err.message, {
                className: "error",
                clickToHide: true
            });
            return "";
        }
    });
}

function Delete(url) {
    if (confirm('Are you sure to delete this record ?') == true) {
        $.ajax({
            type: 'POST',
            url: url,
            success: function (response) {
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else if (response.status = "Success") {
                    $("#firstTab").html(response.html);
                    $.notify(response.message, "warn");
                    if (typeof activatejQueryTable !== 'undefined' && $.isFunction(activatejQueryTable))
                        activatejQueryTable();
                }
                else {
                    $.notify(response.message, "error");
                }
            }
        });
    }
}