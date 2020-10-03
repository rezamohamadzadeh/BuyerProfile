$(function () {
    var placeholder = $("#modal-placeholder");

    $(document).on('click', 'a[data-toggle="add-ajax-modal"]', function () {
        var url = $(this).data('url');
        $.ajax({
            url: url,
            
        }).done(function (result) {
            placeholder.html(result);
            placeholder.find('.modal').modal('show');
        });
    });
    $(document).on('click', 'button[data-toggle="add-ajax-modal"]', function () {
        var url = $(this).data('url');
        $.ajax({
            url: url,
            error: function (jqXHR, ) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'network error';
                } else if (jqXHR.status == 404) {
                    msg = 'not found';

                } else if (jqXHR.status == 401) {
                    msg = 'unauthorized';
                    //window.location.href = 'http://localhost:51056/Account/Login';
                } else if (jqXHR.status == 500) {
                    msg = 'intevral error';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                Swal.fire({ title: "Error", html: msg, icon: "error", showCancelButton: 0, confirmButtonColor: "#3051d3" })
            }
        }).done(function (result) {
            placeholder.html(result);
            placeholder.find('.modal').modal('show');
        });
    });
    // edit as modal
    $(document).on('click', 'button[data-toggle="edit-ajax-modal"]', function () {
        var url = $(this).data('url');
        $.ajax({
            url: url,
            error: function (jqXHR) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'network error';
                } else if (jqXHR.status == 404) {
                    msg = 'not found';

                } else if (jqXHR.status == 401) {
                    msg = 'unauthorized';
                } else if (jqXHR.status == 500) {
                    msg = 'intevral error';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                Swal.fire({ title: "Error", html: msg, icon: "error", showCancelButton: 0, confirmButtonColor: "#3051d3" })
            }
        }).done(function (result) {
            placeholder.html(result);
            placeholder.find('.modal').modal('show');
        });
    });
});

function DeleteData(url,id) {

    Swal.fire({
        title: "Delete Support",
        text: "Are you sure to delete selected support?",
        type: "question",
        showCancelButton: !0,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes",
        cancelButtonText: "No"

    }).then(function (t) {
        
        if (t.value) {
            $.ajax({
                url: url,
                data: { id: id },
                type: 'Post',
                dataType: "json",
                success: function (data) {
                    if (data.success != null) {
                        $('#myTable').DataTable().ajax.reload();
                        Swal.fire({ title: "Successfully", text: data.success, icon: "success", showCancelButton: 0, confirmButtonColor: "#3051d3" })
                    }
                    else if (data.warning != null)
                        Swal.fire({ title: "Warning", html: data.warning, icon: "warning", showCancelButton: 0, confirmButtonColor: "#3051d3" })
                    else {
                        Swal.fire({ title: "Error", html: data.error, icon: "error", showCancelButton: 0, confirmButtonColor: "#3051d3" })
                    }
                },
                error: function () {

                }
            });

        }

    })

}
var AddSuccess = function (context) {
    if (context.error != null) {
        Swal.fire({ title: "Error", html: context.error, icon: "error", showCancelButton: 0, confirmButtonColor: "#3051d3" })
        return;
    }
    if (context.warning != null) {
        Swal.fire({ title: "Warning", html: context.warning, icon: "warning", showCancelButton: 0, confirmButtonColor: "#3051d3" })
        return;
    }
    if (context.success != null) {
        Swal.fire({ title: "Successfully", html: context.success, icon: "success", showCancelButton: 0, confirmButtonColor: "#3051d3" })
        $('#myTable').DataTable().ajax.reload();
        ResetForm();
        return;
    }
    $('#myTable').DataTable().ajax.reload();
    ResetForm();
    Swal.fire({ title: "Successfully", text: "Information added successfully", icon: "success", showCancelButton: 0, confirmButtonColor: "#3051d3" })
};
var EditSuccess = function (context) {
    if (context.error != null) {
        Swal.fire({ title: "Error", html: context.error, icon: "error", showCancelButton: 0, confirmButtonColor: "#3051d3" })
        return;
    }
    if (context.warning != null) {
        Swal.fire({ title: "Warning", html: context.warning, icon: "warning", showCancelButton: 0, confirmButtonColor: "#3051d3" })
        return;
    }

    $('#myTable').DataTable().ajax.reload();
    Swal.fire({ title: "Successfully", text: "Information edited successfully", icon: "success", showCancelButton: 0, confirmButtonColor: "#3051d3" })
    $("#modal-placeholder").find('.modal').modal('hide');
};

var AddFailed = function (context) {
    Swal.fire({ title: "Error", html: "Error in add item", icon: "error", showCancelButton: 0, confirmButtonColor: "#3051d3" })
};
var EditFailed = function (context) {
    Swal.fire({ title: "Error", html: "Error in edit item", icon: "error", showCancelButton: 0, confirmButtonColor: "#3051d3" })
};
function ResetForm() {
    document.getElementById("form").reset();
    $(".select2 > option").removeAttr("selected");
    $(".select2").trigger("change");
}
