var dataTable;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else if (url.includes("completed")) {
        loadDataTable("completed");
    }
    else if (url.includes("pending")) {
        loadDataTable("pending");

    } else if (url.includes("completed")) {
        loadDataTable("completed");
    } else if (url.includes("approved")) {
        loadDataTable("approved");
    
    } else {
        loadDataTable("all");
    }

    
});
function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + status },
        "columns": [
            { data: 'Id', "width": "5%" },
            { data: 'Name', "width": "25%" },
            { data: 'PhoneNumber', "width": "20%" },
            { data: 'ApplicationUser.Email', "width": "20%" },
            { data: 'OrderStatus', "width": "10%" },
            { data: 'OrderTotal', "width": "10%" },
            {
                data: 'Id',
                "render": function (data) {
                    return ` <div class="w-75 btn-group" role="group">
                                <a href="/admin/order/details?orderId=${data}" class="btn btn-primary"><i class="bi bi-pencil-square"></i></a>
                            </div>`
                },
                "width": "10%"
            },
        ]
    });
}