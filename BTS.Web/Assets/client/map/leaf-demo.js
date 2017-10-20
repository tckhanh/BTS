// See post: http://asmaloney.com/2015/06/code/clustering-markers-on-leaflet-maps

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

var markers = getData();


var map = L.map( 'map', {
  center: [10.0, 5.0],
  minZoom: 2,
  zoom: 2
});

L.tileLayer( 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
 attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
 subdomains: ['a','b','c']
}).addTo( map );

var myURL = jQuery( 'script[src$="leaf-demo.js"]' ).attr( 'src' ).replace( 'leaf-demo.js', '' );

var myIcon = L.icon({
  iconUrl: myURL + 'images/pin24.png',
  iconRetinaUrl: myURL + 'images/pin48.png',
  iconSize: [29, 24],
  iconAnchor: [9, 21],
  popupAnchor: [0, -14]
});

var markerClusters = L.markerClusterGroup();

for ( var i = 0; i < markers.length; ++i )
{
    var popup = markers[i].name +
                '<br/>' + markers[i].Address +
                '<br/><b>CertNo:</b> ' + markers[i].CertificateNum +
                '<br/><b>DN:</b> ' + markers[i].OperatorID +
                '<br/><b>City:</b> ' + markers[i].CityID;

  var m = L.marker([markers[i].Latitude, markers[i].Longtitude], { icon: myIcon })
                  .bindPopup( popup );

  markerClusters.addLayer( m );
}

map.addLayer( markerClusters );




