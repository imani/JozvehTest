var app = app || {};

(function ($) {
    'use strict';
    // Table of content view

    app.TableOfContentView = Backbone.View.extend({
        el: '#tableOfContentPanel',
        events: {
        },
        initialize: function () {

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
                imagePath: "Styles/fancytree/",
                tabbable: false,
                init: function () {
                    $(this).find(".fancytree-container").attr("DIR", "RTL").addClass("fancytree-rtl");
                },
                source: {
                    url: ajaxPath.TableOfContentGetRoots,
                    cache: true
                },
                lazyLoad: function (event, data) {
                    var node = data.node;
                    data.result = {
                        url: ajaxPath.TableOfContentGetChildren + "?parentNodeId="+node.key,
                        cache: true
                    };
                },
                postProcess: function (event,data) {
                    if (!checkResponse(data))
                        return;
                    var nodes = data.response.ReturnValue;
                    var processed = [];
                    for (var i = 0; i < nodes.length; i++) {
                        processed.push({ title: nodes[i].Title, key: nodes[i].Key, lazy: nodes[i].HasChild, tooltip: nodes[i].Title, icon: nodes[i].ParentKey ? 'book-open.png' : 'book-close.png', volumeId: nodes[i].VolumeId });
                    }
                    data.result = processed;
                },
                activate: function (event, data) {
                    var node = data.node;
                    app.bookletView.loadVolume(node);
                }
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
        }
    });
})(jQuery);
