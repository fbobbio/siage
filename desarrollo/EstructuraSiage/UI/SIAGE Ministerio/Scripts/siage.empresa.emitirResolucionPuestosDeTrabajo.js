var EmpresaResolucionPuestosDeTrabajo = {};

EmpresaResolucionPuestosDeTrabajo.init = function (instanciaConsultar, prefijo, div) {
    var instancia = {};

    if (div) {
        instancia.div = div;
    }

    EmpresaResolucionPuestosDeTrabajo.initEditores(instancia, prefijo);

    if (prefijo) {
        $(div).agregarPrefijo(prefijo);
        instancia.prefijo = "#" + prefijo + "_";
    }
    else {
        instancia.prefijo = "#";
    }

    //Inicializo los datepicker y máscaras para las fechas
    $(".val-DateTime").mask("99/99/9999", { placeholder: " " });
    $(".val-DateTime").datepicker({
        currentText: 'Now',
        dateFormat: 'dd/mm/yy',
        changeYear: true,
        yearRange: (new Date().getFullYear() - 80) + ":" + (new Date().getFullYear() + 80)
    });

    EmpresaResolucionPuestosDeTrabajo.manejoDeEventos(instancia);
    EmpresaResolucionPuestosDeTrabajo.initGrillaArticulos(instancia);
    EmpresaResolucionPuestosDeTrabajo.eventoClickBotonAceptarCancelar(instanciaConsultar, instancia);

    return instancia;
};

//Inicializacion de editores
EmpresaResolucionPuestosDeTrabajo.initEditores = function (instancia, prefijo) {
    var prefijoIL = prefijo ? prefijo + "_InstrumentoLegalResolucion" : "InstrumentoLegalResolucion";
    instancia.instanciaAsignacionInstrumentoLegal = AsignacionInstrumentoLegal.init("#divInstrumentoLegalEmitirResolucion", prefijoIL);
};

//Manejo de eventos generales
EmpresaResolucionPuestosDeTrabajo.manejoDeEventos = function (instancia) {
    $(instancia.prefijo + "Visto, "
        + instancia.prefijo + "Considerando, "
        + instancia.prefijo + "Protocolicese, "
        + instancia.prefijo + "ObservacionesResolucion").attr("MaxLength", 50);

    $(instancia.prefijo + "DescArticulo").attr("MaxLength", 200);

    EmpresaResolucionPuestosDeTrabajo.navegabilidadInstrumentoLegal(instancia);

    //    $(instancia.instanciaAsignacionInstrumentoLegal.prefijo + "btnRegistrarNuevo").click(function () {
    //        $("#btnAceptarResolucion").show();
    //        $("#divRegistracionInstrumentoLegal").show();
    //        $("#divBuscarInstrumentoLegalPorNumero").hide();
    //        $("#divResolucion").show();
    //    });
};

//Agrego funcionalidad de los botones de instrumento legal
EmpresaResolucionPuestosDeTrabajo.navegabilidadInstrumentoLegal = function (instancia) {
    $(instancia.instanciaAsignacionInstrumentoLegal.prefijo + "InstrumentoLegal_btnNuevo").click(function () {
        $(instancia.prefijo + "divResolucion").show();
    });

    $(instancia.instanciaAsignacionInstrumentoLegal.prefijo + "InstrumentoLegal_btnBusqueda").click(function () {
        $(instancia.prefijo + "divResolucion").hide();
        $(instancia.prefijo + "divResolucion :input").val("");
        $(instancia.prefijo + "btnAgregar").val("Agregar");
        $(instancia.prefijo + "btnEliminar").val("Eliminar");
        $(instancia.prefijo + "listArticulos").clearGridData();
    });
};

//Inicializacion de la grilla de articulos y evento de los botones Agregar Eliminar
EmpresaResolucionPuestosDeTrabajo.initGrillaArticulos = function (instancia) {
    var grid = $(instancia.prefijo + "listArticulos").jqGrid({
        datatype: "local",
        colNames: ["Número", "Detalle"],
        colModel: [
                    { key: true, name: "numero", index: "numero", align: "left" },
                    { key: false, name: "detalle", index: "detalle", align: "left" }
                ],
        rowNum: 10,
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: document.body.offsetWidth - 650,
        height: "100%"
    });

    $(instancia.prefijo + "btnAgregar").click(function () {
        var art = {
            numero: grid.getGridParam("reccount") + 1,
            detalle: $(instancia.prefijo + "DescArticulo").val()
        };
        var articulos = grid.getRowData();
        if (articulos.length == 5) {
            alert("Solo se permiten agregar hasta 5 articulos.");
            $(instancia.prefijo + "DescArticulo").val("");
            return;
        }
        if ($(instancia.prefijo + "DescArticulo").val() != "") {
            var data = grid.getRowData();
            for (i = 0; i < data.length; i++) {
                if (data[i].detalle === $(instancia.prefijo + "DescArticulo").val()) {
                    $(instancia.prefijo + "DescArticulo").val("");
                    alert("Artículo existente");
                    return;
                }
            }
            grid.addRowData(art.numero, art, "last");
            grid.show();
            $(instancia.prefijo + "DescArticulo").val("");
        }
    });

    $(instancia.prefijo + "btnEliminar").click(function () {
        var seleccion = GrillaUtil.getSeleccionFilas(grid, false);
        if (seleccion && seleccion.lenght !== 0) {
            grid.delRowData(seleccion);

            if (grid.getGridParam("reccount") === 0) {
                grid.hide();
            }
            else {
                var data = grid.getRowData();
                var json = {};
                json.total = json.page = 1;
                json.records = data.length;
                json.rows = [];

                grid.clearGridData();
                for (i = 0; i < data.length; i++) {
                    data[i].numero = i + 1;
                    grid.addRowData(data[i].numero, data[i], "last");
                }
            }
        }
        else {
            AbmcUtil.mensajeSeleccion();
        }
    });
};

//Evento click de los botones aceptar y cancelar resolucion
EmpresaResolucionPuestosDeTrabajo.eventoClickBotonAceptarCancelar = function (instanciaConsultar, instancia) {
    $(instancia.prefijo + "btnCancelarResolucion").click(function () {
        $("#divAbmc").hide();
        $(instanciaConsultar.prefijo + "divDatosGeneralesEmpresa").hide();
        $(instanciaConsultar.prefijo + "divFiltrosDeBusqueda").show();
    });

    $(instancia.prefijo + "btnAceptarResolucion").click(function () {
        Validacion.validar();
        $("#divAbmc :input").removeAttr("disabled");
        var arreglo = $("#divAbmc :input, #divAbmc textarea").serializeArray();
        $("#divAbmc :input").attr("disabled", "disabled");

        arreglo.push({
            name: "EmpresaModelId",
            value: GrillaUtil.getSeleccionFilas($(instanciaConsultar.prefijo + "list"), false)
        });
        var articulos = $(instancia.prefijo + "listArticulos").getRowData();
        for (var i = 0; i < articulos.length; i++) {
            var art = articulos[i];
            articulos[i] = {};
            articulos[i].Numero = art.numero;
            articulos[i].Detalle = art.detalle;
        }
        $.formatoModelBinder(articulos, arreglo, "ArticulosResolucion");

        $.post($.getUrl("/GestionEmpresa/EmitirResolucionEmpresa"), arreglo,
                function (data) {
                    if (data.status) {
                        $(instancia.prefijo + "btnAceptarResolucion").hide();
                        $("#divAbmc :input").attr("disabled", "disabled");
                        $(instancia.prefijo + "btnCancelarResolucion").val("Volver");
                        $(instancia.prefijo + "btnCancelarResolucion").removeAttr("disabled");
                        Mensaje.Exito.mostrar();
                    }
                    else {
                        $("#divAbmc :input").removeAttr("disabled");
                        $(instancia.prefijo + "divInstrumentoLegalEmitirResolucion").attr("disabled", "disabled");
                        for (var i = 0; i < data.details.length; i++) {
                            Mensaje.Error.agregarError(data.details[i]);
                        }
                    }
                }, "json");
    });
};