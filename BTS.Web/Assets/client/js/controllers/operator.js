var Popup, dataTable;
$(document).ready(function () {
    dataTable = $("#OperatorDataTable").DataTable({
        "ajax": {
            "url": "/Operator/GetData",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "ID" },
            { "data": "Name" },
            {
                "data": "ID", "render": function (data) {
                    return "<a class='btn btn-success btn-sm' onclick=PopupForm('/Operator/Edit/" + data + "')><i class='fa fa-pencil'></i> Sửa</a><a class='btn btn-danger btn-sm' style='margin-left:5px' onclick=Delete('" + data + "')><i class='fa fa-trash'></i> Xóa</a>";
                },
                "orderable": false,
                "searchable": false,
                "width": "150px"
            }
        ],
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
});

function CloseForm() {
    Popup.close();
}

function PopupForm(url) {
    var formDiv = $('<div />');
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
            dataType: 'json',
            success: function (data) {
                if (data.success) {
                    Popup.dialog('close');
                    dataTable.ajax.reload();

                    displayMessage(data.message, 'success');

                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    });
                }
            },
            failure: function (resp) {
                alert(resp);
            },
            error: function (err) {
                alert(err);
            }
        });
    }
    return false;
}

function SubmitForm_Tedu(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var id = $('#txtID').val();
        var name = $('#txtName').val();
        var operator = {
            ID: id,
            Name: name
        }
        $.ajax({
            type: "POST",
            url: form.action,
            data: {
                strOperator: JSON.stringify(operator)
            },
            dataType: 'json',
            success: function (data) {
                if (data.success) {
                    Popup.dialog('close');
                    dataTable.ajax.reload();

                    displayMessage(data.message, 'success');

                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    });
                }
            },
            failure: function (resp) {
                alert(resp);
            },
            error: function (err) {
                alert(err);
            }
        });
    }
    return false;
}

function Delete(id) {
    if (confirm('Bạn có chắc muốn xóa Nhà mạng này không?')) {
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