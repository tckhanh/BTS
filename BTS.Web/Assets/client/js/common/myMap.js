var myTiles = L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
    maxZoom: 18,
    attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
});
var myLatLng = L.latLng(10.796841, 106.66252);

var myMap = L.map('mapBTS', {layers: [myTiles] });
//var myMap = L.map('mapBTS', { center: myLatLng, zoom: 12, layers: [myTiles] });

if (myMap != undefined && myMap != null) {
    myMap.locate({ setView: true, watch: false }) /* This will return map so you can do chaining */
        .on('locationfound', function (e) {
            var latLon = e.latlng;
            var bounds = latLon.toBounds(4000); // 4000 = metres
            myMap.panTo(latLon).fitBounds(bounds);

            var marker = L.marker(latLon).bindPopup('You are here :)');
            var circle = L.circle(latLon, e.accuracy / 2, {
                weight: 1,
                color: 'blue',
                //fillColor: '#cacaca',
                fillColor: 'blue',
                fillOpacity: 0.1
            });
            myMap.addLayer(marker);
            myMap.addLayer(circle);
        })
        .on('locationerror', function (e) {
            console.log(e);
            alert("Location access denied.");
        });    
}

var myGeocoder = L.Control.Geocoder.nominatim();

if (URLSearchParams && location.search) {
    // parse /?geocoder=nominatim from URL
    var params = new URLSearchParams(location.search);
    var geocoderString = params.get('geocoder');
    if (geocoderString && L.Control.Geocoder[geocoderString]) {
        console.log('Using geocoder', geocoderString);
        geocoder = L.Control.Geocoder[geocoderString]();
    } else if (geocoderString) {
        console.warn('Unsupported geocoder', geocoderString);
    }
}

var myControl = L.Control.geocoder({
    geocoder: myGeocoder
}).addTo(myMap);


//var myGeocoder2 = L.Control.geocoder({
//    defaultMarkGeocode: false
//})
//    .on('markgeocode', function (e) {
//        var bbox = e.geocode.bbox;
//        var poly = L.polygon([
//            bbox.getSouthEast(),
//            bbox.getNorthEast(),
//            bbox.getNorthWest(),
//            bbox.getSouthWest()
//        ]).addTo(myMap);
//        myMap.fitBounds(poly.getBounds());
//    }).addTo(myMap);

//var myMarker;
//myMap.on('click', function (e) {
//    myGeocoder.reverse(e.latlng, myMap.options.crs.scale(myMap.getZoom()), function (results) {
//        var r = results[0];
//        if (r) {
//            if (myMarker) {
//                myMarker
//                    .setLatLng(r.center)
//                    .setPopupContent(r.html || r.name)
//                    .openPopup();
//            } else {
//                myMarker = L.marker(r.center)
//                    .bindPopup(r.name)
//                    .addTo(myMap)
//                    .openPopup();
//            }
//        }
//    });
//});

var myMarkerClusters = L.markerClusterGroup();
