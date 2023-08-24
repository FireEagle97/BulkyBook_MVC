var dataTable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#compData').DataTable({
        "ajax": { url: '/admin/company/getall' },
        "columns": [
            { data: 'Name', "width": "15%" },
            { data: 'StreetAddress', "width": "25%" },
            { data: 'City', "width": "10%" },
            { data: 'State', "width": "20%" },
            { data: 'PhoneNumber', "width": "15%" },
            {
                data: 'Id',
                "render": function (data) {
                    return ` <div class="w-75 btn-group" role="group">
                                <a href="/admin/company/upsert?id=${data}" class="btn btn-primary">Edit <i class="bi bi-pencil-square"></i></a>
                                <a onClick=Delete("/admin/company/delete?id=${data}") class="btn btn-danger">Delete <i class="bi bi-trash"></i></a>
                            </div>`
                },
                "width": "15%"
            },
        ]
    });
}
function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}