$(function () {

    var ven = new Venda();
    ven.init();

    $(document).on('tblProdutosRowCallback', function (e, data) {
        ven.desabilitarDataTableProdutos(e, data);
        return false;
    });

    $(document).on('tblServicosRowCallback', function (e, data) {
        ven.desabilitarDataTableServicos(e, data);
        return false;
    });

    $('#addProduto').click(function () {
        ven.Save();
        return false;
    });

    $('#addServico').click(function () {
        ven.SaveServico();
        return false;
    });

    $('#gerarParcelas').click(function () {
        ven.gerarParcelas();
        return false;
    });

    $('#gerarParcelasServico').click(function () {
        ven.gerarParcelasServicos();
        return false;
    });

    $(document).on("tblProdutosOpenEdit", ven.openEdit);
    $(document).on("tblProdutosCancelEdit", ven.Limpar);
    $(document).bind("tblProdutosAfterDelete", function (e, data) {
        ven.calcTotalProdutos();
        ven.calcValoresFinais();
    });

    $(document).on("tblServicosOpenEdit", ven.openEditServico);
    $(document).on("tblServicosCancelEdit", ven.LimparServico);
    $(document).bind("tblServicosAfterDelete", function (e, data) {
        ven.calcTotalServicos();
        ven.calcValoresFinais();
    });

    $(document).on('AfterLoad_Produto', function (e, data) {
        ven.setValorUnitarioProduto(data.vlPrecoVenda);
        ven.calcTotal();
    });

    $(document).on('AfterLoad_Servico', function (e, data) {
        ven.setValorUnitario(data.vlServico);
        ven.calcTotalServicoIndividual();
    });

    $('#qtServico, #vlUnitarioServico').change(function () {
        ven.calcTotalServicoIndividual();
    });

    $('#qtProduto, #vlUnitario').change(function () {
        ven.calcTotal();
    });

    $('#vlDesconto').change(function () {
        ven.calcValoresFinais();
    });

    function Venda() {
        const self = this;

        this.element = {
            //CAMPOS DA TABELA DTPRODUTO
            $idProduto: $("#Produto_idProduto"),
            $nmProduto: $("#Produto_text"),
            $flUnidade: $("#flUnidade"),
            $vlUnitario: $("#vlUnitario"),
            $qtProduto: $("#qtProduto"),
            $vlTotalProduto: $("#vlTotalProduto"),

            //CAMPOS DA TABELA DTSERVICO
            $idServico: $("#Servico_idServico"),
            $nmServico: $("#Servico_text"),
            $idFuncionario: $("#FuncionarioExecutante_idFuncionario"),
            $nmFuncionario: $("#FuncionarioExecutante_text"),
            $vlUnitarioServico: $("#vlUnitarioServico"),
            $qtServico: $("#qtServico"),
            $vlTotalServico: $("#vlTotalServico"),
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
            get vlTotalProduto() {
                return IsNullOrEmpty(self.element.$vlTotalProduto.val()) ? "" : Functions.parseFloat(self.element.$vlTotalProduto.val());
            },
            set vlTotalProduto(value) {
                IsNullOrEmpty(value) ? self.element.$vlTotalProduto.val("") : self.element.$vlTotalProduto.val(Functions.numberFormat(value, 2));
            },
            //CAMPOS DA TABELA DTSERVICO
            get idServico() {
                return self.element.$idServico.val();
            },
            set idServico(value) {
                self.element.$idServico.val(value);
            },
            get nmServico() {
                return self.element.$nmServico.val();
            },
            set nmServico(value) {
                self.element.$nmServico.val(value);
            },
            get idFuncionario() {
                return self.element.$idFuncionario.val();
            },
            set idFuncionario(value) {
                self.element.$idFuncionario.val(value);
            },
            get nmFuncionario() {
                return self.element.$nmFuncionario.val();
            },
            set nmFuncionario(value) {
                self.element.$nmFuncionario.val(value);
            },
            get qtServico() {
                return self.element.$qtServico.val();
            },
            set qtServico(value) {
                self.element.$qtServico.val(value);
            },
            get vlUnitarioServico() {
                return IsNullOrEmpty(self.element.$vlUnitarioServico.val()) ? "" : Functions.parseFloat(self.element.$vlUnitarioServico.val());
            },
            set vlUnitarioServico(value) {
                IsNullOrEmpty(value) ? self.element.$vlUnitarioServico.val("") : self.element.$vlUnitarioServico.val(Functions.numberFormat(value, 2));
            },
            get vlTotalServico() {
                return IsNullOrEmpty(self.element.$vlTotalServico.val()) ? "" : Functions.parseFloat(self.element.$vlTotalServico.val());
            },
            set vlTotalServico(value) {
                IsNullOrEmpty(value) ? self.element.$vlTotalServico.val("") : self.element.$vlTotalServico.val(Functions.numberFormat(value, 2));
            },
        };

        this.init = function () {
            $("#vlTotal").attr("style", "pointer-events: none;background-color:rgb(233,236,239);text-align: right;");
            $("#vlTotalVendaServicos").attr("style", "pointer-events: none;background-color:rgb(233,236,239);text-align: right;");
            self.dtProduto = new tDataTable({
                table: {
                    jsItem: "jsListItemVenda",
                    name: "tblProdutos",
                    remove: true,
                    edit: true,
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
                                if (data == "G") {
                                    return '<div> GRAMA </div>';
                                } else {
                                    return '<div> UNIDADE </div>';
                                }
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

            self.dtServico = new tDataTable({
                table: {
                    jsItem: "jsListServicosVenda",
                    name: "tblServicos",
                    remove: true,
                    edit: true,
                    pageLength: 10,
                    "order": [[0, 'asc'], [1, 'asc']],
                    paginate: true,
                    columns: [
                        {
                            data: 'idServico',
                            sortable: false,
                            mRender: function (data) {
                                return '<div style="text-align: right">' + data + '</div>';
                            }
                        },
                        { data: "nmServico" },
                        { data: "nmFuncionario" },
                        {
                            data: 'qtServico',
                            sortable: false,
                            mRender: function (data) {
                                return '<div style="text-align: right">' + data + '</div>';
                            }
                        },
                        {
                            data: 'vlUnitarioServico',
                            sortable: false,
                            mRender: function (data) {
                                return '<div style="text-align: right">' + Functions.numberFormat(data, 2) + '</div>';
                            }
                        },
                        {
                            data: 'vlTotalServico',
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
                                console.log(data);
                                return '<div style="text-align: right">' + convertDateJson(data.dtVencimento) + '</div>';
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

            self.dtParcelaServico = new tDataTable({
                table: {
                    jsItem: "jsListParcelaServico",
                    name: "tblParcelasServico",
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
                                console.log(data);
                                return '<div style="text-align: right">' + convertDateJson(data.dtVencimento) + '</div>';
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
            self.calcTotalServicos();
            self.calcValoresFinais();
            self.calcValoresFinaisServico();
        }

        this.openEdit = function (e, data) {
            console.log(data);
            if (data != null) {
                self.Model.idProduto = data.item.idProduto;
                self.Model.nmProduto = data.item.nmProduto;
                self.Model.flUnidade = data.item.flUnidade == "UNIDADE" ? "U" : "G";
                self.Model.qtProduto = data.item.qtProduto;
                self.Model.vlUnitario = data.item.vlUnitario;
                self.Model.vlTotalProduto = data.item.vlTotalProduto;
            }
        };

        this.Save = function () {
            if (self.dtProduto.isEdit) {
                this.edit();
            } else {
                this.adicionarProduto();
            }
        };

        this.gerarParcelas = function () {
            if (IsNullOrEmpty($("#dtVenda").val())) {
                $.notify({ message: 'Por favor, informe a data de venda para gerar as parcelas!', icon: 'fa fa-exclamation' }, { type: 'danger' });
            }
            if (IsNullOrEmpty($("#CondicaoPagamento_idCondicaoPagamento").val())) {
                $.notify({ message: 'Por favor, informe a condição de pagamento da nota para gerar as parcelas!', icon: 'fa fa-exclamation' }, { type: 'danger' });
            }
            if (!IsNullOrEmpty($("#dtVenda").val()) && !IsNullOrEmpty($("#CondicaoPagamento_idCondicaoPagamento").val())) {
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    traditional: true,
                    url: Action.getParcelas,
                    data: { data: $("#dtVenda").val(), idCondicaoPagamento: $("#CondicaoPagamento_idCondicaoPagamento").val(), valor: $("#vlTotal").val() },
                    beforeSend: function () {
                    },
                    success: function (result) {
                        self.dtParcela.clear();
                        for (var i = 0; i < result.length; i++) {
                            self.dtParcela.addItem(result[i]);
                        }
                        self.dtProduto.atualizarItens();
                        self.dtProduto.atualizarGrid();
                        self.desabilitarFormProduto();
                        self.desabilitarValoresForm();
                        self.desabilitarDtVenda();
                    }
                });
            }
        };

        this.gerarParcelasServicos = function () {
            if (IsNullOrEmpty($("#dtVenda").val())) {
                $.notify({ message: 'Por favor, informe a data de venda para gerar as parcelas!', icon: 'fa fa-exclamation' }, { type: 'danger' });
            }
            if (IsNullOrEmpty($("#CondicaoPagamento_idCondicaoPagamento").val())) {
                $.notify({ message: 'Por favor, informe a condição de pagamento da nota para gerar as parcelas!', icon: 'fa fa-exclamation' }, { type: 'danger' });
            }
            if (!IsNullOrEmpty($("#dtVenda").val()) && !IsNullOrEmpty($("#CondicaoPagamento_idCondicaoPagamento").val())) {
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    traditional: true,
                    url: Action.getParcelas,
                    data: { data: $("#dtVenda").val(), idCondicaoPagamento: $("#CondicaoPagamento_idCondicaoPagamento").val(), valor: $("#vlTotal").val() },
                    beforeSend: function () {
                    },
                    success: function (result) {
                        self.dtParcelaServico.clear();
                        for (var i = 0; i < result.length; i++) {
                            self.dtParcelaServico.addItem(result[i]);
                        }
                        self.dtServico.atualizarItens();
                        self.dtServico.atualizarGrid();
                        self.desabilitarFormServico();
                        self.desabilitarValoresFormServico();
                        self.desabilitarDtVendaServico();
                    }
                });
            }
        };

        this.adicionarProduto = function () {
            if (self.validarProduto()) {
                let item = {
                    idProduto: self.Model.idProduto,
                    nmProduto: self.Model.nmProduto,
                    flUnidade: self.Model.flUnidade == "U" ? "UNIDADE" : "GRAMA",
                    qtProduto: self.Model.qtProduto,
                    vlUnitario: self.Model.vlUnitario,
                    vlTotalProduto: self.Model.vlTotalProduto,
                };
                self.dtProduto.addItem(item);
                self.habilitarFormParcelas();
                self.calcTotalProdutos();
                self.calcValoresFinais();
                self.Limpar();
            }
        };

        this.edit = function () {
            if (self.validarProduto()) {
                let item = self.dtProduto.dataSelected.item;

                item.idProduto = self.Model.idProduto;
                item.nmProduto = self.Model.nmProduto;
                item.flUnidade = self.Model.flUnidade == "U" ? "UNIDADE" : "GRAMA",
                    item.qtProduto = self.Model.qtProduto;
                item.vlUnitario = self.Model.vlUnitario;
                item.vlTotalProduto = self.Model.vlTotalProduto;

                self.dtProduto.editItem(item);
                self.calcTotalProdutos();
                self.calcValoresFinais();
                self.Limpar();
            }
        };

        this.calcTotalProdutos = function () {
            let vlTotalProduto = 0;

            for (var i = 0; i < self.dtProduto.data.length; i++) {
                vlTotalProduto = vlTotalProduto + self.dtProduto.data[i].vlTotalProduto;
            }

            $("#vlTotalProd").text(vlTotalProduto.toFixed(2).replace(".", ","));
        };

        this.calcValoresFinaisServico = function () {
            let valorFinal = 0;
            let valorTotalServicos = Functions.parseFloat($("#vlTotalServicos").text());
            if (valorTotalServicos > 0 && Functions.parseFloat($("#vlDescontoServico").val()) >= valorTotalServicos) {
                $.notify({ message: 'Por favor, informe um valor de desconto menor que o valor final da venda!', icon: 'fa fa-exclamation' }, { type: 'danger' });
            } else {
                valorFinal = valorTotalServicos - Functions.parseFloat($("#vlDescontoServico").val());
                $("#vlTotalVendaServicos").val((Functions.numberFormat(valorFinal, 2)));
            }
        };

        this.calcValoresFinais = function () {
            let valorFinal = 0;
            let valorTotalProdutos = Functions.parseFloat($("#vlTotalProd").text());
            if (valorTotalProdutos > 0 && Functions.parseFloat($("#vlDesconto").val()) >= valorTotalProdutos) {
                $.notify({ message: 'Por favor, informe um valor de desconto menor que o valor final da venda!', icon: 'fa fa-exclamation' }, { type: 'danger' });
            } else {
                valorFinal = valorTotalProdutos - Functions.parseFloat($("#vlDesconto").val());
                $("#vlTotal").val((Functions.numberFormat(valorFinal, 2)));
            }
        };

        this.Limpar = function () {
            self.Model.idProduto = "";
            self.Model.nmProduto = "";
            self.Model.qtProduto = "";
            self.Model.vlUnitario = "";
            self.Model.vlTotalProduto = "";
        };

        this.calcTotal = function () {
            let result = self.Model.vlUnitario * self.Model.qtProduto;
            self.Model.vlTotalProduto = result;
        };

        this.validarProduto = function () {
            let valid = true;

            if (IsNullOrEmpty(self.Model.nmProduto)) {
                $.notify({ message: 'Por favor, informe o produto!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                valid = false;
            }

            if (IsNullOrEmpty(self.Model.qtProduto)) {
                $.notify({ message: 'Por favor, informe a quantidade de produtos!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                valid = false;
            }

            return valid;
        };

        this.setValorUnitarioProduto = function (vlUnitario) {
            if (!IsNullOrEmpty(vlUnitario)) {
                self.Model.vlUnitario = vlUnitario;
            } else {
                self.Model.vlUnitario = "";
            }

        };

        //MÉTODOS PARA INSERÇÃO DOS SERVIÇOS NO DATATABLE
        this.openEditServico = function (e, data) {
            if (data != null) {
                self.Model.idServico = data.item.idServico;
                self.Model.nmServico = data.item.nmServico;
                self.Model.idFuncionario = data.item.idFuncionario;
                self.Model.nmFuncionario = data.item.nmFuncionario;
                self.Model.qtServico = data.item.qtServico;
                self.Model.vlUnitarioServico = data.item.vlUnitarioServico;
                self.Model.vlTotalServico = data.item.vlTotalServico;
            }
        };

        this.SaveServico = function () {
            if (self.dtServico.isEdit) {
                this.editServico();
            } else {
                this.adicionarServico();
            }
        };

        this.adicionarServico = function () {
            if (self.validarServico()) {
                let item = {
                    idServico: self.Model.idServico,
                    nmServico: self.Model.nmServico,
                    idFuncionario: self.Model.idFuncionario,
                    nmFuncionario: self.Model.nmFuncionario,
                    qtServico: self.Model.qtServico,
                    vlUnitarioServico: self.Model.vlUnitarioServico,
                    vlTotalServico: self.Model.qtServico * self.Model.vlUnitarioServico,
                };
                self.dtServico.addItem(item);
                self.calcTotalServicos();
                self.calcValoresFinais();
                self.LimparServico();
            }
        };

        this.editServico = function () {
            if (self.validarServico()) {
                let item = self.dtServico.dataSelected.item;

                item.idServico = self.Model.idServico;
                item.nmServico = self.Model.nmServico;
                item.idFuncionario = self.Model.idFuncionario;
                item.nmFuncionario = self.Model.nmFuncionario;
                item.qtServico = self.Model.qtServico;
                item.vlUnitarioServico = self.Model.vlUnitarioServico;
                item.vlTotalServico = self.Model.vlTotalServico;

                self.dtServico.editItem(item);
                self.calcTotalServicos();
                self.calcValoresFinais();
                self.LimparServico();
            }
        };

        this.validarServico = function () {
            let valid = true;
            if (IsNullOrEmpty(self.Model.nmServico)) {
                $.notify({ message: 'Por favor, informe o serviço!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                valid = false;
            }

            if (IsNullOrEmpty(self.Model.nmFuncionario)) {
                $.notify({ message: 'Por favor, informe o funcionário!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                valid = false;
            }

            if (IsNullOrEmpty(self.Model.qtServico)) {
                $.notify({ message: 'Por favor, informe a quantidade de serviço!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                valid = false;
            }

            if (IsNullOrEmpty(self.Model.vlUnitarioServico)) {
                $.notify({ message: 'Por favor, informe o valor unitário!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                valid = false;
            }

            if (self.dtServico.length > 0) {
                if (!self.dtServico.isEdit && self.dtServico.exists("idServico", self.Model.idServico)) {
                    $.notify({ message: 'Esse serviço já foi adicionado na tabela, verifique!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                    valid = false;
                } else if ((self.dtServico.isEdit && self.Model.idServico != self.dtServico.dataSelected.item.idServico) && (self.dtServico.exists("idServico", self.Model.idServico))) {
                    $.notify({ message: 'Esse serviço já foi adicionado na tabela, verifique!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                    valid = false;
                }
            }

            return valid;
        };

        this.LimparServico = function () {
            self.Model.idServico = "";
            self.Model.nmServico = "";
            self.Model.idFuncionario = "";
            self.Model.nmFuncionario = "";
            self.Model.qtServico = "";
            self.Model.vlUnitarioServico = "";
            self.Model.vlTotalServico = "";
        };

        this.setValorUnitario = function (vlUnitario) {
            if (!IsNullOrEmpty(vlUnitario)) {
                self.Model.vlUnitarioServico = vlUnitario;
            } else {
                self.Model.vlUnitarioServico = "";
            }

        };

        this.calcTotalServicoIndividual = function () {
            let result = self.Model.vlUnitarioServico * self.Model.qtServico;
            self.Model.vlTotalServico = result;
        };

        this.calcTotalServicos = function () {
            let vlTotalServicos = 0;

            for (var i = 0; i < self.dtServico.data.length; i++) {
                vlTotalServicos = vlTotalServicos + self.dtServico.data[i].vlTotalServico;
            }

            $("#vlTotalServicos").text(vlTotalServicos.toFixed(2).replace(".", ","));
        };

        this.desabilitarDataTableServicos = function (e, data) {
            if (self.dtParcela.data.length > 0) {
                let btn = $('td a[data-event=remove]', data.nRow);
                btn.attr('title', "Não é possível remover!");
                btn.attr('data-event', false);
                btn.removeClass().addClass("btn btn-secondary btn-sm");
                btn.find("i").removeClass().addClass("fa fa-exclamation-triangle");
                btn.on('click', function (e) {
                    e.preventDefault();
                })

                let btnEdit = $('td a[data-event=edit]', data.nRow);
                btnEdit.attr('title', "Não é possível editar!");
                btnEdit.attr('data-event', false);
                btnEdit.removeClass().addClass("btn btn-secondary btn-sm").css("width", "29px");
                btnEdit.find("i").removeClass().addClass("fa fa-exclamation-triangle");
                btnEdit.click(function (e) {
                    e.preventDefault();
                });
            }
        }

        this.desabilitarFormProduto = function () {
            $("#Produto_idProduto").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#flUnidade").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#qtProduto").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#vlUnitario").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#flUnidade").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#addProduto").prop("disabled", true);
            $("#Produto_btn-localizar").hide();

        }

        this.habilitarFormProduto = function () {
            $("#Produto_idProduto").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#flUnidade").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#qtProduto").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#vlUnitario").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#Produto_idProduto").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#Produto_idProduto").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#addProduto").prop("disabled", false);
            $("#Produto_btn-localizar").show();
        }

        this.desabilitarFormServico = function () {
            $("#Servico_idServico").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#FuncionarioExecutante_idFuncionario").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#qtServico").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#vlUnitarioServico").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#addServico").prop("disabled", true);
            $("#Servico_btn-localizar").hide();
            $("#FuncionarioExecutante_btn-localizar").hide();

        }

        this.habilitarFormServico = function () {
            $("#Servico_idServico").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#FuncionarioExecutante_idFuncionario").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#qtServico").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#vlUnitarioServico").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#addServico").prop("disabled", false);
            $("#Servico_btn-localizar").show();
            $("#FuncionarioExecutante_btn-localizar").show();

        }

        this.desabilitarFormParcelas = function () {
            $("#CondicaoPagamento_idCondicaoPagamento").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#CondicaoPagamento_btn-localizar").hide();
            $("#gerarParcelas").prop("disabled", true);
        }

        this.habilitarFormParcelas = function () {
            $("#CondicaoPagamento_idCondicaoPagamento").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#CondicaoPagamento_btn-localizar").show();
            $("#gerarParcelas").prop("disabled", false);
        }

        this.desabilitarValoresForm = function () {
            $("#vlDesconto").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
        }

        this.desabilitarValoresFormServico = function () {
            $("#vlDescontoServico").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
        }

        this.habilitarValoresForm = function () {
            $("#vlFrete").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#vlSeguro").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#vlDespesas").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
        }

        this.desabilitarInfoPrincipais = function () {
            $("#Fornecedor_idFornecedor").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#modNota").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#serieNota").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#nrNota").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#dtEntrega").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#dtEmissao").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#Fornecedor_btn-localizar").hide();
        }

        this.desabilitarDtVenda = function () {
            $("#dtVenda").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
        }

        this.desabilitarDtVendaServico = function () {
            $("#dtVendaServico").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
        }

        this.habilitarInfoPrincipais = function () {
            $("#Fornecedor_idFornecedor").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#modNota").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#serieNota").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#nrNota").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#dtEntrega").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#dtEmissao").attr("style", "pointer-events: auto;background-color:rgb(255,255,255)");
            $("#Fornecedor_btn-localizar").show();
        }

        this.desabilitarDataTableProdutos = function (e, data) {
            if (self.dtParcela.data.length > 0) {
                let btn = $('td a[data-event=remove]', data.nRow);
                btn.attr('title', "Não é possível remover!");
                btn.attr('data-event', false);
                btn.removeClass().addClass("btn btn-secondary btn-sm");
                btn.find("i").removeClass().addClass("fa fa-exclamation-triangle");
                btn.on('click', function (e) {
                    e.preventDefault();
                })

                let btnEdit = $('td a[data-event=edit]', data.nRow);
                btnEdit.attr('title', "Não é possível editar!");
                btnEdit.attr('data-event', false);
                btnEdit.removeClass().addClass("btn btn-secondary btn-sm").css("width", "29px");
                btnEdit.find("i").removeClass().addClass("fa fa-exclamation-triangle");
                btnEdit.click(function (e) {
                    e.preventDefault();
                });
            }
        }
    }
});
