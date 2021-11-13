$(function () {

    var con = new CondicaoPagamento();
    con.init();

    function CondicaoPagamento() {
        const self = this;


        this.init = function () {
            self.dtParcela = new tDataTable({
                table: {
                    jsItem: "jsList",
                    name: "tblCondicao",
                    remove: false,
                    edit: false,
                    pageLength: 10,
                    "order": [[0, 'asc'], [1, 'asc']],
                    paginate: false,
                    columns: [
                        { data: "nrParcela" },
                        { data: "nmFormaPagamento" },
                        {
                            data: 'nrPrazo',
                            sortable: false,
                            mRender: function (data) {
                                return '<div style="text-align: right">' + data + '</div>';
                            }
                        },
                        {
                            data: 'nrPorcentagem',
                            sortable: false,
                            mRender: function (data) {
                                return '<div style="text-align: right">' + Functions.numberFormat(data, 2) + '</div>';
                            }
                        },
                    ]
                }
            });
            this.calcPorcentagem();
        }

        this.calcPorcentagem = function () {
            let nrPorcentagem = 0;
            self.dtParcela.data.forEach(function (data) {
                nrPorcentagem = nrPorcentagem + Functions.parseFloat(data.nrPorcentagem);
            });
            $("#nrTotalPorcentagem").val(nrPorcentagem.toFixed(2));
        };
    }
});
