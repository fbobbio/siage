var AbmcGrillaMixto = {};

AbmcGrillaMixto.accionListar = "ProcesarBusqueda";
AbmcGrillaMixto.accionDetalle = "ProcesarDetalle";
AbmcGrillaMixto.accionVer = "Ver";
AbmcGrillaMixto.accionEliminar = "Eliminar";
AbmcGrillaMixto.accionEditar = "Editar";
AbmcGrillaMixto.accionRegistrar = "Registrar";
AbmcGrillaMixto.accionReactivar = "Reactivar";
AbmcGrillaMixto.accionSeleccionar = "Seleccionar";
AbmcGrillaMixto.iconos = { "Agregar": "plus", "Editar": "pencil", "Eliminar": "trash", "Ver": "search", "Seleccionar": "pin-s", "Reactivar": "check", "Imprimir": "print" };

AbmcGrillaMixto.init = function (idGrilla, entidad, titulos, propiedades, tipos, id, botones, ordenarPor, titulosDetalle, propiedadesDetalle, tiposDetalle, idDetalle, botonesDetalle) {

    AbmcGrillaMixto.accionListar = $.getUrl("/" + entidad + "/" + AbmcGrillaMixto.accionListar);
    AbmcGrillaMixto.accionDetalle = $.getUrl("/" + entidad + "/" + AbmcGrillaMixto.accionDetalle);

    var grid = $(idGrilla).jqGrid({
        url: AbmcGrillaMixto.accionListar,
        datatype: "local",
        mtype: "GET",
        colNames: titulos,
        colModel: GrillaUtil.crearColumnas(propiedades, tipos, id),
        pager: "#pager",
        toppager: true,
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        sortname: ordenarPor,
        sortorder: "asc",
        viewrecords: true,
        autowidth: true,
        height: "100%",
        emptyrecords: "",
        subGrid: true,
        subGridRowExpanded: function (subgrid_id, row_id) {
            var idSubGrilla = idGrilla.replace("#", "") + "Detalle";
            var pagerSubGrilla = "pagerDetalle";

            $(idGrilla).collapseSubGridRow(grid.Subgrilla.idExtendido);
            grid.Subgrilla.idExtendido = row_id;

            $("#" + subgrid_id).html("<table id='" + idSubGrilla + "' class='scroll'></table><div id='" + pagerSubGrilla + "' class='scroll'></div>");

            var subgrilla = $("#" + idSubGrilla).jqGrid({
                url: AbmcGrillaMixto.accionDetalle + "&id=" + row_id,
                datatype: "json",
                mtype: "GET",
                colNames: titulosDetalle,
                colModel: GrillaUtil.crearColumnas(propiedadesDetalle, tiposDetalle, idDetalle),
                pager: "#" + pagerSubGrilla,
                toppager: true,
                autowidth: true,
                sortname: 'Id',
                sortorder: "asc",
                height: '100%'
            });

            subgrilla.id = "#" + idSubGrilla;
            subgrilla.pager = "#" + pagerSubGrilla;
            subgrilla.botones = botonesDetalle;

            AbmcGrillaMixto.agregarComportamiento(subgrilla);

            // Agrego div para mostrar mensajes personalizado
            $("#gview_" + idSubGrilla).append("<div id='" + idSubGrilla + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados</div>");

            subgrilla.crearBotones(true);
            subgrilla.trigger("reloadGrid");
            subgrilla.setGridParam({ gridComplete: function (data) { GrillaUtil.mostrarBotones(subgrilla); } });
        }
    });

    // Agrego propiedades que almacenan informacion sobre la grilla
    grid.id = idGrilla;
    grid.pager = "#pager";
    grid.botones = botones;

    // Agrego las propiedades que almacenan informacion sobre la subgrilla
    grid.Subgrilla = {};
    grid.Subgrilla.id = grid.id + "Detalle";
    grid.Subgrilla.idExtendido = 0;

    // Agrego funcionalidad util para manejar la grilla
    AbmcGrillaMixto.agregarComportamiento(grid);

    // Agrego los botones a la grilla padre 
    grid.crearBotones(false);

    // Agrego div para mostrar mensajes personalizado
    var divMensajes = "#gview_" + grid.id_limpio;
    $(divMensajes).append("<div id='" + grid.id_limpio + "_sinConsulta' class='ui-mensaje'>Aún no se ha realizado ninguna búsqueda</div>");
    $(divMensajes).append("<div id='" + grid.id_limpio + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados con los datos de búsqueda ingresados</div>");

    // Elimino el pager superior
    $(grid.id + "_toppager_center", grid.id + "_toppager").remove();
    $(".ui-paging-info", grid.id + "_toppager").remove();

    // Limpio los datos de la consulta para el primer ingreso y oculto los botones que no sean necesarios
    grid.setGridParam({ loadComplete: function (data) { GrillaUtil.mostrarBotones(grid); } }).trigger("reloadGrid");

    return grid;
};

AbmcGrillaMixto.agregarComportamiento = function (grid) {
    grid.id_limpio = grid.id.replace("#", "");

    // Permite agregar filtros de consulta
    grid.agregarParametros = function (parametros) {
        $(grid.id).setGridParam({ url: AbmcGrillaMixto.accionListar + parametros });
    };

    // Actualizar el contenido de la grilla
    grid.actualizar = function () {
        GrillaUtil.actualizar(grid);
    };

    // Limpiar los filtros de busqueda y el contenido de la grilla
    grid.limpiar = function () {
        GrillaUtil.limpiar(grid, AbmcGrillaMixto.accionListar);
    };

    // Crear los botones en la barra superior de la grilla
    grid.crearBotones = function (detalle) {
        AbmcGrillaMixto.crearBotones(grid, detalle);
    }
};

AbmcGrillaMixto.crearBotones = function (grid, detalle) {
    // Elimino la paginacion en la barra de arriba
    $(grid.id + "_toppager_center", grid.id + "_toppager").remove();
    $(".ui-paging-info", grid.id + "_toppager").remove();

    if (grid.botones.length !== 0) {
        $(grid.id + "_toppager_left").append('<table cellspacing="0" cellpadding="0" border="0" style="float: left; table-layout: auto;" class="ui-pg-table navtable"><tbody><tr></tr></tbody></table>');
    }

    var i = 0;
    for (i; i < grid.botones.length; i++) {
        switch (grid.botones[i]) {
            case "Agregar":
                AbmcGrillaMixto.crearBotonSeleccion(grid, grid.botones[i], detalle,
                    function () { AbmcMixto.cargarModelo(AbmcGrillaMixto.accionRegistrar, null, detalle); });
                break;
            case "Imprimir":
                AbmcGrillaMixto.crearBotonSeleccion(grid, grid.botones[i], detalle,
                    function () { AbmcUtil.mensajeSeleccion(); });
                break;
            default:
                AbmcGrillaMixto.crearBotonSeleccion(grid, grid.botones[i], detalle);
                break;
        }
    }
};

AbmcGrillaMixto.crearBotonSeleccion = function (grid, accion, detalle, onClick) {
    GrillaUtil.crearBotonSeleccion(grid, accion, AbmcGrillaMixto.iconos[accion], function () {
        if (onClick) {
            onClick();
        }
        else {
            var seleccionado = GrillaUtil.getSeleccionFilas(grid, false);
            if (seleccionado) {
                AbmcMixto.cargarModelo(AbmcGrillaMixto["accion" + accion], seleccionado, detalle);
            }
            else {
                AbmcUtil.mensajeSeleccion();
            }
        }		
	});
};