 var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    var tab = $("#tblData").DataTable();
    tab.destroy();

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Menu/GetAll",
            "error": function (error) {
                if (error.status == "400") {
                    toastr.error(error.responseText);
                }
            }
        },
        "columns": [
            { "data": "menuOrder", "width": "15%" },
            
            { "data": "areaName", "width": "15%" },
            { "data": "menuName", "width": "15%" },
          
            //{ "data": "app_Users_Modified.name", "width": "10%" },
            //{
            //    "data": "modifiedDate", "render": function (value) {
            //        if (value === null) return "";
            //        return moment(value).format('DD-MM-YYYY')
            //    }, "width": "15%"  },
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
                "width": "10%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">                             
                                <a href="/Admin/Menu/SubMenu" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="fas fa-solid fa-list"></i>
                                </a>
                            </div>
                           `;
                }, "width": "25%"
            },



            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="Edit(${data})" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                
                                <a onclick=Delete("/Admin/Menu/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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
        url: '/Admin/Menu/Upsert/' + id,
        success: function (result) {
            $("#PlaceHolderHere").html(result);
            $("#addMenu").modal("show");
        },
        "error": function (error) {
            if (error.status == "400") {
                toastr.error(error.responseText);
            }
        }
    })



}