$(document).ready(function e() {
    $('.select2').select2();
    //$('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })

});
(function ($) {
    $.fn.inputFilter = function (inputFilter) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    };
}(jQuery));

jQuery.fn.ForceNumericOnlyv2 = function () {
    return this.inputFilter(function (value) {
        return /^\d*$/.test(value);    // Allow digits only, using a RegExp
    });
}
function getDecimalValueFromMoneyInput(value) {
    if (isNullOrWhiteSpace(value))
        return 0
    return value.replace(/\./g, '');
}
function formatCurrencyVND(number) {
    var n = number.toString().split('').reverse().join("");
    var n2 = n.replace(/\d\d\d(?!$)/g, "$&.");
    return n2.split('').reverse().join('') + 'VND';
}
function formatCurrency(number) {
    var n = number.toString().split('').reverse().join("");
    var n2 = n.replace(/\d\d\d(?!$)/g, "$&.");
    return n2.split('').reverse().join('') + '';
}
function SetFormatDateTimeDMY(datetime) {
    try {
        var valueDate = parseInt(datetime.replace("/Date(", "").replace(")/", ""));

        if (valueDate < 0)
            return "";
        else {
            var dateObj = new Date(valueDate);
            var dateStr = ('00' + dateObj.getDate()).slice(-2) + "-" + ('00' + (dateObj.getMonth() + 1)).slice(-2) + "-" + dateObj.getFullYear();
            return dateStr;
        }
        return dateStr;
    } catch (e) {
        return "";
    }
}
function convertStringToDMY(input) {
    debugger
    if (isNullOrWhiteSpace(input))
        return
    let mydate = new Date(input)
    console.log(mydate.toLocaleDateString())
    return mydate.toLocaleDateString()
    //return mydate.toDateString()
}
function isNullOrNoItem(arr) {
    if (arr === null || arr === undefined || arr.length === 0)
        return true;
    return false;
}
function isNullOrUndefined(value) {
    if (value === null || value === undefined)
        return true;
    return false;
}
function isNullOrWhiteSpace(text) {
    if (text === null || text === undefined || text === '' || text.toString().trim() === '')
        return true;
    return false;
}
function showBlock(div, text = 'Vui lòng chờ trong giây lát') {
    
    div.block({
        css: {
            border: 'none',
            padding: '15px',
            //backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: 1,
            //color: '#fff'
        },
        message: '<img src="../../img/busy.gif" /> ' + text
    });
}
function showError(jqXHR, exception) {
    if (jqXHR.status == 403) {
        if (jqXHR.responseJSON.Flag == "NoLogin" || jqXHR.responseJSON.Flag == "NoAuthor")
            window.location = jqXHR.responseJSON.Url;
        else
            alert(jqXHR.responseText);
    }
    else {
        alert(jqXHR.responseText);
    }
}
function getValueDisplay(value, type) {
    if (isNullOrWhiteSpace(type)) {
        if (isNullOrWhiteSpace(value))
            return '';
        return value
    }

    var display = null
    switch (type) {
        case 'datetime':
            display = FormatDateTimeDMY(value);
        default: break;
    }
    return display;
}
function getTotalPage(totalRecord, limit) {
    
    return totalRecord > limit ? Math.ceil(totalRecord / limit) : 1
}
function renderPaging(page, limit, totalRecord) {
    let totalPage = getTotalPage(totalRecord, limit)
    var startPage = page > pageMargin ? page - pageMargin : 1;
    var endPage = pageMargin + page > totalPage ? totalPage : pageMargin + page
    var paging = $("#pagination");
    paging.empty();
    var first = renderGoFirstPage(page, totalPage)
    var next = renderGoNextPage(page, totalPage);
    var prev = renderGoPreviousPage(page, totalPage)
    var last = renderGoLastPage(page, totalPage)
    paging.append(first)
    paging.append(prev)
    for (var i = startPage; i <= endPage; i++) {
        var active = page == i ? ' active' : '';
        var item = "<li class='paginate_button" + active + " aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + i + ")' >"
            + "<a href='javascript:;'>" + i + "</a>"
            + "</li>"
        paging.append(item)
    }
    paging.append(next);
    paging.append(last)
    $("#dtSource_info").text("Tổng: " + totalRecord + " items");
    $("#totalPage").text("Tổng: " + totalPage + " trang");
}
function renderTextLeft(value, type, className = '') {
    return "<td class='text-left " + className + "'>" + getValueDisplay(value, type) + "</td>";
}
function renderTextCenter(value, type) {
    return "<td class='text-center'>" + getValueDisplay(value, type) + "</td>";
}
function renderGoPreviousPage(page) {
    let newCurrentPage = page
    if (page > 1) {
        newCurrentPage = page - 1
        return "<li class='paginate_button previous' aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + newCurrentPage + ")'>"
            + "<a href='javascript:;'>Trước</a>"
            + "</li>"

    } else {
        return ""
    }
}
function renderCoBaoHiem(CoBaoHiem) {
    var value = ""

    if (CoBaoHiem == true)
        value = "<td class='text-left'> <div class='orange bolder' title='Không có bảo hiểm' ><i class=\"ace-icon fa fa-ban bigger-130\"></i></div> </td>";
    else
        value = "<td class='text-left'> <div class='green bolder'title = 'Có bảo hiểm' > <i class=\"ace-icon glyphicon glyphicon-ok bigger-130\"></i></div> </td>";
    return value;
}
function renderGoNextPage(page, totalPage) {
    if (page < totalPage) {
        newCurrentPage = page + 1
        return "<li class='paginate_button next' aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + newCurrentPage + ")' >"
            + "<a href='javascript:;'>Sau</a>"
            + "</li>"

    } else {
        return ""
    }
}
function renderGoLastPage(pag, totalPage) {

    if (totalPage > page) {
        return "<li class='paginate_button next' aria-controls='dtSource' tabindex='0' onclick='onClickPage(" + totalPage + ")' >"
            + "<a href='javascript:;'>Trang cuối</a>"
            + "</li>"
    } else {
        return ""
    }
}
function renderGoFirstPage(page, totalPage) {
    
    if (totalPage > 1 && page > 1) {
        return "<li class='paginate_button previous' onclick='onClickPage(" + 1 + ")' >"
            + "<a href='javascript:;'>Trang đầu</a>"
            + "</li>"
    } else {
        return ""
    }
}
function FormatDateTimeDMY(datetime, outputFormat = 'DD-MM-YYYY') {
    
    let date = moment(datetime)
    return date.format(outputFormat)
}
function setCheckedValueOfRadioButtonGroup(name, boolValue) {
    var radios = document.getElementsByName(name);

    for (var j = 0; j < radios.length; j++) {
        if (radios[j].value === boolValue) {
            radios[j].checked = true;
            break;
        }
    }
}
function renderTextLink(textValue, href, type, className = '') {
    let display = getValueDisplay(textValue, type);
    return "<td class='text-left " + className + "'><a href='" + href + "' >" + display + "</a></td>";
}
function setValueForDateInput(controlId, value) {
    if (value === null)
        value = new Date().getDay + 1;
    controlId.val(value);
}
function getDateSpecific(backtoBefore = 0) {
    let toDay = new Date();
    let last = new Date((toDay).getTime() - (backtoBefore * 24 * 60 * 60 * 1000));
    return FormatDateTimeDMY(last, 'YYYY-MM-DD')
}
function getQueryStatus(controlId = null) {
    if (controlId == isNullOrWhiteSpace)
        controlId = document.getElementById("ddlStatus");
    let status = controlId.val()
    if (!isNullOrWhiteSpace(status))
        return status.toString();
    return '';
}
function removeDuplicate(value = null) {
    if (isNullOrWhiteSpace(value))
        return '';
    let arr = value.split(',');
    if (arr == null)
        return [];
    let uniqueSet = new Set(arr);
    let backToArray = [...uniqueSet]
    return backToArray.toString();
}
function getNewGuid() {
    const s4 = () => {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    };
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}
function isExtension(ext, extnArray) {
    var result = false;
    var i;
    if (ext) {
        ext = ext.toLowerCase();
        for (i = 0; i < extnArray.length; i++) {
            if (extnArray[i].toLowerCase() === ext) {
                result = true;
                break;
            }
        }
    }
    return result;
}
function renderStatus(statusName, btnRefresh = null) {

    
    if (isNullOrWhiteSpace(statusName))
        return "<td class='text-left'></td>";
    let firstChar = statusName[0].toLowerCase();
    let greenGroup = ['a', 'b', 'c', 'd', 'đ'];
    let danger = ['e', 'f', 'g', 't'];
    let succsess = ['i', 'k', 'm'];
    let cancel = ['o', 'p', 'q'];
    let inverse = ['j', 'z', 'w'];
    let colorClass = 'label-temp'
    if (greenGroup.indexOf(firstChar) >= 0)
        colorClass = 'label-green';
    if (danger.indexOf(firstChar) >= 0)
        colorClass = 'label-orrange'
    if (succsess.indexOf(firstChar) >= 0)
        colorClass = 'label-primary'
    if (inverse.indexOf(firstChar) >= 0)
        colorClass = 'label-inverse'
    if (cancel.indexOf(firstChar) >= 0)
        colorClass = 'label-cancel'
    if (btnRefresh == null)
        btnRefresh = ''
    let statusString = `<span class='label label-sm ${colorClass} arrowed arrowed-righ'>${statusName}</span>`;
    return "<td class='text-left min-w-150'>" + statusString + btnRefresh + "</td>";
}

function setTextForPTag(controlId, value = '') {
    if (isNullOrWhiteSpace(value))
        return;
    document.getElementById(controlId).innerHTML = value;
}
function setCheckboxValue(controlId, boolValue = false) {
    return $('#' + controlId).prop('checked', boolValue);
}
function setCheckedValueOfRadioButtonGroup(name, boolValue) {
    var radios = document.getElementsByName(name);

    for (var j = 0; j < radios.length; j++) {
        if (radios[j].value === boolValue) {
            radios[j].checked = true;
            break;
        }
    }
}