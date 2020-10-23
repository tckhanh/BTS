﻿
//var data = "";

var homeController = {
    isLoadingData: 0,
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
            $('#btnReset').attr('disabled', false);
            $('#btnSearch').attr('disabled', false);
        } else {
            $('#SelCityID').attr('disabled', true);
            $('#btnReset').attr('disabled', true);
            $('#btnSearch').attr('disabled', true);
        }        
    },
    registerEventDataTable: function () {        
    },
    registerEvent: function () {
        $('#btnSearch').off('click').on('click', function () {
            if (myMap != undefined && myMap != null && myMarkerClusters != null) {
                myMap.removeLayer(myMarkerClusters);
                myMarkerClusters.clearLayers();
            }
            homeController.loadPostData();
            //$('#MyDataTable').DataTable().ajax.reload();
        });

        $("a[href='#mapTab']").on('shown.bs.tab', function (e) {
            myMap.invalidateSize();
        });

        $('#btnReset').off('click').on('click', function () {
            $('#SelCityID').val('');
            if (myMap != undefined && myMap != null && myMarkerClusters != null) {
                myMap.removeLayer(myMarkerClusters);
                myMarkerClusters.clearLayers();
            }
        });

        myMap.on('moveend', function () { 
            var curCenter = myMap.getCenter();
            homeController.loadPostData(curCenter);
            
            //var marker = L.marker(myMap.getCenter()).bindPopup('Your move Dest is here :)');
            //var circle = L.circle(myMap.getCenter(), 5000, {
            //    weight: 1,
            //    color: 'blue',
            //    fillColor: '#cacaca',
            //    fillOpacity: 0.2
            //});
            //myMap.addLayer(marker);
            //myMap.addLayer(circle);
        });
    },

    loadPostData: function (CityID) {
        var form = $("#__AjaxAntiForgeryForm")[0];
        var dataForm = new FormData(form);
        if (!myLib.isEmptyOrNull(CityID)) {
            dataForm.set('SelCityID', CityID);
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
                    if (!myLib.isEmptyOrNull(response)) {
                        if ($.inArray(myConstant.Info_CanViewMap_Role, myArrayRoles) > -1)
                            homeController.loadMap(response.data);
                    }
                    isLoadingData--;
                },
                error: function (response) {
                    $.notify(response.error, "error");
                    isLoadingData--;
                }
            }
            if ($(form).attr('enctype') == "multipart/form-data") {
                ajaxConfig["contentType"] = false;
                ajaxConfig["processData"] = false;
            }
            if (isLoadingData == 0) {
                isLoadingData++;
                $.ajax(ajaxConfig);
            }
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
    
    loadMap: function (markers) {
        var myURL = $('script[src$="leaflet.js"]').attr('src').replace('leaflet.js', '');
        if (!myLib.isEmptyOrNull(markers)) {
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

            var latLon = L.latLng(markers[0].Latitude, markers[0].Longtitude);
            var bounds = latLon.toBounds(500); // 500 = metres
            myMap.panTo(latLon).fitBounds(bounds);
            myMap.setZoom(8);
        }
    },
}

homeController.init();

// See post: http://asmaloney.com/2015/06/code/clustering-markers-on-leaflet-maps