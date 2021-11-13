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
                    return '<div style="text-align: right">' + data.idOrdemServico + '</div>';
                }
            },
            {
                data: null,
                mRender: function (data) {
                    console.log(data);
                    return '<div style="text-align: right">' + convertDateJson(data.dtSituacao) + '</div>';
                }
            },
            { data: "Cliente.nmPessoa" },
            { data: "Funcionario.nmPessoa" },
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
                        return '<div class="badge badge-primary">' + SituacaoHistorico(data.flSituacao) + '</div>';
                    }
                    if (data.flSituacao === "R") {
                        return '<div class="badge badge-info">' + SituacaoHistorico(data.flSituacao) + '</div>';
                    }
                    if (data.flSituacao === "O") {
                        return '<div class="badge badge-success">' + SituacaoHistorico(data.flSituacao) + '</div>';
                    }
                    if (data.flSituacao === "F") {
                        return '<div class="badge badge-warning">' + SituacaoHistorico(data.flSituacao) + '</div>';
                    }
                    if (data.flSituacao === "I") {
                        return '<div class="badge badge-dark">' + SituacaoHistorico(data.flSituacao) + '</div>';
                    }
                    if (data.flSituacao === "C") {
                        return '<div class="badge badge-danger">' + SituacaoHistorico(data.flSituacao) + '</div>';
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