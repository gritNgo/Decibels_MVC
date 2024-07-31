var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall' }, // specify url in case of more properties in the future in the ajax call
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'applicationUser.email', "width": "10%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-100 btn-group" role="group">
                    <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> </a>
                    </div>`
                },
                "width": "15%"
            }
        ]
    });
}

