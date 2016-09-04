var VinculoEmpresaEdificio = {};
var divSeleccionEmpresa = "#divVinculoEmpresaEdificio";
var divSeleccionEdificio = "#divEdificio";
var divVinculo = "#divVinculo";
var divDatosEmpresa = "#divDatosGeneralesEmpresa";
var divConsultaEmpresa = "#divFiltrosDeBusqueda";
var divConsultaEdificio = "#divConsultaEdificio";
var Estado;
var EmpresaSeleccionado;
var listaEdificios;
var grillaRegistrosRecientes = "#divGrillaAbmc";
var PrefijoVinculo;
var grillaVinculos;
VinculoEmpresaEdificio.init = function (prefijo) {

    var controller = "VinculoEmpresaEdificio";
    var orderBy = "CodigoEmpresa";
    var titulos = ['Id', 'Código empresa', 'Identificador edificio', 'Tipo edificio', 'Fecha desde', 'Fecha hasta', 'Estado', 'Observación'];
    var propiedades = ['Id', 'CodigoEmpresa', 'IdentificadorEdificio', 'TipoEdificio', 'FechaDesde', 'FechaHasta', 'Estado', 'Observacion'];
    var tipos = ['integer', 'string', 'string', 'string', 'string', 'string', 'string', 'string'];
    var botones = ["Ver", "Eliminar", "Agregar"]; // en orden inverso al que se mostraran
    var key = 'Id';


    grillaVinculos = AbmcGrilla.init("#list", controller, titulos, propiedades, tipos, key, orderBy, botones, null);
    Abmc.init(controller, grillaVinculos);

    $('#divConsulta :input').changePatch(function () {
        var parametros = "&" + $("#divConsulta :input[type!='button']").getFiltros();
        grillaVinculos.agregarParametros(parametros);
    });

    $("#listRecientes").setGridWidth(730, true);
    $("#seleccionEdificio").hide();

    if (prefijo != "")
        PrefijoVinculo = "#" + prefijo + "_";
    else
        PrefijoVinculo = "#";
};

VinculoEmpresaEdificio.cargarCombosDomicilio = function () {

    $("#FiltroLocalidad").CascadingDropDown("#FiltroDepartamentoProvincial", $.getUrl('/Edificio/CargarLocalidadByDepartamentoProvincial'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idDepartamento: $("#FiltroDepartamentoProvincial").val() };
        }
    });

    $("#FiltroBarrio").CascadingDropDown("#FiltroLocalidad", $.getUrl('/Edificio/CargarBarrioByLocalidad'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idLocalidad: $("#FiltroLocalidad").val() };
        }
    });
    $("#FiltroCalle").CascadingDropDown("#FiltroLocalidad", $.getUrl('/Edificio/CargarCalleByLocalidad'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idLocalidad: $("#FiltroLocalidad").val() };
        }
    });
};

VinculoEmpresaEdificio.editor = function (preficovinculo) {
    PrefijoVinculo = preficovinculo;
    ConsultarEmpresa.procesarSeleccion = $.getUrl('/VinculoEmpresaEdificio/ProcesarSeleccion');
    VinculoEmpresaEdificio.consultaEmpresa();
    $("#list").setGridWidth(680, true);

};

VinculoEmpresaEdificio.consultaEmpresa = function () {
    $(divSeleccionEmpresa).show();
    $(divConsultaEmpresa).show();
    $(divDatosEmpresa).hide();

    $(divSeleccionEdificio).hide();
    $(divVinculo).hide();
    $(grillaRegistrosRecientes).hide();
};

VinculoEmpresaEdificio.grillaEdificios = function () {
    var controllerEdificio = "VinculoEmpresaEdificio";
    var titulosEdificio = ['Id', 'Identificador edificio', 'Tipo edificio', 'Estado', 'Funcion edificio'];
    var propiedadesEdificio = ['Id', 'IdentificadorEdificio', 'TipoEdificio', 'Estado', 'FuncionEdificio'];
    var tiposEdificio = ['integer', 'string', 'string', 'string', 'string'];
    var keyEdificio = 'Id';
    var funcionSeleccionEdificio = VinculoEmpresaEdificio.seleccionarEdificio;
    var captionEdificio = "Selección edificio"
    var urlEdificio = $.getUrl('/VinculoEmpresaEdificio/ProcesarBusquedaEdificio/');
    var pager = "#pagerEdificio";
    var grillaEdificio = Grilla.Seleccion.init("#seleccionEdificio", titulosEdificio, propiedadesEdificio, tiposEdificio, keyEdificio, "", captionEdificio, funcionSeleccionEdificio, true, pager);

    $('#divConsultaEdificio :input').changePatch(function () {
        var parametrosEdificio = $("#divConsultaEdificio :input[type!='button']").getFiltros();
        var urlFinal = urlEdificio + "&" + parametrosEdificio;
        GrillaUtil.setUrl(grillaEdificio, urlFinal);
    });

    $("#btnConsultarEdificio").click(function () {
        var filtros = $("#divConsultaEdificio :input[type!=button][value!='']").length > 0;
        if (!filtros) {
            Mensaje.Advertencia.botones = false;
            Mensaje.Advertencia.texto = "Se debe filtrar por al menos un criterio";
            Mensaje.Advertencia.mostrar();
            return;
        }
        grillaEdificio.setGridParam({ datatype: "json" });
        grillaEdificio.trigger("reloadGrid");
        $("#seleccionEdificio").show();
    });
    $("#seleccionEdificio").setGridWidth(680, true);
};

ConsultarEmpresa.seleccionarEmpresa = function (id) {
    if (id.seleccion && id.seleccion > 0) {
        var empresaSeleccionada = $("#vincularEdificio_list").getRowData(id.seleccion);
        if (empresaSeleccionada && empresaSeleccionada.EstadoEmpresa == "AUTORIZADA") {
            VinculoEmpresaEdificio.limpiarConsultaEdificio();
            $(divSeleccionEmpresa).show();
            $(divSeleccionEdificio).show();
            $(divVinculo).hide();
            idEmpresa = id.seleccion;
            EmpresaSeleccionado = GrillaUtil.getFila($("#" + prefijoVinculo + "_list"), idEmpresa);
            VinculoEmpresaEdificio.cargarEmpresa(EmpresaSeleccionado);
            VinculoEmpresaEdificio.cargarCombosDomicilio();
            $("#divConsultaEdificio").show();
        }
        else {
            Mensaje.Advertencia.botones = false;
            Mensaje.Advertencia.texto = "Solo pueden generar vinculo aquellas empresas que esten en estado: Autorizada";
            Mensaje.Advertencia.mostrar();
        }
    }
};

VinculoEmpresaEdificio.cargarEmpresa = function (empresa) {
    //empresa = JSON.parse(empresa);
    if (empresa) {
        $(PrefijoVinculo + "divFiltrosDeBusqueda").hide();
        $(PrefijoVinculo + "divDatosGeneralesEmpresa").show();

        //$("#Id").val(empresa.Id);
        $(PrefijoVinculo + "VerCodigoEmpresa").val(empresa.CodigoEmpresa);
        $(PrefijoVinculo + "VerNombreEmpresa").val(empresa.NombreEmpresa);
        if (empresa.CUE) {
            $(PrefijoVinculo + "VerCueEmpresa").val(empresa.CUE.split("-")[0]);
            $(PrefijoVinculo + "VerCueAnexoEmpresa").val(empresa.CUE.split("-")[1]);
        }
        if (empresa.TipoEducacion) {
            $(PrefijoVinculo + "VerTipoEducacion").val(empresa.TipoEducacion);
        }
        if (empresa.NivelEducativo) {
            $(PrefijoVinculo + "VerNivelEducativo").val(empresa.NivelEducativo);
        }

        $(PrefijoVinculo + "VerTipoEmpresa").val(empresa.TipoEmpresa);
        $(PrefijoVinculo + "VerEstadoEmpresa").val(empresa.EstadoEmpresa);
    }
};

VinculoEmpresaEdificio.seleccionarEdificio = function (idEdificio) {
    if (idEdificio && idEdificio.length > 0) {
        $(divSeleccionEmpresa).show();
        $(divConsultaEmpresa).hide();
        $(divDatosEmpresa).show();
        $("#seleccionEdificio_toppager_left").hide();
        $("#divEdificio :input").attr("disabled", true);
        $(divSeleccionEdificio).show();
        $(divConsultaEdificio).hide();
        $(divVinculo).show();
        listaEdificios = idEdificio;
    }
};

VinculoEmpresaEdificio.limpiar = function () {
    VinculoEmpresaEdificio.consultaEmpresa();
    VinculoEmpresaEdificio.limpiarConsultaEdificio();
    ConsultarEmpresa.modoConsulta(VinculoEmpresaEdificio.instanciaEmpresa);
};

VinculoEmpresaEdificio.limpiarConsultaEdificio = function () {
    $("#seleccionEdificio_toppager_left").show();
    $("#divEdificio :input").attr("disabled", false);
    $('#divConsultaEdificio :input[type!=button]').val("");
    $(divSeleccionEdificio).hide();
    $(divConsultaEdificio).show();
    $(divVinculo).hide();
    listaEdificios = null;
    $("#seleccionEdificio").clearGridData();
};

VinculoEmpresaEdificio.cargarModelo = Abmc.cargarModelo;
Abmc.cargarModelo = function (estado, id) {
    if (estado == "Eliminar") {
        $.getJSON($.getUrl('/VinculoEmpresaEdificio/ValidarEstadoEliminar'), { idVinculo: id },
                    function (validacion) {
                        if (!validacion) {
                            Mensaje.Advertencia.texto = "El vinculo no se puede dar de baja ya que el estado de la empresa no es 'Autorizado' o 'En proceso de cierre autorizado/notificado'";
                            Mensaje.Advertencia.botones = false;
                            Mensaje.Advertencia.mostrar();
                        }
                        else {
                            VinculoEmpresaEdificio.cargarModelo(estado, id);
                        }
                    });
    }
    else {
        VinculoEmpresaEdificio.cargarModelo(estado, id);
    }
};

Abmc.preCargarModelo = function (estado, id) {
    prefijoVinculo = "vincularEdificio";
    Estado = estado;
    switch (Estado) {
        case 'Registrar':
            return true;
            break;
        case 'Eliminar':
            var seleccionado = GrillaUtil.getFila(grillaVinculos, id);
            if (seleccionado.Estado == "INACTIVO") {
                Mensaje.Advertencia.botones = false;
                Mensaje.Advertencia.texto = "El vínculo ya se encuentra inactivo";
                Mensaje.Advertencia.mostrar();
                return false;
            }
            return true;
            break;
        default:
            return true;
            break;
    };
};

VinculoEmpresaEdificio.cargarVinculo = function (estado, id) {
    $.get($.getUrl('/VinculoEmpresaEdificio/GetVinculoEmpresaEdificio?id=' + id), null,
                    function (data) {
                        if (data != null) {
                            VinculoEmpresaEdificio.cargarEmpresa(data.empresa);
                            $("#edificioTemplate").tmpl(data).appendTo("#divEdificioTemplate");
                            $(divSeleccionEmpresa).show();
                            $(divConsultaEmpresa).hide();
                            $(divDatosEmpresa).show();
                            $("#seleccionEdificio_toppager_left").hide()
                            $("#divEdificio :input").attr("disabled", true)
                            $("#divEmpresa :input").attr("disabled", true);
                            $("#vincularEdificio_btnVolver").hide();
                            $(divSeleccionEdificio).hide();
                            $(divConsultaEdificio).show();
                            $(divVinculo).show();
                            $("#Estado").val(data.estadoVinculo);
                            if (data.fechaHasta)
                                $("#FechaHasta").val(data.fechaHasta);
                        }
                    });
};

Abmc.postCargarModelo = function (estado, id) {
    VinculoEmpresaEdificio.grillaEdificios();
    VinculoEmpresaEdificio.instanciaEmpresa = ConsultarEmpresa.init('SinVista', "#divVinculoEmpresaEdificio", "vincularEdificio", false);
    $("#listaEmpresas").setGridWidth(680, true);
    $("#listRecientes").setGridWidth(720, true);
    $("#vincularEdificio_list").setGridWidth(680, true);
    $("#btnBusquedaAvanzada").hide();
    Estado = estado;
    switch (Estado) {
        case 'Registrar':
            $(divSeleccionEmpresa).show();
            $(divConsultaEmpresa).show();
            $(divDatosEmpresa).hide();
            $("#seleccionEdificio_toppager_left").show()
            $("#divEdificio :input").attr("disabled", false)
            $("#divEmpresa :input").attr("disabled", false);
            $(divSeleccionEdificio).hide();
            $(divConsultaEdificio).hide();
            $(divVinculo).hide();
            break;
        case 'Eliminar':
            VinculoEmpresaEdificio.cargarVinculo(estado, id);
            $("#Motivo").attr("disabled", false);
            break;
        case 'Ver':
            VinculoEmpresaEdificio.cargarVinculo(estado, id);
            $("#Motivo").attr("disabled", true);
            break;
        default: break;
    }
};

Abmc.preEnviarModelo = function (datos) {
    switch (Estado) {
        case 'Registrar':
            var model = {};
            if (EmpresaSeleccionado && EmpresaSeleccionado.Id) {
                model.Empresa = EmpresaSeleccionado.Id;
            }
            if (listaEdificios) {
                model.ListaEdificios = listaEdificios;
            }
            $.formatoModelBinder(model, datos, "");
            break;
        case 'Eliminar':
            break;
    }
};

Abmc.postEnviarModelo = function (data) {
    if (data && data.status) {

        switch (Estado) {
            case 'Registrar':
                VinculoEmpresaEdificio.limpiar();
                //agrego a lista de recientes
                $.getJSON($.getUrl('/VinculoEmpresaEdificio/RegistroReciente'), null,
                    function (vinculosReciente) {
                        Abmc.grilla.recientes.delRowData(0);
                        Abmc.grilla.recientes.addRowData(vinculosReciente.Id, vinculosReciente, "first");
                        $("#divGrillaAbmc").show();
                    });
                break;
            default:
                break;

        }
    }
}
