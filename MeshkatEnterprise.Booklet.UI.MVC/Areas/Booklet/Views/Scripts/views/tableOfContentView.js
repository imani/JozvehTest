var app = app || {};

(function ($) {
    'use strict';
    // Table of content view

    app.TableOfContentView = Backbone.View.extend({
        el: '#tableOfContentPanel',
        TOCItems:[],
        events: {
        },
        initialize: function () {
            var that = this;
            this.$el.layout({
                defaults: {
                    spacing_closed: 0,
                    spacing_open: 0
                },
                north: {
                    resizable: false,
                    size:'30'
                },
            });

            $("#tableOfContentTree").fancytree({
                imagePath: "~/../../Areas/Booklet/Views/Styles/fancytree/",
                tabbable: false,
                autoActivate: false,
                autoScroll: false,
                keyboard: false,
                init: function () {
                    $(this).find(".fancytree-container").attr("DIR", "RTL").addClass("fancytree-rtl");
                },
                source: {
                    url: ajaxPath.TABLE_OF_CONTENT_GET_ROOT,
                    cache: true
                },
                lazyLoad: function (event, data) {
                    var node = data.node;
                    data.result = {
                        url: ajaxPath.TABLE_OF_CONTENT_GET_CHILDREN + "?parentNodeId=" + node.key,
                        cache: true
                    };
                },
                postProcess: function (event, data) {
                    if (!checkResponse(data))
                        return;
                    var nodes = data.response.ReturnValue;
                    var processed = [];
                    for (var i = 0; i < nodes.length; i++) {
                        processed.push({ title: nodes[i].Title, key: nodes[i].Key, lazy: nodes[i].HasChild, tooltip: nodes[i].Title, TOC:new app.BookTableOfContent(nodes[i]) });
                    }
                    data.result = processed;
                },
                //click: function (event, data) {
                //    var targetType = $.ui.fancytree.getEventTargetType(event.originalEvent);
                //    if (targetType==="title")
                //        app.bookletView.loadVolume(data.node.data.TOC);
                //},
                activate: function (event, data) {
                    app.bookletView.loadVolume(data.node.data.TOC);
                },
                renderNode: function (type, obj) {
                    var icons = $(obj.node.li).find(".fancytree-icon");
                    icons.css("background-image", "none");
                    //icons.css("color", "#84b0e1");
                    icons.addClass("fa fa-circle-o");
                }
            });
            
            //var icons = $("#tableOfContentTree").find(".fancytree-icon");
            //icons.css("background-image", "none");
            //icons.addClass("fa fa-circle-o");

            $("#tableOfContentTree").find(".fancytree-container").removeAttr("tabindex");
            $.ajax({
                type: "GET",
                url: ajaxPath.Search_In_TOCItems,
                dataType: "json",
                success:
                    function (data) {
                        if (!checkResponse(data))
                            return;
                        var keys = Object.keys(data.ReturnValue);
                        keys.forEach(function (key) {
                            var item ={ label: data.ReturnValue[key], value: key };
                            that.TOCItems.push(item);
                        });
                        $("#txtTOCSearch").autocomplete({
                            source: that.TOCItems,
                            select: function (event, ui) {
                                $(this).val(ui.item.label);
                                event.preventDefault();
                                showLoading();
                                that.selectNode(ui.item.value);
                            },
                            focus: function (event, ui) {
                                $(this).val(ui.item.label);
                                return false;
                            }

                        });
                    },
                error: function (data) {
                    hideLoading();
                    if (data.responseText === messageList.VERIFICATION_FAILED) {
                        app.timeoutView.show();
                        return false;
                    }
                    showMessage(messageList.CONNECTION_FAILED, messageList.ERROR, "error");
                },
                contentType: "application/json; charset=utf-8",
                cache: false
        });

            
            this.render();
        },
        render: function () {
            return this;
        },
        isClosed: function () {
            return $("#mainContent").layout().state.east.isClosed;
        },
        open: function () {
            $("#mainContent").layout().open("east");
            $('#txtTOCSearch').focus();
        },
        close: function () {
            $("#mainContent").layout().close("east");
        },
        selectNode: function(nodeKey) {
            $.ajax({
                type: "GET",
                url: ajaxPath.SUBTREE_GET,
                dataType: "json",
                data: { nodeId:nodeKey  },
                success: function (loadedNode) {
                    if (!checkResponse(loadedNode))
                        return;
                    hideLoading();
                    var nodes = loadedNode.ReturnValue;
                    var obj = {};
                    var rootNode = $("#tableOfContentTree").fancytree("getRootNode");
                    var treeToc = $("#tableOfContentTree").fancytree("getTree");
                    for (var i = nodes.length-1; i>=0 ; i--) {
                        obj = { title: nodes[i].Title, ParentKey: nodes[i].ParentKey, key: nodes[i].Key, lazy: nodes[i].HasChild, tooltip: nodes[i].Title, icon: nodes[i].ParentKey ? 'book-open.png' : 'book-close.png', TOC: new app.BookTableOfContent(nodes[i]) };
                        if (obj.ParentKey != null && treeToc.getNodeByKey(obj.key.toString())==null)
                            treeToc.getNodeByKey(obj.ParentKey.toString()).addChildren(obj);
                        else {
                            rootNode.fromDict(obj);
                            treeToc.getNodeByKey(obj.key.toString()).removeChildren();
                        }
                    }
                    treeToc.getNodeByKey(obj.key.toString()).setActive();
                },
                error: function (data) {
                    hideLoading();
                    if (data.responseText === messageList.VERIFICATION_FAILED) {
                        app.timeoutView.show();
                        return false;
                    }
                    showMessage(messageList.CONNECTION_FAILED, messageList.ERROR, "error");
                }
            });
        }
    });
})(jQuery);
