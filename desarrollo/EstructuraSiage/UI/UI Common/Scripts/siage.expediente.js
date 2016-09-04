var Expediente = {};

Expediente.init = function (div, prefijo) {
    var instancia = {};
    instancia.div = div;

    var prefijoAgente = prefijo ? prefijo + "_PersonaInicio" : "PersonaInicio";
    instancia.agente = AgenteConsultar.init(div + " #divAgente", prefijoAgente);

    if (prefijo) {
        $(div).agregarPrefijo(prefijo);
        instancia.prefijo = "#" + prefijo + "_";
    }
    else {
        instancia.prefijo = "#";
    }

    // Correcciones para que la grilla de agente muestre correctamente las grilas sin registros
    instancia.agente.grilla.setGridParam({
        loadComplete: function () {
            GrillaUtil.mostrarBotones(instancia.agente.grilla);

            var records = instancia.agente.grilla.getGridParam("reccount");
            console.log(records);
            if (records > 0) {
                $(instancia.prefijo + "divAgente .ui-jqgrid-hbox").show();
            }
            else {
                var page = instancia.agente.grilla.getGridParam('page');
                if (page == 1) {
                    $(instancia.prefijo + "divAgente .ui-jqgrid-hbox").hide();
                }
            }
        }
    });

    // Inicializo el comportamiento de los botones
    Expediente.inicializarBotones(instancia);

    return instancia;
};

Expediente.inicializarBotones = function (instancia) {
    $(instancia.prefijo + "btnBuscar").click(function () {
        Expediente.buscar(instancia);
    });

    $(instancia.prefijo + "btnNuevo").click(function () {
        Expediente.limpiar(instancia);
        Expediente.cambiarEstado(instancia, "Registrar");
    });

    $(instancia.prefijo + "btnBusqueda").click(function () {
        Expediente.limpiar(instancia);
    });
};

Expediente.buscar = function (instancia) {
    var numero = $(instancia.prefijo + "FiltroNumero").val();
    Expediente.cargar(instancia, null, numero);
};

Expediente.cargar = function (instancia, id, numero, success) {
    var url = $.getUrl("/Expediente/GetExpedienteByFilters");

    $.get(url, { numero: numero, id: id }, function (data) {
        Expediente.cargarData(instancia, data, "Ver");

        if (success) {
            success();
        }
    });
};

Expediente.cargarData = function (instancia, data, estado) {

    if (data) {
        $.formatoFormulario(data, instancia.prefijo.replace("#", ""));

        AgenteConsultar.seleccionarAgente(instancia.agente, data.PersonaInicio, function () {
            AgenteConsultar.cambiarEstado(instancia.agente, "Ver");
        });

        Expediente.cambiarEstado(instancia, "Ver");
    }
    else {
        Mensaje.Error.texto = "No se encontro un expediente con ese número";
        Mensaje.Error.mostrar();
        Expediente.limpiar(instancia);
    }
};

Expediente.cambiarEstado = function (instancia, estado) {
    instancia.estado = estado;

    switch (estado) {
        case "Ver":
            $(instancia.prefijo + "divVista :input[type!='button']").attr("disabled", "disabled");
            $(instancia.prefijo + "divVista p.botones").hide();

            $(instancia.prefijo + "divConsulta").hide();
            $(instancia.prefijo + "divVista").show();

            AgenteConsultar.cambiarEstado(instancia.agente, "Ver");
            break;

        case "Editar":
            $(instancia.prefijo + "divVista :input[type!='button']").removeAttr("disabled");
            $(instancia.prefijo + "divVista p.botones").show();

            $(instancia.prefijo + "divConsulta").hide();
            $(instancia.prefijo + "divVista").show();

            AgenteConsultar.cambiarEstado(instancia.agente, "Ver");
            break;

        case "Registrar":
            $(instancia.prefijo + "divVista :input[type!='button']").removeAttr("disabled");
            $(instancia.prefijo + "divVista p.botones").show();

            $(instancia.prefijo + "divConsulta").hide();
            $(instancia.prefijo + "divVista").show();

            AgenteConsultar.cambiarEstado(instancia.agente, "Consultar");
            break;

        case "Consultar":
            $(instancia.prefijo + "divConsulta").show();
            $(instancia.prefijo + "divVista").hide();
            break;
    }
};

Expediente.limpiar = function (instancia) {
    $(instancia.prefijo + "Id").val("0");
    $(instancia.prefijo + "Identificador").val("");
    $(instancia.prefijo + "FechaInicio").val("");
    $(instancia.prefijo + "Numero").val("");
    $(instancia.prefijo + "Asunto").val("");
    $(instancia.prefijo + "FiltroNumero").val("");

    Expediente.cambiarEstado(instancia, "Consultar");
    AgenteConsultar.limpiar(instancia.agente);
};