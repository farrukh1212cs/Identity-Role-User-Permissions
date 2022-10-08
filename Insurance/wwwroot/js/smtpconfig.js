 var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    var tab = $("#tblData").DataTable();
    tab.destroy();

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/SMTPConfigure/GetAll",
            "error": function (error) {
                if (error.status == "400") {
                    toastr.error(error.responseText);
                }
            }
        },
        "columns": [
            { "data": "senderDisplayName", "width": "15%" },
            
            { "data": "senderAddress", "width": "15%" },
            { "data": "email", "width": "15%" }, 
            { "data": "host", "width": "15%" }, 
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="Edit(${data})" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "25%"
            }
        ]
       
    });
}




function Edit(id) {

    $.ajax({
        type: 'GET',
        url: '/Admin/SMTPConfigure/Upsert/' + id,
        success: function (result) {
            $("#PlaceHolderHere").html(result);
            $("#smtpconfig").modal("show");
        },
        "error": function (error) {
            if (error.status == "400") {
                toastr.error(error.responseText);
            }
        }
    })



}