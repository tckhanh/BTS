var addinController = {
    init: function () {
        $("#loaderbody").addClass('hide');
        $(document).bind('ajaxStart', function () {
            $("#loaderbody").removeClass('hide');
        }).bind('ajaxStop', function () {
            $("#loaderbody").addClass('hide');
        });
        addinController.activatejQueryTable();
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

    // for AddOrEdit with Method Post.
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
                        // $("#firstTab").html(response.html);
                        addinController.refreshAddNewTab($(form).attr('data-restUrl'), true);
                        $('#MyDataTable').DataTable().ajax.reload();
                        $.notify(response.message, "success");
                        if (typeof addinController.activatejQueryTable !== 'undefined' && $.isFunction(addinController.activatejQueryTable))
                            addinController.activatejQueryTable();
                    }
                    else {
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

    refreshAddNewTab: function (resetUrl, showViewTab) {
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
                    $("#AddOrEditTab").html(response);
                    $('ul.nav.nav-tabs a:eq(1)').html(' Thêm mới');
                    if (showViewTab)
                        $('ul.nav.nav-tabs a:eq(0)').tab('show');
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

    doContextAction: function (cmd, id, long, lat) {
        switch (cmd) {
            case myConstant.Action_Detail:
                addinController.Detail(window.location.href + '/' + myConstant.Action_Detail + '/' + id);
                break;
            case myConstant.Action_Edit:
                addinController.Edit(window.location.href + '/' + myConstant.Action_Edit + '/' + id);
                break;
            case myConstant.Action_Delete:
                addinController.Delete(window.location.href + '/' + myConstant.Action_Delete + '/' + id);
                break;
            case myConstant.Action_Lock:
                addinController.Lock(window.location.href + '/' + myConstant.Action_Lock + '/' + id);
                break;
            case myConstant.Action_Change:
                addinController.Edit(window.location.href + '/' + myConstant.Action_Change + '/' + id);
                break;
            case myConstant.Action_Reset:
                addinController.Reset(window.location.href + '/' + myConstant.Action_Reset + '/' + id);
                break;
            case myConstant.Action_ViewMap:
                addinController.ViewMap(id, long, lat);
                break;
            case myConstant.Action_Print:
                //addinController.Print('http://' + window.location.host + '/PrintCertificate/Index/' + id);
                if (typeof homeController !== 'undefined') {
                    homeController.printCertificate(id);
                }else if (typeof certificateController !== 'undefined') {
                    certificateController.printCertificate(id)
                }
                break;
        }
    },

    ViewMap: function (id, long, lat) {
        var latLon = L.latLng(lat, long);
        var bounds = latLon.toBounds(500); // 500 = metres
        myMap.panTo(latLon).fitBounds(bounds);
        myMap.setZoom(16);
        $('ul.nav.nav-tabs a:eq(2)').tab('show');
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
                    $("#AddOrEditTab").html(response);
                    $('ul.nav.nav-tabs a:eq(1)').html('Sửa đổi');
                    $('ul.nav.nav-tabs a:eq(1)').tab('show');
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
                    $("#AddOrEditTab").html(response);
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
                    $("#AddOrEditTab").html(response);
                    $('ul.nav.nav-tabs a:eq(1)').html('Chi tiết');
                    $('ul.nav.nav-tabs a:eq(1)').tab('show');
                }
            }
        });
    },

    Print: function (url) {
        //open a new window note:this is a popup so it may be blocked by your browser
        //var newWindow = window.open(url, '_blank');
        var newWindow = window.open(url, '_blank');
        if (newWindow) {
            //Browser has allowed it to be opened
            newWindow.focus();
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
                success: function (response) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Error") {
                        $.notify(response.message, "error");
                    } else if (response.status = "Success") {
                        // $("#firstTab").html(response.html);
                        $('#MyDataTable').DataTable().ajax.reload();
                        $.notify(response.message, "warn");
                        if (typeof addinController.activatejQueryTable !== 'undefined' && $.isFunction(addinController.activatejQueryTable))
                            addinController.activatejQueryTable();
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
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Error") {
                        $.notify(response.message, "error");
                    } else if (response.status = "Success") {
                        // $("#firstTab").html(response.html);
                        $('#MyDataTable').DataTable().ajax.reload();
                        $.notify(response.message, "warn");
                        if (typeof addinController.activatejQueryTable !== 'undefined' && $.isFunction(addinController.activatejQueryTable))
                            addinController.activatejQueryTable();
                    }
                    else {
                        $.notify(response.message, "error");
                    }
                }
            });
        }
    },

    activatejQueryTable: function () {
        //$("#MyDataTable").DataTable({
        //    "language": {
        //        url: '/AppFiles/localization/vi_VI.json'
        //    },
        //    initComplete: function () {
        //        this.api().columns().every(function () {
        //            var column = this;
        //            var select = $('<select><option value=""></option></select>')
        //            .appendTo($(column.footer()).empty())
        //            .on('change', function () {
        //                var val = $.fn.dataTable.util.escapeRegex($(this).val());
        //                column.search(val ? '^' + val + '$' : '', true, false).draw();
        //            });
        //            column.data().unique().sort().each(function (d, j) {
        //                select.append('<option value="' + d + '">' + d + '</option>')
        //            });
        //        });
        //    }
        //});
    }
};
addinController.init();