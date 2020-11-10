$(function () {
    var importController = {
        init: function () {
            importController.registerEvent();
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
                error: function (request, status, error) {
                    //var r = jQuery.parseJSON(request.responseText);
                    //alert("Message: " + r.message);
                    //alert("StackTrace: " + r.StackTrace);
                    //alert("ExceptionType: " + r.ExceptionType);
					var dom_nodes = $($.parseHTML(request.responseText));
                    alert('Error: ' + dom_nodes[1].innerText);

                    $('html').removeClass('waiting');
                    bar.html('Lỗi trong khi thực hiện!');
                    bar.removeClass('active');
                    $('#btnImport').prop('disabled', false);
                    $('#btnReset').prop('disabled', false);
                    $('#FileDialog').prop('disabled', false);
                    $('#ImportAction').prop('disabled', false);
                },
                success: function (response, statusText, xhr, element) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Success") {
                        bar.html('Đã thực hiện xong!');
                        //$('#ImportAction option').eq(2).prop('selected', true)
                    }
                    else {
                        bar.html('Lỗi trong quá trình thực hiện!');
                        alert("Complete: " + response.message);
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
                async: true,

                // other available options: 
                //url:       url         // override for form's 'action' attribute 
                //type:      type        // 'get' or 'post', override for form's 'method' attribute 
                //dataType:  null        // 'xml', 'script', or 'json' (expected server response type) 

                clearForm: false,        // clear all form fields after successful submit 
                resetForm: true       // reset the form after successful submit 

                // $.ajax options can be used here too, for example: 
                //timeout:   3000
            });
        },
        registerEventDataTable: function () {
        },
        registerEvent: function () {
            $('#btnGetSampleFileTT1').off('click').on('click', function () {
                importController.updateFileSample('TT1_Data_Ketqua.xlsm');
            });

            $('#btnGetSampleFileTT3').off('click').on('click', function () {
                importController.updateFileSample('TT3_Data_Ketqua.xlsm');
            });


            $('#FileDialog').change(function (sender) {
                var bar = $('.progress-bar');
                var fileName = sender.target.files[0].name;
                var validExts = new Array(".xlsx", ".xlsm", ".xls");
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
        , 
        updateFileSample: function (updatedFileName) {
            var bar = $('.progress-bar');
            $.ajax({
                url: '/ImportData/GetSampleFile',
                data: {
                    fileName: updatedFileName
                },
                type: 'GET',
                dataType: 'json',
                beforeSend: function () {
                    $('html').addClass('waiting');
                    bar.html('Bắt đầu thực hiện!');
                    bar.addClass('active');
                    $('#progressRow').show();
                },
                error: function (err) {
                    console.log(err.message);
                    $.notify(err.message, {
                        className: "error",
                        clickToHide: true
                    });
                    $('html').removeClass('waiting');
                    bar.removeClass('active');
                    $('#btnImport').prop('disabled', false);
                    $('#btnReset').prop('disabled', false);
                    $('#FileDialog').prop('disabled', false);
                    $('#ImportAction').prop('disabled', false);
                    $('#ImportAction').prop('disabled', false);
                },
                success: function (response) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Success") {
                        $.notify(response.message, "info");
                        bar.html('Đã thực hiện xong!');
                        //$('#ImportAction option').eq(2).prop('selected', true)
                    }
                    else {
                        bar.html('Lỗi trong quá trình thực hiện!');
                        alert("Complete: " + response.message);
                    }
                    $('html').removeClass('waiting');
                    bar.removeClass('active');
                    $('#btnImport').prop('disabled', false);
                    $('#btnReset').prop('disabled', false);
                    $('#FileDialog').prop('disabled', false);
                    $('#ImportAction').prop('disabled', false);
                    //$("#ImportAction").val("ImportBTS");
                },
            });
        }
    }

    importController.init();
});