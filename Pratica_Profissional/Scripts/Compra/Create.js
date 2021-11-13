$(function () {

    var com = new Compra();
    com.init();

    $(document).on('tblProdutosRowCallback', function (e, data) {
        com.desabilitarDataTableProdutos(e, data);
        return false;
    });

    $('#addProduto').click(function () {
        com.Save();
        return false;
    });

    $('#gerarParcelas').click(function () {
        com.gerarParcelas();
        return false;
    });

    $(document).on("tblProdutosOpenEdit", com.openEdit);
    $(document).on("tblProdutosCancelEdit", com.Limpar);
    $(document).bind("tblProdutosAfterDelete", function (e, data) {
        com.calcTotalProdutos();
        com.calcValoresFinais();
    });


    $(document).on('AfterLoad_Produto', function (e, data) {
        com.calcTotal();
    });

    $('#qtProduto, #vlUnitario, #txDumping').change(function () {
        com.calcTotal();
    });

    $('#vlFrete, #vlSeguro, #vlDespesas').change(function () {
        com.calcValoresFinais();
    });

    $('#modNota, #serieNota, #nrNota, #dtEntrega, #dtEmissao ').change(function () {
        com.verificarCompraBD();
    });

    $(document).on('AfterLoad_Fornecedor', function (e, data) {
        com.verificarCompraBD(data.id, data.text);
    });

    $('#dtEmissao').change(function () {
        com.FunctionValidaData();
    });

    $('#dtEntrega').change(function () {
        com.FunctionValidaDataEntrega();
    });

    function Compra() {
        const self = this;

        this.element = {
            //CAMPOS DA TABELA DTPRODUTO
            $idProduto: $("#Produto_idProduto"),
            $nmProduto: $("#Produto_text"),
            $flUnidade: $("#flUnidade"),
            $vlUnitario: $("#vlUnitario"),
            $qtProduto: $("#qtProduto"),
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
            get vlTotalProduto() {
                return IsNullOrEmpty(self.element.$vlTotalProduto.val()) ? "" : Functions.parseFloat(self.element.$vlTotalProduto.val());
            },
            set vlTotalProduto(value) {
                IsNullOrEmpty(value) ? self.element.$vlTotalProduto.val("") : self.element.$vlTotalProduto.val(Functions.numberFormat(value, 2));
            },

        };
        this.init = function () {
            $("#vlTotal").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            self.dtProduto = new tDataTable({
                table: {
                    jsItem: "jsListItemCompra",
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
                        { data: "flUnidade" },
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
            self.desabilitarFormProduto();
            self.desabilitarValoresForm();
            self.desabilitarFormParcelas();
        }

        this.FunctionValidaData = function () {
            var str = document.getElementById("dtEmissao").value; // pega o valor do input
            var date = new Date(str.split('/').reverse().join('/'));
            if (date > new Date()) {
                $.notify({ message: 'A data de emissão não deve ser maior que a data de hoje!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                $("#dtEmissao").val("")
            }
        }

        this.FunctionValidaDataEntrega = function () {
            var dtEmissao = document.getElementById("dtEmissao").value; // pega o valor do input
            var dtEntrega = document.getElementById("dtEntrega").value; // pega o valor do input
            var dtEmissaoSplit = new Date(dtEmissao.split('/').reverse().join('/'));
            var dtEntregaSplit = new Date(dtEntrega.split('/').reverse().join('/'));

            if (dtEntregaSplit < dtEmissaoSplit) {
                $.notify({ message: 'A data de entrega não deve ser menor que a data de emissão!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                $("#dtEntrega").val("")
            }
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
            if (IsNullOrEmpty($("#dtEmissao").val())) {
                $.notify({ message: 'Por favor, informe a data de emissão da nota para gerar as parcelas!', icon: 'fa fa-exclamation' }, { type: 'danger' });
            }
            if (IsNullOrEmpty($("#CondicaoPagamento_idCondicaoPagamento").val())) {
                $.notify({ message: 'Por favor, informe a condição de pagamento da nota para gerar as parcelas!', icon: 'fa fa-exclamation' }, { type: 'danger' });
            }
            if (!IsNullOrEmpty($("#dtEmissao").val()) && !IsNullOrEmpty($("#CondicaoPagamento_idCondicaoPagamento").val())) {
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    traditional: true,
                    url: Action.getParcelas,
                    data: { data: $("#dtEmissao").val(), idCondicaoPagamento: $("#CondicaoPagamento_idCondicaoPagamento").val(), valor: $("#vlTotal").val() },
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
                        self.desabilitarDtEmissao();
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

        this.calcValoresFinais = function () {
            let valorFinal = 0;
            let valorTotalProdutos = Functions.parseFloat($("#vlTotalProd").text());
            valorFinal = valorTotalProdutos + Functions.parseFloat($("#vlFrete").val()) + Functions.parseFloat($("#vlSeguro").val()) + Functions.parseFloat($("#vlDespesas").val());
            $("#vlTotal").val((Functions.numberFormat(valorFinal, 2)));
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

        this.verificarCompraBD = function (idFornecedor, nmFornecedor) {
            var idFornecedor = idFornecedor != null ? idFornecedor : $("#Fornecedor_idFornecedor").val();
            var nmFornecedor = nmFornecedor != null ? nmFornecedor : $("#Fornecedor_text").val();
            if (!IsNullOrEmpty($("#dtEmissao").val()) && !IsNullOrEmpty($("#dtEntrega").val()) && !IsNullOrEmpty($("#modNota").val()) && !IsNullOrEmpty($("#serieNota").val()) && !IsNullOrEmpty($("#nrNota").val()) && !IsNullOrEmpty(idFornecedor) && !IsNullOrEmpty(nmFornecedor)) {
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    traditional: true,
                    url: Action.verificarCompraBD,
                    data: {
                        modNota: $("#modNota").val(),
                        serieNota: $("#serieNota").val(),
                        nrNota: $("#nrNota").val(),
                        idFornecedor: idFornecedor != null ? idFornecedor : $("#Fornecedor_idFornecedor").val()
                    },
                    beforeSend: function () {
                    },
                    success: function (result) {
                        $("#divAlert").hide();
                        if (result == false) {
                            $.notify({ message: 'Dados validados, continue o cadastro de sua compra com a lista de produtos!', icon: 'fa fa-exclamation' }, { type: 'success' });
                            self.habilitarFormProduto();
                            self.habilitarValoresForm();
                            self.desabilitarInfoPrincipais();
                        } else {
                            $.notify({ message: 'Atenção! Já existe uma compra com esses dados cadastrados, verique!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                        }
                    }
                });
            }
        };

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
            $("#vlFrete").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#vlSeguro").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
            $("#vlDespesas").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
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

        this.desabilitarDtEmissao = function () {
            $("#dtEmissao").attr("style", "pointer-events: none;background-color:rgb(233,236,239)");
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
