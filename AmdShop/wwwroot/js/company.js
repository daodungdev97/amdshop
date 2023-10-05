var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall' },
        scrollX: 150,       
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "streetAddress", "width": "20%" },
            { "data": "city", "width": "20%" },
            { "data": "state", "width": "20%" },
            { "data": "phoneNumber", "width": "20%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/company/upsert?id=${data}" title = 'Update' class="hovera mx-2"><i class="fa-solid fa-pen-to-square"></i> </a>               
                     <a onClick=Delete('/admin/company/delete/${data}') title = 'Delete' class=" hovera mx-2 "> <i class="fa-solid fa-trash text-danger"></i> </a>
                    </div>`
                },
                "width": "15%"
            }
        ]
    });
}


function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "Do you want to delete this",
        icon: 'warning',
        iconHtml: '<i class="fa fa-trash" style="color:#d33 "; > </i>',
        iconColor: '#d33',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
    }).then((result) => {
        if (result.isConfirmed) {
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                url: url,
                type: 'DELETE',
                headers: { 'RequestVerificationToken': token },   
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}



