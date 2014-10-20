var app = app || {};

(function ($) {
    'use strict';
    // Table of content view

    app.CommentsView = Backbone.View.extend({
        el: '#commentPanel',
        isVisible: false,
        events: {
            "click #btnCommentMode": "changeCommentMode",
            "click #btnCancelSaveComment": "changeCommentMode",
            "click #btnSaveComment": "saveNewComment"
        },
        tplCommentItem: null,
        selectedComment: null,
        initialize: function () {
            this.tplCommentItem = $("#tplCommentItem");
            this.$el.layout({
                defaults: {
                    spacing_closed: 0,
                    spacing_open: 0
                },
                north: {
                    resizable: false,
                    size: '30'
                }
            });


            this.render();
            //  $("#commentModeButton").on("Click", this.changeCommentMode());
        },
        render: function () {
            if (app.bookletView.commentMode)
                $('#btnCommentMode').addClass('commentBarButton-selected');
            else
                $('#btnCommentMode').removeClass('commentBarButton-selected');



            return this;
        },
        renderCurrentComments: function (comments) {
            $("#currentCommentsPanel").html("");
            $("#currentCommentsPanel").scrollTop();
            for (var i = 0; i < comments.length; i++) {
                var commentItem = new app.CommentItemView({ model: comments[i] });
                $("#currentCommentsPanel").append(commentItem.el);
            }
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
            $("#txtNewComment").text("");
            if (app.bookletView.commentMode) {
                if (app.bookletView.highlightMode) {
                    $("#btnHighlight").click();
                }

                $("#newCommentArea").show(500);
                $("#currentCommentsPanel").animate({ scrollTop: 1000 }, 800, 'swing');
                $("#txtNewComment").val("");
            }
            else {
                $("#newCommentArea").hide(500);
            }
            app.bookletView.clearCommentHighlights();
            this.render();
        },
        saveNewComment: function () {
            var that = this;
            $("#txtNewComment").focus();
            var newComment = new app.BookComment();
            var commentSubject = new app.BookCommentSubject();
            var commentType = new app.BookCommentType();
            commentType.setBookCommentTypeId(1);
            var commentFieldValue = new app.BookCommentFieldValue();
            var bookParagraph = new app.BookParagraph();
            var commentedSections = app.currentVolumeView.getCommentedSections();
            newComment.setType(commentType);
            newComment.setText($("#txtNewComment").text());
            newComment.setSections(commentedSections);
            showLoading();
            $.ajax({
                type: "POST",
                url: ajaxPath.ADD_COMMENT,
                dataType: "json",
                data: JSON.stringify(newComment),
                success: function (data) {
                    if (!checkResponse(data)) return;
                    hideLoading();
                    
                    //////////////////Update Block ////////////////////
                    newComment.setId(data.ReturnValue);
                    var block = app.currentVolumeView.currentBlockView.model.toJSON();
                    block.Comments.push(newComment.toJSON());
                    app.currentVolumeView.currentBlockView.model = new app.BookParagraphsBlock(block);
                    app.currentVolumeView.currentBlockView.render();
                    app.currentVolumeView.updatePage(true);
                    ///////////////////////////////////////////////////

                    that.changeCommentMode();
                    showMessage(messageList.COMMENT_SAVED, "ثبت توضیح", "success");
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
