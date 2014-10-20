var app = app || {};

(function ($) {
    'use strict';
    app.ParagraphView = Backbone.View.extend({
        tagName: "div",
        // Model: BookParagraph
        initialize: function () {
            this.render();
        },
        events: {
            "click":"paragraphClicked"
        },
        render: function () {
            $(this.el).addClass("paragraph");
            $(this.el).html("");
            var carets = this.getCarets();
            var styles = [];
            for (var i = 0; i < carets.length - 1; i++) {
                for (var j = 0; j < carets[i].endStyles.length; j++) {
                    if (carets[i].endStyles[j] === 'Meshkat-highlighted' && _.contains(styles, 'Meshkat-commentedhighlighted')) {
                        styles = _.without(styles, 'Meshkat-commentedhighlighted');
                        styles.push('Meshkat-commented');
                    }
                    else if (carets[i].endStyles[j] === 'Meshkat-commented' && _.contains(styles, 'Meshkat-commentedhighlighted')) {
                        styles = _.without(styles, 'Meshkat-commentedhighlighted');
                        styles.push('Meshkat-highlighted');
                    }
                    styles = _.without(styles, carets[i].endStyles[j]);
                }

                for (var j = 0; j < carets[i].startStyles.length; j++) {
                    if (!_.contains(styles, carets[i].startStyles[j])) {
                        if (carets[i].startStyles[j] === 'Meshkat-highlighted' && _.contains(styles, 'Meshkat-commented')) {
                            styles = _.without(styles, 'Meshkat-commented');
                            styles.push('Meshkat-commentedhighlighted');
                        }
                        else if (carets[i].startStyles[j] === 'Meshkat-commented' && _.contains(styles, 'Meshkat-highlighted')) {
                            styles = _.without(styles, 'Meshkat-highlighted');
                            styles.push('Meshkat-commentedhighlighted');
                        }
                        else if ((carets[i].startStyles[j] === 'Meshkat-commented' || carets[i].startStyles[j] === 'Meshkat-highlighted') && _.contains(styles, 'Meshkat-commentedhighlighted')) {

                        }
                        else
                            styles.push(carets[i].startStyles[j]);
                    }
                }

                var part = this.model.getParagraphText().substring(carets[i].position, carets[i + 1].position);
                var span = $("<span>" + part + "</span>");
                for (var j = 0; j < styles.length; j++) {
                    $(span).addClass(styles[j]);
                    if (styles[j] === "Meshkat-footnoteRef") {
                        $(span).tooltip({
                            position: { my: "middle bottom", at: "left top" },
                            show: { effect: "slide", duration: 200, direction: "down" },
                            tooltipClass: "footnoteTooltip"
                        });
                    }
                    if (styles[j] === "Meshkat-page") {
                        $(this.el).addClass("Meshkat-pageHeader");
                    }
                }
                $(this.el).append(span);
                $(this.el).attr("data-id",this.model.getParagraphId());
            }

            var footnoteElems = this.$el.find(".Meshkat-footnoteRef");
            var footnotes = this.model.getFootnotes().models;
            for (var i = 0; i < footnoteElems.length; i++) {
                $(footnoteElems[i]).attr("title", footnotes[i].getText());
            }

            return this;
        },
        getCarets: function () {
            var styles = this.model.getStyles().models;
            var highlightSections = this.model.getHighlightSections().models;
            var carets = [];
            for (var i = 0; i < styles.length; i++) {
                carets = this.addCaret(carets, styles[i].getSection().getStartOffset(), 'Meshkat-' + styles[i].getStyle(), true);
                carets = this.addCaret(carets, styles[i].getSection().getEndOffset() + 1, 'Meshkat-' + styles[i].getStyle(), false);
            }
            for (var i = 0; i < highlightSections.length; i++) {
                carets = this.addCaret(carets, highlightSections[i].getStartOffset(), 'Meshkat-highlighted', true);
                carets = this.addCaret(carets, highlightSections[i].getEndOffset() + 1, 'Meshkat-highlighted', false);
            }
            var commentSections = this.model.getCommentSections().models;
            for (var i = 0; i < commentSections.length; i++) {
                if (this.sectionIsSelectd(commentSections[i])) {
                    carets = this.addCaret(carets, commentSections[i].getStartOffset(), 'Meshkat-commented', true);
                    carets = this.addCaret(carets, commentSections[i].getEndOffset() + 1, 'Meshkat-commented', false);
                }
            }
            carets = _.sortBy(carets, function (item) { return item.position; });
            return carets;
        },
        addCaret: function (carets, offset, cls, isStart) {
            var index = -1;
            for (var i = 0; i < carets.length; i++) {
                if (carets[i].position === offset) {
                    index = i;
                    break;
                }
            }
            if (index === -1) {
                var caret = { position: offset };
                if (isStart) {
                    caret.startStyles = [cls];
                    caret.endStyles = [];
                }
                else {
                    caret.startStyles = [];
                    caret.endStyles = [cls];
                }
                carets.push(caret);
            }
            else {
                if (isStart) {
                    if (!_.contains(carets[index].startStyles, cls))
                        carets[index].startStyles.push(cls);
                }
                else {
                    if (!_.contains(carets[index].endStyles, cls))
                        carets[index].endStyles.push(cls);
                }
            }
            return carets;
        },
        isShowing: function (baseHeight,top,bottom) {
            var start = baseHeight + $(this.el).position().top;
            var end = start + $(this.el).height();
            if (end >= top && start <= bottom)
                return true;
            return false;
        },
        sectionIsSelectd: function (section) {
            if(app.commentsView.selectedComment===null)
                return false;
            var sections=app.commentsView.selectedComment.getSections().models;
            for (var i = 0; i < sections.length; i++) {
                if (sections[i].getParagraphId() === section.getParagraphId() && sections[i].getStartOffset()===section.getStartOffset() && sections[i].getEndOffset()===section.getEndOffset()) {
                    return true;
                }
            }
            return false;
        },
        paragraphClicked: function (e) {
            if (app.bookletView.commentMode || app.bookletView.highlightMode)
                return;
            //app.tableOfContentView.selectNode(this.model.getTableOfContentNode().getKey());
            this.trigger("paragraphClicked",this);
        }
    });
})(jQuery);
