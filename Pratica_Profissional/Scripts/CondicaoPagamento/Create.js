$(function () {

    var con = new CondicaoPagamento();
    con.init();

    $('#addParcela').click(function () {
        con.adicionarParcela();
        return false;
    });

    $('#removeParcelas').click(function (e) {
        con.removeParcelas();
        return false;
    });

    function CondicaoPagamento() {
        const self = this;

        this.element = {
            $nrParcela: $("#nrParcela"),
            $nmFormaPagamento: $("#formaPagamento_text"),
            $nrPrazo: $("#nrPrazo"),
            $nrPorcentagem: $("#nrPorcentagem"),
        };

        this.Model = {
            get nrParcela() {
                return self.element.$nrParcela.val();
            },
            set nrParcela(value) {
                self.element.$nrParcela.val(value);
            },
            get nmFormaPagamento() {
                return self.element.$nmFormaPagamento.val();
            },
            set nmFormaPagamento(value) {
                self.element.$nmFormaPagamento.val(value);
            },
            get nrPrazo() {
                return IsNullOrEmpty(self.element.$nrPrazo.val()) ? "" : Functions.parseFloat(self.element.$nrPrazo.val());
            },
            set nrPrazo(value) {
                IsNullOrEmpty(value) ? self.element.$nrPrazo.val("") : self.element.$nrPrazo.val(Functions.numberFormat(value, 2));
            },
            get nrPorcentagem() {
                return IsNullOrEmpty(self.element.$nrPorcentagem.val()) ? "" : Functions.parseFloat(self.element.$nrPorcentagem.val());
            },
            set nrPorcentagem(value) {
                IsNullOrEmpty(value) ? self.element.$nrPorcentagem.val("") : self.element.$nrPorcentagem.val(Functions.numberFormat(value, 2));
            },
        };

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

        this.SavParcela = function () {
            this.adicionarParcela();
        };

        this.removeParcelas = function () {
            self.Limpar();
            self.dtParcela.data = null;
            self.calcPorcentagem();
        };

        this.adicionarParcela = function () {
            let auxNrPorcentagem = $("#nrPorcentagem").val().replace(",", ".");
            auxNrPorcentagem = Number.parseFloat(auxNrPorcentagem);

            let nrParcela = 1;

            if (self.dtParcela.data != null && self.dtParcela.data.length > 0) {
                nrParcela = self.dtParcela.data.length + 1;
            }

            if (self.validarParcela()) {
                let item = {
                    nrParcela: nrParcela,
                    nmFormaPagamento: self.Model.nmFormaPagamento,
                    nrPrazo: self.Model.nrPrazo,
                    nrPorcentagem: auxNrPorcentagem,
                    idFormaPagamento: $("#formaPagamento_idFormaPagamento").val(),
                };
                self.dtParcela.addItem(item);
                self.calcPorcentagem();
                self.Limpar();
            }

        };

        this.calcPorcentagem = function () {
            let nrPorcentagem = 0;
            self.dtParcela.data.forEach(function (data) {
                nrPorcentagem = nrPorcentagem + data.nrPorcentagem;
            });
            console.log(nrPorcentagem);
            $("#nrTotalPorcentagem").val(nrPorcentagem);
        };

        this.verificaPorcentagem = function (num) {
            let nrPorcentagem = num;
            self.dtParcela.data.forEach(function (data) {
                nrPorcentagem = nrPorcentagem + data.nrPorcentagem;
            });

            return nrPorcentagem.toFixed(2);
        };

        this.verificaDias = function (num) {
            let nrDias = 0;
            self.dtParcela.data.forEach(function (data) {
                if (data.nrPrazo > nrDias) {
                    nrDias = data.nrPrazo;
                }
            });
            return nrDias;
        };


        this.Limpar = function () {
            self.Model.nrPrazo = "";
            self.Model.nrPorcentagem = "";
            $("#formaPagamento_idFormaPagamento").val("");
            $("#formaPagamento_text").val("");
        };

        this.validarParcela = function () {
            let valid = true;

            if (this.verificaPorcentagem(self.Model.nrPorcentagem) > 100) {
                $.notify({ message: 'A porcentagem máxima é de 100%!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                valid = false;
            }
            if (this.verificaPorcentagem(self.Model.nrPorcentagem) <= 100) {

                if (IsNullOrEmpty($("#formaPagamento_text").val())) {
                    $.notify({ message: 'Por favor, informe a forma de pagamento!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                    valid = false;
                }

                if (IsNullOrEmpty(self.Model.nrPrazo)) {
                    $.notify({ message: 'Por favor, informe o prazo!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                    valid = false;
                }

                if (self.Model.nrPrazo < self.verificaDias()) {
                    $.notify({ message: 'Por favor, informe um prazo superior ao último informado!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                    valid = false;
                }

                if (IsNullOrEmpty(self.Model.nrPorcentagem)) {
                    $.notify({ message: 'Por favor, informe a porcentagem!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                    valid = false;
                }
            }

            return valid;
        };
    }
});
