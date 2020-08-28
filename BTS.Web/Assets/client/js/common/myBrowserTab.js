$(document).ready(function () {
    window.addEventListener('load', tabLoadEventHandler);
    window.addEventListener('beforeunload', tabBeforeUnloadEventHandler);
    window.addEventListener('unload', tabUnloadEventHandler);
    
});

function tabLoadEventHandler() {
    console.log("tabLoadEventHandler");
    if (myNotAuthenticated == false) {
        let hash = 'tab_' + +new Date();
        sessionStorage.setItem('TabHash', hash);
        let tabs = JSON.parse(localStorage.getItem('TabsOpen') || '{}');
        tabs[hash] = true;
        localStorage.setItem('TabsOpen', JSON.stringify(tabs));
    }
}

function tabBeforeUnloadEventHandler() {
    console.log("tabBeforeUnloadEventHandler");
    if (myNotAuthenticated == false) {
        let hash = sessionStorage.getItem('TabHash');
        let tabs = JSON.parse(localStorage.getItem('TabsOpen') || '{}');
        delete tabs[hash];
        localStorage.setItem('TabsOpen', JSON.stringify(tabs));
    }
}

function tabUnloadEventHandler() {
    if (Object.keys(JSON.parse(localStorage.getItem('TabsOpen') || '{}')).length == 0) {
        $.ajax({
            url: '/Account/SignOut',
            type: 'POST',
            success: function (response) {
                $.notify(response.message, {
                    className: "warn"
                });
            },
            error: function (err) {
                console.log(err);
                $.notify(err, {
                    className: "error",
                    clickToHide: true
                });
            }
        });
    }
    console.log("tabUnloadEventHandler");
} 