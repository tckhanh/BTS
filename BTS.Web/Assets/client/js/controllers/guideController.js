var guideController = {
    init: function () {
        commonController.activatejQueryTable();
        commonController.registerEventDataTable();
    },
    registerEventDataTable: function () {
        
    },

    registerEvent: function () {
    },

    activatejQueryTable: function () {
        $("#MyDataTable").DataTable({
            "language": {
                url: '/AppFiles/localization/vi_VI.json'
            },
    }
};
guideController.init();