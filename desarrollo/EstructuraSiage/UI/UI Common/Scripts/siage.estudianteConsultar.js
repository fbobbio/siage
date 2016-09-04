var EstudianteConsultar = {};

EstudianteConsultar.init = function (seleccionFila) {
    var grilla = $("#listEstudiantes").jqGrid({
        datatype: "local",
        colNames: ['Id', 'Nombre', 'Apellido', 'Sexo', 'Número Documento', 'Sexo_Id', 'Persona_Id', 'Tipo Documento', 'Fecha Nacimiento'],
        colModel: [
                    { name: 'Id', index: 'Id', hidden: true },
                    { name: 'Persona_Nombre', index: 'Persona.Nombre', hidden: false },
                    { name: 'Persona_Apellido', index: 'Persona.Apellido', hidden: false },
                    { name: 'Persona_SexoNombre', index: 'Persona.SexoNombre', hidden: false },
                    { name: 'Persona_NumeroDocumento', index: 'Persona.NumeroDocumento', hidden: false },
                    { name: 'Persona_Sexo', index: 'Persona.Sexo', hidden: true },
                    { name: 'Persona_Id', index: 'Persona.Id', hidden: true },
                    { name: 'Persona_TipoDocumento', index: 'Persona.TipoDocumento', hidden: true },
                    { name: 'Persona_FechaNacimiento', index: 'Persona.FechaNacimiento', hidden: true }
                ],
        sortname: "Persona.Nombre",
        sortorder: "asc",
        pager: "#pagerEstudiantes",
        toppager: true,
        rowNum: 30,
        viewrecords: true,
        autowidth: true,
        height: "100%",
        caption: "Estudiantes",
        loadtext: 'Cargando, espere por favor',
        emptyrecords: 'No hay datos para mostrar'
    });

    grilla.pager = "#pagerEstudiantes";
    grilla.id = "#listEstudiantes";
    grilla.id_limpio = "listEstudiantes";
    grilla.botones = ["Seleccionar"];

    $(grilla.id + "_toppager_center", grilla.id + "_toppager").remove();
    $(".ui-paging-info", grilla.id + "_toppager").remove();
    $(grilla.id + "_toppager_left").append('<table cellspacing="0" cellpadding="0" border="0" style="float: left; table-layout: auto;" class="ui-pg-table navtable"><tbody><tr></tr></tbody></table>');

    GrillaUtil.crearBotonSeleccion(grilla, "Seleccionar", "pin-s",
        function () {
            grilla.seleccion = GrillaUtil.getSeleccionFilas(grilla, false);
            if (grilla.seleccion) {
                $("#divVistaEstudiantes").show();
                $("#divConsultarEstudiante").hide();

                var fila = grilla.getRowData(grilla.seleccion);
                $.formatoFormulario(fila, "VerEstudiante_");

                $.get($.getUrl("/Estudiante/CalcularEdad") + "&fecha=" + fila.Persona_FechaNacimiento, function (data) {
                    if (data) {
                        $("#Edad").val(data);
                    }
                });

                if (seleccionFila) {
                    seleccionFila(fila.Id);
                }
            }
            else {
                AbmcUtil.mensajeSeleccion();
            }
        });

    var divMensajes = "#gview_" + grilla.id_limpio;
    $(divMensajes).append("<div id='" + grilla.id_limpio + "_sinConsulta' class='ui-mensaje'>Aún no se ha realizado ninguna búsqueda</div>");
    $(divMensajes).append("<div id='" + grilla.id_limpio + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados con los datos de búsqueda ingresados</div>");

    grilla.setGridParam({ loadComplete: function (data) { GrillaUtil.mostrarBotones(grilla); } }).trigger("reloadGrid");

    EstudianteConsultar.inicializarBotones();
}


EstudianteConsultar.inicializarBotones = function () {
    $("#btnCambiarEstudiante").click(function () {
        $("#divVistaEstudiantes").hide();
        $("#divConsultarEstudiante").show();
    });

    $("#btnConsultarEstudiante").click(EstudianteConsultar.ConsultarEstudiantes);
    $("#btnLimpiarEstudiante").click(EstudianteConsultar.LimpiarBusqueda);

    $("#radioFiltro1").click(function () {
        $("#divFiltro1").show();
        $("#divFiltro2").hide();
    });

    $("#radioFiltro2").click(function () {
        $("#divFiltro1").hide();
        $("#divFiltro2").show();
    });
};


EstudianteConsultar.ConsultarEstudiantes = function () {
    if (($("#FiltroDniEstudianteConsultar").val() == "" && $("#FiltroSexoEstudianteConsultar").val() == "" && $("#FiltroNombreEstudianteConsultar").val() == "" && $("#FiltroApellidoEstudianteConsultar").val() == "") ||
                ($("#FiltroDniEstudianteConsultar").val() != "" && $("#FiltroSexoEstudianteConsultar").val() == "") || ($("#FiltroDniEstudianteConsultar").val() == "" && $("#FiltroSexoEstudianteConsultar").val() != "") ||
                ($("#FiltroNombreEstudianteConsultar").val() != "" && $("#FiltroApellidoEstudianteConsultar").val() == "") || ($("#FiltroNombreEstudianteConsultar").val() == "" && $("#FiltroApellidoEstudianteConsultar").val() != "")) {
        
        Mensaje.Error.texto = "Ingrese alguna de las combinaciones completas.";
        Mensaje.Error.mostrar();
    }
    else {
        var url = $.getUrl("/Estudiante/ProcesarBusqueda");

        if ($("#radioFiltro1").is(":checked")) {
            url += "&filtroSexo=" + $("#FiltroSexoEstudianteConsultar").val() + "&filtroDni=" + $("#FiltroDniEstudianteConsultar").val();
        }
        else {
            url += "&filtroNombre=" + $("#FiltroNombreEstudianteConsultar").val() + "&filtroApellido=" + $("#FiltroApellidoEstudianteConsultar").val();
        }

        $("#listEstudiantes").setGridParam({ url: url, datatype: 'json' }).trigger('reloadGrid');
    }
}


EstudianteConsultar.LimpiarBusqueda = function () {
    $("#FiltroDniEstudianteConsultar").val("");
    $("#FiltroSexoEstudianteConsultar").val("");
    $("#FiltroNombreEstudianteConsultar").val("");
    $("#FiltroApellidoEstudianteConsultar").val("");

    GrillaUtil.limpiar($("#listEstudiantes"), "");
}

EstudianteConsultar.postEnviarModelo = Abmc.postEnviarModelo;
Abmc.postEnviarModelo = function (data) {
    if (EstudianteConsultar.postEnviarModelo != null) {
        EstudianteConsultar.postEnviarModelo(data);
    }

    $("#divVistaEstudiantes input").attr('disabled', 'disabled');
    $("#radioFiltro1").attr('checked', 'checked');
};
