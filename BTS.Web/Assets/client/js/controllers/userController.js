var fileLocation, fileExtension;

var userController = {
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
                $('#btnCheck').prop('disabled', true);
                $('#btnReset').prop('disabled', true);
                $('#FileDialog').prop('disabled', true);
            },
            beforeSend: function () {
                $('html').addClass('waiting');
                bar.html('Bắt đầu thực hiện!');
                bar.addClass('active');
                $('#progressRow').show();
            },
            uploadProgress: function (event, position, total, percentComplete) {
                if (percentComplete = 100)
                    bar.html('Đã Upload File xong đang thực hiện nhập khẩu người dùng ....');
                else
                    bar.html('Đang thực hiện Upload File được: ' + percentComplete + '%');
            },
            error: function (data) {
                var r = jQuery.parseJSON(data.responseText);
                alert("Message: " + r.message);
                alert("StackTrace: " + r.StackTrace);
                alert("ExceptionType: " + r.ExceptionType);
                bar.html('Lỗi trong quá trình thực hiện!');
                $('#btnCheck').prop('disabled', false);
                $('#btnReset').prop('disabled', false);
                $('#FileDialog').prop('disabled', false);
                $('html').removeClass('waiting');
                bar.removeClass('active');
            },
            success: function (responseJSON, statusText, xhr, element) {
                if (responseJSON.status == "Success") {
                    bar.html('Đã thực hiện nhập khẩu người dùng xong!');
                    $.notify(xhr.responseJSON.message, "success");
                    //alert(xhr.responseJSON.message);
                }
                else if (responseJSON.status == "TimeOut") {
                    $.notify(responseJSON.message, "warn");
                    window.location.href = "/Account/Login"
                }
                else {
                    bar.html('Lỗi trong quá trình thực hiện!');
                    $.notify(xhr.responseJSON.message, "error");
                    //alert(xhr.responseJSON.message);
                }
                $('html').removeClass('waiting');
                bar.removeClass('active');
                $('#btnCheck').prop('disabled', false);
                $('#btnReset').prop('disabled', false);
                $('#FileDialog').prop('disabled', false);
            },
            complete: function (xhr) {
            },
            async: true
        });
    },

    registerEvent: function () {
        $('#FileDialog').change(function (sender) {
            var fileName = sender.target.files[0].name;
            var validExts = new Array(".xlsx", ".xls");
            var fileExt = fileName.substring(fileName.lastIndexOf('.'));
            if (validExts.indexOf(fileExt) < 0) {
                //alert("Bạn chỉ được các tập tin Excel " + validExts.toString() + " để nhập liệu");
                $.notify("Bạn chỉ được các tập tin Excel " + validExts.toString() + " để nhập liệu", "warn");
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
userController.init();