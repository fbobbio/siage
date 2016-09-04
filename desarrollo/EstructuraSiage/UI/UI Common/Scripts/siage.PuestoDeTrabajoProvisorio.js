var PuestoDeTrabajoProvisorio = {};
PuestoDeTrabajoProvisorio.prefijo = "PTP";
PuestoDeTrabajoProvisorio.agente = "";
PuestoDeTrabajoProvisorio.PTOrigen = "";
PuestoDeTrabajoProvisorio.PTDestino = "";

PuestoDeTrabajoProvisorio.init = function () {
    //consulta de agente
    AgenteConsultar.init("#divConsultarAgente", "ConsultaAgente");

    //Selección del puesto de trabajo funcional padre
    $("#btnSeleccionarPF").click(function () {
        if ($("#rdbItinerante").attr("checked") || $("#rdbOtrasJurisdicciones").attr("checked")
            || $("#rdbMaestraIntegradora").attr("checked")) {
            if ($("#ConsultaAgente_divConsulta").is(":hidden")) {
                $("#rdbItinerante").attr("disabled", true);
                $("#rdbOtrasJurisdicciones").attr("disabled", true);
                $("#rdbMaestraIntegradora").attr("disabled", true);
                $("#btnSeleccionarPF").attr("disabled", true);

                PuestoDeTrabajoProvisorio.agente = $("#ConsultaAgente_Id").val();
                if ($("#rdbItinerante").attr("checked")) { //Itinerante
                    Abmc.cargarModelo("RegistrarItinerante", null);
                }
                if ($("#rdbOtrasJurisdicciones").attr("checked")) { //Otro
                    Abmc.cargarModelo("RegistrarOtroMinisterio", null);
                }
                if ($("#rdbMaestraIntegradora").attr("checked")) { //Maestra integradora
                    Abmc.cargarModelo("RegistrarMaestraIntegradora", null);
                }
                PuestoDeTrabajoProvisorio.cargarAgente();
               
            }
            else {
                alert("Debe seleccionar un agente");
            }
        }
        else {
            alert("No se ha seleccionado la opción de puesto funcional padre");
        }
    });
};

PuestoDeTrabajoProvisorio.Registrar = {};

PuestoDeTrabajoProvisorio.Registrar.init = function () {
    //Inicializo las fechas
    $(".val-DateTime").mask("99/99/9999", { placeholder: " " });
    $(".val-DateTime").datepicker({
        currentText: 'Now',
        dateFormat: 'dd/mm/yy',
        changeYear: true,
        yearRange: (new Date().getFullYear() - 5) + ":" + (new Date().getFullYear() + 5)
    });
    $("#btnSalir").attr("disabled", false);
    $("#btnSalir").click(function () {
                Abmc.cargarModelo("Registrar", null);

    })
};

//Registrar tareas pasivas
//solo desde MAB:
//PuestoDeTrabajoProvisorio.agente = IdAgente;
//PuestoDeTrabajoProvisorio.PTOrigen = IdPTPadre;
//PuestoDeTrabajoProvisorio.RegistrarTareasPasivas.init()
PuestoDeTrabajoProvisorio.RegistrarTareasPasivas = {};

PuestoDeTrabajoProvisorio.RegistrarTareasPasivas.init = function () {
    PuestoDeTrabajoProvisorio.Registrar.init();
    PuestoDeTrabajoProvisorio.cargarPT();
    PuestoDeTrabajoProvisorio.cargarAgente();
};

// Registrar otro ministerio
PuestoDeTrabajoProvisorio.RegistrarOtroMinisterio = {};

PuestoDeTrabajoProvisorio.RegistrarOtroMinisterio.init = function () {
    //inicializo las fechas
    PuestoDeTrabajoProvisorio.Registrar.init();
  
    // Inicializacion del consultar empresa externa
    PuestoDeTrabajoProvisorio.consultarEmpExterna = ConsultaEmpresaExterna.init("#divEmpresaExterna", "EmpresaExternaId");
    PuestoDeTrabajoProvisorio.consultarEmpExterna.grilla.setGridWidth(600, true);

    // Inicializacion del consultar empresa
    PuestoDeTrabajoProvisorio.consultarEmpresa = ConsultarEmpresa.init("SinVista", "#divEmpresa", "EmpresaId", false);
    PuestoDeTrabajoProvisorio.consultarEmpresa.grilla.setGridWidth(600, true);

    //Check de pt
    $("#checkPTDestino").click(function () {
        if ($("#divPTDestino").is(":hidden")) {
            $('#divTipoCargo').hide();
            $('#divPTDestino').show();
            PuestoDeTrabajoProvisorio.ConfigurarGrillaPTVacantes();
        }
        else {
            $('#divPTDestino').hide();
            $('#divTipoCargo').show();
        }
    });
    //radioB de tipo Cargo
    $("#rdbComun").click(function () {
        if ($("#divTCComun").is(":hidden")) {
            $('#divTCComun').show();
            $('#divTCEspecial').hide();
        }
        else {
            $('#divTCComun').hide();
        }
    });
    $("#rdbEspecial").click(function () {
        if ($("#divTCEspecial").is(":hidden")) {
            $('#divTCEspecial').show();
            $('#divTCComun').hide();
        }
        else {
            $('#divTCEspecial').hide();
        }
    });

    $("#TipoCargoId").change(function () {
        var base = $("#TipoCargoId option:selected").val();
        $.get($.getUrl("/PuestoDeTrabajoProvisorio/GetDetalleTipoCargo/"), { id: base }, function (data) {
            $("#HorasEfectivas").val(data.Horas);
        });
    })
};



// Registrar Itinerante
PuestoDeTrabajoProvisorio.RegistrarItinerante = {};

PuestoDeTrabajoProvisorio.RegistrarItinerante.init = function () {
    //inicializo las fechas
    PuestoDeTrabajoProvisorio.Registrar.init();

    // Inicializacion del consultar empresa
    PuestoDeTrabajoProvisorio.consultarEmpresa = ConsultarEmpresa.init("SinVista", "#divEmpresa", "EmpresaId", false);
    PuestoDeTrabajoProvisorio.consultarEmpresa.grilla.setGridWidth(600, true);
    $("#divPTOrigen").show();
    $("#divDatosPTOrigen").hide();
    
    PuestoDeTrabajoProvisorio.ConfigurarGrillaPTAgente();
    //Selección del puesto de trabajo destino
    $("#btnBuscarPT").click(function () { PuestoDeTrabajoProvisorio.ConfigurarGrillaPTVacantes() });
};

// Registrar Itinerante desde MAB
//PuestoDeTrabajoProvisorio.agente = IdAgente;
//PuestoDeTrabajoProvisorio.PTOrigen = IdPTPadre;
//PuestoDeTrabajoProvisorio.RegistrarItineranteConPT.init()
PuestoDeTrabajoProvisorio.RegistrarItineranteConPT = {};

PuestoDeTrabajoProvisorio.RegistrarItineranteConPT.init = function () {
    //inicializo las fechas
    PuestoDeTrabajoProvisorio.Registrar.init();

    // Inicializacion del consultar empresa
    PuestoDeTrabajoProvisorio.consultarEmpresa = ConsultarEmpresa.init("SinVista", "#divEmpresa", "EmpresaId", false);
    PuestoDeTrabajoProvisorio.consultarEmpresa.grilla.setGridWidth(600, true);

    PuestoDeTrabajoProvisorio.CargarPT();
    //Selección del puesto de trabajo destino
    $("#btnBuscarPT").click(function () { PuestoDeTrabajoProvisorio.ConfigurarGrillaPTVacantes() });
};

// Registrar maestra integradora
PuestoDeTrabajoProvisorio.RegistrarMaestraIntegradora = {};

PuestoDeTrabajoProvisorio.RegistrarMaestraIntegradora.init = function () {
    PuestoDeTrabajoProvisorio.Registrar.init();
    PuestoDeTrabajoProvisorio.consultarInscripcion = ConsultarInscripcion.init('ConsultarInscripcion');
};

PuestoDeTrabajoProvisorio.ConfigurarGrillaPTVacantes = function () {
    var grillaFechas = $("#listPTDestino").jqGrid({
        url: $.getUrl("/PuestoDeTrabajoProvisorio/GetPTVacantesByEmpresa?empresaId=" + $("#EmpresaId_Id").val()),
        datatype: 'json',
        colNames: ['Id', 'Codigo', 'Codigo Tipo Cargo', 'Tipo Cargo', 'Nivel Cargo'],
        colModel: [
                    { name: 'Id', index: 'Id', hidden: true },
                    { name: 'CodigoPosicionPn', index: 'CodigoPosicionPn', hidden: false },
                    { name: 'CodigoPn', index: 'CodigoPn', hidden: false },
                    { name: 'Nombre', index: 'Nombre', hidden: false },
                    { name: 'NivelCargoNombre', index: 'NivelCargoNombre', hidden: false }
                ],
        sortname: "CodigoPosicionPn",
        sortorder: "asc",
        toppager: true,
        viewrecords: true,
        autowidth: true,
        width: document.body.offsetWidth - 650,
        height: "100%",
        caption: "Puestos de trabajo",
        loadtext: 'Cargando, espere por favor',
        emptyrecords: 'No hay datos para mostrar'
    });

    $("#listPTDestino_toppager_center", "#listPTDestino_toppager").remove();
    $(".ui-paging-info", "#listPTDestino_toppager").remove();
    $("#listPTDestino_toppager_left").append('<table cellspacing="0" cellpadding="0" border="0" style="float: left; table-layout: auto;" class="ui-pg-table navtable"><tbody><tr></tr></tbody></table>');
    $('#listPTDestino').setGridWidth(650, true);
    $("#listPTDestino").navButtonAdd("#listPTDestino_toppager_left",
    {
        id: "grid_button_seleccionar",
        position: "last",
        caption: "Seleccionar",
        title: "Seleccionar",
        buttonicon: "ui-icon-pin-s",
        onClickButton: function () {
            PuestoDeTrabajoProvisorio.CargarPTDestinoSeleccionado();
        }
    });
    $('#listPTDestino').trigger('reloadGrid');
};

PuestoDeTrabajoProvisorio.ConfigurarGrillaPTAgente = function () {
    var grillaFechas = $("#listPTOrigen").jqGrid({
        url: $.getUrl("/PuestoDeTrabajoProvisorio/GetPTOrigenByAgente?agenteId=" + PuestoDeTrabajoProvisorio.agente),
        datatype: 'json',
        colNames: ['Id', 'Codigo', 'Codigo Tipo Cargo', 'Tipo Cargo', 'Nivel Cargo'],
        colModel: [
                    { name: 'Id', index: 'Id', hidden: true },
                    { name: 'CodigoPosicionPn', index: 'CodigoPosicionPn', hidden: false },
                    { name: 'CodigoPn', index: 'CodigoPn', hidden: false },
                    { name: 'Nombre', index: 'Nombre', hidden: false },
                    { name: 'NivelCargoNombre', index: 'NivelCargoNombre', hidden: false }
                ],
        sortname: "CodigoPosicionPn",
        sortorder: "asc",
        toppager: true,
        viewrecords: true,
        autowidth: true,
        width: document.body.offsetWidth - 650,
        height: "100%",
        caption: "Puestos de trabajo",
        loadtext: 'Cargando, espere por favor',
        emptyrecords: 'No hay datos para mostrar'
    });

    $("#listPTOrigen_toppager_center", "#listPTOrigen_toppager").remove();
    $(".ui-paging-info", "#listPTOrigen_toppager").remove();
    $("#listPTOrigen_toppager_left").append('<table cellspacing="0" cellpadding="0" border="0" style="float: left; table-layout: auto;" class="ui-pg-table navtable"><tbody><tr></tr></tbody></table>');
    $('#listPTOrigen').setGridWidth(650, true);
    $("#listPTOrigen").navButtonAdd("#listPTOrigen_toppager_left",
    {
        id: "grid_button_seleccionar",
        position: "last",
        caption: "Seleccionar",
        title: "Seleccionar",
        buttonicon: "ui-icon-pin-s",
        onClickButton: function () {
            PuestoDeTrabajoProvisorio.CargarPTOrigenSeleccionado();
        }
    });
    $('#listPTOrigen').trigger('reloadGrid');
};



Abmc.preEnviarModelo = function (datos) {
    var model = {
        PuestoTrabajoFuncionalPadre: PuestoDeTrabajoProvisorio.PTOrigen,
        PuestoTrabajoFuncionalDestino: PuestoDeTrabajoProvisorio.PTDestino,
        EstudianteId: $("#VerEstudiante_Id").val(),
        AgenteEspecialId: PuestoDeTrabajoProvisorio.agente,
        InscripcionId: $("#VerInscripcion_Id").val()
    };
    var empresa = $("#EmpresaId_Id").val();
    if (empresa != 'null') {
        model.EmpresaId = empresa;
    }
    var empresa = $("#EmpresaExternaId_Id").val();
    if (empresa != 'null') {
        model.EmpresaExternaId = empresa;
    }
    $.formatoModelBinder(model, datos, "");
    $("#btnSalir").attr("disabled", false);
};

Abmc.postEnviarModelo = function (data) {
    if (data && data.status) {
        $("#btnSalir").attr("disabled", false);
    }
};

PuestoDeTrabajoProvisorio.cargarPT = function () {
    $("#divPTOrigen").hide();
    $("#divDatosPTOrigen").show();
    $("#IdPT").val(PuestoDeTrabajoProvisorio.PTOrigen);
    $.get($.getUrl("/PuestoDeTrabajo/GetDetalle/"), { id: PuestoDeTrabajoProvisorio.PTOrigen }, function (data) {
        $("#VerCodigoCargo").val(data.CodigoCargo);
        $("#VerNombreCargo").val(data.NombreCargo);
        $("#VerHoras").val(data.Horas);
        $("#VerPlanEstudio").val(data.PlanEstudio);
        $("#VerMateria").val(data.Materia);
        $("#VerTurno").val(data.Turno);
        $("#VerGradoAnio").val(data.GradoAnio);
        $("#VerSeccionDivision").val(data.SeccionDivision);
        $("#VerCupof").val(data.Cupof);
    });
};

PuestoDeTrabajoProvisorio.CargarPTOrigenSeleccionado = function () {
    var gr = jQuery("#listPTOrigen").jqGrid('getGridParam', 'selrow');
        if (gr != null) {
            var seleccion = $("#listPTOrigen").getRowData(parseInt($("#listPTOrigen").getGridParam("selrow")));
            PuestoDeTrabajoProvisorio.PTOrigen= seleccion.Id;
        }
        else {
            Mensaje.Error.texto = "Seleccione un puesto de trabajo.";
            Mensaje.Error.mostrar();
        }
};

PuestoDeTrabajoProvisorio.CargarPTDestinoSeleccionado = function () {
    var gr = jQuery("#listPTDestino").jqGrid('getGridParam', 'selrow');
        if (gr != null) {
            var seleccion = $("#listPTDestino").getRowData(parseInt($("#listPTDestino").getGridParam("selrow")));
            PuestoDeTrabajoProvisorio.PTDestino= seleccion.Id;
        }
        else {
            Mensaje.Error.texto = "Seleccione un puesto de trabajo.";
            Mensaje.Error.mostrar();
        }
};

PuestoDeTrabajoProvisorio.cargarAgente = function () {

    $("#IdAgente").val(PuestoDeTrabajoProvisorio.agente);
    $.get($.getUrl("/PuestoDeTrabajoProvisorio/GetDetalleAgente/"), { id: PuestoDeTrabajoProvisorio.agente }, function (data) {
        $("#VerLegajo").val(data.Legajo);
        $("#VerNombre").val(data.Nombre);
        $("#VerApellido").val(data.Apellido);
        $("#VerSexo").val(data.Sexo);
        $("#VerTipoAgente").val(data.TipoAgente);
        $("#VerTipoDocumento").val(data.TipoDocumento);
        $("#VerNroDocumento").val(data.NroDocumento);

    });
};

PuestoDeTrabajoProvisorio.Eliminar = function () {
    $('#divTareaPasiva').hide();
    $('#divProfesorItinerante').hide();
    $('#divMaestraIntegradora').hide();
    $('#divOtrasJurisdicciones').hide();
    if ($("#TipoPuestoProvisorio").val() = "TAREAS PASIVAS") {
        $('#divTareaPasiva').show();
    }
    if ($("#TipoPuestoProvisorio").val() = "COMPLETA HORAS") {
        $('#divProfesorItinerante').show();
    }
    if ($("#TipoPuestoProvisorio").val() = "ALTA DOCENTE INTEGRADOR") {
        $('#divMaestraIntegradora').show();
    }
    if ($("#TipoPuestoProvisorio").val() = "PASE DE OTRAS JURISDICCIONES O MINISTERIOS") {
        $('#divOtrasJurisdicciones').show();
    }

};


PuestoDeTrabajoProvisorio.cargarPTEliminar = function () {

    $("#IdPT").val(PuestoDeTrabajoProvisorio.PTOrigen);

};

PuestoDeTrabajoProvisorio.Eliminar.init = function () {

    $(".val-DateTime").mask("99/99/9999", { placeholder: " " });
    $(".val-DateTime").datepicker({
        currentText: 'Now',
        dateFormat: 'dd/mm/yy',
        changeYear: true,
        yearRange: (new Date().getFullYear() - 5) + ":" + (new Date().getFullYear() + 5)
    });

    $("#btnSalir").click(function () {
        $("#divConsulta").show();
        $("#divAbmc", "#divGrillaAbmc").hide();
        Mensaje.ocultar();
    })
};