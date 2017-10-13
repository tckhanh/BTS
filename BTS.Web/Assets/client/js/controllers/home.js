var home = {
    init: function () {
        home.registerEvent();
    },
    registerEvent: function () {
        home.getCertificateSumary();
    },

    getCertificateSumary: function () {
        $.ajax({
            url: '/Home/GetCertificateSumary',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var OperatorID = [];
                var Certificates = [];

                if (response.status) {
                    $.each(response.data, function (i, item) {
                        OperatorID.push(item.OperatorID);
                        Certificates.push(item.Certificates);
                    });
                    chartData.push(OperatorID);
                    chartData.push(Certificates);
                    return chartData;
                }
            }
        });
    },

    initMap: function () {
        var uluru = { lat: parseFloat($('#hidLat').val()), lng: parseFloat($('#hidLng').val()) };
        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 17,
            center: uluru
        });

        var contentString = $('#hidAddress').val();

        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        var marker = new google.maps.Marker({
            position: uluru,
            map: map,
            title: $('#hidName').val()
        });
        infowindow.open(map, marker);
    }
}

home.init();