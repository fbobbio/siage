var AgenteConsultar = {};
AgenteConsultar.grilla = "seleccionAgenteList";

AgenteConsultar.init = function (div, prefijo) {
    var instancia = {};
    instancia.div = div;

    if (prefijo) {
        $(div).agregarPrefijo(prefijo);
        instancia.prefijo = "#" + prefijo + "_";
    }
    else {
        instancia.prefijo = "#";
    }

    AgenteConsultar.inicializarGrilla(instancia);
    AgenteConsultar.inicializarCombosDomicilio(instancia);
    AgenteConsultar.inicializarBotones(instancia);
    AgenteConsultar.inicializarCheck(instancia);

    $(instancia.prefijo + "divFiltroAvanzado").hide();
    $(instancia.prefijo + "divFiltroAvanzado .val-DateTime").datepicker({
        currentText: 'Now',
        dateFormat: 'dd/mm/yy',
        changeYear: true,
        yearRange: (new Date().getFullYear() - 80) + ":" + (new Date().getFullYear() + 80)
    });

    return instancia;
};

AgenteConsultar.inicializarCombosDomicilio = function (instancia) {
    $(instancia.prefijo + "FiltroLocalidad").CascadingDropDown(instancia.prefijo + "FiltroDepartamentoProvincial", $.getUrl('/Agente/CargarLocalidadByDepartamentoProvincial'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idDepartamento: $(instancia.prefijo + "FiltroDepartamentoProvincial").val() };
        }
    });
};

AgenteConsultar.inicializarBotones = function (instancia) {
    $(instancia.prefijo + "btnLimpiar").click(function () {
        AgenteConsultar.limpiar(instancia);
    });

    $(instancia.prefijo + "btnBuscar").click(function () {
        var parametrosAgente = "&" + $(instancia.prefijo + "divConsulta :input").getFiltros();
        parametrosAgente = parametrosAgente.replace(new RegExp(instancia.prefijo.replace("#", ""), "g"), "");

        var urlFinal = instancia.grilla.url + parametrosAgente;
        GrillaUtil.setUrl(instancia.grilla, urlFinal);

        if (AgenteConsultar.validarFiltrosIngresados(instancia)) {
            GrillaUtil.actualizar(instancia.grilla);
        }
    });
};

AgenteConsultar.inicializarCheck = function (instancia) {
    $(instancia.prefijo + "chkBusquedaAvanzada").changePatch(function () {
        if ($(instancia.prefijo + "chkBusquedaAvanzada").is(":checked")) {
            $(instancia.prefijo + "divFiltroAvanzado").show();
        }
        else {
            $(instancia.prefijo + "divFiltroAvanzado :input[type!='button']").val("");
            $(instancia.prefijo + "divFiltroAvanzado").hide();
        }
    });
};

AgenteConsultar.inicializarGrilla = function (instancia) {
    var id = instancia.prefijo + AgenteConsultar.grilla;
    var pager = instancia.prefijo + "pagerSelect";
    var url = $.getUrl('/Agente/ProcesarBusquedaSeleccion/');

    var grilla = $(id).jqGrid({
        datatype: "local",
        url: url,
        colNames: ['Id', 'Tipo documento', 'Nro documento.', 'Nro legajo', 'Nombre', 'Apellido', 'Sexo'],
        colModel: GrillaUtil.crearColumnas(
			['Id', 'Persona.TipoDocumento', 'Persona.NumeroDocumento', 'NumLegajoSiage', 'Persona.Nombre', 'Persona.Apellido', 'Persona.Sexo'],
			['integer', null, null, null, null, null, null],
			'Id'),
        pager: pager,
        toppager: true,
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "Id",
        height: "100%",
        emptyrecords: "",
        caption: "Selección de Agente"
    });

    grilla.id = id;
    grilla.id_limpio = id.replace("#", "");
    grilla.pager = pager;
    grilla.botones = ["Seleccionar"];
    grilla.url = url;
    grilla.setGridWidth(670, true);

    $(grilla.id + "_toppager_center", grilla.id + "_toppager").remove();
    $(".ui-paging-info", grilla.id + "_toppager").remove();
    $(grilla.id + "_toppager_left").append('<table cellspacing="0" cellpadding="0" border="0" style="float: left; table-layout: auto;" class="ui-pg-table navtable"><tbody><tr></tr></tbody></table>');

    var divMensajes = "#gview_" + grilla.id_limpio;
    $(divMensajes).append("<div id='" + grilla.id_limpio + "_sinConsulta' class='ui-mensaje'>Aún no se ha realizado ninguna búsqueda</div>");
    $(divMensajes).append("<div id='" + grilla.id_limpio + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados con los datos de búsqueda ingresados</div>");

    GrillaUtil.crearBotonSeleccion(grilla, "Seleccionar", "pin-s", function () {
        var id = GrillaUtil.getSeleccionFilas(grilla, false);
        if (id && id.lenght !== 0) {
            AgenteConsultar.seleccionarAgente(instancia, id);
        }
        else {
            AbmcUtil.mensajeSeleccion();
        }
    });

    grilla.setGridParam({ loadComplete: function (data) { GrillaUtil.mostrarBotones(grilla); } }).trigger("reloadGrid");
    instancia.grilla = grilla;
};

AgenteConsultar.validarFiltrosIngresados = function (instancia) {
    if ($(instancia.prefijo + "divConsulta fieldset :input[value!=''][type!='button'][type!='hidden'][type!='checkbox']").length === 0) {
        Mensaje.Advertencia.texto = "Debe ingresar al menos un filtro de busqueda";
        Mensaje.Advertencia.botones = false;
        Mensaje.Advertencia.mostrar();
        return false;
    }

    if ($(instancia.prefijo + "FiltroNumeroDocumento").val() || $(instancia.prefijo + "FiltroTipoDocumento").val() || $(instancia.prefijo + "FiltroSexo").val()) {
        if (!$(instancia.prefijo + "FiltroNumeroDocumento").val() || !$(instancia.prefijo + "FiltroTipoDocumento").val()) {
            Mensaje.Advertencia.texto = "Para filtrar por datos de la persona asociada al agente, debe ingresar al menos Número de documento y Tipo de documento";
            Mensaje.Advertencia.botones = false;
            Mensaje.Advertencia.mostrar();
            return false;
        }
    }

    if ($(instancia.prefijo + "FiltroFechaDesdeAlta").val() || $(instancia.prefijo + "FiltroFechaHastaAlta").val()) {
        if (!$(instancia.prefijo + "FiltroFechaDesdeAlta").val() || $(instancia.prefijo + "FiltroFechaHastaAlta").val()) {
            Mensaje.Advertencia.texto = "Para filtrar por fecha Alta de agente debe ingresar Fecha desde y Fecha Hasta";
            Mensaje.Advertencia.botones = false;
            Mensaje.Advertencia.mostrar();
            return false;
        }
    }
    if ($(instancia.prefijo + "FiltroFechaDesdeBaja").val() || $(instancia.prefijo + "FiltroFechaHastaBaja").val()) {
        if (!$(instancia.prefijo + "FiltroFechaDesdeBaja").val() || !$(instancia.prefijo + "FiltroFechaHastaBaja").val()) {
            Mensaje.Advertencia.texto = "Para filtrar por fecha Baja de AgenteConsultar debe ingresar Fecha desde y Fecha Hasta";
            Mensaje.Advertencia.botones = false;
            Mensaje.Advertencia.mostrar();
            return false;
        }
    }
    Mensaje.ocultar();
    return true;
};

AgenteConsultar.seleccionarAgente = function (instancia, id, estado) {
    var url = $.getUrl("/Agente/GetAgenteById/");

    instancia.seleccion = id;

    $.get(url, { id: instancia.seleccion }, function (data) {
        var prefijo = instancia.prefijo.substring(1, instancia.prefijo.length - 1);

        $(instancia.prefijo + "divVista").html(data);
        $(instancia.prefijo + "divVista fieldset").append("<p class='botones'><br /><button id='btnVolver' type='button'>Buscar agente</button></p>");
        $(instancia.prefijo + "divVista").agregarPrefijo(prefijo);

        // corto la fecha de nacimiento
        $(instancia.prefijo + "FechaNacimiento").mask("99/99/9999", { placeholder: " " });

        $(instancia.prefijo + "btnVolver").click(function () {
            AgenteConsultar.limpiar(instancia);
        });

        if (estado) {
            AgenteConsultar.cambiarEstado(instancia, estado);
        }
        else {
            AgenteConsultar.cambiarEstado(instancia, "Registrar");
        }

        if (instancia.successSeleccion) {
            instancia.successSeleccion(id);
        }
    }, "html");
};

AgenteConsultar.limpiar = function (instancia) {
    $(instancia.prefijo + "divVista :input[type!='button']").val("");
    $(instancia.prefijo + "divConsulta :input[type!='button']").val("");

    $(instancia.prefijo + "chkBusquedaAvanzada").removeAttr("checked").changePatch();

    GrillaUtil.limpiar(instancia.grilla, instancia.grilla.url);
    AgenteConsultar.cambiarEstado(instancia, "Consultar");
};

AgenteConsultar.cambiarEstado = function (instancia, estado) {
    instancia.estado = estado;

    switch (estado) {
        case "Consultar":
            $(instancia.prefijo + "divVista").hide();
            $(instancia.prefijo + "divConsulta").show();
            break;

        case "Ver":
            $(instancia.prefijo + "divVista p.botones").hide();
            $(instancia.prefijo + "divVista").show();
            $(instancia.prefijo + "divConsulta").hide();
            break;

        default:
            $(instancia.prefijo + "divVista p.botones").show();
            $(instancia.prefijo + "divVista").show();
            $(instancia.prefijo + "divConsulta").hide();
            break;
    }
};