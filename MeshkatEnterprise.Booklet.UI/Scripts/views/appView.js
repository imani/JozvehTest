var app = app || {};

(function ($) {
    'use strict';

    // The Booklet Application View
    app.AppView = Backbone.View.extend({

        el: '#bookletApplication',

        events: {
            "mousemove":"showTopBar"
        },

        initialize: function () {

            // Initialize UI Views

            app.tableOfContentView = new app.TableOfContentView();
            app.topBarView = new app.TopBarView();
            app.bookletView = new app.BookletView();
            app.commentsView = new app.CommentsView();

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
                },
            });
            $("#mainContent").layout({
                defaults: {
                    fxName: "slide",
                    fxSpeed: "medium",
                    spacing_closed: 0,
                    spacing_open: 0
                },
                east: {
                    size: "20%",
                    spacing_closed: 3,
                    spacing_open: 3,
                    initClosed:true
                },
                west: {
                    size: "20%",
                    spacing_closed: 3,
                    spacing_open: 3,
                    initClosed: true
                }
            });
        },
        showTopBar: function (event) {

            if (event.pageY < 40) {
                if ($(this.el).layout().state.north.isClosed)
                    $(this.el).layout().show("north");
            }
            else {
                if (!$(this.el).layout().state.north.isClosed && !app.topBarView.pinned)
                    $(this.el).layout().hide("north");
            }
        }

    });
})(jQuery);
