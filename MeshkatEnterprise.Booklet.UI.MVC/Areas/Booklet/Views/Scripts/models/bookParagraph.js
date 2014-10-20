var app = app || {};

(function () {
    'use strict';

    app.BookParagraph = Backbone.AssociatedModel.extend({
        defaults: {
            VolumeInfo:null,
            ParagraphId: 0,
            ParagraphText: "",
            TableOfContentNode: null,
            ParagraphPageNumber: 1,
            Styles: [],
            HighlightSections: [],
            CommentSections: [],
            Footnotes: [],
            CommentIndices:[]
        },
        constructor: function (attributes, options) {
            Backbone.Model.apply(this, arguments);
        },

        //Relations
        relations: [
            {
                type: Backbone.One,
                key: 'VolumeInfo',
                relatedModel: app.BookVolume
            },
            {
                type: Backbone.One,
                key: "TableOfContentNode",
                relatedModel: app.BookTableOfContent
            },
            {
                type: Backbone.Many,
                key: "Styles",
                relatedModel: app.BookSectionStyle
            },
            ,
            {
                type: Backbone.Many,
                key: "HighlightSections",
                relatedModel: app.Section
            },
            {
                type: Backbone.Many,
                key: "CommentSections",
                relatedModel: app.Section
            },
            {
                type: Backbone.Many,
                key: "Footnotes",
                relatedModel: app.Footnote
            }
        ],
        //Getters
        getVolumeInfo: function () {
            return this.get("VolumeInfo");
        },
        getParagraphId: function () {
            return this.get("ParagraphId");
        },
        getParagraphText: function () {
            return this.get("ParagraphText");
        },
        getTableOfContentNode: function () {
            return this.get("TableOfContentNode");
        },
        getParagraphPageNumber: function () {
            return this.get("ParagraphPageNumber");
        },
        getStyles: function () {
            return this.get("Styles");
        },
        getHighlightSections: function () {
            return this.get("HighlightSections");
        },
        getCommentSections: function () {
            return this.get("CommentSections");
        },
        getFootnotes: function () {
            return this.get("Footnotes");
        },
        getCommentIndices: function () {
            return this.get("CommentIndices");
        },
        //Setters
        setVolumeInfo: function (val) {
            this.set("VolumeInfo",val);
        },
        setParagraphId: function (val) {
            this.set("ParagraphId",val);
        },
        setParagraphText: function (val) {
            this.set("ParagraphText",val);
        },
        setTableOfContentNode: function (val) {
            this.set("TableOfContentNode",val);
        },
        setParagraphPageNumber: function (val) {
            this.set("ParagraphPageNumber",val);
        },
        setStyles: function (val) {
            this.set("Styles",val);
        },
        setHighlightSections: function (val) {
            this.set("HighlightSections",val);
        },
        setCommentSections: function (val) {
            this.set("CommentSections", val);
        },
        setFootnotes: function (val) {
            this.set("Footnotes", val);
        },
        setCommentIndices: function (val) {
            this.set("CommentIndices", val);
        }
    });
})();