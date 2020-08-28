var contextMenuDataInfoController = {

    init: function () {
        contextMenuDataInfoController.registerEventDataTable();
    },
    registerEventDataTable: function () {
        $("#MyDataTable").contextmenu({
            autoFocus: true,
            preventContextMenuForPopup: true,
            preventSelect: true,
            taphold: true,
            delegate: ".mainRow",
            menu: [
                {
                    title: "Chi tiết", cmd: myConstant.Action_Detail, uiIcon: "ui-icon-folder-open",
                    disabled: function (event, ui) {
                        // return `true` to disable, `"hide"` to remove entry:
                        if ($.inArray(myConstant.Data_CanViewDetail_Role, myArrayRoles) > -1) {
                            return false;
                        }
                        return true;
                    }
                },
                {
                    title: "Sửa đổi", cmd: myConstant.Action_Edit, uiIcon: "ui-icon-pencil",
                    disabled: function (event, ui) {
                        // return `true` to disable, `"hide"` to remove entry:
                        if ($.inArray(myConstant.Data_CanEdit_Role, myArrayRoles) > -1) {
                            return false;
                        }
                        return true;
                    }
                },
                {
                    title: "Xóa", cmd: myConstant.Action_Delete, uiIcon: "ui-icon-trash",
                    disabled: function (event, ui) {
                        // return `true` to disable, `"hide"` to remove entry:
                        if (typeof applicationUserProfileController == 'undefined') {
                            if ($.inArray(myConstant.Data_CanDelete_Role, myArrayRoles) > -1) {
                                return false;
                            }
                            return true;
                        } else {
                            return 'hide';
                        }
                    }
                },
                {
                    title: "Thu hồi/Hủy bỏ GCNKĐ", cmd: myConstant.Action_Cancel, uiIcon: "ui-icon-cancel",
                    disabled: function (event, ui) {
                        // return `true` to disable, `"hide"` to remove entry:
                        if (typeof homeController !== 'undefined' || typeof certificateController !== 'undefined') {
                            if ($.inArray(myConstant.Data_CanCancel_Role, myArrayRoles) > -1) {
                                return false;
                            }
                            return true;
                        } else {
                            return 'hide';
                        }
                    }
                },
                { title: "----" },
                {
                    title: "Xem trên bản đồ", cmd: myConstant.Action_ViewMap, uiIcon: " ui-icon-circle-zoomin",
                    disabled: function (event, ui) {
                        // return `true` to disable, `"hide"` to remove entry:
                        if (typeof homeController !== 'undefined' || typeof certificateController !== 'undefined' || typeof noCertificateController !== 'undefined') {
                            if ($.inArray(myConstant.Info_CanViewMap_Role, myArrayRoles) > -1)
                                return false;
                            else
                                return true;
                        } else {
                            return 'hide';
                        }
                    }
                },
                {
                    title: "In Giấy CNKĐ", cmd: myConstant.Action_Print, uiIcon: "ui-icon-print",
                    disabled: function (event, ui) {
                        // return `true` to disable, `"hide"` to remove entry:
                        if (typeof homeController !== 'undefined' || typeof certificateController !== 'undefined' ) {
                            if ($.inArray(myConstant.Info_CanPrintCertificate_Role, myArrayRoles) > -1)
                                return false;
                            else
                                return true;
                        } else {
                            return 'hide';
                        }
                    }
                },
                //{ title: "Edit <kbd>[F2]</kbd>", cmd: "edit", uiIcon: "ui-icon-pencil" },
                //{
                //    title: "More", children: [
                //        {
                //            title: "Use an 'action' callback", action: function (event, ui) {
                //                alert("action callback sub1");
                //            }
                //        },
                //        { title: "Tooltip (static)", cmd: "sub2", tooltip: "Static tooltip" },
                //        { title: "Tooltip (dynamic)", tooltip: function (event, ui) { return "" + Date(); } },
                //        { title: "Custom icon", cmd: "browser", uiIcon: "ui-icon custom-icon-firefox" },
                //        {
                //            title: "Disabled (dynamic)", disabled: function (event, ui) {
                //                return false;
                //            }
                //        }
                //    ]
                //}
            ],
            //Implement the beforeOpen callback to dynamically change the entries
            beforeOpen: function (event, ui) {
                var $menu = ui.menu,
                    $target = ui.target,
                    extraData = ui.extraData; // passed when menu was opened by call to open()

                // console.log("beforeOpen", event, ui, event.originalEvent.type);

                //$("#MyDataTable")
                //    .contextmenu("replaceMenu", [{ title: "aaa" }, { title: "bbb" }])
                //    .contextmenu("replaceMenu", "#options2")
                //    .contextmenu("setEntry", "cut", { title: "Cuty", uiIcon: "ui-icon-heart", disabled: true })
                //    .contextmenu("setEntry", "copy", "Copy '" + $target.text() + "'")
                //    .contextmenu("setEntry", "paste", "Paste" + (CLIPBOARD ? " '" + CLIPBOARD + "'" : ""))
                //    .contextmenu("enableEntry", "paste", (CLIPBOARD !== ""));

                // Optionally return false, to prevent opening the menu now
            },
            select: function (event, ui) {
                $('#MyDataTable tbody tr').on('click', addinController.doContextAction(ui.cmd, ui.target.closest('tr').data('id'), ui.target.closest('tr').data('long'), ui.target.closest('tr').data('lat')));
                //var m = "clicked: " + ui.target.text();
                //window.console && console.log(m) || alert(m);
                return true;
            },
        })
    },

    registerEvent: function () {
    },
};
contextMenuDataInfoController.init();