var myLib  = {
    isEmptyOrNull: function (value) {
        return typeof value == 'string' && !value.trim() || typeof value == 'undefined' || value === null;
    },
};

