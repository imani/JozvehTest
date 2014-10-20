ajaxPath = {
    TABLE_OF_CONTENT_GET_ROOT: "/Booklet/BookTableOfContent/GetRoots",
    TABLE_OF_CONTENT_GET_CHILDREN: "/Booklet/BookTableOfContent/GetChildren",
    VOLUME_GET: "/Booklet/BookVolume/GetVolume",
    LOG_OUT: "/Security/Account/LogOut",
    Search_Result: "/Booklet/Search/GetSearchResult",
    Search_In_TOCItems: "/Booklet/Search/TOCTags",
    TOC_GET_PATH: "/Booklet/Search/GetPath",
    BLOCK_GET: "/Booklet/BookParagraph/GetParagraphsBlock",
    BLOCK_GET_BY_PAGE_NUMBER: "/Booklet/BookParagraph/GetParagraphsByPageNumber",
    SUBTREE_GET: "/Booklet/Search/GetSubTree",
    REMOVE_COMMENT: "/Booklet/BookComment/RemoveComment",
    EDIT_COMMENT: "/Booklet/BookComment/EditComment",
    ADD_COMMENT: "/Booklet/BookComment/AddComment",
    COMMENTTYPES_GET: "/Booklet/BookComment/GetCommentTypes",
    HIGHLIGHT_SAVE: "/Booklet/BookHighlight/AddHighlight",
    PERSON_GET:"/Booklet/Person/GetPerson"
};

messageList = {
    ERROR: "خطا",
    CONNECTION_FAILED: "ارتباط با سرور برقرار نشد.",
    COMMENT_SAVED: "توضیح با موفقیت ثبت شد.",
    COMMENT_REMOVED: "توضیح حذف شد",
    VERIFICATION_FAILED: "TokenVerificationFailed",
    COMMENT_EDITED:"توضیح با موفقیت ویرایش شد"
};

function checkResponse(data) {
    var serviceResponse = data;
    if (serviceResponse !== undefined && serviceResponse.response !== undefined && serviceResponse.response.Message !== undefined) {
        var messageText = serviceResponse.Response.Message;
        showMessage(messageText, '', 'default');
    }
    return serviceResponse.Successful || (serviceResponse.response!==undefined && serviceResponse.response.Successful);
}
function showMessage(messageText, title, type) {
    $.ambiance({
        message: messageText,
        title: title,
        type: type,
        timeout: 5,
        fade: true
    });
}
function showLoading(el) {
    var options = {
        message: "<div><img src='../Areas/Booklet/Views/Styles/images/ajax-loader.gif' /> لطفاً منتظر بمانید...</div>",
        css: { 'color': '#066f77', 'font-size': 'medium', 'width': '20%', 'left': '40%', 'border': 'solid 1px #066f77', 'padding': '10px','background-color':'#FFFFFF' },
        overlayCSS: {
            backgroundColor: '#a3a3a3',
            opacity: 0.5,
            cursor: 'wait'
        }
    };
    $.blockUI(options);
}


function hideLoading(el) {
    if(el!==undefined)
        $(el).unblock();
    else
        $.unblockUI();
}


