var ConsultaEmpresaExterna = {};
ConsultaEmpresaExterna.url = $.getUrl("/EmpresaExterna/ProcesarBusqueda");

ConsultaEmpresaExterna.init = function (div, prefijo) {
    var instancia = {};
    instancia.div = div;

    instancia.Filtros = {};
    instancia.url = ConsultaEmpresaExterna.url;

    if (prefijo) {
        $(div).agregarPrefijo(prefijo);
        instancia.prefijo = "#" + prefijo + "_";
    }
    else {
        instancia.prefijo = "#";
    }

    ConsultaEmpresaExterna.inicializarBotones(instancia);
    ConsultaEmpresaExterna.inicializarGrilla(instancia);

    return instancia;
};

ConsultaEmpresaExterna.inicializarGrilla = function (instancia) {
    var id = instancia.prefijo + "list";
    var pager = instancia.prefijo + "pager";
    var titulos = ['Id', 'Nombre', 'Razón Social', 'Cuil', 'Cuit', 'Activo'];
    var propiedades = ['Id', 'Nombre', 'RazonSocial', 'Cuil', 'Cuit', 'Activo'];
    var tipos = ['integer', null, null, null, null, null];

    var grid = $(id).jqGrid({
        datatype: "local",
        colNames: titulos,
        colModel: GrillaUtil.crearColumnas(propiedades, tipos, "Id"),
        pager: pager,
        toppager: true,
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        multiselect: false,
        width: document.body.offsetWidth - 650,
        height: "100%",
        emptyrecords: ""
    });

    grid.id = id;
    grid.id_limpio = id.replace("#", "");
    grid.pager = pager;
    grid.botones = ["Seleccionar"];

    $(grid.id + "_toppager_left").append('<table cellspacing="0" cellpadding="0" border="0" style="float: left; table-layout: auto;" class="ui-pg-table navtable"><tbody><tr></tr></tbody></table>');
    grid.navButtonAdd(grid.id + "_toppager_left",
    {
        position: "first",
        caption: "Seleccionar",
        title: "Seleccionar",
        buttonicon: "ui-icon-pin-s",
        onClickButton: function () {
            instancia.seleccion = GrillaUtil.getSeleccionFilas(grid, false);
            if (instancia.seleccion && instancia.seleccion.lenght !== 0) {
                ConsultaEmpresaExterna.seleccionar(instancia);
            }
            else {
                AbmcUtil.mensajeSeleccion();
            }
        }
    });

    $(grid.id + "_toppager_center", grid.id + "_toppager").remove();
    $(".ui-paging-info", grid.id + "_toppager").remove();

    // Agrego div para mostrar mensajes personalizado
    $("#gview_" + grid.id_limpio).append("<div id='" + grid.id_limpio + "_sinConsulta' class='ui-mensaje'>Aún no se ha realizado ninguna búsqueda</div>");
    $("#gview_" + grid.id_limpio).append("<div id='" + grid.id_limpio + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados con los datos de búsqueda ingresados</div>");

    // Limpio los datos de la consulta para el primer ingreso y oculto los botones que no sean necesarios
    grid.setGridParam({ loadComplete: function (data) { GrillaUtil.mostrarBotones(grid); } }).trigger("reloadGrid");

    instancia.grilla = grid;
};

ConsultaEmpresaExterna.inicializarBotones = function (instancia) {
    $(instancia.prefijo + "btnConsultar").click(function () {
        var url = ConsultaEmpresaExterna.url
            + "&filtroNombre=" + $(instancia.prefijo + "filtroNombre").val()
            + "&filtroCuitCuil=" + $(instancia.prefijo + "filtroCuitCuil").val()
            + "&filtroTipoEmpresa=" + $(instancia.prefijo + "filtroTipoEmpresa").val()
            + "&chkBusquedaEmpresaExternasEliminadas=" + $(instancia.prefijo + "filtroEliminadas").is(":checked");

        GrillaUtil.setUrl(instancia.grilla, url);
        GrillaUtil.actualizar(instancia.grilla);
    });

    $(instancia.prefijo + "btnLimpiar").click(function () {
        $(instancia.prefijo + "divConsulta :input[type!='button']").val("");
        GrillaUtil.limpiar(instancia.grilla, ConsultaEmpresaExterna.url);
    });

    $(instancia.prefijo + "btnVolver").click(function () {
        $(instancia.prefijo + "divConsulta").show();
        $(instancia.prefijo + "divDatos").hide();
    });
};

ConsultaEmpresaExterna.seleccionar = function (instancia) {
    var url = $.getUrl("/EmpresaExterna/GetEmpresaExterna");
    $(instancia.prefijo + "Id").val(instancia.seleccion);
    $.get(url, { empresa: instancia.seleccion }, function (datos) {
        if (datos) {
            $.formatoFormulario(datos, instancia.prefijo.replace("#", ""));
            $(instancia.prefijo + "divConsulta").hide();
            $(instancia.prefijo + "divDatos").show();
           
        }
    });
};