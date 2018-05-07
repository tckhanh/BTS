$(function () {
    var inCaseOfController = {
        init: function () {
            inCaseOfController.loadData();
            inCaseOfController.registerEventDataTable();
            inCaseOfController.registerEvent();
        },
        registerEventDataTable: function () {
            var table = $("#MyDataTable").DataTable();
            table.on('draw', function () {
                $('.btn-edit').off('click').on('click', function () {
                    $('#modalAddUpdate').modal('show');
                    var id = $(this).data('myid');
                    inCaseOfController.loadDetail(id);
                });

                $('.btn-delete').off('click').on('click', function () {
                    var id = $(this).data('myid');
                    bootbox.confirm("Bạn có chắc chắn muốn xóa dữ liệu này không?", function (result) {
                        inCaseOfController.deleteItem(id);
                    });
                });
            });
        },
        registerEvent: function () {
            $('#frmSaveData').validate({
                rules: {
                    txtName: {
                        required: true,
                        minlength: 5
                    },
                    txtSalary: {
                        required: true,
                        number: true,
                        min: 0
                    }
                },
                messages: {
                    txtName: {
                        required: "Bạn phải nhập tên",
                        minlength: "Tên phải lớn hơn 5 ký tự"
                    },
                    txtSalary: {
                        required: "Bạn phải nhập lương",
                        number: "Lương phải là số",
                        min: "Lương của bạn phải lớn hơn hoặc bằng 0"
                    }
                }
            });
            $('.txtSalary').off('keypress').on('keypress', function (e) {
                if (e.which == 13) {
                    var id = $(this).data('id');
                    var value = $(this).val();

                    inCaseOfController.updateSalary(id, value);
                }
            });
            $('#txtNameS').off('keypress').on('keypress', function (e) {
                if (e.which == 13) {
                    inCaseOfController.loadData(true);
                }
            });

            $('#btnAddNew').off('click').on('click', function () {
                $('#modalAddUpdate').modal('show');
                inCaseOfController.resetForm();
            });

            $('#btnSave').off('click').on('click', function () {
                if ($('#frmSaveData').valid()) {
                    inCaseOfController.saveData();
                }
            });

            $('#btnSearch').off('click').on('click', function () {
                inCaseOfController.loadData(true);
            });
            $('#btnReset').off('click').on('click', function () {
                $('#txtNameS').val('');
                $('#ddlStatusS').val('');
                inCaseOfController.loadData(true);
            });
        },

        loadData: function () {
            var dataTable = $("#MyDataTable").DataTable({
                "ajax": {
                    "url": "/InCaseOf/LoadData",
                    "type": "GET",
                    "datatype": "json",
                    "asyn": "false"
                },
                "columns": [
                    { "data": "Code" },
                    { "data": "Name" },
                    {
                        "data": "CreatedDate", "name": "CreatedDate",
                        "render": function (data, type, row) {
                            if (data != null)
                                return (moment(row["CreatedDate"]).format("DD/MM/YYYY"));
                            else
                                return "";
                        }
                    },
                    {
                        "data": "UpdatedDate", "name": "UpdatedDate",
                        "render": function (data, type, row) {
                            if (data != null)
                                return (moment(row["UpdatedDate"]).format("DD/MM/YYYY"));
                            else
                                return "";
                        }
                    },
                    {
                        "data": "ID", "render": function (data) {
                            return "<a class='btn btn-primary btn-sm btn-edit' data-myid='" + data + "'><i class='fa fa-pencil'></i></a><a class='btn btn-danger btn-sm btn-delete' style='margin-left:5px' data-myid='" + data + "'><i class='fa fa-trash'></i></a>";
                            //return "<a class='btn btn-success btn-sm btn-edit' onclick=inCaseOfController.editData() data-id='" + data + "'><i class='fa fa-pencil'></i> Sửa</a><a class='btn btn-danger btn-sm btn-delete' style='margin-left:5px' onclick=Delete('" + data + "')><i class='fa fa-trash'></i> Xóa</a>";
                        },
                        "orderable": false,
                        "searchable": false,
                        "width": "150px"
                    }
                ],
                "language": {
                    url: '/localization/vi_VI.json'
                }
            });
        },
        loadDetail: function (id) {
            $.ajax({
                url: '/InCaseOf/GetDetail',
                data: {
                    id: id
                },
                type: 'GET',
                dataType: 'json',
                success: function (response) {
                    if (response.status == true) {
                        var data = response.data;
                        $('#hidID').val(data.ID);
                        $('#txtCode').val(data.Code);
                        $('#txtName').val(data.Name);
                    }
                    else {
                        //bootbox.alert(response.message);
                        $.notify(response.message, {
                            className: "warn"
                        });
                    }
                },
                error: function (err) {
                    console.log(err);
                    $.notify(err.Message, {
                        className: "error",
                        clickToHide: true
                    });
                }
            });
        },
        saveData: function () {
            var code = $('#txtCode').val();
            var name = $('#txtName').val();
            var id = parseInt($('#hidID').val());
            var senderObj = {
                Code: code,
                Name: name,
                ID: id
            }
            $.ajax({
                url: '/InCaseOf/SaveData',
                data: {
                    strSenderObj: JSON.stringify(senderObj)
                },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.status == true) {
                        //bootbox.alert("Save Success", function () {
                        //    $('#modalAddUpdate').modal('hide');
                        //    $('#MyDataTable').DataTable().ajax.reload();
                        //});

                        $.notify("Cập nhật dữ liệu thành công", "success");
                        $('#modalAddUpdate').modal('hide');
                        $('#MyDataTable').DataTable().ajax.reload();
                    }
                    else {
                        //bootbox.alert(response.message);
                        $.notify(response.message, {
                            className: "warn"
                        });
                    }
                },
                error: function (err) {
                    console.log(err);
                    $.notify(err.Message, {
                        className: "error",
                        clickToHide: true
                    });
                }
            });
        },
        resetForm: function () {
            $('#hidID').val('0');
            $('#txtCode').val('');
            $('#txtName').val('');
        },

        deleteItem: function (id) {
            $.ajax({
                url: '/InCaseOf/Delete',
                data: {
                    id: id
                },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.status == true) {
                        //bootbox.alert("Delete Success", function () {
                        //    $('#MyDataTable').DataTable().ajax.reload();
                        //});
                        $.notify("Xóa dữ liệu thành công", "success");
                        $('#MyDataTable').DataTable().ajax.reload();
                    }
                    else {
                        //bootbox.alert(response.message);
                        $.notify(response.message, {
                            className: "warn"
                        });
                    }
                },
                error: function (err) {
                    console.log(err);
                    $.notify(err.Message, {
                        className: "error",
                        clickToHide: true
                    });
                }
            });
        },

        updateField: function (id, value) {
            var data = {
                ID: id,
                Field: value
            };
            $.ajax({
                url: '/InCaseOf/UpdateField',
                data: { model: JSON.stringify(data) },
                type: 'POST',
                dataType: 'json',

                success: function (response) {
                    if (response.status) {
                        $.notify("Cập nhật dữ liệu thành công", "success");
                    }
                    else {
                        $.notify(response.message, {
                            className: "warn"
                        });
                    }
                },
                error: function (err) {
                    console.log(err);
                    $.notify(err.Message, {
                        className: "error",
                        clickToHide: true
                    });
                }
            })
        }
    }
    inCaseOfController.init();
});