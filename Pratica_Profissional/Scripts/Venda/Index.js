$(function () {

    var table = $('#lista').DataTable({
        ajax: { url: $('#lista').attr('data-url') },
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/Portuguese-Brasil.json',
        },
        columns: [
            { data: "modNota" },
            { data: "serieNota" },
            {
                data: null,
                mRender: function (data) {
                    console.log(data);
                    return '<div style="text-align: right">' + data.nrNota + '</div>';
                }
            },
            {
                data: null,
                mRender: function (data) {
                    console.log(data);
                    return '<div style="text-align: right">' + convertDateJson(data.dtVenda) + '</div>';
                }
            },
            { data: "Cliente.nmPessoa" },
            {
                data: 'vlTotal',
                sortable: false,
                mRender: function (data) {
                    return '<div style="text-align: right">' + Functions.numberFormat(data, 2) + '</div>';
                }
            },
            {
                data: null,
                mRender: function (data) {
                    if (data.flSituacao === "A") {
                        return '<div class="badge badge-primary">' + SituacaoVenda(data.flSituacao) + '</div>';
                    }
                    if (data.flSituacao === "F") {
                        return '<div class="badge badge-info">' + SituacaoVenda(data.flSituacao) + '</div>';
                    }
                }
            },
            {
                sortable: false,
                data: null,
                sClass: 'details-control',
                mRender: function (data) {
                    return '<a class="btn btn-success btn-sm" href="ordemServico/Details/' + data.idOrdemServico + '" title="Visualizar registro"><i class="fa fa-eye"></i> </a>' +
                         ' '+
                        '<a class="btn btn-info btn-sm" href="ordemServico/Edit/' + data.idOrdemServico + '" title="Alterar registro"><i class="fa fa-pencil-square-o"></i> </a>';
                }
            },
        ]
    });

    $('#searchForm').submit(function (e) {
        e.preventDefault();
        table.draw();
    });
});