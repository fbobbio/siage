var AbmcGrillaDetalle = {};

AbmcGrillaDetalle.accionListar = "ProcesarBusqueda";
AbmcGrillaDetalle.accionDetalle = "ProcesarDetalle";
AbmcGrillaDetalle.accionVer = "Ver";
AbmcGrillaDetalle.accionEliminar = "Eliminar";
AbmcGrillaDetalle.accionEditar = "Editar";
AbmcGrillaDetalle.accionRegistrar = "Registrar";
AbmcGrillaDetalle.accionReactivar = "Reactivar";
AbmcGrillaDetalle.accionSeleccionar = "Seleccionar";

AbmcGrillaDetalle.iconos = { "Agregar": "plus", "Editar": "pencil", "Eliminar": "trash", "Ver": "search", "Seleccionar": "pin-s", "Reactivar": "check", "Imprimir": "print" };

AbmcGrillaDetalle.init = function (idGrilla, entidad, titulos, propiedades, tipos, id, ordenarPor, titulosDetalle, propiedadesDetalle, tiposDetalle, idDetalle, botones) {

    AbmcGrillaDetalle.accionListar = $.getUrl("/" + entidad + "/" + AbmcGrillaDetalle.accionListar);
    AbmcGrillaDetalle.accionDetalle = $.getUrl("/" + entidad + "/" + AbmcGrillaDetalle.accionDetalle);

    var grid = $(idGrilla).jqGrid({
        url: AbmcGrillaDetalle.accionListar,
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
                url: AbmcGrillaDetalle.accionDetalle + "&id=" + row_id,
                datatype: "local",
                mtype: "GET",
                colNames: titulosDetalle,
                colModel: GrillaUtil.crearColumnas(propiedadesDetalle, tiposDetalle, idDetalle),
                rowNum: 10,
                pager: "#" + pagerSubGrilla,
                toppager: true,
                autowidth: true,
                sortname: 'Id',
                sortorder: "asc",
                height: '100%'
            });

            subgrilla.id = "#" + idSubGrilla;
            subgrilla.pager = "#" + pagerSubGrilla;
            subgrilla.botones = botones;
            AbmcGrillaDetalle.agregarComportamiento(subgrilla);

            // Agrego div para mostrar mensajes personalizado
            $("#gview_" + idSubGrilla).append("<div id='" + idSubGrilla + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados</div>");

            subgrilla.crearBotones();
            subgrilla.setGridParam({ 
                loadComplete: function (data) { GrillaUtil.mostrarBotones(subgrilla); },
                datatype: 'json'
            }).trigger('reloadGrid');
        }
    });

    // Agrego propiedades que almacenan informacion sobre la grilla
    grid.id = idGrilla;
    grid.pager = "#pager";
    grid.botones = [];

    // Agrego las propiedades que almacenan informacion sobre la subgrilla
    grid.Subgrilla = {};
    grid.Subgrilla.id = grid.id + "Detalle";
    grid.Subgrilla.pager = "pagerDetalle";
    grid.Subgrilla.idExtendido = 0;

    // Agrego funcionalidad util para manejar la grilla
    AbmcGrillaDetalle.agregarComportamiento(grid);

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

AbmcGrillaDetalle.agregarComportamiento = function (grid) {
    grid.id_limpio = grid.id.replace("#", "");

    // Permite agregar filtros de consulta
    grid.agregarParametros = function (parametros) {
        $(grid.id).setGridParam({ url: AbmcGrillaDetalle.accionListar + parametros });
    };

    // Actualizar el contenido de la grilla
    grid.actualizar = function () {
        GrillaUtil.actualizar(grid);
    };

    // Limpiar los filtros de busqueda y el contenido de la grilla
    grid.limpiar = function () {
        GrillaUtil.limpiar(grid, AbmcGrillaDetalle.accionListar);
    };

    // Crea los botones segun el array seteado en el objeto
    grid.crearBotones = function () {
        AbmcGrillaDetalle.crearBotones(grid);
    };
};

AbmcGrillaDetalle.crearBotones = function (grid) {
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
                AbmcGrillaDetalle.crearBotonSeleccion(grid, grid.botones[i],
                    function () { AbmcDetalle.cargarModelo(AbmcGrillaDetalle.accionRegistrar, null); });
                break;
            case "Imprimir":
                AbmcGrillaDetalle.crearBotonSeleccion(grid, grid.botones[i], 
                    function () { AbmcUtil.mensajeSeleccion(); });
                break;
            default:
                AbmcGrillaDetalle.crearBotonSeleccion(grid, grid.botones[i]);
                break;
        }
    }
};

AbmcGrillaDetalle.crearBotonSeleccion = function (grid, accion, onClick) {
    GrillaUtil.crearBotonSeleccion(grid, accion, AbmcGrillaDetalle.iconos[accion], function() {
        if (onClick) {
            onClick();
        }
        else {
            var seleccionado = GrillaUtil.getSeleccionFilas(grid, false);
            if (seleccionado) {
                AbmcDetalle.cargarModelo(AbmcGrillaDetalle["accion" + accion], seleccionado);
            }
            else {
                AbmcUtil.mensajeSeleccion();
            }
        };
	});
};