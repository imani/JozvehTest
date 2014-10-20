var app = app || {};

(function ($) {
    "use strict";
    app.VolumeView = Backbone.View.extend({
        tagName: "div",
        events: {
            "mousewheel": "mouseWheel",
            "keypress .txtPageNumber": "pageNumberPressed",
            "keydown .txtPageNumber": "pageNumberDown",
            "click .volumePage":"pageClicked"
        },
        paragraphId: 1,             // Current paragraph id: always is the topmost paragraph that the user is viewing.
        firstCachedId: 1,           // Id of the first paragraph in the loaded block.
        lastCachedId: 1,            // Id of the last paragraph in the loaded block.
        cacheSize: 91,              // Size of the block that should be filled from the server side.
        scrollStep: 40,              // Number of pixels that the block moves at each step when the user is scrolling volume.
        pageNumber: 1,              // Current page Number.
        currentBlockView: null,        // The view of current loaded block
        currentPageView:null,
        waitingForResponse: false,
        reloadThreshold: 200,
        isLastBlock: false,
        isFirstBlock: true,
        initialize: function (args) {
            this.paragraphId = args.paragraphId;
            $(this.el).addClass("volume");
            this.tplVolume = $("#tplVolume");
            this.render();
            this.loadBlock(true);
        },
        render: function () {
            
            $(this.el).html(_.template(this.tplVolume.html(), {}));
            $(this.el).layout({
                defaults: {
                    spacing_closed: 0,
                    spacing_open: 0
                },
                south: {
                    size:"30"
                },
            });
            this.content = $(this.el).find(".ui-layout-center");
        },
        updateBlock: function (block) {
            var that = this;
            var oldPos = 0;
            var paragraphView=this.currentBlockView.getParagraphView(this.paragraphId);
            if (paragraphView !== null)
                oldPos = paragraphView.$el.offset().top;
            this.currentBlockView = new app.BlockView({ model: block });
            $(this.content).html(this.currentBlockView.render().el);
            var newPos = this.currentBlockView.getParagraphView(this.paragraphId).$el.offset().top;
            this.currentBlockView.$el.css("top", (this.currentBlockView.$el.position().top - (newPos - oldPos)) + "px");
            this.currentBlockView.on("paragraphClicked", function (view) {
                that.paragraphClicked(view);
            });
        },
        updateBlockSpecific: function (block) {
            var that = this;
            this.currentBlockView = new app.BlockView({ model: block });
            $(this.content).html(this.currentBlockView.render().el);
            var a = this.content.offset().top;
            var b = this.currentBlockView.getParagraphView(this.paragraphId).$el.offset().top;
            this.currentBlockView.$el.css("top", (a - b) + "px");
            this.currentBlockView.on("paragraphClicked", function (view) {
                that.paragraphClicked(view);
            });
        },
        loadBlock: function (isSpecific) {
            if (this.waitingForResponse)
                return;
            this.waitingForResponse = true;
            
            // Specific loading means load block and move it so that the current paragraph be at the top of view.
            // Non-specific loading happens when the user scrolls the volume.
            var that = this;
            this.firstCachedId = this.paragraphId - (this.cacheSize-1) / 3;
            this.lastCachedId = this.paragraphId + 2*(this.cacheSize-1) / 3;
            var start = this.model.getStartParagraphId();
            var end = this.model.getEndParagraphId();
            if (this.firstCachedId < start) {
                this.lastCachedId = (start + that.cacheSize);
                this.firstCachedId = start;
            }
            if (this.lastCachedId > end) {
                this.firstCachedId = end - that.cacheSize;
                this.lastCachedId = end;
            }
            if (this.paragraphId < this.firstCachedId)
                this.paragraphId = this.firstCachedId;
            else if (this.paragraphId > this.lastCachedId)
                this.paragraphId = this.lastCachedId;
            
            if (this.model.getEndParagraphId() >= this.firstCachedId && this.model.getEndParagraphId() <= this.lastCachedId)
                this.isLastBlock = true;
            else
                this.isLastBlock = false;
            
            if (this.model.getStartParagraphId() >= this.firstCachedId && this.model.getStartParagraphId() <= this.lastCachedId)
                this.isFirstBlock = true;
            else
                this.isFirstBlock = false;
            this.$el.find(".imgLazyLoading").show();
            $.ajax({
                type: "GET",
                url: ajaxPath.BLOCK_GET + "?startParagraphId=" + this.firstCachedId + "&endParagraphId=" + this.lastCachedId+"&volumeId="+this.model.getVolumeId(),
                dataType: "json",
                success: function (data) {
                    that.$el.find(".imgLazyLoading").hide();
                    that.waitingForResponse = false;
                    if (!checkResponse(data)) return false;
                    var currentBlock = new app.BookParagraphsBlock(data.ReturnValue);
                    if (!isSpecific)
                        that.updateBlock(currentBlock);
                    else
                        that.updateBlockSpecific(currentBlock);
                    that.updatePage(true);
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
        mouseWheel: function (event, delta) {
            if (delta === -1)
                this.goDown();
            else
                this.goUp();
        },
        goUp: function () {
            if (this.currentBlockView.$el.position().top < 0) {
                this.currentBlockView.$el.css("top", this.currentBlockView.$el.position().top + this.scrollStep + "px");
            }
            this.updateParagraph();
            
            // Check top expiration
            if (this.currentBlockView.$el.position().top > -this.reloadThreshold && this.paragraphId > this.model.getStartParagraphId() && !this.isFirstBlock) {
                this.loadBlock(false);
            }
        },
        goDown: function () {
            var volumeBottom = this.$el.position().top + this.$el.height();
            var blockBottom = this.currentBlockView.$el.position().top + this.currentBlockView.$el.height();
            if (blockBottom > volumeBottom) {
                var scrollValue = this.scrollStep;
                if (blockBottom - volumeBottom < 2 * (this.scrollStep)) {
                    scrollValue = blockBottom - volumeBottom + 50;
                }
                this.currentBlockView.$el.css("top", (this.currentBlockView.$el.position().top - scrollValue) + "px");
            }
            this.updateParagraph();
            
            // Check bottom expiration
            if (blockBottom - volumeBottom < this.reloadThreshold && this.paragraphId < this.model.getEndParagraphId() && !this.isLastBlock) {
                    this.loadBlock(false);
            }
        },
        updateParagraph: function () {
            var volumeTop = this.$el.position().top;
            var volumeBottom = volumeTop + this.$el.height();
            var topParagraph = this.currentBlockView.getTopParagraph(volumeTop, volumeBottom);
            if (topParagraph === null)
                return;
            this.paragraphId = topParagraph.model.getParagraphId();
            this.updatePage(false);
            
        },
        updatePage: function (forceUpdate) {
            var volumeTop = this.$el.position().top;
            var volumeBottom = volumeTop + this.$el.height();
            var prevPageView = this.currentPageView;
            this.currentPageView = this.currentBlockView.getCurrentPageView(volumeTop, volumeBottom);
            if (this.currentPageView === null)
                return;
            var pn = this.currentPageView.getPageNumber();
            if (forceUpdate || pn != this.pageNumber) {

                this.trigger("pageChanged", this.getPageComments(this.currentPageView));
                this.pageNumber = pn;
                if (prevPageView !== null) {
                    prevPageView.$el.removeClass("volumePage-selected");
                    prevPageView.$el.find(".paragraph-bold").removeClass("paragraph-bold");
                }
                this.currentPageView.boldPage();

                $(this.el).find(".volumeFooter").find(".txtPageNumber").val(pn);
                $(this.el).find(".volumeFooter").find(".lblPageCount").html(this.model.getPages());
                $(this.el).layout().show("south");
            }
        },
        gotoParagraph: function (paragraphId) {
            this.paragraphId = paragraphId;
            this.loadBlock(true);
        },
        gotoPage: function (pageNumber) {
            var that = this;
            var fetchSizeBefore = (this.cacheSize-1) / 3;
            var fetchSizeAfter = 2 * (this.cacheSize - 1) / 3;
            this.pageNumber = pageNumber;
            this.$el.find(".imgLazyLoading").show();
            $.ajax({
                type: "GET",
                url: ajaxPath.BLOCK_GET_BY_PAGE_NUMBER + "?volumeId=" + this.model.getVolumeId() + "&pageNumber=" + pageNumber + "&fetchSizeBefore=" + fetchSizeBefore + "&fetchSizeAfter=" + fetchSizeAfter,
                dataType: "json",
                success: function (data) {
                    that.$el.find(".imgLazyLoading").hide();
                    if (!checkResponse(data)) return false;
                    var currentBlock = new app.BookParagraphsBlock(data.ReturnValue);
                    var paragraphs = currentBlock.getParagraphs().models;
                    for (var i = 0; i < paragraphs.length; i++) {
                        if (paragraphs[i].getParagraphPageNumber()=== that.pageNumber) {
                            that.paragraphId = paragraphs[i].getParagraphId();
                            break;
                        }
                    }
                    that.updateBlockSpecific(currentBlock);
                    that.updatePage(true);
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
        pageNumberPressed: function (e) {
            e.which = e.which || e.keyCode || e.charCode || e.metaKey;

            if (e.which == 13) {
                var pageNumber =parseInt(this.$el.find(".txtPageNumber").val());
                if (pageNumber < 1)
                    pageNumber = 1;
                if (pageNumber > this.model.getPages())
                    pageNumber = this.model.getPages();
                this.gotoPage(pageNumber);
            }
        },
        pageNumberDown: function (e) {
            e.which = e.which || e.keyCode || e.charCode || e.metaKey;
            // Allow: backspace, delete, tab, escape, enter, ctrl+A and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190,116,123]) !== -1 ||
                // Allow: Ctrl+A
                (e.keyCode == 65 && e.ctrlKey === true) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39) || (e.keyCode >= 96 && e.keyCode <= 105) || (e.keyCode >= 48 && e.keyCode <= 57)) {
                // let it happen, don't do anything
                return;
            }
            e.preventDefault();
        },
        getParagraphComments: function (paragraphView) {
            var comments = this.currentBlockView.model.getComments().models;
            var commentIndices = paragraphView.model.getCommentIndices();
            var result = [];
            for (var i = 0; i < commentIndices.length; i++) {
                var ind = commentIndices[i];
                result.push(comments[ind]);
            }
            return result;
        },
        getPageComments: function (pageView) {
            var comments = this.currentBlockView.model.getComments().models;
            var indices = pageView.commentIndices;
            var result = [];
            for (var i = 0; i < indices.length; i++) {
                result.push(comments[indices[i]]);
            }
            return result;
        },
        getCommentedSections: function () {
            return this.currentBlockView.getCommentedSections();
        },
        keyPressed: function (code) {
            if (code !== 40 && code !== 38)
                return;
            var current=this.$el.find(".paragraph-bold");
            if (current === undefined)
                return;
            var id = current.data().id;
            if (code === 40) { // Arrow down
                var next = this.$el.find(".paragraph[data-id='" + (id + 1) + "']");
                if (next !== undefined)
                    next.click();

            }
            else if (code === 38) { // Arrow up
                var prev = this.$el.find(".paragraph[data-id='" + (id - 1) + "']");
                if (prev !== undefined)
                    prev.click();
            }
        },
        paragraphClicked: function (paragraphView) {
            this.$el.find(".paragraph-bold").removeClass("paragraph-bold");
            paragraphView.$el.addClass("paragraph-bold");
            this.trigger("pageChanged", this.getParagraphComments(paragraphView));
        }
    });
})(jQuery);
