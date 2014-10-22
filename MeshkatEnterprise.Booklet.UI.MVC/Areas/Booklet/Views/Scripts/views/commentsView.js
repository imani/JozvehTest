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
                    size: '30'
                },
                south: {
                    size: '30'
                }
            });
            $("#newCommentArea").layout({

                defaults: {
                    spacing_closed: 0,
                    spacing_open: 0
                },
                north: {
                    size: '30'
                },
                south: {
                    size: '30'
                }
            });
            $(".lblCommentType").tooltip({
                position: { my: "middle bottom", at: "middle top" },
                show: { effect: "slide", duration: 100, direction: "down" },
                tooltipClass: "commentOptionToolTip"
            });
            this.render();
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
                $("#currentCommentsPanel").hide();
                $("#newCommentArea").show(200);
                $("#currentCommentsPanel").animate({ scrollTop: 1000 }, 800, 'swing');
                $("#txtNewComment").val("");
            }
            else {
                $("#newCommentArea").hide(200);
                $("#currentCommentsPanel").show();
            }
            app.bookletView.clearCommentHighlights();
            this.render();
        },
        saveNewComment: function () {
            var that = this;
            var commentedSections = app.currentVolumeView.getCommentedSections();
            debugger;
            if ($("#txtNewComment").text().trim().length == 0) {
                showMessage("متن توضیح نمیتواند خالی باشد", "", "error");
            }
            else if (commentedSections.length == 0) {
                showMessage("قسمتی برای توضیح انتخاب نشده است", "", "error");
            }
            else {

                $("#txtNewComment").focus();
                var newComment = new app.BookComment();
                var commentSubject = new app.BookCommentSubject();
                var commentType = new app.BookCommentType();
                commentType.setBookCommentTypeId($("#cboNewCommentType").val());
                commentType.setBookCommentTypeTitle($("#cboNewCommentType option:selected").text());
                commentType.setBookCommentTypeColor($("#cboNewCommentType option:selected").data("typeColor"));

                var commentFieldValue = new app.BookCommentFieldValue();
                var bookParagraph = new app.BookParagraph();
                newComment.setType(commentType);
                newComment.setPersonName($("#lblUserFullName").text());
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

                        //////////////////Update Block ////////////////////
                        newComment.setId(data.ReturnValue);
                        var block = app.currentVolumeView.currentBlockView.model.toJSON();
                        block.Comments.push(newComment.toJSON());
                        app.currentVolumeView.currentBlockView.model = new app.BookParagraphsBlock(block);
                        app.currentVolumeView.currentBlockView.render();
                        app.currentVolumeView.updatePage(true);
                        ///////////////////////////////////////////////////

                        hideLoading();
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

        },
        fillTypes: function (bookId, el) {
            $(el).html("");

            $.ajax({
                type: "GET",
                url: ajaxPath.COMMENTTYPES_GET + "?bookId=" + bookId,
                dataType: "json",
                success:
                    function (data) {
                        if (!checkResponse(data)) return;
                        var commentTypeList = data.ReturnValue;
                        debugger;
                        for (var i = 0; i < commentTypeList.length; i++) {
                            var option = $('<option>', {
                                value: commentTypeList[i].BookCommentTypeId,
                                text: commentTypeList[i].BookCommentTypeTitle
                            });
                            

                            $(option).data('typeColor', commentTypeList[i].BookCommentTypeColor);
                            // $(option).data('commentFields', commentTypeList[i].BookCommentFields);
                            $(el).append(option);
                        }

                    },
                error: function (data) {
                    showMessage(messageList.CONNECTION_FAILED, messageList.ERROR, "error");
                    hideLoading();
                },
                contentType: "application/json; charset=utf-8",
                cache: false
            });
            return el;
        }
    });
})(jQuery);
