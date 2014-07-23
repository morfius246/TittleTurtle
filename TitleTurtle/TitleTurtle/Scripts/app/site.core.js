//------------------------------------------------------------
// Just a sample to start coding HTML5
// Author: Calabonga
// site: www.calabonga.net
//--------------------------------------------------------------

/// <reference path="jquery-1.8.3.js" />
/// <reference path="site.core.js" />
/// <reference path="knockout-2.2.1.debug.js" />
/// <reference path="knockout.mapping-latest.debug.js" />
/// <reference path="toastr.js" />

// base namespace
var site = site || {};

// config module
site.cfg = site.cfg || {};

// model's module 
site.m = site.m || {};

// viewmodel's module
site.vm = site.vm || {};

// services module
site.services = site.services || {};

// utilites module
site.utils = site.utils || {};

// controls module
site.controls = site.controls || {};

// start engine
var bootstrapper = function () {

    var root = this,
        initLibs = function () {

            // initialization for third-party libs
            site.amplify = root.amplify;
            site.$ = root.jQuery;
            site.logger = root.toastr;
            site._ = root._;
        },
        initConfig = function () {

            // settings for site
            site.cfg.throttle = 600;
            site.cfg.busyIndicatorImageName = "/images/ms-loader.gif";
        },
        init = function () {
            initConfig();
            initLibs();
        };

    return {
        run: init
    };

}();

bootstrapper.run();