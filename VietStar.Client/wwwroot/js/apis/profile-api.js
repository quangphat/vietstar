
function SearchDatas(apiPath, model, isPopState, callback1 = null, callback2 = null, onPopState = null, onExport = null) {
    
    if (isNullOrUndefined(model) || isNullOrWhiteSpace(apiPath))
        return;

    let query = jQuery.param(model)
    let fullPath = `${apiPath}?${query}`
    
    showBlock($('#panel_body'));
    fetch(fullPath)
        .then(response => response.json())
        .then(response => {
            if (response.success == true) {
                if (callback1 != null) {
                    callback1(response.data.Datas)
                }
                if (callback2 != null) {
                    let totalRecord = response.data.TotalRecord;
                    callback2(model.page, model.limit, totalRecord)
                }
                if (onPopState != null && !isPopState) {
                    onPopState(fullPath)
                }
                if (onExport != null) {
                    onExport(response.data)
                }
            }
            else {
                swal({
                    title: "",
                    text: response.error.code,
                    type: "error",
                    timer: 4000,
                    showConfirmButton: true,
                });
            }
            $('#panel_body').unblock();
        })
        .catch(err => {
            $('#panel_body').unblock();
            swal({
                title: errorMsg,
                text: "Đã có lỗi xảy ra",
                type: "error",
                timer: 4000,
                showConfirmButton: true,
            });
            return null;
        });
}