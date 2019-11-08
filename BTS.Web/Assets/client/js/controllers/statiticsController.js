$(function () {
    'use strict';

    window.chartColors = {
        red: 'rgb(255, 99, 132)',
        orange: 'rgb(255, 159, 64)',
        yellow: 'rgb(255, 205, 86)',
        green: 'rgb(75, 192, 192)',
        blue: 'rgb(54, 162, 235)',
        purple: 'rgb(153, 102, 255)',
        Red: 'rgb(255,0,0)',
        Lime: 'rgb(0,255,0)',
        Blue: 'rgb(0,0,255)',
        Yellow: 'rgb(255,255,0)',
        Cyan: 'rgb(0,255,255)',
        Magenta: 'rgb(255,0,255)',
        Maroon: 'rgb(128,0,0)',
        Olive: 'rgb(128,128,0)',
        Green: 'rgb(0,128,0)',
        Purple: 'rgb(128,0,128)',
        Teal: 'rgb(0,128,128)',
        Navy: 'rgb(0,0,128)',
        red: 'rgb(255,0,0)',
        almon: 'rgb(255,160,122)',
        antique_white: 'rgb(250,235,215)',
        aqua: 'rgb(0,255,255)',
        aqua_marine: 'rgb(127,255,212)',
        bchocolate: 'rgb(210,105,30)',
        beige: 'rgb(245,245,220)',
        bisque: 'rgb(255,228,196)',
        blanched_almond: 'rgb(255,235,205)',
        blue: 'rgb(0,0,255)',
        blue_steel: 'rgb(70,130,180)',
        blue_violet: 'rgb(138,43,226)',
        brown: 'rgb(165,42,42)',
        cadet_blue: 'rgb(95,158,160)',
        chid: 'rgb(218,112,214)',
        corn_silk: 'rgb(255,248,220)',
        crimson: 'rgb(220,20,60)',
        cyan: 'rgb(0,255,255)',
        Cyan_Aqua: 'rgb(0,255,255)',
        dark_blue: 'rgb(0,0,139)',
        dark_cyan: 'rgb(0,139,139)',
        dark_golden_rod: 'rgb(184,134,11)',
        dark_khaki: 'rgb(189,183,107)',
        dark_magenta: 'rgb(139,0,139)',
        dark_olive_green: 'rgb(85,107,47)',
        dark_orchid: 'rgb(153,50,204)',
        dark_red: 'rgb(139,0,0)',
        dark_salmon: 'rgb(233,150,122)',
        dark_sea_green: 'rgb(143,188,143)',
        dark_slate_blue: 'rgb(72,61,139)',
        dark_turquoise: 'rgb(0,206,209)',
        dark_violet: 'rgb(148,0,211)',
        deep_pink: 'rgb(255,20,147)',
        deep_sky_blue: 'rgb(0,191,255)',
        dfirebrick: 'rgb(178,34,34)',
        digo: 'rgb(75,0,130)',
        dodger_blue: 'rgb(30,144,255)',
        each_puff: 'rgb(255,218,185)',
        flower_blue: 'rgb(100,149,237)',
        gold: 'rgb(255,215,0)',
        green: 'rgb(0,128,0)',
        green_dark: 'rgb(0,100,0)',
        green_light: 'rgb(32,178,170)',
        green_yellow: 'rgb(173,255,47)',
        hot_pink: 'rgb(255,105,180)',
        igolden_rod: 'rgb(218,165,32)',
        indian_red: 'rgb(205,92,92)',
        khaki: 'rgb(240,230,140)',
        lawn_green: 'rgb(124,252,0)',
        lemon_chiffon: 'rgb(255,250,205)',
        light_blue: 'rgb(173,216,230)',
        light_coral: 'rgb(240,128,128)',
        light_cyan: 'rgb(224,255,255)',
        light_green: 'rgb(144,238,144)',
        light_pink: 'rgb(255,182,193)',
        light_yellow: 'rgb(255,255,224)',
        lime: 'rgb(0,255,0)',
        lime_green: 'rgb(50,205,50)',
        lmon: 'rgb(250,128,114)',
        lue: 'rgb(135,206,235)',
        Magenta_Fuchsia: 'rgb(255,0,255)',
        maroon: 'rgb(128,0,0)',
        maroon_light: 'rgb(128,0,0)',
        mcoral: 'rgb(255,127,80)',
        medium_aqua_marine: 'rgb(102,205,170)',
        medium_blue: 'rgb(0,0,205)',
        medium_orchid: 'rgb(186,85,211)',
        medium_purple: 'rgb(147,112,219)',
        medium_sea_green: 'rgb(60,179,113)',
        medium_slate_blue: 'rgb(123,104,238)',
        misty_rose: 'rgb(255,228,225)',
        moccasin: 'rgb(255,228,181)',
        navajo_white: 'rgb(255,222,173)',
        Navy: 'rgb(0,0,128)',
        navy: 'rgb(0,0,128)',
        night_blue: 'rgb(25,25,112)',
        nna: 'rgb(160,82,45)',
        Olive: 'rgb(128,128,0)',
        olive_drab: 'rgb(107,142,35)',
        orange_red: 'rgb(255,69,0)',
        pale_golden_rod: 'rgb(238,232,170)',
        pale_green: 'rgb(152,251,152)',
        pale_turquoise: 'rgb(175,238,238)',
        pale_violet_red: 'rgb(219,112,147)',
        peru: 'rgb(205,133,63)',
        pink: 'rgb(255,192,203)',
        plum: 'rgb(221,160,221)',
        powder_blue: 'rgb(176,224,230)',
        pring_green: 'rgb(0,255,127)',
        purple: 'rgb(128,0,128)',
        quoise: 'rgb(64,224,208)',
        red: 'rgb(255,0,0)',
        rest_green: 'rgb(34,139,34)',
        reuse: 'rgb(127,255,0)',
        rod_yellow: 'rgb(250,250,210)',
        rosy_brown: 'rgb(188,143,143)',
        royal_blue: 'rgb(65,105,225)',
        saddle_brown: 'rgb(139,69,19)',
        sandy_brown: 'rgb(244,164,96)',
        sea_green: 'rgb(46,139,87)',
        slate_blue: 'rgb(106,90,205)',
        spring_green: 'rgb(0,250,154)',
        tan: 'rgb(210,180,140)',
        teal: 'rgb(0,128,128)',
        thistle: 'rgb(216,191,216)',
        tLime: 'rgb(0,255,0)',
        tomato: 'rgb(255,99,71)',
        turquoise: 'rgb(72,209,204)',
        violet: 'rgb(238,130,238)',
        violet_red: 'rgb(199,21,133)',
        wheat: 'rgb(245,222,179)',
        wood: 'rgb(222,184,135)',
        Yellow: 'rgb(255,255,0)',
        yellow_green: 'rgb(154,205,50)',
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
            return Math.round(Samples.utils.rand(Date.now()));
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

    var statiticsController = {
        init: function () {
            statiticsController.loadPieChart("/Statitics/CerStatByOperator", "#pieChart_CerStatByOperator");
            statiticsController.loadPieChart("/Statitics/BtsStatInProcess", "#pieChart_BtsStatInProcess");

            statiticsController.loadLineChart("/Statitics/CertStatByOperatorCity", "#lineChart_CertStatByOperatorCity", "Tỉnh/ Thành phố", "Giấy CNKĐ", "Nhà mạng");
            statiticsController.loadLineChart("/Statitics/CertStatByOperatorYear", "#lineChart_CertStatByOperatorYear", "Năm", "Giấy CNKĐ", "Nhà mạng");
            
            statiticsController.loadPieChart("/Statitics/BtsStatByBand", "#pieChart_BtsStatByBand");
            statiticsController.loadPieChart("/Statitics/BtsStatByManufactory", "#pieChart_BtsStatByManufactory");

            statiticsController.loadLineChart("/Statitics/BtsStatByBandCity", "#lineChart_BtsStatByBandCity", "Tỉnh/ Thành phố", "Số trạm BTS", "Băng tần");
            statiticsController.loadLineChart("/Statitics/BtsStatByOperatorCity", "#lineChart_BtsStatByOperatorCity", "Tỉnh/ Thành phố", "Số trạm BTS", "Nhà mạng");
        },
        registerEventDataTable: function () {
        },
        registerEvent: function () {
        },

        loadLineChart: function (strUrl, ChartId, strAx, strAy, strTitle) {
            $.ajax({
                url: strUrl,
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
                                text: strTitle
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
                                        labelString: strAx
                                    }
                                }],
                                yAxes: [{
                                    display: true,
                                    scaleLabel: {
                                        display: true,
                                        labelString: strAy
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

                        var lineChartCanvas = $(ChartId);
                        var lineChart = new Chart(lineChartCanvas, config);
                        var firstColor = randomScalingFactor();

                        var colorNames = Object.keys(window.chartColors);

                        for (var i in response.chartData) {
                            if (i > 1) {
                                var colorName = colorNames[(firstColor + i) % colorNames.length];
                                var newColor = window.chartColors[colorName];
                                var lineChartValues = response.chartData[i];

                                statiticsController.addDataSetLine(lineChart, lineChartColumNames[i - 1], newColor, lineChartValues);
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

        loadPieChart: function (strUrl, ChartId) {
            $.ajax({
                url: strUrl,
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
                                maintainAspectRatio: true,
                                showTooltips: true,
                                tooltips: {
                                    callbacks: {
                                        label: function (item, data) {
                                            //console.log(item)
                                            var label = data.datasets[item.datasetIndex].label;
                                            label += '_' + data.labels[item.index];
                                            var value = data.datasets[item.datasetIndex].data[item.index];
                                            return label + ': ' + value;
                                        }
                                    }
                                },
                                pieceLabel: {
                                    render: 'percentage',
                                    fontColor: function (args) {
                                        var rgbStr = args.dataset.backgroundColor[args.index]
                                        var a = rgbStr.split("(")[1].split(")")[0];
                                        a = a.split(",");
                                        var threshold = 140;
                                        var luminance = 0.299 * a[0] + 0.587 * a[1] + 0.114 * a[2];
                                        return luminance > threshold ? 'black' : 'white';
                                    },
                                    precision: 2
                                }
                                //onAnimationProgress: drawSegmentValues
                            }
                        };
                        var pieChartCanvas1 = $(ChartId);
                        var pieChart1 = new Chart(pieChartCanvas1, config1);
                        var firstColor = randomScalingFactor();

                        for (var i in response.chartData) {
                            if (i > 1) {
                                var pieChartValues1 = response.chartData[i];
                                statiticsController.addDataSetPie(pieChart1, pieChartColumNames[i - 1], pieChartValues1, firstColor);
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

        addDataSetPie: function (chart, label, data, firstColor) {
            var newDataset = {
                backgroundColor: [],
                data: data,
                label: label,
            };
            var colorNames = Object.keys(window.chartColors);
            for (var index = 0; index < data.length; ++index) {
                var colorName = colorNames[(firstColor + index) % colorNames.length];
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

    statiticsController.init();
});