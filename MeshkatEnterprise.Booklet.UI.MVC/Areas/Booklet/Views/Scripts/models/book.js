var app = app || {};

(function () {
    'use strict';

    app.Book = Backbone.AssociatedModel.extend({
        defaults: {
            BookId: 0,
            BookName: 0
        },
        constructor: function (attributes, options) {
            Backbone.Model.apply(this, arguments);
        },

        //Relations

        //Getters
        getBookId: function () {
            return this.get("BookId");
        },
        getBookName: function () {
            return this.get("BookName");
        },

        //Setters
        setBookId: function (val) {
            this.set("BookId", val);
        },
        setBookName: function (val) {
            this.set("BookName", val);
        }
    });
})();
