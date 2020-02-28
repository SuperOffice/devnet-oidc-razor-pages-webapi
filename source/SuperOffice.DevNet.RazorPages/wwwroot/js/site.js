// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


const DevNet = function () {
    /// <summary>DevNet object</summary>
    var pub = {};

    this.emptyFunc = function () {

    };

    $(document).ready(function () {
        pub.init();
        console.log("DevNet1 init!");
    });


    pub.init = function () {
        $(window).on('load', function () {
            console.log("DevNet window loaded!");
        });

        $(window).on('unload', function () {
            console.log("DevNet window unloaded!");
        });
    };

    pub.getTenantCtx = function (args) {
        let link = "/Account/Login?handler=Login";

        var url = (window.location !== window.parent.location)
            ? document.referrer
            : document.location.href;

        var pathArray = url.split('/').filter(x => x);

        for (var i = 0; i < pathArray.length; i++) {
            console.log("Part " + i.toString() + " " + pathArray[i]);
        }

        if (pathArray && pathArray.length >= 3)
            return link + "&ctx=" + pathArray[2];

        return link;
    };

    return pub;
}();



