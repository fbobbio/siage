var AbmcGrilla = {};

AbmcGrilla.accionListar = "ProcesarBusqueda";
AbmcGrilla.accionDetalle = "ProcesarDetalle";
AbmcGrilla.accionVer = "Ver";
AbmcGrilla.accionEliminar = "Eliminar";
AbmcGrilla.accionEditar = "Editar";
AbmcGrilla.accionRegistrar = "Registrar";
AbmcGrilla.accionReactivar = "Reactivar";
AbmcGrilla.accionSeleccionar = "Seleccionar";
AbmcGrilla.accionHistorial = "Historial";

AbmcGrilla.iconos = { "Agregar": "plus", "Editar": "pencil", "Eliminar": "trash", "Ver": "search", "Seleccionar": "pin-s", "Reactivar": "check", "Imprimir": "print" };

AbmcGrilla.init = function (idGrilla, entidad, titulos, propiedades, tipos, id, ordenarPor, botones, subgrilla, pager) {
    AbmcGrilla.accionListar = $.getUrl("/" + entidad + "/" + AbmcGrilla.accionListar);

    if (!pager) { pager = "#pager" }
    if (!subgrilla) { subgrilla = false; }

    var grid = $(idGrilla).jqGrid({
        url: AbmcGrilla.accionListar,
        datatype: "local",
        mtype: "GET",
        colNames: titulos,
        colModel: GrillaUtil.crearColumnas(propiedades, tipos, id),
        pager: pager,
        toppager: true,
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        sortname: ordenarPor,
        sortorder: "asc",
        viewrecords: true,
        autowidth: true,
        width: document.body.offsetWidth - 650,
        height: "100%",
        emptyrecords: "",
        subGrid: subgrilla
    });

    // Agrego propiedades que almacenan informacion sobre la grilla
    grid.id = idGrilla;
    grid.pager = pager;
    grid.botones = botones;

    // Agrego funcionalidad util para manejar la grilla
    AbmcGrilla.agregarComportamiento(grid);

    // Creo los botones de la barra de herramientas
    grid.crearBotones();

    // Agrego div para mostrar mensajes personalizado
    var divMensajes = "#gview_" + grid.id_limpio;
    $(divMensajes).append("<div id='" + grid.id_limpio + "_sinConsulta' class='ui-mensaje'>Aún no se ha realizado ninguna búsqueda</div>");
    $(divMensajes).append("<div id='" + grid.id_limpio + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados con los datos de búsqueda ingresados</div>");

    // Limpio los datos de la consulta para el primer ingreso y oculto los botones que no sean necesarios
    grid.setGridParam({ loadComplete: function (data) { GrillaUtil.mostrarBotones(grid); } }).trigger("reloadGrid");

    grid.recientes = AbmcGrilla.Recientes.init(grid.id + "Recientes", titulos, propiedades, tipos, id);
    return grid;
};

AbmcGrilla.agregarComportamiento = function (grid) {
    grid.id_limpio = grid.id.replace("#", "");

    // Permite agregar filtros de consulta
    grid.agregarParametros = function (parametros) {
        $(grid.id).setGridParam({ url: AbmcGrilla.accionListar + parametros });
    };

    // Actualizar el contenido de la grilla
    grid.actualizar = function () {
        GrillaUtil.actualizar(grid);
    };

    // Limpiar los filtros de busqueda y el contenido de la grilla
    grid.limpiar = function () {
        GrillaUtil.limpiar(grid, AbmcGrilla.accionListar);
    };

    // Crea los botones segun el array seteado en el objeto
    grid.crearBotones = function () {
        AbmcGrilla.crearBotones(grid);
    };
};

AbmcGrilla.crearBotones = function (grid) {
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
                AbmcGrilla.crearBotonSeleccion(grid, grid.botones[i], function () {
                    Abmc.cargarModelo(AbmcGrilla.accionRegistrar, null); 
                });
                break;

            case "Historial":
                AbmcGrilla.crearBotonSeleccion(grid, grid.botones[i], function () {
                    var seleccionado = GrillaUtil.getSeleccionFilas(grid, false);
                    if (seleccionado) {
                        Abmc.mostrarHistorial(seleccionado);
                    }
                    else {
                        AbmcUtil.mensajeSeleccion();
                    }
                });
                break;

            case "Imprimir":
                AbmcGrilla.crearBotonImpresion(grid);
                break;

            default:
                AbmcGrilla.crearBotonSeleccion(grid, grid.botones[i]);
                break;
        }
    }
};

AbmcGrilla.crearBotonSeleccion = function (grid, accion, onClick) {
	GrillaUtil.crearBotonSeleccion(grid, accion, AbmcGrilla.iconos[accion], function() {
		if (onClick) {
			onClick();
        }
        else {
			var seleccionado = GrillaUtil.getSeleccionFilas(grid, false);
            if (seleccionado) {
            	Abmc.cargarModelo(AbmcGrilla["accion" + accion], seleccionado);
            }
            else {
                AbmcUtil.mensajeSeleccion();
            }
        }
	});
};

AbmcGrilla.crearBotonImpresion = function (grid) {
    GrillaUtil.crearBotonSeleccion(grid, "Imprimir", "print", function () {
        var filtros = [];
        $(Abmc.divConsulta + " fieldset p").not("p.botones").each(function (ind, val) {
            var campo = $(val).children(":input:first");
            var valor = $(campo).attr("type") == "checkbox" ? $(campo).is(":checked") : $(campo).val();

            if (valor) {
                var filtro = {
                    key: $(val).children("label").html(),
                    value: valor
                };
                filtros.push(filtro);
            }
        });

        var filas = grid.getRowData();
        var matriz = [];
        $.each(filas, function (ind, val) {
            var fila = [];
            $.each(val, function (keyProp, valueProp) {
                if (val.hasOwnProperty(keyProp)) {
                    fila.push(valueProp);
                }
            });
            matriz.push(fila);
        });

        var titulos = grid.getGridParam("colNames");

        var datos = [
            { name: "TituloPagina", value: $("#areaCuerpo h2:first").html().trim() },
            { name: "TotalResultados", value: $("#pager_left .ui-paging-info").html() }
        ];

        $.formatoModelBinder(filtros, datos, "Filtros");
        $.formatoModelBinder(titulos, datos, "Titulos");
        $.formatoModelBinder(matriz, datos, "Datos");

        $.post($.getUrl('/' + Abmc.controller + '/ImprimirData'), datos, function (data) {
            window.open($.getUrl('/' + Abmc.controller + '/Imprimir'), '_blank');
        });
    });
};

/******************************************* MUESTRA LOS REGISTROS RECIENTES ***********************************************/

AbmcGrilla.Recientes = {};

AbmcGrilla.Recientes.init = function (idGrilla, titulos, propiedades, tipos, id) {

    var grid = $(idGrilla).jqGrid({
        datatype: "local",
        colNames: titulos,
        colModel: GrillaUtil.crearColumnas(propiedades, tipos, id),
        pager: "#pagerRecientes",
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: document.body.offsetWidth - 650,
        height: "100%",
        emptyrecords: "",
        caption: "Registros recientes"
    });

    var divMensajes = "#gview_" + idGrilla.replace("#", "");
    $(divMensajes).append("<div id='sinRegistrosAbmc' class='ui-mensaje'>No hay registros recientes</div>");

    // Agrego funcionalidad util para manejar la grilla
    grid.limpiar = function () {
        grid.clearGridData();
        $(divMensajes + " .ui-jqgrid-hbox, #pagerRecientes").hide();
        $("#sinRegistrosAbmc").show();
    };

    grid.agregarFila = function (fila) {
        if ($("#sinRegistrosAbmc").is(":visible")) {
            $(divMensajes + " .ui-jqgrid-hbox, #pagerRecientes").show();
            $("#sinRegistrosAbmc").hide();
        }

        grid.addRowData(fila.Id, fila, "first");
    };

    return grid;
};
