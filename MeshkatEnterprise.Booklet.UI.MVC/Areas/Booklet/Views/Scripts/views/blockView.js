var app = app || {};

(function ($) {
    'use strict';
    app.BlockView = Backbone.View.extend({
        tagName: "div",
        events: {
            "mouseup": "rangeSelected",
            "click .Meshkat-highlighted,.Meshkat-commentedhighlighted": "removeHighlight",
            "click .Meshkat-commented,.Meshkat-commentedhighlighted": "removeCommentHighlight",
        },
        pageViews: [],
        // Model: BookPragraphsBlock
        initialize: function () {
            this.render();
        },
        render: function () {
            var that = this;
            $(this.el).addClass("paragraphBlock");
            $(this.el).html("");
            this.pageViews = [];
            var pages=this.model.getPages().models;
            for (var i = 0; i < pages.length; i++) {
                var pageView = new app.PageView({ model: pages[i] });
                pageView.on("paragraphClicked", function (view) {
                    that.trigger("paragraphClicked",view);
                });
                this.pageViews.push(pageView);
                $(this.el).append(pageView.render().el);
            }
            return this;
        },
        getTopParagraph: function (top, bottom) {
            var showingParagraphs = [];
            for (var i = 0; i < this.pageViews.length; i++) {
                if (this.pageViews[i].isShowing(this.$el.position().top, top, bottom)) {
                    for (var j = 0; j < this.pageViews[i].paragraphViews.length; j++) {
                        if (this.pageViews[i].paragraphViews[j].isShowing(this.$el.position().top, top, bottom))
                            return this.pageViews[i].paragraphViews[j];
                    }
                }
            }
            return null;
        },
        getCurrentPageView: function (top, bottom) {
            for (var i = 0; i < this.pageViews.length; i++) {
                if (this.pageViews[i].isShowing(this.$el.position().top, top, bottom))
                    return this.pageViews[i];
            }
            return null;
        },
        getParagraphView: function (paragraphId) {
            for (var i = 0; i < this.pageViews.length; i++) {
                var paragraph = this.pageViews[i].getParagraph(paragraphId);
                if (paragraph !== null)
                    return paragraph;
            }
            return null;
        },
        rangeSelected: function () {
            var that = this;
            if (app.bookletView.highlightMode) {
                var sections = doHighlight('Meshkat-highlighted', this.$el);
                if (sections === undefined)
                    return;
                this.saveHighlightSections(sections);
            }
            else if (app.bookletView.commentMode) {
                doHighlight('Meshkat-commented', this.$el);
            }
        },
        getCommentedSections: function () {
            return getClassSections(this.$el.find(".paragraph"), "Meshkat-commented");
        },
        removeHighlight: function (e) {
            if (app.bookletView.highlightMode) {
                var paragraph = $(e.target).closest(".paragraph");
                $(e.target).removeClass("Meshkat-highlighted");
                if ($(e.target).hasClass("Meshkat-commentedhighlighted")) {
                    $(e.target).removeClass("Meshkat-commentedhighlighted");
                    $(e.target).addClass("Meshkat-commented");
                }
                var sections = getClassSections(paragraph, "Meshkat-highlighted");
                this.saveHighlightSections(sections);
            }
        },
        removeCommentHighlight: function (e) {
            if (app.bookletView.commentMode) {
                $(e.target).removeClass("Meshkat-commented");
                if ($(e.target).hasClass("Meshkat-commentedhighlighted")) {
                    $(e.target).removeClass("Meshkat-commentedhighlighted");
                    $(e.target).addClass("Meshkat-highlighted");
                }
            }
        },
        saveHighlightSections: function (sections) {
            var highlights = [];
            for (var i = 0; i < sections.length; i++) {
                highlights.push({ HighlightId: -1, HighlightSection: { ParagraphId: sections[i].getParagraphId(), StartOffset: sections[i].getStartOffset(), EndOffset: sections[i].getEndOffset() }, Color: "yellow", PersonId: -1 });
            }
            showLoading();
            $.ajax({
                type: "POST",
                url: ajaxPath.HIGHLIGHT_SAVE,
                dataType: "json",
                data: JSON.stringify(highlights),
                success:
                    function (data) {
                        hideLoading();
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
        },
        removeComment: function(commentId) {
            var comments = [];
            for (var i = 0; i < this.model.getComments().count; i++) {
                if (this.model.getComments()[i].getId() != commentId)
                    comments.push(this.model.getComments()[i]);
            }
            this.model.setComments(comments);
        }
    });
})(jQuery);
