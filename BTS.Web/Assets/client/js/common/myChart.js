var myChart = {
    chartColors: {
        red: 'rgb(255, 99, 132)',
        orange: 'rgb(255, 159, 64)',
        yellow: 'rgb(255, 205, 86)',
        green: 'rgb(75, 192, 192)',
        blue: 'rgb(54, 162, 235)',
        purple: 'rgb(153, 102, 255)',

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
        yellow_green: 'rgb(154,205,50)'
    },
    init: function () {
    },
    registerEventDataTable: function () {
    },
    registerEvent: function () {
    },

    loadBarChart: function (strUrl, ChartId, strAx, strAy, strTitle) {
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
                    var barChartColumNames = response.chartData[0];

                    var barChartLabels = response.chartData[1];

                    var barChartOptions = {
                        responsive: true,
                        maintainAspectRatio: true,
                        showTooltips: true,
                        tooltips: {
                            mode: 'index',
                            callbacks: {
                                label: function (tooltipItem, data) {
                                    var sum = data.datasets.reduce((sum, dataset) => {
                                        return sum + parseInt(dataset.data[tooltipItem.index]);
                                    }, 0);
                                    var percent = 2 * parseInt(data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index]) / sum * 100;
                                    percent = percent.toFixed(2); // make a nice string
                                    return data.datasets[tooltipItem.datasetIndex].label + ': ' + percent + '%';
                                    //return data.datasets[tooltipItem.datasetIndex].label + "-" + data.labels[tooltipItem.index] + ":" + percentage + "%";
                                }
                            }
                        },

                        plugins: {
                            labels: {
                                // render 'label', 'value', 'percentage', 'image' or custom function, default is 'percentage'
                                render: function (args) {
                                    // args will be something like:
                                    // { label: 'Label', value: 123, percentage: 50, index: 0, dataset: {...} }
                                    return args.value;
                                },

                                fontColor: 'black',
                                /* Adjust data label font size according to chart size */
                                font: function (context) {
                                    var width = context.chart.width;
                                    var size = Math.round(width / 32);

                                    return {
                                        weight: 'bold',
                                        size: size
                                    };
                                }
                            },

                            beforeDraw: function (c) {
                                var chartHeight = c.chart.height;
                                c.scales['y-axis-0'].options.ticks.fontSize = chartHeight * 6 / 100;
                            }
                        },

                        title: {
                            display: false,
                            text: strTitle
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
                                },
                                ticks: {
                                    beginAtZero: true
                                }
                            }],
                            yAxes: [{
                                display: true,
                                scaleLabel: {
                                    display: true,
                                    labelString: strAy
                                },
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        },
                    }

                    var config = {
                        type: 'bar',
                        data: {
                            labels: barChartLabels,
                            datasets: []
                        },
                        options: barChartOptions,
                    };

                    var barChartCanvas = $(ChartId);
                    var barChart = new Chart(barChartCanvas, config);
                    var colorNames = Object.keys(myChart.chartColors);
                    var firstColorIndex = myChart.randomize(colorNames.length);

                    for (var i in response.chartData) {
                        if (i > 1) {
                            var colorName = colorNames[(firstColorIndex + i) % colorNames.length];
                            var newColor = myChart.chartColors[colorName];
                            var barChartValues = response.chartData[i];

                            myChart.addDataSetBar(barChart, barChartColumNames[i - 1], newColor, barChartValues);
                        }
                    }
                }
                else {
                    alert(response.message);
                }
                $('html').removeClass('waiting');
            },
            error: function (data) {
                alert("Message: " + data.message);
                $('html').removeClass('waiting');
            }
        });
    },

    loadStackedBarChart: function (strUrl, ChartId, strAx, strAy, strTitle) {
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
                    var stackedBarChartColumNames = response.chartData[0];

                    var stackedBarChartLabels = response.chartData[1];

                    var stackedBarChartOptions = {
                        responsive: true,
                        maintainAspectRatio: true,
                        tooltips: {
                            mode: 'index',
                            intersect: false,
                            callbacks: {
                                afterLabel: function (tooltipItem, data) {
                                    var sum = data.datasets.reduce((sum, dataset) => {
                                        return sum + parseInt(dataset.data[tooltipItem.index]);
                                    }, 0);
                                    var percent = parseInt(data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index]) / sum * 100;
                                    percent = percent.toFixed(2); // make a nice string
                                    return data.datasets[tooltipItem.datasetIndex].label + ': ' + percent + '% (' + $.number(sum, 0, ', ', '.') + ')';
                                    //return data.datasets[tooltipItem.datasetIndex].label + "-" + data.labels[tooltipItem.index] + ":" + percentage + "%";
                                }
                            }
                        },
                        plugins: {
                            labels: {
                                // render 'label', 'value', 'percentage', 'image' or custom function, default is 'percentage'
                                render: function (args) {
                                    // args will be something like:
                                    // { label: 'Label', value: 123, percentage: 50, index: 0, dataset: {...} }
                                    return '';
                                },
                                fontColor: 'black'
                            }
                        },
                        title: {
                            display: false,
                            text: strTitle
                        },
                        hover: {
                            mode: 'nearest',
                            intersect: true
                        },
                        scales: {
                            xAxes: [{
                                stacked: true,
                                display: true,
                                scaleLabel: {
                                    display: true,
                                    labelString: strAx
                                },
                                ticks: {
                                    beginAtZero: true
                                }
                            }],
                            yAxes: [{
                                stacked: true,
                                display: true,
                                scaleLabel: {
                                    display: true,
                                    labelString: strAy
                                },
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }

                    var config = {
                        type: 'bar',
                        data: {
                            labels: stackedBarChartLabels,
                            datasets: []
                        },
                        options: stackedBarChartOptions,
                    };

                    var stackedBarChartCanvas = $(ChartId);
                    var stackedBarChart = new Chart(stackedBarChartCanvas, config);
                    var colorNames = Object.keys(myChart.chartColors);
                    var firstColorIndex = myChart.randomize(colorNames.length);

                    for (var i in response.chartData) {
                        if (i > 1) {
                            var colorName = colorNames[(firstColorIndex + i) % colorNames.length];
                            var newColor = myChart.chartColors[colorName];
                            var stackedBarChartValues = response.chartData[i];

                            myChart.addDataSetLine(stackedBarChart, stackedBarChartColumNames[i - 1], newColor, stackedBarChartValues);
                        }
                    }
                }
                else {
                    alert(response.message);
                }
                $('html').removeClass('waiting');
            },
            error: function (data) {
                alert("Message: " + data.message);
                $('html').removeClass('waiting');
            }
        });
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
                            display: false,
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
                        options: lineChartOptions,
                    };

                    var lineChartCanvas = $(ChartId);
                    var lineChart = new Chart(lineChartCanvas, config);
                    var colorNames = Object.keys(myChart.chartColors);
                    var firstColorIndex = myChart.randomize(colorNames.length);

                    for (var i in response.chartData) {
                        if (i > 1) {
                            var colorName = colorNames[(firstColorIndex + i) % colorNames.length];
                            var newColor = myChart.chartColors[colorName];
                            var lineChartValues = response.chartData[i];

                            myChart.addDataSetLine(lineChart, lineChartColumNames[i - 1], newColor, lineChartValues);
                        }
                    }
                }
                else {
                    alert(response.message);
                }
                $('html').removeClass('waiting');
            },
            error: function (data) {
                alert("Message: " + data.message);
                $('html').removeClass('waiting');
            }
        });
    },

    loadBarLineChart: function (strUrl, ChartId, strAx, strAy, strTitle) {
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
                    var barLineChartColumNames = response.chartData[0];

                    var barLineChartLabels = response.chartData[1];

                    var barLineChartOptions = {
                        responsive: true,
                        maintainAspectRatio: true,
                        showTooltips: true,
                        tooltips: {
                            mode: 'index',
                            intersect: false,
                            callbacks: {
                                label: function (tooltipItem, data) {
                                    //get the concerned dataset
                                    var dataset = data.datasets[tooltipItem.datasetIndex];
                                    //calculate the total of this data set
                                    var total = dataset.data.reduce(function (previousValue, currentValue, currentIndex, array) {
                                        return previousValue + currentValue;
                                    });
                                    //get the current items value
                                    var currentValue = dataset.data[tooltipItem.index];
                                    //calculate the precentage based on the total and current item, also this does a rough rounding to give a whole number
                                    var percentage = Math.floor(((currentValue / total) * 100) + 0.5);

                                    return data.datasets[tooltipItem.datasetIndex].label + "-" + data.labels[tooltipItem.index] + ":" + percentage + "%";
                                }

                                //label: function (item, data) {
                                //    //console.log(item)
                                //    var label = data.datasets[item.datasetIndex].label;
                                //    label += '_' + data.labels[item.index];
                                //    var value = data.datasets[item.datasetIndex].data[item.index];
                                //    return label + ': ' + value;
                                //}
                            }
                        },
                        plugins: {
                            labels: {
                                // render 'label', 'value', 'percentage', 'image' or custom function, default is 'percentage'
                                render: function (args) {
                                    // args will be something like:
                                    // { label: 'Label', value: 123, percentage: 50, index: 0, dataset: {...} }
                                    return args.value;
                                }
                            }
                        },
                        title: {
                            display: false,
                            text: strTitle
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
                        type: 'bar',
                        data: {
                            labels: barLineChartLabels,
                            datasets: []
                        },
                        options: barLineChartOptions
                    };

                    var barLineChartCanvas = $(ChartId);
                    var barLineChart = new Chart(barLineChartCanvas, config);
                    var colorNames = Object.keys(myChart.chartColors);
                    var firstColorIndex = myChart.randomize(colorNames.length);

                    for (var i in response.chartData) {
                        if (i > 1) {
                            var colorName = colorNames[(firstColorIndex + i) % colorNames.length];
                            var newColor = myChart.chartColors[colorName];
                            var barLineChartValues = response.chartData[i];

                            myChart.addDataSetBarLine(barLineChart, barLineChartColumNames[i - 1], newColor, barLineChartValues);
                        }
                    }
                }
                else {
                    alert(response.message);
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
                                    label: function (tooltipItem, data) {
                                        //get the concerned dataset
                                        var dataset = data.datasets[tooltipItem.datasetIndex];
                                        //calculate the total of this data set
                                        var total = dataset.data.reduce(function (previousValue, currentValue, currentIndex, array) {
                                            return parseInt(previousValue) + parseInt(currentValue);
                                        });
                                        //get the current items value
                                        var currentValue = parseInt(dataset.data[tooltipItem.index]);
                                        //calculate the precentage based on the total and current item, also this does a rough rounding to give a whole number
                                        var percentage = Math.floor(((currentValue / total) * 100) + 0.5);

                                        return data.datasets[tooltipItem.datasetIndex].label + "-" + data.labels[tooltipItem.index] + ":" + percentage + "% (" + $.number(total, 0, ',', '.') + ")";
                                    }

                                    //label: function (item, data) {
                                    //    //console.log(item)
                                    //    var label = data.datasets[item.datasetIndex].label;
                                    //    label += '_' + data.labels[item.index];
                                    //    var value = data.datasets[item.datasetIndex].data[item.index];
                                    //    return label + ': ' + value;
                                    //}
                                }
                            },
                            plugins: {
                                labels: {
                                    // render 'label', 'value', 'percentage', 'image' or custom function, default is 'percentage'
                                    render: function (args) {
                                        // args will be something like:
                                        // { label: 'Label', value: 123, percentage: 50, index: 0, dataset: {...} }
                                        return args.value;
                                    },
                                    fontColor: function (args) {
                                        var rgbStr = args.dataset.backgroundColor[args.index]
                                        var a = rgbStr.split("(")[1].split(")")[0];
                                        a = a.split(",");
                                        var threshold = 140;
                                        var luminance = 0.299 * a[0] + 0.587 * a[1] + 0.114 * a[2];
                                        return luminance > threshold ? 'black' : 'white';
                                    },
                                    font: function (context) {
                                        var width = context.chart.width;
                                        var size = Math.round(width / 32);

                                        return {
                                            weight: 'bold',
                                            size: size
                                        };
                                    }
                                }
                            }
                        }
                    };
                    var pieChartCanvas1 = $(ChartId);
                    var pieChart1 = new Chart(pieChartCanvas1, config1);
                    var colorNames = Object.keys(myChart.chartColors);
                    var firstColorIndex = myChart.randomize(colorNames.length);

                    for (var i in response.chartData) {
                        if (i > 1) {
                            var pieChartValues1 = response.chartData[i];
                            myChart.addDataSetPie(pieChart1, pieChartColumNames[i - 1], firstColorIndex, pieChartValues1);
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

    loadRadarChart: function (strUrl, ChartId) {
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
                    var radarChartColumNames = response.chartData[0];
                    var radarChartLabels = response.chartData[1];
                    var config1 = {
                        type: 'radar',
                        data: {
                            labels: radarChartLabels,
                            datasets: []
                        },
                        options: {
                            responsive: true,
                            maintainAspectRatio: true,
                            showTooltips: true,
                            tooltips: {
                                callbacks: {
                                    afterLabel: function (tooltipItem, data) {
                                        //get the concerned dataset
                                        var dataset = data.datasets[tooltipItem.datasetIndex];
                                        //calculate the total of this data set
                                        var total = dataset.data.reduce(function (previousValue, currentValue, currentIndex, array) {
                                            return parseInt(previousValue) + parseInt(currentValue);
                                        });
                                        //get the current items value
                                        var currentValue = parseInt(dataset.data[tooltipItem.index]);
                                        //calculate the precentage based on the total and current item, also this does a rough rounding to give a whole number
                                        var percentage = Math.floor(((currentValue / total) * 100) + 0.5);

                                        return data.datasets[tooltipItem.datasetIndex].label + "-" + data.labels[tooltipItem.index] + ":" + percentage + "% (" + $.number(total, 0, ',', '.') + ")";
                                    }

                                    //label: function (item, data) {
                                    //    //console.log(item)
                                    //    var label = data.datasets[item.datasetIndex].label;
                                    //    label += '_' + data.labels[item.index];
                                    //    var value = data.datasets[item.datasetIndex].data[item.index];
                                    //    return label + ': ' + value;
                                    //}
                                }
                            },
                            plugins: {
                                labels: {
                                    // render 'label', 'value', 'percentage', 'image' or custom function, default is 'percentage'
                                    render: function (args) {
                                        // args will be something like:
                                        // { label: 'Label', value: 123, percentage: 50, index: 0, dataset: {...} }
                                        return args.value;
                                    },
                                    fontColor: function (args) {
                                        var rgbStr = args.dataset.backgroundColor[args.index]
                                        var a = rgbStr.split("(")[1].split(")")[0];
                                        a = a.split(",");
                                        var threshold = 140;
                                        var luminance = 0.299 * a[0] + 0.587 * a[1] + 0.114 * a[2];
                                        return luminance > threshold ? 'black' : 'white';
                                    },
                                    font: function (context) {
                                        var width = context.chart.width;
                                        var size = Math.round(width / 32);

                                        return {
                                            weight: 'bold',
                                            size: size
                                        };
                                    }
                                }
                            }
                        }
                    };
                    var radarChartCanvas1 = $(ChartId);
                    var radarChart1 = new Chart(radarChartCanvas1, config1);
                    var colorNames = Object.keys(myChart.chartColors);
                    var firstColorIndex = myChart.randomize(colorNames.length);

                    for (var i in response.chartData) {
                        if (i > 1) {
                            var colorName = colorNames[(firstColorIndex + i) % colorNames.length];
                            var newColor = myChart.chartColors[colorName];
                            var radarChartValues1 = response.chartData[i];
                            myChart.addDataSetRadar(radarChart1, radarChartColumNames[i - 1], newColor, radarChartValues1);
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


    addDataSetPie: function (chart, label, colorIndex, data) {
        var newDataset = {
            backgroundColor: [],
            data: data,
            label: label,
        };
        var colorNames = Object.keys(myChart.chartColors);
        for (var index = 0; index < data.length; ++index) {
            var colorName = colorNames[(colorIndex + index) % colorNames.length];
            var newColor = myChart.chartColors[colorName];
            newDataset.backgroundColor.push(newColor);
        }

        chart.data.datasets.push(newDataset);
        chart.update();
    },

    addDataSetLine: function (chart, label, color, data) {
        chart.data.datasets.push({
            type: 'line',
            label: label,
            borderColor: color,
            fill: false,
            data: data
        });
        chart.update();
    },

    addDataSetRadar: function (chart, label, color, data) {
        chart.data.datasets.push({
            label: label,
            borderColor: color,
            backgroundColor: Color(color).alpha(0.2).rgbString(),
            pointBackgroundColor: color,
            data: data
        });
        chart.update();
    },

    addDataSetBar: function (chart, label, color, data) {
        chart.data.datasets.push({
            type: 'bar',
            label: label,
            backgroundColor: color,
            data: data
        });
        chart.update();
    },

    addDataSetBarLine: function (chart, label, color, data) {
        chart.data.datasets.push({
            type: 'bar',
            label: label,
            backgroundColor: color,
            data: data
        });

        chart.data.datasets.push({
            type: 'line',
            borderColor: color,
            fill: false,
            data: data
        });
        chart.update();
    },

    randomize: function (max) {
        return Math.floor(Math.random() * max);
    },

    color: function (index) {
        return COLORS[index % COLORS.length];
    },

    transparentize: function (color, opacity) {
        var alpha = opacity === undefined ? 0.5 : 1 - opacity;
        return Color(color).alpha(alpha).rgbString();
    }
}
myChart.init();