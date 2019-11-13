
//var data = "";

var certificateController = {
    startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 1),
    endDate: new Date(),
    token: function () {
        var form = $('#__AjaxAntiForgeryForm');
        return $('input[name="__RequestVerificationToken"]', form).val();
    },

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

        $('#MyDataTable tbody').on('click', 'td.details-control', function () {
            var tr = $(this).closest('tr');
            var row = table.row(tr);

            if (row.child.isShown()) {
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                row.child(certificateController.format(row.data())).show();
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
        certificateController.startDate = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        certificateController.endDate = new Date();

        $('input[name="DateRange"]').daterangepicker(
            {
                locale: {
                    format: 'DD/MM/YYYY'
                },
                startDate: certificateController.startDate,
                endDate: certificateController.endDate
            },
            function (start, end, label) {
                //alert("A new date range was chosen: " + start.format('DD/MM/YYYY') + ' to ' + end.format('DD/MM/YYYY'));
                certificateController.startDate = start;
                certificateController.endDate = end;
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
                    certificateController.initCompleteFunction(settings, json);
                })
                .on('xhr.dt', function (e, settings, json, xhr) {
                    //new $.fn.dataTable.Api(settings).one('draw', function () {
                    //    certificateController.initCompleteFunction(settings, json);
                    //});
                    new $.fn.dataTable.Api(settings).one('draw', function () {
                        certificateController.initCompleteFunction(settings, json);
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
                            certificateController.loadMap(json.data);
                        if ($.inArray(myConstant.Info_CanViewStatitics_Role, myArrayRoles) > -1)
                            certificateController.loadPivotTable(json.data);
                    }
                })
                .dataTable({
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
                    "paging": true,
                    "info": true,
                    //"scrollX": true, // ảnh hưởng đến DataTable Id
                    "selector": true,
                    "ajax": {
                        "async": true,
                        "url": "/Certificate/loadCertificate",
                        "type": "POST",
                        "data": function (d) {
                            d.CityID = $('#CityID').val().trim();
                            d.OperatorID = $('#OperatorID').val().trim();
                            d.ProfileID = $('#ProfileID').val().trim();
                            d.StartDate = certificateController.startDate.toISOString();
                            d.EndDate = certificateController.endDate.toISOString();
                            d.BtsCodeOrAddress = $('#BtsCodeOrAddress').val().trim();
                            d.IsExpired = $('input[name=IsExpired]:checked').val();
                            d.__RequestVerificationToken = certificateController.token();
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
                        { "data": "OperatorID", "name": "OperatorID" },
                        { "data": "CityID", "name": "CityID", "className": "dt-body-center" },
                        {
                            "data": "Id", "name": "Id", "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<a target='_blank' href='/PrintCertificate/Index/" + oData.Id + "'>" + oData.Id + "</a>");
                            }
                        },
                        {
                            "data": "BtsCode", "name": "BtsCode",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<a href='/Bts/Detail/" + oData.BtsCode + "'>" + oData.BtsCode + "</a>");
                            }
                        },        // index 2
                        { "data": "Address", "name": "Address" },
                        {
                            "data": "IssuedDate", "name": "IssuedDate", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                return (moment(row["IssuedDate"]).format("DD/MM/YYYY"));
                            }
                        },
                        {
                            "data": "ExpiredDate", "name": "ExpiredDate", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                return (moment(row["ExpiredDate"]).format("DD/MM/YYYY"));
                            }
                        },
                        {
                            "data": "Id", "name": "Id", "width": "14%",  "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                var htmlLink = "";
                                if ($.inArray(myConstant.Data_CanViewDetail_Role, myArrayRoles) > -1) {
                                    htmlLink += '<a class="btn btn-info btn-sm" onclick="addinController.Detail(\'/Certificate/AddOrEdit/' + oData.Id + '?act=Detail\')" data-toggle="tooltip" data-placement="top" title="Chi tiết"><i class="fa fa-address-card fa-lg"></i></a>';
                                }
                                if ($.inArray(myConstant.Data_CanEdit_Role, myArrayRoles) > -1) {
                                    htmlLink += ' <a class="btn btn-primary btn-sm" onclick="addinController.Edit(\'/Certificate/AddOrEdit/' + oData.Id + '?act=Edit\')" data-toggle="tooltip" data-placement="top" title="Sửa"><i class="fa fa-pencil fa-lg"></i></a>';
                                }
                                if ($.inArray(myConstant.Data_CanDelete_Role, myArrayRoles) > -1) {
                                    htmlLink += ' <a class="btn btn-danger btn-sm" onclick="addinController.Delete(\'/Certificate/Delete/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Xóa"><i class="fa fa-trash fa-lg"></i></a>';
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
            //var t = $('#MyDataTable').DataTable();
            //t.on('order.dt search.dt', function () {
            //    t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            //        cell.innerHTML = i + 1;
            //    });
            //}).draw();
        } else {
            $("#MyDataTable")
                .on('draw.dt', function (e, settings, json, xhr) {
                    certificateController.initCompleteFunction(settings, json);
                })
                .on('xhr.dt', function (e, settings, json, xhr) {
                    //new $.fn.dataTable.Api(settings).one('draw', function () {
                    //    certificateController.initCompleteFunction(settings, json);
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
                            certificateController.loadMap(json.data);
                        if ($.inArray(myConstant.Info_CanViewStatitics_Role, myArrayRoles) > -1)
                            certificateController.loadPivotTable(json.data);
                    }
                })
                .dataTable({
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
                    "paging": true,
                    "info": true,
                    //"scrollX": true, // ảnh hưởng đến DataTable Id
                    "selector": true,
                    "ajax": {
                        "async": true,
                        "url": "/Certificate/loadCertificate",
                        "type": "POST",
                        "data": function (d) {
                            d.CityID = $('#CityID').val().trim();
                            d.OperatorID = $('#OperatorID').val().trim();
                            d.ProfileID = $('#ProfileID').val().trim();
                            d.StartDate = certificateController.startDate.toISOString();
                            d.EndDate = certificateController.endDate.toISOString();
                            d.BtsCodeOrAddress = $('#BtsCodeOrAddress').val().trim();
                            d.IsExpired = $('input[name=IsExpired]:checked').val();
                            d.__RequestVerificationToken = certificateController.token();
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
                        { "data": "OperatorID", "name": "OperatorID"},
                        { "data": "CityID", "name": "CityID", "className": "dt-body-center"},
                        {
                            "data": "Id", "name": "Id", "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<a target='_blank' href='/PrintCertificate/Index/" + oData.Id + "'>" + oData.Id + "</a>");
                            }
                        },
                        {
                            "data": "BtsCode", "name": "BtsCode",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<a href='/Bts/Detail/" + oData.BtsCode + "'>" + oData.BtsCode + "</a>");
                            }
                        },
                        { "data": "Address", "name": "Address"},
                        {
                            "data": "IssuedDate", "name": "IssuedDate", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                return (moment(row["IssuedDate"]).format("DD/MM/YYYY"));
                            }
                        },
                        {
                            "data": "ExpiredDate", "name": "ExpiredDate", "className": "dt-body-center",
                            "render": function (data, type, row) {
                                return (moment(row["ExpiredDate"]).format("DD/MM/YYYY"));
                            }
                        },
                        {
                            "data": "Id", "name": "Id", "width": "14%",  "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                var htmlLink = "";
                                if ($.inArray(myConstant.Data_CanViewDetail_Role, myArrayRoles) > -1) {
                                    htmlLink += '<a class="btn btn-info btn-sm" onclick="addinController.Detail(\'/Certificate/AddOrEdit/' + oData.Id + '?act=Detail\')" data-toggle="tooltip" data-placement="top" title="Chi tiết"><i class="fa fa-address-card fa-lg"></i></a>';
                                }
                                if ($.inArray(myConstant.Data_CanEdit_Role, myArrayRoles) > -1) {
                                    htmlLink += ' <a class="btn btn-primary btn-sm" onclick="addinController.Edit(\'/Certificate/AddOrEdit/' + oData.Id + '?act=Edit\')" data-toggle="tooltip" data-placement="top" title="Sửa"><i class="fa fa-pencil fa-lg"></i></a>';
                                }
                                if ($.inArray(myConstant.Data_CanDelete_Role, myArrayRoles) > -1) {
                                    htmlLink += ' <a class="btn btn-danger btn-sm" onclick="addinController.Delete(\'/Certificate/Delete/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Xóa"><i class="fa fa-trash fa-lg"></i></a>';
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
            //var t = $('#MyDataTable').DataTable();
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
                rendererName: "Bar Chart",
                rowOrder: "value_a_to_z", colOrder: "value_z_to_a",
                hiddenAttributes: ["select.pvtRenderer", "renderers"]
            }, true);
    },

    jQueryAjaxPost: function () {
        var form = $("#__AjaxAntiForgeryForm")[0];
        var dataForm = new FormData(form);
        dataForm.append('StartDate', certificateController.startDate.toISOString());
        dataForm.append('EndDate', certificateController.endDate.toISOString());
        $.validator.unobtrusive.parse(form);
        if ($(form).valid()) {
            var ajaxConfig = {
                type: 'POST',
                url: '/PrintCertificate/Print',
                data: dataForm,
                success: function (response) {
                    //open a new window note:this is a popup so it may be blocked by your browser
                    var newWindow = window.open("", "Print Certificates");

                    //write the data to the document of the newWindow
                    newWindow.document.write(response);
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