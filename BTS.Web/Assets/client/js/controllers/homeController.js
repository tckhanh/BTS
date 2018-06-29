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
                    alert("Message: " + r.Message);
                    alert("StackTrace: " + r.StackTrace);
                    alert("ExceptionType: " + r.ExceptionType);
                    $('html').removeClass('waiting');
                    bar.removeClass('active');
                    $('#btnSubmit').prop('disabled', false);
                    $('#btnReset').prop('disabled', false);
                    $('#selectOperatorId').prop('disabled', false);
                    $('#selectCityId').prop('disabled', false);
                },
                success: function (responseJSON, statusText, xhr, element) {
                    if (responseJSON.Status == "Success") {
                        bar.html('Đã thực hiện xong!');

                        var pieChartValues = [];
                        var pieChartLabels = [];

                        for (var i in responseJSON.DataByOperator) {
                            pieChartLabels.push(responseJSON.DataByOperator[i].OperatorID);
                            pieChartValues.push(responseJSON.DataByOperator[i].Certificates);
                        }
                        pieChartData = {
                            // These labels appear in the legend and in the tooltips when hovering different arcs
                            labels: pieChartLabels,
                            datasets: [{
                                label: 'Player Score',
                                backgroundColor: ["rgba(54, 162, 235, 0.2)", "rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                                borderColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                                hoverBackgroundColor: "rgba(153, 102, 255, 1)",
                                hoverBorderColor: "rgba(153, 102, 255, 1)",
                                data: pieChartValues
                            }],
                        };
                        var pieChartOptions = {
                            responsive: true,
                            maintainAspectRatio: true
                        }
                        var pieChartCanvas = $("#pieChart");
                        var pieChartt = new Chart(pieChartCanvas, {
                            type: 'pie',
                            data: pieChartData,
                            options: pieChartOptions
                        });
                    }
                    else {
                        bar.html('Lỗi trong quá trình thực hiện!');
                        alert(xhr.responseJSON.Message);
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
        },
        registerEventDataTable: function () {
        },
        registerEvent: function () {
        }
    }

    importController.init();
});