﻿
//var data = "";

var inspectController = {
    startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 1),
    endDate: new Date(),
    token: function () {
        var form = $('#__AjaxAntiForgeryForm');
        return $('input[name="__RequestVerificationToken"]', form).val();
    },

    init: function () {
        inspectController.loadData();
        inspectController.registerEventDataTable();
        inspectController.registerEvent();
    },
    registerEventDataTable: function () {
        var table = $("#MyDataTable").DataTable();
        table.on('draw', function () {
            $('.btn-edit').off('click').on('click', function () {
                $('#modalAddUpdate').modal('show');
                var id = $(this).data('myid');
                inspectController.loadDetail(id);
            });

            $('.btn-delete').off('click').on('click', function () {
                var id = $(this).data('myid');
                bootbox.confirm("Bạn có chắc chắn muốn xóa dữ liệu này không?", function (result) {
                    inspectController.deleteItem(id);
                });
            });
        });

        $('#MyDataTable tbody').on('click', 'td.details-control', function () {
            var tr = $(this).closest('tr');
            var row = table.row(tr);

            if (row.child.isShown()) {
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                row.child(inspectController.format(row.data())).show();
                tr.addClass('shown');
            }
        });

        $("#MyDataTable").contextmenu({
            delegate: "tbody > tr",
            menu: [
                { title: "Copy", cmd: "copy", uiIcon: "ui-icon-copy" },
                { title: "----" },
                {
                    title: "More", children: [
                        { title: "Sub 1", cmd: "sub1" },
                        { title: "Sub 2", cmd: "sub1" }
                    ]
                }
            ],
            select: function (event, ui) {
                alert("select " + ui.cmd + " on " + ui.target.text());
            }
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
            inspectController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                inspectController.saveData();
            }
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            inspectController.loadData(true);
        });
    },

    format: function (rowData) {
        var div = $('<div/>')
            .addClass('loading')
            .text('Loading...');

        $.ajax({
            url: '/Certificate/SubDetail',
            type: "GET",
            data: {
                Id: rowData.Id,
            },
            dataType: 'json',
            success: function (json) {
                div
                    .html(json.html)
                    .removeClass('loading');
            }
        });

        return div;
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
        inspectController.startDate = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        inspectController.endDate = new Date();

        $('input[name="DateRange"]').daterangepicker(
            {
                locale: {
                    format: 'DD/MM/YYYY'
                },
                startDate: inspectController.startDate,
                endDate: inspectController.endDate
            },
            function (start, end, label) {
                //alert("A new date range was chosen: " + start.format('DD/MM/YYYY') + ' to ' + end.format('DD/MM/YYYY'));
                inspectController.startDate = start;
                inspectController.endDate = end;
            });

        //$.ajax({
        //    url: 'Certificate/GetUserRoles',
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
                    inspectController.initCompleteFunction(settings, json);
                })
                .on('xhr.dt', function (e, settings, json, xhr) {
                    //new $.fn.dataTable.Api(settings).one('draw', function () {
                    //    inspectController.initCompleteFunction(settings, json);
                    //});
                    new $.fn.dataTable.Api(settings).one('draw', function () {
                        inspectController.initCompleteFunction(settings, json);
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
                            inspectController.loadMap(json.data);
                        if ($.inArray(myConstant.Info_CanViewStatitics_Role, myArrayRoles) > -1)
                            inspectController.loadPivotTable(json.data);
                    }
                })
                .dataTable({
                    order: [[3, "desc"]],
                    columnDefs: [{
                        orderable: false,
                        className: 'select-checkbox',
                        targets: 0
                    }],
                    select: {
                        style: 'multi',
                        selector: 'td:first-child'
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
                    "paging": false,
                    "info": true,
                    //"scrollX": true, // ảnh hưởng đến DataTable Id
                    "selector": true,
                    "ajax": {
                        "async": true,
                        "url": "/Inspect/loadCertificate",
                        "type": "POST",
                        "data": function (d) {
                            d.CertNunStart = $('#CertNunStart').val();
                            d.CertNunEnd = $('#CertNunEnd').val();
                            d.StartDate = inspectController.startDate.toISOString();
                            d.EndDate = inspectController.endDate.toISOString();
                            d.__RequestVerificationToken = inspectController.token();
                        }
                    },
                    "columns": [
                        {
                            className: 'details-control',
                            orderable: false,
                            data: null,
                            defaultContent: '',
                            width: "3%"
                        },
                        { "data": null },
                        {
                            "data": "ProfileID", "name": "ProfileID", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                return "";
                            }
                        },
                        {
                            "data": "BtsCode", "name": "BtsCode",
                            //fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            //    $(nTd).html("<a href='/Bts/Detail/" + oData.BtsCode + "'>" + oData.BtsCode + "</a>");
                            //}
                        },        // index 2
                        { "data": "Address", "name": "Address" },                        
                        { "data": "TestReportNo", "name": "TestReportNo" },
                        {
                            "data": "IsMeasuringExposure", "name": "IsMeasuringExposure", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                if (row["IsMeasuringExposure"] == true)
                                    return "Có";
                                else
                                    return "Không";
                            }
                        },
                        {
                            "data": "IsSafeLimit", "name": "IsSafeLimit", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                if (row["IsSafeLimit"] == true)
                                    return "Có";
                                else
                                    return "Không";
                            }
                        },
                        { "data": "", "name": "SharedOperators", 
                            "render": function (data, type, row) {
                                return "";
                            }
                        },
                        {
                            "data": "Id", "name": "Id", "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                if ($.inArray(myConstant.Info_CanPrintCertificate_Role, myArrayRoles) > -1) {
                                    $(nTd).html("<a target='_blank' href='/PrintCertificate/Index/" + oData.Id + "'>" + oData.Id + "</a>");
                                } else {
                                    $(nTd).html(oData.Id);
                                }
                            }
                        },
                                                
                        //{
                        //    "data": "Id", "name": "Id", "width": "14%",  "className": "dt-body-center",
                        //    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        //        var htmlLink = "";
                        //        if ($.inArray(myConstant.Data_CanViewDetail_Role, myArrayRoles) > -1) {
                        //            htmlLink += '<a class="btn btn-info btn-sm" onclick="addinController.Detail(\'/Certificate/AddOrEdit/' + oData.Id + '?act=Detail\')" data-toggle="tooltip" data-placement="top" title="Chi tiết"><i class="fa fa-address-card fa-lg"></i></a>';
                        //        }
                        //        if ($.inArray(myConstant.Data_CanEdit_Role, myArrayRoles) > -1) {
                        //            htmlLink += ' <a class="btn btn-primary btn-sm" onclick="addinController.Edit(\'/Certificate/AddOrEdit/' + oData.Id + '?act=Edit\')" data-toggle="tooltip" data-placement="top" title="Sửa"><i class="fa fa-pencil fa-lg"></i></a>';
                        //        }
                        //        if ($.inArray(myConstant.Data_CanDelete_Role, myArrayRoles) > -1) {
                        //            htmlLink += ' <a class="btn btn-danger btn-sm" onclick="addinController.Delete(\'/Certificate/Delete/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Xóa"><i class="fa fa-trash fa-lg"></i></a>';
                        //        }
                        //        $(nTd).html(htmlLink);
                        //    }
                        //}
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
            t.on('order.dt search.dt', function () {
                t.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        } else {
            $("#MyDataTable")
                .on('draw.dt', function (e, settings, json, xhr) {
                    inspectController.initCompleteFunction(settings, json);
                })
                .on('xhr.dt', function (e, settings, json, xhr) {
                    //new $.fn.dataTable.Api(settings).one('draw', function () {
                    //    inspectController.initCompleteFunction(settings, json);
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
                            inspectController.loadMap(json.data);
                        if ($.inArray(myConstant.Info_CanViewStatitics_Role, myArrayRoles) > -1)
                            inspectController.loadPivotTable(json.data);
                    }
                })
                .dataTable({
                    order: [[3, "desc"]],
                    columnDefs: [{
                        orderable: false,
                        className: 'select-checkbox',
                        targets: 0
                    }],
                    select: {
                        style: 'os',
                        selector: 'td:first-child'
                    },
                    "processing": true,
                    "paging": false,
                    "info": true,
                    //"scrollX": true, // ảnh hưởng đến DataTable Id
                    "selector": true,
                    "ajax": {
                        "async": true,
                        "url": "/Inspect/loadCertificate",
                        "type": "POST",
                        "data": function (d) {
                            d.CertNunStart = $('#CertNunStart').val().trim();
                            d.CertNunEnd = $('#CertNunEnd').val().trim();
                            d.StartDate = inspectController.startDate.toISOString();
                            d.EndDate = inspectController.endDate.toISOString();
                            d.__RequestVerificationToken = inspectController.token();
                        }
                    },
                    "columns": [
                        {
                            className: 'details-control',
                            orderable: false,
                            data: null,
                            defaultContent: '',
                            width: "3%"
                        },
                        { "data": null },
                        {
                            "data": "ProfileID", "name": "ProfileID", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                return " ";
                            }
                        },
                        {
                            "data": "BtsCode", "name": "BtsCode",
                            //fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            //    $(nTd).html("<a href='/Bts/Detail/" + oData.BtsCode + "'>" + oData.BtsCode + "</a>");
                            //}
                        },        // index 2
                        { "data": "Address", "name": "Address" },
                        { "data": "TestReportNo", "name": "TestReportNo" },
                        {
                            "data": "IsMeasuringExposure", "name": "IsMeasuringExposure", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                if (row["IsMeasuringExposure"]== true)
                                    return "Có";
                                else
                                    return "Không";
                            }
                        },
                        {
                            "data": "IsSafeLimit", "name": "IsSafeLimit", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                if (row["IsSafeLimit"] == true)
                                    return "Có";
                                else
                                    return "Không";
                            }
                        },
                        {
                            "data": "", "name": "SharedOperators",
                            "render": function (data, type, row) {
                                return "";
                            }
                        },
                        {
                            "data": "Id", "name": "Id", "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                if ($.inArray(myConstant.Info_CanPrintCertificate_Role, myArrayRoles) > -1) {
                                    $(nTd).html("<a target='_blank' href='/PrintCertificate/Index/" + oData.Id + "'>" + oData.Id + "</a>");
                                } else {
                                    $(nTd).html(oData.Id);
                                }
                            }
                        },
                        
                        //{
                        //    "data": "Id", "name": "Id", "width": "14%",  "className": "dt-body-center",
                        //    fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                        //        var htmlLink = "";
                        //        if ($.inArray(myConstant.Data_CanViewDetail_Role, myArrayRoles) > -1) {
                        //            htmlLink += '<a class="btn btn-info btn-sm" onclick="addinController.Detail(\'/Certificate/AddOrEdit/' + oData.Id + '?act=Detail\')" data-toggle="tooltip" data-placement="top" title="Chi tiết"><i class="fa fa-address-card fa-lg"></i></a>';
                        //        }
                        //        if ($.inArray(myConstant.Data_CanEdit_Role, myArrayRoles) > -1) {
                        //            htmlLink += ' <a class="btn btn-primary btn-sm" onclick="addinController.Edit(\'/Certificate/AddOrEdit/' + oData.Id + '?act=Edit\')" data-toggle="tooltip" data-placement="top" title="Sửa"><i class="fa fa-pencil fa-lg"></i></a>';
                        //        }
                        //        if ($.inArray(myConstant.Data_CanDelete_Role, myArrayRoles) > -1) {
                        //            htmlLink += ' <a class="btn btn-danger btn-sm" onclick="addinController.Delete(\'/Certificate/Delete/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Xóa"><i class="fa fa-trash fa-lg"></i></a>';
                        //        }
                        //        $(nTd).html(htmlLink);
                        //    }
                        //}
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
            t.on('order.dt search.dt', function () {
                t.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        }
    },
    initCompleteFunction: function (settings, json) {
        //var api = new $.fn.dataTable.Api(settings);
        //api.columns().every(function () {
        //    var column = this;
        //    var select = $('<select><option value=""></option></select>')
        //        .appendTo($(column.footer()).empty())
        //        .on('change', function () {
        //            var val = $.fn.dataTable.util.escapeRegex(
        //                $(this).val()
        //            );

        //            column
        //                .search(val ? '^' + val + '$' : '', true, false)
        //                .draw();
        //        });

        //    column.data().unique().sort().each(function (d, j) {
        //        select.append('<option value="' + d + '">' + d + '</option>')
        //    });
        //});
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
                rows: ["CityID"],
                cols: ["OperatorID"],
                rendererName: "Table",
                rowOrder: "value_a_to_z", colOrder: "value_z_to_a",
                hiddenAttributes: ["select.pvtRenderer", "renderers"]
            }, true);
    },

    printCertificate: function () {
        var form = $("#__AjaxAntiForgeryForm")[0];
        var dataForm = new FormData(form);
        dataForm.append('StartDate', inspectController.startDate.toISOString());
        dataForm.append('EndDate', inspectController.endDate.toISOString());
        dataForm.append('action', 'GetReport');
        $.validator.unobtrusive.parse(form);
        if ($(form).valid()) {
            $('html').addClass('waiting');
            var ajaxConfig = {
                type: 'POST',
                url: '/PrintCertificate/PrintFromInspect',
                data: dataForm,
                success: function (response) {
                    $('html').removeClass('waiting');
                    //open a new window note:this is a popup so it may be blocked by your browser
                    var newWindow = window.open("/PrintCertificate/Index", "Print Certificates");

                    //write the data to the document of the newWindow
                    newWindow.document.open();
                    newWindow.document.write(response);
                    newWindow.document.close();
                },
                error: function (response) {
                    $.notify(response.error, "error");
                }
            }
            if ($(form).attr('enctype') == "multipart/form-data") {
                ajaxConfig["contentType"] = false;
                ajaxConfig["processData"] = false;
            }
            $.ajax(ajaxConfig);
        }
        return false;
    }
}

inspectController.init();

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