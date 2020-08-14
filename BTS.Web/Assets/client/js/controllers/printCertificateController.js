var printCertificateController = {
    init: function () {

    },
    registerEventDataTable: function () {
    },
    registerEvent: function () {

    },
    buildReportTree: function () {
        dm = new dTree('dm');

        // Categories
        dm.add(0, -1, '');
        dm.add(1, 0, 'Bản in Giấy CNKĐ', null, null, null, null, null, true);
        dm.add(2, 0, 'Bản sao Giấy CNKĐ');

        dm.add(101, 1, 'Loại 1 trạm BTS', '/PrintCertificate/Index?Template=false&BtsNum=1');
        dm.add(102, 1, 'Loại 2 trạm BTS', '/PrintCertificate/Index?Template=false&BtsNum=2');
        dm.add(103, 1, 'Loại 3 trạm BTS', '/PrintCertificate/Index?Template=false&BtsNum=3');
        dm.add(104, 1, 'Loại 4 trạm BTS', '/PrintCertificate/Index?Template=false&BtsNum=4');
        dm.add(105, 1, 'Loại 5 trạm BTS', '/PrintCertificate/Index?Template=false&BtsNum=5');
        dm.add(106, 1, 'Loại 6 trạm BTS', '/PrintCertificate/Index?Template=false&BtsNum=6');

        dm.add(201, 2, 'Loại 1 trạm BTS', '/PrintCertificate/Index?Template=true&BtsNum=1');
        dm.add(202, 2, 'Loại 2 trạm BTS', '/PrintCertificate/Index?Template=true&BtsNum=2');
        dm.add(203, 2, 'Loại 3 trạm BTS', '/PrintCertificate/Index?Template=true&BtsNum=3');
        dm.add(204, 2, 'Loại 4 trạm BTS', '/PrintCertificate/Index?Template=true&BtsNum=4');
        dm.add(205, 2, 'Loại 5 trạm BTS', '/PrintCertificate/Index?Template=true&BtsNum=5');
        dm.add(206, 2, 'Loại 6 trạm BTS', '/PrintCertificate/Index?Template=true&BtsNum=6');

        document.write(dm);
    }
}

printCertificateController.init();

