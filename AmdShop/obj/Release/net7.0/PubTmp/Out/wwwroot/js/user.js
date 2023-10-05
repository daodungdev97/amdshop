var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        scrollX: 150,

        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "email", "width": "20%" },
            { "data": "phoneNumber", "width": "20%" },
            { "data": "company.name", "width": "20%" },
            { "data": "role", "width": "20%" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {//hết thời hạn lock, cần khóa lại
                        return `<div class="w-75 btn-group" role="group">                  
                                 <a onclick=LockUnlock('${data.id}') class=" hovera mx-2" style="cursor:pointer; title="LOCK"">
                                  <i class="fa-solid fa-lock-open"></i>
                                </a> 
                                <a href="/admin/user/RoleManagement?userId=${data.id}" class="hovera mx-2" style="cursor:pointer; title="PERMISSION"">
                                 <i class="fa-solid fa-pen-to-square"></i>
                                </a>
                    </div>`

                    }
                    else {
                        return `
                        <div class="w-75 btn-group" role="group">
                              <a onclick=LockUnlock('${data.id}') class="hovera mx-2" style="cursor:pointer;" title="UNLOCK">                          
                                    <i class="fa-solid fa-lock"></i>
                                </a>
                                <a href="/admin/user/RoleManagement?userId=${data.id}" class="hovera mx-2" style="cursor:pointer;" title="PERMISSION">
                                  <i class="fa-solid fa-pen-to-square"></i>
                                </a>
                        </div>
                    `}
                },
                "width": "15%"
            }
        ]
    });
}

function LockUnlock(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "Bạn có muốn khóa/mở khóa tài khoản này ",
        icon: 'warning',
        iconHtml: '<i class="fa-solid fa-lock" style="color:#d33 "; > </i>',
        iconColor: '#d33',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, confirm!',
    }).then((result) => {
        if (result.isConfirmed) {
            var token = $('[name="__RequestVerificationToken"]').val();
            $.ajax({
                type: "POST",
                url: '/Admin/User/LockUnlock',
                headers: { 'RequestVerificationToken': token },
                data: JSON.stringify(id),
                contentType: "application/json",

                success: function (data) {

                    toastr.success(data.message);
                    dataTable.ajax.reload();

                }
            })
        }
    })
}




