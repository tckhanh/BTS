
function PopupForm(url) {
    var formDiv = $('<div/>');
    $.get(url)
    .done(function (response) {
        formDiv.html(response);

        Popup = formDiv.dialog({
            autoOpen: true,
            resizable: false,
            title: 'Fill Operator Details',
            height: 500,
            width: 700,
            close: function () {
                Popup.dialog('destroy').remove();
            }
        });
    });
}

function SubmitForm(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        $.ajax({
            type: "POST",
            url: form.action,
            data: $(form).serialize(),
            success: function (data) {
                if (data.success) {
                    Popup.dialog('close');
                    dataTable.ajax.reload();

                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                }
            }
        });
    }
    return false;
}

function Delete(id) {
    if (confirm('Are You Sure to Delete this Operator Record ?')) {
        $.ajax({
            type: "POST",
            url: '@Url.Action("Delete","Operator")/' + id,
            success: function (data) {
                if (data.success) {
                    dataTable.ajax.reload();

                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                }
            }
        });
    }
}