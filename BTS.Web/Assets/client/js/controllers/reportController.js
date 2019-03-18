﻿$(document).ready(function () {
    var tiles = L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
        maxZoom: 18,
        attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
    }),
        latlng = L.latLng(10.796841, 106.66252);
    var myMap = L.map('mapBTS', { center: latlng, zoom: 8, layers: [tiles] });
    var myMarkerClusters = L.markerClusterGroup();
    var userRoleAdmin = "@(User.IsInRole('System_CanExport') ? 'true' : 'false')";
    var data = "";
    var currDate = new Date();
    var month = currDate.getMonth();
    var year = currDate.getFullYear();
    var startDate = new Date(year, month, 1);
    var operator_GTEL = "GTEL";
    var operator_VNMOBILE = "VNMOBILE";
    var operator_VIETTEL = "VIETTEL";
    var operator_MOBIFONE = "MOBIFONE";
    var operator_VINAPHONE = "VINAPHONE";

    var reportController = {
        init: function () {
            reportController.loadData();
            reportController.registerEventDataTable();
            reportController.registerEvent();
        },
        registerEventDataTable: function () {
            var table = $("#MyDataTable").DataTable();
            table.on('draw', function () {
                $('.btn-edit').off('click').on('click', function () {
                    $('#modalAddUpdate').modal('show');
                    var id = $(this).data('myid');
                    reportController.loadDetail(id);
                });

                $('.btn-delete').off('click').on('click', function () {
                    var id = $(this).data('myid');
                    bootbox.confirm("Bạn có chắc chắn muốn xóa dữ liệu này không?", function (result) {
                        reportController.deleteItem(id);
                    });
                });
            });
        },
        registerEvent: function () {
            $('#btnSearch').off('click').on('click', function () {
                $("#BTS-Result").html(" " + (parseInt(month) + 1) + "/" + year);
                $('#MyDataTable').DataTable().ajax.reload();
            });

            $("a[href='#mapTab']").on('shown.bs.tab', function (e) {
                myMap.invalidateSize();
            });

            $('#btnAddNew').off('click').on('click', function () {
                $('#modalAddUpdate').modal('show');
                reportController.resetForm();
            });

            $('#btnSave').off('click').on('click', function () {
                if ($('#frmSaveData').valid()) {
                    reportController.saveData();
                }
            });

            $('#btnReset').off('click').on('click', function () {
                $('#txtNameS').val('');
                $('#ddlStatusS').val('');
                reportController.loadData(true);
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
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Success") {
                        data = response.data;
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
                    $.notify(err.message, {
                        className: "error",
                        clickToHide: true
                    });
                }
            });
        },

        loadData: function () {
            $.datepicker.setDefaults($.datepicker.regional['vi']);
            $("#DateRange").datepicker(
                        {
                            dateFormat: "mm/yy",
                            changeMonth: true,
                            changeYear: true,
                            showButtonPanel: true,
                            onClose: function (dateText, inst) {
                                function isDonePressed() {
                                    return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                                }

                                if (isDonePressed()) {
                                    month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                                    year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                                    $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');

                                    $('.date-picker').focusout()//Added to remove focus from datepicker input box on selecting date
                                }
                            },
                            beforeShow: function (input, inst) {
                                inst.dpDiv.addClass('month_year_datepicker')

                                if ((datestr = $(this).val()).length > 0) {
                                    year = datestr.substring(datestr.length - 4, datestr.length);
                                    month = datestr.substring(0, 2);
                                    $(this).datepicker('option', 'defaultDate', new Date(year, month - 1, 1));
                                    $(this).datepicker('setDate', new Date(year, month - 1, 1));
                                    $(".ui-datepicker-calendar").hide();
                                }
                            }
                        });
            $("#DateRange").datepicker("setDate", currDate);
            $("#BTS-Result").html(" " + (parseInt(month) + 1) + "/" + year);

            //$.ajax({
            //    url: 'Certificate/GetUserRoles',
            //    dataType: 'json',
            //    data: {},
            //    type: 'post',
            //    success: function (data) {
            //        userRoleAdmin = data.Roles;
            //    }
            //});

            if (userRoleAdmin) {
                $("#MyDataTable")
                    .on('draw.dt', function (e, settings, json, xhr) {
                        reportController.initCompleteFunction(settings, json);
                    })
                    .on('xhr.dt', function (e, settings, json, xhr) {
                        //new $.fn.dataTable.Api(settings).one('draw', function () {
                        //    reportController.initCompleteFunction(settings, json);
                        //});
                        new $.fn.dataTable.Api(settings).one('draw', function () {
                            reportController.initCompleteFunction(settings, json);
                        });

                        if (myMap != undefined && myMap != null && myMarkerClusters != null) {
                            myMap.removeLayer(myMarkerClusters);
                            myMarkerClusters.clearLayers();
                            //myMap.eachLayer(function (layer) {
                            //    myMap.removeLayer(layer);
                            //});
                        }
                        reportController.loadMap(json.data);
                        reportController.loadPivotTable(json.data);
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
                        "paging": false,
                        "info": true,
                        "selector": true,
                        "ajax": {
                            "async": true,
                            "url": "/Report/loadCertificate",
                            "type": "POST",
                            "data": function (d) {
                                d.Month = month;
                                d.Year = year;
                            }
                        },
                        "columns": [
                            {
                                "data": null,
                            },
                            {
                                "data": "Id", "name": "Id",
                                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                    $(nTd).html("<a href='/Certificate/Detail/" + oData.Id + "'>" + oData.Id + "</a>");
                                }
                            },
                            { "data": "OperatorID", "name": "OperatorID" },
                            {
                                "data": "SubBtsOperatorIDs",
                                "render": function (data, type, row) {
                                    return data.includes(operator_GTEL) ? "GTEL" : "";
                                }
                            },
                            {
                                "data": "OperatorID",
                                "render": function (data, type, row) {
                                    return data.includes(operator_VNMOBILE) ? "HNT" : "";
                                }
                            },
                            {
                                "data": "OperatorID",
                                "render": function (data, type, row) {
                                    return data.includes(operator_VIETTEL) ? "VTEL" : "";
                                }
                            },
                            {
                                "data": "OperatorID",
                                "render": function (data, type, row) {
                                    return data.includes(operator_MOBIFONE) ? "VMS" : "";
                                }
                            },
                            {
                                "data": "OperatorID",
                                "render": function (data, type, row) {
                                    return data.includes(operator_VINAPHONE) ? "VNP" : "";
                                }
                            },
                            {
                                "data": "BtsCode", "name": "BtsCode",
                                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                    $(nTd).html("<a href='/Bts/Detail/" + oData.BtsCode + "'>" + oData.BtsCode + "</a>");
                                }
                            },
                            { "data": "Address", "name": "Address" },
                            { "data": "CityID", "name": "CityID" },
                            {
                                "data": "IssuedDate", "name": "IssuedDate",
                                "render": function (data, type, row) {
                                    return (moment(row["IssuedDate"]).format("DD/MM/YYYY"));
                                }
                            },
                            {
                                "data": "ExpiredDate", "name": "ExpiredDate",
                                "render": function (data, type, row) {
                                    return (moment(row["ExpiredDate"]).format("DD/MM/YYYY"));
                                }
                            }],
                        //rowsGroup: ['OperatorID:name', 1],
                        "language": {
                            url: '/AppFiles/localization/vi_VI.json'
                        },
                        "initComplete": function () {
                        }
                    });

                var t = $('#MyDataTable').DataTable();
                t.on('order.dt search.dt', function () {
                    t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                        cell.innerHTML = i + 1;
                    });
                }).draw();

            } else {
                $("#MyDataTable")
                    .on('draw.dt', function (e, settings, json, xhr) {
                        reportController.initCompleteFunction(settings, json);
                    })
                    .on('xhr.dt', function (e, settings, json, xhr) {
                        //new $.fn.dataTable.Api(settings).one('draw', function () {
                        //    reportController.initCompleteFunction(settings, json);
                        //});
                        if (myMap != undefined && myMap != null && myMarkerClusters != null) {
                            myMap.removeLayer(myMarkerClusters);
                            myMarkerClusters.clearLayers();
                            //myMap.eachLayer(function (layer) {
                            //    myMap.removeLayer(layer);
                            //});
                        }
                        if (json != null) {
                            var data = json.data;
                            reportController.loadMap(data);
                            reportController.loadPivotTable(data);
                        }
                    })
                    .dataTable({
                        "processing": true,
                        "info": true,
                        "selector": true,
                        "ajax": {
                            "async": true,
                            "url": "/Report/loadCertificate",
                            "type": "POST",
                            "data": function (d) {
                                d.Month = month;
                                d.Year = year;
                            }
                        },
                        "columns": [
                            {
                                "data": null,
                            },
                            {
                                "data": "Id", "name": "Id",
                                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                    $(nTd).html("<a href='/Certificate/Detail/" + oData.Id + "'>" + oData.Id + "</a>");
                                }
                            },
                            { "data": "OperatorID", "name": "OperatorID" },
                            {
                                "data": "SubBtsOperatorIDs",
                                "render": function (data, type, row) {
                                    return data.includes(operator_GTEL) ? "GTEL" : "";
                                }
                            },
                            {
                                "data": "OperatorID",
                                "render": function (data, type, row) {
                                    return data.includes(operator_VNMOBILE) ? "HNT" : "";
                                }
                            },
                            {
                                "data": "OperatorID",
                                "render": function (data, type, row) {
                                    return data.includes(operator_VIETTEL) ? "VTEL" : "";
                                }
                            },
                            {
                                "data": "OperatorID",
                                "render": function (data, type, row) {
                                    return data.includes(operator_MOBIFONE) ? "VMS" : "";
                                }
                            },
                            {
                                "data": "OperatorID",
                                "render": function (data, type, row) {
                                    return data.includes(operator_VINAPHONE) ? "VNP" : "";
                                }
                            },
                            {
                                "data": "BtsCode", "name": "BtsCode",
                                fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                    $(nTd).html("<a href='/Bts/Detail/" + oData.BtsCode + "'>" + oData.BtsCode + "</a>");
                                }
                            },
                            { "data": "Address", "name": "Address" },
                            { "data": "CityID", "name": "CityID" },
                            {
                                "data": "IssuedDate", "name": "IssuedDate",
                                "render": function (data, type, row) {
                                    return (moment(row["IssuedDate"]).format("DD/MM/YYYY"));
                                }
                            },
                            {
                                "data": "ExpiredDate", "name": "ExpiredDate",
                                "render": function (data, type, row) {
                                    return (moment(row["ExpiredDate"]).format("DD/MM/YYYY"));
                                }
                            }],
                        rowsGroup: ['OperatorID:name', 1],
                        "language": {
                            url: '/AppFiles/localization/vi_VI.json'
                        },
                        initComplete: function () {
                        }
                    });
                var t = $('#MyDataTable').DataTable();
                t.on('order.dt search.dt', function () {
                    t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                        cell.innerHTML = i + 1;
                    });
                }).draw();
            }
        },
        initCompleteFunction: function (settings, json) {
            var api = new $.fn.dataTable.Api(settings);
            api.columns().every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );

                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });

                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
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
                        "OperatorID": pivotTableData[i].OperatorID,
                        "CityID": pivotTableData[i].CityID,
                        "LabID": pivotTableData[i].LabID,
                        "Year": moment(pivotTableData[i].IssuedDate).year()
                    });
                }
            };

            // This example adds Plotly chart renderers.

            var derivers = $.pivotUtilities.derivers;
            var renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers, $.pivotUtilities.c3_renderers);
            $("#pivotTable").pivotUI(inputFunction,
                        {
                            renderers: renderers,
                            rows: ["OperatorID"],
                            cols: ["CityID"],
                            rendererName: "Horizontal Stacked Bar Chart",
                            rowOrder: "value_a_to_z", colOrder: "value_z_to_a",
                            hiddenAttributes: ["select.pvtRenderer", "renderers"]
                        }, true);
        }
    }

    reportController.init();

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
});