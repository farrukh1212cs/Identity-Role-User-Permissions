var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {


    var tab = $("#UsertblData").DataTable();
    tab.destroy();

    dataTable = $('#UsertblData').DataTable({
        "ajax": {
            "url": "/Admin/UserRight/GetAll",
            "error": function (error) {
                if (error.status == "400") {
                    toastr.error(error.responseText);
                }
            }
        },
        "columns": [
           
            { "data": "name", "width": "20%" },
            { "data": "email", "width": "20%" },
            { "data": "phoneNumber", "width": "20%" },
            
            {
                "data": "id",

                "render": function (data) {
                    return `
                            <div class="text-center">
                             
                            <a onclick="EditPermission('${data}')" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-tasks"></i>
                                </a>
                            </div>
                           `;
                }, "width": "25%"
            }            
        ]
       
    });
}




function EditPermission(id) {
    $.ajax({
        type: 'GET',
        url: '/Admin/UserRight/EditRolePermission/' + id,
        success: function (result) {
            $("#PlaceHolderHere").html(result);
            $("#EditUserPer").modal("show");
        },
        "error": function (error) {
            if (error.status == "400") {
                toastr.error(error.responseText);
            }
        }
    });

  

}









