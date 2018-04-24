var startDate = new Date(new Date().getFullYear(), 0, 1);
var endDate = new Date();

function initDataTable() {
    $('input[name="DateRange"]').daterangepicker(
    {
        locale: {
            format: 'DD/MM/YYYY'
        },
        startDate: startDate,
        endDate: endDate
    },
    function (start, end, label) {
        //alert("A new date range was chosen: " + start.format('DD/MM/YYYY') + ' to ' + end.format('DD/MM/YYYY'));
        startDate = start;
        endDate = end;
    });

    var userRoleAdmin = '@(Request.IsAuthenticated && User.IsInRole("Admin")) ? "true" : "false")';

    if (userRoleAdmin) {
        $("#CertificatedataTable").dataTable({
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'colvis',
                    text: 'Ẩn/hiện cột',
                    className: 'btn-success'
                },
                {
                    extend: 'excel',
                    text: 'Xuất Excel',
                    className: 'btn-success',
                },
                {
                    extend: 'pdf',
                    text: 'Xuất Pdf',
                    className: 'btn-success'
                },
                {
                    extend: 'print',
                    text: 'In ấn',
                    className: 'btn-success'
                }
            ],
            "processing": true,
            "info": true,
            "selector": true,
            "ajax": {
                "async": false,
                "url": "/Certificate/loadCertificate",
                "type": "POST",
                "data": function (d) {
                    d.CityID = $('#CityID').val().trim();
                    d.OperatorID = $('#OperatorID').val().trim();
                    d.ProfileID = $('#ProfileID').val().trim();
                    d.StartDate = startDate.toISOString();
                    d.EndDate = endDate.toISOString();
                    d.BtsCodeOrAddress = $('#BtsCodeOrAddress').val().trim();
                }
            },
            "columns": [
                { "data": "ID", "name": "ID", "width": "20%" },                  // index 0
                { "data": "OperatorID", "name": "OperatorID", "width": "10%" },  // index 1
                { "data": "BtsCode", "name": "BtsCode", "width": "10%" },        // index 2
                { "data": "Address", "name": "Address", "width": "40%" },        // index 3
                { "data": "CityID", "name": "CityID", "width": "4%" },        // index 3
                {
                    "data": "IssuedDate", "name": "IssuedDate", "width": "8%",  // index 4
                    "render": function (data, type, row) {
                        return (moment(row["IssuedDate"]).format("DD/MM/YYYY"));
                    }
                },
                {
                    "data": "ExpiredDate", "name": "ExpiredDate", "width": "8%", // index 5
                    "render": function (data, type, row) {
                        return (moment(row["ExpiredDate"]).format("DD/MM/YYYY"));
                    }
                }]
        });
    } else {
        $("#CertificatedataTable").dataTable({
            "processing": true,
            "info": true,
            "selector": true,
            "ajax": {
                "async": false,
                "url": "/Certificate/loadCertificate",
                "type": "POST",
                "data": function (d) {
                    d.CityID = $('#CityID').val().trim();
                    d.OperatorID = $('#OperatorID').val().trim();
                    d.ProfileID = $('#ProfileID').val().trim();
                    d.StartDate = startDate.toISOString();
                    d.EndDate = endDate.toISOString();
                    d.BtsCodeOrAddress = $('#BtsCodeOrAddress').val().trim();
                }
            },
            "columns": [
                { "data": "ID", "name": "ID", "width": "20%" },                  // index 0
                { "data": "OperatorID", "name": "OperatorID", "width": "10%" },  // index 1
                { "data": "BtsCode", "name": "BtsCode", "width": "10%" },        // index 2
                { "data": "Address", "name": "Address", "width": "40%" },        // index 3
                { "data": "CityID", "name": "CityID", "width": "4%" },        // index 3
                {
                    "data": "IssuedDate", "name": "IssuedDate", "width": "8%",  // index 4
                    "render": function (data, type, row) {
                        return (moment(row["IssuedDate"]).format("DD/MM/YYYY"));
                    }
                },
                {
                    "data": "ExpiredDate", "name": "ExpiredDate", "width": "8%", // index 5
                    "render": function (data, type, row) {
                        return (moment(row["ExpiredDate"]).format("DD/MM/YYYY"));
                    }
                }]
        });
    }
}

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
    var myURL = $('script[src$="leaflet.js"]').attr('src').replace('leaflet.js', '');

    //var markers = getData();
    //Apply custom Search on dataTable here
    var oTable2 = $('#CertificatedataTable').DataTable();
    var markers = oTable2.rows().data();

    for (var i = 0; i < markers.length; ++i) {
        var popup = '<br/><b>Mã trạm:</b> ' + markers[i].BtsCode +
                    '<br/><b>Nhà mạng:</b> ' + markers[i].OperatorID +
                    '<br/><b>G.CNKĐ:</b> ' + markers[i].ID +
                    '<br/><b>Địa chỉ:</b> ' + markers[i].Address;
        var img24 = 'images/pin24.png';
        var img48 = 'images/pin48.png';
        if (markers[i].OperatorID == "VINAPHONE") {
            img24 = 'images/vinaphone24.png';
            img48 = 'images/vinaphone48.png';
        } else if (markers[i].OperatorID == "MOBIFONE") {
            img24 = 'images/mobifone24.png';
            img48 = 'images/mobifone48.png';
        } else if (markers[i].OperatorID == "VIETTEL") {
            img24 = 'images/viettel24.png';
            img48 = 'images/viettel48.png';
        } else if (markers[i].OperatorID == "VNMOBILE") {
            img24 = 'images/vnmobile24.png';
            img48 = 'images/vnmobile48.png';
        }

        var myIcon = L.icon({
            iconUrl: myURL + img24,
            iconRetinaUrl: myURL + img48,
            iconSize: [29, 24],
            iconAnchor: [9, 21],
            popupAnchor: [0, -14]
        });

        var m = L.marker([markers[i].Latitude, markers[i].Longtitude], { icon: myIcon })
                        .bindPopup(popup);
        markerClusters.addLayer(m);
    }
    map.addLayer(markerClusters);

    $("#output").pivotUI($("#input"), {
        rows: ["color"],
        cols: ["shape"]
    });
}

$('#btnSearch').click(function () {
    $('#CertificatedataTable').DataTable().ajax.reload();
    if (map != undefined && map != null && markerClusters != null) {
        map.removeLayer(markerClusters);
        markerClusters.clearLayers();
        //map.eachLayer(function (layer) {
        //    map.removeLayer(layer);
        //});
        loadCertificatedBTS();
    }
    loadCertificatePivotTable();
});

initDataTable();

var table = $('#CertificatedataTable').DataTable();

$('#CertificatedataTable tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    }
    else {
        table.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});

$('#button').click(function () {
    table.row('.selected').remove().draw(false);
});

// See post: http://asmaloney.com/2015/06/code/clustering-markers-on-leaflet-maps
var tiles = L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
    maxZoom: 18,
    attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
}),
    latlng = L.latLng(10.796841, 106.66252);

var map = L.map('mapBTS', { center: latlng, zoom: 8, layers: [tiles] });
var markerClusters = L.markerClusterGroup();

$("a[href='#mapTab']").on('shown.bs.tab', function (e) {
    map.invalidateSize();
});

loadCertificatedBTS();

function loadCertificatePivotTable() {
    var oTable2 = $('#CertificatedataTable').DataTable();
    var pivotTableData = oTable2.rows().data();

    var inputFunction = function (callback) {
        pivotTableData.each(function (element, index) {
            callback({
                OperatorID: element.OperatorID,
                cityID: element.CityID,
                year: moment(element.IssuedDate).year()
            });
        });
    };

    // This example adds Plotly chart renderers.

    var derivers = $.pivotUtilities.derivers;
    var renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers);

    $("#pivotTable").pivotUI(inputFunction,
    {
        renderers: renderers,
        rows: ["OperatorID"],
        cols: ["CityID"],
        rendererName: "Horizontal Stacked Bar Chart",
        rowOrder: "value_a_to_z", colOrder: "value_z_to_a",
    });
}

loadCertificatePivotTable();