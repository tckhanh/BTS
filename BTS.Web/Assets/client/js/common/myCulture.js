var myCulture = {
    init: function () {
        //Ask ASP.NET what culture we prefer, because we stuck it in a meta tag
        var data = $("meta[name='accept-language']").attr("content")

        //Tell jQuery to figure it out also on the client side.
        $.global.preferCulture(data);

        //Tell the validator, for example,
        // that we want numbers parsed a certain way!
        $.validator.methods.number = function (value, element) {
            if ($.global.parseFloat(value)) {
                return true;
            }
            return false;
        }

        //Fix the range to use globalized methods
        jQuery.extend(jQuery.validator.methods, {
            range: function (value, element, param) {
                //Use the Globalization plugin to parse the value
                var val = $.global.parseFloat(value);
                return this.optional(element) || (val >= param[0] && val <= param[1]);
            }
        });

        //Setup datepickers if we don't support it natively!
        //if (!Modernizr.inputtypes.date) {
        //    if ($.global.culture.name != 'en-us' && $.global.culture.name != 'en') {

        //        var datepickerScriptFile = "/Scripts/globdatepicker/jquery.ui.datepicker-" + $.global.culture.name + ".js";
        //        //Now, load the date picker support for this language 
        //        // and set the defaults for a localized calendar
        //        $.getScript(datepickerScriptFile, function () {
        //            $.datepicker.setDefaults($.datepicker.regional[$.global.culture.name]);
        //        });
        //    }
        //    $("input[type='datetime']").datepicker();
        //}
    }
};
myCulture.init();


