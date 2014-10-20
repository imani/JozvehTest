var app = app || {};

(function () {
    'use strict';

    app.Volume = Backbone.Model.extend({
        defaults: {
            Id: 0,
            Title: '',
            StartParagraphId: 0,
            EndParagraphId: 0
        },
        getId: function () {
            return this.get('Id');
        },
        setId: function (t) {
            this.set('Id', t);
        },
        getTitle: function () {
            return this.get('Title');
        },
        setTitle: function (t) {
            this.set('Title', t);
        },
        getStartParagraphId: function () {
            return this.get('StartParagraphId');
        },
        setStartParagraphId: function (t) {
            this.set('StartParagraphId', t);
        },
        getEndParagraphId: function () {
            return this.get('EndParagraphId');
        },
        setEndParagraphId: function (t) {
            this.set('EndParagraphId', t);
        }
    });
})();
