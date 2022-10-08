

function showmodlAdd(id) {


    $.ajax({
        type: 'GET',
        url: '/AppCore/Claim/Upsert/' + id,
        success: function (result) {
            $("#PlaceHolderHere").html(result);
            $("#AddCustProfile").modal("show");

        },
        "error": function (error) {
            if (error.status == "400") {
                alert(error.responseText)
               
             
            }
        }
    })



}
