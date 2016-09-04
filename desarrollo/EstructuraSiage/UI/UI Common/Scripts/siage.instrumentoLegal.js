var InstrumentoLegal = {};

InstrumentoLegal.init = function (div, prefijo) {
    var instancia = {};
    instancia.div = div;

    // Inicializo el editor de Expediente
    var prefijoExpediente = prefijo ? prefijo + "_Expediente" : "Expediente";
    instancia.expediente = Expediente.init(div + " #divExpediente", prefijoExpediente);

    // Agrego el prefijo a los controles HTML y creo la variable que lo contiene dentro de la instancia
    if (prefijo) {
        $(div).agregarPrefijo(prefijo);
        instancia.prefijo = "#" + prefijo + "_";

        // Corrijo problema con los datepicker
        $(div + " .val-DateTime").each(function (input) {
            var data = $(this).data("datepicker");
            if (data) {
                data.id = this.id;
                $(this).data("datepicker", data);
            }
        });
    }
    else {
        instancia.prefijo = "#";
    }

    // Llamo a las funciones de inicialización de los demás controles
    InstrumentoLegal.inicializarBotones(instancia);
    InstrumentoLegal.inicializarCheck(instancia);

    InstrumentoLegal.cambiarEstado(instancia, "Consultar");
    return instancia;
};


InstrumentoLegal.inicializarBotones = function (instancia) {
    $(instancia.prefijo + "btnBuscar").click(function () {
        InstrumentoLegal.buscar(instancia);
    });

    $(instancia.prefijo + "btnNuevo").click(function () {
        InstrumentoLegal.cambiarEstado(instancia, "Registrar");
    });

    $(instancia.prefijo + "btnBusqueda").click(function () {
        InstrumentoLegal.limpiar(instancia);
    });
};


InstrumentoLegal.inicializarCheck = function (instancia) {
    $(instancia.prefijo + "divExpediente").editorOpcional(instancia.prefijo + "RegistrarExpediente");
    $(instancia.prefijo + "RegistrarExpediente").changePatch();
};


InstrumentoLegal.buscar = function (instancia) {
    var numero = $(instancia.prefijo + "FiltroNumeroIL").val();
    InstrumentoLegal.cargar(instancia, null, numero);
};


InstrumentoLegal.cargar = function (instancia, id, numero, success) {
    var url = $.getUrl("/InstrumentoLegal/GetInstrumentoLegalByFilters");

    $.getJSON(url, { numero: numero, id: id }, function (data) {
        if (instancia.estado) {
            InstrumentoLegal.cargarData(instancia, data, instancia.estado, success);
        }
        else {
            InstrumentoLegal.cargarData(instancia, data, "Ver", success);
        }
    });
};

InstrumentoLegal.cargarData = function (instancia, data, estado, success) {

    if (data) {
        $.formatoFormulario(data, instancia.prefijo.replace("#", ""));
        instancia.id = data.Id;
        InstrumentoLegal.cambiarEstado(instancia, estado);

        if (data.Expediente) {
            $(instancia.prefijo + "RegistrarExpediente").attr("checked", "checked");
            $(instancia.prefijo + "RegistrarExpediente").changePatch();
            if (data.Expediente.Id) {
                Expediente.cargarData(instancia.expediente, data.Expediente, estado);
            }
            else if (data.Expediente) {
                Expediente.cargar(instancia.expediente, data.Expediente, null);
            }
        }
        $(instancia.prefijo + "RegistrarExpediente").changePatch();
    }
    else {
        Mensaje.Error.texto = "No se encontro un instrumento legal con ese número";
        Mensaje.Error.mostrar();

        InstrumentoLegal.limpiar(instancia);
    }
    if (success) {
        success();
    }
};


InstrumentoLegal.limpiar = function (instancia) {
    $(instancia.prefijo + "divVista :input[type!='button']").val("");
    $(instancia.prefijo + "divConsulta :input[type!='button']").val("");

    $(instancia.prefijo + "RegistrarExpediente").removeAttr("checked");
    $(instancia.prefijo + "RegistrarExpediente").changePatch();

    //Expediente.limpiar(instancia.expediente);
    InstrumentoLegal.cambiarEstado(instancia, "Consultar");
};


InstrumentoLegal.cambiarEstado = function (instancia, estado) {
    switch (estado) {
        case "Consultar":
            $(instancia.prefijo + "divConsulta").show();
            $(instancia.prefijo + "divVista").hide();
            break;

        case "Ver":
            $(instancia.prefijo + "divVista :input[type!='button']").attr("disabled", "disabled");
            $(instancia.prefijo + "divBotonesVista").hide();

            $(instancia.prefijo + "divConsulta").hide();
            $(instancia.prefijo + "divVista").show();

            //Expediente.cambiarEstado(instancia.expediente, "Ver");
            break;

        case "Editar":
            $(instancia.prefijo + "divVista :input[type!='button']").removeAttr("disabled");
            $(instancia.prefijo + "NroInstrumentoLegal").attr("disabled", "disabled");

            $(instancia.prefijo + "divBotonesVista").show();
            $(instancia.prefijo + "divConsulta").hide();
            $(instancia.prefijo + "divVista").show();

            //Expediente.cambiarEstado(instancia.expediente, "Editar");

        case "Registrar":
            $(instancia.prefijo + "divVista :input[type!='button']").removeAttr("disabled");
            $(instancia.prefijo + "NroInstrumentoLegal").removeAttr("disabled");

            $(instancia.prefijo + "divBotonesVista").show();
            $(instancia.prefijo + "divConsulta").hide();
            $(instancia.prefijo + "divVista").show();

            //Expediente.cambiarEstado(instancia.expediente, "Consultar");
            break;
    }
};