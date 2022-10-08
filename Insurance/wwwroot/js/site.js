// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.








// Write your JavaScript code.


$(function () {


    var PlaceHolderElement = $('#PlaceHolderHere');

    $('button[data-toggle="ajax-model"]').click(function (event) {


        var url = $(this).data('url');

        var decodedUrl = decodeURIComponent(url);

        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        })

    })
    PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');

        var actionUrl = form.attr('action');

        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            PlaceHolderElement.find('.modal').modal('hide');
            if (data.success) {
                toastr.success(data.message);
                loadDataTable();
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        })
    })

    PlaceHolderElement.on('click', '[data-save="modal1"]', function (event) {
        var form = $(this).parents('.modal').find('form');

        var actionUrl = form.attr('action');

        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            PlaceHolderElement.find('.modal').modal('hide');
            if (data.success) {
                toastr.success(data.message);

                window.location.href = 'index';
            }
            else {
                toastr.error(data.message);
            }
        })
    })

    PlaceHolderElement.on('click', '[data-save="modal2"]', function (event) {

        var form = $(this).parents('.modal').find('form');

        var actionUrl = form.attr('action');

        var sendData = form.serialize();
        var isvalid = true;

        $.each($(this).parents('.modal').find('form').serializeArray(), function (i, field) {


            if ($("#" + field.name.replace(".", "_")).hasClass("required") == true) {

                if (field.value == "") {

                    $("#span_" + field.name.replace(".", "_")).text("The field is required.");
                    isvalid = false;
                } else {
                    $("#span_" + field.name.replace(".", "_")).text("");
                    isvalid = true;
                }
            }

          
           

        });

        if (isvalid) {
            $.post(actionUrl, sendData).done(function (data) {
                PlaceHolderElement.find('.modal').modal('hide');
                if (data.success) {
                    location.reload();
                    toastr.success(data.message);
                }

            })
        }

    })




})