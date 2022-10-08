var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    var tab = $("#tblData").DataTable();
    tab.destroy();


    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll",
            "error": function (error) {
                if (error.status == "400") {
                    toastr.error(error.responseText);
                }
            }
        },
        "columns": [
            { "data": "name", "width": "15%" },
          
            {
                "data": "Id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="Edit('${data}')" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/Admin/Product/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "40%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                },
                "error": function (error) {
                    if (error.status == "400") {
                        toastr.error(error.responseText);
                    }
                }
            });
        }
    });
}


function Edit(id) {

    $.ajax({
        type: 'GET',
        url: '/Admin/Product/Upsert/' + id,
        success: function (result) {
            $("#PlaceHolderHere").html(result);
            $("#addProduct").modal("show");
        },
        "error": function (error) {
            if (error.status == "400") {
                toastr.error(error.responseText);
            }
        }
    })



}
