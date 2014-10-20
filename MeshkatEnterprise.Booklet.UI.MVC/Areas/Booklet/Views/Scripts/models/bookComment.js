var app = app || {};

(function () {
    'use strict';

    app.BookComment = Backbone.AssociatedModel.extend({
        defaults: {
            Id: null,
            Text: null,
            PersonId: null,
            PersonName:null,
            CreationDateTime: null,
            Subjects: [],
            Type: null,
            Sections: [],
            RelatedParagraphs: [],
            FieldValues:[]
        },
        constructor: function (attributes, options) {
            Backbone.Model.apply(this, arguments);
        },

        //Relations
        relations: [
            {
                type: Backbone.Many,
                key: 'Subjects',
                relatedModel: app.BookCommentSubject
            },
            {
                type: Backbone.Many,
                key: 'Sections',
                relatedModel: app.Section
            },
            {
                type: Backbone.Many,
                key: 'RelatedParagraphs',
                relatedModel: app.BookParagraph
            },
            {
                type: Backbone.Many,
                key: 'FieldValues',
                relatedModel: app.BookCommentFieldValue
            },
            {
                type: Backbone.One,
                key: 'Type',
                relatedModel: app.BookCommentType
            }
        ],

        //Getters
        getId: function () {
            return this.get("Id");
        },
        getText: function () {
            return this.get("Text");
        },
        getPersonId: function () {
            return this.get("PersonId");
        },
        getPersonName: function() {
            return this.get("PersonName");
        },
        getCreationDateTime: function () {
            return this.get("CreationDateTime");
        },
        getSubjects: function () {
            return this.get("Subjects");
        },
        getType: function () {
            return this.get("Type");
        },
        getSections: function () {
            return this.get("Sections");
        },
        getRelatedParagraphs: function () {
            return this.get("RelatedParagraphs");
        },
        getFieldValues: function () {
            return this.get("FieldValues");
        },

        //Setters
        setId: function (val) {
            this.set("Id",val);
        },
        setText: function (val) {
            this.set("Text",val);
        },
        setPersonId: function (val) {
            this.set("PersonId",val);
        },
        setPersonName: function(val) {
            this.set("PersonName", val);
        },
        setCreationDateTime: function (val) {
            this.set("CreationDateTime",val);
        },
        setSubjects: function (val) {
            this.set("Subjects",val);
        },
        setType: function (val) {
            this.set("Type",val);
        },
        setSections: function (val) {
            this.set("Sections",val);
        },
        setRelatedParagraphs: function (val) {
            this.set("RelatedParagraphs",val);
        },
        setFieldValues: function (val) {
            this.set("FieldValues",val);
        },
    });
})();
