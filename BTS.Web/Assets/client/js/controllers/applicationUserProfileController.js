var applicationUserProfileController = {
    init: function () {
        applicationUserProfileController.registerEvent();
    },

    registerEventDataTable: function () {
    },

    registerEvent: function () {
        $('.chkSelectedAreaItems').change(function () {
            $('.' + this.id).prop('checked', this.checked);
        });

        $('.chkSelectedCityItems').change(function () {
            var group = $(this).attr("group");
            if ($('.' + group + ':checked').length == $('.' + group).length) {
                $('#' + group).prop('checked', true);
            }
            else {
                $('#' + group).prop('checked', false);
            }
        });       
    },
};

applicationUserProfileController.init();