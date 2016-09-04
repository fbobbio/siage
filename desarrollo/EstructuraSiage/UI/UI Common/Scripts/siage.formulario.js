var Formulario = {};

Formulario.Botones = {}
Formulario.Botones.init = function () {
    $(":button").parent().addClass("botones");
};

Formulario.DropDownList = {};
Formulario.DropDownList.init = function () {
    $("select").each(function () {
        YAHOO.Hack.FixIESelectWidth($(this).attr('id'));
    });
    //fix el ancho fijo de los combos en internet explorer
    //$("select").wrap("<div class='select-box'>");
};

Formulario.Calendario = {};
Formulario.Calendario.init = function () {
    $.datepicker.regional.es = {
        closeText: 'Cerrar',
        prevText: '&#x3c;Ant',
        nextText: 'Sig&#x3e;',
        currentText: 'Hoy',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['es']);
};

Formulario.Procesando = {};
Formulario.Procesando.init = function () {
    $("#dlgProcesando").dialog({
        width: 120,
        height: 100,
        modal: true,
        autoOpen: false,
        resizable: false,
        closeOnEscape: false
    });

    $("div[aria-labelledby='ui-dialog-title-dlgProcesando'] div:first").hide();
    $("#dlgProcesando").css("text-align", "center");
    $("#dlgProcesando").css("vertical-align", "middle");

    $("body").ajaxStart(function () {
        if (!$("#dlgProcesando").dialog("isOpen")) {
            $("#imgProcesando").fadeIn();
        }
    });

    $("body").ajaxStop(function () {
        $("#imgProcesando").fadeOut();
    });
};

Formulario.Procesando.bloquear = function () {
    $("#dlgProcesando").dialog("open");
    $("#dlgProcesando").css("min-height", "100px");
    $("#imgProcesando").hide();
};

Formulario.Procesando.desbloquear = function () {
    $("#dlgProcesando").dialog("close");
};

Formulario.init = function () {
    Formulario.Botones.init();
    //Formulario.DropDownList.init();
    Formulario.Calendario.init();

    if (!('filter' in Array.prototype)) {
        Array.prototype.filter = function (filter, that /*opt*/) {
            var other = [], v;
            for (var i = 0, n = this.length; i < n; i++)
                if (i in this && filter.call(that, v = this[i], i, this))
                    other.push(v);
            return other;
        };
    }
};