var Grilla = {};

/*********************************************** MUESTRA DATOS DE HISTORIAL O DETALLES ******************************************/

Grilla.Detalle = {};
Grilla.Detalle.init = function (idGrilla, titulos, propiedades, tipos, key, url, caption, id, pagerDetalle) {
    var grid = $(idGrilla).jqGrid({
        datatype: url ? "json" : "local",
        url: url + "?id=" + id,
        colNames: titulos,
        colModel: GrillaUtil.crearColumnas(propiedades, tipos, key),
        pager: pagerDetalle ? pagerDetalle : "#pagerDetalle",
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: document.body.offsetWidth - 650,
        height: "100%",
        emptyrecords: "",
        caption: caption
    });

    grid.id = idGrilla;
    return grid;
}

/************************************ PERMITE LA SELECCION DE REGISTROS DE LA GRILLA ********************************************/

Grilla.SeleccionMensajes = {};
Grilla.SeleccionMensajes.init = function (idGrilla, titulos, propiedades, tipos, key, url, caption, onSelect, selectMultiple, pager) {
    if (!pager) {
        pager = "#pagerSelect";
    }

    var grid = $(idGrilla).jqGrid({
        datatype: url ? "json" : "local",
        url: url,
        colNames: titulos,
        colModel: GrillaUtil.crearColumnas(propiedades, tipos, key),
        pager: pager,
        toppager: true,
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        multiselect: selectMultiple,
        width: document.body.offsetWidth - 650,
        height: "100%",
        emptyrecords: "",
        caption: caption
    });

    grid.id = idGrilla;
    grid.id_limpio = idGrilla.replace("#", "");
    grid.botones = ["Seleccionar"];
    grid.pager = pager;

    $(grid.id).navGrid(pager,
        { cloneToTop: true, view: false, add: false, edit: false, del: false, search: false, refresh: false }, // opciones
        {}, // editar registro
        {}, // insertar registro
        {}, // eliminar registro
        {}, // buscar registros (ventana modal)
        {}  // ver registro
    );

    $(grid.id).navButtonAdd(grid.id + "_toppager_left",
    {
        position: "first",
        caption: "Seleccionar",
        title: "Seleccionar",
        buttonicon: "ui-icon-pin-s",
        onClickButton: function () {
            var seleccion = GrillaUtil.getSeleccionFilas(grid, selectMultiple);
            if (seleccion && seleccion.lenght !== 0) {
                onSelect(seleccion);
            }
            else {
                AbmcUtil.mensajeSeleccion();
            }
        }
    });

    // Agrego div para mostrar mensajes personalizado
    var divMensajes = "#gview_" + grid.id_limpio;
    $(divMensajes).append("<div id='" + grid.id_limpio + "_sinConsulta' class='ui-mensaje'>Aún no se ha realizado ninguna búsqueda</div>");
    $(divMensajes).append("<div id='" + grid.id_limpio + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados con los datos de búsqueda ingresados</div>");

    // Limpio los datos de la consulta para el primer ingreso y oculto los botones que no sean necesarios
    grid.setGridParam({ loadComplete: function (data) { GrillaUtil.mostrarBotones(grid); } }).trigger("reloadGrid");
    console.log(grid);

    // Elimino la paginacion superior
    $(grid.id + "_toppager_center", grid.id + "_toppager").remove();
    $(".ui-paging-info", grid.id + "_toppager").remove();

    return grid;
};


Grilla.Seleccion = {};
Grilla.Seleccion.init = function (idGrilla, titulos, propiedades, tipos, key, url, caption, onSelect, selectMultiple, pager) {
    if(!pager) {
		pager = "#pagerSelect";
	}
	
	var grid = $(idGrilla).jqGrid({
	    datatype: url ? "json" : "local",
        url: url,
        colNames: titulos,
        colModel: GrillaUtil.crearColumnas(propiedades, tipos, key),
        pager: pager,
        toppager: true,
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        multiselect: selectMultiple,
        width: document.body.offsetWidth - 650,
        height: "100%",
        emptyrecords: "",
        caption: caption
    });

    grid.id = idGrilla;

    $(grid.id).navGrid(pager,
        { cloneToTop: true, view: false, add: false, edit: false, del: false, search: false, refresh: false }, // opciones
        {}, // editar registro
        {}, // insertar registro
        {}, // eliminar registro
        {}, // buscar registros (ventana modal)
        {}  // ver registro
    );

    $(grid.id).navButtonAdd(grid.id + "_toppager_left",
    {
        position: "first",
        caption: "Seleccionar",
        title: "Seleccionar",
        buttonicon: "ui-icon-pin-s",
        onClickButton: function () {
            var seleccion = GrillaUtil.getSeleccionFilas(grid, selectMultiple);
            if (seleccion && seleccion.lenght !== 0) {
                onSelect(seleccion);
            }
            else {
                AbmcUtil.mensajeSeleccion();
            }
        }
    });

    $(grid.id + "_toppager_center", grid.id + "_toppager").remove();
    $(".ui-paging-info", grid.id + "_toppager").remove();

    return grid;
};