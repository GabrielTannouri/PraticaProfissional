$(function () {

    var com = new Compra();
    com.init();

    function Compra() {
        const self = this;

        this.element = {
            //CAMPOS DA TABELA DTPRODUTO
            $idProduto: $("#Produto_idProduto"),
            $nmProduto: $("#Produto_text"),
            $flUnidade: $("#flUnidade"),
            $vlUnitario: $("#vlUnitario"),
            $qtProduto: $("#qtProduto"),
            $vlDesconto: $("#vlDesconto"),
            $vlTotalProduto: $("#vlTotalProduto"),

        };

        this.Model = {
            //CAMPOS DA TABELA DTPRODUTO
            get idProduto() {
                return self.element.$idProduto.val();
            },
            set idProduto(value) {
                self.element.$idProduto.val(value);
            },
            get nmProduto() {
                return self.element.$nmProduto.val();
            },
            set nmProduto(value) {
                self.element.$nmProduto.val(value);
            },
            get flUnidade() {
                return self.element.$flUnidade.val();
            },
            set flUnidade(value) {
                self.element.$flUnidade.val(value);
            },
            get vlUnitario() {
                return IsNullOrEmpty(self.element.$vlUnitario.val()) ? "" : Functions.parseFloat(self.element.$vlUnitario.val());
            },
            set vlUnitario(value) {
                IsNullOrEmpty(value) ? self.element.$vlUnitario.val("") : self.element.$vlUnitario.val(Functions.numberFormat(value, 2));
            },
            get qtProduto() {
                return self.element.$qtProduto.val();
            },
            set qtProduto(value) {
                self.element.$qtProduto.val(value);
            },
            get vlDesconto() {
                return IsNullOrEmpty(self.element.$vlDesconto.val()) ? "" : Functions.parseFloat(self.element.$vlDesconto.val());
            },
            set vlDesconto(value) {
                IsNullOrEmpty(value) ? self.element.$vlDesconto.val("") : self.element.$vlDesconto.val(Functions.numberFormat(value, 2));
            },
            get vlTotalProduto() {
                return IsNullOrEmpty(self.element.$vlTotalProduto.val()) ? "" : Functions.parseFloat(self.element.$vlTotalProduto.val());
            },
            set vlTotalProduto(value) {
                IsNullOrEmpty(value) ? self.element.$vlTotalProduto.val("") : self.element.$vlTotalProduto.val(Functions.numberFormat(value, 2));
            },

        };
        this.init = function () {
            self.dtProduto = new tDataTable({
                table: {
                    jsItem: "jsListItemCompra",
                    name: "tblProdutos",
                    remove: false,
                    edit: false,
                    pageLength: 10,
                    "order": [[0, 'asc'], [1, 'asc']],
                    paginate: false,
                    columns: [
                        {
                            data: 'idProduto',
                            sortable: false,
                            mRender: function (data) {
                                return '<div style="text-align: right">' + data + '</div>';
                            }
                        },
                        { data: "nmProduto" },
                        {
                            data: 'flUnidade',
                            sortable: false,
                            mRender: function (data) {
                                let result = data == "U" ? "UNIDADE" : "GRAMA";
                                    return '<span>' + result + '</span>'
                               
                            }
                        },
                        {
                            data: 'qtProduto',
                            sortable: false,
                            mRender: function (data) {
                                return '<div style="text-align: right">' + data + '</div>';
                            }
                        },
                        {
                            data: 'vlUnitario',
                            sortable: false,
                            mRender: function (data) {
                                return '<div style="text-align: right">' + Functions.numberFormat(data, 2) + '</div>';
                            }
                        },
                        {
                            data: 'vlTotalProduto',
                            sortable: false,
                            mRender: function (data) {
                                return '<div style="text-align: right">' + Functions.numberFormat(data, 2) + '</div>';
                            }
                        },
                    ]
                }
            });

            self.dtParcela = new tDataTable({
                table: {
                    jsItem: "jsListParcela",
                    name: "tblParcelas",
                    remove: false,
                    edit: false,
                    pageLength: 10,
                    "order": [[0, 'asc'], [1, 'asc']],
                    paginate: false,
                    columns: [
                        {
                            data: 'nrParcela',
                            sortable: false,
                            mRender: function (data) {
                                return '<div style="text-align: right">' + data + '</div>';
                            }
                        },
                        { data: "nmFormaPagamento" },
                        {
                            data: null,
                            mRender: function (data) {
                                var data = new Date(data.dtVencimento),
                                    dia = data.getDate().toString(),
                                    diaF = (dia.length == 1) ? '0' + dia : dia,
                                    mes = (data.getMonth() + 1).toString(), //+1 pois no getMonth Janeiro começa com zero.
                                    mesF = (mes.length == 1) ? '0' + mes : mes,
                                    anoF = data.getFullYear();

                                let dtVencimento = diaF + "/" + mesF + "/" + anoF;

                                return '<div style="text-align: right">' + dtVencimento + '</div>';
                            }
                        },
                        {
                            data: 'vlParcela',
                            sortable: false,
                            mRender: function (data) {
                                return '<div style="text-align: right">' + Functions.numberFormat(data, 2) + '</div>';
                            }
                        },
                    ]
                }
            });
            self.calcTotalProdutos();
        }

        this.calcTotalProdutos = function () {
            let vlTotalProduto = 0;

            for (var i = 0; i < self.dtProduto.data.length; i++) {
                vlTotalProduto = vlTotalProduto + self.dtProduto.data[i].vlTotalProduto;
            }

            $("#vlTotalProd").text(vlTotalProduto.toFixed(2).replace(".", ","));
        };

    }
});
