$(function () {
    function getData() {
        var data = [];
        $.ajax({
            url: '/Home/GetCertificateSumary',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else {
                    data = response;
                }
            }
        });
        return data;
    }

    var chartData = getData();

    $("#output").pivotUI(chartData,
        {
            rows: ["Year"],
            cols: ["Certificates"]
        });
});