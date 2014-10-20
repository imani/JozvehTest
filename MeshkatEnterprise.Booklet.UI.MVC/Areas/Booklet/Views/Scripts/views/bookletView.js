var app = app || {};

(function ($) {
    'use strict';
    app.BookletView = Backbone.View.extend({
        el: '#bookletPanel',
        commentMode: false,
        highlightMode:false,
        initialize: function () {
            this.render();
        },
        render: function () {
            
            $('#bookletPanel').meshkatTab({
                select: function (e, data) {
                    var tag = $(data).data().tag;
                }
            });
            
            return this;
        },
        loadVolume: function (TOC) {
            this.TOC = TOC;
            showLoading();
            $.ajax({
                type: "GET",
                url: ajaxPath.VOLUME_GET + "?volumeId=" + TOC.getVolumeId(),
                dataType: "json",
                success:
                    function (data) {
                        hideLoading();
                        if (!checkResponse(data)) return false;
                        var volume = new app.BookVolume(data.ReturnValue);
                        var title = volume.getBook().getBookName() + " (جلد" + volume.getVolumeNumber() + ")";
                        if (volume.getVolumeNumber() === 0)
                            title = volume.getBook().getBookName();

                        var tab=$("#bookletPanel").meshkatTab("getTabById","volume" + volume.getVolumeId());
                         
                        if (tab === null) {
                            app.currentVolumeView = new app.VolumeView({ model: volume, paragraphId: app.bookletView.TOC.getBookParagraphId() });
                            $("#bookletPanel").meshkatTab("addTab", title, app.currentVolumeView, "volume" + volume.getVolumeId());
                        } else {
                            $("#bookletPanel").meshkatTab("selectTab", tab);
                            app.currentVolumeView = $(tab).data().view;
                            app.currentVolumeView.gotoParagraph(app.bookletView.TOC.getBookParagraphId());
                        }
                        //fill CommentType Of ComboBox
                        app.commentsView.fillTypes(app.currentVolumeView.model.getBook().getBookId(), $("#cboNewCommentType"));
                        app.commentsView.fillTypes(app.currentVolumeView.model.getBook().getBookId(), $("#cboEditCommentType"));
                        app.currentVolumeView.on("pageChanged", function (comments) {
                            app.commentsView.renderCurrentComments(comments);
                        });

                    },
                error: function (data) {
                    hideLoading();
                    if (data.responseText === messageList.VERIFICATION_FAILED) {
                        app.timeoutView.show();
                        return;
                    }
                    showMessage(messageList.CONNECTION_FAILED, messageList.ERROR, "error");
                },
                contentType: "application/json; charset=utf-8",
                cache: false
            });
        },
        showCommentHighlights: function () {
            var selectedComment = app.commentsView.selectedComment;
            if (selectedComment === null || app.currentVolumeView === null || app.currentVolumeView.currentBlockView===null)
                return;
            var sections = selectedComment.getSections().models;
            var renderedList = [];
            for (var i = 0; i < sections.length; i++) {
                var id = sections[i].getParagraphId();
                if (!_.contains(renderedList, id)) {
                    renderedList.push(id);
                    var view = app.currentVolumeView.currentBlockView.getParagraphView(id);
                    view.render();
                }
            }
        },
        clearCommentHighlights: function () {
            app.commentsView.selectedComment = null;
            var paragraphs = $(".Meshkat-commented,.Meshkat-commentedhighlighted").closest(".paragraph");
            for (var i = 0; i < paragraphs.length; i++) {
                var view = app.currentVolumeView.currentBlockView.getParagraphView($(paragraphs[i]).data().id);
                view.render();
            }
        },
        keyPressed: function (code) {
            if(app.currentVolumeView!==undefined)
                app.currentVolumeView.keyPressed(code);
        }
    });
})(jQuery);
