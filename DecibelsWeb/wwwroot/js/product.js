

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall'}, // specify url in case of more properties in the future in the ajax call
        "columns": [
            { data: 'name', "width": "15%" },
            { data: 'description', "width": "25%" },
            { data: 'price', "width": "5%" },
            { data: 'category.name', "width": "10%" }
        ]
    });
}

