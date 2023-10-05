var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + status },
        scrollX: 150,

        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "25%" },
            { "data": "phoneNumber", "width": "20%" },
            { "data": "applicationUser.email", "width": "25%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a  href="/admin/order/details?orderId=${data}" class="hovera mx-2"><i class="fa-solid fa-pen-to-square"></i> </a>`              
                   
                },
                "width": "15%"
            }
        ]
    });
}
