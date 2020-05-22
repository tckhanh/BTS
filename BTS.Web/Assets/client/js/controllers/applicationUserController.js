var applicationUserController = {
    init: function () {
        applicationUserController.registerEvent();
    },

    registerEventDataTable: function () {
    },

    registerEvent: function () {
        $('.chkSelectedAreaItems').change(function () {
            $('.' + this.id).prop('checked', this.checked);
        });

        $('.chkSelectedCityItems').change(function () {
            var group = $(this).attr("group");
            if ($('.' + group + ':checked').length == $('.' + group ).length) {
                $('#' + group ).prop('checked', true);
            }
            else {
                $('#' + group ).prop('checked', false);
            }
        });

        //$("input[name='chkTextEffects']").change(function () {
        //    if ($("#cbSolid").is(':checked') == true) {
        //        alert('Solid');
        //    } else if ($("#cbOutline").is(':checked') == true) {
        //        alert('Outline');
        //    } else if ($("#cbSolid", "#cbOutline").is(':checked') == true) {
        //        alert('SolidOutline');
        //    } else if ($("#cbSolid", "#cbOutline").is(':checked') == false) {
        //        alert('No Effects');
        //    }
        //});

        //$('#getCheckboxesButton').on('click', function (event) {
        //    var checkboxValues = [];
        //    $('input[type="checkbox"]:checked').each(function (index, elem) {
        //        checkboxValues.push($(elem).val());
        //    });
        //    alert(checkboxValues.join(', '));
        //});

        //$("input[name='chkTextEffects']").change(function () {
        //    //do stuff
        //});
    },
};

applicationUserController.init();

