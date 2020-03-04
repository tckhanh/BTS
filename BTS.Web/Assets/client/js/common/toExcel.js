var toExcel = {
    init: function () {

    },
    registerEventDataTable: function () {
    },
    registerEvent: function () {
    },
    loadDetail: function (id) {
    },
    saveData: function () {
    },
    resetForm: function () {
    },
    cancelForm: function () {
    },
    deleteItem: function (id) {
    },
    fnExcelReport: function () {
        var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
        var textRange; var j = 0;
        tab = document.getElementById('MyDataTable'); // id of table

        for (j = 0; j < tab.rows.length; j++) {
            tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
        }

        tab_text = tab_text + "</table>";
        tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
        tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
        tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
        {
            txtArea1.document.open("txt/html", "replace");
            txtArea1.document.write(tab_text);
            txtArea1.document.close();
            txtArea1.focus();
            sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
        }
        else                 //other browser not tested on IE 11
            sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

        return (sa);
    }
};
toExcel.init();

var tableToExcel = (function (table, name) {
    var uri = 'data:application/vnd.ms-excel;base64,'
        , template = '<html xmlns: o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x: ExcelWorkbook><x: ExcelWorksheets><x: ExcelWorksheet><x: Name>{worksheet}</x: Name><x: WorksheetOptions><x: DisplayGridlines/></x: WorksheetOptions></x: ExcelWorksheet></x: ExcelWorksheets></x: ExcelWorkbook></xml><![endif]--> <meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head> <body><table>{table}</table></body></html> '
        , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
        , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
    return function (table, name) {
        if (!table.nodeType) {
            table = document.getElementById(table);
            $("td:hidden,th:hidden", table).remove();
            $("a", table).removeAttr("href");
        }
        var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
        window.location.href = uri + base64(format(template, ctx))
    }
})();


var ExportTableToExcel = {
    getIEVersion: function () {
        var rv = -1;
        if (navigator.appName == 'Microsoft Internet Explorer') {
            var ua = navigator.userAgent;
            var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
            if (re.exec(ua) != null)
                rv = parseFloat(RegExp.$1);
        }
        return rv;
    },

    tableToExcel: function (table, sheetName, fileName) {        
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");
        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) // If Internet Explorer
        {
            return ExportTableToExcel.fnExcelReport(table, fileName);
        }

        var uri = 'data:application/vnd.ms-excel;base64,',
            templateData = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>',
            base64Conversion = function (s) { return window.btoa(unescape(encodeURIComponent(s))) },
            formatExcelData = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

        $("tbody > tr[data-level='0']").show();

        if (!table.nodeType) {
            var clonedTable = $("#" + table).clone(true);

            //$("td:hidden,th:hidden", clonedTable).remove();
            //$("a", clonedTable).removeAttr("href");

            clonedTable.find('[style*="display: none"]').remove();
            clonedTable.find('a').removeAttr("href");            
            //clonedTable.find('td').css('mso-number-format', '\@');
            clonedTable.find('td').css('text-align', 'center');

            var tab_text = clonedTable[0].innerHTML;
            tab_text = tab_text.replace(/<td style=\"/gi, '<td style=\" mso-number-format: \\@; '); // set format as text

            var ctx = { worksheet: sheetName || 'Worksheet', table: tab_text }

            

            var element = document.createElement('a');
            element.setAttribute('href', 'data:application/vnd.ms-excel;base64,' + base64Conversion(formatExcelData(templateData, ctx)));
            element.setAttribute('download', fileName);
            element.style.display = 'none';
            document.body.appendChild(element);
            element.click();
            document.body.removeChild(element);
            $("tbody > tr[data-level='0']").hide();
        }
    },

    fnExcelReport: function (table, fileName) {
        var tab_text = "<table border='2px'>";
        var textRange;

        if (!table.nodeType) {
            var clonedTable = $("#" + table).clone(true);

            clonedTable.find('[style*="display: none"]').remove();
            clonedTable.find('a').removeAttr("href");
            clonedTable.find('td').css('text-align', 'center');

            $("tbody > tr[data-level='0']").show();

            tab_text = tab_text + clonedTable[0].innerHTML;
            tab_text = tab_text + "</table>";

            tab_text = tab_text.replace(/<td style=\"/gi, '<td style=\" mso-number-format: \\@; '); // set format as text
            tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
            tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
            tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

            txtArea1.document.open("txt/html", "replace");
            txtArea1.document.write(tab_text);
            txtArea1.document.close();
            txtArea1.focus();
            sa = txtArea1.document.execCommand("SaveAs", false, fileName + ".xlsx");
            $("tbody > tr[data-level='0']").hide();
            return (sa);
        }
    }
};