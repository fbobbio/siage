// Script necesarios para usar este editor:
//      siage.abmc.grilla.js
//      siage.abmc.util.js
//      siage.grillas.util.js (solo si no esta incluida en la master page)

var PuestoDeTrabajoConsultar = {};
PuestoDeTrabajoConsultar.ProcesarFiltroBasico = "ProcesarBusquedaBasico"; 
PuestoDeTrabajoConsultar.ProcesarFiltroAvanzado = "ProcesarBusquedaAvanzado";

PuestoDeTrabajoConsultar.init = function (div, prefijo) {
    var instancia = {};
    instancia.div = div;

    instancia.Filtros = {};
    instancia.url = PuestoDeTrabajoConsultar.ProcesarFiltroBasico;

    if (prefijo) {
        $(div).agregarPrefijo(prefijo);
        instancia.prefijo = "#" + prefijo + "_";
    }
    else {
        instancia.prefijo = "#";
    }

    PuestoDeTrabajoConsultar.inicializarGrilla(instancia);
    PuestoDeTrabajoConsultar.inicializarBotones(instancia);

    $("#fltCodigoPN").attr("maxLength", 9);

    instancia.reset = function () {
        PuestoDeTrabajoConsultar.reset(instancia);
    };

    return instancia;
};

PuestoDeTrabajoConsultar.inicializarBotones = function (instancia) {
    $(instancia.prefijo + "btnConsultarBasico").click(function () {
        PuestoDeTrabajoConsultar.actualizarFiltroBasico(instancia);
    });

    $(instancia.prefijo + "btnConsultarAvanzado").click(function () {
        PuestoDeTrabajoConsultar.actualizarFiltroAvanzado(instancia);
    });

    $(instancia.prefijo + "btnLimpiarBasico").click(function () {
        AbmcUtil.limpiarFormulario(instancia.prefijo + "divPTFiltroBasico");
        PuestoDeTrabajoConsultar.limpiarGrilla(instancia);
    });

    $(instancia.prefijo + "btnLimpiarAvanzado").click(function () {
        AbmcUtil.limpiarFormulario(instancia.prefijo + "divPTFiltroAvanzado");
        PuestoDeTrabajoConsultar.limpiarGrilla(instancia);
    });

    $(instancia.prefijo + "btnFiltroBasico").click(function () {
        instancia.url = PuestoDeTrabajoConsultar.ProcesarFiltroBasico;
        PuestoDeTrabajoConsultar.limpiarGrilla(instancia);

        $(instancia.prefijo + "divPTFiltroBasico").show();
        $(instancia.prefijo + "divPTFiltroAvanzado").hide();
    });

    $(instancia.prefijo + "btnFiltroAvanzado").click(function () {
        instancia.url = PuestoDeTrabajoConsultar.ProcesarFiltroAvanzado;
        PuestoDeTrabajoConsultar.limpiarGrilla(instancia);

        $(instancia.prefijo + "divPTFiltroBasico").hide();
        $(instancia.prefijo + "divPTFiltroAvanzado").show();
    });

    $(instancia.prefijo + "btnVolver").click(function () {
        $(instancia.prefijo + "divConsulta").show();
        $(instancia.prefijo + "divVista").hide();
        $(instancia.prefijo + "divVista :input[type!='button']").val("");

    });
};

PuestoDeTrabajoConsultar.actualizarFiltroBasico = function (instancia) {
    var url = $.getUrl("/PuestoDeTrabajo/" + instancia.url);

    if (instancia.Filtros.Agente) {
        url += "&agente=" + instancia.Filtros.Agente;
    };

    if (instancia.Filtros.IdEmpresa) {
        url += "&idEmpresa=" + instancia.Filtros.IdEmpresa;
    }

    if (instancia.Filtros.IdSituacionDeRevista) {
        url += "&situacionRevista=" + instancia.Filtros.IdSituacionDeRevista;

    }

    if (instancia.Filtros.estadoPuestoActual) {
        url += "&esPuestoActualMab=" + instancia.Filtros.estadoPuestoActual;
    } else {
        url += "&esPuestoActualMab=" + false;
    }

    if (instancia.Filtros.estadoEmpresaNoCerrada) {
        url += "&estadoEmpresaNoCerrada=" + instancia.Filtros.estadoEmpresaNoCerrada;
    } else {
        url += "&estadoEmpresaNoCerrada=" + false;
    }


    url += "&codigoTipoCargo=" + $(instancia.prefijo + "fltCodigoTipoCargo").val();
    url += "&nombreTipoCargo=" + $(instancia.prefijo + "fltNombreTipoCargo").val();
    url += "&codigoAgrupamiento=" + $(instancia.prefijo + "fltCodigoAgrupamiento").val();
    url += "&codigoNivelCargo=" + $(instancia.prefijo + "fltCodigoNivelCargo").val();
    url += "&estadoPT=" + $(instancia.prefijo + "fltEstadoPuestoDeTrabajo").val();
    url += "&nombreAsignatura=" + $(instancia.prefijo + "fltNombreAsignatura").val();
    url += "&tipoDocumento=" + $(instancia.prefijo + "fltTipoDocumento").val();
    url += "&numeroDocumento=" + $(instancia.prefijo + "fltNumeroDocumento").val();
    url += "&tipoAgente=" + $(instancia.prefijo + "fltTipoAgente").val();
    url += "&codigoPosicionPN=" + $(instancia.prefijo + "fltCodigoPN").val();

    GrillaUtil.setUrl(instancia.grilla, url);
    GrillaUtil.actualizar(instancia.grilla);
};

PuestoDeTrabajoConsultar.actualizarFiltroAvanzado = function (instancia) {
    var url = $.getUrl("/PuestoDeTrabajo/" + instancia.url);

    if (instancia.Filtros.Agente) {
        url += "&agente=" + instancia.Filtros.Agente;
    }

    if (instancia.Filtros.estadoPuestoActual) {
        url += "&esPuestoActualMab=" + instancia.Filtros.estadoPuestoActual;
    }
    else {
        url += "&esPuestoActualMab=" + false;
    }

    if (instancia.Filtros.IdEmpresa) {
        url += "&idEmpresa=" + instancia.Filtros.IdEmpresa;
    }

    if (instancia.Filtros.estadoEmpresaNoCerrada) {
        url += "&estadoEmpresaNoCerrada=" + instancia.Filtros.estadoEmpresaNoCerrada;
    } else {
        url += "&estadoEmpresaNoCerrada=" + false;
    }

    // filtro por empresa
    url += "&cue=" + $(instancia.prefijo + "fltCUE").val();
    url += "&codigoEmpresa=" + $(instancia.prefijo + "fltCodigoEmpresa").val();
    url += "&nombreEmpresa=" + $(instancia.prefijo + "fltNombreEmpresa").val();
    url += "&estadoEmpresa=" + $(instancia.prefijo + "fltEstadoEmpresa").val();
    url += "&tipoEmpresa=" + $(instancia.prefijo + "fltTipoEmpresa").val();
    url += "&escuela=" + $(instancia.prefijo + "fltEsEscuela").val();
    url += "&nivelEducativo=" + $(instancia.prefijo + "fltNivelEducativo").val();
    url += "&nombreProgPresup=" + $(instancia.prefijo + "fltProgramaPresupuestado").val();

    // filtro por puesto de trabajo
    url += "&presupuestado=" + $(instancia.prefijo + "fltEsPresupuestado").val();
    url += "&liquidado=" + $(instancia.prefijo + "fltEsLiquidado").val();
    url += "&itinerante=" + $(instancia.prefijo + "fltEsItinerante").val();
    url += "&tipoPuesto=" + $(instancia.prefijo + "fltTipoPuesto").val();
    url += "&estadoAsignacion=" + $(instancia.prefijo + "fltEstadoAsignacion").val();

    
    url += "&situacionRevista=" + $(instancia.prefijo + "fltSituacionRevista").val();

    url += "&tipoInspeccion=" + $(instancia.prefijo + "fltTipoInspeccion").val();

    // filtro por asignatura
    url += "&materiaEspecial=" + $(instancia.prefijo + "fltEsEspecial").val();
    url += "&codigoAsignatura=" + $(instancia.prefijo + "fltCodigoAsignatura").val();

    // filtro por division
    url += "&turno=" + $(instancia.prefijo + "fltTurno").val();
    url += "&gradoAnio=" + $(instancia.prefijo + "fltGradoAnio").val();
    url += "&division=" + $(instancia.prefijo + "fltDivision").val();

    // filtro por carrera
    url += "&nombreCarrera=" + $(instancia.prefijo + "fltNombreCarrera").val();

    GrillaUtil.setUrl(instancia.grilla, url);
    GrillaUtil.actualizar(instancia.grilla);
};

PuestoDeTrabajoConsultar.inicializarGrilla = function (instancia) {
    var titulos = ["Id", "CUE", "Código Empresa", "Nombre Empresa", "Estado Empresa", "Código PN", "Código Tipo Cargo", "Nombre Tipo Cargo", "Nivel Cargo", "Estado Puesto"];
    var propiedades = ["Id", "CUE", "CodigoEmpresa", "NombreEmpresa", "EstadoEmpresa", "CodigoPN", "CodigoTipoCargo", "NombreTipoCargo", "NivelCargo", "EstadoPuesto"];
    var tipos = ["integer", null, null, null, null, null, null, null, null, null];
    var url = $.getUrl("/PuestoDeTrabajo/" + instancia.url);
    var id = instancia.prefijo + "listPuestoDeTrabajo";
    var pager = instancia.prefijo + "pagerPuestoDeTrabajo";

    var grilla = $(id).jqGrid({
        datatype: "local",
        url: url,
        colNames: titulos,
        colModel: GrillaUtil.crearColumnas(propiedades, tipos, "Id"),
        pager: pager,
        toppager: true,
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "Id",
        multiselect: false,
        height: "100%",
        emptyrecords: ""
    });

    grilla.id = id;
    grilla.id_limpio = id.replace("#", "");
    grilla.pager = pager;
    grilla.botones = ["Seleccione"];
    instancia.grilla = grilla;

    $(grilla.id + "_toppager_center", grilla.id + "_toppager").remove();
    $(".ui-paging-info", grilla.id + "_toppager").remove();
    $(grilla.id + "_toppager_left").append('<table cellspacing="0" cellpadding="0" border="0" style="float: left; table-layout: auto;" class="ui-pg-table navtable"><tbody><tr></tr></tbody></table>');

    GrillaUtil.crearBotonSeleccion(instancia.grilla, "Seleccionar", "pin-s",
        function () {
            instancia.seleccion = GrillaUtil.getSeleccionFilas(instancia.grilla, false);
            if (instancia.seleccion) {
                PuestoDeTrabajoConsultar.onSelectPuestoDeTrabajo(instancia)
            }
            else {
                AbmcUtil.mensajeSeleccion();
            }
        });

    var divMensajes = "#gview_" + grilla.id_limpio;
    $(divMensajes).append("<div id='" + grilla.id_limpio + "_sinConsulta' class='ui-mensaje'>Aún no se ha realizado ninguna búsqueda</div>");
    $(divMensajes).append("<div id='" + grilla.id_limpio + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados con los datos de búsqueda ingresados</div>");

    instancia.grilla.setGridParam({ loadComplete: function (data) { GrillaUtil.mostrarBotones(instancia.grilla); } }).trigger("reloadGrid");
};

// Redefinir para agregar comportamiento particular de cada caso de uso
PuestoDeTrabajoConsultar.seleccionarPT = null;

PuestoDeTrabajoConsultar.limpiarGrilla = function (instancia) {
    var url = $.getUrl("/PuestoDeTrabajo/" + instancia.url);
    GrillaUtil.limpiar(instancia.grilla, url);
};

PuestoDeTrabajoConsultar.cargarVista = function (instancia) {
    $(instancia.prefijo + "divConsulta").hide();
    $(instancia.prefijo + "divVista").show();

    $(instancia.prefijo + "Id").val(instancia.seleccion);

    // TODO Vicky cargar los datos mediante un template
    $.get($.getUrl("/PuestoDeTrabajo/GetDetalle/"), { id: instancia.seleccion }, function (data) {
        $(instancia.prefijo + "VerCodigoCargo").val(data.CodigoCargo);
        $(instancia.prefijo + "VerNombreCargo").val(data.NombreCargo);
        $(instancia.prefijo + "VerHoras").val(data.Horas);
        $(instancia.prefijo + "VerPlanEstudio").val(data.PlanEstudio);
        $(instancia.prefijo + "VerMateria").val(data.Materia);
        $(instancia.prefijo + "VerTurno").val(data.Turno);
        $(instancia.prefijo + "VerGradoAnio").val(data.GradoAnio);
        $(instancia.prefijo + "VerSeccionDivision").val(data.SeccionDivision);
        $(instancia.prefijo + "VerCupof").val(data.Cupof);
    });
};

PuestoDeTrabajoConsultar.reset = function (instancia) {
    $(instancia.prefijo + "btnFiltroBasico").click();
    $(instancia.prefijo + "btnLimpiarBasico").click();
    $(instancia.prefijo + "btnLimpiarAvanzado").click();

    $(instancia.prefijo + "divConsulta").show();
    $(instancia.prefijo + "divVista").hide();
};

PuestoDeTrabajoConsultar.onSelectPuestoDeTrabajo = function (instancia) {
    PuestoDeTrabajoConsultar.cargarVista(instancia);
    if (PuestoDeTrabajoConsultar.seleccionarPT) {
        PuestoDeTrabajoConsultar.seleccionarPT(instancia);
    }
};

PuestoDeTrabajoConsultar.setFiltroAgente = function (instancia, idAgente) { 
    instancia.Filtros.Agente = idAgente;
};

PuestoDeTrabajoConsultar.setFiltroEstadoPuestoActual = function (instancia, esPuestoActual) {
    instancia.Filtros.estadoPuestoActual = esPuestoActual;
};

PuestoDeTrabajoConsultar.setFiltroEmpresa = function (instancia, idEmpresa) {
    instancia.Filtros.IdEmpresa = idEmpresa;
};

PuestoDeTrabajoConsultar.setFiltroSituacionDeRevista = function (instancia, idSituacionRevista) {
    instancia.Filtros.IdSituacionDeRevista = idSituacionRevista;
};

PuestoDeTrabajoConsultar.setFiltroEstadoEmpresaNoCerrada = function (instancia, estadoEmpresaNoCerrada) {
    instancia.Filtros.estadoEmpresaNoCerrada = estadoEmpresaNoCerrada;
};