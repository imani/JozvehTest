(function($) {
    "use strict";
    app.EditCommentView = Backbone.View.extend({
        events: {
            "click #btnEditComment": "editComment",
            "click #btnCancelEditComment": "hideEditPanel"
        },
        initialize: function () {
            this.loadComment();
        },
        render: function() {
        },
        loadComment: function () {
            $("#txtCommentEdit").text(this.model.getText());
            $("#currentCommentsPanel").hide();
            $("#editCommentArea").show(300);
        },
        hideEditPanel: function () {
                $("#editCommentArea").hide(300);
                $("#currentCommentsPanel").show(300);
            },
           editComment: function () {
            this.model.setText($("#txtCommentEdit").val());
            var that = this;
            showLoading();
            $.ajax({
                type: "POST",
                url: ajaxPath.EDIT_COMMENT,
                dataType: "json",
                data: JSON.stringify(that.model),
                success:
                    function (data) {
                        if (!checkResponse(data)) return;
                        hideLoading();
                        that.$el.hide(200);
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
        }
    });
})(jQuery)
