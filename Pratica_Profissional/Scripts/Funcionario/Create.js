﻿$(function () {
    var funcionario = new Funcionario();
    funcionario.init();

    $("#flTipo").change(function () {
        funcionario.changeTipo();
    });

    $("#cpf").inputmask("999.999.999-99");
    $("#cep").inputmask("99999-999");
    $("#tel").inputmask("(99) 9999-9999");
    $("#cel").inputmask("(99) 99999-9999");

    $('#cpf').blur(function () {
        funcionario.validaCPF();
    });

    $('#dtNascimento').blur(function () {
        funcionario.FunctionValidaData();
    });

    $('#email').blur(function () {
        funcionario.validaEmail();
    });

    function Funcionario() {
        const self = this;


        this.init = function () {
            self.changeTipo();
        }

        this.FunctionValidaData = function () {
            var data = document.getElementById("dtNascimento").value; // pega o valor do input
            data = data.replace(/\//g, "-"); // substitui eventuais barras (ex. IE) "/" por hífen "-"
            var data_array = data.split("-"); // quebra a data em array

            // para o IE onde será inserido no formato dd/MM/yyyy
            if (data_array[0].length != 4) {
                data = data_array[2] + "-" + data_array[1] + "-" + data_array[0]; // remonto a data no formato yyyy/MM/dd
            }

            // comparo as datas e calculo a idade
            var hoje = new Date();
            var nasc = new Date(data);
            var idade = hoje.getFullYear() - nasc.getFullYear();
            var m = hoje.getMonth() - nasc.getMonth();
            if (m < 0 || (m === 0 && hoje.getDate() < nasc.getDate())) idade--;

            if (idade < 18) {
                $.notify({ message: 'O funcionário deve ter pelo menos 18 anos!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                $("#dtNascimento").val("")
            }
        }

        this.validaCPF = function () {
            if (!self.FunctionvalidaCPF()) {
                $.notify({ message: 'CPF inválido, verifique!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                $("#cpf").val("")
                valid = false;
            }
        }

        this.validaEmail = function () {
            if (!self.FunctionvalidaEmail()) {
                $.notify({ message: 'E-mail inválido, verifique!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                $("#email").val("")
                valid = false;
            }
        }

        this.FunctionvalidaEmail = function () {
            if (/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/.test($("#email").val())) {
                return true
            } else {
                return false
            }
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
        this.FunctionvalidaCPF = function () {
            var cpf = $("#cpf").val().replace(/[^\d]+/g, '');
            console.log(cpf);
            if (cpf == '') return false;
            // Elimina CPFs invalidos conhecidos	
            if (cpf.length != 11 ||
                cpf == "00000000000" ||
                cpf == "11111111111" ||
                cpf == "22222222222" ||
                cpf == "33333333333" ||
                cpf == "44444444444" ||
                cpf == "55555555555" ||
                cpf == "66666666666" ||
                cpf == "77777777777" ||
                cpf == "88888888888" ||
                cpf == "99999999999")
                return false;
            // Valida 1o digito	
            add = 0;
            for (i = 0; i < 9; i++)
                add += parseInt(cpf.charAt(i)) * (10 - i);
            rev = 11 - (add % 11);
            if (rev == 10 || rev == 11)
                rev = 0;
            if (rev != parseInt(cpf.charAt(9)))
                return false;
            // Valida 2o digito	
            add = 0;
            for (i = 0; i < 10; i++)
                add += parseInt(cpf.charAt(i)) * (11 - i);
            rev = 11 - (add % 11);
            if (rev == 10 || rev == 11)
                rev = 0;
            if (rev != parseInt(cpf.charAt(10)))
                return false;
            return true;
        }
    }
});
