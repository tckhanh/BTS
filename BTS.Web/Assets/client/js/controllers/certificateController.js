
var tiles = L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
    maxZoom: 18,
    attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
}),
    latlng = L.latLng(10.796841, 106.66252);
var myMap = L.map('mapBTS', { center: latlng, zoom: 8, layers: [tiles] });
var myMarkerClusters = L.markerClusterGroup();

var certificateController = {
    init: function () {
        certificateController.loadData();
        certificateController.registerEventDataTable();
        certificateController.registerEvent();
    },
    registerEventDataTable: function () {
        var table = $("#MyDataTable").DataTable();
        table.on('draw', function () {
            $('.btn-edit').off('click').on('click', function () {
                $('#modalAddUpdate').modal('show');
                var id = $(this).data('myid');
                certificateController.loadDetail(id);
            });

            $('.btn-delete').off('click').on('click', function () {
                var id = $(this).data('myid');
                bootbox.confirm("Bạn có chắc chắn muốn xóa dữ liệu này không?", function (result) {
                    certificateController.deleteItem(id);
                });
            });
        });
    },
    registerEvent: function () {
        $('#btnSearch').off('click').on('click', function () {
            $('#MyDataTable').DataTable().ajax.reload();
        });

        $("a[href='#mapTab']").on('shown.bs.tab', function (e) {
            myMap.invalidateSize();
        });

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            certificateController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                certificateController.saveData();
            }
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            certificateController.loadData(true);
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
                if (response.status == true) {
                    var data = response.data;
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
                $.notify(err.Message, {
                    className: "error",
                    clickToHide: true
                });
            }
        });
    },

    loadData: function () {
        var startDate = new Date(new Date().getFullYear(), 0, 1);
        var endDate = new Date();
        var userRoleAdmin = '@(Request.IsAuthenticated && User.IsInRole("Admin")) ? "true" : "false")';
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

        if (userRoleAdmin) {
            $("#MyDataTable")
                .on('xhr.dt', function (e, settings, json, xhr) {
                    if (myMap != undefined && myMap != null && myMarkerClusters != null) {
                        myMap.removeLayer(myMarkerClusters);
                        myMarkerClusters.clearLayers();
                        //myMap.eachLayer(function (layer) {
                        //    myMap.removeLayer(layer);
                        //});
                    }
                    certificateController.loadMap(json.data);
                    certificateController.loadPivotTable(json.data);
                })
                .dataTable({
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
                        "async": true,
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
                        }],
                    "language": {
                        url: '/localization/vi_VI.json'
                    }
                });
        } else {
            $("#MyDataTable")
                .on('xhr.dt', function (e, settings, json, xhr) {
                    if (myMap != undefined && myMap != null && myMarkerClusters != null) {
                        myMap.removeLayer(myMarkerClusters);
                        myMarkerClusters.clearLayers();
                        //myMap.eachLayer(function (layer) {
                        //    myMap.removeLayer(layer);
                        //});
                    }
                    if (json != null) {
                        var data = json.data;
                        certificateController.loadMap(data);
                        certificateController.loadPivotTable(data);
                    }

                })
                .dataTable({
                    "processing": true,
                    "info": true,
                    "selector": true,
                    "ajax": {
                        "async": true,
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
                        }],
                    "language": {
                        url: '/localization/vi_VI.json'
                    }
                });
        }
    },
    loadMap: function (markers) {
        var myURL = $('script[src$="leaflet.js"]').attr('src').replace('leaflet.js', '');
        if (markers != null) {
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
                myMarkerClusters.addLayer(m);
            }
            myMap.addLayer(myMarkerClusters);
        }
    },
    loadPivotTable: function (pivotTableData) {
        var inputFunction = function (callback) {
            for (var i = 0; i < pivotTableData.length; ++i){
                callback({
                    OperatorID: pivotTableData[i].OperatorID,
                    cityID: pivotTableData[i].CityID,
                    year: moment(pivotTableData[i].IssuedDate).year()
                });
            }
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
}

certificateController.init();

//$('#MyDataTable tbody').on('click', 'tr', function () {
//    if ($(this).hasClass('selected')) {
//        $(this).removeClass('selected');
//    }
//    else {
//        table.$('tr.selected').removeClass('selected');
//        $(this).addClass('selected');
//    }
//});

//$('#button').click(function () {
//    table.row('.selected').remove().draw(false);
//});

// See post: http://asmaloney.com/2015/06/code/clustering-markers-on-leaflet-maps