﻿(function($) {
    "use strict";
    app.CommentItemView = Backbone.View.extend({
        events: {

            "mouseenter .commentItem": "showOptions",
            "mouseleave .commentItem": "hideOptions",
            "click .iconRemoveComment": "removeComment",
            "click .iconEditComment": "showEditPanel",
            "click": "commentClicked",
            "clickoutside": "outsideClicked",
            "click .commentBody-more":"expandBody"
        },
        tplCommentItem:null,
        initialize: function() {
            this.tplCommentItem = $("#tplCommentItem");
            this.render();
            $(this.el).find(".commentTypeColor").css("color",this.model.getType().getBookCommentTypeColor());
            $(this.el).layout({
                defaults: {
                    spacing_closed: 0,
                    spacing_open: 0
                },
                north: {
                    size:"25"
                }
            });
            this.$el.find(".btnCommentHeader").tooltip({
                position: { my: "middle bottom", at: "right top" },
                show: { effect: "slide", duration: 100, direction: "down" },
                tooltipClass: "commentOptionToolTip"
            });

        },
        render: function() {
            $(this.el).html(_.template(this.tplCommentItem.html(), { commentItem: this.model }));
            return this;

        },
        showOptions: function (e) {
            this.$el.find(".iconRemoveComment,.iconEditComment").show(100);
            this.$el.find(".commentType").fadeIn();
            if (!this.$el.find(".commentItem").hasClass("commentItem-selected")) {
                this.$el.find(".commentItem").addClass("commentItem-hover");
            }
        },
        hideOptions: function () {
            this.$el.find(".iconRemoveComment,.iconEditComment,.commentType").hide(100);
            this.$el.find(".commentType").fadeOut();
            this.$el.find(".commentItem").removeClass("commentItem-hover");
        },
        removeComment: function (e) {
            e.stopPropagation();
            var that = this;
            showLoading();
            $.ajax({
                type: "GET",
                url: ajaxPath.REMOVE_COMMENT + "?commentId=" + that.model.getId(),
                dataType: "json",
                success:
                    function (data) {
                        if (!checkResponse(data)) return;
                        hideLoading();
                        that.$el.hide(200);
                        showMessage(messageList.COMMENT_REMOVED, "حذف توضیح", "success");
                        
                    },
                error: function (data) {
                    showMessage(messageList.CONNECTION_FAILED, messageList.ERROR, "error");
                    hideLoading();
                },
                contentType: "application/json; charset=utf-8",
                cache: false
            });

        },
        showEditPanel: function (e) {
            e.stopPropagation();
            app.editCommentView.loadComment(this.model);

        },
        commentClicked: function (e) {
            $(".commentItem").removeClass("commentItem-selected");
            this.$el.find(".commentItem").addClass("commentItem-selected");
            this.$el.find(".commentItem").removeClass("commentItem-hover");
            app.bookletView.clearCommentHighlights();
            app.commentsView.selectedComment = this.model;
            app.bookletView.showCommentHighlights();
        },
        outsideClicked: function (e) {
            if (!app.bookletView.commentMode && $(e.target).closest(".commentItem").length === 0) {
                app.bookletView.clearCommentHighlights();
                $(".commentItem").removeClass("commentItem-selected");
            }
        },
        expandBody: function (e) {
            e.stopPropagation();
            this.$el.find(".commentBody").html(this.model.getText());
        }

    });
})(jQuery);