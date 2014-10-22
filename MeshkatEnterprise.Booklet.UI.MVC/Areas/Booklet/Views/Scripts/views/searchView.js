var app = app || {};

(function($) {
    "use strict";
    app.SearchView = Backbone.View.extend({
        tagName: "div",
            events: {
                "mousewheel": "mouseWheel"
            },
            searchResults: null,
            searchQuery: null,
            start: null,
            end: null,
            loadCount: null,
            resultsCount: 20,
            scrollFlag: false,
            scrollOffset: 5000,
            resultNumber: 1,
            endOfSearch:false,
            initialize: function () {
                $("#searchBox").hide();
                $(this.el).addClass("searchPanel");  
            },
            render: function () {
                this.resultNumber = 1;
                $('.searchPanel').html("");

                var totalHitsNumber = this.searchResults.getTotalHits();
                var totalHits = "تعداد کل نتایج:  " + totalHitsNumber;
                var allHits = $("<p/>");
                allHits.attr("id", "allHits");
                $(allHits).text(totalHits);
                $(this.el).append(allHits);
                $(this.el).append("<hr/>");
                var resultCount = this.resultsCount > totalHitsNumber ? totalHitsNumber : this.resultsCount;
                for (var i = 0; i < resultCount; i++) {
                    var resultItem = new app.SearchResultItemView({ model: this.searchResults.getItems().models[i], resultNumber: this.resultNumber });
                    $(this.el).append(resultItem.el);
                    this.resultNumber++;
                }
              //  this.el = $(this.el).highlight(this.searchQuery);
                return this;
            },
            mouseWheel: function (event, delta) {
                if (!this.endOfSearch) {
                    var currentScroll = Math.abs(Math.floor(this.$el.position().top));
                    var divHeight = $(this.el).height();
                    if (!this.scrollFlag)
                        this.scrollFlag = true;
                    if ((divHeight - currentScroll) < this.scrollOffset+200) {
                        var start = this.loadCount * this.resultsCount;
                        var end = start + this.resultsCount;
                        this.loadMoreResults(this.searchQuery, start, end);
                    }
                }
            },
            loadResult: function (query, start, end) {
                this.searchQuery = query;
                this.loadCount = 1;
                showLoading();
                var that = this;
                $.ajax({
                    type: "GET",
                    url: ajaxPath.Search_Result + "?query=" + query + "&start=" + start + "&end=" + end,
                    dataType: "json",
                    success:
                        function (data) {
                            if (!checkResponse(data)) return;
                            var results = new app.SearchResultItems(data.ReturnValue);
                            that.searchResults = results;
                            that.render();
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
            loadMoreResults: function(query, start, end) {
                var that = this;
                //    showLoading();

                $.ajax({
                    type: "GET",
                    url: ajaxPath.Search_Result + "?query=" + query + "&start=" + start + "&end=" + end,
                    dataType: "json",
                    success:
                        function (data) {
                            if (!checkResponse(data)) return;
                            var results = new app.SearchResultItems(data.ReturnValue);
                            var totalHits = results.getTotalHits();
                            if (end > totalHits) {
                                end = totalHits;
                                this.endOfSearch = true;
                            }
                            var count = end % that.resultsCount == 0 ? that.resultsCount : totalHits % that.resultsCount;
                            for (var i = 0; i < count; i++) {
                                var resultItem = new app.SearchResultItemView({ model: results.getItems().models[i], resultNumber: that.resultNumber });
                                that.resultNumber++;
                                $(that.el).append(resultItem.el);
                            }
                            that.scrollFlag = false;
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
                this.loadCount++;
            },
        gotoSearchResult: function (tocId, paragraphId) {
        $.ajax({
            type: "GET",
            url: ajaxPath.SUBTREE_GET,
            dataType: "json",
            data: { nodeId: tocId },
            success: function (loadedNode) {
                if (!checkResponse(loadedNode))
                    return;
                hideLoading();
                var nodes = loadedNode.ReturnValue;
                var obj = {};
                var rootNode = $("#tableOfContentTree").fancytree("getRootNode");
                var treeToc = $("#tableOfContentTree").fancytree("getTree");
                for (var i = 1; i < nodes.length; i++) {
                    obj = { title: nodes[i].Title, ParentKey: nodes[i].ParentKey, key: nodes[i].Key, lazy: nodes[i].HasChild, tooltip: nodes[i].Title, icon: nodes[i].ParentKey ? 'book-open.png' : 'book-close.png', volumeId: nodes[i].VolumeId };
                    if (obj.ParentKey != null)
                        treeToc.getNodeByKey(obj.ParentKey.toString()).addChildren(obj);
                    else {
                        rootNode.fromDict(obj);
                        treeToc.getNodeByKey(obj.key.toString()).removeChildren();
                    }
                }
                treeToc.getNodeByKey(obj.key.toString()).setActive();
            },
            error: function (data) {
                hideLoading();
                if (data.responseText === messageList.VERIFICATION_FAILED) {
                    app.timeoutView.show();
                    return false;
                }
                showMessage(messageList.CONNECTION_FAILED, messageList.ERROR, "error");
            }
        });
    }

    });
})(jQuery);