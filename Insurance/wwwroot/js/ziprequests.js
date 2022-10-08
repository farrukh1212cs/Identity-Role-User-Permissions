var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    var tab = $("#tblData").DataTable();
    tab.destroy();


    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/TeamLead/PostCodeRequest/GetAll",
            "error": function (error) {
                if (error.status == "400") {
                    toastr.error(error.responseText);
                }
            }
        },
        "columns": [
            { "data": "userName", "className": "dt-center" },
            { "data": "zip", "className": "dt-center"},
            {

                "data": "request", "className": "dt-center"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick=Approve("/TeamLead/PostCodeRequest/Approve/${data}") class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-check"></i>
                                </a>




                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 <a onclick=Delete("/TeamLead/PostCodeRequest/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            
                            </div>
                           `;
                }, "className": "dt-center"
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

function Approve(url) {
    swal({
        title: "Are you sure you want to Approve?",
        text: "You want to approve ?",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "Get",
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


