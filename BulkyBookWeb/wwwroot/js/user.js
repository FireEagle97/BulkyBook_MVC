var dataTable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [
            { data: 'Name', "width": "15%" },
            { data: 'Email', "width": "20%" },
            { data: 'PhoneNumber', "width": "10%" },
            { data: 'Company.Name', "width": "15%" },
            { data: 'Role', "width": "15%" },
            {
                data: { Id: 'Id', LockoutEnd: "LockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.LockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                            <div class="row text-center" style="padding-left:1.5rem;">
                                <a onclick=LockUnlock('${data.Id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-lock-fill"></i> lock
                                </a>
                                <a href=/admin/User/RoleManagement?userId=${data.Id} class="btn btn-danger text-white" style="cursor:pointer; width:150px;margin-left:0.5rem;">
                                    <i class="bi bi-pencil-square"></i> Permission
                                </a>
                            </div>
                        `
                    }
                    else {
                        return `
                            <div class="row text-center" style="padding-left:1.5rem;">                                                              
                                <a onclick=LockUnlock('${data.Id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-unlock-fill"></i> Unlock
                                </a>
                                <a href=/admin/User/RoleManagement?userId=${data.Id} class="btn btn-danger text-white" style="cursor:pointer; width:150px;margin-left:0.5rem;">
                                    <i class="bi bi-pencil-square"></i> Permission
                                </a>
                            </div>
                        `
                    }
 
                },
                "width": "25%"
            },
        ]
    });
}
function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            toastr.success(data.message);
            dataTable.ajax.reload();
        }
    })
}