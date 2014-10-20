var app = app || {};

(function () {
    'use strict';

    app.BookVolume = Backbone.AssociatedModel.extend({
        defaults: {
            StartParagraphId: 0,
            EndParagraphId: 0,
            VolumeId: 0,
            VolumeNumber: 0,
            Book: null,
            Pages:500
        },
        constructor: function (attributes, options) {
            Backbone.Model.apply(this, arguments);
        },

        //Relations
        relations: [
            {
                type: Backbone.One,
                key: 'Book',
                relatedModel: app.Book
            }
        ],

        //Getters
        getStartParagraphId: function () {
            return this.get("StartParagraphId");
        },
        getEndParagraphId: function () {
            return this.get("EndParagraphId");
        },
        getVolumeId: function () {
            return this.get("VolumeId");
        },
        getVolumeNumber: function () {
            return this.get("VolumeNumber");
        },
        getBook: function () {
            return this.get("Book");
        },
        getPages: function () {
            return this.get("Pages");
        },

        //Setters
        setStartParagraphId: function (val) {
            this.set("StartParagraphId",val);
        },
        setEndParagraphId: function (val) {
            this.set("EndParagraphId",val);
        },
        setVolumeId: function (val) {
            this.set("VolumeId",val);
        },
        setVolumeNumber: function (val) {
            this.set("VolumeNumber",val);
        },
        setBook: function (val) {
            this.set("Book",val);
        },
        setPages: function (val) {
            this.set("Pages", val);
        }
    });
})();
