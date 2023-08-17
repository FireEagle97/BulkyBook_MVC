var dataTable;
$(document).ready(function () {
    loadDataTable();
    toastr.success("test toastr");
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'Title', "width": "25%" },
            { data: 'Isbn', "width": "15%" },
            { data: 'Price', "width": "10%" },
            { data: 'Author', "width": "20%" },
            { data: 'Category.Name', "width": "15%" },
            {
                data: 'Id',
                "render": function (data) {
                    return ` <div class="w-75 btn-group" role="group">
                                <a href="/admin/product/upsert?id=${data}" class="btn btn-primary">Edit <i class="bi bi-pencil-square"></i></a>
                                <a onClick=Delete("/admin/product/delete?id=${data}") class="btn btn-danger">Delete <i class="bi bi-trash"></i></a>
                            </div>`
                },
                "width": "15%"
            },
        ]
    });
}
function Delete (url){
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