﻿$(function () {

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
});
