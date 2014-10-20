var app = app || {};

(function () {
    'use strict';
    var VolumeList = Backbone.Collection.extend({
        
        model: app.Volume,

    });

    app.volumes = new VolumeList();
})();