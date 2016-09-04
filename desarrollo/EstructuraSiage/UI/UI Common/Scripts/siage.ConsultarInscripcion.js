var ConsultarInscripcion = {};
ConsultarInscripcion.grillaInscripciones = null;
ConsultarInscripcion.UrlGeneral = null;

ConsultarInscripcion.init = function (prefijo, seleccionFila) {
    ConsultarInscripcion.prefijo = prefijo;

    var ConsultarEmpresaEditor = ConsultarEmpresa.init('SinVista', "#divConsultaEmpresaInscripcion", "ConsultarInscripcion", false);
    ConsultarInscripcion.SeleccionarEmpresa = ConsultarEmpresa.seleccionarEmpresa;

    $("#ConsultarInscripcion_list_toppager_left td[title='Seleccionar']").unbind("click");
        $("#ConsultarInscripcion_list_toppager_left td[title='Seleccionar']").click(function () {
        var instancia = ConsultarEmpresa.inicializarVariables();
        instancia.vista = 'SinVista';
        instancia.prefijo = "#ConsultarInscripcion_";
        instancia.seleccionMultiple = false;
        instancia.seleccion = GrillaUtil.getSeleccionFilas($("#ConsultarInscripcion_list"), instancia.seleccionMultiple);
        instancia.grilla = $("#ConsultarInscripcion_list");

        if (instancia.seleccion) {
            Mensaje.ocultar();
            var empresa = $("#ConsultarInscripcion_list").getRowData(instancia.seleccion);
            if (empresa.TipoEmpresa == "ESCUELA_MADRE" || empresa.TipoEmpresa == "ESCUELA_ANEXO") {
                ConsultarEmpresa.seleccionarEmpresa(instancia);
                $("#divBusquedaInscripciones").show();
                $("#divGrillaInscripciones").show();
                $.getJSON($.getUrl("/Inscripcion/GetGradoAñoByEscuela") + "&idEmpresa=" + empresa.Id,
                function (data) {
                    $('#' + ConsultarInscripcion.prefijo + '_FiltroGradoAnioConsultarInscripcion').cargarCombo(data, "Value", "Text");
                });
            }
            else {
                Mensaje.Error.texto = "Debe seleccionar una empresa de tipo ESCUELA MADRE o ESCUELA ANEXO.";
                Mensaje.Error.mostrar();
            }
        }
        else {
            AbmcUtil.mensajeSeleccion();
        }
    });

    $("#divBusquedaInscripciones").agregarPrefijo(ConsultarInscripcion.prefijo);
    $("#divBusquedaInscripciones, #divGrillaInscripciones, #divDatosInscripcion").hide();
    $("#" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionDesdeConsultarInscripcion, #" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionHastaConsultarInscripcion").mask("99/99/9999", { placeholder: " " });
    $("#" + ConsultarInscripcion.prefijo + "_FiltroDivisionConsultarInscripcion, #" + ConsultarInscripcion.prefijo + "_FiltroTurnoConsultarInscripcion, #" + ConsultarInscripcion.prefijo + "_FiltroEspecialidadConsultarInscripcion").attr("disabled", "disabled");

    ConsultarInscripcion.InicializarGrillaInscripciones(seleccionFila);
    ConsultarInscripcion.InicializarBotones();

    $("#divBusquedaInscripciones .val-DateTime").each(function (input) {
        var data = $(this).data("datepicker");

        if (data) {
            data.id = this.id;
            $(this).data("datepicker", data);
        }
    });

    $("#" + ConsultarInscripcion.prefijo + "_FiltroTurnoConsultarInscripcion").CascadingDropDown("#" + ConsultarInscripcion.prefijo + "_FiltroGradoAnioConsultarInscripcion", $.getUrl('/Inscripcion/GetTurnosByGradoAnio'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idGradoAnio: $("#" + ConsultarInscripcion.prefijo + "_FiltroGradoAnioConsultarInscripcion").val(), filtroEscuela: $("#" + ConsultarInscripcion.prefijo + "_Id").val() };
        }
    });

    $("#" + ConsultarInscripcion.prefijo + "_FiltroEspecialidadConsultarInscripcion").CascadingDropDown("#" + ConsultarInscripcion.prefijo + "_FiltroGradoAnioConsultarInscripcion", $.getUrl('/Inscripcion/GetEspecialidadesEscuelaAsociadasAGradoAnio'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idGradoAnio: $("#" + ConsultarInscripcion.prefijo + "_FiltroGradoAnioConsultarInscripcion").val(), filtroEscuela: $("#" + ConsultarInscripcion.prefijo + "_Id").val() };
        }
    });

    $("#" + ConsultarInscripcion.prefijo + "_FiltroDivisionConsultarInscripcion").CascadingDropDown("#" + ConsultarInscripcion.prefijo + "_FiltroTurnoConsultarInscripcion", $.getUrl('/Inscripcion/GetDivisionesEscuelaByGradoAnioTurno'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idGradoAnio: $("#" + ConsultarInscripcion.prefijo + "_FiltroGradoAnioConsultarInscripcion").val(), idTurno: $("#" + ConsultarInscripcion.prefijo + "_FiltroTurnoConsultarInscripcion").val(), filtroEscuela: $("#" + ConsultarInscripcion.prefijo + "_Id").val() };
        }
    });

    $("#" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionDesdeConsultarInscripcion").changePatch(function () {
        var desde = $("#" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionDesdeConsultarInscripcion").val();
        var hasta = $("#" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionHastaConsultarInscripcion").val();
        if (hasta != "" && desde > hasta) {
            Mensaje.Error.texto = "La fecha desde no puede ser mayor a la fecha hasta. Ingrese nuevamente la fecha desde.";
            Mensaje.Error.mostrar();
            $("#" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionDesdeConsultarInscripcion").val('');
        }
    });

    $("#" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionHastaConsultarInscripcion").changePatch(function () {
        var desde = $("#" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionDesdeConsultarInscripcion").val();
        var hasta = $("#" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionHastaConsultarInscripcion").val();
        if (hasta != "" && desde > hasta) {
            Mensaje.Error.texto = "La fecha hasta no puede ser mayor a la fecha desde. Ingrese nuevamente la fecha hasta.";
            Mensaje.Error.mostrar();
            $("#" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionHastaConsultarInscripcion").val('');
        }
    });
};

ConsultarInscripcion.InicializarGrillaInscripciones = function (seleccionFila) {
    ConsultarInscripcion.grillaInscripciones = $("#listInscripciones").jqGrid({
        datatype: "local",
        colNames: ['Id', 'Turno', 'Grado Año', 'GradoAño_Id', 'División', 'IdEstudiante', 'Tipo Documento', 'Número Documento', 'Sexo', 'Nombre', 'Apellido', 'Fecha Nacimiento'],
        colModel: [
                        { name: 'Id', index: 'Id', hidden: true },
                        { name: 'Turno', index: 'Turno', hidden: false },
                        { name: 'GradoAnio', index: 'GradoAnio', hidden: false },
                        { name: 'GradoAnioId', index: 'GradoAnioId', hidden: true },
                        { name: 'Division', index: 'Division', hidden: false },
                        { name: 'IdEstudiante', index: 'IdEstudiante', hidden: true },
                        { name: 'Persona_TipoDocumento', index: 'Persona_TipoDocumento', hidden: false },
                        { name: 'Persona_NumeroDocumento', index: 'Persona_NumeroDocumento', hidden: true },
                        { name: 'Persona_SexoNombre', index: 'Persona_SexoNombre', hidden: false },
                        { name: 'Persona_Nombre', index: 'Persona_Nombre', hidden: false },
                        { name: 'Persona_Apellido', index: 'Persona_Apellido', hidden: false },
                        { name: 'FechaNacimiento', index: 'FechaNacimiento', hidden: true }
                    ],
        sortname: "VerInscripcion_Persona_NumeroDocumento",
        sortorder: "asc",
        pager: "#pagerInscripciones",
        toppager: true,
        rowNum: 30,
        viewrecords: true,
        autowidth: true,
        height: "100%",
        caption: "Inscripciones",
        loadtext: 'Cargando, espere por favor',
        emptyrecords: 'No hay datos para mostrar'
    });

    ConsultarInscripcion.grillaInscripciones.pager = "#pagerInscripciones";
    ConsultarInscripcion.grillaInscripciones.id = "#listInscripciones";
    ConsultarInscripcion.grillaInscripciones.id_limpio = "listInscripciones";
    ConsultarInscripcion.grillaInscripciones.botones = ["Seleccionar"];

    $(ConsultarInscripcion.grillaInscripciones.id + "_toppager_center", ConsultarInscripcion.grillaInscripciones.id + "_toppager").remove();
    $(".ui-paging-info", ConsultarInscripcion.grillaInscripciones.id + "_toppager").remove();
    $(ConsultarInscripcion.grillaInscripciones.id + "_toppager_left").append('<table cellspacing="0" cellpadding="0" border="0" style="float: left; table-layout: auto;" class="ui-pg-table navtable"><tbody><tr></tr></tbody></table>');

    GrillaUtil.crearBotonSeleccion(ConsultarInscripcion.grillaInscripciones, "Seleccionar", "pin-s",
        function () {
            ConsultarInscripcion.grillaInscripciones.seleccion = GrillaUtil.getSeleccionFilas(ConsultarInscripcion.grillaInscripciones, false);
            if (ConsultarInscripcion.grillaInscripciones.seleccion) {
                $("#divDatosInscripcion").show();
                $("#divBusquedaInscripciones, #divGrillaInscripciones, #" + ConsultarInscripcion.prefijo + "_btnVolver").hide();

                var fila = ConsultarInscripcion.grillaInscripciones.getRowData(ConsultarInscripcion.grillaInscripciones.seleccion);
                $.formatoFormulario(fila, "VerInscripcion_");

                if (seleccionFila) {
                    seleccionFila(ConsultarInscripcion.grillaInscripciones.seleccion);
                }
            }
            else {
                AbmcUtil.mensajeSeleccion();
            }
        });

    var divMensajes = "#gview_" + ConsultarInscripcion.grillaInscripciones.id_limpio;
    $(divMensajes).append("<div id='" + ConsultarInscripcion.grillaInscripciones.id_limpio + "_sinConsulta' class='ui-mensaje'>Aún no se ha realizado ninguna búsqueda</div>");
    $(divMensajes).append("<div id='" + ConsultarInscripcion.grillaInscripciones.id_limpio + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados con los datos de búsqueda ingresados</div>");

    ConsultarInscripcion.grillaInscripciones.setGridParam({ gridComplete: function (data) { GrillaUtil.mostrarBotones(ConsultarInscripcion.grillaInscripciones); } });
    ConsultarInscripcion.grillaInscripciones.trigger("reloadGrid");
};

ConsultarInscripcion.InicializarBotones = function () {
    $("#" + ConsultarInscripcion.prefijo + "_btnConsultarInscripcion").click(function () {
        Mensaje.ocultar();
        if ($("#" + ConsultarInscripcion.prefijo + "_FiltroNroDocumentoEstudianteConsultarInscripcion").val() != "" && $("#" + ConsultarInscripcion.prefijo + "_FiltroSexoEstudianteConsultarInscripcion").val() == "") {
            Mensaje.Error.texto = "Debe seleccionar sexo.";
            Mensaje.Error.mostrar();
            return;
        }
        if ($("#" + ConsultarInscripcion.prefijo + "_FiltroNroDocumentoEstudianteConsultarInscripcion").val() == "" && $("#" + ConsultarInscripcion.prefijo + "_FiltroSexoEstudianteConsultarInscripcion").val() != "") {
            Mensaje.Error.texto = "Debe ingresar un número de documento.";
            Mensaje.Error.mostrar();
            return;
        }
        
        var parametros = "&filtroNroDocumentoEstudiante=" + $("#" + ConsultarInscripcion.prefijo + "_FiltroNroDocumentoEstudianteConsultarInscripcion").val() + "&filtroSexoEstudiante=" + $("#" + ConsultarInscripcion.prefijo + "_FiltroSexoEstudianteConsultarInscripcion").val()
                  + "&filtroGradoAnio=" + $("#" + ConsultarInscripcion.prefijo + "_FiltroGradoAnioConsultarInscripcion").val() + "&filtroTurno=" + $("#" + ConsultarInscripcion.prefijo + "_FiltroTurnoConsultarInscripcion").val()
                   + "&filtroDivision=" + $("#" + ConsultarInscripcion.prefijo + "_FiltroDivisionConsultarInscripcion").val() + "&filtroEspecialidad=" + $("#" + ConsultarInscripcion.prefijo + "_FiltroEspecialidadConsultarInscripcion").val()
                    + "&filtroPeriodoInscripcionDesde=" + $("#" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionDesdeConsultarInscripcion").val() + "&filtroPeriodoInscripcionHasta=" + $("#" + ConsultarInscripcion.prefijo + "_FiltroPeriodoInscripcionHastaConsultarInscripcion").val()
                    + "&filtroCicloLectivo=" + $("#" + ConsultarInscripcion.prefijo + "_FiltroCicloLectivoConsultarInscripcion").val() + "&filtroEscuela=" + $("#" + ConsultarInscripcion.prefijo + "_Id").val();
        ConsultarInscripcion.UrlGeneral = $.getUrl("/Inscripcion/ProcesarBusqueda") + parametros;
        $(ConsultarInscripcion.grillaInscripciones.id).setGridParam({ url: ConsultarInscripcion.UrlGeneral });
        GrillaUtil.actualizar(ConsultarInscripcion.grillaInscripciones);

    });


    $("#btnCambiarInscripcion").click(function () {
        $("#divDatosInscripcion").hide();
        $("#divBusquedaInscripciones, #divGrillaInscripciones, #" + ConsultarInscripcion.prefijo + "_btnVolver").show();
    });

    $("#" + ConsultarInscripcion.prefijo + "_btnVolver").click(function () {
        var cargando = $("load_" + ConsultarInscripcion.grillaInscripciones.id_limpio).is(":visible");
        if (!cargando) {
            ConsultarInscripcion.ComportamientoGenericoBotones();
            $("#divBusquedaInscripciones, #divGrillaInscripciones").hide();
        }
    });

    $("#" + ConsultarInscripcion.prefijo + "_btnLimpiarConsultarInscripcion").click(function () {
        var cargando = $("load_" + ConsultarInscripcion.grillaInscripciones.id_limpio).is(":visible");
        if (!cargando) {
            ConsultarInscripcion.ComportamientoGenericoBotones();
        }
    });
};

ConsultarInscripcion.ComportamientoGenericoBotones = function () {
    AbmcUtil.limpiarFormulario("#" + ConsultarInscripcion.prefijo + "_divFiltrosInscripcion");
    $("#" + ConsultarInscripcion.prefijo + "_FiltroDivisionConsultarInscripcion, #" + ConsultarInscripcion.prefijo + "_FiltroTurnoConsultarInscripcion, #" + ConsultarInscripcion.prefijo + "_FiltroEspecialidadConsultarInscripcion").attr("disabled", "disabled");
    ConsultarInscripcion.grillaInscripciones.setGridParam({ datatype: "local", url: ConsultarInscripcion.UrlGeneral, rowNum: 10, page: 1 });
    ConsultarInscripcion.grillaInscripciones.trigger("reloadGrid");
};