//var data = "";

var noCertificateController = {
    startDate: new Date(new Date().getFullYear(), 0, 1),
    endDate: new Date(),
    token: function () {
        var form = $('#__AjaxAntiForgeryForm');
        return $('input[name="__RequestVerificationToken"]', form).val();
    },

    init: function () {
        noCertificateController.loadData();
        noCertificateController.registerEventDataTable();
        noCertificateController.registerEvent();
    },
    registerEventDataTable: function () {
        var table = $("#MyDataTable").DataTable();
        table.on('draw', function () {
            $('.btn-edit').off('click').on('click', function () {
                $('#modalAddUpdate').modal('show');
                var id = $(this).data('myid');
                noCertificateController.loadDetail(id);
            });

            $('.btn-delete').off('click').on('click', function () {
                var id = $(this).data('myid');
                bootbox.confirm("Bạn có chắc chắn muốn xóa dữ liệu này không?", function (result) {
                    noCertificateController.deleteItem(id);
                });
            });
        });
    },
    registerEvent: function () {

        $('input[name="CertificateStatus]"]').change(function () {
            if ($('input[name="CertificateStatus]"]:checked').val() == myConstant.CertStatus_Valid) {
                $('input[name="DateRange"]').prop("disabled", false);
            } else {
                $('input[name="DateRange"]').prop("disabled", true);
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            $('#MyDataTable').DataTable().ajax.reload();
        });

        $("a[href='#mapTab']").on('shown.bs.tab', function (e) {
            myMap.invalidateSize();
        });

        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            noCertificateController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                noCertificateController.saveData();
            }
        });

        $('#btnReset').off('click').on('click', function () {
            $('#SelOperatorID').val('');
            $('#SelCityID').val('');
            $('#SelProfileID').val('');
            $('#BtsCodeOrAddress').val('');
            noCertificateController.endDate = new Date();
            noCertificateController.startDate = new Date(noCertificateController.endDate.getFullYear() - 5, noCertificateController.endDate.getMonth(), noCertificateController.endDate.getDate());

            $('input[name="DateRange"]').daterangepicker(
                {
                    locale: {
                        format: 'DD/MM/YYYY'
                    },
                    startDate: noCertificateController.startDate,
                    endDate: noCertificateController.endDate
                },
                function (start, end, label) {
                    //alert("A new date range was chosen: " + start.format('DD/MM/YYYY') + ' to ' + end.format('DD/MM/YYYY'));
                    noCertificateController.startDate = start;
                    noCertificateController.endDate = end;
                });
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
        //var startDate = new Date(new Date().getFullYear(), 0, 1);
        //var endDate = new Date();

        //$('input[name="DateRange"]').daterangepicker(
        //    {
        //        locale: {
        //            format: 'DD/MM/YYYY'
        //        },
        //        startDate: startDate,
        //        endDate: endDate
        //    },
        //    function (start, end, label) {
        //        //alert("A new date range was chosen: " + start.format('DD/MM/YYYY') + ' to ' + end.format('DD/MM/YYYY'));
        //        startDate = start;
        //        endDate = end;
        //    });

        $('#SelProfileID').attr('disabled', myNotAuthenticated);

        noCertificateController.startDate = new Date(new Date().getFullYear(), 0, 1);
        noCertificateController.endDate = new Date();

        $('input[name="DateRange"]').daterangepicker(
            {
                locale: {
                    format: 'DD/MM/YYYY'
                },
                startDate: noCertificateController.startDate,
                endDate: noCertificateController.endDate
            },
            function (start, end, label) {
                //alert("A new date range was chosen: " + start.format('DD/MM/YYYY') + ' to ' + end.format('DD/MM/YYYY'));
                noCertificateController.startDate = start;
                noCertificateController.endDate = end;
            });

        //$.ajax({
        //    url: 'NoCertificate/GetUserRoles',
        //    dataType: 'json',
        //    data: {},
        //    type: 'post',
        //    success: function (data) {
        //        userRoleAdmin = data.Roles;
        //        __RequestVerificationToken = btsController.token();
        //    }
        //});

        if ($.inArray(myConstant.System_CanExport_Role, myArrayRoles) > -1) {
            $("#MyDataTable")
                .on('draw.dt', function (e, settings, json, xhr) {
                    noCertificateController.initCompleteFunction(settings, json);
                })
                .on('xhr.dt', function (e, settings, json, xhr) {
                    //new $.fn.dataTable.Api(settings).one('draw', function () {
                    //    noCertificateController.initCompleteFunction(settings, json);
                    //});
                    new $.fn.dataTable.Api(settings).one('draw', function () {
                        noCertificateController.initCompleteFunction(settings, json);
                    });

                    if (myMap != undefined && myMap != null && myMarkerClusters != null) {
                        myMap.removeLayer(myMarkerClusters);
                        myMarkerClusters.clearLayers();
                        //myMap.eachLayer(function (layer) {
                        //    myMap.removeLayer(layer);
                        //});
                    }

                    if (json != null) {
                        if ($.inArray(myConstant.Info_CanViewMap_Role, myArrayRoles) > -1)
                            noCertificateController.loadMap(json.data);
                        if ($.inArray(myConstant.Info_CanViewStatitics_Role, myArrayRoles) > -1)
                            noCertificateController.loadPivotTable(json.data);
                    }
                })
                .dataTable({
                    createdRow: function (row, data, dataIndex) {
                        $(row).attr('data-id', data.Id);
                        $(row).attr('data-long', data.Longtitude);
                        $(row).attr('data-lat', data.Latitude);
                        $(row).addClass('mainRow');
                    },
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
                    //"scrollX": true, // ảnh hưởng đến DataTable Id
                    "selector": true,
                    "ajax": {
                        "async": true,
                        "url": "/NoCertificate/loadNoCertificate",
                        "type": "POST",
                        "data": function (d) {
                            d.CityID = $('#SelCityID').val().trim();
                            d.OperatorID = $('#SelOperatorID').val().trim();
                            d.ProfileID = $('#SelProfileID').val().trim();
                            d.StartDate = noCertificateController.startDate.toISOString();
                            d.EndDate = noCertificateController.endDate.toISOString();
                            d.BtsCodeOrAddress = $('#BtsCodeOrAddress').val().trim();
                            d.CertificateStatus = $('input[name=CertificateStatus]:checked').val();
                            d.__RequestVerificationToken = noCertificateController.token();
                        }
                    },
                    "columns": [
                        { "data": "OperatorID", "name": "OperatorID", "width": "6%" },
                        { "data": "CityID", "name": "CityID", "width": "4%", "className": "dt-body-center"},
                        {
                            "data": "BtsCode", "name": "BtsCode", "width": "10%",
                            //fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            //    $(nTd).html("<a href='/Bts/Detail/" + oData.BtsCode + "'>" + oData.BtsCode + "</a>");
                            //}
                        },        // index 2
                        { "data": "Address", "name": "Address", "width": "25%" },
                        {
                            "data": "LabID", "name": "LabID", "width": "8%", "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<a href='/Lab/Detail/" + oData.LabID + "'>" + oData.LabID + "</a>");
                            }
                        },
                        {
                            "data": "TestReportDate", "name": "TestReportDate", "width": "8%", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                return (moment(row["TestReportDate"]).format("DD/MM/YYYY"));
                            }
                        },
                        {
                            "data": "ReasonNoCertificate", "name": "ReasonNoCertificate", "width": "25%"
                        },
                        {
                            "data": "Id", "name": "Id", "width": "14%", "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                var htmlLink = "";
                                if ($.inArray(myConstant.Data_CanViewDetail_Role, myArrayRoles) > -1) {                                    
                                    htmlLink += '<a class="btn btn-info btn-sm" onclick="addinController.Detail(\'/NoCertificate/Detail/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Chi tiết"><i class="fa fa-address-card fa-lg"></i></a>';
                                }
                                if ($.inArray(myConstant.Data_CanEdit_Role, myArrayRoles) > -1) {
                                    htmlLink += ' <a class="btn btn-primary btn-sm" onclick="addinController.Edit(\'/NoCertificate/Edit/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Sửa"><i class="fa fa-pencil fa-lg"></i></a>';
                                }
                                if ($.inArray(myConstant.Data_CanDelete_Role, myArrayRoles) > -1) {
                                    htmlLink += ' <a class="btn btn-danger btn-sm" onclick="addinController.Delete(\'/NoCertificate/Delete/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Xóa"><i class="fa fa-trash fa-lg"></i></a>';                                    
                                }
                                $(nTd).html(htmlLink);
                            }
                        }                        
                    ],
                    "language": {
                        url: '/AppFiles/localization/vi_VI.json'
                    },
                    "initComplete": function () {
                    }
                });
            var t = $('#MyDataTable').DataTable();
            t.on('preXhr.dt', function (e, settings, data) {
                $('html').addClass('waiting');
            });
            t.on('xhr.dt', function (e, settings, json, xhr) {
                $('html').removeClass('waiting');
            });
            //t.on('order.dt search.dt', function () {
            //    t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            //        cell.innerHTML = i + 1;
            //    });
            //}).draw();
        } else {
            $("#MyDataTable")
                .on('draw.dt', function (e, settings, json, xhr) {
                    noCertificateController.initCompleteFunction(settings, json);
                })
                .on('xhr.dt', function (e, settings, json, xhr) {
                    //new $.fn.dataTable.Api(settings).one('draw', function () {
                    //    noCertificateController.initCompleteFunction(settings, json);
                    //});
                    if (myMap != undefined && myMap != null && myMarkerClusters != null) {
                        myMap.removeLayer(myMarkerClusters);
                        myMarkerClusters.clearLayers();
                        //myMap.eachLayer(function (layer) {
                        //    myMap.removeLayer(layer);
                        //});
                    }

                    if (json != null) {
                        if ($.inArray(myConstant.Info_CanViewMap_Role, myArrayRoles) > -1)
                            noCertificateController.loadMap(json.data);
                        if ($.inArray(myConstant.Info_CanViewStatitics_Role, myArrayRoles) > -1)
                            noCertificateController.loadPivotTable(json.data);
                    }
                })
                .dataTable({
                    createdRow: function (row, data, dataIndex) {
                        $(row).attr('data-id', data.Id);
                        $(row).attr('data-long', data.Longtitude);
                        $(row).attr('data-lat', data.Latitude);
                        $(row).addClass('mainRow');
                    },
                    "processing": true,
                    "info": true,
                    "selector": true,
                    //"scrollX": true, // ảnh hưởng đến DataTable Id
                    "ajax": {
                        "async": true,
                        "url": "/NoCertificate/loadNoCertificate",
                        "type": "POST",
                        "data": function (d) {
                            d.CityID = $('#SelCityID').val().trim();
                            d.OperatorID = $('#SelOperatorID').val().trim();
                            d.ProfileID = $('#SelProfileID').val().trim();
                            d.StartDate = noCertificateController.startDate.toISOString();
                            d.EndDate = noCertificateController.endDate.toISOString();
                            d.BtsCodeOrAddress = $('#BtsCodeOrAddress').val().trim();
                            d.CertificateStatus = $('input[name=CertificateStatus]:checked').val();
                            d.__RequestVerificationToken = noCertificateController.token();
                        }
                    },
                    "columns": [
                        { "data": "OperatorID", "name": "OperatorID", "width": "6%" },
                        { "data": "CityID", "name": "CityID", "width": "4%", "className": "dt-body-center" },
                        {
                            "data": "BtsCode", "name": "BtsCode", "width": "10%",
                            //fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            //    $(nTd).html("<a href='/Bts/Detail/" + oData.BtsCode + "'>" + oData.BtsCode + "</a>");
                            //}
                        },        // index 2
                        { "data": "Address", "name": "Address", "width": "25%" },
                        {
                            "data": "LabID", "name": "LabID", "width": "8%", "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<a href='/Lab/Detail/" + oData.LabID + "'>" + oData.LabID + "</a>");
                            }
                        },
                        {
                            "data": "TestReportDate", "name": "TestReportDate", "width": "8%", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                return (moment(row["TestReportDate"]).format("DD/MM/YYYY"));
                            }
                        },
                        {
                            "data": "ReasonNoCertificate", "name": "ReasonNoCertificate", "width": "25%",
                        },
                        {
                            "data": "Id", "name": "Id", "width": "14%", "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                var htmlLink = "";
                                if ($.inArray(myConstant.Data_CanViewDetail_Role, myArrayRoles) > -1) {
                                    htmlLink += '<a class="btn btn-info btn-sm" onclick="addinController.Detail(\'/NoCertificate/Detail/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Chi tiết"><i class="fa fa-address-card fa-lg"></i></a>';
                                }
                                if ($.inArray(myConstant.Data_CanEdit_Role, myArrayRoles) > -1) {
                                    htmlLink += ' <a class="btn btn-primary btn-sm" onclick="addinController.Edit(\'/NoCertificate/Edit/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Sửa"><i class="fa fa-pencil fa-lg"></i></a>';
                                }
                                if ($.inArray(myConstant.Data_CanDelete_Role, myArrayRoles) > -1) {
                                    htmlLink += ' <a class="btn btn-danger btn-sm" onclick="addinController.Delete(\'/NoCertificate/Delete/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Xóa"><i class="fa fa-trash fa-lg"></i></a>';                                    
                                }
                                $(nTd).html(htmlLink);
                            }
                        }
                    ],
                    "language": {
                        url: '/AppFiles/localization/vi_VI.json'
                    },
                    initComplete: function () {
                    }
                });
            var t = $('#MyDataTable').DataTable();
            t.on('preXhr.dt', function (e, settings, data) {
                $('html').addClass('waiting');
            });
            t.on('xhr.dt', function (e, settings, json, xhr) {
                $('html').removeClass('waiting');
            });
            //t.on('order.dt search.dt', function () {
            //    t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            //        cell.innerHTML = i + 1;
            //    });
            //}).draw();
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
                    '<br/><b>Địa chỉ:</b> ' + markers[i].Address +
                    '<br/><b>Lý do Không cấp:</b> ' + markers[i].ReasonNoCertificate;
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
                    "Year": moment(pivotTableData[i].TestReportDate).year()
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
                rendererName: "Bar Chart",
                rowOrder: "value_a_to_z", colOrder: "value_z_to_a",
                hiddenAttributes: ["select.pvtRenderer", "renderers"]
            }, true);
    }
}

noCertificateController.init();

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