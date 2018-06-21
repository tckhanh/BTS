var tiles = L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
    maxZoom: 18,
    attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
}),
latlng = L.latLng(10.796841, 106.66252);
var myMap = L.map('mapBTS', { center: latlng, zoom: 8, layers: [tiles] });
var myMarkerClusters = L.markerClusterGroup();
var userRoleAdmin = "@(User.IsInRole('System_CanExport') ? 'true' : 'false')";
var fileLocation, fileExtension;
var bar = $('.progress-bar');

var checkController = {
    init: function () {
        checkController.loadData();
        checkController.registerEventDataTable();
        checkController.registerEvent();

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
                $('#btnCheck').prop('disabled', true);
                $('#btnReset').prop('disabled', true);
                $('#FileDialog').prop('disabled', true);                
            },
            beforeSend: function () {
                $('html').addClass('waiting');
                bar.html('Bắt đầu thực hiện!');
                bar.addClass('active');
                $('#progressRow').show();
            },
            uploadProgress: function (event, position, total, percentComplete) {
                if (percentComplete = 100)
                    bar.html('Đã Upload File xong đang thực hiện kiểm tra BTS ....');
                else
                    bar.html('Đang thực hiện Upload File được: ' + percentComplete + '%');
            },
            error: function (data) {
                var r = jQuery.parseJSON(data.responseText);
                alert("Message: " + r.Message);
                alert("StackTrace: " + r.StackTrace);
                alert("ExceptionType: " + r.ExceptionType);
                bar.html('Lỗi trong quá trình thực hiện!');
                $('#btnCheck').prop('disabled', false);
                $('#btnReset').prop('disabled', false);
                $('#FileDialog').prop('disabled', false);
                $('html').removeClass('waiting');
                bar.removeClass('active');
            },
            success: function (responseJSON, statusText, xhr, element) {
                fileLocation = responseJSON.fileLocation;
                fileExtension = responseJSON.fileExtension;
                if (responseJSON.Status == "Success") {
                    bar.html('Đã thực hiện kiểm tra BTS xong!');
                }
                else {
                    bar.html('Lỗi trong quá trình thực hiện!');
                    alert("Complete: " + xhr.responseJSON.Message);
                }
            },
            complete: function (xhr) {
                bar.html('Đang hiển thị kết quả!');
                $('#MyDataTable').DataTable().ajax.reload();
            },
            async: true
        });
    },

    registerEventDataTable: function () {
    },

    registerEvent: function () {
        $('#FileDialog').change(function (sender) {
            var fileName = sender.target.files[0].name;
            var validExts = new Array(".xlsx", ".xls");
            var fileExt = fileName.substring(fileName.lastIndexOf('.'));
            if (validExts.indexOf(fileExt) < 0) {
                alert("Bạn chỉ được các tập tin Excel " + validExts.toString() + " để nhập liệu");
                $("#FileDialog").val('');
                return false;
            }
            else {
                bar.html('');
                return true;
            }
        });
    },

    registerEvent1: function () {
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
        if (userRoleAdmin) {
            $("#MyDataTable")
                .on('xhr.dt', function (e, settings, json, xhr) {
                    if (myMap != undefined && myMap != null && myMarkerClusters != null) {
                        myMap.removeLayer(myMarkerClusters);
                        myMarkerClusters.clearLayers();
                        bar.html('Đã hoàn tất hiển thị kết quả!');
                        $('#btnCheck').prop('disabled', false);
                        $('#btnReset').prop('disabled', false);
                        $('#FileDialog').prop('disabled', false);
                        $('html').removeClass('waiting');
                        bar.removeClass('active');

                        //myMap.eachLayer(function (layer) {
                        //    myMap.removeLayer(layer);
                        //});
                    }
                    //certificateController.loadMap(json.data);
                    //certificateController.loadPivotTable(json.data);
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
                        "url": "/Check/loadBTS",
                        "type": "POST",
                        "data": function (d) {
                            d.fileLocation = fileLocation;
                            d.fileExtension = fileExtension;
                        }
                    },
                    "columns": [
                        { "data": "OperatorID", "name": "OperatorID", "width": "5%" },  // index 1
                        { "data": "BtsCode", "name": "BtsCode", "width": "5%" },        // index 2
                        { "data": "Address", "name": "Address", "width": "30%" },        // index 3
                        { "data": "LastOwnCertificateIDs", "name": "LastOwnCertificateIDs", "width": "20%" },
                        { "data": "LastNoOwnCertificateIDs", "name": "LastNoOwnCertificateIDs", "width": "20%" },
                        { "data": "ProFilesInProcess", "name": "ProFilesInProcess", "width": "10%" },
                        { "data": "ReasonsNoCertificate", "name": "ReasonsNoCertificate", "width": "10%" },
                    ],
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
                        bar.html('Đã hoàn tất hiển thị kết quả!');
                        //myMap.eachLayer(function (layer) {
                        //    myMap.removeLayer(layer);
                        //});
                    }
                    if (json != null) {
                        var data = json.data;
                        //certificateController.loadMap(data);
                        //certificateController.loadPivotTable(data);
                    }
                })
                .dataTable({
                    "processing": true,
                    "info": true,
                    "selector": true,
                    "ajax": {
                        "async": true,
                        "url": "/Check/loadBTS",
                        "type": "POST",
                        "data": function (d) {
                            d.fileLocation = fileLocation;
                            d.fileExtension = fileExtension;
                        }
                    },
                    "columns": [
                        { "data": "OperatorID", "name": "OperatorID", "width": "5%" },  // index 1
                        { "data": "BtsCode", "name": "BtsCode", "width": "5%" },        // index 2
                        { "data": "Address", "name": "Address", "width": "30%" },        // index 3
                        { "data": "LastOwnCertificateIDs", "name": "LastOwnCertificateIDs", "width": "20%" },
                        { "data": "LastNoOwnCertificateIDs", "name": "LastNoOwnCertificateIDs", "width": "20%" },
                        { "data": "ProFilesInProcess", "name": "ProFilesInProcess", "width": "10%" },
                        { "data": "ReasonsNoCertificate", "name": "ReasonsNoCertificate", "width": "10%" },
                    ],
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
                            '<br/><b>G.CNKĐ:</b> ' + markers[i].Id +
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
            for (var i = 0; i < pivotTableData.length; ++i) {
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
checkController.init();