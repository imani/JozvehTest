﻿$.widget("book.meshkatTab", {
    options: {
        scrollPos: 0,
        delayTimer: 0,
        scrollDistance: 300,
        scrollDuration: 300
    },
    _create: function () {
        var that = this;
        
        this.tabContainer = $('<div></div>');
        $(this.tabContainer).addClass('tabContainer');
        $(this.tabContainer).addClass('ui-layout-north');

        this.tabContent = $('<div></div>');
        $(this.tabContent).addClass('ui-layout-center');

        this.tabBar = $('<div></div>');
        $(this.tabBar).addClass('tabBar');

        this.leftButton = $('<span></span>');
        this.leftButton.addClass('fa fa-chevron-left');
        this.leftButton.addClass('tabButton');
        this.leftButton.addClass('tabLeftButton');

        this.rightButton = $('<span></span>');
        this.rightButton.addClass('fa fa-chevron-right');
        this.rightButton.addClass('tabButton');
        this.rightButton.addClass('tabRightButton');

        $(this.tabContainer).append(this.leftButton);
        $(this.tabContainer).append(this.tabBar);
        $(this.tabContainer).append(this.rightButton);

        $(this.element).append(this.tabContainer).append(this.tabContent);
        $(this.element).layout({
            defaults: {
                spacing_closed: 0,
                spacing_open: 0
            },
            north: {
                size: '20'
            },
        });

        $(this.tabBar).mousewheel(function (event, delta) {
            this.scrollLeft -= (delta * 30);
            that.options.scrollPos = this.scrollLeft;
            event.preventDefault();
        });

        $('.tabButton').hover(function () {
            if (!$(this).hasClass('tabButtonDisabled'))
                $(this).addClass('tabButtonOver');
        }, function () {
            $(this).removeClass('tabButtonOver');
        });

        $(this.tabBar).animate({ scrollLeft: this.options.scrollPos + 'px' }, 0);

        var sizeChecking = function () {
            var panelWidth = $(that.tabBar).outerWidth();

            if ($(that.tabBar)[0].scrollWidth > panelWidth) {
                $(that.rightButton).show();
                $(that.leftButton).show();
                $(that.tabBar).css({ left: '14px', right: '14px' });

                if ($(that.tabBar)[0].scrollWidth - panelWidth == $(that.tabBar).scrollLeft()) {
                    $(that.rightButton).addClass('tabButtonDisabled');
                } else {
                    $(that.rightButton).removeClass('tabButtonDisabled');
                }
                if ($(that.tabBar).scrollLeft() == 0) {
                    $(that.leftButton).addClass('tabButtonDisabled');
                } else {
                    $(that.leftButton).removeClass('tabButtonDisabled');
                }
            } else {
                $(that.leftButton).hide();
                $(that.rightButton).hide();
                $(that.tabBar).css({ left: '0px', right: '0px' });
            }
        };
        sizeChecking();
        this.options.delayTimer = setInterval(function () {
            sizeChecking();
        }, 500);

        var pressAndHoldTimeout;
        $(this.rightButton).mousedown(function (e) {
            e.stopPropagation();
            var scrollRightFunc = function () {
                var left = $(that.tabBar).scrollLeft();
                that.options.scrollPos = Math.min(left + that.options.scrollDistance, $(that.tabBar)[0].scrollWidth - $(that.tabBar).outerWidth());
                $(that.tabBar).animate({ scrollLeft: (left + that.options.scrollDistance) + 'px' }, that.options.scrollDuration);
            };
            scrollRightFunc();

            pressAndHoldTimeout = setInterval(function () {
                scrollRightFunc();
            }, that.options.scrollDuration);
        }).bind("mouseup mouseleave", function () {
            clearInterval(pressAndHoldTimeout);
        });

        $(this.leftButton).mousedown(function (e) {
            e.stopPropagation();
            var scrollLeftFunc = function () {
                var left = $(that.tabBar).scrollLeft();
                that.options.scrollPos = Math.max(left - that.options.scrollDistance, 0);
                $(that.tabBar).animate({ scrollLeft: (left - that.options.scrollDistance) + 'px' }, that.options.scrollDuration);
            };
            scrollLeftFunc();

            pressAndHoldTimeout = setInterval(function () {
                scrollLeftFunc();
            }, that.options.scrollDuration);
        }).bind("mouseup mouseleave", function () {
            clearInterval(pressAndHoldTimeout);
        });
    },
    addOrSelectTab: function (title, panel, tag) {
        var theTab = null;
        var tabs=$(this.tabContainer).find('.tabSpan');
        for (var i = 0; i < tabs.length; i++) {
            if ($(tabs[i]).data().tag=== tag)
            {
                theTab = tabs[i];
                break;
            }
        }
        if (theTab === null)
            this.addTab(title, panel, tag);
        else
            this.selectTab(theTab);
    },
    addTab: function (title,panel, tag) {
        var that = this;
        var tab = $('<span></span>');
        
        $(tab).data('tag', tag);
        $(tab).data('panel', panel);
        $(tab).addClass('tabSpan');

        var tabBox = $('<span></span>');
        $(tabBox).addClass('tabBox');
        $(tabBox).html(title)
        $(tab).html(tabBox);

        var tabClose = $('<span><span>');
        $(tabClose).addClass('tabClose');
        $(tabClose).addClass('fa fa-times');
        $(tab).prepend(tabClose);

        $(tabClose).click(function () {
            $(that.tabContent).html('');
            $(this).parent().remove();
            if ($(that.tabContainer).find('.tabSpan').length > 0)
                that.selectLastTab();
        });

        $(tab).hover(function (e) {
            $(this).find('.tabClose').css('visibility', 'visible');
        }, function (e) {
            $(this).find('.tabClose').css('visibility', 'hidden');
        });

        $(tab).click(function(e) {
            that.selectTab(this);
        });
        $(this.tabBar).append(tab);
        this.selectTab(tab);
    },
    selectLastTab: function () {
        var lastTab = $(this.tabContainer).find('.tabSpan').last();
        if(lastTab)
            this.selectTab(lastTab);
    },
    selectTab: function (tab) {
        var selectedBefore = $(tab).hasClass('tabSelected');
        this.scrollSelectedIntoView();
        $(this.tabContent).html($(tab).data().panel);
        if (!selectedBefore) {
            $(this.tabBar).find('.tabSpan').removeClass('tabSelected');
            $(tab).addClass('tabSelected');
            this._trigger('select', null, tab);
        }
    },
    scrollSelectedIntoView: function () {
        var that = this;
        var selectedTab = $(this.tabContainer).find('.tabSelected');
        if (!selectedTab || !selectedTab.position() || typeof (selectedTab.position()) === 'undefined')
            return;
        var left = $(this.tabBar).scrollLeft();
        var scrollWidth = $(this.tabBar).width();
        if (selectedTab.position().left < 0) {
            that.options.scrollPos = Math.max(left + selectedTab.position().left + 1, 0);
            $(that.tabBar).animate({ scrollLeft: (left + selectedTab.position().left + 1) + 'px' }, that.options.scrollDuration);
        } else if ((selectedTab.position().left + selectedTab.outerWidth()) > scrollWidth) {
            that.options.scrollPos = Math.min(left + ((selectedTab.position().left + selectedTab.outerWidth()) - scrollWidth), $(that.tabBar)[0].scrollWidth - $(that.tabBar).outerWidth());
            $(that.tabBar).animate({ scrollLeft: (left + ((selectedTab.position().left + selectedTab.outerWidth()) - scrollWidth)) + 'px' }, that.options.scrollDuration);
        }
    },
    getCurrentPanel: function () {
        var tab = $(this.tabBar).find('.tabSelected');
        return $(tab).data().panel;
    }
});