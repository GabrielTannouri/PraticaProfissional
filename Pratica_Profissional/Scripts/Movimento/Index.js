$(function () {

    var table = $('#lista').DataTable({
        ajax: { url: $('#lista').attr('data-url') },
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/Portuguese-Brasil.json',
        },
        columns: [
            {
                data: null,
                mRender: function (data) {
                    console.log(data);
                    return '<div style="text-align: right">' + convertDateJson(data.dtPagamento) + '</div>';
                }
            },
            {
                data: null,
                mRender: function (data) {
                    return '<div style="text-align: right">' + data.idMovimento + '</div>';
                }
            },
            { data: "conta.nmConta" },
            {
                data: null,
                mRender: function (data) {
                    return '<div style="text-align: right">' + Functions.numberFormat(data.vlPagamento, 2) + '</div>';
                }
            },
            { data: "descricao" },
        ]
    });

    $('#searchForm').submit(function (e) {
        e.preventDefault();
        table.draw();
    });
});