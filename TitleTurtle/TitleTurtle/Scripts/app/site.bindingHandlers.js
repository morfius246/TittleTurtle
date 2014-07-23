
(function (ko) {

	"use strict";

	//#region Selector

	///////////////////////////////////////////////////////////////
	//  Утилиты и расширения для knockoutjs
	//  bindingHandlers для отображения модального окна (bootstrap)
	//  для выбора сущности 
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  controls.DataSource
	//                  infuser
	//                  bootstrap
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.dblookup = {
		init: function (element, valueAccessor, allBindingAccessor) {
			var observable = valueAccessor(),
				uniqueId = guid(), modalId = '#' + uniqueId,
				lookups = allBindingAccessor().lookups || {},
				defaultFieldTemplate = lookups.fieldTemplate && window.infuser.getSync(lookups.fieldTemplate),
				defaultModalTemplate = lookups.modalTemplate && window.infuser.getSync(lookups.modalTemplate),
				modalWindowTemplate = defaultModalTemplate || '<div id="' + uniqueId + '" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
											<div class="modal-header">\
												<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>\
												<h3 data-bind="text: title"></h3>\
											</div>\
											<div class="modal-body">\
											</div>\
											<div class="modal-footer">\
												<button class="btn btn-primary" data-bind="enable: canSelect, click: select">выбрать</button>\
												<button class="btn" data-dismiss="modal" aria-hidden="true">закрыть</button>\
											</div>\
										</div>',
				dataSourceTemplate = lookups.dsTemplate && window.infuser.getSync(lookups.dsTemplate),
				dataSource = lookups.dataSource,
				previewElement = document.createElement('div');

			if (!dataSource) throw 'DataSource not found!';
			if (!dataSourceTemplate) throw 'DataSourceTemplate not found!';
			if (!defaultFieldTemplate) throw 'DataSourceTemplate not found!';
			dataSource.resetSelectedItem();

			var openSelectorHandler = function () {
				var model = {
					title: 'Выбор из списка',
					canSelect: ko.computed(function () {
						return dataSource && dataSource.currentItem();
					}),
					select: function () {
						var selectedItem = dataSource.currentItem();
						observable(selectedItem);
						$(modalId).modal('hide');
					},
					cancel: function () {

					}
				};
				var div = document.createElement('div');
				ko.renderTemplate(modalWindowTemplate, model, { templateEngine: stringTemplateEngine }, div, null);
				var table = $('.modal-body', div)[0];

				ko.renderTemplate(dataSourceTemplate, dataSource, { templateEngine: stringTemplateEngine }, table, null);
				$('body').append(div);
				$(modalId).modal({ keyboard: true }).on('hidden', function () {
					return;
				});

				ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
					var el = $(modalId)[0];
					ko.removeNode(el);
				});
			};

			var previewModel = {
				model: observable,
				openSelector: openSelectorHandler
			};

			ko.renderTemplate(defaultFieldTemplate, previewModel, { templateEngine: stringTemplateEngine }, previewElement, null);
			$(element).replaceWith(previewElement);
		}
	};

	//#endregion

	//#region RadioBox Button

	///////////////////////////////////////////////////////////////
	//  Утилиты и расширения для knockoutjs
	//  bindingHandlers для отображения RadioButton (bootstrap)
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  bootstrap
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.radio = {
		init: function (element, valueAccessor, allBindings, data, context) {
			var $buttons, $element, elementBindings = allBindings(), observable;
			observable = valueAccessor();
			if (!ko.isWriteableObservable(observable)) {
				throw "You must pass an observable or writeable computed";
			}
			$element = $(element);
			if ($element.hasClass("btn")) {
				$buttons = $element;
			} else {
				$buttons = $(".btn", $element);
			};
			$buttons.each(function () {
				var $btn, btn, radioValue;
				btn = this;
				$btn = $(btn);
				radioValue = elementBindings.radioValue || $btn.attr("data-value") || $btn.attr("value") || $btn.text();
				$btn.on("click", function () {
					observable(ko.utils.unwrapObservable(radioValue));
				});
				return ko.computed({
					disposeWhenNodeIsRemoved: btn,
					read: function () {
						$btn.toggleClass("active", observable() === ko.utils.unwrapObservable(radioValue));
					}
				});
			});
		}
	};

	//#endregion

	//#region CheckBox Button

	///////////////////////////////////////////////////////////////
	//  Утилиты и расширения для knockoutjs
	//  bindingHandlers для отображения Checkbox (bootstrap)
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  bootstrap
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.checkbox = {
		init: function (element, valueAccessor, allBindings, data, context) {
			var $element, observable;
			observable = valueAccessor();
			if (!ko.isWriteableObservable(observable)) {
				throw "You must pass an observable or writeable computed";
			}
			$element = $(element);
			$element.on("click", function () {
				observable(!observable());
			});
			ko.computed({
				disposeWhenNodeIsRemoved: element,
				read: function () {
					$element.toggleClass("active", observable());
				}
			});
		}
	};

	//#endregion

	//#region hidden

	///////////////////////////////////////////////////////////////
	//  Утилиты расширения knockoutjs
	//  bindingHandlers для сокрытия объекта 
	//  на основании противосостояния
	//  автор: calabonga.net
	//  зависит от:     knockout
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.hidden = {
		update: function (element, valueAccessor) {
			var value = ko.utils.unwrapObservable(valueAccessor());
			ko.bindingHandlers.visible.update(element, function () { return !value; });
		}
	};

	//#endregion

	//#region  ckeditor

	///////////////////////////////////////////////////////////////
	//  Утилиты
	//  Расширение knockoutjs
	//  bindingHandlers для редактирования HTML-кода
	//  автор: calabonga.net & Шлёнов Дмитрий
	//  зависит от:     knockout
	//                  moment
	//                  blockUI
	///////////////////////////////////////////////////////////////
	ko.bindingHandlers.ckeditor = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var
				ckOptions = {
					uiColor: '#577AB5',
					height: 400,
					extraPlugins: 'onchange'
				},
				value = valueAccessor(),
				allBindings = ko.utils.unwrapObservable(allBindingsAccessor()),
				options = ko.utils.extend(allBindings.options || ckOptions);
			if (!value) {
				value = ko.observable();
			}

			var editor = CKEDITOR.replace(element, options);
			var editorName = editor.name;
			$(element).attr("data-customname", editorName);
			editor.on('change', function (e) {
				var data = editor.getData();
				if (data) {
					if (ko.isWriteableObservable(value)) {
						value(data);
					} else {
						value = data;
					}
				}
			});

			editor.on('blur', function () {
				var self = this;
				if (ko.isWriteableObservable(self)) {
					var data = editor.getData();
					if (data) {
						value(data);
					}
				}
			}, value, element);

			ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
				var editorName = $(element).attr("data-customname");
				var editor = CKEDITOR.instances[editorName];
				editor.destroy(true);
			});
		},

		update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var value = ko.utils.unwrapObservable(valueAccessor());
			$(element).html(value);
		}
	};

	//#endregion

	//#region  editableText

	///////////////////////////////////////////////////////////////
	//  Утилиты
	//  Расширение knockoutjs
	//  bindingHandlers для редактирования единочного поля 
	//  конкретной сущности
	//  параметр: action - delegate получаеющий как параметр редактируемый
	//  объект с обовленным свойством
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  moment
	//                  blockUI
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.editableText = {
		init: function (element, valueAccessor, allBindingAccessor) {
			var observable = valueAccessor(),
				span = document.createElement("span"),
				input = document.createElement("input"),
				otherOptions = allBindingAccessor().params || {},
				action = allBindingAccessor().action;

			var data = ko.dataFor(element);
			data.options = otherOptions;
			element.appendChild(span);
			element.appendChild(input);
			input.onblur = function () {
				if (action) action.call(this, data);
				$(span).show();
			};
			observable.editing = ko.observable(false);
			ko.applyBindingsToNode(span, {
				text: observable,
				visible: !ko.utils.unwrapObservable(observable.editing),
				click: function () {
					observable.editing(true);
					$(span).hide();
				}
			});
			ko.applyBindingsToNode(input, {
				value: observable,
				visible: observable.editing,
				hasfocus: observable.editing
			});
		}
	};

	//#endregion

	//#region  blockUI

	///////////////////////////////////////////////////////////////
	//  Утилиты
	//  Расширение knockoutjs
	//  bindingHandlers для блокировки DIV
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  moment
	//                  blockUI
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.blockUI = {
		init: function (element, valueAccessor) {
			var value = valueAccessor(),
				ctrl = ko.utils.unwrapObservable(value);
			$(element).css('position', 'relative');
			$(element).css('min-height', '70px');
			ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
				var el = $("#block" + ctrl.uniqueId)[0];
				if (el) ko.removeNode(el);
			});
		},
		update: function (element, valueAccessor, allBindingAccessor) {
			var value = valueAccessor(),
				ctrl = ko.utils.unwrapObservable(value);
			var el;
			if (ctrl.isbusy()) {
				if (ctrl && ctrl.template) {
					var block = ctrl.template(element);
					$(element).append(block);
				}
			} else {
				el = $("#block" + ctrl.uniqueId)[0];
				if (el) ko.removeNode(el);
			}
		}
	};

	//#endregion

	//#region progressbar

	ko.bindingHandlers.progressbar = {
		init: function (element, valueAccessor) {
			var options = valueAccessor() || {};
			$(element).progressbar(options);
			ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
				$(element).progressbar("destroy");
			});
		},
		update: function (element, valueAccessor) {
			var value = valueAccessor();
			$(element).progressbar("value", parseInt(value.value));
		}
	};

	//#endregion

	//#region jqDialog

	ko.bindingHandlers.jqDialog = {
		init: function (element, valueAccessor) {
			var model = ko.utils.unwrapObservable(valueAccessor()),
				options = ko.utils.extend(model.options || {}, ko.bindingHandlers.jqDialog.defaultOptions);

			//initialize the dialog
			$(element).dialog(options);

			//handle disposal
			ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
				$(element).dialog("destroy");
			});
		},
		update: function (element, valueAccessor) {
			var value = ko.utils.unwrapObservable(valueAccessor());
			$(element).dialog(ko.utils.unwrapObservable(value.open) ? "open" : "close");

			if (value.title) {
				var title = value.title();
				if (title) {
					$(element).dialog("option", "title", title);
				}
			}
			//handle positioning
			if (value.position) {
				var target = value.position();
				if (target) {
					var pos = $(target).position();
					$(element).dialog("option", "position", [pos.left + $(target).width(), pos.top + $(target).height()]);
				}
			}
		},
		defaultOptions: {
			options: {
				buttons: {}
			},
			autoOpen: false,
			resizable: false,
			modal: true
		}
	};

	//#endregion

	//#region jqDialogContext

	///////////////////////////////////////////////////////////////
	//  Утилиты
	//  Расширение knockoutjs
	//  bindingHandlers для модального окна
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  moment
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.jqDialogContext = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
			// alert("initing");
			var model = ko.utils.unwrapObservable(valueAccessor()),
				options = ko.utils.extend(model.options || {}, ko.bindingHandlers.jqDialogContext.defaultContextOptions);

			if (!options.isInitialized) {
				//setup our buttons
				options.buttons = {
					"Готово": model.accept.bind(viewModel, viewModel),
					"Отмена": model.cancel.bind(viewModel, viewModel)
				};
				options.closeOnEscape = false;

				//handle disposal
				ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
					$(element).dialog("destroy");
				});
				$(element).append("<span>\\текст/</span>");

				//initialize the dialog
				$(element).dialog(options);

				options.isInitialized = true;

			}
			return {
				"controlsDescendantBindings": true
			};
		},
		update: function (element, valueAccessor) {
			var value = ko.utils.unwrapObservable(valueAccessor());
			var openState = ko.utils.unwrapObservable(value.open);

			$(element).dialog(openState ? "open" : "close");

			if (openState && !value.isOpenNow) {
				value.isOpenNow = true;
				if (value.title) {
					var title = value.title();
					if (title) {
						$(element).dialog("option", "title", title);
					}
				}

				if (value.width) {
					var width = value.width();
					if (width) {
						$(element).dialog("option", "width", width);
					}
				}

				if (value.height) {
					var height = value.height();
					if (height) {
						$(element).dialog("option", "height", height);
					}
				}

				$(element).dialog("option", "position", "center");

				if (value.context() !== undefined) {
					var context = value.context();
					if (context) {
						ko.editable(context);
						context.beginEdit();

						var divv = document.createElement("DIV");
						ko.renderTemplate(value.options.contextTemplate /*ko.bindingHandlers.jqDialogContext.defaultContextOptions.contextTemplate*/, context, {
							templateEngine: stringTemplateEngine
						}, divv, null);
						$(element).html(divv);
					}
				}
			}
		},
		defaultContextOptions: {
			autoOpen: false,
			resizable: false,
			modal: true//,
			//contextTemplate: '<div data-bind="html: PeriodEndFullName"></div><p>propverka</p>'
		}
	};

	//#endregion

	//#region date

	///////////////////////////////////////////////////////////////
	//  Утилиты
	//  Расширение knockoutjs
	//  bindingHandlers для DateTime
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  moment
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.date = {
		update: function (element, valueAccessor) {
			var value = valueAccessor();
			var valueUnwrapped = ko.utils.unwrapObservable(value);
			if (valueUnwrapped) {
				var t = moment(valueUnwrapped).format("DD.MM.YYYY");
				$(element).text(t);
			}
		}
	};

	//#endregion

	//#region pager

	///////////////////////////////////////////////////////////////
	//  Утилиты
	//  Расширение knockoutjs
	//  bindingHandlers для Pager (control)
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  bridge.controls
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.pager = {
		update: function (element, valueAccessor) {
			var value = valueAccessor(),
				ds = ko.utils.unwrapObservable(value);
			if (ds && ds.hasPages()) {
				var template = "<div class='pagination'>\
									<ul data-bind='foreach: pages'>\
										<li data-bind='css: css'>\
											<a data-bind='text: name, attr: {href:\"#\"}, click: $parent.getData'></a>\
										</li>\
									</ul>\
								</div>";

				var div = document.createElement("DIV");
				ko.renderTemplate(template, ds, { templateEngine: stringTemplateEngine }, div, null);
				$(element).html(div);
			}
		}
	};

	//#endregion

	//#region progress

	///////////////////////////////////////////////////////////////
	//  Утилиты
	//  Расширение knockoutjs
	//  bindingHandlers для отображения процесса загрузки
	//  IsBusy progress
	//  автор: calabonga.net
	//  зависит от:     knockout
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.progress = {
		update: function (element, valueAccessor) {
			var value = valueAccessor(),
				isReady = ko.utils.unwrapObservable(value);
			if (isReady) {
				$.blockUI({
					fadeIn: 500,
					message: "<p>Ждите...<img src='/content/images/loader.gif'  alt=''/></p>",
					overlayCSS: { backgroundColor: '#0a6698', opacity: .7 }
				});
			} else {
				$.unblockUI();
			}
		}
	};

	//#endregion

	//#region fadeInText

	///////////////////////////////////////////////////////////////
	//  Утилиты
	//  Расширение knockoutjs
	//  bindingHandlers для затухания текста 
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  jQuery
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.fadeInText = {
		update: function (element, valueAccessor) {
			$(element).hide();
			ko.bindingHandlers.text.update(element, valueAccessor);
			$(element).fadeIn();
		}
	};


	//#endregion

	//#region flash

	///////////////////////////////////////////////////////////////
	//  Утилиты
	//  Расширение knockoutjs
	//  bindingHandlers вспышка текста 
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  jQuery
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.flash = {
		update: function (element, valueAccessor) {
			ko.utils.unwrapObservable(valueAccessor()); //unwrap to get subscription
			$(element).hide().fadeIn(500);
		}
	};

	//#endregion

	//#region jqButton

	///////////////////////////////////////////////////////////////
	//  Утилиты
	//  Расширение knockoutjs
	//  bindingHandlers кнопка jQuery
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  jQuery
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.jqButton = {
		init: function (element, valueAccessor) {
			var options = valueAccessor() || {};
			$(element).button(options);

			ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
				$(element).button("destroy");
			});
		},
		update: function (element, valueAccessor) {
			var value = valueAccessor();
			$(element).button("option", "disabled", value.enable === false);
		}
	};

	//#endregion

	//#region datepicker

	///////////////////////////////////////////////////////////////
	//  Утилиты
	//  Расширение knockoutjs
	//  bindingHandlers для DateTime
	//  автор: calabonga.net
	//  зависит от:     knockout
	//                  moment
	///////////////////////////////////////////////////////////////

	ko.bindingHandlers.datepicker = {
		init: function (element, valueAccessor, allBindingsAccessor) {
			//initialize datepicker with some optional options
			var options = allBindingsAccessor().datepickerOptions || {};
			options.onSelect = function (dateText, inst) {
				//alert('selected date');
				$(element).hide();
			};
			$(element).datepicker(options);

			//handle the field changing
			ko.utils.registerEventHandler(element, "change", function () {
				var observable = valueAccessor();
				observable($(element).datepicker("getDate"));
			});

			//handle disposal (if KO removes by the template binding)
			ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
				$(element).datepicker("destroy");
			});

		},
		update: function (element, valueAccessor) {
			var value = ko.utils.unwrapObservable(valueAccessor());

			//handle date data coming via json from Microsoft
			if (String(value).indexOf('/Date(') == 0) {
				value = new Date(parseInt(value.replace(/\/Date\((.*?)\)\//gi, "$1")));
			}

			var current = $(element).datepicker("getDate");

			if (value - current !== 0) {
				$(element).datepicker("setDate", value);
			}
		}
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

})(ko);