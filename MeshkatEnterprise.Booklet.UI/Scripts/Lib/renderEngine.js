function getBlockHTML(block) {
    var html = $("<div></div>");
    for (var i = 0; i < block.Paragraphs.length; i++)
    {
        var text = block.Paragraphs[i].ParagraphText;
        var styles = block.Paragraphs[i].SectionStyles;
        var highlights = getParagraphItems(block.Highlights, block.Paragraphs[i].ParagraphId);
        html.append(render(text, styles, highlights, null));
    }
    return html;
}
function getParagraphItems(items, paragraphId) {
    var result = [];
    if (items === null)
        return result;
    for (var i = 0; i < items.length; i++) {
        var sections = items[i].Sections;
        for (var j = 0; j < sections.length; j++) {
            if (sections[j].ParagraphId === paragraphId && !_.contains(result, items[i]))
                result.push(items[i]);
        }
    }
    return result;
}
function render(text, styles,highlights,commentToShow) {
    var html = $("<div></div>");
    var tags = getTags(styles, highlights, commentToShow);
    var styles=[];
    for (var i = 0; i < tags.length - 1; i++) {
        for (var j = 0; j < tags[i].endStyles.length; j++) {
            if (tags[i].endStyles[j] === 'highlighted' && _.contains(styles, 'commentedhighlighted')) {
                styles = _.without(styles, 'commentedhighlighted');
                styles.push('commented');
            }
            else if (tags[i].endStyles[j] === 'commented' && _.contains(styles, 'commentedhighlighted')) {
                styles = _.without(styles, 'commentedhighlighted');
                styles.push('highlighted');
            }
            styles = _.without(styles, tags[i].endStyles[j]);
        }

        for (var j = 0; j < tags[i].startStyles.length; j++) {
            if (!_.contains(styles, tags[i].startStyles[j])) {
                if (tags[i].startStyles[j] === 'highlighted' && _.contains(styles, 'commented')) {
                    styles = _.without(styles, 'commented');
                    styles.push('commentedhighlighted');
                }
                else if (tags[i].startStyles[j] === 'commented' && _.contains(styles, 'highlighted')) {
                    styles = _.without(styles, 'highlighted');
                    styles.push('commentedhighlighted');
                }
                else if ((tags[i].startStyles[j] === 'commented' || tags[i].startStyles[j] === 'highlighted') && _.contains(styles, 'commentedhighlighted')) {

                }
                else
                    styles.push(tags[i].startStyles[j]);
            }
        }

        var part = text.substring(tags[i].position, tags[i + 1].position);
        var span = $("<span>" + part + "</span>");
        for (var j = 0; j < styles.length; j++)
            $(span).addClass(styles[j]);
        html.append(span);
    }
    return html;
}
function getTags(styles, highlights, commentToShow) {
    var tags = [];
    for (var i = 0; i < styles.length; i++) {
        tags=addTag(tags, styles[i].Section.StartOffset, 'bookletStyle' + styles[i].StyleType.BookStyleTypeId, true);
        tags = addTag(tags, styles[i].Section.EndOffset + 1, 'bookletStyle' + styles[i].StyleType.BookStyleTypeId, false);
    }
    for (var i = 0; i < highlights.length; i++) {
        for (var j = 0; j < highlights[i].Sections.length; j++) {
            tags = addTag(tags, highlights[i].Sections[j].StartOffset, 'highlighted', true);
            tags = addTag(tags, highlights[i].Sections[j].EndOffset+1, 'highlighted', false);
        }
    }
    if (commentToShow !== null) {
        for (var j = 0; j < commentToShow.Sections.length; j++) {
            tags = addTag(tags, commentToShow.Sections[j].StartOffset, 'commented', true);
            tags = addTag(tags, commentToShow.Sections[j].EndOffset + 1, 'commented', false);
        }
    }
    tags = _.sortBy(tags, function (item) { return item.position; });
    return tags;
}
function addTag(tags, offset,cls,isStart) {
    var index = -1;
    for (var i = 0; i < tags.length; i++) {
        if (tags[i].position === offset){
            index = i;
            break;
        }
    }
    if (index === -1) {
        var tag = { position: offset};
        if (isStart) {
            tag.startStyles = [cls];
            tag.endStyles = [];
        }
        else {
            tag.startStyles = [];
            tag.endStyles = [cls];
        }
        tags.push(tag);
    }
    else {
        if (isStart) {
            if (!_.contains(tags[index].startStyles, cls))
                tags[index].startStyles.push(cls);
        }
        else {
            if (!_.contains(tags[index].endStyles, cls))
                tags[index].endStyles.push(cls);
        }
    }
    return tags;
}
//////////////////////////////////////////// Highlighter//////////////////////////////////////////////////////////////////////////////

var nodeTypes = {
    ELEMENT_NODE: 1,
    TEXT_NODE: 3
};

function doHighlight(css,container) {
    var range = getCurrentRange();
    if (!range || range.collapsed) return;
    var rangeText = range.toString();
    var $wrapper = $('<span></span>').addClass(css);
    var createdHighlights = highlightRange(range, $wrapper,container);
    removeAllRanges();
    normalize(css,container);
}

function normalize(css, container) {
    var areas = $(container).children();
    for (var i = 0; i < areas.length; i++) {
        var spans = $(areas[i]).children();
        for (var j = 0; j < spans.length; j++) {
            var span = $(spans[j]);
            if ($(span).children().length > 0) {
                if ($(span).hasClass(css)) {
                    $(span).html($(span).text());
                    $(span).addClass(css);
                }
                else {
                    var contents = $(span).contents();
                    var html = "";
                    for (var k = 0; k < contents.length; k++) {
                        if ($(contents[k]).hasClass(css)) {
                            html += "<span class='" + css + " " + $(span).attr('class') + "'>" + $(contents[k]).html()+ "</span>";
                        }
                        else if ($(contents[k]).text().length > 0) {
                            html += "<span class='" + $(span).attr('class') + "'>" + $(contents[k]).text() + "</span>";
                        }
                    }
                    $(span).replaceWith($(html));
                }
            }
        }
    }
    for (var i = 0; i < areas.length; i++) {
        var spans = $(areas[i]).children();
        for (var j = 0; j < spans.length; j++) {
            var span = $(spans[j]);
            if ($(span).hasClass('highlighted') && $(span).hasClass('commented')) {
                $(span).removeClass('highlighted');
                $(span).removeClass('commented');
                $(span).addClass('commentedhighlighted');
            }
        }
    }
    for (var i = 0; i < areas.length; i++) {
        var spans = $(areas[i]).children();
        var j = 0;
        while (spans[j] !== undefined) {
            var current = $(spans[j]);
            var next = $(spans[j+1]);
            if (next != undefined) {
                var sameClass = true;
                var styles = current.attr('class').split(' ');
                for (var k = 0; k < styles.length; k++) {
                    if (!next.hasClass(styles[k])) {
                        sameClass = false;
                        break;
                    }
                }
                if (sameClass) {
                    var newText = $(current).text() + $(next).text();
                    var classes=next.attr('class');
                    current.remove();
                    next.html(newText);
                    next.attr('class', classes);
                    j++
                }
            }
            j++;
        }
    }
}
function getCurrentRange() {
    var selection = getCurrentSelection();

    var range;
    if (selection.rangeCount > 0) {
        range = selection.getRangeAt(0);
    }
    return range;
}
function getCurrentSelection() {
    var currentWindow = getCurrentWindow();
    var selection;

    if (currentWindow.getSelection) {
        selection = currentWindow.getSelection();
    } else if ($('iframe').length) {
        $('iframe', top.document).each(function() {
            if (this.contentWindow === currentWindow) {
                selection = rangy.getIframeSelection(this);
                return false;
            }
        });
    } else {
        selection = rangy.getSelection();
    }

    return selection;
}
function getCurrentWindow() {
    var currentDoc = document;
    if (currentDoc.defaultView) {
        return currentDoc.defaultView; // Non-IE
    } else {
        return currentDoc.parentWindow; // IE
    }
}

/**
 * Wraps given range (highlights it) object in the given wrapper.
 */
function highlightRange(range, $wrapper, container) {
    if (range.collapsed) return;

    // Don't highlight content of these tags
    var ignoreTags = ['SCRIPT', 'STYLE', 'SELECT', 'BUTTON', 'OBJECT', 'APPLET'];
    var startContainer = range.startContainer;
    var endContainer = range.endContainer;
    var ancestor = range.commonAncestorContainer;
    var goDeeper = true;

    if (range.endOffset == 0) {
        while (!endContainer.previousSibling && endContainer.parentNode != ancestor) {
            endContainer = endContainer.parentNode;
        }
        endContainer = endContainer.previousSibling;
    } else if (endContainer.nodeType == nodeTypes.TEXT_NODE) {
        if (range.endOffset < endContainer.nodeValue.length) {
            endContainer.splitText(range.endOffset);
        }
    } else if (range.endOffset > 0) {
        endContainer = endContainer.childNodes.item(range.endOffset - 1);
    }

    if (startContainer.nodeType == nodeTypes.TEXT_NODE) {
        if (range.startOffset == startContainer.nodeValue.length) {
            goDeeper = false;
        } else if (range.startOffset > 0) {
            startContainer = startContainer.splitText(range.startOffset);
            if (endContainer == startContainer.previousSibling) endContainer = startContainer;
        }
    } else if (range.startOffset < startContainer.childNodes.length) {
        startContainer = startContainer.childNodes.item(range.startOffset);
    } else {
        startContainer = startContainer.nextSibling;
    }

    var done = false;
    var node = startContainer;
    var highlights = [];

    do {
        if (goDeeper && node.nodeType == nodeTypes.TEXT_NODE) {

            //if (/\S/.test(node.nodeValue))
            //{
            var wrapper = $wrapper.clone(true).get(0);
            var nodeParent = node.parentNode;

            // highlight if node is inside the context
            if ($(container).get(0).contains(nodeParent)) {
                var highlight = $(node).wrap(wrapper).parent().get(0);
                highlights.push(highlight);
            }
            //}

            goDeeper = false;
        }
        if (node == endContainer && (!endContainer.hasChildNodes() || !goDeeper)) {
            done = true;
        }

        if ($.inArray(node.tagName, ignoreTags) != -1) {
            goDeeper = false;
        }
        if (goDeeper && node.hasChildNodes()) {
            node = node.firstChild;
        } else if (node.nextSibling != null) {
            node = node.nextSibling;
            goDeeper = true;
        } else {
            node = node.parentNode;
            goDeeper = false;
        }
    } while (!done);

    return highlights;
}
function removeAllRanges() {
    var selection = this.getCurrentSelection();
    selection.removeAllRanges();
}
function getClassRanges(panel, css) {
    var spans = $(panel).find('span');
    var ranges = [];
    for (var i = 0; i < spans.length; i++) {
        if ($(spans[i]).hasClass(css) || $(spans[i]).hasClass('commentedhighlighted')) {
            var start = i;
            while ($(spans[i]).hasClass(css) || $(spans[i]).hasClass('commentedhighlighted')) {
                i++
            }
            var end = i - 1;
            if (end < start)
                end = start;
            var startOffset=0;
            var length=0;
            for (var j = 0; j < start; j++)
                startOffset += $(spans[j]).text().length;
            for (var j = start; j <= end; j++)
                length += $(spans[j]).text().length;
            ranges.push({start:startOffset,end:startOffset+length-1});
        }
    }
    return ranges;
}