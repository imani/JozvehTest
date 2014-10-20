var app = app || {};

(function ($) {
    'use strict';
    // Table of content view

    app.CommentsView = Backbone.View.extend({
        el: '#commentPanel',
        isVisible: false,
        events: {
            "click #commentModeButton": "changeCommentMode"
        },
        initialize: function () {
            $(this.el).tooltip({
                position: { my: "right-5px center+2px", at: "left center" },
                tooltipClass: "topBarTooltip"
            });
            $(this.el).layout({
                defaults: {
                    fxName: "slide",
                    fxSpeed: "medium",
                    spacing_closed: 0,
                    spacing_open: 0
                },
                north: {
                    resizable: false,
                    initHidden: false,
                    size:'20'
                },
            });
            this.render();
        },
        render: function () {
            if (app.bookletView.commentMode)
                $('#commentModeButton').addClass('commentBarButton-selected');
            else
                $('#commentModeButton').removeClass('commentBarButton-selected');
            return this;
        },
        isClosed: function () {
            return $("#mainContent").layout().state.west.isClosed;
        },
        open: function () {
            $("#mainContent").layout().open("west");
        },
        close: function () {
            $("#mainContent").layout().close("west");
        },
        changeCommentMode: function () {
            app.bookletView.commentMode = !app.bookletView.commentMode;
            this.render();
        }

    });
})(jQuery);
