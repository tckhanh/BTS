var commonController = {
    init: function () {
        $("#loaderbody").addClass('hide');
        $(document).bind('ajaxStart', function () {
            $("#loaderbody").removeClass('hide');
        }).bind('ajaxStop', function () {
            $("#loaderbody").addClass('hide');
        });
        commonController.activatejQueryTable();
    },
    registerEventDataTable: function () {
    },
    registerEvent: function () {
    },

    loadData: function (url) {
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
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
                $.notify(err.Message, {
                    className: "error",
                    clickToHide: true
                });
                return "";
            }
        });
    },
    loadDetail: function (id) {
    },
    saveData: function () {
    },
    resetForm: function () {
    },
    cancelForm: function () {
    },

    deleteItem: function (id) {
    },

    ShowImagePreview: function (imageUploader, previewImage) {
        if (imageUploader.files && imageUploader.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $(previewImage).attr('src', e.target.result);
            };
            reader.readAsDataURL(imageUploader.files[0]);
        }
    },

    jQueryAjaxPost: function (form) {
        $.validator.unobtrusive.parse(form);
        if ($(form).valid()) {
            var ajaxConfig = {
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                success: function (response) {
                    if (response.success) {
                        $("#firstTab").html(response.html);
                        commonController.refreshAddNewTab($(form).attr('data-restUrl'), true);
                        $.notify(response.message, "success");
                        if (typeof activatejQueryTable !== 'undefined' && $.isFunction(activatejQueryTable))
                            activatejQueryTable();
                    }
                    else {
                        $.notify(response.message, "error");
                    }
                },
                error: function (response) { }
            }
            if ($(form).attr('enctype') == "multipart/form-data") {
                ajaxConfig["contentType"] = false;
                ajaxConfig["processData"] = false;
            }
            $.ajax(ajaxConfig);
        }
        return false;
    },

    refreshAddNewTab: function (resetUrl, showViewTab) {
        $.ajax({
            type: 'GET',
            url: resetUrl,
            success: function (response) {
                $("#secondTab").html(response);
                $('ul.nav.nav-tabs a:eq(1)').html(' Thêm mới');
                if (showViewTab)
                    $('ul.nav.nav-tabs a:eq(0)').tab('show');
            }
        });
    },

    refreshShowTab: function (resetUrl, showViewTab) {
        $.ajax({
            type: 'GET',
            url: resetUrl,
            success: function (response) {
                $('ul.nav.nav-tabs a:eq(1)').html(' Thêm mới');
                if (showViewTab)
                    $('ul.nav.nav-tabs a:eq(0)').tab('show');
            }
        });
    },

    Edit: function (url) {
        $.ajax({
            type: 'GET',
            url: url,
            success: function (response) {
                $("#secondTab").html(response);
                $('ul.nav.nav-tabs a:eq(1)').html('Sửa đổi');
                $('ul.nav.nav-tabs a:eq(1)').tab('show');
            }
        });
    },

    Reset: function (url) {
        $.ajax({
            type: 'GET',
            url: url,
            success: function (response) {
                $("#secondTab").html(response);
                $('ul.nav.nav-tabs a:eq(1)').html('Đặt lại mật khẩu');
                $('ul.nav.nav-tabs a:eq(1)').tab('show');
            }
        });
    },

    Detail: function (url) {
        $.ajax({
            type: 'GET',
            url: url,
            success: function (response) {
                $("#secondTab").html(response);
                $('ul.nav.nav-tabs a:eq(1)').html('Chi tiết');
                $('ul.nav.nav-tabs a:eq(1)').tab('show');
            }
        });
    },
    Lock: function (url) {
        if (confirm('Bạn có chắc chắn muốn Khóa/ Mở khóa dữ liệu này không?') == true) {
            $.ajax({
                type: 'POST',
                url: url,
                success: function (response) {
                    if (response.success) {
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
    },

    Delete: function (url) {
        if (confirm('Bạn có chắc chắn muốn xóa dữ liệu này không?') == true) {
            $.ajax({
                type: 'POST',
                url: url,
                success: function (response) {
                    if (response.success) {
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
    },

    activatejQueryTable: function () {
        $("#MyDataTable").DataTable({
            "language": {
                url: '/localization/vi_VI.json'
            }
        });
    }
};

commonController.init();