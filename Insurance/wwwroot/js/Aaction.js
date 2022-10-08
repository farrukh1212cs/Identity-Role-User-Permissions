var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    var tab = $("#tblData").DataTable();
    tab.destroy();


    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Aaction/GetAll",
            "error": function (error) {
                if (error.status == "400") {
                    toastr.error(error.responseText);
                }

            }
        },
        "columns": [
            { "data": "menu.menuName", "width": "20%" },
            { "data": "actionName", "width": "20%" } ,     
            {
   
                "data": "isActive",
                "render": function (data) {
                    if (data) {
                      
                        return `<input type="checkbox" disabled checked />`
                    }
                    else {
                        return `<input type="checkbox" disabled/>`
                    }
                },
                "width": "20%"
            },



            {
                "data": "id",

                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="Edit(${data})" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/Admin/Aaction/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                                
                            </div>
                           `;
                }, "width": "25%"
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
        url: '/Admin/Aaction/Upsert/' + id,
        success: function (result) {
            $("#PlaceHolderHere").html(result);
            $("#addAaction").modal("show");
        },
        "error": function (error) {
            if (error.status == "400") {
                toastr.error(error.responseText);
            }
        }
    })



}