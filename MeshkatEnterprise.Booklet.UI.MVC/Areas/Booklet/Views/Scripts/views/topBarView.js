var app = app || {};

(function ($) {
    'use strict';
    // Top bar view
    app.TopBarView = Backbone.View.extend({
        el: '#topBar',
        pinned: true,
        btnSearchClicked:false,
        events: {
            "click #btnTableOfContent": "showHideTableOfContent",
            "click #btnComments": "showHideComments",
            "click #btnHighlight": "toggleHighlightMode",
            "click #btnSearch": "showHideSearch",
            "click #btnLogout": "logOut",
            'keydown #txtSearch': "txtSearchKeyDown"
        },
        initialize: function () {
            $(this.el).tooltip({
                position: { my: "middle top", at: "left bottom" },
                show: { effect: "slide", duration: 200, direction: "up" },
                tooltipClass: "topBarTooltip"
            });
            $('#btnLogout').click(function () {
                showLoading();
                $.ajax({
                    type: "GET",
                    url: ajaxPath.LOG_OUT,
                    dataType: "json",
                    success:
                        function (data) {
                            hideLoading();
                            window.location = "/Security/Account";
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
            });
        },
        render: function () {
            return this;
        },
        showHideTableOfContent: function () {
            if (app.tableOfContentView.isClosed()) {
                app.tableOfContentView.open();
                $('#btnTableOfContent').addClass('toolBarButtonSelected');
            }
            else {
                app.tableOfContentView.close();
                $('#btnTableOfContent').removeClass('toolBarButtonSelected');
            }
        },
        showHideComments: function () {
            if (app.commentsView.isClosed()) {
                app.commentsView.open();
                $('#btnComments').addClass('toolBarButtonSelected');
            }
            else {
                app.commentsView.close();
                $('#btnComments').removeClass('toolBarButtonSelected');
            }
        },
        showHideSearch: function ()
        {
            this.btnSearchClicked = !this.btnSearchClicked;
            if (this.btnSearchClicked) {
                $('#btnSearch').addClass("toolBarButtonSelected");
                $("#txtSearch").animate({ width: "300" }, 200);
                $("#txtSearch").val('');
                $("#txtSearch").focus();
            }
            else {
                $('#btnSearch').removeClass("toolBarButtonSelected");
                $("#txtSearch").animate({ width: "0" }, 200);
            }
        },
        logOut: function ()
        {   
        },
        txtSearchKeyDown: function (e) {
            if (e.which == 13) {
                e.preventDefault();
                $("#bookletPanel").meshkatTab("addTab", "نتایج جستجو", app.searchView, 111);
                app.searchView.loadResult($("#txtSearch").val(), 0, 20);
            }
        },
        toggleHighlightMode: function () {
            app.bookletView.highlightMode = !app.bookletView.highlightMode;
            if (app.bookletView.highlightMode) {
                if (app.bookletView.commentMode) {
                    $("#btnCommentMode").click();
                }
                $("#btnHighlight").addClass("toolBarButtonSelected");
            } else {
                $("#btnHighlight").removeClass("toolBarButtonSelected");
            }
        }
    });
})(jQuery);
