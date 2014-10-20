var app = app || {};

(function ($) {
    'use strict';
    // Table of content view

    app.TreeView = Backbone.View.extend({
        tagName: 'div',
        nodeTemplate: $("#nodeTemplate").html(),
        url:'Service.asmx/GetNodes',
        events: {

        },
        initialize: function () {
            this.render();
        },
        render: function () {
            $(this.el).html('');
            this.loadNodes(this.el, null);
            return this;
        },
        loadNodes: function (elem, id) {
            debugger
            var that = this;
            $.ajax({
                type: "POST",
                url: this.url,
                dataType: 'json',
                contentType: 'application/json; charset:utf-8',
                data: JSON.stringify({ id: id }),
                success: function (nodes) {
                    debugger
                    for (var i = 0; i < nodes.length; i++) {
                        debugger
                        var nodeElem = _.template($(nodeTemplate).html(), { Title: nodes[i].Title });
                        $(elem).append(nodeElem);
                    }
                }
            });
        }

    });
})(jQuery);
