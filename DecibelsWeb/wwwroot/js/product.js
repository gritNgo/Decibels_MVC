﻿

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall'}, // specify url in case of more properties in the future in the ajax call
        "columns": [
            { data: 'name', "width": "15%" },
            { data: 'price', "width": "10%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit</a>
                    <a class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "15%"
            }
        ]
    });
}

