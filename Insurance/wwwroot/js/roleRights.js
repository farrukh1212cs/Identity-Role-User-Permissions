var dataTable;

$(document).ready(function () {

    loadRolesDataTable();
});

function loadRolesDataTable() {


    var tab = $("#RolestblData").DataTable();
    tab.destroy();

    dataTable = $('#RolestblData').DataTable({
        "ajax": {
            "url": "/Admin/RoleRight/GetRoles"
        },
        "columns": [
           
            { "data": "name", "width": "80%" },
          
            {
                "data": "id",

                "render": function (data) {
                    return `
                            <div class="text-center">
                               
                                 <a onclick="EditPermission('${data}')" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                
                            </div>
                           `;
                }, "width": "20%"
            }
        ]
       
    });
};

function EditPermission(id) {
    $.ajax({
        type: 'GET',
        url: '/Admin/RoleRight/EditRolePermission/' + id,
        success: function (result) {
            $("#PlaceHolderHere").html(result);
            $("#EditRolePer").modal("show");
        },
        "error": function (error) {
            if (error.status == "400") {
                toastr.error(error.responseText);
            }
        }
    }) 

}






