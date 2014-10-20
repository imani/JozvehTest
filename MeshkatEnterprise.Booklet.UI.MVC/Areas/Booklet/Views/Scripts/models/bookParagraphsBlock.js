var app = app || {};

(function () {
    'use strict';

    app.BookParagraphsBlock = Backbone.AssociatedModel.extend({
        defaults: {
            Paragraphs: [],
            Comments: [],
            Highlights: [],
            Styles: [],
            Pages:[]
        },
        constructor: function (attributes, options) {
            this.fillExtraData(attributes);
            Backbone.Model.apply(this, arguments);
        },

        //Relations
        relations: [
            {
                type: Backbone.Many,
                key: 'Paragraphs',
                relatedModel: app.BookParagraph
            },
            {
                type: Backbone.Many,
                key: 'Comments',
                relatedModel: app.BookComment
            },
            {
                type: Backbone.Many,
                key: 'Highlights',
                relatedModel: app.BookHighlight
            },
            {
                type: Backbone.Many,
                key: 'Styles',
                relatedModel: app.BookSectionStyle
            },
            {
                type: Backbone.Many,
                key: 'Pages',
                relatedModel: app.BookPage
            }
        ],

        //Getters
        getParagraphs: function () {
            return this.get("Paragraphs");
        },
        getComments: function () {
            return this.get("Comments");
        },
        getHighlights: function () {
            return this.get("Highlights");
        },
        getStyles: function () {
            return this.get("Styles");
        },
        getPages: function () {
            return this.get("Pages");
        },

        //Setters
        setParagraphs: function (val) {
            this.set("Paragraphs", val);
        },
        setComments: function (val) {
            this.set("Comments", val);
        },
        setHighlights: function (val) {
            this.set("Highlights", val);
        },
        setStyles: function (val) {
            this.set("Styles", val);
        },
        setPages: function (val) {
            this.set("Pages", val);
        }



        ,
        fillExtraData: function (block) {
            var prevPageNumber = -1;
            var pageParagraphs = [];
            block.Pages = [];
            for (var i = 0; i < block.Paragraphs.length; i++) {

                // Add Styles,Highlight sections and Comment sections to the paragraph. it is needed to make each paragraph self renderable.
                block = this.fillSections(i, block);
                if (block.Paragraphs[i].ParagraphPageNumber !== prevPageNumber && prevPageNumber !== -1) {
                    // new page visited
                    // build a page with accumulated paragraphs
                    block.Pages.push({ PageNumber: prevPageNumber, Paragraphs: pageParagraphs });
                    // now start to accumulate new page paragraphs
                    pageParagraphs = [block.Paragraphs[i]];
                }
                else {
                    pageParagraphs.push(block.Paragraphs[i]);
                }
                prevPageNumber = block.Paragraphs[i].ParagraphPageNumber;
            }
            return block;
        },
        fillSections: function (index, block) {
            block.Paragraphs[index].Styles = [];
            block.Paragraphs[index].HighlightSections = [];
            block.Paragraphs[index].CommentSections = [];
            block.Paragraphs[index].CommentIndices = [];
            if (block.Styles !== null) {
                for (var i = 0; i < block.Styles.length; i++) {
                    if (block.Styles[i].Section.ParagraphId === block.Paragraphs[index].ParagraphId)
                        block.Paragraphs[index].Styles.push(block.Styles[i]);
                }
            }
            if (block.Paragraphs[index].Styles.length === 0) {
                block.Paragraphs[index].Styles.push({ Style: "default", Section: { ParagraphId: block.Paragraphs[index].ParagraphId, StartOffset: 0, EndOffset: block.Paragraphs[index].ParagraphText.length - 1 } });
            }

            if (block.Highlights !== null) {
                for (var i = 0; i < block.Highlights.length; i++) {
                    if (block.Highlights[i].HighlightSection.ParagraphId === block.Paragraphs[index].ParagraphId) {
                        block.Paragraphs[index].HighlightSections.push(block.Highlights[i].HighlightSection);
                    }
                }
            }

            if (block.Comments !== null) {
                for (var i = 0; i < block.Comments.length; i++) {
                    var sections = block.Comments[i].Sections;
                    for (var j = 0; j < block.Comments[i].Sections.length; j++) {
                        if (block.Comments[i].Sections[j].ParagraphId === block.Paragraphs[index].ParagraphId) {
                            block.Paragraphs[index].CommentSections.push(block.Comments[i].Sections[j]);
                            if (!_.contains(block.Paragraphs[index].CommentIndices, i))
                                block.Paragraphs[index].CommentIndices.push(i);
                        }
                    }

                }
            }
            return block;
        }
    });
})();
