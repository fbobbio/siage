var GrillaUtil = {};

GrillaUtil.crearBotonSeleccion = function (grid, accion, icono, onClick) {
    grid.navButtonAdd(grid.id + "_toppager_left",
    {
        position: "first",
        caption: accion,
        title: accion,
        buttonicon: "ui-icon-" + icono,
        onClickButton: onClick
    });
};

GrillaUtil.crearColumnas = function (propiedades, tipos, key) {
    var json = [];

    var i = 0;
    for (i; i <= propiedades.length - 1; i++) {
        var valor = propiedades[i];
        json[i] = {
            key: (valor === key),
            hidden: (valor === key),
            name: valor,
            index: valor,
            align: "left"
        };

        switch (tipos[i]) {
            case "integer":
                json[i].formatter = "integer";
                //json[i].searchoptions = { dataInit: function (a) { $(a).numeric(); } };
                break;

            case "number":
                json[i].formatter = "number";
                //json[i].searchoptions = { dataInit: function (a) { $(a).numeric(); } };
                break;

            case "year":
                json[i].formatter = "integer";
                json[i].formatoptions = { thousandsSeparator: "" };
                //json[i].searchoptions = { dataInit: function (a) { $(a).numeric(); } };
                break;

            case "date":
                json[i].formatter = "date";
                //json[i].searchoptions = { dataInit: function (a) { $(a).datepicker({ dateFormat: "dd/mm/yy" }); } };
                json[i].formatoptions = { srcformat: "d-m-Y", newformat: "d/m/Y" };
                break;

            case "checkbox":
                json[i].formatter = "checkbox";
                json[i].stype = "select";
                json[i].editoptions = { value: { 1: "Seleccione...", 2: "Si", 3: "No"} };
                break;
        }
    }
    return json;
};

GrillaUtil.setSeleccionMultiple = function (grid, habilitar) {
    grid.setGridParam({ multiselect: habilitar });
};

GrillaUtil.getSeleccionMultiple = function (grid) {
    return grid.getGridParam("multiselect");
};

GrillaUtil.filasSeleccionadas = function (grilla) {
    return $(grilla).getGridParam("selarrrow");
};

GrillaUtil.getSeleccionFilas = function (grid, multiple) {
    if (multiple) {
        var seleccion = $.unique(grid.getGridParam("selarrrow").sort());
        seleccion = seleccion.filter(function (value) {
            return (value != null && value != undefined);
        });

        return seleccion;
    }
    else {
        return grid.getGridParam("selrow");
    }
};

GrillaUtil.getFila = function (grid, id) {
    return grid.getRowData(id);
};

GrillaUtil.setUrl = function (grid, url) {
    grid.setGridParam({ url: url });
};

GrillaUtil.actualizar = function (grid) {
    grid.setGridParam({ datatype: "json" });
    grid.trigger("reloadGrid");
};

GrillaUtil.onSelectRow = function (grid, funcion) {
    var funcionAnterior = grid.getGridParam("onSelectRow");

    $(grid.id).jqGrid().setGridParam({
        onSelectRow: function (id) {
            funcionAnterior(id);
            funcion(id);
        }
    });
};

GrillaUtil.limpiar = function (grid, urlDefault) {
    grid.setGridParam({ datatype: "local", url: urlDefault, rowNum: 10, page: 1 });
    grid.trigger("reloadGrid");
    $("#gbox_list select").val(10);
};

GrillaUtil.mostrarBotones = function (grid) {
    if (!grid.botones) { return; }

    // la grilla trajo datos
    var records = grid.getGridParam("reccount");
    if (records > 0) {
        $("#gview_" + grid.id_limpio + " .ui-jqgrid-hbox, " + grid.pager).show();
        $(grid.id + "_sinRegistros, " + grid.id + "_sinConsulta").hide();

        var j = 0;
        for (j; j < grid.botones.length; j++) {
            $(grid.id + "_toppager_left td[title='" + grid.botones[j] + "']").show();
        }
    }
    // la grilla no trajo datos
    else {
        var page = grid.getGridParam('page');
        if (page > 1) {
            grid.setGridParam({ page: page - 1 });
            GrillaUtil.actualizar(grid);
        }
        else {
            // oculto la cabecera de la tabla
            $("#gview_" + grid.id_limpio + " .ui-jqgrid-hbox, " + grid.pager).hide();

            var type = grid.getGridParam("datatype");
            if (type === "local") {
                $(grid.id + "_sinConsulta").show();
                $(grid.id + "_sinRegistros").hide();
            }
            else {
                $(grid.id + "_sinRegistros").show();
                $(grid.id + "_sinConsulta").hide();
            }

            // oculto los botones
            var i = 0;
            for (i; i < grid.botones.length; i++) {
                $(grid.id + "_toppager_left td[title='" + grid.botones[i] + "']").hide();
            }
            $(grid.id + "_toppager_left td[title='Agregar']").show();
        }
    }
}