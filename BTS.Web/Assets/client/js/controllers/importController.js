$(function () {
    var importController = {
        init: function () {
            var bar = $('.progress-bar');
            $('#jqueryForm').ajaxForm({
                clearForm: true,
                dataType: 'json',
                forceSync: false,
                beforeSerialize: function ($form, options) {
                    // return false to cancel submit
                },
                beforeSubmit: function (arr, $form, options) {
                    // The array of form data takes the following form:
                    // [ { name: 'username', value: 'jresig' }, { name: 'password', value: 'secret' } ]
                    // return false to cancel submit
                    $('#btnImport').prop('disabled', true);
                    $('#btnReset').prop('disabled', true);
                    $('#FileDialog').prop('disabled', true);
                    $('#ImportAction').prop('disabled', true);
                },
                beforeSend: function () {
                    $('html').addClass('waiting');
                    bar.html('Bắt đầu thực hiện!');
                    bar.addClass('active');
                    $('#progressRow').show();
                },
                uploadProgress: function (event, position, total, percentComplete) {
                    if (percentComplete = 100)
                        bar.html('Đã Upload File xong đang thực hiện nhập hồ sơ Kiểm định ....');
                    else
                        bar.html('Đang thực hiện Upload File được: ' + percentComplete + '%');
                },
                error: function (data) {
                    var r = jQuery.parseJSON(data.responseText);
                    alert("Message: " + r.message);
                    alert("StackTrace: " + r.StackTrace);
                    alert("ExceptionType: " + r.ExceptionType);
                    $('html').removeClass('waiting');
                    bar.removeClass('active');
                    $('#btnImport').prop('disabled', false);
                    $('#btnReset').prop('disabled', false);
                    $('#FileDialog').prop('disabled', false);
                    $('#ImportAction').prop('disabled', false);
                },
                success: function (responseJSON, statusText, xhr, element) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (responseJSON.status == "Success")
                        bar.html('Đã thực hiện xong!');
                    else {
                        bar.html('Lỗi trong quá trình thực hiện!');
                        alert("Complete: " + xhr.responseJSON.message);
                    }
                    $('html').removeClass('waiting');
                    bar.removeClass('active');
                    $('#btnImport').prop('disabled', false);
                    $('#btnReset').prop('disabled', false);
                    $('#FileDialog').prop('disabled', false);
                    $('#ImportAction').prop('disabled', false);
                    //$("#ImportAction").val("ImportBTS");
                },
                complete: function (xhr) {
                },
                async: true
            });
        },
        registerEventDataTable: function () {
        },
        registerEvent: function () {
            $('#FileDialog').change(function (sender) {
                var fileName = sender.target.files[0].name;
                var validExts = new Array(".xlsx", ".xls");
                var fileExt = fileName.substring(fileName.lastIndexOf('.'));
                if (validExts.indexOf(fileExt) < 0) {
                    alert("Bạn chỉ được chọn các tập tin Excel " + validExts.toString() + " để nhập liệu");
                    $("#FileDialog").val('');
                    return false;
                }
                else {
                    bar.html('');
                    return true;
                }
            });
        }
    }

    importController.init();
});