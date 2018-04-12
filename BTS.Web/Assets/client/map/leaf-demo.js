// See post: http://asmaloney.com/2015/06/code/clustering-markers-on-leaflet-maps

var tiles = L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
    maxZoom: 18,
    attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
}),
    latlng = L.latLng(10.796841, 106.66252);

var map = L.map('mapBTS', { center: latlng, zoom: 11, layers: [tiles] });

            //$('body').on('shown.bs.modal', function (e) {
            //    setTimeout(function () { map.invalidateSize() }, 500);
            //})

var markerClusters = L.markerClusterGroup();
loadCertificatedBTS();
map.addLayer(markerClusters);

function getData() {
    var data = [];
    $.ajax({
        url: '/Certificate/GetCertificate',
        type: 'GET',
        dataType: 'json',
        async: false,
        success: function (response) {
            data = response;
        }
    });
    return data;
}

function loadCertificatedBTS() {
    var myURL = jQuery('script[src$="leaf-demo.js"]').attr('src').replace('leaf-demo.js', '');
    var myIcon = L.icon({
        iconUrl: myURL + 'images/pin24.png',
        iconRetinaUrl: myURL + 'images/pin48.png',
        iconSize: [29, 24],
        iconAnchor: [9, 21],
        popupAnchor: [0, -14]
    });

    var markers = getData();
    //oTable.fnGetData();

    for (var i = 0; i < markers.length; ++i) {
        var popup = '<br/><b>Tỉnh/Tp:</b> ' + markers[i].CityID +
                    '<br/><b>Mã trạm:</b> ' + markers[i].BtsCode +
                    '<br/><b>DN:</b> ' + markers[i].OperatorID +
                    '<br/><b>G.CNKĐ:</b> ' + markers[i].ID +
                    '<br/><b>Địa chỉ:</b> ' + markers[i].Address;

        var m = L.marker([markers[i].Latitude, markers[i].Longtitude], { icon: myIcon })
                        .bindPopup(popup);

        markerClusters.addLayer(m);
    }
}