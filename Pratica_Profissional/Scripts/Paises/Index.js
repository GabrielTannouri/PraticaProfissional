$(function () {

    var config = window.getDatatableConfig({ process: true });

    var table = $('#lista').DataTable({
        ajax: { url: $('#lista').attr('data-url') },
        order: config.order,
        iDisplayStart: config.displayStart,
        iDisplayLength: config.displayLength,
        search: { search: config.search },
        pageLength: config.displayLength,
        lengthMenu: config.lengthMenu,
        dom: config.dom,
        buttons: config.buttons,
        columnDefs: config.columnDefs,
        language: config.language,
        columns: [
            {
                sortable: false,
                data: null,
                sClass: 'center',
                mRender: function (data) {
                    return tmpl("lista-actions", data);
                }
            },
            { data: "idTipo" },
            { data: "nmTipo" },
            //{
            //    data: "flSituacao",
            //    mRender: function (data) {
            //        return Functions.Label("AcompanhamentoTipo", data);
            //    }

            //}
        ]
    });

    $('#searchForm').submit(function (e) {
        e.preventDefault();
        table.draw();
    });
});