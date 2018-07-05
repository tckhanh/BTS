$(function () {
    'use strict';

    window.chartColors = {
        red: 'rgb(255, 99, 132)',
        orange: 'rgb(255, 159, 64)',
        yellow: 'rgb(255, 205, 86)',
        green: 'rgb(75, 192, 192)',
        blue: 'rgb(54, 162, 235)',
        purple: 'rgb(153, 102, 255)',
        grey: 'rgb(201, 203, 207)'
    };

    (function (global) {
        var Months = [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June',
            'July',
            'August',
            'September',
            'October',
            'November',
            'December'
        ];

        var COLORS = [
            '#4dc9f6',
            '#f67019',
            '#f53794',
            '#537bc4',
            '#acc236',
            '#166a8f',
            '#00a950',
            '#58595b',
            '#8549ba'
        ];

        var Samples = global.Samples || (global.Samples = {});
        var Color = global.Color;

        Samples.utils = {
            // Adapted from http://indiegamr.com/generate-repeatable-random-numbers-in-js/
            srand: function (seed) {
                this._seed = seed;
            },

            rand: function (min, max) {
                var seed = this._seed;
                min = min === undefined ? 0 : min;
                max = max === undefined ? 1 : max;
                this._seed = (seed * 9301 + 49297) % 233280;
                return min + (this._seed / 233280) * (max - min);
            },

            numbers: function (config) {
                var cfg = config || {};
                var min = cfg.min || 0;
                var max = cfg.max || 1;
                var from = cfg.from || [];
                var count = cfg.count || 8;
                var decimals = cfg.decimals || 8;
                var continuity = cfg.continuity || 1;
                var dfactor = Math.pow(10, decimals) || 0;
                var data = [];
                var i, value;

                for (i = 0; i < count; ++i) {
                    value = (from[i] || 0) + this.rand(min, max);
                    if (this.rand() <= continuity) {
                        data.push(Math.round(dfactor * value) / dfactor);
                    } else {
                        data.push(null);
                    }
                }

                return data;
            },

            labels: function (config) {
                var cfg = config || {};
                var min = cfg.min || 0;
                var max = cfg.max || 100;
                var count = cfg.count || 8;
                var step = (max - min) / count;
                var decimals = cfg.decimals || 8;
                var dfactor = Math.pow(10, decimals) || 0;
                var prefix = cfg.prefix || '';
                var values = [];
                var i;

                for (i = min; i < max; i += step) {
                    values.push(prefix + Math.round(dfactor * i) / dfactor);
                }

                return values;
            },

            months: function (config) {
                var cfg = config || {};
                var count = cfg.count || 12;
                var section = cfg.section;
                var values = [];
                var i, value;

                for (i = 0; i < count; ++i) {
                    value = Months[Math.ceil(i) % 12];
                    values.push(value.substring(0, section));
                }

                return values;
            },

            color: function (index) {
                return COLORS[index % COLORS.length];
            },

            transparentize: function (color, opacity) {
                var alpha = opacity === undefined ? 0.5 : 1 - opacity;
                return Color(color).alpha(alpha).rgbString();
            }
        };

        // DEPRECATED
        window.randomScalingFactor = function () {
            return Math.round(Samples.utils.rand(-100, 100));
        };

        // INITIALIZATION

        Samples.utils.srand(Date.now());

        // Google Analytics
        /* eslint-disable */
        if (document.location.hostname.match(/^(www\.)?chartjs\.org$/)) {
            (function (i, s, o, g, r, a, m) {
                i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                    (i[r].q = i[r].q || []).push(arguments)
                }, i[r].l = 1 * new Date(); a = s.createElement(o),
                m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
            })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
            ga('create', 'UA-28909194-3', 'auto');
            ga('send', 'pageview');
        }
        /* eslint-enable */
    }(this));

    var homeController = {
        init: function () {
            var bar = $('.progress-bar');
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
                    $('#btnSubmit').prop('disabled', true);
                    $('#btnReset').prop('disabled', true);
                    $('#selectOperatorId').prop('disabled', true);
                    $('#selectCityId').prop('disabled', true);
                },
                beforeSend: function () {
                    $('html').addClass('waiting');
                    bar.html('Bắt đầu thực hiện!');
                    bar.addClass('active');
                    $('#progressRow').show();
                },
                uploadProgress: function (event, position, total, percentComplete) {
                    if (percentComplete = 100)
                        bar.html('Đã gửi yêu cầu thống kê ....');
                    else
                        bar.html('Đang gửi yêu cầu thống kê : ' + percentComplete + '%');
                },
                error: function (data) {
                    var r = jQuery.parseJSON(data.responseText);
                    alert("Message: " + r.message);
                    alert("StackTrace: " + r.StackTrace);
                    alert("ExceptionType: " + r.ExceptionType);
                    $('html').removeClass('waiting');
                    bar.removeClass('active');
                    $('#btnSubmit').prop('disabled', false);
                    $('#btnReset').prop('disabled', false);
                    $('#selectOperatorId').prop('disabled', false);
                    $('#selectCityId').prop('disabled', false);
                },
                success: function (responseJSON, statusText, xhr, element) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (responseJSON.status == "Success") {
                        bar.html('Đã thực hiện xong!');
                        var pieChartColumNames = responseJSON.chartData[0];

                        var pieChartLabels = responseJSON.chartData[1];

                        var pieChartOptions = {
                            responsive: true,
                            maintainAspectRatio: true
                        }

                        var pieChartValues = responseJSON.chartData[2];
                        var pieChartData = {
                            // These labels appear in the legend and in the tooltips when hovering different arcs
                            labels: pieChartLabels,
                            datasets: [{
                                label: 'Player Score',
                                backgroundColor: ["rgba(54, 162, 235, 0.2)", "rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                                borderColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                                hoverBackgroundColor: "rgba(153, 102, 255, 1)",
                                hoverBorderColor: "rgba(153, 102, 255, 1)",
                                data: pieChartValues
                            }],
                        };
                        var pieChartCanvas = $("#pieChartValidCertificates");
                        var pieChart = new Chart(pieChartCanvas, {
                            type: 'pie',
                            data: pieChartData,
                            options: pieChartOptions
                        });

                        var pieChartValues = responseJSON.chartData[3];
                        var pieChartData = {
                            // These labels appear in the legend and in the tooltips when hovering different arcs
                            labels: pieChartLabels,
                            datasets: [{
                                label: 'Player Score',
                                backgroundColor: ["rgba(54, 162, 235, 0.2)", "rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
                                borderColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                                hoverBackgroundColor: "rgba(153, 102, 255, 1)",
                                hoverBorderColor: "rgba(153, 102, 255, 1)",
                                data: pieChartValues
                            }],
                        };
                        var pieChartCanvas = $("#pieChartValidCertificates");
                        var pieChart = new Chart(pieChartCanvas, {
                            type: 'pie',
                            data: pieChartData,
                            options: pieChartOptions
                        });

                        //var colorNames = Object.keys(window.chartColors);

                        //for (var i in responseJSON.chartData) {
                        //    if (i > 2) {
                        //        var colorName = colorNames[i % colorNames.length];
                        //        var newColor = window.chartColors[colorName];
                        //        var pieChartValues = responseJSON.chartData[i];
                        //        homeController.addData(pieChart, pieChartColumNames[i - 1], newColor, pieChartValues);
                        //    }
                        //}
                    }
                    else {
                        bar.html('Lỗi trong quá trình thực hiện!');
                        alert(xhr.responseJSON.message);
                    }
                    $('html').removeClass('waiting');
                    bar.removeClass('active');
                    $('#btnSubmit').prop('disabled', false);
                    $('#btnReset').prop('disabled', false);
                    $('#selectOperatorId').prop('disabled', false);
                    $('#selectCityId').prop('disabled', false);
                },
                complete: function (xhr) {
                },
                async: true
            });
        },
        registerEventDataTable: function () {
        },
        registerEvent: function () {
        },
        addData: function (chart, label, color, data) {
            chart.data.datasets.push({
                label: label,
                backgroundColor: color,
                data: data
            });
            chart.update();
        }
    }

    homeController.init();
});