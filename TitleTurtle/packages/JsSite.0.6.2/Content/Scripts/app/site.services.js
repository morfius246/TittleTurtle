/// <reference path="../lib/knockout-2.2.1.debug.js" />
/// <reference path="../lib/knockout.mapping-latest.debug.js" />
/// <reference path="../lib/amplify.js" />
/// <reference path="site.m.mappings.js" />
/// <reference path="site.core.js" />

///////////////////////////////////////////////////////////////
//  just for demo
//  author: calabonga.net
///////////////////////////////////////////////////////////////

/*
------------------------
--- THIS JUST A DEMO ---
------------------------
(function (site) {

    site.services.documents = function () {
        var
            init = function () {
                site.amplify.request.define("getdocument", "ajax", {
                    url: "/api/documentapi",
                    dataType: "json",
                    type: "GET",
                    cache: false
                });
                site.amplify.request.define("postdocument", "ajax", {
                    url: "/api/documentapi",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    cache: false
                });
                site.amplify.request.define("putdocument", "ajax", {
                    url: "/api/documentapi",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "PUT",
                    cache: false
                });
                site.amplify.request.define("deldocument", "ajax", {
                    url: "/api/documentapi",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "DELETE",
                    cache: false
                });
            },
            mapItem = function (data) {
                return new site.m.Document(data);
            },
            mapItems = function (data) {
                var mapped = [];
                site._.each(data, function (item) {
                    mapped.push(mapItem(item));
                });
                return mapped;
            },
            getData = function (params, back) {
                if (typeof back !== "function") throw new Error("callback not a function");
                if (!params) throw new Error("queryParams notis null");
                return site.amplify.request({
                    resourceId: "getdocument",
                    data: { qp: ko.toJSON(params) },
                    success: function (json) {
                        if (json) {
                            if (json.success) {
                                params.total(json.total);
                                var result = mapItems(json.items);
                                back(result);
                                return;
                            }
                            if (json.warning) {
                                site.logger.warning(json.warning);
                            }
                            if (json.error) {
                                site.logger.error(json.error);
                            }
                        }
                        back();
                    },
                    error: function () {
                        site.logger.error("Ошибка загрузки сущности \"Документ\" (method \"get\") Document");
                        back();
                    }
                });
            },
            getDataById = function (params, back) {
                if (typeof back !== "function") throw new Error("callback not a function");
                if (!params) throw new Error("queryParams notis null");
                return site.amplify.request({
                    resourceId: "getdocument",
                    data: { id: params },
                    success: function (json) {
                        if (json) {
                            if (json.success) {
                                var result = mapItem(json.item);
                                back(result);
                                return;
                            }
                            if (json.warning) {
                                site.logger.warning(json.warning);
                            }
                            if (json.error) {
                                site.logger.error(json.error);
                            }
                        }
                        back();
                    },
                    error: function () {
                        site.logger.error("Ошибка загрузки сущности \"Должность\" (method \"get\") JobTitle");
                        back();
                    }
                });
            },
            postData = function (params, back) {
                if (typeof back !== "function") throw new Error("callback not a function");
                return site.amplify.request({
                    resourceId: "postdocument",
                    data: ko.toJSON(params),
                    success: function (json) {
                        if (json) {
                            if (json.success) {
                                site.logger.success(json.success);
                                back(new mapItem(json.item));
                                return;
                            }
                            if (json.warning) {
                                site.logger.warning(json.warning);
                            }
                            if (json.error) {
                                site.logger.error(json.error);
                            }
                        }
                        back();
                    },
                    error: function () {
                        site.logger.error("Ошибка сохранения сущности \"Документ\" (method \"post\") Document");
                        back();
                    }
                });
            },
            putData = function (params, back) {
                if (typeof back !== "function") throw new Error("callback not a function");
                return site.amplify.request({
                    resourceId: "putdocument",
                    data: ko.toJSON(params),
                    success: function (json) {
                        if (json) {
                            if (json.success) {
                                site.logger.success(json.success);
                                back(new mapItem(json.item));
                                return;
                            }
                            if (json.warning) {
                                site.logger.warning(json.warning);
                            }
                            if (json.error) {
                                site.logger.error(json.error);
                            }
                        }
                        back();
                    },
                    error: function () {
                        site.logger.error("Ошибка обновления сущности \"Документ\" (method \"put\") Document");
                        back();
                    }
                });
            },
            delData = function (params, back) {
                if (typeof back !== "function") throw new Error("callback not a function");
                return site.amplify.request({
                    resourceId: "deldocument",
                    data: ko.toJSON(params),
                    success: function (json) {
                        if (json) {
                            if (json.success) {
                                site.logger.success(json.success);
                                back(new mapItem(json.item));
                                return;
                            }
                            if (json.warning) {
                                site.logger.warning(json.warning);
                            }
                            if (json.error) {
                                site.logger.error(json.error);
                            }
                        }
                        back();
                    },
                    error: function () {
                        site.logger.error("Ошибка удаления сущности \"Документ\" (method \"del\") Document");
                        back();
                    }
                });
            };

        init();

        return {
            getDataById: getDataById,
            postData: postData,
            getData: getData,
            putData: putData,
            delData: delData
        };
    }();

})(site);

*/