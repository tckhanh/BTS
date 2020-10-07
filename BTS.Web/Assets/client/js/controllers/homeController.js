
//var data = "";

var homeController = {
    DoPost: function (url, id) {
        $.post(url, { id: id });  //Your values here..
    },
    token: function () {
        var form = $('#__AjaxAntiForgeryForm');
        return $('input[name="__RequestVerificationToken"]', form).val();
    },

    init: function () {
        //myMap.invalidateSize();
        //homeController.loadMap();
        //homeController.loadPostData();
        homeController.registerEventDataTable();
        homeController.registerEvent();
        if ($.inArray(myConstant.Info_CanViewMap_Role, myArrayRoles) > -1) {
            $('#SelCityID').attr('disabled', false);
        } else {
            $('#SelCityID').attr('disabled', true);
        }
            
    },
    registerEventDataTable: function () {        
    },
    registerEvent: function () {
        $('input[name="CertificateStatus"]').change(function () {
            if ($('input[name="CertificateStatus"]:checked').val() == myConstant.CertStatus_Valid) {
                $('input[name="DateRange"]').prop("disabled", false);
            } else {
                $('input[name="DateRange"]').prop("disabled", true);
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            homeController.loadPostData();
            //$('#MyDataTable').DataTable().ajax.reload();
        });

        $("a[href='#mapTab']").on('shown.bs.tab', function (e) {
            myMap.invalidateSize();
        });

        $('#btnReset').off('click').on('click', function () {
            $('#SelCityID').val('');
        });
    },

    loadPostData: function (CityId) {
        var form = $("#__AjaxAntiForgeryForm")[0];
        var dataForm = new FormData(form);
        if (CityId != null) {
            dataForm.set('SelCityID', CityId);
        }
        dataForm.append('action', 'GetReport');
        $.validator.unobtrusive.parse(form);
        if ($(form).valid()) {
            $('html').addClass('waiting');
            var ajaxConfig = {
                type: 'POST',
                url: '/Home/loadCertificate',
                data: dataForm,
                dataType: 'json',
                success: function (response) {
                    $('html').removeClass('waiting');
                    if (myMap != undefined && myMap != null && myMarkerClusters != null) {
                        myMap.removeLayer(myMarkerClusters);
                        myMarkerClusters.clearLayers();
                        //myMap.eachLayer(function (layer) {
                        //    myMap.removeLayer(layer);
                        //});
                    }
                    if (response != null) {
                        if ($.inArray(myConstant.Info_CanViewMap_Role, myArrayRoles) > -1)
                            homeController.loadMap(response.data);
                    }
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
    },

    loadGetData: function (url) {
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == "TimeOut") {
                    $.notify(response.message, "warn");
                    window.location.href = "/Account/Login"
                } else if (response.status == "Error") {
                    $.notify(response.message, "error");
                } else if (response.status == "Success") {
                    return response.data;
                }
                else {
                    //bootbox.alert(response.message);
                    $.notify(response.message, {
                        className: "warn"
                    });
                    return "";
                }
            },
            error: function (err) {
                console.log(err);
                $.notify(err.message, {
                    className: "error",
                    clickToHide: true
                });
                return "";
            }
        });
    },
    
    loadData: function () {

        $('#SelProfileID').attr('disabled', myNotAuthenticated);

        if ($.inArray(myConstant.System_CanExport_Role, myArrayRoles) > -1) {
            $("#MyDataTable")
                .on('draw.dt', function (e, settings, json, xhr) {
                    homeController.initCompleteFunction(settings, json);
                })
                .on('xhr.dt', function (e, settings, json, xhr) {
                    //new $.fn.dataTable.Api(settings).one('draw', function () {
                    //    homeController.initCompleteFunction(settings, json);
                    //});
                    new $.fn.dataTable.Api(settings).one('draw', function () {
                        homeController.initCompleteFunction(settings, json);
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
                            homeController.loadMap(json.data);
                        if ($.inArray(myConstant.Info_CanViewStatitics_Role, myArrayRoles) > -1)
                            homeController.loadPivotTable(json.data);
                    }
                })
                .dataTable({
                    createdRow: function (row, data, dataIndex) {
                        $(row).attr('data-id', data.Id);
                        $(row).attr('data-long', data.Longtitude);
                        $(row).attr('data-lat', data.Latitude);
                        $(row).addClass('mainRow');
                    },
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
                    "paging": true,
                    "info": true,
                    //"scrollX": true, // ảnh hưởng đến DataTable Id
                    "selector": true,
                    "ajax": {
                        "async": true,
                        "url": "/Home/loadCertificate",
                        "type": "POST",
                        "data": function (d) {
                            d.CityID = $('#SelCityID').val().trim();
                            d.OperatorID = $('#SelOperatorID').val().trim();
                            d.ProfileID = $('#SelProfileID').val().trim();
                            d.StartDate = homeController.startDate.toISOString();
                            d.EndDate = homeController.endDate.toISOString();
                            d.CertificateNum = $('#CertificateNum').val().trim();
                            d.BtsCodeOrAddress = $('#BtsCodeOrAddress').val().trim();
                            d.CertificateStatus = $('input[name=CertificateStatus]:checked').val();
                            d.__RequestVerificationToken = homeController.token();
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
                                if ($.inArray(myConstant.Info_CanPrintCertificate_Role, myArrayRoles) > -1) {
                                    $(nTd).html("<a href='javascript:homeController.printCertificate(\"" + oData.Id + "\")'>" + oData.Id + "</a>");
                                    //$(nTd).html("<a target='_blank' href='/PrintCertificate/Print/" + oData.Id + "'>" + oData.Id + "</a>");
                                } else {
                                    $(nTd).html(oData.Id);
                                }
                            }
                        },
                        {
                            "data": "BtsCode", "name": "BtsCode",
                            //fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            //    $(nTd).html("<a href='/Bts/Detail/" + oData.BtsCode + "'>" + oData.BtsCode + "</a>");
                            //}
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
                            "data": "Id", "name": "Id", "width": "14%", "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                var htmlLink = "";
                                if ($.inArray(myConstant.Data_CanViewDetail_Role, myArrayRoles) > -1) {
                                    htmlLink += '<a class="btn btn-info btn-sm" onclick="addinController.Detail(\'/Home/Detail/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Chi tiết"><i class="fa fa-address-card fa-lg"></i></a>';
                                }
                                if ($.inArray(myConstant.Data_CanEdit_Role, myArrayRoles) > -1) {
                                    htmlLink += ' <a class="btn btn-primary btn-sm" onclick="addinController.Edit(\'/Home/Edit/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Sửa"><i class="fa fa-pencil fa-lg"></i></a>';
                                }
                                if ($.inArray(myConstant.Data_CanDelete_Role, myArrayRoles) > -1) {
                                    htmlLink += ' <a class="btn btn-danger btn-sm" onclick="addinController.Delete(\'/Home/Delete/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Xóa"><i class="fa fa-trash fa-lg"></i></a>';
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
                    homeController.initCompleteFunction(settings, json);
                })
                .on('xhr.dt', function (e, settings, json, xhr) {
                    //new $.fn.dataTable.Api(settings).one('draw', function () {
                    //    homeController.initCompleteFunction(settings, json);
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
                            homeController.loadMap(json.data);
                        if ($.inArray(myConstant.Info_CanViewStatitics_Role, myArrayRoles) > -1)
                            homeController.loadPivotTable(json.data);
                    }
                })
                .dataTable({
                    createdRow: function (row, data, dataIndex) {
                        $(row).attr('data-id', data.Id);
                        $(row).attr('data-long', data.Longtitude);
                        $(row).attr('data-lat', data.Latitude);
                        $(row).addClass('mainRow');
                    },
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
                    "paging": true,
                    "info": true,
                    //"scrollX": true, // ảnh hưởng đến DataTable Id
                    "selector": true,
                    "ajax": {
                        "async": true,
                        "url": "/Home/loadCertificate",
                        "type": "POST",
                        "data": function (d) {
                            d.CityID = $('#SelCityID').val().trim();
                            d.OperatorID = $('#SelOperatorID').val().trim();
                            d.ProfileID = $('#SelProfileID').val().trim();
                            d.StartDate = homeController.startDate.toISOString();
                            d.EndDate = homeController.endDate.toISOString();
                            d.CertificateNum = $('#CertificateNum').val().trim();
                            d.BtsCodeOrAddress = $('#BtsCodeOrAddress').val().trim();
                            d.CertificateStatus = $('input[name=CertificateStatus]:checked').val();
                            d.__RequestVerificationToken = homeController.token();
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
                                if ($.inArray(myConstant.Info_CanPrintCertificate_Role, myArrayRoles) > -1) {
                                    $(nTd).html("<a href='javascript:homeController.printCertificate(\"" + oData.Id + "\")'>" + oData.Id + "</a>");
                                    //$(nTd).html("<a target='_blank' href='/PrintCertificate/Print/" + oData.Id + "'>" + oData.Id + "</a>");
                                } else {
                                    $(nTd).html(oData.Id);
                                }
                            }
                        },
                        {
                            "data": "BtsCode", "name": "BtsCode",
                            //fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            //    $(nTd).html("<a href='/Bts/Detail/" + oData.BtsCode + "'>" + oData.BtsCode + "</a>");
                            //}
                        },
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
                            "data": "Id", "name": "Id", "width": "14%", "className": "dt-body-center",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                var htmlLink = "";
                                if ($.inArray(myConstant.Data_CanViewDetail_Role, myArrayRoles) > -1) {
                                    htmlLink += '<a class="btn btn-info btn-sm" onclick="addinController.Detail(\'/Certificate/Detail/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Chi tiết"><i class="fa fa-address-card fa-lg"></i></a>';
                                }
                                if ($.inArray(myConstant.Data_CanEdit_Role, myArrayRoles) > -1) {
                                    htmlLink += ' <a class="btn btn-primary btn-sm" onclick="addinController.Edit(\'/Certificate/Edit/' + oData.Id + '\')" data-toggle="tooltip" data-placement="top" title="Sửa"><i class="fa fa-pencil fa-lg"></i></a>';
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
}

homeController.init();

// See post: http://asmaloney.com/2015/06/code/clustering-markers-on-leaflet-maps