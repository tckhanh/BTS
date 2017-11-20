$(function () {

    function getData() {
        var data = [];
        $.ajax({
            url: '/Home/GetCertificateSumary',
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                data = response;
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