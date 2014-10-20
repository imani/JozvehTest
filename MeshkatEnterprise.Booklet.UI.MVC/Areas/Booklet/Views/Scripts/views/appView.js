var app = app || {};

(function ($) {
    'use strict';

    // The Booklet Application View
    app.AppView = Backbone.View.extend({

        el: '#bookletApplication',

        initialize: function () {

            // Initialize UI Views

            app.tableOfContentView = new app.TableOfContentView();
            app.topBarView = new app.TopBarView();
            app.bookletView = new app.BookletView();
            app.commentsView = new app.CommentsView();
            app.searchView = new app.SearchView();
            app.timeoutView = new app.TimeoutView();
            app.editCommentView = new app.EditCommentView();
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
            $(document).keydown(function (e) {
                e.which = e.which || e.keyCode || e.charCode || e.metaKey;
                if (e.which === 40 || e.which === 38) {
                    e.preventDefault();
                }
                if (e.which === 13 && app.timeoutView.isShowing()) {
                    app.timeoutView.gotoLogin();
                }
                app.bookletView.keyPressed(e.which);
            });
            this.loadUserInfo();
        },
        loadUserInfo: function () {
            showLoading();
            $.ajax({
                type: "GET",
                url: ajaxPath.PERSON_GET,
                dataType: "json",
                success:
                    function (data) {
                        hideLoading();
                        if (!checkResponse(data)) return false;
                        var person = new app.Person(data.ReturnValue);
                        $("#lblUserFullName").html(person.getPersonName() + " " + person.getPersonLastName());
                        $("#imgUser").attr("src", person.getPersonImagePath());
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
        }
    });
})(jQuery);
