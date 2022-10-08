var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    var tab = $("#tblData").DataTable();
    tab.destroy();


    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetProducts",
            "error": function (error) {
                if (error.status == "400") {
                    toastr.error(error.responseText);
                }
            }
        },
        "columns": [
            { "data": "country", "width": "15%" },
            { "data": "keyWords", "width": "15%" },
            { "data": "storeName", "width": "15%" },
            { "data": "oderPerDay", "width": "15%" },
            { "data": "productLink", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="Edit('${data}')" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-plus"></i> 
                                </a>
                            
                            </div>
                           `;
                }, "width": "40%"
            }
        ]
    });
}




function Edit(id) {
    var param = "?id=0&Product_ID=" + id
    $.ajax({
        type: 'GET',
        url: '/Admin/Order/Upsert/' + param,
        success: function (result) {
            $("#PlaceHolderHere").html(result);
            $("#addOrder").modal("show");
        },
        "error": function (error) {
            if (error.status == "400") {
                toastr.error(error.responseText);
            }
        }
    })



}
