
var EmpresaRegistrar = {};

//EmpresaRegistrar.initInstrumentoLegal = function () {
//    $("#divBotonesBusquedaInstrumentoLegal").hide();
//    $("#divRegistracionInstrumentoLegal").hide();
//    $("#divExpedienteEditor").hide();



//    $("#btnBuscarInstrumento").click(function () {
//        $("#divBotonesBusquedaInstrumentoLegal").show();
//    });
//    $("#btnRegistrarNuevo").click(function () {
//      $("#divRegistracionInstrumentoLegal").hide();
//     });
//    $("#btnVolverBusquedaInstrumentoLegal").click(function () { });
//    $("#btnRegistrarNuevo").click(function () { });
//    $("#btnLimpiarCamposInstrumento").click(function () { });

//}

EmpresaRegistrar.cargarGrillaInstrumentosLegales = function () {
    var grid = $("#listInstrumentosLegales").jqGrid({
        datatype: 'json',
        url: $.getUrl("/GestionEmpresa/GetInstrumentosLegalesByEmpresaId/?empresaId=" + $("#Id").val()),

        colNames: ['Id', 'Número', 'Emisor', 'Fecha Notificacion', 'Expediente'],
        colModel: [
                            { name: 'Id', index: 'Id', width: 55, key: true, jsonmap: 'Id', hidden: true },
                            { name: 'Numero', index: 'Numero', width: "25%", sortable: false, jsonmap: 'Numero' },
                            { name: 'Emisor', index: 'Emisor', width: "25%", sortable: false, jsonmap: 'Emisor' },
                            { name: 'FechaNotificacion', index: 'FechaNotificacion', width: "25%", sortable: false, jsonmap: 'FechaNotificacion' },
                            { name: 'Expediente', index: 'Expediente', width: "25%", sortable: false, jsonmap: 'Expediente' }
                            ],
        rowNum: 10,
        pager:'paginadorInstrumentoLegal',
        sortname: 'invid',
        sortorder: 'desc',
        viewrecords: true,
        width: '100%',
        autowidth: true,
        height: "100%"

    });

    grid.setGridWidth(700,true);

}

EmpresaRegistrar.cargarGrillaPeriodoLectivo = function () {

    $("#listPeriodosLectivos").jqGrid({
        datatype: 'local',
        colNames: ['Id', 'Periodo Lectivo', 'PeriodoLectivoId'],
        colModel: [
                            { name: 'id', index: 'id', width: 55, key: true, jsonmap: 'id', hidden: true },
                            { name: 'PeriodoLectivoText', index: 'PeriodoLectivoText', width: 250, sortable: false, jsonmap: 'PeriodoLectivoText' },
                            { name: 'PeriodoLectivoId', index: 'PeriodoLectivoId', sortable: false, jsonmap: 'PeriodoLectivoId', hidden: true }
                            ],
        rowNum: 10,
        sortname: 'invid',
        sortorder: 'desc',
        viewrecords: true,
        width: '100%',
        height: "100%"

    });

    //funcionalidad al boton Agregar

    $("#btnAgregarPeriodoLectivo").click(function () {

        if ($('#PeriodoLectivoId option:selected').text() == "SELECCIONE") {
            Mensaje.Error.texto = 'Debe seleccionar un periodo lectivo';
            Mensaje.Error.mostrar();
            return;
        }

        var peridoLectivo = {
            Id: $("#listPeriodosLectivos").getGridParam("reccount") + 1,
            PeriodoLectivoId: $("#PeriodoLectivoId :selected").val(),
            PeriodoLectivoText: $("#PeriodoLectivoId :selected").text()
        };

        //validar que el estado no este agregado
        if ($("#PeriodoLectivoId").val() != "") {
            var data = $("#listPeriodosLectivos").getRowData();

            for (i = 0; i < data.length; i++) {
                if (data[i].PeriodoLectivoText === $("#PeriodoLectivoId :selected").text()) {
                    Mensaje.Error.texto = "El periodo lectivo " + data[i].PeriodoLectivoText + " ya se encuentra agregado";
                    Mensaje.Error.mostrar();
                    return;
                }
            }
        };

        $("#listPeriodosLectivos").addRowData(peridoLectivo.Id, peridoLectivo, 'last');
        $('#PeriodoLectivoId').val(-1);

    });

    //funcionalidad al btnEliminarPeriodoLectivo
    $("#btnEliminarPeriodoLectivo").click(function () {
        var data = $("#listPeriodosLectivos").jqGrid("getGridParam", "selrow");
        $("#listPeriodosLectivos").delRowData(data);
    })


}

EmpresaRegistrar.TipoEducacion = null;
EmpresaRegistrar.BotonSugerirNombreEsVisible = false;
EmpresaRegistrar.EstructuraEscuelaEsRequerido = false;
EmpresaBloquearEmpresaPadreDeInspeccion = false;
EmpresaRegistrar.estadoText;
EmpresaRegistrar.mostrarInstrumentoLegalParaZonaDesfavorable = function () {
    $("#instrumentoLegalParaZonaDesfaforable").show();
}
EmpresaRegistrar.cargarComboTurnoEstructuraEscolar = function () {
    var turnos = $("#listTurnos").getRowData();
    var html = "<option value='-1'>SELECCIONE</option>";
    for (var i = 0; i < turnos.length; i++) {
        html += "<option value='" + turnos[i].idTurnos + "'>"  + turnos[i].turnos + "</option>";
    }
    $("#EstructuraEscolar_Turno").html(html);
}
function inicializarInstrumentoLegal() {
    
    InstrumentoLegal.init("InstrumentosLegales_InstrumentoLegal_");
    Expediente.init("InstrumentosLegales_InstrumentoLegal_Expediente_");
    InstrumentoLegal.mostrarBusquedaInstrumento();


}

EmpresaRegistrar.VisibilidadBusquedaEmpresaPadre = function () {
    if ($('#TipoEmpresa option:selected').text() == 'MINISTERIO') {
        $('#divSeleccionEmpresaPadre').hide();
    }
    else {
        if ($('#TipoEmpresa option:selected').text() == 'ESCUELA_MADRE' || $('#TipoEmpresa option:selected').text() == 'ESCUELA_ANEXO') {
            $.get($.getUrl('/GestionEmpresa/ParametroJerarquiaSigueOrganigrama'), {},
                function (data) {
                    if (data == "True") {
                        $("#divSeleccionEmpresaPadre").hide();
                    }
                    else {
                        $("#divSeleccionEmpresaPadre").show();
                    }
                }
            );
        }
        else {
            $('#divSeleccionEmpresaPadre').show();
            if ($('#TipoEmpresa option:selected').text() == 'INSPECCION') {
                EmpresaBloquearEmpresaPadreDeInspeccion = true;
            }
            else {
                EmpresaBloquearEmpresaPadreDeInspeccion = false;
            }
        }
    }
}


EmpresaRegistrar.VisibilidadBotonSugerirNombre = function (valor) {    
    if(valor){
        $('#btnNombreSugerido').show();
        EmpresaRegistrar.BotonSugerirNombreEsVisible = true;
    }
    else{
        $('#btnNombreSugerido').hide();
        EmpresaRegistrar.BotonSugerirNombreEsVisible = false;
    }
}

EmpresaRegistrar.init = function (estadoText) {
    Validacion.init();
    EmpresaRegistrar.estadoText = estadoText;

    //cambio el valor a mostrar en el combo de categoria escuela, segun requerimiento de analisis
    var categorias = $("#CategoriaEscuela option");
    for (var i = 1; i < categorias.length; i++) {
        categorias[i].text = i;
    }


    if (estadoText == "Ver") {
        EmpresaRegistrar.cargarGrillaInstrumentosLegales();
    }

    $("#NumeroAnexo").attr("maxlength", 2);
    $("#CodigoEmpresa").attr("maxlength", 9);
    $("#Sigla").attr("maxlength", 10);
    $("#btnEmpresaPadre").hide();
    $("#btnSelecEmpresaInspeccion").hide();
    $("#btnEmpresaSupervisora").hide();
    $("#divMensajeEmpresaSupervisora").hide();
    document.getElementById("CodigoEmpresa").maxlength = 9;
    $("#Telefono").numeric();
    $("#Telefono").attr("maxlength", 11);

    if (estadoText == "Ver" || estadoText == "Editar") { $("#divVincularEdificioAEmpresa").show(); $("#fieldsetTabs").show(); $("#tabs").tabs(); EmpresaRegistrar.CargarComboTipoInspeccion(); }
    $('#btnModificarTipoEmpresa').click(function () {
        var empresa = {};
        empresa.Id = $('#Id').val();
        empresa.CodigoEmpresa = $('#CodigoEmpresa').val();
        empresa.NombreEmpresa = $('#Nombre').val();
        empresa.CUE = $('#CUE').val();
        empresa.CUEAnexo = $('#CUEAnexo').val();
        empresa.TipoEducacion = $('#TipoEducacion').val();
        empresa.NivelEducativo = $('#NivelEducativoId').val();
        empresa.TipoEmpresa = $('#TipoEmpresa').val();
        ModificarTipoEmpresa.empresaSeleccionada(empresa);
    });

    $("#divInstrumentoLegal > fieldset").append("<div align='justify'><input align='justify' value='Asignar' type='button' id='btnAsignar'/><input align='justify' value='Editar' type='button' id='btnEditar'/></div>");
    AsignacionInstrumentoLegal.mostrarFechaNotificacion();
    // $("#divInstrumentoLegal > fieldset > p").hide();

    //estos son botones que le dan funcionalidad a parte del instrumento legal
    $("#btnAsignar").click(function () {
        // if ($("#InstrumentosLegales_InstrumentoLegal_FechaEmision").val() != "" && $("#InstrumentosLegales_InstrumentoLegal_NroInstrumentoLegal").val() != "") {
        $("#divInstrumentoLegal > fieldset > fieldset").setEnabled(false);
        $("#btnEditar").setEnabled(true)
        //}

    });
    $("#btnEditar").click(function () {
        $("#divInstrumentoLegal > fieldset > fieldset").setEnabled(true);
        $("#btnBuscarInstrumento").setEnabled(true);
        $("#btnRegistrarNuevo").setEnabled(true);
        $("#btnAsignar").setEnabled(true)
        //  $("#btnEditar").setEnabled(true)
    });

    $(".val-DateTime").mask("99/99/9999", { placeholder: " " });
    $(".val-DateTime").datepicker({ currentText: 'Now', dateFormat: 'dd/mm/yy' });
    if (estadoText != "Ver") {
        $("#FechaInicioActividades").val("");
        $("#FechaNotificacion").val("");
    }

    //guarda en el data aquellos atributos q se repiten en distintos tipos de empresa
    EmpresaRegistrar.obtenerParrafos('TipoEducacion');
    EmpresaRegistrar.obtenerParrafos('TipoEscuela');
    EmpresaRegistrar.obtenerParrafos('NivelEducativoId');
    EmpresaRegistrar.obtenerParrafos('CodigoEmpresa');
    EmpresaRegistrar.obtenerParrafos('Nombre');

    EmpresaRegistrar.ocultarTodos();
    EmpresaRegistrar.mostrarCapas();


    EmpresaRegistrar.domiciliosAgregados = new Array();
    EmpresaRegistrar.edificiosAgregados = new Array();
    EmpresaRegistrar.initVinculos();
    EmpresaRegistrar.initEdificios();
    //check para mostrar/ocultar vinculo edificio
    $("#VincularEdificioCheck").changePatch(function () {
        if ($("#VincularEdificioCheck").is(":checked")) {
            $("#divVincularEdificioAEmpresa").show();
        }
        else {
            $("#divVincularEdificioAEmpresa").hide();
        }
    });
    $("#btnVincularEdificio").click(function () {
        var edificioId = GrillaUtil.getSeleccionFilas($("#listEdificios"), false); // Trae el id del edificio
        if (edificioId == null || edificioId == "") {
            Mensaje.Error.texto = "Seleccione el edificio de la lista que quiere vincular a la empresa";
            Mensaje.Error.mostrar();
        }
        else {
            EmpresaRegistrar.agregarVinculo(edificioId);
        }
    });

    $("#btnBorrarVinculo").click(function () {
        var vinculoId = GrillaUtil.getSeleccionFilas($("#listVinculos"), false); // Trae el id del edificio
        if (vinculoId == null || vinculoId == "") {
            Mensaje.Error.texto = "Seleccione el vínculo de la lista que desea borrar";
            Mensaje.Error.mostrar();
        }
        else {
            EmpresaRegistrar.borrarVinculo();
        }
    });


    //Script del modificar asignacion escuelas a inspección
    EmpresaRegistrar.ModificarAsignacion.init();
    //Grilla de niveles educativos por tipo educacion
    EmpresaRegistrar.cargarGrillaNETE();
    //Grilla tipo de escuelas
    EmpresaRegistrar.cargarGrillaTE();
    //Grilla turnos
    EmpresaRegistrar.cargarGrillaTutrnos();
    //Estructura escolar
    EmpresaRegistrar.EstructuraEscolar();

    //Carga combo tipo inspeccion con enumeracion o con las inspecciones intermedias
    EmpresaRegistrar.CargarComboTipoInspeccion();

    EmpresaRegistrar.cargarGrillaPeriodoLectivo();
    $('#CUE').attr('maxLength', 7);
    $('#CUEAnexo').attr('maxLength', 2);
    $('#NumeroEscuela').attr('maxLength', 4);

    $('#TipoEmpresa').changePatch(function () {
        EmpresaRegistrar.ocultarTodos();
        EmpresaRegistrar.mostrarCapas();
        EmpresaRegistrar.VisibilidadBusquedaEmpresaPadre();
        EmpresaRegistrar.VisibilidadBotonSugerirNombre(EmpresaRegistrar.BotonSugerirNombreEsVisible);
    });

    //Cargo los ids de los editoresn de Director y Representante (para escuela privada)
    EmpresaRegistrar.idRepresentante = $("#RepresentanteLegal_Id").val();
    EmpresaRegistrar.idDirector = $("#Director_Id").val();
    if (EmpresaRegistrar.idRepresentante == null || EmpresaRegistrar.idRepresentante == "") {
        EmpresaRegistrar.idRepresentante = 0;
    }
    if (EmpresaRegistrar.idDirector == null || EmpresaRegistrar.idDirector == "") {
        EmpresaRegistrar.idDirector = 0;
    }
    $("#divRepresentanteLegal").html("");
    $("#divDirector").html("");

    //funcionalidad (al btn aceptar y demas) dependiendo si es modificar, registrar o ver
    switch (estadoText) {
        case "Registrar":
            $("#btnModificarTipoEmpresa").hide();
            $("#divBtnEmpresaPadre").show();
            //si se cambia el tipo de empresa, vuelve todas las consultas a su estado inicial
            $('#TipoEmpresa').changePatch(function () { EmpresaRegistrar.resetearConsultas() });
            $("#btnAceptar").click(function () {
                var hidden = $("#divVista input[type='button']:hidden");
                var disabled = $("#divVista :disabled");
                if (!Validacion.validar()) {
                    $.each(hidden, function (ind, val) {
                        $(this).hide();
                    });
                    $(disabled).each(function () {
                        $(this).attr("disabled", "disabled");
                    });
                    //return;                    
                }
                else {
                    EmpresaRegistrar.VisibilidadBusquedaEmpresaPadre();
                    //EmpresaRegistrar.VisibilidadBotonSugerirNombre(EmpresaRegistrar.BotonSugerirNombreEsVisible);
                    EmpresaRegistrar.mostrarCampos($("#TipoEmpresa option:selected").val());
                    if (!$("#checkRegistrarInstrumento").is(":checked")) {
                        $("#divInstrumentoLegal .val-Required").removeClass("val-Required");
                        $("#divInstrumentoLegal :input[type!=button]").val("");
                    }
                    if ($("#instrumentoLegalParaZonaDesfaforable").is(":hidden")) {
                        $("#instrumentoLegalParaZonaDesfaforable .val-Required").removeClass("val-Required");
                        $("#instrumentoLegalParaZonaDesfaforable :input[type!=button]").val("");
                    }
                    //verifica q la cantidad de caracteres del telefono sea como minimo de 6
                    if ($('#Telefono').val().length > 0 && $('#Telefono').val().length < 6) {

                        return;
                    }
                    if ($("#CUE").val().length !== 0 && $("#CUEAnexo").val().length === 0) {
                        Mensaje.Error.texto = "Ingrese un CUE Anexo";
                        Mensaje.Error.mostrar();
                        return;
                    }
                    //if ($('#RegistrarEstructuraEscolarCheck').is(':visible')) {
                    if (EmpresaRegistrar.EstructuraEscuelaEsRequerido) {
                        if ($('#listaEstructura').getDataIDs().length === 0) {
                            Mensaje.Error.texto = "Registrar estructura escolar es requerido.";
                            Mensaje.Error.mostrar();
                            return;
                        }
                    }
                    $("#divVista form :input").removeAttr("disabled");
                    if (EmpresaRegistrar.editorConsultarPadre) {
                        if (EmpresaRegistrar.editorConsultarPadre.seleccion) {
                            $('#EmpresaPadreOrganigramaId').val(EmpresaRegistrar.editorConsultarPadre.seleccion);
                        }
                    }
                    if (EmpresaRegistrar.editorConsultarRaiz) {
                        if (EmpresaRegistrar.editorConsultarRaiz.seleccion) {
                            $('#EscuelaRaizId').val(EmpresaRegistrar.editorConsultarRaiz.seleccion);
                        }
                    }
                    if (EmpresaRegistrar.editorConsultarMadre) {
                        if (EmpresaRegistrar.editorConsultarMadre.seleccion) {
                            $('#EscuelaMadreId').val(EmpresaRegistrar.editorConsultarMadre.seleccion);
                        }
                    }
                    if (EmpresaRegistrar.editorConsultarEInspeccion) {
                        if (EmpresaRegistrar.editorConsultarEInspeccion.seleccion) {
                            $('#EmpresaInspeccionId').val(EmpresaRegistrar.editorConsultarEInspeccion.seleccion);
                        }
                    }
                    if (EmpresaRegistrar.editorConsultarESupervisora) {
                        if (EmpresaRegistrar.editorConsultarESupervisora.seleccion) {
                            $('#EmpresaInspeccionSupervisoraId').val(EmpresaRegistrar.editorConsultarESupervisora.seleccion);
                        }
                    }

                    var DomicilioId = GrillaUtil.getSeleccionFilas($("#listDomicilio"), false); // Trae el id del domicilio
                    if (!DomicilioId) {
                        Mensaje.Advertencia.texto = "Para sugerir nombre, se requiere que se seleccione un domicilio. (Paso 2)";
                        Mensaje.Advertencia.botones = false;
                        Mensaje.Advertencia.mostrar();
                        return;
                    }
                    $('#DomicilioId').val(DomicilioId);

                    var formulario = $("#divVista form").serializeArray();

                    if ($("#TipoEmpresa").val() == 'DIRECCION_DE_NIVEL') {
                        var NETETemporal = $("#listNETE").getRowData();
                        var NETE = [];
                        for (var i = 0; i < NETETemporal.length; i++) {
                            var nete = NETETemporal[i];
                            NETE[i] = {};
                            NETE[i].NivelEducativo = {};
                            NETE[i].NivelEducativo.Id = nete.idNE;
                            NETE[i].NivelEducativo.Nombre = nete.nivelEducativo;
                            NETE[i].TipoEducacion = nete.tipoEducacion;
                        }
                        $.formatoModelBinder(NETE, formulario, "NivelEducativoPorTipoEducacion");
                    }

                    if ($("#TipoEmpresa").val() == 'DIRECCION_DE_NIVEL') {
                        var TETemporal = $("#listTE").getRowData();
                        var TE = [];
                        for (var i = 0; i < TETemporal.length; i++) {
                            var te = TETemporal[i];
                            TE[i] = {};
                            TE[i].Id = te.idTE;
                            TE[i].Nombre = te.tipoEscuela;
                        }
                        $.formatoModelBinder(TE, formulario, "TiposEscuelas");
                    }

                    if ($("#TipoEmpresa").val() == 'ESCUELA_MADRE' || $("#TipoEmpresa").val() == 'ESCUELA_ANEXO') {
                        var Turnos = $("#listTurnos").getRowData();
                        var T = [];
                        for (var i = 0; i < Turnos.length; i++) {
                            var turno = Turnos[i];
                            T[i] = {};
                            T[i].Id = turno.idTurnos;
                            T[i].Nombre = turno.turnos;
                        }
                        $.formatoModelBinder(T, formulario, "Turnos");


                        //periodo lectivo

                        var PeriodosLectivos = $("#listPeriodosLectivos").getRowData();
                        var P = [];
                        for (var i = 0; i < PeriodosLectivos.length; i++) {
                            var periodo = PeriodosLectivos[i];
                            P[i] = {};
                            P[i].Id = periodo.PeriodoLectivoId;
                            P[i].Nombre = periodo.PeriodoLectivoText;
                        }
                        $.formatoModelBinder(P, formulario, "PeriodosLectivos");


                        var estructuras = $("#listaEstructura").getRowData();
                        var DE = [];
                        $.each(estructuras, function (ind, val) {
                            DE[ind] = {};
                            DE[ind].Id = 0;
                            DE[ind].Turno = val.Turno;
                            DE[ind].GradoAnio = val.GradoAnio;
                            DE[ind].Division = val.Division;
                            DE[ind].Cupo = val.Cupo;
                            DE[ind].FechaApertura = val.FechaApertura;
                        });
                        if (estructuras != null) {
                            $.formatoModelBinder(DE, formulario, "EstructuraEscolar");
                        }
                    }

                    var Vinculos = $("#listVinculos").getRowData();
                    var V = [];
                    for (var i = 0; i < Vinculos.length; i++) {
                        V[i] = {};
                        V[i].EdificioId = Vinculos[i].IdEdificio;
                        V[i].Observacion = Vinculos[i].Observacion;
                        V[i].FechaDesde = Vinculos[i].FechaDesde;
                        V[i].DeterminaDomicilio = Vinculos[i].DeterminaDomicilio;
                    }
                    $.formatoModelBinder(V, formulario, "VinculoEmpresaEdificio");

                    $("#btnAceptar").attr('disabled', 'disabled');
                    formulario = formulario.filter(function (campo) {
                        return (campo.value !== "" && campo.value !== null && campo.value !== undefined);
                    });

                    $.post($.getUrl("/GestionEmpresa/Registrar"), formulario,
                    function (data) {
                        if (data.status) {

                            location.reload(true);

                            $("#tabs").tabs("select", 0)
                            $('#btnCancelarRegistro').click();
                            Mensaje.Exito.mostrar();
                            EmpresaRegistrar.limpiarCampos();
                            return; //Este RETURN es para evitar que el metodo se siga ejecutando, ya que el mensaje de EXITO lo mostramos
                            //en el INDEX, para evitar tener que activar y desactivar la edicion de los textbox y demas estupefacientes

                            if (!data.mail) {
                                // fallo el envio de email
                                Mensaje.Advertencia.texto = data.mensaje;
                                Mensaje.Advertencia.botones = false;

                                //  Mensaje.Advertencia.mostrar();
                                Mensaje.mostrarSinOcultar(Mensaje.Advertencia.div, Mensaje.Advertencia.texto);
                                $(".confirmacion-botones").hide();
                            }
                            if (!data.mail) {
                                $(Mensaje.Advertencia.div).show();
                            }
                            // Mensaje.Exito.mostrar();

                            Mensaje.mostrarSinOcultar(Mensaje.Exito.div, Mensaje.Exito.texto);

                            $('#divVista input, #divVista select, #divVista textarea').attr('disabled', true);

                            $("#btnAceptar").hide();


                            $("#btnCancelarRegistro").attr('disabled', false);
                            $("#btnCancelarRegistro").attr('value', 'Volver');

                        }
                        else {
                            $("#btnAceptar").attr('disabled', false);
                            for (var i = 0; i < data.details.length; i++) {
                                Mensaje.Error.agregarError(data.details[i]);
                            }
                        }

                        $(hidden).each(function () {
                            $(this).hide();
                        });

                        $(disabled).each(function () {
                            $(this).attr("disabled", "disabled");
                        });
                    }, "json");
                }
            });
            EmpresaRegistrar.VisibilidadBotonSugerirNombre(EmpresaRegistrar.BotonSugerirNombreEsVisible);
            $("#TipoEmpresa").attr("disabled", "disabled");
            $("#TipoEmpresa").CascadingDropDown("#TipoGestion", $.getUrl('/GestionEmpresa/CargarTipoEmpresa'), { promptText: 'SELECCIONE' });

            //si el tipo de empresa es ESCUELA, mostrar el btn para q el sistema sugiera un nombre para la escuela.
            $('#btnNombreSugerido').hide();
            $('#NumEscuela').hide();
            $('#TipoGestion').changePatch(function () {
                EmpresaRegistrar.VisibilidadBotonSugerirNombre(false);
                $("#fieldsetTabs").hide();
            });

            $('#btnNombreSugerido').attr('disabled', false);

            $('#btnNombreSugerido').click(function () {
                var model = {};
                if (EmpresaRegistrar.editorConsultarRaiz) {
                    if (EmpresaRegistrar.editorConsultarRaiz.seleccion) {
                        $('#EscuelaRaizId').val(EmpresaRegistrar.editorConsultarRaiz.seleccion);
                    }
                }
                if (EmpresaRegistrar.editorConsultarMadre) {
                    if (EmpresaRegistrar.editorConsultarMadre.seleccion) {
                        $('#EscuelaMadreId').val(EmpresaRegistrar.editorConsultarMadre.seleccion);
                    }
                }
                model.NivelEducativoId = $('#NivelEducativoId option:selected').text();
                model.TipoEducacion = $('#TipoEducacion option:selected').text();
                var DomicilioId = GrillaUtil.getSeleccionFilas($("#listDomicilio"), false); // Trae el id del domicilio
                if (!DomicilioId) {
                    Mensaje.Advertencia.texto = "Para sugerir nombre, se requiere que se seleccione un domicilio. (Paso 2)";
                    Mensaje.Advertencia.botones = false;
                    Mensaje.Advertencia.mostrar();
                    return;
                }
                if ($("#TipoEscuela").val() == "") {
                    Mensaje.Advertencia.texto = "Para sugerir nombre, se requiere que se seleccione el tipo de escuela. (Paso 5)";
                    Mensaje.Advertencia.botones = false;
                    Mensaje.Advertencia.mostrar();
                    return;
                }

                if ($("#TipoEmpresa").val() == "ESCUELA_ANEXO" && ($("#buscarEscuelaMadre_Id").val() == "0" || $("#buscarEscuelaMadre_Id").val() == undefined)) {
                    Mensaje.Advertencia.texto = "Para sugerir nombre, se requiere que se seleccione la escuela madre. (Paso 1)";
                    Mensaje.Advertencia.botones = false;
                    Mensaje.Advertencia.mostrar();
                    return;
                }

                if ($("#TipoEmpresa").val() == "ESCUELA_MADRE" && !$("#EsRaiz").is(":checked") && ($("#buscarEscuelaRaiz_Id").val() == "0" || $("#buscarEscuelaRaiz_Id").val() == undefined)) {
                    Mensaje.Advertencia.texto = "Para sugerir nombre, se requiere que se seleccione la escuela raiz. (Paso 1)";
                    Mensaje.Advertencia.botones = false;
                    Mensaje.Advertencia.mostrar();
                    return;
                }
                //model.DomicilioId = DomicilioId;

                $('#DomicilioId').val(DomicilioId)
                var datos = $("#divVista form").serializeArray();
                $.formatoModelBinder(model, datos, "");

                $.post($.getUrl('/GestionEmpresa/SugerirNombre'),
                    datos,
                    function (data) {
                        if (data.status) {
                            $('#Nombre').val(data.NombreEscuela);
                        }
                        else {
                            Mensaje.Error.limpiar();
                            Mensaje.Error.texto = "Error intentando sugerir nombre";
                            for (var i = 0; i < data.details.length; i++) {
                                Mensaje.Error.agregarError(data.details[i]);
                            }
                            //Mensaje.Error.mostrar();
                        }
                    },
                    'json'
                );
            });
            break;

        case "Editar":
            $("#divBtnEmpresaPadre").show();
        case "Ver":
            var formulario = $("#divVista form").serializeArray();
            if ($("#CUEAnexo").val() == "0") { $("#CUEAnexo").val(""); };
            $('#listaEstructura').setGridParam({ url: $.getUrl("/GestionEmpresa/GetEstructuraEscolar?empresaId=" + $("#Id").val()),
                datatype: "json",
                loadComplete: function (data) { $('#RegistrarEstructuraEscolarCheck').attr('checked', true).changePatch(); }
            });
            $('#ObservacionVinculo').attr('disabled', true);

            if ($("#TipoEmpresa").val() == "INSPECCION") {
                $("#divTipoInspeccionEnum").show();
                EmpresaBloquearEmpresaPadreDeInspeccion = true;
                $("#divTipoInspeccionEnum").children().show();
            }
            else {
                $("#divTipoInspeccionEnum").hide();
                EmpresaBloquearEmpresaPadreDeInspeccion = false;
                $("#divTipoInspeccionEnum").children().hide();
            }

            $('#listaEstructura').trigger('reloadGrid');

            EmpresaRegistrar.VisibilidadBusquedaEmpresaPadre();
            if ($("#TipoEmpresa").val() == 'MINISTERIO') {
                $('#btnEmpresaPadre').attr('disabled', 'disabled');
            }
            else {
                $('#btnEmpresaPadre').removeAttr('disabled');
                EmpresaRegistrar.setearEmpresaPadreSeleccionada();
            }
            EmpresaRegistrar.VisibilidadBotonSugerirNombre(EmpresaRegistrar.BotonSugerirNombreEsVisible);

            //Si estoy en el editar, que inicialice el script del modificar asignacion escuela a inspeccion
            if (EmpresaRegistrar.estadoText === "Editar") {
                EmpresaRegistrar.ModificarAsignacion.init();
            }

            if ($("#TipoEmpresa").val() == "ESCUELA_MADRE" || $("#TipoEmpresa").val() == "ESCUELA_ANEXO") {
                $("#btnModificarTipoEmpresa").show();

            }
            else {
                $("#btnModificarTipoEmpresa").hide();
            }

            $("#btnAceptar").click(function () {

                var modelJson = $("#divVista").formatoJson(); // serialize();
                modelJson.Albergue = $("#Albergue").is(":checked");
                modelJson.Religioso = $("#Religioso").is(":checked");
                modelJson.ContextoDeEncierro = $("#ContextoDeEncierro").is(":checked");
                modelJson.Arancelado = $("#Arancelado").is(":checked");
                modelJson.EsHospitalaria = $("#EsHospitalaria").is(":checked");
                modelJson.Privado = $("#Privado").is(":checked");
                //tomar turnos
                var Turnos = $("#listTurnos").getRowData();
                modelJson.Turnos = [];
                for (var i = 0; i < Turnos.length; i++) {
                    var turno = Turnos[i];
                    modelJson.Turnos[i] = {};
                    modelJson.Turnos[i].Id = turno.id;
                    modelJson.Turnos[i].Nombre = turno.turnos;
                }

                //vinculo edificio
                var Vinculos = $("#listVinculos").getRowData();
                var V = [];
                for (var i = 0; i < Vinculos.length; i++) {
                    V[i] = {};
                    V[i].EdificioId = Vinculos[i].IdEdificio;
                    V[i].Observacion = Vinculos[i].Observacion;
                    V[i].FechaDesde = Vinculos[i].FechaDesde;
                    V[i].DeterminaDomicilio = Vinculos[i].DeterminaDomicilio;
                }
                $.formatoModelBinder(V, formulario, "VinculoEmpresaEdificio");

                //periodo lectivo

                var PeriodosLectivos = $("#listPeriodosLectivos").getRowData();
                modelJson.PeriodosLectivos = [];
                for (var i = 0; i < PeriodosLectivos.length; i++) {
                    var periodo = PeriodosLectivos[i];
                    modelJson.PeriodosLectivos[i] = {};
                    modelJson.PeriodosLectivos[i].Id = periodo.PeriodoLectivoId;
                    modelJson.PeriodosLectivos[i].Nombre = periodo.PeriodoLectivoText;
                }
                // $.formatoModelBinder(P, formulario, "PeriodosLectivos");


                $("#TipoGestion").removeAttr('disabled');
                //  var modelJson = $("#divVista").formatoJson();// serialize();
                if ($("#NivelEducativoId").length > 0 && $("#NivelEducativoId").val() != "") {
                    modelJson.NivelEducativoId = $("#NivelEducativoId").val();
                }

                //Bindeo la lista de las asignaciones esculeas a inspeccion.
                var AsignacionEscuela = $("#buscarEscuelasAAsignar_listSeleccionadas").getRowData();
                modelJson.AsignacionEscuela = [];
                for (var i = 0; i < AsignacionEscuela.length; i++) {
                    var asignacion = AsignacionEscuela[i];
                    modelJson.AsignacionEscuela[i] = {};
                    modelJson.AsignacionEscuela[i].EscuelaId = asignacion.Id;
                    modelJson.AsignacionEscuela[i].InspeccionId = $("#Id").val();
                }
                //                var datos = [];
                //                $.formatoModelBinder(modelJson, datos, "");

                $.post($.getUrl("/GestionEmpresa/GetCampos"), formulario, function (formulario) {

                    if (formulario.details) {
                        for (var i = 0; i < formulario.details.length; i++) {
                            Mensaje.Error.agregarError(formulario.details[i]);
                        }
                    }
                    else {
                        if (formulario.status) {
                            Mensaje.Exito.mostrar();
                            EmpresaRegistrar.limpiarCampos();
                        } else {
                            $("#divVista").html(formulario);
                            $("#divVista").setEnabled(false);
                            $("#divVista").setEditable(false);
                        }
                    }
                });

                $("#TipoGestion").attr("disabled", "disabled");

            });

            EmpresaRegistrar.escuelaPrivada();
            if ($("#Privado").is(":checked") && $("#Privado").is(':visible')) {
                if (estadoText == "Ver") {
                    $("#Privado").attr("disabled", false);
                }
                else {
                    $("#Privado").attr("disabled", true);
                }

                $("#Privado").changePatch();
            }
            var tipoGestionSelec = $("#TipoGestion option:selected").text()
            $("#TipoGestion").attr("disabled", "disabled");
            var tipoEmpresaSelec = $("#TipoEmpresa option:selected").val();
            EmpresaRegistrar.mostrarCampos(tipoEmpresaSelec);
            $("#TipoEmpresa option").remove();
            $.getJSON($.getUrl('/GestionEmpresa/CargarTipoEmpresa'), { TipoGestion: tipoGestionSelec }, function (data) {
                var html = '';
                var len = data.length;
                for (var i = 0; i < len; i++) {
                    html += '<option value="' + data[i].Value + '">' + data[i].Text + '</option>';
                    //$('#TipoEmpresa').append('<option value="' + data[i].Id + '">' + data[i].Nombre + '</option>');
                }
                $("#TipoEmpresa").append(html);
                $("#TipoEmpresa").val(tipoEmpresaSelec);

                //carga las 2 grillas con los datos q vienen en el model
                if ($('#TipoEmpresa').val() === "ESCUELA_MADRE" || $('#TipoEmpresa').val() === "ESCUELA_ANEXO") {
                    EmpresaRegistrar.cargarListaTurnos();
                    EmpresaRegistrar.cargarListaPeriodosLectivos();
                    if ($('#EsRaiz').is(':checked')) {
                        $('#btnNombreSugerido').hide();
                        EmpresaRegistrar.BotonSugerirNombreEsVisible = false;
                    }
                }
                else {
                    $('#btnNombreSugerido').hide();
                    EmpresaRegistrar.BotonSugerirNombreEsVisible = false;
                }

                if ($('#TipoEmpresa').val() === "DIRECCION_DE_NIVEL") {
                    EmpresaRegistrar.cargarListaNETE();
                    EmpresaRegistrar.cargarListaTE();
                }

                EmpresaRegistrar.cargarVinculoEdificio();

                //campos en modo lectura segun el tipo de gestion de la empresa a modificar
                $('#TipoEmpresa').attr('disabled', true);
                switch (tipoGestionSelec) {
                    case 'ESCUELA':
                        $('#NivelEducativoId').attr('disabled', true);
                        $('#Privado').attr('disabled', true);
                    default:
                        break;
                }
            });
            if (EmpresaRegistrar.estadoText === 'Ver') {
                $('#divVista input').attr('disabled', true);
                $('#btnAceptar').hide();
                $('#btnCancelarRegistro').val('Volver').attr('disabled', false);
                $("#grillaEstructura TD[title='Agregar'], #grillaEstructura TD[title='Editar'], #grillaEstructura TD[title='Eliminar']").hide();
            }

            //EmpresaRegistrar.verificarParametroJerarquiaInspeccionOrganigrama();

            //Cargo los datos de la empresa que registro esta empresa (la seleccionada para ver), y si la empresa está en estado "CERRADA" muestro la fecha de cierre.
            //Tambien trae el Id de la empresa padre
            $.get($.getUrl('/GestionEmpresa/GetDatosNecesariosVerEditarEmpresa'), { idEmpresa: $("#Id").val() },
                function (data) {
                    if (data != null) {
                        if (data.CodigoEmpresa != null) {
                            $("#CodigoEmpresaQueRegistro").val(data.CodigoEmpresa);
                        }
                        if (data.NombreEmpresa != null) {
                            $("#NombreEmpresaQueRegistro").val(data.NombreEmpresa);
                        }
                        if (data.FechaCierre != null) {
                            $("#pFechaCierrre").show();
                            $("#FechaCierre").val(data.FechaCierre);
                        }
                        //                        if (data.IdEmpresaPadre != null) {
                        //                            alert(EmpresaRegistrar.editorConsultarPadre);
                        //                            EmpresaRegistrar.editorConsultarPadre.seleccion = data.IdEmpresaPadre;
                        //                            EmpresaRegistrar.editorConsultarPadre.prefijo = "buscarEmpresaPadre";
                        //                            console.log(EmpresaRegistrar.editorConsultarPadre);
                        //                            ConsultarEmpresa.seleccionarEmpresa(EmpresaRegistrar.editorConsultarPadre);
                        //                        }
                    }
                });
            //Muestro/Oculto las solapas de acuerdo al tipo de empresa.
            EmpresaRegistrar.mostrarSelectores(tipoEmpresaSelec);
            break;

        case "Revisar":
            $("#btnAceptar").hide();
            $("#btnCancelarRegistro").val("Volver");
            break;
    } //fin funcionalidad
    //funcionalidad al boton cancelar
    $("#btnCancelarRegistro").click(function () {
        if ($("#btnCancelarRegistro").val() == "Volver") {
            var instancia = {};
            instancia.prefijo = "#consultaIndex_";
            ConsultarEmpresa.cargarFiltrosBusqueda(instancia);
        }

        EmpresaRegistrar.limpiarCampos();
        $("#divVista").hide();
        $("#divMensajeError").hide();
        Mensaje.ocultar();
        $("#divFiltrosDeBusqueda").show();
        $("#divConsulta").show();
        $("#checkRegistrarInstrumento").removeAttr("checked");
    }); //fin funcionalidad

    EmpresaRegistrar.deshabilitarCampos();


    //habilita/deshabilita btn buscar escuela raiz
    $("#btnEscuelaRaiz").removeAttr("disabled");
    $("#EsRaiz").changePatch(function () {
        if ($("#EsRaiz").is(":checked")) {
            $("#btnNombreSugerido").hide();
            EmpresaRegistrar.BotonSugerirNombreEsVisible = false;
            $("#btnEscuelaRaiz").attr("disabled", "disabled");
            if ($('#divSeleccionEscuelaRaiz').is(':visible')) {
                $('#btnEscuelaRaiz').click();
            }
        }
        else {
            $("#btnNombreSugerido").show();
            EmpresaRegistrar.BotonSugerirNombreEsVisible = true;
            $("#btnEscuelaRaiz").removeAttr("disabled");
        }
    });

    //funcionalidad al btn modificar tipo empresa
    $('#btnModificarTipoEmpresa').click(function () {
        $.getUrl('GestionEmpresa/ModificarTipoEmpresa');
    });
    //fin funcionalidad
    //-----------------------------------------------------------------------------------------------------------------------
    //FUNCIONALIDAD A LAS DISTINTA LLAMADAS AL CONSULTAR EMPRESA DENTRO DEL REGISTRAR

    //----Empresa padre----//
    var banderaEPadre = false;
    $('#btnEmpresaPadre').click(function () {
        $("#divMensajeEmpresaSupervisora").hide();
        if (EmpresaBloquearEmpresaPadreDeInspeccion == true) {
            $("#divMensajeEmpresaSupervisora").show();
            $("#divSeleccionEmpresaPadre").hide();
            return;
        }
        //obtiene la vistaEnum dependiendo del tipo de empresa a registrar
        var filtroTipoEmpresa = EmpresaRegistrar.getNuevaVistaEnum();

        //pregunto si ya se creo el editor para empresa padre
        if (banderaEPadre) {

            if ($('#divSeleccionEmpresaPadre').is(':visible')) {
                $('#divSeleccionEmpresaPadre').hide();
                $('#DatosEmpresaPadre').show();
                //valido que si es inspeccion no retorne, pues es necesario que se inicialice la busqueda de empresa padre
                if ($('#TipoEmpresa').val() != "INSPECCION") {
                    return;
                }
            }
            else {
                $('#divSeleccionEmpresaPadre').show();
                $('#DatosEmpresaPadre').hide();
                if ($('#TipoEmpresa').val() != "INSPECCION") {
                    return;
                }
            }
        }

        banderaEPadre = true;

        if (!EmpresaRegistrar.editorConsultarPadre) {
            EmpresaRegistrar.editorConsultarPadre = ConsultarEmpresa.init(filtroTipoEmpresa, '#divSeleccionEmpresaPadre', 'buscarEmpresaPadre', false);
            $("#buscarEmpresaPadre_divDatosGeneralesEmpresa > fieldset").append("<p style='display: block'><input type='button' id='btnVolverABuscar' value='Buscar Nuevamente'/></p>")
        }
        else {
            EmpresaRegistrar.editorConsultarPadre.vista = filtroTipoEmpresa;
        }
        $("#btnVolverABuscar").click(function () {
            $("#buscarEmpresaPadre_divDatosGeneralesEmpresa").hide();
            $("#buscarEmpresaPadre_divFiltrosDeBusqueda").show();
            $('#conjuntoEmpresaPadre').css("visibility", "hidden");
            $("label[for='Seleccione empresa padre']").show()
        });
        // texto que se muestra cuando seleccionamos
        $("td [title|='Seleccionar']").click(function () {
            $('#conjuntoEmpresaPadre').css("visibility", "visible");
            $("label[for='Seleccione empresa padre']").hide();
        });
        $('#divSeleccionEmpresaPadre').show();
        $('#DatosEmpresaPadre').hide();
        $("#divVista").show();

        $(EmpresaRegistrar.editorConsultarPadre.grilla.id).setGridWidth(580, true);
        $(EmpresaRegistrar.editorConsultarPadre.grilla.id + "sinRegistros").hide();
    });

    //----Escuela raiz----//
    var banderaERaiz = false
    $("#btnEscuelaRaiz").click(function () {
        if (banderaERaiz) {
            if ($('#divSeleccionEscuelaRaiz').is(':visible')) {
                $('#divSeleccionEscuelaRaiz').hide();
            }
            else {
                $('#divSeleccionEscuelaRaiz').show();
            }
            return;
        }
        banderaERaiz = true;
        $('#conjuntoEscuelaRaiz').show();
        EmpresaRegistrar.editorConsultarRaiz = ConsultarEmpresa.init('BusquedaPorEscuelaRaiz', '#divSeleccionEscuelaRaiz', 'buscarEscuelaRaiz', false);

        $('#divSeleccionEscuelaRaiz').show();
        $("#divVista").show();
        $(EmpresaRegistrar.editorConsultarRaiz.grilla.id).setGridWidth(580, true);
        $(EmpresaRegistrar.editorConsultarRaiz.grilla.id + "sinRegistros").hide();
    });

    //----Escuela madre----//
    var banderaEMadre = false;
    $("#btnEscuelaMadre").click(function () {
        if (banderaEMadre) {
            if ($('#divSeleccionEscuelaMadre').is(':visible')) {
                $('#divSeleccionEscuelaMadre').hide();
                $('#DatosEscuelaMadre').show();
            }
            else {
                $('#divSeleccionEscuelaMadre').show();
                $('#DatosEscuelaMadre').hide();
            }
            return;
        }
        banderaEMadre = true;
        $('#conjuntoEscuelaMadre').show();
        EmpresaRegistrar.editorConsultarMadre = ConsultarEmpresa.init('BusquedaPorEscuelaAnexo', '#divSeleccionEscuelaMadre', 'buscarEscuelaMadre', false);
        $('#divSeleccionEscuelaMadre').show();
        $('#DatosEscuelaMadre').hide();
        $("#divVista").show();
        $(EmpresaRegistrar.editorConsultarMadre.grilla.id).setGridWidth(580, true);
        $(EmpresaRegistrar.editorConsultarMadre.grilla.id + "sinRegistros").hide();
    });

    $('#TipoInspeccionEnum').changePatch(function () {
        if ($('#TipoEmpresa').val() == "INSPECCION") {
            if ($('#TipoInspeccionEnum').val() == "") {
                EmpresaBloquearEmpresaPadreDeInspeccion = true;
            }
            else {
                EmpresaBloquearEmpresaPadreDeInspeccion = false;
                $("#btnEmpresaPadre").click();
            }
        }
    });

    //----Empresa inspeccion----//
    var banderaEInsp = false;
    $("#btnSelecEmpresaInspeccion").click(function () {
        if (banderaEInsp) {
            if ($('#divSeleccionEmpresaInspeccion').is(':visible')) {
                $('#divSeleccionEmpresaInspeccion').hide();
            }
            else {
                $('#divSeleccionEmpresaInspeccion').show();
            }
            return;
        }
        banderaEInsp = true;
        EmpresaRegistrar.editorConsultarEInspeccion = ConsultarEmpresa.init('BuscarInspeccionesZonal', '#divSeleccionEmpresaInspeccion', 'buscarEInspeccion', false);
        $('#divSeleccionEmpresaInspeccion').show();
        $("#divVista").show();
        $(EmpresaRegistrar.editorConsultarEInspeccion.grilla.id).setGridWidth(580, true);
        $(EmpresaRegistrar.editorConsultarEInspeccion.grilla.id + "sinRegistros").hide();
    });

    //----Empresa supervisora----//
    var banderaESuperv = false;
    $("#btnEmpresaSupervisora").click(function () {
        if (banderaESuperv) {
            if ($('#divSeleccionEmpresaSupervisora').is(':visible')) {
                $('#divSeleccionEmpresaSupervisora').hide();
                $('#DatosEmpresaSupervisora').show();
            }
            else {
                $('#divSeleccionEmpresaSupervisora').show();
                $('#DatosEmpresaSupervisora').hide();
            }
            return;
        }
        banderaESuperv = true;
        EmpresaRegistrar.editorConsultarESupervisora = ConsultarEmpresa.init('BuscarInspecciones', '#divSeleccionEmpresaSupervisora', 'buscarESupervisora', false);
        $('#divSeleccionEmpresaSupervisora').show();
        $('#DatosEmpresaSupervisora').show();
        $("#divVista").show();
        $(EmpresaRegistrar.editorConsultarESupervisora.grilla.id).setGridWidth(580, true);
        $(EmpresaRegistrar.editorConsultarESupervisora.grilla.id + "sinRegistros").hide();
    });

    //END FUNCIONALIDAD A LAS DISTINTAS CONSULTAS
    //-----------------------------------------------------------------------------------------------------------------------
    //activa o no el btn de instrumento legal, dependiendo de la zona desfavorable
    $("#ZonaDesfavorableId").changePatch(function () {
        if ($("#ZonaDesfavorableId").val() != "" && $("#ZonaDesfavorableId").val() != "1") {
            $("#btnInstrumento").removeAttr("disabled");
            $("#instrumentoLegalParaZonaDesfaforable").show();
            EmpresaRegistrar.mostrarInstrumentoLegalParaZonaDesfavorable();


        }
        else {
            $("#btnInstrumento").attr("disabled", "disabled");
            $("#instrumentoLegalParaZonaDesfaforable").hide();
        }
    });

    //Inicializamos la asignación de instrumento legal
    $("#divSeleccionEmpresaInspeccion #btnCancelar").show();
    $("#divSeleccionEmpresaInspeccion #btnCancelar").click(function () {
        $("#divSeleccionEmpresaInspeccion").hide();
    });

    inicializarInstrumentoLegal();

    $("#checkRegistrarInstrumento").click(function () {

        $("#divInstrumentoLegal").toggle();
    });

}

//Método que selecciona en el editorEmpresaPadre la empresa padre seleccionada (EL VENENO)
EmpresaRegistrar.setearEmpresaPadreSeleccionada = function () {
    $('#divSeleccionEmpresaPadre').show();
    var vista = EmpresaRegistrar.getNuevaVistaEnum();
    //Inicializo el consultarEmpresaPadre
    EmpresaRegistrar.editorConsultarPadre = ConsultarEmpresa.init(vista, '#divSeleccionEmpresaPadre', 'buscarEmpresaPadre', false);
    EmpresaRegistrar.editorConsultarPadre.seleccion = $("#EmpresaPadreOrganigramaId").val();
    ConsultarEmpresa.seleccionarEmpresa(EmpresaRegistrar.editorConsultarPadre);

    if (EmpresaRegistrar.estadoText === "Ver") {
        //Deshabilitar "buscar nuevamente"
    }
};

//oculta todas los campos menos los comunes para todos los TipoEmpresa
EmpresaRegistrar.ocultarTodos = function () {
    $(".DireccionNivel, .EscuelaMadreAnexo, .EscuelaMadre, .EscuelaAnexo, .Inspeccion, .EsPrivado").hide();
    //$("#conjuntoEmpresaPadre").css("visibility","hidden");
    $("#conjuntoPaquete").hide();
    $("#conjuntoEscuelaRaiz").hide();
    $("#conjuntoEscuelaMadre").hide();
    $("#conjuntoEmpresaSupervisora").hide();
}

EmpresaRegistrar.verificarParametroJerarquiaInspeccionOrganigrama = function () {  
    $.get($.getUrl('/GestionEmpresa/GetParametroJerarquiaInspeccionOrganigrama'), {},
        function (data) {
            if (data != null) {
                if (data == "Y") {
                    //$("#divSeleccionEmpresaInspeccion").show();
                    $("#btnSelecEmpresaInspeccion").click();
                }
                else {
                    $("#btnEmpresaPadre").click();
                }
            }
            else {
                $("#divSeleccionEmpresaInspeccion").hide();
            }
        }
    );
}

//muestra/oculta divs dependiendo q tipo de empresa se selecciona
EmpresaRegistrar.mostrarCapas = function () {

    EmpresaRegistrar.ocultarTodos();
    // se ejecuta solo para el estado de Registrar
    if (EmpresaRegistrar.estadoText === "Registrar") {
        $("#TipoEmpresa").changePatch(function () {
            $("#pTipoEscuela, #pNivelEducativoId, #pTipoEducacion, #pNombre, #pCodigoEmpresa").remove();
            $('#ProgramaPresupuestarioId, #OrdenDePagoId').attr('disabled', false);
            $("btnEmpresaPadre").removeAttr('disabled');
            switch ($("#TipoEmpresa").val()) {
                case 'DIRECCION_DE_INFRAESTRUCTURA':
                case 'DIRECCION_DE_RECURSOS_HUMANOS':
                case 'DIRECCION_DE_SISTEMAS':
                case 'DIRECCION_DE_TESORERIA':
                case 'SECRETARIA':
                case 'SUBSECRETARIA':
                case 'APOYO_ADMINISTRATIVO':
                case 'MINISTERIO':
                    var empresa = "Todos";
                    EmpresaRegistrar.cargarParrafos(empresa);
                    //$("#divCheckRegistrarInstrumento").show();
                    inicializarInstrumentoLegal();
                    break;

                case "DIRECCION_DE_NIVEL":
                    EmpresaRegistrar.ocultarTodos();
                    var empresa = "DN";
                    EmpresaRegistrar.cargarParrafos(empresa);
                    $(".DireccionNivel").show();
                    inicializarInstrumentoLegal();
                    break;

                case "ESCUELA_MADRE":
                    EmpresaRegistrar.ocultarTodos();
                    $('#ProgramaPresupuestarioId, #OrdenDePagoId').removeAttr('disabled');
                    var empresa = "EMA";
                    EmpresaRegistrar.cargarParrafos(empresa);
                    //$("#divCheckRegistrarInstrumento").hide();
                    $(".EscuelaMadreAnexo, .EscuelaMadre").show();
                    EmpresaRegistrar.escuelaPrivada();
                    //Verificamos el valor del parametro Jerarquia Inspeccion Organigrama
                    EmpresaRegistrar.verificarParametroJerarquiaInspeccionOrganigrama();
                    break;

                case "ESCUELA_ANEXO":
                    EmpresaRegistrar.ocultarTodos();
                    $('#ProgramaPresupuestarioId, #OrdenDePagoId').removeAttr('disabled');
                    var empresa = "EMA";
                    EmpresaRegistrar.cargarParrafos(empresa);
                    //$("#divCheckRegistrarInstrumento").hide();
                    $(".EscuelaMadreAnexo, .EscuelaAnexo").show();
                    EmpresaRegistrar.escuelaPrivada();
                    //Verificamos el valor del parametro Jerarquia Inspeccion Organigrama
                    EmpresaRegistrar.verificarParametroJerarquiaInspeccionOrganigrama();
                    break;

                case "INSPECCION":
                    EmpresaRegistrar.ocultarTodos();
                    var empresa = "Inspeccion";
                    EmpresaRegistrar.cargarParrafos(empresa);
                    //$("#divCheckRegistrarInstrumento").hide();
                    $(".Inspeccion").show();
                    $("#btnEmpresaSupervisora").hide();
                    $("#TipoInspeccion").changePatch(function () {
                        if ($("#TipoInspeccion option:selected").text() != "SELECCIONE") {
                            if ($("#TipoInspeccion option:selected").text() == "GENERAL") {
                                $("#divSeleccionEmpresaSupervisora").hide();
                            }
                            else {
                                //$("#btnEmpresaSupervisora").show();
                                $("#btnEmpresaSupervisora").click();
                            }
                        }
                        else {
                            $("#divSeleccionEmpresaSupervisora").hide();
                        }
                    });
                    break;

                default: EmpresaRegistrar.ocultarTodos();
                    //$("#divCheckRegistrarInstrumento").show();
                    inicializarInstrumentoLegal();
                    break;
            }

            if ($("#TipoEmpresa").val() == "ESCUELA_MADRE" || $("#TipoEmpresa").val() == "ESCUELA_ANEXO") {
                if (EmpresaRegistrar.estadoText === "Registrar") {
                    $("#EsRaiz").attr("checked", false);
                }
                $("#btnNombreSugerido").show();
                EmpresaRegistrar.BotonSugerirNombreEsVisible = true;
                $.getJSON($.getUrl("/GestionEmpresa/OrdenDePagoProgramaPresupuestarioFromDireccionDeNivelActual"), {}, function (data) {
                    $('#ProgramaPresupuestarioId').val(data.programaPresupuestarioId);
                    $('#OrdenDePagoId').val(data.ordenDePagoId);
                });
            }
            else {
                $('#ProgramaPresupuestarioId').val("");
                $('#OrdenDePagoId').val("");
            }

            $('#NivelEducativoId').attr('disabled', true);
            $('#NivelEducativoId').CascadingDropDown('#TipoEducacion', $.getUrl('/GestionEmpresa/CargarNivelEducativo'), { promptText: 'SELECCIONE', loadingText: "Cargando.." });
            if ($("#TipoEmpresa").val() != "") {
                if (!($("#TipoEmpresa").val() == "MINISTERIO" || $("#TipoEmpresa").val() == "ESCUELA_MADRE" || $("#TipoEmpresa").val() == "ESCUELA_ANEXO")) {
                    $('#btnEmpresaPadre').click();
                }
            }
        });
    }
}

//--------------------- Grilla Niveles Educativo por Tipo Educacion ------------------------//

EmpresaRegistrar.cargarGrillaNETE = function () {
    var yaPasoPorAca = false;
    var grid = $("#listNETE").jqGrid({
        datatype: "local",
        colNames: ["id", "IdNE", "Nivel Educativo", "Tipo Educación"],
        colModel: [
                    { key: true, name: "id", index: "id", align: "left", hidden: true },
                    { key: false, name: "idNE", index: "idNE", align: "left", hidden: true },
                    { key: false, name: "nivelEducativo", index: "nivelEducativo", align: "left" },
                    { key: false, name: "tipoEducacion", index: "tipoEducacion", align: "left", hidden: true }
                ],
        rowNum: 10,
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: document.body.offsetWidth - 650,
        height: "100%"
    });

    $("#btnAgregarNETE").click(function () {

        //si es la primera vez q se agrega, guardar el tipo de educacion
        if (!yaPasoPorAca) {
            EmpresaRegistrar.TipoEducacion = $('#TipoEducacion').val();
            yaPasoPorAca = true;
        }

        //si el tipo de educacion cambió y se intenta agregar otro nivel educativo, limpiar la grilla
        if ($('#TipoEducacion').val() != "SELECCIONE" && $('#TipoEducacion').val() != EmpresaRegistrar.TipoEducacion) {
            $('#listNETE').clearGridData(false);
            EmpresaRegistrar.TipoEducacion = $('#TipoEducacion').val();
        }

        if ($('#TipoEducacion option:selected').text() == "SELECCIONE") {
            Mensaje.Error.texto = "Debe seleccionar un nivel educativo y un tipo educación";
            Mensaje.Error.mostrar();
            return;
        }
        var NETE = {
            id: grid.getGridParam("reccount") + 1,
            idNE: $("#NivelEducativoId").val(),
            nivelEducativo: $('#NivelEducativoId option:selected').text(),
            tipoEducacion: $('#TipoEducacion option:selected').text()
        };
        //verifico q no se quiera agregar un nivel educativo repetido
        if ($("#NivelEducativoId").val() != "") {
            var data = grid.getRowData();
            for (i = 0; i < data.length; i++) {
                if (data[i].idNE === $("#NivelEducativoId").val() && data[i].tipoEducacion === $('#TipoEducacion option:selected').text()) {
                    Mensaje.Error.mostrar("Nivel educativo por tipo educación ya existe");
                    Mensaje.Error.mostrar();
                    return;
                }
            }
            grid.addRowData(NETE.id, NETE, "last");
            $('#NivelEducativoId').val(-1);
            grid.show();
        }
    });

    $("#btnEliminarNETE").click(function () {
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
                    data[i].id = i + 1;
                    grid.addRowData(data[i].id, data[i], "last");
                }
            }
        }
        else {
            AbmcUtil.mensajeSeleccion();
        }
    });


}                         //--------------------- END Grilla Niveles Educativo por Tipo Educacion ------------------------//


//--------------------- Grilla tipo escuelas ------------------------//

EmpresaRegistrar.cargarGrillaTE = function () {

    var grid = $("#listTE").jqGrid({
        datatype: "local",
        colNames: ["id", "IdTE", "Tipos de escuelas"],
        colModel: [
                    { key: true, name: "id", index: "id", align: "left", hidden: true },
                    { key: false, name: "idTE", index: "idTE", align: "left", hidden: true },
                    { key: false, name: "tipoEscuela", index: "tipoEscuela", align: "left" }
                ],
        rowNum: 10,
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: document.body.offsetWidth - 650,
        height: "100%"
    });

    $("#btnAgregarTE").click(function () {
        if ($('#TipoEscuela option:selected').text() == "SELECCIONE") {
            Mensaje.Error.texto = "Debe seleccionar un tipo de escuela";
            Mensaje.Error.mostrar();
            return;
        }
        var TE = {
            id: grid.getGridParam("reccount") + 1,
            idTE: $("#TipoEscuela").val(),
            tipoEscuela: $('#TipoEscuela option:selected').text()
        };
        if ($("#TipoEscuela").val() != "") {
            var data = grid.getRowData();
            for (i = 0; i < data.length; i++) {
                if (data[i].idTE === $("#TipoEscuela").val()) {
                    Mensaje.Error.texto = "El tipo de escuela ya se encuentra cargado";
                    Mensaje.Error.mostrar();
                    return;
                }
            }
            grid.addRowData(TE.id, TE, "last");
            $('#TipoEscuela').val(-1);
            grid.show();
        }
    });

    $("#btnEliminarTE").click(function () {
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
                    data[i].id = i + 1;
                    grid.addRowData(data[i].id, data[i], "last");
                }
            }
        }
        else {
            AbmcUtil.mensajeSeleccion();
        }
    });

}            //--------------------- END Grilla tipo escuelas ------------------------//


//--------------------- Grilla turnos ------------------------//

EmpresaRegistrar.cargarGrillaTutrnos = function () {

    var grid = $("#listTurnos").jqGrid({
        datatype: "local",
        colNames: ["id", "IdTurnos", "Turnos"],
        colModel: [
                    { key: true, name: "id", index: "id", align: "left", hidden: true },
                    { key: false, name: "idTurnos", index: "idTurnos", align: "left", hidden: true },
                    { key: false, name: "turnos", index: "turnos", align: "left" }
                ],
        rowNum: 10,
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: document.body.offsetWidth - 650,
        height: "100%"
    });

    $("#btnAgregarTurno").click(function () {
        if ($('#TurnoId option:selected').text() == "SELECCIONE") {
            Mensaje.Error.texto = 'Debe seleccionar un turno';
            Mensaje.Error.mostrar();
            return;
        }
        var Turno = {
            id: grid.getGridParam("reccount") + 1,
            idTurnos: $("#TurnoId").val(),
            turnos: $('#TurnoId option:selected').text()
        };
        if ($("#TurnoId").val() != "") {
            var data = grid.getRowData();
            for (i = 0; i < data.length; i++) {
                if (data[i].idTurnos === $("#TurnoId").val()) {
                    Mensaje.Error.texto = "El turno " + data[i].turnos + " ya se encuentra agregado";
                    Mensaje.Error.mostrar();
                    return;
                }
            }
            grid.addRowData(Turno.id, Turno, "last");
            $('#TurnoId').val(-1);
            grid.show();
        }
    });

    $("#EstructuraEscolar_Turno").focus(function () {
        EmpresaRegistrar.cargarComboTurnoEstructuraEscolar();
    });

    $("#btnEliminarTurno").click(function () {
        var seleccion = GrillaUtil.getSeleccionFilas(grid, false);
        if (seleccion && seleccion.lenght !== 0) {
            var diagramaciones = $("#listaEstructura").getRowData();
            for (var i = 0; i < diagramaciones.length; i++)
                if (diagramaciones[i].Turno == seleccion) {
                    Mensaje.Error.texto = "No puede eliminarse el turno " + diagramaciones[i].TurnoNombre + " ya que se utiliza en estrucura escolar";
                    Mensaje.Error.mostrar();
                    return;
                }
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
                    data[i].id = i + 1;
                    grid.addRowData(data[i].id, data[i], "last");
                }
            }
        }
        else {
            AbmcUtil.mensajeSeleccion();
        }
    });

}                         //--------------------- END Grilla turnos ------------------------//

EmpresaRegistrar.escuelaPrivada = function () {
    $("#Privado").changePatch(function () {
        if ($("#Privado").is(":checked")) {
            $(".EsPrivado").show();
            var DirectorYRep = null;
            if (EmpresaRegistrar.estadoText === 'Editar' || EmpresaRegistrar.estadoText === 'Ver') {
                $.ajax({
                    type: 'GET',
                    url: $.getUrl("/GestionEmpresa/GetDatosEscuelaPrivada?empresaId=" + $("#Id").val()),
                    data: null,
                    async: false,
                    success: function (data) {
                        if (data != null) {
                            DirectorYRep = data;
                        }
                    },
                    dataType: 'json'
                });
            }

            //Cargo una la Director en el div de director
            PersonaFisica.cargarPersonaById("#divDirector", "Director", EmpresaRegistrar.estadoText, null);
            $("#divDirector").one("ajaxStop", function () {
                $("#Director_divConsultaPF legend").html("Buscar Director");
                $("#Director_divFormularioPF legend").html("Director");
                if (DirectorYRep) {
                    PersonaFisica.cargarPersonaFisica($('#divDirector').data('persona'), DirectorYRep.director);
                }
            });
            
            //Cargo una la RepresentanteLegal en el div de representante
            PersonaFisica.cargarPersonaById("#divRepresentanteLegal", "RepresentanteLegal", Abmc.estadoText, null);
            $("#divRepresentanteLegal").one("ajaxStop", function () {
                $("#RepresentanteLegal_divConsultaPF legend").html("Buscar Representante Legal");
                $("#RepresentanteLegal_divFormularioPF legend").html("Representante Legal");
                if (DirectorYRep) {
                    PersonaFisica.cargarPersonaFisica($('#divRepresentanteLegal').data('persona'), DirectorYRep.representante);
                }
            });            
        }
        else {
            $(".EsPrivado").hide();
            $('#divDirector').html("");
            $('#divRepresentanteLegal').html("");
        }
    });
}

EmpresaRegistrar.limpiarCampos = function () {
    $(":input[type=text], :input[type=textarea]").val("");
    $(":input[type=checkbox]").removeAttr("checked");
}

EmpresaRegistrar.deshabilitarCampos = function () {
    //$("#btnEdificios, #btnPaquete, #btnEstructura, #btnEmpresaSupervisora").attr("disabled", "disabled");
    $("#btnPaquete, #btnEstructura").attr("disabled", "disabled");
    $("#btnEmpresaSupervisora").hide();
    $("input[name=EmpresaPadre], input[name=EscuelaRaiz]").attr("disabled", "disabled");
    $("input[name=EscuelaMadre], input[name=EmpresaSupervisora]").attr("disabled", "disabled");
    $("input[name=PaquetePresupuestado]").attr("disabled", "disabled");
    $('#NombreDirector').attr('disabled', true);
    $('#ApellidoDirector').attr('disabled', true);
    $('#NumeroDocumentoDirector').attr('disabled', true);
    $('#SexoDirector').attr('disabled', true);
    $('#ApellidoRepresentanteLegal').attr('disabled', true);
    $('#NombreRepresentanteLegal').attr('disabled', true);
    $('#NumeroDocumentoRepresentanteLegal').attr('disabled', true);
    $('#SexoRepresentanteLegal').attr('disabled', true);
    //por cambio de requerimiento, el cod empresa se carga manualmente POR EL MOMENTO hasta q definan la composicion del mismo
    //$("#CodigoEmpresa").attr("disabled", "disabled");
}

EmpresaRegistrar.mostrarCampos = function (tipoEmpresaSelec) {
    switch (tipoEmpresaSelec) {
        case "ESCUELA_MADRE":
            EmpresaRegistrar.cargarParrafos("EMA");
            $(".EscuelaMadreAnexo, .EscuelaMadre").show();
            break;

        case "ESCUELA_ANEXO":
            EmpresaRegistrar.cargarParrafos("EMA");
            $(".EscuelaMadreAnexo, .EscuelaAnexo").show();
            break;

        case "INSPECCION":
            EmpresaRegistrar.cargarParrafos("Inspeccion");
            $(".Inspeccion").show();
            break;

        case "DIRECCION_DE_NIVEL":
            EmpresaRegistrar.cargarParrafos("DN");
            $(".DireccionNivel").show();
            break;

        default:
            EmpresaRegistrar.cargarParrafos("Todos");
            //EmpresaRegistrar.cargarParrafos();

            //$("#botonRegistrarInstrumentoLegal").show();
            break;
    }
}

EmpresaRegistrar.mostrarSelectores = function (tipoEmpresaSelec) {
    //primero oculto todos los selectores para mostrarlos dependiendo del tipo de empresa
    //las solapas 1, 2 y 3 siempre van visibles
    $('#liSolapa4').attr("style", "display: none;");
    $('#liSolapa5').attr("style", "display: none;");
    $('#liSolapa6').attr("style", "display: none;");
    $('#liSolapa7').attr("style", "display: none;");

    if (tipoEmpresaSelec == "ESCUELA_MADRE" || tipoEmpresaSelec == "ESCUELA_ANEXO") {
        $('#liSolapa4').attr("style", "display: block;");
        $('#liSolapa5').attr("style", "display: block;");
        $('#liSolapa6').attr("style", "display: block;");
        $('#liSolapa7').attr("style", "display: block;");
    }
    else if (tipoEmpresaSelec != "MINISTERIO") {
        $('#liSolapa4').attr("style", "display: block;");
    }
}

//agrego id al parrafo q contiene al input con id pidInput
//luego, agrego en el data del body la etiqueta <p> completa
//por ultimo elimino el parrafo
EmpresaRegistrar.obtenerParrafos = function (idInput) {
    $('#' + idInput).parent('p').attr('id', 'p' + idInput);
    var html = $('<div>').append($('#p' + idInput).clone()).remove().html();
    $('body').data('p' + idInput, html);
    $("#p" + idInput).remove();
}

EmpresaRegistrar.cargarParrafos = function (empresa) {
    //recibe la empresa por parametro, 
    //y con eso seleccionamos el div y le hacemos 
    //append obteniendo del data el <p> q corresponde
    $("#CodigoEmpresa").attr("maxlength", 9);
    if ($("#TipoEscuela" + empresa).html() == "")
        $("#TipoEscuela" + empresa).append($('body').data("pTipoEscuela"));
    if ($("#NivelEducativo" + empresa).html() == "")
        $("#NivelEducativo" + empresa).append($('body').data("pNivelEducativoId"));
    if ($("#TipoEducacion" + empresa).html() == "")
        $("#TipoEducacion" + empresa).append($('body').data("pTipoEducacion"));
    if ($("#CodigoEmpresa" + empresa).html() == "")
        $("#CodigoEmpresa" + empresa).append($('body').data("pCodigoEmpresa"));
    if ($("#Nombre" + empresa).html() == "")
        $("#Nombre" + empresa).append($('body').data("pNombre"));
}

//devuelve un string con la vista q se va a usar en la consulta de empresa correspondiente
EmpresaRegistrar.getNuevaVistaEnum = function () {
    var filtroTipoEmpresa;
    switch ($('#TipoEmpresa option:selected').text()) {
        case 'DIRECCION_DE_INFRAESTRUCTURA':
        case 'DIRECCION_DE_RECURSOS_HUMANOS':
        case 'DIRECCION_DE_SISTEMAS':
        case 'DIRECCION_DE_TESORERIA':
            filtroTipoEmpresa = 'SoloMinisterio';
            break;
        case 'SECRETARIA':
            filtroTipoEmpresa = 'BusquedaPorSecretaria';
            break;
        case 'SUBSECRETARIA':
            filtroTipoEmpresa = 'BusquedaPorSubSecretaria';
            break;
        case 'APOYO_ADMINISTRATIVO':
            filtroTipoEmpresa = 'BusquedaPorApoyoAdm';
            break;
        case 'INSPECCION':
            $.ajax({
                url: $.getUrl('/GestionEmpresa/GetFiltroEmpresaPadreInspeccion/?tipoInspeccion=' + $('#TipoInspeccionEnum  option:selected').text()),
                type: "GET",
                async: false,
                success: function (data) {
                    filtroTipoEmpresa = data;
                }
            }
            );
            break;
        case 'DIRECCION_DE_NIVEL':
            filtroTipoEmpresa = 'SoloMinisterio';
            break;
        case 'ESCUELA_MADRE':
            filtroTipoEmpresa = 'BusquedaPorEscuelaMadre';
            break;
        case 'ESCUELA_ANEXO':
            filtroTipoEmpresa = 'BusquedaPorEscuelaAnexo';
            break;
        default:
            break;
    }
    return filtroTipoEmpresa;
};

//vuelve al estado inicial de los consultar empresa creados
EmpresaRegistrar.resetearConsultas = function () {
    if (EmpresaRegistrar.editorConsultarPadre) {
        ConsultarEmpresa.limpiar(EmpresaRegistrar.editorConsultarPadre);
        ConsultarEmpresa.Grilla.limpiar(EmpresaRegistrar.editorConsultarPadre);
        $(EmpresaRegistrar.editorConsultarPadre.prefijo + "divDatosGeneralesEmpresa").hide();
        $(EmpresaRegistrar.editorConsultarPadre.prefijo + "divFiltrosDeBusqueda").show();
    }
    if (EmpresaRegistrar.editorConsultarMadre) {
        ConsultarEmpresa.limpiar(EmpresaRegistrar.editorConsultarMadre);
        ConsultarEmpresa.Grilla.limpiar(EmpresaRegistrar.editorConsultarMadre);
        $(EmpresaRegistrar.editorConsultarMadre.prefijo + "divDatosGeneralesEmpresa").hide();
        $(EmpresaRegistrar.editorConsultarMadre.prefijo + "divFiltrosDeBusqueda").show();
    }
    if (EmpresaRegistrar.editorConsultarRaiz) {
        ConsultarEmpresa.limpiar(EmpresaRegistrar.editorConsultarRaiz);
        ConsultarEmpresa.Grilla.limpiar(EmpresaRegistrar.editorConsultarRaiz);
        $(EmpresaRegistrar.editorConsultarRaiz.prefijo + "divDatosGeneralesEmpresa").hide();
        $(EmpresaRegistrar.editorConsultarRaiz.prefijo + "divFiltrosDeBusqueda").show();
    }
    if (EmpresaRegistrar.editorConsultarEInspeccion) {
        ConsultarEmpresa.limpiar(EmpresaRegistrar.editorConsultarEInspeccion);
        ConsultarEmpresa.Grilla.limpiar(EmpresaRegistrar.editorConsultarEInspeccion);
        $(EmpresaRegistrar.editorConsultarEInspeccion.prefijo + "divDatosGeneralesEmpresa").hide();
        $(EmpresaRegistrar.editorConsultarEInspeccion.prefijo + "divFiltrosDeBusqueda").show();
    }
    if (EmpresaRegistrar.editorConsultarESupervisora) {
        ConsultarEmpresa.limpiar(EmpresaRegistrar.editorConsultarESupervisora);
        ConsultarEmpresa.Grilla.limpiar(EmpresaRegistrar.editorConsultarESupervisora);
        $(EmpresaRegistrar.editorConsultarESupervisora.prefijo + "divDatosGeneralesEmpresa").hide();
        $(EmpresaRegistrar.editorConsultarESupervisora.prefijo + "divFiltrosDeBusqueda").show();
    }
};

EmpresaRegistrar.CargarComboTipoInspeccion = function () {

    $("#divTipoInspeccionEnum").hide();
    EmpresaBloquearEmpresaPadreDeInspeccion = false;
    $("#divTipoInspeccionLista").hide();
    $("#btnEmpresaSupervisora").hide();
    $("#TipoEmpresa").changePatch(function () {
        if ($('#TipoEmpresa option:selected').text() == 'INSPECCION') {
            $.get($.getUrl('/GestionEmpresa/CargarComboTipoInspeccion'), {},
                function (data) {
                    if (data == "True") {
                        $("#divTipoInspeccionEnum").show();
                        $("#divTipoInspeccionEnum").children().show();
                        $("#divTipoInspeccionLista").hide();
                        $("#btnEmpresaPadre").click();
                    }
                    else {
                        $("#divTipoInspeccionEnum").hide();
                        $("#divTipoInspeccionLista").show();
                        $("#divTipoInspeccionLista").children().show();
                        $("#divSeleccionEmpresaPadre").hide();
                        $("#btnEmpresaSupervisora").click();
                    }
                }
            );
        }
        EmpresaRegistrar.mostrarSelectores($("#TipoEmpresa option:selected").text());
    });
};

EmpresaRegistrar.cargarListaTurnos = function () {
    $.getJSON($.getUrl('/GestionEmpresa/GetTurnosByEscuelaId?escuelaId=' + parseInt($('#Id').val())), null,
        function (datos) {
            if (datos) {
                $("#listTurnos").addRowData("id", datos, "last");
            }
        }
    );
    };
    EmpresaRegistrar.cargarListaPeriodosLectivos = function () {
        $.get($.getUrl("/GestionEmpresa/GetPeriodosLectivosPorEscuela/?escuelaId=" + $('#Id').val()), null,
        function (datos) {
            if (datos) {
                $("#listPeriodosLectivos").addRowData("id", datos, "last");
                $('#listPeriodosLectivos').trigger('reloadGrid');

            };
        });
    };

EmpresaRegistrar.cargarListaNETE = function () {
    $.getJSON($.getUrl('/GestionEmpresa/GetNETEByEscuelaId?escuelaId=' + parseInt($('#Id').val())), null,
        function (datos) {
            if (datos) {
                $("#listNETE").addRowData("id", datos, "last");
            }
        }
    );
};

EmpresaRegistrar.cargarListaTE = function () {
    $.getJSON($.getUrl('/GestionEmpresa/GetTiposEscuelaByEmpresaId?empresaId=' + parseInt($('#Id').val())), null,
        function (datos) {
            if (datos) {
                $("#listTE").addRowData("id", datos, "last");
            }
        });
};


EmpresaRegistrar.cargarVinculoEdificio = function () {
    $.getJSON($.getUrl('/GestionEmpresa/GetVinculoEdificio?empresaId=' + parseInt($('#Id').val())), null,
    function (datos) {
        if (datos) {
            $("#listVinculos").addRowData("id", datos, "last");
            //Una vez cargada la grilla de domicilio, seteo la seleccion del domicilio correspondiente a la empresa.
            $("#listDomicilio").setGridParam({
                loadComplete: function (data) {
                    //Seteo de domicilio en la grilla de domicilio
                    EmpresaRegistrar.seleccionarDomicilioEmpresa();
                }
            }).trigger('reloadGrid');
        }
        else {
            //$("#divGrillaVinculos").hide();
        }
    });
};

EmpresaRegistrar.cargarInstrumentosLegales = function () {
    $.getJSON($.getUrl('/GestionEmpresa/GetInstrumentosLegalesByEmpresaId?empresaId=' + parseInt($('#Id').val())), null,
        function (datos) {
            if (datos) {
                $("#listInstrumentosLegales").addRowData("id", datos, "last");
            }
        }
    );
};

EmpresaRegistrar.EstructuraEscolar = function () {

 
    //$("#TipoEmpresa").changePatch();
//    $("#TipoEmpresa").changePatch(function () {
//        EmpresaRegistrar.EstructuraEscuelaEsRequerido = false;
//        if ($('#TipoGestion option:selected').text() == 'ESCUELA' && $('#TipoEmpresa option:selected').text() != "SELECCIONE") {
//            $.get($.getUrl('/GestionEmpresa/ParametroEstructura'), {},
//                function (data) {
//                    if (data == "True") {
//                        $("#divEstructuraEscolar").show();
//                        EmpresaRegistrar.EstructuraEscuelaEsRequerido = true;
//                    }
//                    else {
//                        $("#divEstructuraEscolar").hide();
//                        EmpresaRegistrar.EstructuraEscuelaEsRequerido = false;
//                    }
//                    //$("#TipoEmpresa").trigger("change");
//                }
//            );
//        }
//    });

    $("#RegistrarEstructuraEscolarCheck").changePatch(function () {
        EmpresaRegistrar.cargarComboTurnoEstructuraEscolar();
        if ($("#RegistrarEstructuraEscolarCheck").is(":checked")) {
            $("#grillaEstructura").show();
            $("#divEstructuraDefinitiva").show();
            $("#listaEstructura").setGridWidth(560);
        }

        else {
            $("#divAreaEstructuraEscolar").hide();
            $("#grillaEstructura").hide();
            $("#divEstructuraDefinitiva").hide();
            $("#listaEstructura").setGridWidth(560);
        }
    });
};


//********************* Script de Asignacion escuelas a inspeccion *********************//

EmpresaRegistrar.ModificarAsignacion = {};
EmpresaRegistrar.ModificarAsignacion.init = function () {

    EmpresaRegistrar.editorConsultarAsignacionEscuelas = ConsultarEmpresa.init('SoloEscuelas', '#divConsultarEmpresaAsignacion', 'buscarEscuelasAAsignar', true);
    $(EmpresaRegistrar.editorConsultarAsignacionEscuelas.grilla.id).setGridWidth(580, true);
    $(EmpresaRegistrar.editorConsultarAsignacionEscuelas.grilla.id + "sinRegistros").hide();

    //Si la empresa es inspeccion y ademas es de tipo zonal, q muestre el boton asignar escuela           
    if ($("#TipoEmpresa").val() === "INSPECCION") {
        if ($("#TipoInspeccionEnum").val() === "ZONAL") {
            $("#divAsignacionEscuela").show();
        }
        else {
            $("#divAsignacionEscuela").hide();
        }
    }

    $("#btnAsignarEscuela").click(function () {
        $("#divConsultarEmpresaAsignacion").show();
        $("#divVista").show();
    });

    $("#btnCancelarAsignacion").click(function () {
        GrillaUtil.limpiar($(EmpresaRegistrar.editorConsultarAsignacionEscuelas.grilla.id));
        $("#divConsultarEmpresaAsignacion").hide();
    });

    //ConsultarEmpresa.seleccionarEmpresa(EmpresaRegistrar.editorConsultarAsignacionEscuelas.grilla.getRowData());
};

//********************* FIN Script de Asignacion escuelas a inspeccion *********************//

//SCRIPTS QUE TIENE QUE VER CON VINCULO EDIFICIOS
//Inicializo grilla de Domicilios
EmpresaRegistrar.initDomicilios = function () {
    //Inicio grilla//
    //var controller = "GestionEmpresa";
    //var orderBy = "Id";
    var titulos = ['Id','Identificador Edificio', 'Calle', 'Altura', 'Barrio', 'Localidad'];
    var propiedades = ['Id', 'EntidadId','Calle', 'Altura', 'Barrio', 'Localidad'];
    var tipos = ['integer', null, null, null, null,null];
    var key = 'Id';
    //var url = $.getUrl("/GestionEmpresa/ProcesarBusquedaDomicilio"); //?filtroIdEmpresa="; + $("#Id").val();

    //var grillaDomicilio = Grilla.Detalle.init("#listDomicilio", titulos, propiedades, tipos, key, url, "Domicilio", parseInt($("#Id").val()));
    //fin grilla//
    //$("TD[title='Seleccionar']").hide(); // Borro al choto el botón de selección
    //$('#DivGrillaDomicilio').show(); //Muestro la grilla

    var grid = $('#listDomicilio').jqGrid({ 
        datatype: 'json',
        url: $.getUrl("/GestionEmpresa/ProcesarBusquedaDomicilio/?id="+$("#Id").val()),
        colNames: titulos,
        colModel: GrillaUtil.crearColumnas(propiedades, tipos, key),
        pager: "#pagerDomicilio",
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: document.body.offsetWidth - 650,
        height: "100%",
        emptyrecords: "",
        caption: 'Domicilio'
    });
    $("#listDomicilio").setGridWidth(550, true);
};

//Inicializ grilla de Vínculos
EmpresaRegistrar.initVinculos = function () {
    var titulos = ['Id', 'Id Edificio', 'Identificador Edificio', 'Tipo edificio', 'Fecha desde', 'Observación', 'Determina Domicilio'];
    var propiedades = ['Id', 'IdEdificio', 'IdentificadorEdificio', 'IdTipoEstructuraEdilicia', 'FechaDesde', 'Observacion', 'DeterminaDomicilio'];
    var tipos = ['integer', 'integer', 'string', 'string', 'string', 'string', 'bool'];
    var caption = 'Nuevas Vinculaciones de Edificios a Empresa';
    var key = 'Id';

    var grid = $("#listVinculos").jqGrid({
        datatype: 'local',
        colNames: titulos,
        colModel: GrillaUtil.crearColumnas(propiedades, tipos, key),
        pager: "#pagerVinculos",
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: document.body.offsetWidth - 650,
        height: "100%",
        emptyrecords: "",
        caption: caption,
        loadComplete: function (data) {
            $("#TipoEmpresa").changePatch();
            EmpresaRegistrar.initDomicilios();
        }
    });

    //fin grilla//
    $("#listVinculos").hideCol('IdEdificio');
    $("#listVinculos").hideCol('DeterminaDomicilio');
    $("TD[title='Seleccionar']").hide(); // Borro al choto el botón de selección
    $("#listVinculos").setGridWidth(550, true);
    $('#divGrillaVinculos').show(); //Muestro la grilla
};

//Cargo los combos en cascada de vinculo edifico
EmpresaRegistrar.CargarCombosVinculoEdificio = function () {
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
}

//Inicializo grilla de Edificios
EmpresaRegistrar.initEdificios = function () {
    //Cargo los combos en cascada.
    EmpresaRegistrar.CargarCombosVinculoEdificio();

    var controller = "VinculoEmpresaEdificio";
    var titulos = ['Id', 'Identificador edificio', 'Tipo edificio', 'Estado', 'Funcion edificio'];
    var propiedades = ['Id', 'IdentificadorEdificio', 'TipoEdificio', 'Estado', 'FuncionEdificio'];
    var tipos = ['integer', 'string', 'string', 'string', 'string'];
    var key = 'Id';
    var caption = "Selección Edificio"
    var url = $.getUrl("/VinculoEmpresaEdificio/ProcesarBusquedaEdificio/");

    var grillaEdificios = Grilla.Detalle.init("#listEdificios", titulos, propiedades, tipos, key, url, caption, parseInt($("#Id").val()));

    //fin grilla//
    $('#divConsultaEdificio :input').changePatch(function () {
        var parametrosEdificio = "&TipoEdificioConsulta=" + $("#TipoEdificioConsulta").val()
        + "&IdentificadorEdificioConsulta=" + $("#IdentificadorEdificioConsulta").val()
        + "&funcionEdificio=" + $("#FuncionEdificioConsulta").val() + "&identificadorPredio=" + $("#IdentificadorPredioConsultaEdificio").val()
        + "&DescripcionPredioConsultaEdificio=" + $("#DescripcionPredioConsultaEdificio").val()
        + "&NombreCasaHabitacionConsulta=" + $("#NombreCasaHabitacionConsulta").val()
        + '&FiltroDepartamentoProvincial=' + $('#FiltroDepartamentoProvincial').val()
        + '&FiltroLocalidad=' + $('#FiltroLocalidad').val()
        + '&FiltroBarrio=' + $('#FiltroBarrio').val()
        + '&FiltroCalle=' + $('#FiltroCalle').val()
        + '&FiltroAltura=' + $('#FiltroAltura').val();
        //        if ($("#TipoEdificioConsulta").val() +
        //            $("#IdentificadorEdificioConsulta").val() +
        //            $("#FuncionEdificioConsulta").val() +
        //            $("#IdentificadorPredioConsultaEdificio").val() +
        //            $("#DescripcionPredioConsultaEdificio").val() +
        //            $("#NombreCasaHabitacionConsulta").val() +
        //            $('#FiltroDepartamentoProvincial').val() +
        //            $('#FiltroLocalidad').val() +
        //            $('#FiltroBarrio').val() +
        //            $('#FiltroCalle').val() +
        //            $('#FiltroAltura').val() != "")
        GrillaUtil.setUrl(grillaEdificios, url + parametrosEdificio);
    });
    $("#btnConsultarEdificio").click(function () {
        if ($("#TipoEdificioConsulta").val() +
            $("#IdentificadorEdificioConsulta").val() +
            $("#FuncionEdificioConsulta").val() +
            $("#IdentificadorPredioConsultaEdificio").val() +
            $("#DescripcionPredioConsultaEdificio").val() +
            $("#NombreCasaHabitacionConsulta").val() +
            $('#FiltroDepartamentoProvincial').val() +
            $('#FiltroLocalidad').val() +
            $('#FiltroBarrio').val() +
            $('#FiltroCalle').val() +
            $('#FiltroAltura').val() == "") {
            Mensaje.Error.texto = "Se debe filtrar por al menos un criterio";
            Mensaje.Error.mostrar();
            return;
        }
        grillaEdificios.setGridParam({ datatype: "json" });
        grillaEdificios.trigger("reloadGrid");
        $("#listEdificios").show();
    });
    $("#listEdificios").setGridWidth(550, true);
    $("TD[title='Seleccionar']").hide(); // Borro al choto el botón de selección
    $('#divGrillaEdificios').show(); //Muestro la grilla
    $("#btnLimpiarConsultaEdificio").click(function () {
        GrillaUtil.limpiar($("#listEdificios"));
        $("#divFiltrosDeConsultaEdificio :input").val("");
    });
};

EmpresaRegistrar.agregarVinculo = function (idEdificio) {
    var filaActual = GrillaUtil.getFila($("#listEdificios"), idEdificio);
    var fila = $("#listVinculos").getGridParam("reccount") * (-1); //Fila a insertar, en número negatifo para que no haya problemas de id con los otros objetos
    var tipoEdificio = filaActual.TipoEdificio; //Traigo el tipo de edificio de la grilla
    var identificadorEdificio = filaActual.IdentificadorEdificio; //Traigo el identificador de edificio de la grilla
    var fechaDesde = $("#FechaDesdeVinculo").val();
    var observacion = $("#ObservacionVinculo").val();
    var data = { Id: fila, IdEdificio: idEdificio, IdentificadorEdificio: identificadorEdificio,
        IdTipoEstructuraEdilicia: tipoEdificio, FechaDesde: fechaDesde, Observacion: observacion
    }; // Array con los datos de la fila
    if (fechaDesde == null || fechaDesde == "") { // Checkeamos obligatoriamente la fecha
        Mensaje.Error.texto = "Ingrese la 'Fecha Desde' del Vínculo Empresa a Edificio";
        Mensaje.Error.mostrar();
        return;
    } else {

        if (!Validacion.Fecha({ value: fechaDesde })) {
            Mensaje.Error.texto = "La 'Fecha Desde' del Vínculo Empresa a Edificio tiene un formato invalido";
            Mensaje.Error.mostrar();
            return;
        }
    }
    //$("#listVinculos").jqGrid().addRowData(fila, data);
    var nroColumnaRepetida = -1;
    for (var i = 0; i < EmpresaRegistrar.edificiosAgregados.length; i++) {
        if (EmpresaRegistrar.edificiosAgregados[i] == idEdificio) {
            nroColumnaRepetida = i;
            i = EmpresaRegistrar.edificiosAgregados.length;
        }
    }
    if (nroColumnaRepetida == -1) {
        $("#listVinculos").jqGrid().addRowData(fila, data);
        EmpresaRegistrar.edificiosAgregados[EmpresaRegistrar.edificiosAgregados.length] = idEdificio;
    }
    else {
        $("#listVinculos").jqGrid().setCell(nroColumnaRepetida, "FechaDesde", data.FechaDesde);
        $("#listVinculos").jqGrid().setCell(nroColumnaRepetida, "Observacion", data.Observacion);
    }


    //Limpio los campos
    $("#listVinculos").jqGrid().resetSelection();
    $("#listEdificios").jqGrid().resetSelection();
    $("#FechaDesdeVinculo").val("");
    $("#ObservacionVinculo").val("");

    //Agrego el Domicilio
    EmpresaRegistrar.agregarDomicilio(idEdificio);
};

/** Método que se encarga de borrar de la lista de vínculos alguno seleccionado */
EmpresaRegistrar.borrarVinculo = function () {
    // Traigo el identificador del edificio
    var identificador = GrillaUtil.getFila($("#listVinculos"), GrillaUtil.getSeleccionFilas($("#listVinculos"))).IdentificadorEdificio;
    // Con el identificador traigo el Id de la otra grilla para poder ir a buscar el domicilio
    var idEdificio = GrillaUtil.getFila($("#listVinculos"), GrillaUtil.getSeleccionFilas($("#listVinculos"))).IdEdificio;
    if (idEdificio == -1) {
        Mensaje.Error.texto = "No se encontró el edificio correspondiente al vínculo. No se pudo borrar.";
        Mensaje.Error.mostrar();
        return;
    }
    // Borro el vínculo
    $("#listVinculos").jqGrid().delRowData(GrillaUtil.getSeleccionFilas($("#listVinculos"), false));

    //Mando a borrar el domicilio, si es que fue cargado "manualmente"
    if (EmpresaRegistrar.domiciliosAgregados.length > 0) // Si hay domicilios cargados manualmente
    {
        EmpresaRegistrar.borrarDomicilio(idEdificio);
    }
};

/** Método que trae un domicilio mediante un GET a partir de un edificio y lo inserta en la lista de domicilios si corresponde */
EmpresaRegistrar.agregarDomicilio = function (idEdificio) {
    $.get($.getUrl("/GestionEmpresa/FindDomicilioDeEdificio"), { idEdificio: idEdificio },
 function (data) {
     var domGrilla = GrillaUtil.getFila($("#listDomicilio"), data.Id);
     if (domGrilla == null || domGrilla == "" || jQuery.isEmptyObject(domGrilla)) { //Si el domicilio del edificio no se encuentra en la grilla, lo agrego.
         var id = data.Id;
         var calle = data.Calle;
         var altura = data.Altura;
         var barrio = data.Barrio;
         var EdificioId = idEdificio;
         var localidad;
         $.get($.getUrl("/GestionEmpresa/GetLocalidadString"), { idLocalidad: data.Localidad }, // Traigo el string de la localidad
 function (ret) {
     localidad = ret;
     var data = { Id: id, Calle: calle, Altura: altura, Barrio: barrio, Localidad: localidad, EntidadId: EdificioId }
     $("#listDomicilio").jqGrid().addRowData(id, data);
     // Guardo el Id de los domicilios que haya agregado por acá, para poder borrarlos si quito los vínculos
     EmpresaRegistrar.domiciliosAgregados[EmpresaRegistrar.domiciliosAgregados.length] = id;
 }, "json");
     }
 }, "json");
};

/** Método llamado desde borrar vínculo que borra un domicilio de la lista si este fue cargado manualmente en este UC, si lo trajo de db a partir de los vínculos no lo borra */
EmpresaRegistrar.borrarDomicilio = function (idEdificio) {
    $.get($.getUrl("/GestionEmpresa/FindDomicilioDeEdificio"), { idEdificio: idEdificio }, // Traigo el domicilio
 function (data) {
     for (var i = 0; i < EmpresaRegistrar.domiciliosAgregados.length; i++) { // Si el id del domicilio se encuentra en la lista de agregados, tengo q borrarlo
         if (data.Id == EmpresaRegistrar.domiciliosAgregados[i]) {
             $("#listDomicilio").jqGrid().delRowData(data.Id); // Borro el domicilio
             EmpresaRegistrar.domiciliosAgregados[i] = null;
             return;
         };
     };
 }, "json");
};

//Seteo de domicilio en la grilla de domicilio
EmpresaRegistrar.seleccionarDomicilioEmpresa = function () {
    var vinculos = $("#listVinculos").getRowData();
    var domicilios = $("#listDomicilio").getRowData();
    var identificadorEdificio = null;
    for (var i = 0; i < vinculos.length; i++) {
        if (vinculos[i].DeterminaDomicilio == 'true') {
            identificadorEdificio = vinculos[i].IdEdificio;
        }
    }
    for (var i = 0; i < domicilios.length; i++) {
        if (domicilios[i].EntidadId === identificadorEdificio) {
            $("#listDomicilio").setSelection(domicilios[i].Id);
        }
    }
};