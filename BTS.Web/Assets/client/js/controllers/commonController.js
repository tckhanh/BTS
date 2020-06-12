var commonController = {
    token: function () {
        var form = $('#__AjaxAntiForgeryForm');
        return $('input[name="__RequestVerificationToken"]', form).val();
    },

    init: function () {
        $("#loaderbody").addClass('hide');
        $(document).bind('ajaxStart', function () {
            $("#loaderbody").removeClass('hide');
        }).bind('ajaxStop', function () {
            $("#loaderbody").addClass('hide');
        });
        commonController.activatejQueryTable();
        commonController.registerEventDataTable();
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
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else if (response.status == "Error") {
                    $.notify(response.message, "error");
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
    },    

    loadDetail: function (id) {
        alert(id);
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
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Error") {
                        $.notify(response.message, "error");
                    } else if (response.status = "Success") {
                        $("#firstTab").html(response.html);                       
                        $.notify(response.message, "Success");
                        if (typeof applicationUserProfileController !== 'undefined') {
                            commonController.refreshAddNewTab(response.data_restUrl, 1, 'Chi tiết');
                        } else {
                            commonController.refreshAddNewTab(response.data_restUrl, 0, 'Thêm mới');
                        }
                        if (typeof commonController.activatejQueryTable !== 'undefined' && $.isFunction(commonController.activatejQueryTable))
                            commonController.activatejQueryTable();
                    } else if (response.status = "Recovery") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else {
                        $.notify(response.message, "error");
                    }
                },
                error: function (response) {
                    $.notify(response.message, "error");
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

    refreshAddNewTab: function (resetUrl, showTab, secondTabTitle) {
        $.ajax({
            type: 'GET',
            url: resetUrl,
            success: function (response) {
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else if (response.status == "Error") {
                    $.notify(response.message, "error");
                } else {
                    $("#secondTab").html(response);
                    $('ul.nav.nav-tabs a:eq(1)').html(secondTabTitle);
                    $('ul.nav.nav-tabs a:eq(' + showTab + ')').tab('show');
                }
            }
        });
    },

    refreshShowTab: function (resetUrl, showViewTab) {
        $.ajax({
            type: 'GET',
            url: resetUrl,
            success: function (response) {
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else if (response.status == "Error") {
                    $.notify(response.message, "error");
                } else {
                    $('ul.nav.nav-tabs a:eq(1)').html(' Thêm mới');
                    if (showViewTab)
                        $('ul.nav.nav-tabs a:eq(0)').tab('show');
                }
            }
        });
    },

    doContextAction: function (cmd, id) {
        switch (cmd) {
            case myConstant.Action_Detail:
                commonController.Detail(window.location.href + '/' + myConstant.Action_Detail + '/' + id);
                break;
            case myConstant.Action_Edit:
                commonController.Edit(window.location.href + '/' + myConstant.Action_Edit + '/' + id);
                break;
            case myConstant.Action_Delete:
                commonController.Delete(window.location.href + '/' + myConstant.Action_Delete + '/' + id);
                break;
            case myConstant.Action_Lock:
                commonController.Lock(window.location.href + '/' + myConstant.Action_Lock + '/' + id);
                break;
            case myConstant.Action_Change:
                commonController.Edit(window.location.href + '/' + myConstant.Action_Change + '/' + id);
                break;
            case myConstant.Action_Reset:
                commonController.Reset(window.location.href + '/' + myConstant.Action_Reset + '/' + id);
                break;
        }
    },

    Edit: function (url) {
        $.ajax({
            type: 'GET',
            url: url,
            success: function (response) {
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else if (response.status == "Error") {
                    $.notify(response.message, "error");
                } else {
                    $("#secondTab").html(response);
                    $('ul.nav.nav-tabs a:eq(1)').html('Sửa đổi');
                    $('ul.nav.nav-tabs a:eq(1)').tab('show');
                    if (typeof applicationUserController !== 'undefined') {
                        applicationUserController.init();
                    }
                    if (typeof applicationUserProfileController !== 'undefined') {
                        applicationUserProfileController.init();
                    }
                }
            }
        });
    },

    Reset: function (url) {
        $.ajax({
            type: 'GET',
            url: url,
            success: function (response) {
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else if (response.status == "Error") {
                    $.notify(response.message, "error");
                } else {
                    $("#secondTab").html(response);
                    $('ul.nav.nav-tabs a:eq(1)').html('Đặt lại mật khẩu');
                    $('ul.nav.nav-tabs a:eq(1)').tab('show');
                }
            }
        });
    },

    Detail: function (url) {
        $.ajax({
            type: 'GET',
            url: url,
            success: function (response) {
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else if (response.status == "Error") {
                    $.notify(response.message, "error");
                } else {
                    $("#secondTab").html(response);
                    $('ul.nav.nav-tabs a:eq(1)').html('Chi tiết');
                    $('ul.nav.nav-tabs a:eq(1)').tab('show');
                }
            }
        });
    },

    Print: function (url) {
        var win = window.open(url, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        } else {
            //Browser has blocked it
            alert('Please allow popups for this website');
        }        
    },

    Lock: function (url) {
        if (confirm('Bạn có chắc chắn muốn Khóa/ Mở khóa dữ liệu này không?') == true) {
            $.ajax({
                type: 'POST',
                url: url,
                data: {
                    __RequestVerificationToken: commonController.token()
                },
                success: function (response) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Error") {
                        $.notify(response.message, "error");
                    } else if (response.status = "Success") {
                        $("#firstTab").html(response.html);
                        $.notify(response.message, "warn");
                        if (typeof commonController.activatejQueryTable !== 'undefined' && $.isFunction(commonController.activatejQueryTable))
                            commonController.activatejQueryTable();
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
                data: {
                    __RequestVerificationToken: commonController.token()
                },
                success: function (response) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Error") {
                        $.notify(response.message, "error");
                    } else if (response.status = "Success") {
                        $("#firstTab").html(response.html);
                        $.notify(response.message, "warn");
                        if (typeof applicationUserProfileController !== 'undefined') {
                            commonController.refreshAddNewTab(response.data_restUrl, 1, 'Chi tiết');
                        } else {
                            commonController.refreshAddNewTab(response.data_restUrl, 0, 'Thêm mới');
                        }
                        
                        if (typeof commonController.activatejQueryTable !== 'undefined' && $.isFunction(commonController.activatejQueryTable))
                            commonController.activatejQueryTable();
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
                url: '/AppFiles/localization/vi_VI.json'
            },
            initComplete: function () {
                this.api().columns().every(function () {
                    var column = this;
                    var select = $('<select><option value=""></option></select>')
                        .appendTo($(column.footer()).empty())
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex($(this).val());
                            column.search(val ? '^' + val + '$' : '', true, false).draw();
                        });
                    column.data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>')
                    });
                });
            }
        });
    }
};
commonController.init();