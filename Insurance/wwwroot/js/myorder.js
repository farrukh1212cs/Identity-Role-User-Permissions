var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    var tab = $("#tblData").DataTable();
    tab.destroy();


    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll",
            "error": function (error) {
                if (error.status == "400") {
                    toastr.error(error.responseText);
             
                        }
            }
        },
        "columns": [
            { "data": "product.productName", "width": "15%" },
            {
                "data": "order_Date", 'render': function (jsonDate) {
                    var ddd = jsonDate.substr(0, 10);
             
                    var year = ddd.substr(0, 4);
                    var month = ("0" + (ddd.substr(5, 2))).slice(-2);
                    var date = ("0" + (ddd.substr(8, 2))).slice(-2);
                    return ( date + '-' + month + '-' + year);
                } , "width": "15%" },
            { "data": "order_Id", "width": "15%" },
            { "data": "orderScreenShot", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "buyer_Name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="Edit('${data}')" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                            
                            </div>
                           `;
                }, "width": "40%"
            }
        ]
    });
}




function Edit(id) {
    var param = "?id=" + id +"&Product_ID=0"
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
    });



}
$(document).ready(function () {
    var table = $('#tblData').DataTable();
    $('#tblData tbody').on('click', 'tr', function () {
        var data = table.row(this).data();
        var tab = $("#tblDataReviews").DataTable();
        tab.destroy();
        GetReviewDetails(data.id);
    });
});

function GetReviewDetails(id) {

    dataTable = $('#tblDataReviews').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetOrderReview/" + id,
            "error": function (error) {
                if (error.status == "400") {
                    toastr.error(error.responseText);
                }
            }
        },
        "columns": [


            {                
                "data": "review_Date", 'render': function (jsonDate) {
                    var ddd = jsonDate.substr(0, 10);

                    var year = ddd.substr(0, 4);
                    var month = ("0" + (ddd.substr(5, 2))).slice(-2);
                    var date = ("0" + (ddd.substr(8, 2))).slice(-2);

                    return (date + '-' + month + '-' + year);
                }, "width": "15%"

            },
            { "data": "reviewScreenShot", "width": "20%" },
            { "data": "remarks", "width": "20%" },
            { "data": "refundDone", "width": "20%" },
            { "data": "commission", "width": "20%" }
        ]

    });
}