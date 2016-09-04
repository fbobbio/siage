var Mab = {};
/** Variable que mantiene el tipo de novedad del mab (Alta = 1, Baja = 2, Movimiento = 3, Ausentismo = 4)*/
Mab.TipoNovedadMab = {};
Mab.IdEmpresaUsuarioLogueado;
//var idAsignacionSeleccionada;
Mab.idAgenteSeleccionado;
Mab.idAgenteReemplazado = null;
Mab.idPuestoSeleccionado;
Mab.idPuestoAnteriorSeleccionado;
Mab.idSucursalBancaria;
Mab.estadoText;


Mab.prefijoConsultarAgenteReemplazado = "MabEditorCAReemplazado";
Mab.prefijoConsultarAgentePuestoTrabajoSeleccionado = "MabEditorCAPuestoTrabajoSeleccionado";
Mab.instanciaConsultarAgentePuestoTrabajoSeleccionado;
Mab.prefijoConsultarPT = "MabEditorPT";
Mab.instanciaConsultarPT;
Mab.prefijoConsultarPTAnterior = "MabEditorPTAnterior";
Mab.instanciaConsultarPTAnterior;

Mab.instanciaConsultarAgenteReemplazado;
Mab.instanciaAgenteConsultar;
Mab.instanciaInstrumentoLegal;

Mab.isPuestoRelacionadoAPlanEstudio = false;

Mab.init = function (estado) {
    Mab.estadoText = estado;

    $("#CodigoSucursalBancaria").numeric();

    //Inicializo la fecha actual
    Mab.initFecha();
    //Inicialización de editores
    Mab.initEditores();
    //Deshabilitar campos de muestra
    Mab.deshabilitarCampos();

    //Manejo de eventos
    Mab.manejoDeEventos();

    //Mostrar datos de Empresa del usuario logueado, va a ser una escuela en este caso
    Mab.mostrarDatosEmpresaUsuarioLogueado();

    //Verifico el estado text.
    Mab.verificarEstadoText();
};

Mab.manejoDeEventos = function () {
    //Botón de selección de novedad
    $("#btnSeleccionarNovedad").click(function () {
        if ($('#TipoNovedadId').val() != "") {
            $('#btnAceptar').show();
        }
        //Selecciono tipo de novedad de MAB (Alta, baja, movimiento, ausentismo) y lo guardo en variable global
        Mab.cargarSeleccionTipoNovedadMab();
        $("#divGrillaAbmc").hide();
    });

    //Evento de volver del agente
    $(Mab.instanciaAgenteConsultar.prefijo + "_btnVolverAgente").click(function () {
        Mab.idAgenteSeleccionado = undefined;
    });

    //Botón de modificación de domicilio agente
    $("#btnModificarDomicilioAgente").click(function () {
        Mab.modificarDomicilioAgente();
    });

    //Selección del tipo de movimiento del puesto de trabajo
    $("#btnSeleccionarMovimiento").click(function () {
        if ($("#rdbPuestoActual").attr("checked") || $("#rdbPuestoNuevo").attr("checked")) {
            if (!Mab.idAgenteSeleccionado) {
                alert("Debe seleccionarse el agente");
                return;
            }
            $("#rdbPuestoActual").attr("disabled", true);
            $("#rdbPuestoNuevo").attr("disabled", true);
            $("#btnSeleccionarMovimiento").attr("disabled", true);
            $("#divOcultarPorMovimiento").show();

            var idSituacionDeRevista;
            if ($(Mab.instanciaAgenteConsultar.prefijo + "FiltroSituacionRevista option:selected").val() != "") {
                idSituacionDeRevista = $(Mab.instanciaAgenteConsultar.prefijo + "FiltroSituacionRevista option:selected").val();
                Mab.cargarSituacionDeRevista(Mab.instanciaConsultarPT, idSituacionDeRevista);


            }

            if ($("#rdbPuestoActual").attr("checked")) { //Si se seleccionó puesto de trabajo actual
                PuestoDeTrabajoConsultar.setFiltroAgente(Mab.instanciaConsultarPT, Mab.idAgenteSeleccionado);
                PuestoDeTrabajoConsultar.setFiltroEstadoPuestoActual(Mab.instanciaConsultarPT, true);
                Mab.cargarDniAgenteAConsultaPT();
                Mab.eliminarEstadoVacanteDelPuesto();
                //Falta filtrar que no traiga los vacantes
            }
        }
        else {
            alert("No se ha seleccionado la opción de movimiento");
        }
    });

    Mab.cargarSituacionDeRevista = function (intanciaConsultaPT, idSituacionDeRevista) {
        if (idSituacionDeRevista) {
            $(intanciaConsultaPT.prefijo + "fltSituacionRevista").attr("disabled", true);
            $(intanciaConsultaPT.prefijo + "fltSituacionRevista option[value='" + idSituacionDeRevista + "']").attr("selected", true);
            PuestoDeTrabajoConsultar.setFiltroSituacionDeRevista(intanciaConsultaPT, idSituacionDeRevista);
        }
    }

    //Evento de selección de puesto de trabajo
    PuestoDeTrabajoConsultar.seleccionarPT = function (instancia) {
        if (instancia.seleccion > 0) {
            if (instancia == Mab.instanciaConsultarPT) //Si es de la consulta del agente
            {
                Mab.idPuestoSeleccionado = instancia.seleccion;
                Mab.seleccionarPuestoDeTrabajo();
            }
            if (instancia == Mab.instanciaConsultarPTAnterior) //Si es de la consulta del agente reemplazado
            {
                Mab.idPuestoAnteriorSeleccionado = instancia.seleccion;
                //Mab.getInstrumentoLegalDeAgenteReemplazado();
            }
        }
    };

    $("#CargarInstrumentoLegalCheck").changePatch(function () {
        if ($("#CargarInstrumentoLegalCheck").is(":checked")) {
            $("#divCargarInstrumentoLegal").show();
            $("#divInstrumentoLegalPendiente").hide();
        }
        else {
            $("#divCargarInstrumentoLegal").hide();
            $("#divInstrumentoLegalPendiente").show();
        }
    });

    //Evento de selección de sucursal bancaria por grilla
    onSelectSucursal = function (idSucursal) {
        mostrarSucursalBancaria(idSucursal);
        if (idSucursal > 0) {
            $('#divExisteSucursalBancaria').show();
            $('#divSeleccionSucursal').hide();
        }
    }
    //Evento de selección de sucursal bancaria por textbox
    $("#btnCargarSucursal").click(function () {
        var sucursal = $("#CodigoSucursalBancaria").val();
        if (sucursal != null && sucursal != "" && sucursal > 0) {
            mostrarSucursalBancaria(sucursal);
            if (Mab.idSucursalBancaria > 0) {
                $('#divExisteSucursalBancaria').show();
                $('#divSeleccionSucursal').hide();
            }
        }
        else {
            alert("El código de sucursal bancaria ingresado es incorrecto");
        }
    });

    //Evento de checkbox de empresa y cargo anterior
    $("#RegistrarCargoAnterior").changePatch(function () {
        if ($("#RegistrarCargoAnterior").is(":checked")) {
            $("#divSeleccionEmpresaMinisterio").show();
        }
        else {
            $("#divSeleccionEmpresaMinisterio").hide();
            $("#EsCargoDeEmpresaMinisterio").attr("checked", false);
            $("#divEsEmpresaDelMinisterio").hide();
            $("#divNoEsEmpresaDelMinisterio").show();
        }
    });

    //Muestro y oculto las formas de seleccionar la sucursal bancaria de acuerdo al evento del check relacionado.
    $("#SeleccionarSucursalBancaria").attr('checked', false);
    $("#divGrillaSucursalBancaria").hide();
    $("#SeleccionarSucursalBancaria").changePatch(function () {
        if ($("#SeleccionarSucursalBancaria").is(":checked")) {
            $("#divGrillaSucursalBancaria").show();
            $("#divCodigoSucursal").hide();
        }
        else {
            $("#divCodigoSucursal").show();
            $("#divGrillaSucursalBancaria").hide();
        }
    });

    //Verifico si la empresa del cargo anterior pertenece al ministerio, mostrando u ocultado divs.
    $("#EsCargoDeEmpresaMinisterio").changePatch(function () {
        if ($("#EsCargoDeEmpresaMinisterio").is(":checked")) {
            $("#divEsEmpresaDelMinisterio").show();
            $("#divNoEsEmpresaDelMinisterio").hide();
        }
        else {
            $("#divEsEmpresaDelMinisterio").hide();
            $("#divNoEsEmpresaDelMinisterio").show();
        }
    });

    //Valido las fechas de novedad (desde/hasta)
    $('#FechaNovedadHasta').blur(function () {
        Mab.validarFechasDeNovedadHastaMayorFechaNovedadDesde();
    });

    //De acuerdo a la situacion de revista seleccionada, si el mab es de alta, determinar funcionalidad de las fechas desde y hasta.
    $("#SituacionRevistaId").changePatch(function () {
        $("#FechaNovedadDesde").removeClass("val-Required");
        $("#FechaNovedadHasta").removeClass("val-Required");
        $("label[for='FechaNovedadDesde']").text("Fecha desde");
        $("label[for='FechaNovedadHasta']").text("Fecha hasta");
        $("#FechaNovedadHasta").show();
        $("label[for='FechaNovedadHasta']").show();

        //Si el mab seleccionado es ALTA
        if ($("#TipoNovedadId").val() === "1") {

            switch ($("#SituacionRevistaId option:selected").text()) {
                case "TITULAR":
                    //Fecha desde requerido
                    $("#FechaNovedadDesde").addClass("val-Required");
                    $("label[for='FechaNovedadDesde']").text("Fecha desde (*)");
                    //Fecha hasta oculto
                    $("#FechaNovedadHasta").val("");
                    $("#FechaNovedadHasta").hide();
                    $("label[for='FechaNovedadHasta']").hide();
                    break;

                case "SUPLENTE":
                    //Fecha desde requerido
                    $("#FechaNovedadDesde").addClass("val-Required");
                    $("label[for='FechaNovedadDesde']").text("Fecha desde (*)");
                    //Fecha hasta requerido
                    $("#FechaNovedadHasta").addClass("val-Required");
                    $("label[for='FechaNovedadHasta']").text("Fecha hasta (*)");
                    break;

                default:
                    break;
            }
        }
    });

    //Si es mab de ausentismo, y seleccione ese codigo de novedad, no permite ingresar fecha hasta.
    $("#CodigoDeNovedadId").changePatch(function () {
        $("#FechaNovedadHasta").show();
        $("label[for='FechaNovedadHasta']").show();
        if ($("#TipoNovedadId").val() === "4") {
            if ($("#CodigoDeNovedadId option:selected").text() === "99 - 7233 LIC. POR CARGO MAYOR JERARQUIA" || $("#CodigoDeNovedadId").val() === "115") {
                //Fecha hasta oculto
                $("#FechaNovedadHasta").val("");
                $("#FechaNovedadHasta").hide();
                $("label[for='FechaNovedadHasta']").hide();
            }
        }
    });
};

//Elimino el estado vacante del puesto de trabajo.
Mab.eliminarEstadoVacanteDelPuesto = function () {
    $("#MabEditorPT_fltEstadoPuestoDeTrabajo option").each(function () {
        if ($(this).text() == 'VACANTE') {
            $(this).remove();
            return;
        }
    });
};

//Método que inicializa los editores
Mab.initEditores = function () {
    //Consultar agente
    Mab.instanciaAgenteConsultar = AgenteConsultar.init("#divConsultarAgenteMab", "MabEditorCA");
    //Método que redefine el success luego del seleccionarAgente
    Mab.instanciaAgenteConsultar.successSeleccion = function () {
        Mab.seleccionarAgente(Mab.instanciaAgenteConsultar.seleccion);
    };

    //Consultar puesto de trabajo
    Mab.instanciaConsultarPT = PuestoDeTrabajoConsultar.init("#divConsultarPuestoDeTrabajo", Mab.prefijoConsultarPT);
    $("#divPuestoDeTrabajo").width(700);
    $('#' + Mab.prefijoConsultarPT + '_listPuestoDeTrabajo').setGridWidth(650, true);

    Mab.filtroPuestoParaEmpresasNoCerradas(Mab.instanciaConsultarPT);

    //Inicializo el Consultar puesto de trabajo anterior
    Mab.instanciaConsultarPTAnterior = PuestoDeTrabajoConsultar.init("#divEsEmpresaDelMinisterio", Mab.prefijoConsultarPTAnterior);
    $("#divEsEmpresaDelMinisterio").width(700);
    $('#' + Mab.prefijoConsultarPTAnterior + '_listPuestoDeTrabajo').setGridWidth(650, true);

    //Instrumento legal
    Mab.instanciaInstrumentoLegal = AsignacionInstrumentoLegal.init("#divCargarInstrumentoLegal", "AsignacionInstrumentoLegalAgente");

    //Limpio campos de Fecha
    $("#FechaNovedadDesde").val('');
    $("#FechaNovedadHasta").val('');
};

//Seteo el filtro del puesto para que traiga aquellos cuya empresa no está cerrada. (borro del filtro avanzado la opcion de estado CERRADA)
Mab.filtroPuestoParaEmpresasNoCerradas = function (instancia) {
    PuestoDeTrabajoConsultar.setFiltroEstadoEmpresaNoCerrada(instancia, true);
    //Borro del filtro avanzado la opcion de estado empresa CERRADA
    $(instancia.prefijo + "fltEstadoEmpresa option").each(function () {
        if ($(this).text() == 'CERRADA') {
            $(this).remove();
            return;
        }
    });
};


Mab.quitarCamposInstrumentoLegal = function () {
    $("#btnVolverBusquedaInstrumentoLegal").hide();
    $(Mab.instanciaInstrumentoLegal.prefijo + 'RegistrarExpediente').hide();
    $(Mab.instanciaInstrumentoLegal.prefijo + 'RegistrarExpediente').prev().hide();
    $(Mab.instanciaInstrumentoLegal.prefijo + 'Observaciones').hide();
    $(Mab.instanciaInstrumentoLegal.prefijo + 'Observaciones').prev().hide();
};

/** Método que inicializa la fecha */
Mab.initFecha = function () {
    //Limpio los campos porque por defecto el nro de acto adm se iniciaba con 0 y la fecha con 01/01/0001
    $('#NumeroActoAdministrativo').val('');
    $('#FechaActoAdministrativo').val('');

    //Muestro la fecha actual del sistema.
    var fecha = new Date();
    var diames = fecha.getDate();
    var mes = fecha.getMonth() + 1;
    var anio = fecha.getFullYear();
    var diasemana = fecha.getDay();
    var fechaCompleta = diames + "/" + mes + "/" + anio;
    $('#FechaActual').attr('disabled', true);
    $('#FechaActual').val(fechaCompleta);
};

/** Método que deshabilita los campos que son únicamente informativos */
Mab.deshabilitarCampos = function () {
    $('#btnAceptar').hide();
    $('#NroInstrumentoLegalAgenteReemplazado').attr('disabled', true);
    $('#FechaInstrumentoLegalAgenteReemplazado').attr('disabled', true);
    $('#CodigoMotivoAusenciaReemplazado').attr('disabled', true);
    $('#DescripcionMotivoAusenciaReemplazado').attr('disabled', true);
    $('#CodigoEmpresa').attr('disabled', true);
    $('#NombreEmpresa').attr('disabled', true);
    $('#SucursalBancariaText').attr('disabled', true);
    $('#btnModificarDomicilioAgente').hide();
    $(Mab.instanciaAgenteConsultar.prefijo + '_btnVolver').hide();

    //Bloqueo el ingreso de un pipe en las observaciones del cargo anterior
    $("#ObservacionesCargoAnterior").alphanumeric({ ichars: "|" });
};

/** Método que carga el tipo de novedad de MAB (Alta, baja, movimiento, ausentismo) seleccionado y lo guarda en variable global */
Mab.cargarSeleccionTipoNovedadMab = function () {
    // Si seleccioné el tipo de novedad
    if ($("#TipoNovedadId").val() > 0) {
        //Guardo el tipo de novedad
        Mab.TipoNovedadMab = $("#TipoNovedadId").val();
        //Deshabilito y muestro lo correspondiente
        $("#btnSeleccionarNovedad").hide();
        $("#TipoNovedadId").attr("disabled", true);
        $("#divBodyMab").show();
        Mab.cargarGruposCodigoNovedad();
        switch (Mab.TipoNovedadMab) {
            case "1":
                Mab.mabSeleccionadoAlta();
                break;
            case "2":
                Mab.mabSeleccionadoBaja();
                break;
            case "3":
                Mab.mabSeleccionadoMovimiento();
                break;
            case "4":
                Mab.mabSeleccionadoAusentismo();
                break;
            default:
                break;
        }
    }
    else {
        alert("Debe seleccionar un tipo de novedad");
    }
};

/** Método que carga el combo de tipo de novedad según los grupos que correspondan */
Mab.cargarGruposCodigoNovedad = function () {
    $.get($.getUrl("/Mab/GetCodigosNovedadByGrupo"), { idTipoNovedad: Mab.TipoNovedadMab },
            function (data) {
                $("#CodigoDeNovedadId").cargarCombo(data.Codigos, "Id", "Nombre");
            }, "json");
        };

/** Método para manipular la vista de acuerdo al tipo de novedad seleccionado como ALTA */
Mab.mabSeleccionadoAlta = function () {
    //Oculto el div del filtro avanzado, que permite filtrar por agentes dados de baja.
    $(Mab.instanciaAgenteConsultar.prefijo + '_divFiltroAgenteBaja').hide();

    //Valido el parámetro de ingreso de fechas
    /*$.get($.getUrl("/Mab/GetValorParametroFechasMab"), {},
    function (data) {
    if (data) {
    $("#SeleccionarFechasNovedad").attr("disabled", true);
    }
    }, "json");*/
};

/** Método para manipular la vista de acuerdo al tipo de novedad seleccionado como BAJA */
Mab.mabSeleccionadoBaja = function () {
    $('#divDatosCargoAnterior').hide();
    $("#divPuestoDeTrabajo").hide();
    $("#FechaNovedadHasta").attr("disabled", true);
    $("#divSituacionDeRevista").hide();
    //oculto la fecha hasta
    $("#FechaNovedadHasta").prev().hide();
    $("#FechaNovedadHasta").hide();
};

/** Método para manipular la vista de acuerdo al tipo de novedad seleccionado como MOVIMIENTO */
Mab.mabSeleccionadoMovimiento = function () {
    $("#divTipoMovimiento").show();
    $("#divOcultarPorMovimiento").hide();
};

/** Método para manipular la vista de acuerdo al tipo de novedad seleccionado como AUSENTISMO */
Mab.mabSeleccionadoAusentismo = function () {
    $('#divDatosCargoAnterior').hide();
    $("#divPuestoDeTrabajo").hide();
    $("#divSituacionDeRevista").hide();
};

Mab.CargarSituacionRevista = function () {
    $.get($.getUrl("/Mab/GetSituacionRevista"), { idAgente: Mab.idAgenteSeleccionado, idPuesto: Mab.idPuestoSeleccionado },
            function (data) {
                $("#divSituacionDeRevista").show();
                $("#SituacionRevistaId").val(data);
                $("#SituacionRevistaId").attr("disabled", true);
            }, "json");
};

/** Método que carga el dni del agente seleccionado en la consulta de puesto de trabajo para Mab de baja y ausentismo */
Mab.cargarDniAgenteAConsultaPT = function () {
    $.get($.getUrl("/Mab/GetDniAgenteById"), { idAgente: Mab.idAgenteSeleccionado },
    function (data) {
        if (data != null) {
            $("#" + Mab.prefijoConsultarPT + "_fltNumeroDocumento").val(data.NumeroDocumento);
            $("#" + Mab.prefijoConsultarPT + "_fltNumeroDocumento").attr("disabled", true);
            $("#" + Mab.prefijoConsultarPT + "_fltTipoDocumento").val(data.TipoDocumento);
            $("#" + Mab.prefijoConsultarPT + "_fltTipoDocumento").attr("disabled", true);
        }
    }, "json");
};

/** Método que responde al evento de selección de agente */
Mab.seleccionarAgente = function (idAgente) {
    if (idAgente != null) {
        Mab.idAgenteSeleccionado = idAgente;
    }
    else {
        alert("No se seleccionó un agente");
        return;
    }

    var sucursal = Mab.getSucursalBancariaAgente();

    //Muestro el botón de modificar el domicilio
    $('#btnModificarDomicilioAgente').show();

    if (Mab.TipoNovedadMab == 1) { //Si es alta
        //Filtro todos los puestos de trabajo del agente seleccionado
        PuestoDeTrabajoConsultar.setFiltroAgente(Mab.instanciaConsultarPTAnterior, Mab.idAgenteSeleccionado);
    }
    if (Mab.TipoNovedadMab == 3) {//Si es movimiento 
        //Filtro todos los puestos de trabajo del agente seleccionado
        PuestoDeTrabajoConsultar.setFiltroAgente(Mab.instanciaConsultarPTAnterior, Mab.idAgenteSeleccionado);
    }
    if (Mab.TipoNovedadMab == 4 || Mab.TipoNovedadMab == 2) { //Si es de ausentismo o baja filtro los puestos de trabajo de este
        PuestoDeTrabajoConsultar.setFiltroAgente(Mab.instanciaConsultarPT, Mab.idAgenteSeleccionado);
        Mab.cargarDniAgenteAConsultaPT();
        $("#divPuestoDeTrabajo").show();
        //TODO: setear el documento y el tipo agente en el filtro del puesto de trabajo
        $("#divDatosCargoAnterior").html("");
    }
};

mostrarSucursalBancaria = function (idSucursal) {
    Mab.idSucursalBancaria = idSucursal;
    Mab.armarStringSucursal(Mab.idSucursalBancaria);
};
    
/** Método que busca la sucursal bancaria del agente y si no tiene devuelve null*/
Mab.getSucursalBancariaAgente = function () {
    var sucursal = -1;
    $.get($.getUrl("/Mab/GetSucursalBancariaAgente"), { idAgente: Mab.idAgenteSeleccionado },
            function (data) {
                if (data != null) { //Si hay sucursal cargada
                    sucursal = data.Id;
                }
                if (sucursal > 0) { // Si tiene sucursal bancaria, solo muestro
                    $('#divSucursales').show();
                    $('#divExisteSucursalBancaria').show();
                    mostrarSucursalBancaria(sucursal);
                    $('#divSeleccionSucursal').hide();
                }
                else { //Si no tiene sucursal permito la opción de seleccionarla de una grilla o ingresar el código
                    //Inicializo la grilla de sucursales
                    Mab.initGrillaSucursalBancaria();
                    $("#SucursalBancariaText").val('');
                    $('#divSucursales').show();
                    $('#divExisteSucursalBancaria').hide();
                    $('#divSeleccionSucursal').show();
                }
            }, "json");
    return sucursal;
};

Mab.armarStringSucursal = function (idSucursal) {
    var sucursal = null;
    $.get($.getUrl("/Mab/GetSucursalBancaria"), { Id: idSucursal },
            function (data) {
                if (data != null) { //Si hay sucursal cargada
                    sucursal = data;
                    $("#SucursalBancariaText").val(sucursal.Id + " - " + sucursal.Nombre + " " + sucursal.Domicilio);
                    $("#SucursalBancariaText").width(280);
                }
            }, "json");
};

/** Método que trae los datos de la empresa del usuario logueado y los setea */
Mab.mostrarDatosEmpresaUsuarioLogueado = function () {
    $.get($.getUrl("/Mab/GetEmpresaUsuarioLogueado"), {},
    function (data) {
        if (data != null) {
            Mab.IdEmpresaUsuarioLogueado = data.idEmpresa;
            $("#CodigoEmpresa").val(data.codigo);
            $("#NombreEmpresa").val(data.nombre);
        }
        else {
            alert("El usuario logueado no posee una empresa válida asignada");
        }
    }, "json");
};

/** Método que maneja la modificación del domicilio del agente */
Mab.modificarDomicilioAgente = function () {
    //TODO:
};

/** Método que procesa la selección de un puesto de trabajo */
Mab.seleccionarPuestoDeTrabajo = function () {
    //TODO:
    Mab.verificarRelacionPTconPlanEstudio();
    //Mab.mostrarHorarioPuestoDeTrabajo();
    //Mab.verificarItineranteOProlongacion();
    if (Mab.TipoNovedadMab == 1) { //Si es de alta busco el agente reemplazado si hay
        Mab.buscarAgenteReemplazado();
    }
    if (Mab.TipoNovedadMab == 3) { //Si es movimiento
        if ($("#rdbPuestoActual").attr("checked")) {
            Mab.CargarSituacionRevista();
        }
    }
    if (Mab.TipoNovedadMab == 2 || Mab.TipoNovedadMab == 4) {
        //SE SACA ESTO PORQUE LA ASIGNACIÓN PUEDE SER SÓLO 1
        /*$('#divListaAsignacionesRelacionadasAPuestoYAgente').show();
        Mab.initGrillaAsignaciones();*/
        Mab.CargarSituacionRevista();
    }
};

/** Método que me selecciona el agente que está ocupando ese puesto de trabajo actualmente */
Mab.seleccionarAgenteDePuestoDeTrabajo = function () {
    $.get($.getUrl("/Mab/GetAgentePuestoTrabajoSeleccionado"), { idPuesto: Mab.idPuestoSeleccionado, idEmpresa: IdEmpreMab.IdEmpresaUsuarioLogueado },
            function (data) {
                if (data > 0) {
                    Mab.idAgenteSeleccionado = data;
                    AgenteConsultar.seleccionarAgente(Mab.instanciaAgenteConsultar, Mab.idAgenteSeleccionado);
                    $('#divDatosAgente').show();
                }
                else {
                    alert("No se encontró agente ocupando el puesto de trabajo seleccionado");
                }
            }, "json");
};

/** Método que verifica si un puesto de trabajo está relacionado con un plan de estudio o no */
Mab.verificarRelacionPTconPlanEstudio = function () {
    $.get($.getUrl("/Mab/PuestoDeTrabajoHasPlanDeEstudioRelacionado"), { idPuesto: Mab.idPuestoSeleccionado },
        function (data) {
            if (data) { // Si tiene puesto de trabajo relacionado
                $("#divModalidadPuestoTrabajo").show();
                Mab.isPuestoRelacionadoAPlanEstudio = true;
            }
            else { // Si no tiene puesto de trabajo relacionado
                $("#divModalidadPuestoTrabajo").hide();
                Mab.isPuestoRelacionadoAPlanEstudio = false;
            }
        }, "json");
};

/** Método que muesta el horario del puesto de trabajo */
Mab.mostrarHorarioPuestoDeTrabajo = function () {
    //TODO: Diferenciar según si tiene o no Plan de estudio asociado el puesto
    if (Mab.isPuestoRelacionadoAPlanEstudio) { // Si puesto de trabajo está relacionado con plan de estudio, saco el horario de unidad académica
        Mab.cargarHorarioDesdeUnidadAcademica();
    }
    else { // Si no está relacionado saco el horario del puesto de trabajo
        Mab.cargarHorarioDesdePuestoDeTrabajo();
    }
};

    /** Método que carga el horario en la grilla sacándolo de unidades académicas si el puesto está relacionado a plan de estudio */
Mab.cargarHorarioDesdeUnidadAcademica = function () {
    //TODO: traer horarios con GET, cargarlos en la grilla
};

    /** Método que carga el horario en la grilla sacándolo de unidades académicas si el puesto está relacionado a plan de estudio */
Mab.cargarHorarioDesdePuestoDeTrabajo = function () {
    //TODO: traer horarios con GET, cargarlos en la grilla
};

/** Método que verifica si el puesto de trabajo es itinerante o prolongación de jornada */
Mab.verificarItineranteOProlongacion = function () {
    //TODO: validar si es prolongación, itinerante o ninguno. Si es alguno de los primeros, se carga la grilla de prolong/itinerante
};

/** Método que se encarga de buscar el agente al que se está reemplazando en el puesto de trabajo */
Mab.buscarAgenteReemplazado = function () {
    //TODO: buscar el agente por GET y después forzar la ejecución del consultarAgente para el de reemplazado
    $.get($.getUrl("/Mab/GetAgenteReemplazado"), { idPuesto: Mab.idPuestoSeleccionado },
        function (data) {
            if (data != -1) {
                Mab.idAgenteReemplazado = data;
                //TODO: ejecutar el consultar para que se muestren los datos
                AgenteConsultar.init("#divDatosABuscarAgenteReemplazado", Mab.prefijoConsultarAgenteReemplazado);
                //Método que redefine el success luego del seleccionarAgente
                Mab.instanciaConsultarAgenteReemplazado.successSeleccion = function () {
                    Mab.buscarAgenteReemplazado();
                };

                AgenteConsultar.seleccionarAgente(Mab.idAgenteReemplazado);
                $("#divDatosAgenteReemplazado").show();
                $("#divLabelAgenteReemplazadoInexistente").hide();
            }
            else {
                $("#divDatosAgenteReemplazado").hide();
                $("#divLabelAgenteReemplazadoInexistente").show();
            }
        }, "json");
};

/** Método que inicializa la grilla de sucursal bancaria para que pueda seleccionarse una */
Mab.initGrillaSucursalBancaria = function () {
    var controller = "Mab";
    var orderBy = "Id";
    var titulos = ['Id', 'Código', 'Nombre', 'Domicilio'];
    var propiedades = ['Id', 'Codigo', 'Nombre', 'Domicilio'];
    var tipos = ['integer', null, null, null];
    var key = 'Id';
    var url = $.getUrl("/Mab/ProcesarBusquedaSucursales");

    var grillaSucursal = Grilla.Seleccion.init("#listSucursales", titulos, propiedades, tipos, key, url, "Sucursales bancarias", onSelectSucursal, false, "#pagerSelect");
};

Mab.armarStringAsignacion = function (idAsignacion) {
    $.get($.getUrl("/Mab/GetAsignacion"), { Id: idAsignacion },
            function (data) {
                if (data != null) {
                    $("#txtAsignacionSeleccionada").val(data.Id + " - " + data.Agente.Persona.Nombre + " " + data.Agente.Persona.Apellido + " " + data.PuestoDeTrabajo.TipoCargo.Nombre);
                    $("#txtAsignacionSeleccionada").width(310);
                }
            }, "json");
};

/** Método que busca los datos del instrumento legal del agente reemplazado y los carga en el div correspondiente */
Mab.getInstrumentoLegalDeAgenteReemplazado = function () {
    $.get($.getUrl("/Mab/GetInstrumentoLegalAgenteReemplazado"), { Id: Mab.idPuestoSeleccionado },
            function (data) {

            }, "json");
};

/** Método para validar que la fecha hasta sea mayor a la fecha desde */
Mab.validarFechasDesdeHasta = function (fechaDesde, fechaHasta) {
    var fechaIni = fechaDesde.split("/");
    var fechaFin = fechaHasta.split("/");
    if (parseInt(fechaIni[2], 10) > parseInt(fechaFin[2], 10)) {
        return (true);
    }
    else {
        if (parseInt(fechaIni[2], 10) == parseInt(fechaFin[2], 10)) {
            if (parseInt(fechaIni[1], 10) > parseInt(fechaFin[1], 10)) {
                return (true);
            }
            if (parseInt(fechaIni[1], 10) == parseInt(fechaFin[1], 10)) {
                if (parseInt(fechaIni[0], 10) > parseInt(fechaFin[0], 10)) {
                    return (true);
                }
                else {
                    return (false);
                }
            } else {
                return (false);
            }
        } else {
            return (false);
        }
    }
};

/** Valido las fechas de novedad (desde/hasta) */
Mab.validarFechasDeNovedadHastaMayorFechaNovedadDesde = function () {
    var fechaDesde = $("#FechaNovedadDesde").val();
    var fechaHasta = $("#FechaNovedadHasta").val();
    if (Mab.validarFechasDesdeHasta(fechaDesde, fechaHasta)) {
        alert("Fecha incorrecta, por favor ingrese una fecha posterior a la fecha desde.");
        $("#FechaNovedadHasta").val("");
        $("#FechaNovedadHasta").focus();
    }
};

/** Valido que se ingrese la fecha hasta solo si la vacante es por reemplazo (o existe agente reemplazado)
y la situacion de revista no es titular*/
Mab.validarFechaDeNovedadHasta = function () {
    $('#FechaNovedadHasta').hide();
    if ((Mab.idAgenteReemplazado > 0) && ($('#SituacionRevistaId option:selected').text() != 'TITULAR')) {
        $('#FechaNovedadHasta').show();
    }
};

/** Obtengo el Mab seleccionado a partir del mab seleccionado que le llega por parámetro */
Mab.obtenerMabSeleccionado = function (idMabSeleccionado) {
    $.get($.getUrl("/Mab/GetMabById"), { idMab: idMabSeleccionado },
        function (data) {
            if (data != null) {
                //Obtengo el agente seleccionado y lo muestro
                Mab.idAgenteSeleccionado = data.IdAgenteMab;
                //Obtengo el puesto de trabajo seleccionado y lo muestro
                Mab.idPuestoSeleccionado = data.IdPuestoDeTrabajoMab;
                if (data.idAgenteReemplazadoMab > 0) {
                    Mab.idAgenteReemplazado = data.idAgenteReemplazadoMab;
                }
                if (data.IdSucursalMab > 0) {
                    Mab.idSucursalBancaria = data.IdSucursalMab;
                }
                if (Mab.idAgenteReemplazado > 0) {
                    AgenteConsultar.agenteSeleccionado(Mab.idAgenteReemplazado);
                }
                Mab.instanciaConsultarPT.seleccion = Mab.idPuestoSeleccionado;
                PuestoDeTrabajoConsultar.onSelectPuestoDeTrabajo(Mab.instanciaConsultarPT);
                AgenteConsultar.seleccionarAgente(Mab.instanciaAgenteConsultar, Mab.idAgenteSeleccionado);

                //Seteo el valor de las fechas que vienen cargadas del model.
                $("#FechaNovedadDesde").attr("disabled", false);
                $("#FechaNovedadDesde").val(data.FechaNovedadDesde);
                $("#FechaNovedadDesde").attr("disabled", true);

                $("#FechaNovedadHasta").attr("disabled", false);
                $("#FechaNovedadHasta").val(data.FechaNovedadHasta);
                $("#FechaNovedadHasta").attr("disabled", true);

                $("#divBodyMab").one("ajaxStop", function () {
                    if (data.CodigoNovedadId != null) {
                        $("#CodigoDeNovedadId").val(data.CodigoNovedadId);
                        $("#CodigoDeNovedadId").attr("disabled", true);
                    }
                    if (Mab.estadoText === "Editar") {
                        $("#ObservacionesMab").attr("disabled", false);
                        $("#ObservacionesCargoAnterior").attr("disabled", true);
                        $("#SituacionRevistaId").attr("disabled", true);
                    }

                    if (data.FechaNovedadDesde != "") {
                        $("#FechaNovedadDesde").attr("disabled", true);
                    }
                    else {
                        $("#FechaNovedadDesde").attr("disabled", false);
                    }
                    if (data.FechaNovedadHasta != "") {
                        $("#FechaNovedadHasta").attr("disabled", true);
                    }
                    else {
                        $("#FechaNovedadHasta").attr("disabled", false);
                    }

                    if (data.ObservacionesMab != "") {
                        $("#ObservacionesMab").val(data.ObservacionesMab);
                    }
                    if (data.ObservacionesCargoAnterior != "" || data.IdPuestoAnteriorDelMinisterio > 0) {
                        $("#RegistrarCargoAnterior").attr("disabled", true);
                        $("#RegistrarCargoAnterior").attr("checked", true);
                        $("#RegistrarCargoAnterior").changePatch();
                        if (data.ObservacionesCargoAnterior != "") {
                            $("#ObservacionesCargoAnterior").val(data.ObservacionesCargoAnterior);
                            $("#EsCargoDeEmpresaMinisterio").attr("disabled", true);
                        }
                        else {
                            $("#EsCargoDeEmpresaMinisterio").attr("checked", true);
                            $("#EsCargoDeEmpresaMinisterio").changePatch();
                            Mab.instanciaConsultarPTAnterior.seleccion = data.IdPuestoAnteriorDelMinisterio;
                            PuestoDeTrabajoConsultar.onSelectPuestoDeTrabajo(Mab.instanciaConsultarPTAnterior);
                        }
                    }
                    else {
                        $("#RegistrarCargoAnterior").attr("disabled", true);
                    }
                    /*
                    if (data.IdAsignacionInstrumentoLegalAgente > 0) {
                    $("#CargarInstrumentoLegalCheck").attr("checked", true);
                    $("#CargarInstrumentoLegalCheck").changePatch();
                    $("#btnLimpiarCamposInstrumento").hide();
                    $("#divInstrumentoLegalMab :input").attr("disabled", true);
                    }
                    else {
                    $("#divCargarInstrumentoLegal").show();
                    $("#CargarInstrumentoLegalCheck").attr("checked", false);
                    $("#CargarInstrumentoLegalCheck").attr("disabled", false);
                    $("#CargarInstrumentoLegalCheck").changePatch();
                    InstrumentoLegal.mostrarBusquedaInstrumento();
                    }*/
                    AsignacionInstrumentoLegal.cargar(Mab.instanciaInstrumentoLegal, data.IdAsignacionInstrumentoLegalAgente);
                });
            }
            else {
                alert("Error al recuperar el mab seleccionado");
            }
        }, "json");
};

/** Manejo del estadoText */
Mab.verificarEstadoText = function () {
    switch (Mab.estadoText) {
        case "Eliminar":
        case "Editar":
        case "Ver":
            //Cuando haga click en ver, que se ejecute el evento click del seleccionar novedad.
            $('#btnSeleccionarNovedad').click();

            $(Mab.instanciaAgenteConsultar.prefijo + "_btnVolverAgente").hide();
            $("#" + Mab.prefijoConsultarPT + "_btnVolver").hide();

            //Obtengo el Id del mab seleccionado.
            var idMabSeleccionado = $("#list").getGridParam("selrow");
            Mab.obtenerMabSeleccionado(idMabSeleccionado);

            if (Mab.TipoNovedadMab == 3) //Si es movimiento oculto la selecciónd el tipo de puesto de trabajo
            {
                $("#divTipoMovimiento").hide();
                $("#divOcultarPorMovimiento").show();
            }
            if (Mab.TipoNovedadMab == 2 || Mab.TipoNovedadMab == 4) //Si es movimiento oculto la selecciónd el tipo de puesto de trabajo
            {
                $("#divPuestoDeTrabajo").show()
            }

            //Si existen datos del cargo anterior muestro los datos (ejecuto el evento change del check asociado).
            if ($('#RegistrarCargoAnterior').is(':checked')) {
                $('#RegistrarCargoAnterior').changePatch();
            }

            //Si existe un instrumento legal asociado, muestro los datos (ejecuto el evento change del check asociado).
            if ($('#CargarInstrumentoLegalCheck').is(':checked')) {
                $('#CargarInstrumentoLegalCheck').changePatch();
                $('#CargarInstrumentoLegalCheck').attr('disabled', true);
                $('#btnLimpiarCamposInstrumento').hide();
                $(Mab.instanciaInstrumentoLegal.prefijo + 'RegistrarExpediente').attr('disabled', true);
            }
            else {
                $('#CargarInstrumentoLegalCheck').attr('disabled', true);
            }

            if (Mab.estadoText === "Editar") {

            }
            break;
        default:
            break;
    }
};

/** Método redefinido que arma por script la estructura necesaria para el model */
Abmc.preEnviarModelo = function (datos) {
    var model = {};
    model.AsignacionAgente = {};
    model.AsignacionAgente.Agente = {};
    model.AsignacionAgente.PuestoDeTrabajo = {};
    model.SucursalBancaria = {};
    if (!Mab.idAgenteSeleccionado) { // si no se seleccionó agente
        model.AsignacionAgente.Agente.Id = 0;
    }
    else {
        model.AsignacionAgente.Agente.Id = Mab.idAgenteSeleccionado;
    }
    if (!Mab.idPuestoSeleccionado) { // si no se seleccionó puesto de trabajo
        model.AsignacionAgente.PuestoDeTrabajo.Id = 0;
    }
    else {
        model.AsignacionAgente.PuestoDeTrabajo.Id = Mab.idPuestoSeleccionado;
    }
    if (!Mab.idSucursalBancaria) { // si no se seleccionó sucursal, cargo el id en cero para que lo valide la regla
        model.SucursalBancaria.Id = 0;
    }
    else {
        model.SucursalBancaria.Id = Mab.idSucursalBancaria;
    }

    if (Mab.idAgenteReemplazado != null && Mab.idAgenteReemplazado > 0) {
        model.AsignacionAgenteReemplazado = {};
        model.AsignacionAgenteReemplazado.Agente = {};
        model.AsignacionAgenteReemplazado.PuestoDeTrabajo = {};
        model.AsignacionAgenteReemplazado.Agente.Id = Mab.idAgenteSeleccionado;
        model.AsignacionAgenteReemplazado.PuestoDeTrabajo.Id = Mab.idPuestoSeleccionado;
    }

    if ($("#RegistrarCargoAnterior").is(":checked") && $("#EsCargoDeEmpresaMinisterio").is(":checked")) //Si el cargo anterior es del ministerio
    {
        //Si no tengo puesto seleccionado
        if (!Mab.idPuestoAnteriorSeleccionado) {
            model.IdPuestoAnteriorDeMinisterio = 0;
        }
        else {
            model.IdPuestoAnteriorDeMinisterio = Mab.idPuestoAnteriorSeleccionado;
        }
    }

    if ($("#TipoNovedadId").val() == 3) //Si es movimiento cargo el tipo de puesto de trabajo
    {
        if ($("#rdbPuestoActual").attr("checked")) {
            model.PuestoTrabajoActualMovimiento = true;
        }
        else {
            model.PuestoTrabajoActualMovimiento = false;
        }
    }

    $.formatoModelBinder(model, datos, "");
};

//Muestro mensaje de confirmación
Mab.enviarModelo = Abmc.enviarModelo;
Abmc.enviarModelo = function () {    
    Mensaje.Advertencia.texto = "¿Desea continuar con la operación?";
    Mensaje.Advertencia.botones = true;
    Mensaje.Advertencia.mostrar();
    Mensaje.Advertencia.cancelar = function () {
        $("#divMensajeAdvertencia").hide();
    };
    Mensaje.Advertencia.aceptar = function () {
        Mab.enviarModelo();
    };
};

Abmc.postEnviarModelo = function (data) {
    if (data && data.status) {
        //Cuando se registra un mab exitosamente, se vuelve a la consulta mostrando el mensaje de éxito.
        if (Mab.estadoText === "Registrar") {
            $("#divAbmc").hide();
            $("#divConsulta").show();
        }
    }
};