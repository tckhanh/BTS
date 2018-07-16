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
            homeController.loadChartStatisticByOperator();
            homeController.loadChartStatisticBtsInProcess();
            homeController.loadChartStatisticByOperatorYear();
            homeController.loadChartStatisticByOperatorCity();
        },
        registerEventDataTable: function () {
        },
        registerEvent: function () {
        },

        loadChartStatisticByOperator: function () {
            $.ajax({
                url: '/Home/StatisticByOperator',
                dataType: 'json',
                data: {},
                type: 'post',
                success: function (response) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Success") {
                        var pieChartColumNames = response.chartData[0];
                        var pieChartLabels = response.chartData[1];
                        var config1 = {
                            type: 'pie',
                            data: {
                                labels: pieChartLabels,
                                datasets: []
                            },
                            options: {
                                responsive: true,
                                maintainAspectRatio: true
                            }
                        };
                        var pieChartCanvas1 = $("#pieChartValidCertificates");
                        var pieChart1 = new Chart(pieChartCanvas1, config1);

                        for (var i in response.chartData) {
                            if (i > 1) {                                
                                var pieChartValues1 = response.chartData[i];
                                homeController.addDataSetPie(pieChart1, pieChartColumNames[2 - 1], pieChartValues1);
                            }
                        }                                               
                    }
                    else {
                        alert(xhr.response.message);
                    }
                    $('html').removeClass('waiting');
                },
                error: function (data) {
                    alert("Message: " + data.message);
                    $('html').removeClass('waiting');
                }
            });
        },

        loadChartStatisticBtsInProcess: function () {
            $.ajax({
                url: '/Home/StatisticBtsInProcess',
                dataType: 'json',
                data: {},
                type: 'post',
                success: function (response) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Success") {
                        var pieChartColumNames = response.chartData[0];
                        var pieChartLabels = response.chartData[1];
                        var config1 = {
                            type: 'pie',
                            data: {
                                labels: pieChartLabels,
                                datasets: []
                            },
                            options: {
                                responsive: true,
                                maintainAspectRatio: true
                            }
                        };
                        var pieChartCanvas1 = $("#pieChartBtsInProcess");
                        var pieChart1 = new Chart(pieChartCanvas1, config1);

                        for (var i in response.chartData) {
                            if (i > 1) {
                                var pieChartValues1 = response.chartData[i];
                                homeController.addDataSetPie(pieChart1, pieChartColumNames[2 - 1], pieChartValues1);
                            }
                        }
                    }
                    else {
                        alert(xhr.response.message);
                    }
                    $('html').removeClass('waiting');
                },
                error: function (data) {
                    alert("Message: " + data.message);
                    $('html').removeClass('waiting');
                }
            });
        },

        loadChartStatisticByOperatorYear: function () {
            $.ajax({
                url: '/Home/IssuedStatisticByOperatorYear',
                dataType: 'json',
                data: {},
                type: 'post',
                success: function (response) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Success") {
                        var lineChartColumNames = response.chartData[0];

                        var lineChartLabels = response.chartData[1];

                        var lineChartOptions = {
                            responsive: true,
                            maintainAspectRatio: true,
                            title: {
                                display: true,
                                text: 'Thống kê theo năm'
                            },
                            tooltips: {
                                mode: 'index',
                                intersect: false,
                            },
                            hover: {
                                mode: 'nearest',
                                intersect: true
                            },
                            scales: {
                                xAxes: [{
                                    display: true,
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'Năm'
                                    }
                                }],
                                yAxes: [{
                                    display: true,
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'Giấy CNKĐ'
                                    }
                                }]
                            }
                        }

                        var config = {
                            type: 'line',
                            data: {
                                labels: lineChartLabels,
                                datasets: []
                            },
                            options: lineChartOptions
                        };

                        var lineChartCanvas = $("#lineChartIssuedCertificatesByYear");
                        var lineChart = new Chart(lineChartCanvas, config);

                        var colorNames = Object.keys(window.chartColors);

                        for (var i in response.chartData) {
                            if (i > 1) {
                                var colorName = colorNames[i % colorNames.length];
                                var newColor = window.chartColors[colorName];
                                var lineChartValues = response.chartData[i];

                                homeController.addDataSetLine(lineChart, lineChartColumNames[i - 1], newColor, lineChartValues);
                            }
                        }
                    }
                    else {
                        alert(xhr.response.message);
                    }
                    $('html').removeClass('waiting');
                },
                error: function (data) {
                    alert("Message: " + data.message);
                    $('html').removeClass('waiting');
                }
            });
        },

        loadChartStatisticByOperatorCity: function () {
            $.ajax({
                url: '/Home/IssuedStatisticByOperatorCity',
                dataType: 'json',
                data: {},
                type: 'post',
                success: function (response) {
                    if (response.status == "TimeOut") {
                        $.notify(response.message, "warn");
                        window.location.href = "/Account/Login"
                    } else if (response.status == "Success") {
                        var lineChartColumNames = response.chartData[0];

                        var lineChartLabels = response.chartData[1];

                        var lineChartOptions = {
                            responsive: true,
                            maintainAspectRatio: true,
                            title: {
                                display: true,
                                text: 'Thống kê theo Tỉnh/ Thành phố'
                            },
                            tooltips: {
                                mode: 'index',
                                intersect: false,
                            },
                            hover: {
                                mode: 'nearest',
                                intersect: true
                            },
                            scales: {
                                xAxes: [{
                                    display: true,
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'Tỉnh/ Thành phố'
                                    }
                                }],
                                yAxes: [{
                                    display: true,
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'Giấy CNKĐ'
                                    }
                                }]
                            }
                        }

                        var config = {
                            type: 'line',
                            data: {
                                labels: lineChartLabels,
                                datasets: []
                            },
                            options: lineChartOptions
                        };

                        var lineChartCanvas = $("#lineChartvalidCertificatesByCity");
                        var lineChart = new Chart(lineChartCanvas, config);

                        var colorNames = Object.keys(window.chartColors);

                        for (var i in response.chartData) {
                            if (i > 1) {
                                var colorName = colorNames[i % colorNames.length];
                                var newColor = window.chartColors[colorName];
                                var lineChartValues = response.chartData[i];

                                homeController.addDataSetLine(lineChart, lineChartColumNames[i - 1], newColor, lineChartValues);
                            }
                        }
                    }
                    else {
                        alert(xhr.response.message);
                    }
                    $('html').removeClass('waiting');
                },
                error: function (data) {
                    alert("Message: " + data.message);
                    $('html').removeClass('waiting');
                }
            });
        },
        addDataSetPie: function (chart, label, data) {
            var newDataset = {
                backgroundColor: [],
                data: data,
                label: label,
            };
            var colorNames = Object.keys(window.chartColors);
            for (var index = 0; index < data.length; ++index) {
                var colorName = colorNames[index % colorNames.length];
                var newColor = window.chartColors[colorName];
                newDataset.backgroundColor.push(newColor);
            }

            chart.data.datasets.push(newDataset);
            chart.update();
        },

        addDataSetLine: function (chart, label, color, data) {
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