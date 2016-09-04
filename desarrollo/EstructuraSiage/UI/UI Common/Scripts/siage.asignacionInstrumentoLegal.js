var AsignacionInstrumentoLegal = {};

AsignacionInstrumentoLegal.init = function (div, prefijo) {
    var instancia = {};
    instancia.div = div;

    // Inicializo el editor de Instrumento Legal
    var prefijoIL = prefijo ? prefijo + "_InstrumentoLegal" : "InstrumentoLegal";
    instancia.instrumento = InstrumentoLegal.init(div + " #divInstrumentoLegal", prefijoIL);

    if (prefijo) {
        $(div).agregarPrefijo(prefijo);
        instancia.prefijo = "#" + prefijo + "_";
    }
    else {
        instancia.prefijo = "#";
    }

    // Agrego comportamiento
    instancia.mostrarFechaNotificacion = function () {
        $(instancia.prefijo + "divFechaNotificacion").show();
    }

    instancia.ocultarFechaNotificacion = function () {
        $(instancia.prefijo + "divFechaNotificacion").hide();
    }
    $(instancia.prefijo + "Observaciones").changePatch(
                                    function () {
                                        $(this).val($(this).val().trim());
                                    });

    return instancia;
};

AsignacionInstrumentoLegal.cargar = function (instancia, id, estado, success) {
    var url = $.getUrl("/AsignacionInstrumentoLegal/GetAsignacionInstrumentoLegalById");

    $.get(url, { id: id }, function (data) {
        $.formatoFormulario(data, instancia.prefijo.replace("#", ""));
        $(instancia.prefijo + "Observaciones").attr("disabled", true);
        InstrumentoLegal.cargar(instancia.instrumento, data.InstrumentoLegal, null);

        if (success) {
            success();
        }
    });
};

AsignacionInstrumentoLegal.limpiar = function (instancia) {
    $(instancia.div + " :input[type!='button']").val("");
    $(instancia.prefijo + "Observaciones").attr("disabled", false);
    InstrumentoLegal.limpiar(instancia.instrumento);
};

AsignacionInstrumentoLegal.cargarData = function (instancia, data, estado) {
    $.formatoFormulario(data, instancia.prefijo.replace("#", ""));
    InstrumentoLegal.cargarData(instancia.instrumento, data.InstrumentoLegal, estado);
    $(instancia.prefijo + "Observaciones").attr("disabled", true);
};