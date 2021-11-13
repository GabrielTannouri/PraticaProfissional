$(function () {
    var fornecedor = new Fornecedor();
    fornecedor.init();

    $("#flTipo").change(function () {
        fornecedor.changeTipo();
    });

    $("#cpf").inputmask("999.999.999-99");
    $("#rg").inputmask("99.999.999-9");
    $("#cep").inputmask("99999-999");
    $("#tel").inputmask("(99) 9999-9999");
    $("#cel").inputmask("(99) 99999-9999");


    function Fornecedor() {
        const self = this;


        this.init = function () {
            self.changeTipo();
        }

        this.changeTipo = function () {
            if ($("#flTipo").val() == "F") {
                $("#divNmJuridico").hide();
                $("#divNmFantasia").hide();
                $("#divCNPJ").hide();
                $("#divInscricao").hide();
                $("#divDocumento").hide();
                $("#divNmFisico").show();
                $("#divNmApelido").show();
                $("#divDtNascimento").show();
                $("#divGenero").show();
                $("#divCPF").show();
                $("#divRG").show();
            } else {
                $("#divNmApelido").hide();
                $("#divDtNascimento").hide();
                $("#divGenero").hide();
                $("#divCPF").hide();
                $("#divRG").hide();
                $("#divDocumento").hide();
                $("#divNmJuridico").show();
                $("#divNmFantasia").show();
                $("#divCNPJ").show();
                $("#divInscricao").show();
                $("#divNmFisico").hide();
            }
        }
    }
});
