$(function () {
    $(document).on("input", ".numeric", function (e) {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
    $.fn.datepicker.dates['en'] = {
        days: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo"],
        daysShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom"],
        daysMin: ["Do", "Se", "Te", "Qu", "Qu", "Se", "Sa", "Do"],
        months: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
        monthsShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
        today: "Hoje",
        clear: "Limpar",
        titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
        weekStart: 0
    };
    $('.datepicker').datepicker()

    $(".alert-success").fadeTo(5000, 500).slideUp(500, function () {
        $(".alert-success").slideUp(500);
    });

    $(".alert-danger").fadeTo(5000, 500).slideUp(500, function () {
        $(".alert-danger").slideUp(500);
    });

    jQuery(':input').keyup(function () {
        $(this).val($(this).val().toUpperCase());
    });

    jQuery('.text').keyup(function () {
        this.value = this.value.replace(/[^a-zA-Z\À-ú ]/g, '');
    });

    jQuery('.textNotSpace').keyup(function () {
        this.value = this.value.replace(/[^a-zA-Z]/g, '');
    });

    jQuery('.textDDI').keyup(function () {
        this.value = this.value.replace(/[^0-9+]/g, '');
    });

    $('.currency').each(function () {

        var aSep = $(this).attr('asep');
        var aDec = $(this).attr('adec');
        var vMin = $(this).attr('vmin');
        var vMax = $(this).attr('vmax');
        var mDec = $(this).attr('mdec');
        var metod = $(this).attr('metod');

        vMin = IsNullOrEmpty(vMin) ? '0' : vMin;
        vMax = IsNullOrEmpty(vMax) ? '9999999999.99999' : vMax;
        aSep = aSep == null ? '.' : aSep;
        aDec = IsNullOrEmpty(aDec) ? ',' : aDec;
        mDec = IsNullOrEmpty(mDec) ? '2' : mDec;
        metod = IsNullOrEmpty(metod) ? "init" : metod;

        $(this).autoNumeric(metod, {
            aSep: aSep,
            aDec: aDec,
            vMin: vMin,
            vMax: vMax,
            mDec: mDec,
        }).css('text-align', 'right');
    });

    $(".taxa").autoNumeric('init', {
        aSep: '.',
        aDec: ',',
        nBracket: '(,)',
        vMin: -999.99,
        vMax: 999.99
    }).css('text-align', 'right');

    $(".peso").autoNumeric('init', {
        aSep: '.',
        aDec: ',',
        nBracket: '(,)',
        vMin: 0,
        mDec: 3,
        vMax: 199.99
    }).css('text-align', 'right');

    $(".integer").autoNumeric('init', {
        vMin: 0,
        vMax: 9999999,
        mDec: 0,
        aSep: ''
    }).css('text-align', 'right');

});

function convertDateJson(data) {
    var dateString = data.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = ("0" + (currentTime.getMonth() + 1)).slice(-2);
    var day = ("0" + currentTime.getDate()).slice(-2);
    var year = currentTime.getFullYear();
    var date = day + '/' + month + '/' + year;

    return date;
}

function convertDateISO(data) {
    let date = moment(data).format("DD/MM/YYYY");
    return date;
}

function SituacaoHistorico(data) {
    let flSituacao = data;
    if (flSituacao == "A") {
        return "ABERTA";
    }
    if (flSituacao == "R") {
        return "ORÇAMENTO REALIZADO";
    }
    if (flSituacao == "O") {
        return "ORÇAMENTO APROVADO";
    }
    if (flSituacao == "F") {
        return "FECHADO";
    }
    if (flSituacao == "I") {
        return "FINALIZADO";
    }
    if (flSituacao == "C") {
        return "CANCELADO";
    }
}