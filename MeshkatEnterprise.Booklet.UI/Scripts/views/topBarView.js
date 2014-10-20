var app = app || {};

(function ($) {
    'use strict';
    // Top bar view

    app.TopBarView = Backbone.View.extend({
        el: '#topBar',
        pinned: true,
        searchButtonClicked:false,
        events: {
            "mouseenter #userImage": "showUserInfo",
            "mouseleave #userImage": "hideUserInfo",
            "mouseleave #userPanel": "hideUserInfo",
            "click #topBarPinButton": "changePin",
            "click #tableOfContentButton": "showHideTableOfContent",
            "click #commentsButton": "showHideComments",
            "click #searchButton": "showHideSearch",
            "click #logoutButton":"logOut"
        },
        initialize: function () {
            $('#topBarPinButton').addClass('toolBarButtonSelected');
            $(this.el).tooltip({
                position: { my: "middle top", at: "left bottom" },
                show: { effect: "slide", duration: 200, direction: "up" },
                tooltipClass: "topBarTooltip"
            });
            $("#userImage").on('mouseenter', function () {
                $('#userPanel').finish();
                $('#userPanel').show(200);
            });

            $("#userImage").on('mouseleave', function () {
        
                setTimeout(function () {

                    $('#userPanel').finish();
                    $('#userPanel').hide(200);
                }, 1500);

            });

            $("#userPanel").on('mouseleave', function () {
                setTimeout(function () {

                    $('#userPanel').finish();
                    $('#userPanel').hide(200);
                }, 1500);
            });

            $('#logoutButton').click(function () {
                $.ajax({
                    type: "GET",
                    url: "http://localhost:8080/api/Account/LogOut",
                    dataType: "json",
                    success:
                        function (data) {
                            window.location = "/login.html";
                        },
                    error: function (data) {
                        showMessage(data, "خطا -عدم موفقیت", "error");
                    },
                    contentType: "application/json; charset=utf-8",
                    cache: false
                });
            });
            $("#searchInput").hide();
        },
        render: function () {
            return this;
        },
        showUserInfo: function () {
            debugger
            $('#userPanel').finish();
            $('#userPanel').show(200);
        },
        hideUserInfo: function () {
            if ($(e.relatedTarget).parent().attr('id') === 'userPanel')
                return;
            $('#userPanel').finish();
            $('#userPanel').hide(200);
        },
        changePin: function ()
        {
            this.pinned = !this.pinned;
            if (this.pinned)
                $('#topBarPinButton').addClass('toolBarButtonSelected');
            else
                $('#topBarPinButton').removeClass('toolBarButtonSelected');
        },
        showHideTableOfContent: function () {
            if (app.tableOfContentView.isClosed()) {
                app.tableOfContentView.open();
                $('#tableOfContentButton').addClass('toolBarButtonSelected');
            }
            else {
                app.tableOfContentView.close();
                $('#tableOfContentButton').removeClass('toolBarButtonSelected');
            }
        },
        showHideComments: function () {
            if (app.commentsView.isClosed()) {
                app.commentsView.open();
                $('#commentsButton').addClass('toolBarButtonSelected');
            }
            else {
                app.commentsView.close();
                $('#commentsButton').removeClass('toolBarButtonSelected');
            }
        },
        showHideSearch: function ()
        {

            this.searchButtonClicked = !this.searchButtonClicked;
            if (this.searchButtonClicked) {
                debugger;
                $('#searchButton').addClass('toolBarButtonSelected');
                $("#searchInput").animate({ width: "300" }, 200);
                $("#searchInput").val('');
                $("#searchInput").show();
            }
            else {
                $('#searchButton').removeClass('toolBarButtonSelected');
                $("#searchInput").animate({ width: "0" }, 400);
                $("#searchInput").hide();
            }
        },
        logOut: function ()
        {
           
        }


    });
})(jQuery);
