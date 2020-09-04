var profile_status_key = 'profile_statuses';
var salesbyuser_key = 'salesbyuser';
var couriersbyuser_key = 'couriersbyuser';
var checkdup_partners_key = 'checkdup-partners';
var partners_key = 'partners';
var provinces_key = 'provinces';
var approvegroups_key = 'approvegroups';
var groups_key = 'groups';
var parent_groups_key = 'parent-groups';
var mcredit_location_key = 'mcredit-location'
var mcredit_period_key = 'mcredit-period';
var mcredit_city_key = 'mcredit-city';
var mcredit_product = 'mcredit-product';
var all_employee_key = 'all_employee_key';
var employee_role_key ='employee_roles'

function setLocalStorage(key, data) {
    window.localStorage.removeItem(key);
    window.localStorage.setItem(key, JSON.stringify(data));
}
function getLocalStorage(key) {
    return JSON.parse(window.localStorage.getItem(key))
}

function removeAllGroup_Cache() {
    window.localStorage.removeItem(approvegroups_key);
    window.localStorage.removeItem(groups_key);
    window.localStorage.removeItem(parent_groups_key);
}
function getComments(profileId, profileType, commentDisplayControl) {
    if (isNullOrUndefined(profileType) || isNullOrUndefined(profileId))
        return

    $.ajax({
        type: "GET",
        url: `/comment/${profileId}/${profileType}`,
        success: function (data) {

            if (data.data != null && data.success == true) {
                commentDisplayControl.empty();
                $.each(data.data, function (index, item) {
                    appendComment(commentDisplayControl, item.Content, item.Commentator, item.CommentTime)
                });
            }
        },
        complete: function () {

        },
        error: function (jqXHR, exception) {
            //showError(jqXHR, exception);
        }
    });

}
function appendComment(control, content, commenttator, commenttime) {
    if (isNullOrWhiteSpace(content))
        return
    control.append(
        '<div class="timeline-event-content  active">' +
        '<div class="timeline-item">' +
        '<div class="timeline-body">' +
        '<div class="timeline__message-container">' +
        '<strong>' + commenttator + ' (' + commenttime + '): </strong><span>' + content + '</span>' +
        '</div></div></div></div>'
    )
}
function AddNote(profileId, profileType, content, commentBox, commentDisplayControl) {

    if (isNullOrWhiteSpace(profileType) || isNullOrWhiteSpace(content))
        return

    let data = JSON.stringify({
        'ProfileId': profileId,
        'ProfileTypeId': profileType,
        'Content': content
    })
    $.ajax({
        type: "POST",
        url: `/comment`,
        data: data,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success == true) {
                swal({
                    title: "",
                    text: "Thành công",
                    type: "success",
                    timer: 4000,
                    showConfirmButton: true,
                });
                commentBox.val('')
                getComments(profileId, profileType, commentDisplayControl)
            }
            else {
                swal({
                    title: "",
                    text: data.error.code,
                    type: "error",
                    timer: 4000,
                    showConfirmButton: true,
                });
            }
        },
        complete: function () {

        },
        error: function (jqXHR, exception) {
            //showError(jqXHR, exception);
        }
    });

}

function getRoles(control, value = 0, defaultText = "Tất cả") {
    control.empty();
    control.append('<option value="0">' + defaultText + '</option > ');
    let data = getLocalStorage(employee_role_key);
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (value != null) {
            control.val(value);
        }
        return;
    }
    $.ajax({
        type: "GET",
        url: '/employees/roles',
        success: function (data) {
            if (data.data != null && data.success == true) {
                setLocalStorage(employee_role_key, data.data)
                $.each(data.data, function (index, optionData) {
                    $(control).append("<option value='" + optionData.Id + "'>" + optionData.Name + "</option>");
                });
            }
            if (value > 0) {
                $(control).val(value);
            }
           
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function renderStatusList(profileType, value = null) {
    if (isNullOrWhiteSpace(profileType))
        return
    let control = $('#ddlStatus')
    control.empty();
    let data = getLocalStorage(`${profile_status_key}-${profileType}`);
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (value != null) {
            control.val(value);
        }
        return;
    }
    $.ajax({
        type: "GET",
        url: `/Common/GetStatusList/${profileType}`,
        data: {},
        success: function (data) {
            $('#ddlStatus').append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                setLocalStorage(`${profile_status_key}-${profileType}`, data.data)
                $.each(data.data, function (index, item) {
                    $('#ddlStatus').append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (value != null) {
                    //let uniqueSet = new Set(value);
                    //let backToArray = [...uniqueSet]
                    $("#ddlStatus").val(value);
                }

            }
        },
        complete: function () {

        },
        error: function (jqXHR, exception) {
            //showError(jqXHR, exception);
        }
    });

}
function GetSales(control = null, defaultValue = 0) {
    if (control == null)
        control = $("#saleId");
    control.empty();
    control.append("<option value='0'>Chọn sale</option>");
    let data = getLocalStorage(salesbyuser_key);
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (defaultValue != null) {
            control.val(defaultValue);
        }
        return;
    }
    $.ajax({
        type: "GET",
        url: '/common/sales',
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn sale</option>");
            if (data.data != null && data.success == true) {
                setLocalStorage(salesbyuser_key, data.data)
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null) {
                    control.val(defaultValue);
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetCouriers(control = null, defaultValue = 0) {
    if (control == null)
        control = $("#courierId");
    control.empty();
    control.append("<option value='0'>Chọn courier</option>");
    let data = getLocalStorage(couriersbyuser_key);
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (defaultValue != null) {
            control.val(defaultValue);
        }
        return;
    }
    $.ajax({
        type: "GET",
        url: '/common/couriers',
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn courier</option>");
            if (data.data != null && data.success == true) {
                setLocalStorage(couriersbyuser_key, data.data)
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null) {
                    control.val(defaultValue);
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetCheckDupPartners(control = null, defaultValue = 0) {
    if (control == null)
        control = $("#partnerId");
    control.empty();
    control.append("<option value='0'>Chọn đối tác</option>");
    let data = getLocalStorage(checkdup_partners_key);
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });

        if (defaultValue > 0) {

            control.val(defaultValue);

        }
        return;
    }
    $.ajax({
        type: "GET",
        url: '/common/checkdup-partners',
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn đối tác</option>");
            if (data.data != null && data.success == true) {
                setLocalStorage(checkdup_partners_key, data.data)
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null && defaultValue > 0) {
                    control.val(defaultValue);

                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetPartners(control = null, defaultValue = 0, productId = 0) {
    if (control == null)
        control = $("#partnerId");
    control.empty();
    control.append("<option value='0'>Chọn đối tác</option>");
    let data = getLocalStorage(partners_key);
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (defaultValue != null && defaultValue > 0) {
            control.val(defaultValue);
            GetProducts(defaultValue, null, productId)
        }
        return;
    }
    $.ajax({
        type: "GET",
        url: '/common/partners',
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn đối tác</option>");
            if (data.data != null && data.success == true) {
                setLocalStorage(partners_key, data.data)
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null && defaultValue > 0) {
                    control.val(defaultValue);
                    GetProducts(defaultValue, null, productId)
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetProducts(partnerId, control = null, defaultValue = 0) {
    if (control == null)
        control = $("#productId");
    control.empty();
    $.ajax({
        type: "GET",
        url: `/common/products/${partnerId}`,
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn sản phẩm</option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null) {
                    control.val(defaultValue);
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetProvinces(control = null, defaultValue = 0, districId = 0) {
    if (control == null)
        control = $("#provinceId");
    control.append("<option value='0'>Chọn tỉnh/thành</option>");
    let data = getLocalStorage(provinces_key);
    if (data != null && !isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (defaultValue != null && defaultValue > 0) {
            control.val(defaultValue);
            GetDistricts(defaultValue, null, districId)
        }
        return;
    }
    control.empty();
    $.ajax({
        type: "GET",
        url: '/common/provinces',
        data: {},
        success: function (data) {

            if (data.data != null && data.success == true) {
                setLocalStorage(provinces_key, data.data)
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null && defaultValue > 0) {
                    control.val(defaultValue);
                    GetDistricts(defaultValue, null, districId)
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetDistricts(provinceId, control = null, defaultValue = 0) {
    if (control == null)
        control = $("#districtId");
    control.empty();
    $.ajax({
        type: "GET",
        url: `/common/districts/${provinceId}`,
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn quận/huyện</option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null && defaultValue > 0) {
                    control.val(defaultValue);
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetApproveGroupByUser(control = null, defaultValue = 0, followControl = null, followValue = 0) {
    if (control == null)
        control = $("#groupId");

    control.empty();
    control.append("<option value='0'>Chọn nhóm</option>");
    let data = getLocalStorage(approvegroups_key)
    if (data != null) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "(" + item.ShortName + ")</option>");
        });
        if (defaultValue != null && defaultValue > 0) {
            control.val(defaultValue);
            if (followControl != null) {
                GetMemberByGroup(defaultValue, followControl, followValue)
            }

        }
        return
    }
    $.ajax({
        type: "GET",
        url: '/Groups/ApproveGroupsByUserId',
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn nhóm</option>");
            if (data.data != null && data.success == true) {
                setLocalStorage(approvegroups_key, data.data)
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "(" + item.ShortName + ")</option>");
                });
                if (defaultValue != null) {
                    control.val(defaultValue);
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}
function GetGroupByUser(control = null, defaultValue = 0) {
    if (control == null)
        control = $("#groupId");
    control.empty();
    control.append("<option value='0'>Chọn nhóm</option>");
    let data = getLocalStorage(groups_key)
    if (data != null) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "(" + item.ShortName + ")</option>");
        });
        if (defaultValue != null && defaultValue > 0) {
            control.val(defaultValue);
            // GetMemberByGroup(defaultValue, null,)
        }
        return
    }
    $.ajax({
        type: "GET",
        url: '/Groups/GroupsByUserId',
        data: {},
        success: function (data) {
            if (data.data != null && data.success == true) {
                setLocalStorage(groups_key, data.data)
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "(" + item.ShortName + ")</option>");
                });
                if (defaultValue != null && defaultValue > 0) {
                    control.val(defaultValue);
                    // GetMemberByGroup(defaultValue, null,)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function GetParentGroups(control = null, defaultValue = 0, followControl = null, followValue = 0) {
    if (control == null)
        control = $("#groupId");
    control.empty();

    control.append("<option value='0'>Chọn nhóm</option>");
    let data = getLocalStorage(parent_groups_key)
    if (data != null) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (defaultValue != null && defaultValue > 0) {
            control.val(defaultValue);
            if (followControl != null) {
                GetMemberByGroup(defaultValue, followControl, followValue)
            }
                
        }
        return
    }
    $.ajax({
        type: "GET",
        url: '/Groups/ParentGroups',
        data: {},
        success: function (data) {
            if (data.data != null && data.success == true) {
                setLocalStorage(parent_groups_key, data.data)
                $.each(data.data, function (index, item) {
                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null && defaultValue > 0) {
                    control.val(defaultValue);
                    GetMemberByGroup(defaultValue, followControl, defaultValue)
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function GetMemberByGroup(groupId, control = null, defaultValue = 0) {
    if (isNullOrUndefined(groupId) || isNullOrWhiteSpace(groupId) || groupId == 0)
        return;
    if (control == null)
        control = $("#memberId");
    debugger
    control.empty();
    $.ajax({
        type: "GET",
        url: `/Employees/GetByGroupId/${groupId}`,
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn thành viên</option>");
            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {

                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null && defaultValue > 0) {
                    control.val(defaultValue);
                }

                //control.chosen().trigger("chosen:updated").change();
            }

        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function GetEmployeeByProvinceId(provinceId, control = null, defaultValue = 0) {
    if (isNullOrUndefined(provinceId) || isNullOrWhiteSpace(provinceId) || provinceId == 0)
        return;
    if (control == null)
        control = $("#courierId");
    control.empty();
    $.ajax({
        type: "GET",
        url: `/Employees/GetByProvinceId/${provinceId}`,
        data: {},
        success: function (data) {
            control.append("<option value='0'>Chọn courier</option>");

            if (data.data != null && data.success == true) {
                $.each(data.data, function (index, item) {

                    control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
                if (defaultValue != null && defaultValue > 0) {
                    control.val(defaultValue);
                }

                //control.chosen().trigger("chosen:updated").change();
            }

        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function renderOneItemFile(model, className = '',
    _initialPreview = [],
    _initialPreviewConfig = [],
    allowUpload = true,
    continueUpload = true
) {

    let header = ''
    if (!isNullOrUndefined(model.titleName)) {
        if (model.isRequire) {
            header = '<h5  class="header green ' + className + '">' + model.titleName + '<span class="required">(*)</span></h5>';
        }
        else {
            header = '<h5  class="header green ' + className + '">' + model.titleName + '<span > </span></h5>';
        }
    }

    let guidId = isNullOrWhiteSpace(model.guidId) ? getNewGuid() : model.guidId;
    let content = header + "<div class='col-sm-3'> ";
    content += "<div class=\"file-loading\">";
    content += "<input class='attachFile' key=" + model.key + " id=\"attachFile-" + model.itemId + "\" type=\"file\">";
    content += "</div>";
    content += "</div>";
    $('#tailieu-' + model.key).append(content);

    let item = $("#attachFile-" + model.itemId);

    let uploadUrl = '';
    if (model.isMCredit) {

        let params = `key=${model.key}&profileId=${model.profileId}&fileId=${model.fileId}&guidId=${guidId}&docCode=${model.documentCode}&docId=${model.documentId}&groupId=${model.groupId}&mcId=${model.mcId}`;
        uploadUrl = `/media/UploadMCredit?${params}`
    }
    else {
        
        let params = `key=${model.key}&profileType=${model.profileType}&profileId=${model.profileId}&fileId=${model.fileId}&guidId=${guidId}`;
        uploadUrl = `/media/UploadFile?${params}`
    }
    item.fileinput({
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
        showDownload: true,
        showUpload: false, // hide upload button
        showRemove: false, // hide remove button
        browseOnZoneClick: true,
        removeLabel: '',
        fileId: model.fileId,
        outsideGuidId: `'${guidId}'`,
        btnDeleteId: `btnRemoveFile-${guidId}`,
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
            showUpload: true,
            showZoom: true,
            showDrag: false
        },
        append: true

    }).on("filebatchselected", function (event, files) {

        //if (countFilesByKey(filesUploaded, parseInt(model.key)) >= 50)
        //    return;
        $(item).fileinput("upload");
    }).on("filebeforedelete", function () {

        //if (onDelete !== null) {
        //    onDelete(key, fileId, isFileExist);
        //}
    }).on('filebatchuploadsuccess', function (event, data) {

        if (continueUpload === true && model.fileId <= 0) {
            let newItem = { ...model }
            newItem.guidId = getNewGuid
            newItem.fileId = 0
            newItem.titleName = ''
            renderOneItemFile({
                key: model.key,
                type: model.type,
                profileType: model.profileType,
                profileId: model.profileId,
                fileId: 0,
                isRequire: false,
                titleName: '',
                guidId: '',
                documentName: model.documentName,
                documentCode: model.documentCode,
                documentId: model.documentId,
                groupId: model.groupId,
                isMCredit: model.isMCredit,
                mcId: model.mcId
            }, '', [], [], true, continueUpload);
        }
    });

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
function onDeleteFile(fileId, guidId) {
    $.ajax({
        type: "POST",
        url: `/media/delete/${fileId}/${guidId}`,
        success: function (data) {

            if (data.data != null && data.success == true) {

            }

        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function GetLocSigns(controlId, defaultValue = 0) {

    if (controlId == null)
        return;
    $(controlId).empty();
    let data = getLocalStorage(mcredit_location_key)
    if (data != null) {
        $.each(data, function (index, item) {
            $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
        });
        if (!isNullOrWhiteSpace(defaultValue) && defaultValue > 0) {
            $(controlId).val(defaultValue);
        }
        return
    }
    $.ajax({
        type: "GET",
        url: '/MCredit/GetMCSimpleList?type=location',
        data: {},
        success: function (data) {
            $(controlId).append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                setLocalStorage(mcredit_location_key, data.data)
                $.each(data.data, function (index, item) {
                    $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
                });
                if (!isNullOrWhiteSpace(defaultValue)) {
                    $(controlId).val(defaultValue);
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function GetLoanPeriods(controlId, defaultValue = 0) {

    if (controlId == null)
        return;
    $(controlId).empty();
    let data = getLocalStorage(mcredit_period_key)
    if (!isNullOrNoItem(data)) {
        $(controlId).append("<option value='0'>Vui lòng chọn kỳ hạn vay</option>");
        $.each(data, function (index, item) {
            $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
        });
        if (!isNullOrWhiteSpace(defaultValue) && defaultValue > 0) {
            $(controlId).val(defaultValue);
        }
        return
    }
    $.ajax({
        type: "GET",
        url: '/MCredit/GetMCSimpleList?type=loanperiod',
        data: {},
        success: function (data) {

            if (data.data != null && data.success == true) {
                setLocalStorage(mcredit_period_key, data.data)
                $.each(data.data, function (index, item) {
                    $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
                });
                if (!isNullOrWhiteSpace(defaultValue)) {
                    $(controlId).val(defaultValue);
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function GetLoanCities(controlId, defaultValue = 0) {

    if (controlId == null)
        return;
    $(controlId).empty();
    let data = getLocalStorage(mcredit_city_key)
    if (!isNullOrNoItem(data)) {
        $.each(data, function (index, item) {
            console.log(item.Id)
            $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
        });
        if (!isNullOrWhiteSpace(defaultValue) && defaultValue > 0) {
            $(controlId).val(defaultValue);
        }
        return
    }
    $.ajax({
        type: "GET",
        url: '/MCredit/GetMCSimpleList?type=city',
        data: {},
        success: function (data) {
            $(controlId).append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                setLocalStorage(mcredit_city_key, data.data)
                $.each(data.data, function (index, item) {
                    $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
                });
                if (!isNullOrWhiteSpace(defaultValue)) {
                    $(controlId).val(defaultValue);
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function GetLoanProducts(controlId, defaultValue = 0) {

    if (controlId == null)
        return;
    $(controlId).empty();
    let data = getLocalStorage(mcredit_product)
    if (data != null) {
        $.each(data, function (index, item) {
            $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
        });
        if (!isNullOrWhiteSpace(defaultValue) && defaultValue > 0) {
            $(controlId).val(defaultValue);
        }
        return
    }
    $.ajax({
        type: "GET",
        url: '/MCredit/GetMCSimpleList?type=product',
        data: {},
        success: function (data) {
            $(controlId).append("<option value='0'></option>");
            if (data.data != null && data.success == true) {
                setLocalStorage(mcredit_product, data.data)
                $.each(data.data, function (index, item) {
                    $(controlId).append("<option value='" + item.Code + "'>" + item.Name + "</option>");
                });
                if (!isNullOrWhiteSpace(defaultValue)) {
                    $(controlId).val(defaultValue);
                }

            }
        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}

function GetAllEmployees(control, defaultValue = 0) {
    if (control == null)
        control = $("#employeeId");
    control.empty();
    control.append("<option value='0'>Chọn nhân viên</option>");
    let data = getLocalStorage(all_employee_key)
    if (data != null) {
        $.each(data, function (index, item) {
            control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        if (defaultValue != null && defaultValue > 0) {
            control.val(defaultValue);
        }
        return
    }

    $.ajax({
        type: "GET",
        url: `/employees/all`,
        data: {},
        success: function (data) {
            if (data.data != null && data.success == true) {
                if (data.data != null && data.data.length > 0) {
                    setLocalStorage(all_employee_key, data.data)
                    $.each(data.data, function (index, item) {
                        control.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                    });
                    if (data.data.length == 1) {
                        control.val(data.data[0].Id);
                    }
                    else {
                        if (defaultValue != null && defaultValue > 0)
                            control.val(defaultValue);
                    }
                }
            }

        },
        complete: function () {
        },
        error: function (jqXHR, exception) {
            showError(jqXHR, exception);
        }
    });
}