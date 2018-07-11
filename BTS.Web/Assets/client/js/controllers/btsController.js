
var userRoleAdmin = "@(User.IsInRole('System_CanExport') ? 'true' : 'false')";
var data = "";

var btsController = {
    init: function () {       
        btsController.registerEventDataTable();
        btsController.registerEvent();
    },
    registerEventDataTable: function () {
        
    },
    registerEvent: function () {
        $('#btnSearch').off('click').on('click', function () {
            btsController.loadData();
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/Operator/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else if (response.status == "Success") {
                    data = response.data;
                    $('#hidID').val(data.ID);
                    $('#txtCode').val(data.Code);
                    $('#txtName').val(data.Name);
                }
                else {
                    //bootbox.alert(response.message);
                    $.notify(response.message, {
                        className: "warn"
                    });
                }
            },
            error: function (err) {
                console.log(err);
                $.notify(err.message, {
                    className: "error",
                    clickToHide: true
                });
            }
        });
    },

    loadDdata: function () {
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
                $('#btnSubmit').prop('disabled', true);
                $('#btnReset').prop('disabled', true);
                $('#selectOperatorId').prop('disabled', true);
                $('#selectCityId').prop('disabled', true);
            },
            beforeSend: function () {
                $('html').addClass('waiting');
                bar.html('Bắt đầu thực hiện!');
                bar.addClass('active');
                $('#progressRow').show();
            },
            uploadProgress: function (event, position, total, percentComplete) {
                if (percentComplete = 100)
                    bar.html('Đã gửi yêu cầu thống kê ....');
                else
                    bar.html('Đang gửi yêu cầu thống kê : ' + percentComplete + '%');
            },
            error: function (data) {
                var r = jQuery.parseJSON(data.responseText);
                alert("Message: " + r.message);
                alert("StackTrace: " + r.StackTrace);
                alert("ExceptionType: " + r.ExceptionType);
                $('html').removeClass('waiting');
                bar.removeClass('active');
                $('#btnSubmit').prop('disabled', false);
                $('#btnReset').prop('disabled', false);
                $('#selectOperatorId').prop('disabled', false);
                $('#selectCityId').prop('disabled', false);
            },
            success: function (response, statusText, xhr, element) {
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else if (response.status == "Success") {
                    bar.html('Đã thực hiện xong!');
                    var pieChartColumNames = response.chartData[0];

                    var pieChartLabels = response.chartData[1];

                    var pieChartOptions = {
                        responsive: true,
                        maintainAspectRatio: true
                    }
                }
                else {
                    bar.html('Lỗi trong quá trình thực hiện!');
                    alert(xhr.response.message);
                }
                $('html').removeClass('waiting');
                bar.removeClass('active');
                $('#btnSubmit').prop('disabled', false);
                $('#btnReset').prop('disabled', false);
                $('#selectOperatorId').prop('disabled', false);
                $('#selectCityId').prop('disabled', false);
            },
            complete: function (xhr) {
            },
            async: true
        });
    }

}

certificateController.init();
