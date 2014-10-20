(function ($) {
    "use strict";
    app.EditCommentView = Backbone.View.extend({
        el:"#editCommentArea",
        events: {
            "click #btnEditComment": "editComment",
            "click #btnCancelEditComment": "hideEditPanel"
        },
        initialize: function () {
            $(this.el).layout({
                defaults: {
                    spacing_closed: 0,
                    spacing_open: 0
                },

                    north: {
                size:"25"
                    }
            });
        },
        render: function () {
        },
        loadComment: function (commentModel) {
            this.model = commentModel;
            $("#txtCommentEdit").text(this.model.getText());
            $("#cboEditCommentType").val(this.model.getType().getBookCommentTypeId());
            $("#currentCommentsPanel").hide();
            $("#editCommentArea").show(300);
        },
        hideEditPanel: function () {
            $("#editCommentArea").hide(300);
            $("#currentCommentsPanel").show(300);
        },
        editComment: function () {
            
            this.model.setText($("#txtCommentEdit").text());
            this.model.getType().setBookCommentTypeId($("#cboEditCommentType").val());
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
                        showMessage(messageList.COMMENT_EDITED, "ویرایش توضیح", "success");
                        hideLoading();
                        that.$el.hide(200);
                        that.hideEditPanel();
                    },
                error: function (data) {
                    showMessage(messageList.CONNECTION_FAILED, messageList.ERROR, "error");
                    hideLoading();
                },
                contentType: "application/json; charset=utf-8",
                cache: false
            });
            
        }
    });
})(jQuery)
