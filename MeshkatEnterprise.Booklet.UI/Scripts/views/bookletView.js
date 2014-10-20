var app = app || {};

(function ($) {
    'use strict';
    // Top bar view

    app.BookletView = Backbone.View.extend({
        el: '#bookletPanel',
        events: {
            "mouseup":"rangeSelected"
        },
        commentMode: false,
        initialize: function () {
            this.render();
        },
        render: function () {
            
            $('#bookletPanel').meshkatTab({
                select: function (e, data) {
                    var tag = $(data).data().tag;
                }
            });
            
            return this;
        },
        loadVolume: function (node) {
            var title = node.title;
            //path=ajaxPath.VolumeGet + "?volumeId=" + node.data.volumeId;
            
            //while (node.data.VolumeInfo === null) {
            //    node = node.getParent();
            //}
            //if (node.data.VolumeInfo.Book!=null)
            //    this.loadStyleTypes(node.data.VolumeInfo.Book.BookStyleTypes);
            //else
            //    this.loadStyleTypes(node.getParent().data.VolumeInfo.Book.BookStyleTypes);
            //$('#bookletPanel').meshkatTab("addOrSelectTab", title, panel, node.data.VolumeInfo);
        },
        loadStyleTypes: function (types) {
            $('#bookletStyles').html('');
            for (var i = 0; i < types.length; i++) {
                var type = ".bookletStyle" + types[i].BookStyleTypeId;
                type += "{" + types[i].StyleTypeCSS + "}";
                $('#bookletStyles').append(type);
            }
        },
        rangeSelected: function () {
            if (this.commentMode) {
                var container = $('#bookletPanel').meshkatTab('getCurrentPanel');
                doHighlight('commented', container);
            }
        }
    });
})(jQuery);
