function getCICMessage(data) {
    if (data == null || data == undefined)
        return ''
    if (data.status == "error")
        return data.msg
    return data.Result
}

function checkDup(controlId, value) {
    var objectSend = JSON.stringify({
        'Value': value
    });
    $.ajax({
        traditional: true,
        url: '/MCredit/CheckDupApi',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: objectSend,
        success: function (data) {
            if (data.success == true) {
                if (data.data.status == "success") {
                    document.getElementById(controlId).innerHTML = `${data.data.result} - ${data.data.desc}`;
                    swal({
                        title: "Thành công",
                        text: `${data.data.result} - ${data.data.desc}`,
                        type: "success",
                        timer: 4000,
                        showConfirmButton: true,
                    }, function () {

                    });
                }
                else {
                    document.getElementById(controlId).innerHTML = data.error.code;
                    swal({
                        title: "Đã có lỗi xảy ra",
                        text: data.error.code,
                        type: "error",
                        timer: 4000,
                        showConfirmButton: true,
                    });
                }

            }
            else {
                swal({
                    title: "Đã có lỗi xảy ra",
                    text: data.error.code,
                    type: "error",
                    timer: 4000,
                    showConfirmButton: true,
                });
            }
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        },
        complete: function () {
            $('#panel_body').unblock();
        },
    });


}
function checkCIC(controlId, value, name) {
    if (isNullOrWhiteSpace(value))
        return;
    var objectSend = JSON.stringify({
        'IdNumber': value,
        'Name': name
    });
    $.ajax({
        traditional: true,
        url: '/MCredit/CheckCICApi',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: objectSend,
        success: function (data) {
            if (data.success == true) {
                swal({
                    title: "Thành công",
                    text: `${getCICMessage(data.data)}`,
                    type: "success",
                    timer: 4000,
                    showConfirmButton: true,
                }, function () {

                });
            }
            else {
                swal({
                    title: "Đã có lỗi xảy ra",
                    text: data.error.code,
                    type: "error",
                    timer: 4000,
                    showConfirmButton: true,
                });
            }
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        },
        complete: function () {
            $('#panel_body').unblock();
        },
    });


}
function checkCAT(controlId, value) {

    var objectSend = JSON.stringify({
        'Value': value
    });
    $.ajax({
        traditional: true,
        url: '/MCredit/CheckCatApi',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: objectSend,
        success: function (data) {
            
            if (data.success == true) {
                swal({
                    title: "Thành công",
                    text: data.error.code,
                    type: "success",
                    timer: 4000,
                    showConfirmButton: true,
                }, function () {

                });
            }
            else {
                swal({
                    title: "Đã có lỗi xảy ra",
                    text: data.error.code,
                    type: "error",
                    timer: 4000,
                    showConfirmButton: true,
                });
            }
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        },
        complete: function () {
            $('#panel_body').unblock();
        },
    });
}
function checkSale(controlId, value, controlToSetId = null, profileId = 0) {

    var objectSend = JSON.stringify({
        'SaleCode': value,
        'ProfileId': profileId
    });
    $.ajax({
        traditional: true,
        url: '/MCredit/CheckSaleApi',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: objectSend,
        success: function (data) {
            if (data.success == true) {
                swal({
                    title: "Thành công",
                    text: data.error.code,
                    type: "success",
                    timer: 4000,
                    showConfirmButton: true,
                }, function () {
                    //document.getElementById(con)
                    document.getElementById(controlId).innerHTML ='Thành công'
                    $(controlToSetId).val(data.data.obj.id);
                    $("#nameSale").text("Tên sale: " + data.data.obj.name);
                });
                
            }
            else {
                swal({
                    title: "Đã có lỗi xảy ra",
                    text: data.error.code,
                    type: "error",
                    timer: 4000,
                    showConfirmButton: true,
                });
                document.getElementById(controlId).innerHTML = data.error.code;
            }
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        },
        complete: function () {
            $('#panel_body').unblock();
        },
    });
}


function renderOneItemFile_MCredit(model, className = '', generateInput = false,
    _initialPreview = [],
    _initialPreviewConfig = [],
    allowUpload = false,
    isFileExist = false,
    onUpload = null,
    onDelete = null,
    filesUploaded = []
) {
    if (model == null || model == undefined)
        return;
    let content = "<div class='col-sm-3'";

    if (!isNullOrUndefined(model.titleName)) {
        if (model.isRequire) {
            content += '<h5  class="header green ' + className + '">' + model.titleName + '<span class="required">(*)</span></h5>';
        }
        else {
            content += '<h5  class="header green ' + className + '">' + model.titleName + '<span > </span></h5>';
        }
    }
    let existDocumentIds = filesUploaded.filter(p => p.documentId == model.documentId);
    let orderId = existDocumentIds == null || existDocumentIds.length <= 0 ? 1 : existDocumentIds.length + 1;
    content += "<div class=\"file-loading\">";
    content += "<input class='attachFile' key=" + model.key + " id=\"attachFile-" + model.fileId + "\" type=\"file\">";
    content += "</div>";
    content += "</div>";
    $('#tailieu-' + model.key).append(content);
    if (generateInput === true) {
        let item = $("#attachFile-" + model.fileId);

        model.fileId = (isFileExist === true) ? model.fileId : getNewGuid();
        let uploadUrl = isFileExist === true ? `/MCredit/UploadFile?key=${model.key}&fileId=${model.fileId}
    &orderId=${orderId}&profileId=${model.profileId}&documentName=${model.documentName}&documentCode=${model.documentCode}&documentId=${model.documentId}&groupId=${model.groupId}&mcId=${model.mcId}`
            : `/MCredit/UploadFile?key=${model.key}&fileId=0&orderId=${orderId}&profileId=${model.profileId}
                &documentName=${model.documentName}&documentCode=${model.documentCode}&documentId=${model.documentId}&groupId=${model.groupId}&mcId=${model.mcId}`;
        if (allowUpload == false)
            uploadUrl = null;

        $(item).fileinput({
            uploadUrl: allowUpload === true ? uploadUrl : null,
            validateInitialCount: true,
            maxFileSize: 25 * 1024,
            msgSizeTooLarge: 'File "{name}" (<b>{size} KB</b>)'
                + 'exceeds maximum allowed upload size of <b>{25} MB</b>. '
                + 'Please retry your upload!',
            allowedFileExtensions: ['png', 'jpg', 'pdf'],
            initialPreviewAsData: true, // identify if you are sending preview data only and not the raw markup
            initialPreviewFileType: 'image',
            overwriteInitial: false,
            showUploadedThumbs: false,
            uploadAsync: false,
            showClose: false,
            showCaption: false,
            showBrowse: false,
            showUpload: false, // hide upload button
            showRemove: false, // hide remove button
            browseOnZoneClick: true,
            removeLabel: '',
            fileId: model.fileId,
            btnDeleteId: 'btn-remove-file-' + model.fileId,
            dropZoneTitle: 'Kéo và thả tập tin vào đây',
            dropZoneClickTitle: '<br>(hoặc nhấp để chọn)',
            removeIcon: '<i class="glyphicon glyphicon-remove"></i>',
            removeTitle: 'Cancel or reset changes',
            elErrorContainer: '#kv-avatar-errors-2',
            msgErrorClass: 'alert alert-block alert-danger',
            //layoutTemplates: { main2: '{preview} ' + btnCust + ' {remove} {browse}' },
            //layoutTemplates: { footer: '' },
            initialPreview: _initialPreview,
            initialPreviewDownloadUrl: _initialPreview,
            initialPreviewConfig: _initialPreviewConfig,
            fileActionSettings: {
                showDownload: true,
                showRemove: true,
                showUpload: false,
                showZoom: true,
                showDrag: false
            },
            append: true

        }).on("filebatchselected", function (event, files) {
            if (countFilesByKey(filesUploaded, parseInt(model.key)) >= 50)
                return;
            $(item).fileinput("upload");
        }).on("filebeforedelete", function (event, key2) {

            if (onDelete !== null) {
                onDelete(model.key, model.fileId, isFileExist);
            }
        }).on('filebatchuploadsuccess', function (event, data) {
            if (onUpload !== null) {
                onUpload(model, data.response, isFileExist);
            }
        });

    }

}
function isReach5Files(filesUpload, key) {
    if (isNullOrWhiteSpace(key))
        return true;
    if (isNullOrNoItem(filesUpload))
        return false;
    let sameKeyFile = filesUpload.find(p => p.key === key);
    if (isNullOrUndefined(sameKeyFile))
        return false;
    if (isNullOrNoItem(sameKeyFile.files))
        return 0;
    if (sameKeyFile.files.length === 50)
        return true;
    return false;
}
function countFilesByKey(filesUpload, key) {
    if (isNullOrWhiteSpace(key))
        return 0;
    let sameKeyFile = filesUpload.find(p => p.key === key);
    if (isNullOrUndefined(sameKeyFile))
        return 0;
    if (isNullOrNoItem(sameKeyFile.files))
        return 0;
    return sameKeyFile.files.length;
}