
// The Booklet Application. Define globals here.
$(function () {
    'use strict';
    new app.AppView();
});

appPath = "http://localhost:8080/";
ajaxPath = {
    TableOfContentGetRoots: appPath+"api/BookTableOfContent/GetRoots",
    TableOfContentGetChildren: appPath + "api/BookTableOfContent/GetChildren",
    VolumeGet: appPath + "api/BookVolume/GetVolume"
};

function checkResponse(data) {
    var serviceResponse = data;
    serviceResponse = JSON.parse(data);
    if (typeof serviceResponse != 'object' || serviceResponse!== 'undefined') {
        var messageText = serviceResponse.Response.Message;
        showMessage(messageText, '', 'default');
        }
    return serviceResponse.Successful;
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

    $(el).block({ message: '<img class="ajaxgif" style="align:center" src="styles/images/ajax.gif" /> ' });   
}


function hideLoading(el) {
    $(el).unblock();
}
function ajaxPost(url, sendData, successFunction, errorFunction, el) {
    (function ($) {
        showLoading(el);
        $.ajax({
            type: "POST",
            url: url,
            data: sendData,
            dataType: "json",
            success:
                function (data) {
                    hideLoading(el);
                    if (!checkResponse(data)) return false;
                    successFunction;
                },
            error: function (data) {
                showMessage("خطا", "خطا", "error");
                hideLoading(el);
                errorFunction;
            },
            contentType: "application/json; charset=utf-8",
            cache: false
        });
    })(jQuery);
}
function ajaxGet(url, successFunction, errorFunction,el) {
    (function ($) {
        showLoading(el);
        $.ajax({
            type: "Get",
            url: url,
            dataType: "json",
            success: function (data) {
                alert('success');
                hideLoading(el);
                if (!checkResponse(data)) return false;
                successFunction;
            },
            error: function () {
                alert('error');
                hideLoading(el);
                errorFunction;
            },
            contentType: "application/json; charset=utf-8",
            cache: false
        });
    })(jQuery);
}