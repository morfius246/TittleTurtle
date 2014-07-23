/// <reference path="site.core.js" />
/// <reference path="../lib/knockout-2.2.1.debug.js" />
/// <reference path="site.services.js" />

(function (site, ko) {

    "use strict";

    //#region Metadata

    ///////////////////////////////////////////////////////////////
    //  Metadata
    //  метадата (используются для определения настроек и 
    //  содеражния страницы (представления). Это вспомогательная
    //  сущность
    //  автор: calabonga.net
    //  зависит от:     knockout.js
    ///////////////////////////////////////////////////////////////

    site.controls.Metadata = function (title, description, helplink) {
        var metatitle = ko.observable(title),
            metadescription = ko.observable(description),
            metahelplink = ko.observable(helplink);
        function setTitle() { document.title = metatitle(); }
        metatitle.subscribe(function () { setTitle(); }); setTitle();
        return {
            title: metatitle,
            description: metadescription,
            helplink: metahelplink
        };
    };

    //#endregion

    //#region Clock

    ///////////////////////////////////////////////////////////////
    //  Clock
    //  часы (используются в основном, для проверки, что скрипт загружен)
    //  автор: calabonga.net
    //  Требуется для работы всех ViewModels
    //  зависит от:     knockout.js
    ///////////////////////////////////////////////////////////////

    site.controls.Clock = function () {
        var clock = ko.observable(),
            v1 = ko.observable(),
            v2 = ko.observable(),
            v3 = ko.observable(),
            getTime = function () {
                var ct = new Date();
                v1(ct.getHours());
                v2(c(ct.getMinutes()));
                v3(c(ct.getSeconds()));
                clock(v1() + ":" + v2() + ":" + v3());
            },
            c = function (i) {
                if (i < 10) {
                    i = "0" + i;
                }
                return i;
            };

        setInterval(function () { getTime(); }, 500);

        return {
            time: clock
        };
    };

    //#endregion

    //#region BusyIndicator

    ///////////////////////////////////////////////////////////////
    //  BusyIndicator
    //  Индикатор работы запроса
    //  автор: calabonga.net
    //  Требуется для работы ViewModels
    //  зависит от:     knockout.js
    ///////////////////////////////////////////////////////////////

    site.controls.BusyIndicator = function () {
        var ctrl = this;
        ctrl.uniqueId = guid();
        ctrl.isbusy = ko.observable(false);
        ctrl.imageName = site.cfg.busyIndicatorImageName || '/images/ms-loader.gif';
        ctrl.modalCss = '<style type="text/css">' +
            '.modalBusy {' +
            'position:absolute;' +
            'z-index:9998;' +
            'margin-left:0;' +
            'top:0;' +
            'left:0;' +
            'height:100%;' +
            'width:100%;' +
            'background:rgba(200,200,200,.5)url("' + ctrl.imageName + '") 50% 50% no-repeat;}' +
            '</style>';
        ctrl.ctrlTemplate = function () {
            var modalDiv = '<div id="block' + ctrl.uniqueId + '" ' + 'class="modalBusy">&nbsp;</div>';

            return modalDiv;
        };
        ctrl.show = function () { ctrl.isbusy(true); };
        ctrl.hide = function () { ctrl.isbusy(false); };
        ctrl.init = function () {
            if (!window.hasModelBlocker) {
                $("head").append(ctrl.modalCss);
                window.hasModelBlocker = true;
            }
            return;
        }();

        return {
            template: ctrl.ctrlTemplate,
            imageName: ctrl.imageName,
            uniqueId: ctrl.uniqueId,
            isbusy: ctrl.isbusy,
            show: ctrl.show,
            hide: ctrl.hide
        };
    };

    //#endregion 

    //#region QueryParams

    site.controls.QueryParams = function (options) {

        var settings = {
            index: 0,
            size: site.cfg.pageSize,
            groupSize: site.cfg.groupSize,
            total: 0
        };

        ko.utils.extend(settings, options);

        return {
            total: ko.observable(settings.total),
            index: ko.observable(settings.index),
            size: ko.observable(settings.size),
            groupSize: ko.observable(settings.groupSize)
        };
    };

    //#endregion

    //#region DataSource

    site.controls.DataSource = function (options, queryParams, aggr) {
        var prop, isObservable = false, srcVal = undefined, hasPages = ko.observable(false), hasItems = ko.observable(false),
            dataItems = ko.observableArray([]), pagesItems = ko.observableArray([]),
            item = null,
            currentItem = ko.observable(),
            _selectedItems = ko.observableArray([]),
            selectedItems = ko.computed({
                read: function () {
                    return _selectedItems;
                },
                write: function (newValue) {
                    _selectedItems = newValue;
                    if (newValue && newValue.length > 0) {
                        unselect();
                        site._.each(newValue, function (it) {
                            it.selected(true);
                        });
                        if (settings.events.selectedHandler)
                            settings.events.selectedHandler(newValue[newValue.length - 1]);
                    }
                }
            }),
            isSelectedItemChanged = false,
            isSelectMethodFired = false,
            aggregate = aggr || {},
            indicator = new site.controls.BusyIndicator(),
            settings = {
                service: null,
                items: null,
                entityName: "",
                autoLoad: true,
                pager: {
                    prev: { text: "«", css: "" },
                    current: { css: "active" },
                    next: { text: "»", css: "" }
                },
                events: {
                    selectedHandler: function (json) { },
                    getCompleteHandler: function () { },
                    postCompleteHandler: function (json) { },
                    putCompleteHandler: function (json) { },
                    deleteCompleteHandler: function (json) { }
                }
            },
            generate = function () {
                var currentIndex = defaultParams.index() ? defaultParams.index() : 0,
                    pages = [],
                    totalItems = defaultParams.total(),
                    totalPages, currentGroup,
                    totalGroups,
                    pageSize = defaultParams.size() ? defaultParams.size() : 10,
                    groupSize = defaultParams.groupSize() ? defaultParams.groupSize() : 10;
                pagesItems([]);
                if (totalItems > 0) {
                    totalPages = Math.ceil(totalItems / pageSize);
                    totalGroups = Math.ceil(totalPages / groupSize);
                    currentGroup = Math.floor(currentIndex / groupSize);
                    var minPage = currentGroup * groupSize, maxPage = minPage + groupSize;
                    if (maxPage > totalPages) { maxPage = totalPages; }
                    if (currentGroup > 0) { pages.push(new pagerItem(minPage - 1, settings.pager.prev.text, settings.pager.prev.css)); }
                    for (var i = minPage; i < maxPage; i++) { var css = currentIndex == i ? settings.pager.current.css : ""; pages.push(new pagerItem(i, (i + 1), css)); }
                    if (currentGroup < totalGroups - 1) { pages.push(new pagerItem(maxPage, settings.pager.next.text, settings.pager.next.css)); }
                    if (pages.length > 0) { pagesItems(pages); }
                    hasPages(totalPages > 1);
                    hasItems(true);
                } else {
                    hasPages(false);
                    hasItems(false);
                }
            },

            //#region getData
            getData = function (i) {
                hasPages(false);
                hasItems(false);
                if (i && typeof i === "object") { if (i.number) { defaultParams.index(i.number); } else { defaultParams.index(0); } }
                indicator.isbusy(true);
                if (!settings.service) { throw new Error(settings.entityName + " DataSource: Service instance not found"); }
                settings.service.getData(defaultParams, getDataComplete, aggregate);
            },
            getDataComplete = function (json, a) {
                if (a) aggregate = a;
                if (json) {
                    dataItems(json);
                    generate();
                    unselect();
                }
                if (settings.events.getCompleteHandler) settings.events.getCompleteHandler();
                indicator.isbusy(false);
            },
            //#endregion

            //#region postData

            postData = function (json, complete) {
                indicator.isbusy(true);
                settings.service.postData(json, postDataComplete.bind(complete), aggregate);
            },
            postDataComplete = function (json, a) {
                if (a) aggregate = a;
                if (json) {
                    dataItems.push(json);
                    var t = defaultParams.total();
                    defaultParams.total(t + 1);
                    select(json);
                }
                ;
                indicator.isbusy(false);
                var complete = this;
                if (site._.isFunction(complete)) {
                    if (json) complete(true);
                    else complete(false);
                }
                if (settings.events.postCompleteHandler) {
                    settings.events.postCompleteHandler(json);
                }
            },
            append = function (json) {
                dataItems.push(json);
            },
            //#endregion

            //#region putData
            putData = function (json, complete) {
                indicator.isbusy(true);
                settings.service.putData(json, putDataComplete.bind(complete), aggregate);
            },
            putDataComplete = function (json, a) {
                if (a) aggregate = a;
                indicator.isbusy(false);
                var complete = this;
                if (site._.isFunction(complete)) {
                    if (json) complete(json);
                    else complete();
                }
                if (settings.events.putCompleteHandler) {
                    settings.events.putCompleteHandler(json);
                }
            },
            //#endregion

            //#region delData
                delData = function (complete) {
                    indicator.isbusy(true);
                    settings.service.delData(ko.toJS(currentItem()), delDataComplete.bind(complete), aggregate);
                },
                delDataComplete = function (json, a) {
                    if (a) aggregate = a;
                    if (json) {
                        dataItems.remove(currentItem());
                        //currentItem(null);
                        unselect();
                    }
                    var t = defaultParams.total();
                    defaultParams.total(t - 1);
                    indicator.isbusy(false);
                    var complete = this;
                    if (site._.isFunction(complete)) {
                        complete();
                    }
                    if (settings.events.deleteCompleteHandler) {
                        settings.events.deleteCompleteHandler(json);
                    }
                },
                remove = function (obj) {
                    dataItems.remove(obj);
                },
                //#endregion

            //#region select
            select = function (json) {
                if (isSelectedItemChanged || !isSelectMethodFired) {
                    if (json) {
                        if (item != json) {
                            unselect();
                            json.selected(true);
                            item = json;
                            isSelectedItemChanged = false;
                            currentItem(json);

                            isSelectedItemChanged = true;
                            if (settings.events.selectedHandler) {
                                settings.events.selectedHandler(json);
                            }
                        }
                        isSelectMethodFired = true;
                    }
                }
                //console.log("select method fired!" + Date());
            },
            unselect = function () {
                ko.utils.arrayForEach(dataItems(), function (it) {
                    it.selected(false);
                    if (it.children) {
                        it.unselect(it.children);
                    }
                });
                item = null;
                currentItem(null);
            },
            //#endregion

            //#region reload currentItem

            reload = function (id) {
                indicator.isbusy(true);
                settings.service.getDataById(id, reloadComplete, aggregate);
            },
            reloadComplete = function (json) {
                indicator.isbusy(false);
                if (json) {
                    var index = site._.indexOf(dataItems(), currentItem(), false);
                    dataItems.replace(dataItems()[index], json);
                }
            },

            //#endregion

            //#region clear

            clear = function () {
                dataItems([]);
                pagesItems([]);
                hasPages(false);
                hasItems(false);
            },

            //#endregion

            pagerItem = function (number, name, css) {
                return {
                    number: number,
                    name: name,
                    css: css
                };
            },

            defaultParams = site.controls.QueryParams();

        //#region extend properties and check
        $.extend(true, settings, options);

        for (prop in queryParams) {
            if (!queryParams.hasOwnProperty(prop)) { continue; }
            if (ko.isWriteableObservable(queryParams[prop])) {
                isObservable = true; srcVal = queryParams[prop]();
            } else if (typeof (queryParams[prop]) !== 'function') {
                srcVal = queryParams[prop];
            }
            if (ko.isWriteableObservable(defaultParams[prop])) {
                defaultParams[prop](srcVal);
            } else if (defaultParams[prop] === null || defaultParams[prop] === undefined) {
                defaultParams[prop] = isObservable ? ko.observable(srcVal) : srcVal;
            } else if (typeof (defaultParams[prop]) !== 'function') {
                defaultParams[prop] = srcVal;
            }
            isObservable = false;
        }

        //#endregion

        //#region autoLoad
        if (settings.service && settings.autoLoad) { getData(); }
        //#endregion

        //#region autoLoad
        if (!settings.service && settings.items) { dataItems(settings.items); }
        //#endregion

        defaultParams.size.subscribe(function (size) {
            defaultParams.size(size);
            defaultParams.index(0);
            getData(0);
        });

        return {
            selectedItems: selectedItems,
            resetSelectedItem: unselect,
            queryParams: defaultParams,
            execute: settings.service,
            currentItem: currentItem,
            events: settings.events,
            aggregate: aggregate,
            indicator: indicator,
            hasPages: hasPages,
            hasItems: hasItems,
            postData: postData,
            pages: pagesItems,
            items: dataItems,
            getData: getData,
            putData: putData,
            delData: delData,
            select: select,
            remove: remove,
            append: append,
            reload: reload,
            clear: clear
        };
    };

    //#endregion 

    //#region Pager
    ///////////////////////////////////////////////////////////////
    //  Pager
    //  пейджер для коллекций
    //  автор: calabonga.net
    //  Требуется для работы всех ViewModels
    //  зависит от:     knockout.js
    ///////////////////////////////////////////////////////////////

    site.controls.Pager = function (ds, options) {
        var settings = {
            prev: { text: "<<", css: "prev" },
            current: { css: "active" },
            next: { text: ">>", css: "next" }
        };

        ko.utils.extend(settings, options);

        var
            isEnabled = ko.observable(false),
            pagerItem = function (number, name, css) {
                return {
                    number: number,
                    name: name,
                    css: css
                };
            },
            pagesItems = ko.observableArray([]),
            select = function () {
                ds.load(this.number);
            },
            generate = function () {
                var currentIndex = ds.queryParams.index(), pages = [], totalItems = ds.queryParams.total(), totalPages, currentGroup, totalGroups, pageSize = ds.queryParams.size(), groupSize = ds.queryParams.groupSize();
                pagesItems([]);
                if (totalItems > 0) {
                    totalPages = Math.ceil(totalItems / pageSize);
                    totalGroups = Math.ceil(totalPages / groupSize);
                    currentGroup = Math.floor(currentIndex / groupSize);
                    var minPage = currentGroup * groupSize, maxPage = minPage + groupSize;
                    if (maxPage > totalPages) {
                        maxPage = totalPages;
                    }
                    if (currentGroup > 0) {
                        pages.push(new pagerItem(minPage - 1, settings.prev.text, settings.prev.css));
                    }
                    for (var i = minPage; i < maxPage; i++) {
                        var css = currentIndex == i ? settings.current.css : "";
                        pages.push(new pagerItem(i, (i + 1), css));
                    }
                    if (currentGroup < totalGroups - 1) {
                        pages.push(new pagerItem(maxPage, settings.next.text, settings.next.css));
                    }
                    if (pages.length > 0) {
                        pagesItems(pages);
                    }
                    isEnabled(totalPages > 1);
                }
            };

        ds.queryParams.total.subscribe(function () {
            generate();
        });
        ds.queryParams.index.subscribe(function () {
            generate();
        });
        return {
            isEnabled: isEnabled,
            pages: pagesItems,
            select: select
        };
    };

    //#endregion 

    //#region Unilites
    function guid() {
        return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
    };
    var stringTemplate = function (key, template) {
        if (arguments.length === 1) {
            this.template = key;
        } else {
            this.templateName = key;
            this.template = template;
        }
    };
    stringTemplate.prototype.text = function () {
        return this.template;
    };
    var stringTemplateEngine = new ko.nativeTemplateEngine();
    stringTemplateEngine.makeTemplateSource = function (templateName) {
        return new stringTemplate(templateName);
    };

    //#endregion

})(site, ko);