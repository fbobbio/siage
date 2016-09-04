var CalendarioEscolar = {};

CalendarioEscolar.init = function () {
    conInstrumentoLegal = false;
    $("#btnLimpiarFecha").click(function () {
        CalendarioEscolar.LimpiarCamposFechas();
    });

    $("#btnAgregarFecha").click(function () {
        if (CalendarioEscolar.AgregarFecha()) {
            CalendarioEscolar.LimpiarCamposFechas();
        }
    });

    $("#rdbEtapa").click(function () {
        if ($("#divEtapa").is(":hidden")) {
            $('#divEtapa').show();
            $('#divProceso').hide();
            $('#divConcepto').hide();
        }
        else {
            $('#divEtapa').hide();
        }
    });

    $("#rdbProceso").click(function () {
        if ($("#divProceso").is(":hidden")) {
            $('#divEtapa').hide();
            $('#divProceso').show();
            $('#divConcepto').hide();

        }
        else {
            $('#divProceso').hide();
        }
    });
    $("#rdbOtroConcepto").click(function () {
        if ($("#divConcepto").is(":hidden")) {
            $('#divEtapa').hide();
            $('#divProceso').hide();
            $('#divConcepto').show();
        }
        else {
            $('#divConcepto').hide();
        }
    });
    ///////////////////// INICIO GRILLA DE FECHAS ///////////////////////////////
    var grillaFechas = $("#listFechas").jqGrid({
        //        url: $.getUrl("/CalendarioEscolar/GetFechasByCicloLectivo?ciclo=" + $("#Id").val()),
        datatype: "fechas",
        colNames: ['Id', 'Fecha inicio', 'Fecha Fin', 'Hora', 'Etapa', 'Proceso', 'Concepto', 'Hábil'],
        colModel: [
                    { name: 'Id', index: 'Id', hidden: true },
                    { name: 'FechaInicio', index: 'FechaInicio', hidden: false },
                    { name: 'FechaFin', index: 'FechaFin', hidden: false },
                    { name: 'Hora', index: 'Hora', hidden: false },
                    { name: 'Etapa', index: 'Etapa', hidden: false },
                    { name: 'Proceso', index: 'Proceso', hidden: false },
                    { name: 'Concepto', index: 'Concepto', hidden: false },
                    { name: 'EsHabil', index: 'EsHabil', hidden: false }
                ],
        sortname: "FechaInicio",
        sortorder: "asc",
        toppager: true,
        viewrecords: true,
        autowidth: true,
        width: document.body.offsetWidth - 650,
        height: "100%",
        caption: "Fechas",
        loadtext: 'Cargando, espere por favor',
        emptyrecords: 'No hay datos para mostrar'
    });

    $("#listFechas_toppager_center", "#listFechas_toppager").remove();
    $(".ui-paging-info", "#listFechas_toppager").remove();
    $("#listFechas_toppager_left").append('<table cellspacing="0" cellpadding="0" border="0" style="float: left; table-layout: auto;" class="ui-pg-table navtable"><tbody><tr></tr></tbody></table>');

    $("#divFechasCalendario").width(700);
    $('#listFechas').setGridWidth(650, true);
    $("#listFechas").navButtonAdd("#listFechas_toppager_left",
    {
        id: "grid_button_editar",
        position: "last",
        caption: "Editar",
        title: "Editar",
        buttonicon: "ui-icon-pencil",
        onClickButton: function () {
            CalendarioEscolar.CargarCamposFecha();
        }
    });
    $("#listFechas").navButtonAdd("#listFechas_toppager_left",
    {
        id: "grid_button_delete",
        position: "last",
        caption: "Eliminar",
        title: "Eliminar",
        buttonicon: "ui-icon-trash",
        onClickButton: function () {
            CalendarioEscolar.QuitarFechaClick();
        }

    });
    $('#listFechas').trigger('reloadGrid');
    CalendarioEscolar.CargarGrillaFechas();
    ///////////////////// FIN GRILLA DE FECHAS ///////////////////////////////

    switch (CalendarioEscolar.EstadoEditor) {
        case "Registrar":
            CalendarioEscolar.Registrar();
            break;
        case "Ver":
            CalendarioEscolar.Ver();
            break;
        case "Editar":
            CalendarioEscolar.Editar();
            break;
        case "Eliminar":
            CalendarioEscolar.Eliminar();
            break;
    }

    $("#A_oCiclo").change(function () {
        var base = $("#A_oCiclo option:selected").val();
        var inicio = base - 1;

        $("#FechaInicio").datepicker("option", "yearRange", inicio + ":" + base);
        $("#FechaFin").datepicker("option", "yearRange", base + ":" + (inicio + 2));
        $("#fechaInicioCalendario").datepicker("option", "yearRange", inicio + ":" + (inicio + 2));
        $("#fechaFinCalendario").datepicker("option", "yearRange", inicio + ":" + (inicio + 2));
    })

    CalendarioEscolar.instrumentoLegal = AsignacionInstrumentoLegal.init("#divAsignacionInstrumentoLegal", "AsignacionInstrumentoLegal");
    $("#divAsignacionInstrumentoLegal").editorOpcional("#checkRegistrarInstrumento");
    $("#checkRegistrarInstrumento").changePatch();
    if ($("#AsignacionInstrumentoLegal_Id").val() > 0) {
        $("#checkRegistrarInstrumento").attr("checked", "checked").changePatch();
        AsignacionInstrumentoLegal.cargar(CalendarioEscolar.instrumentoLegal, $("#AsignacionInstrumentoLegal_Id").val(), "Editar");

    }
}
    //Fin init
CalendarioEscolar.LimpiarCamposFechas = function () {
    $("#IdFecha").val("");
    $("#fechaInicioCalendario").val("");
    $("#fechaFinCalendario").val("");
    $("#Hora").val("");
    $("#Proceso").val("");
    $("#Etapa").val("");
    $("#Concepto").val("");
    $("#EsHabil").val(["EsHabil"]);
    $("#rdbEtapa").val(["rdb"]);
    $("#rdbProceso").val(["rdb"]);
    $("#rdbOtroConcepto").val(["rdb"]);

    $('#divEtapa').hide();
    $('#divProceso').hide();
    $('#divConcepto').hide();
    var base = $("#A_oCiclo option:selected").val();
    var inicio = base - 1;
    $("#fechaInicioCalendario").datepicker("option", "yearRange", inicio + ":" + (inicio + 2));
    $("#fechaFinCalendario").datepicker("option", "yearRange", inicio + ":" + (inicio + 2));
}

CalendarioEscolar.AgregarFecha = function () {
    if ($("#fechaInicioCalendario").val() != ""
            && $("#fechaFinCalendario").val() != ""
            &&
            ($("#Etapa option:selected").val() != 0
            ||
            $("#Proceso option:selected").val() != 0
            ||
             $("#Concepto").val() != ""
            )) {

        if ($("#Hora").val() != "") {
            // regular expression to match required time format
            re = /^\d{1,2}:\d{2}([ap]m)?$/;
            if (!$("#Hora").val().match(re)) {
                Mensaje.Error.texto = "Hora incorrecta (formato hh:mm)";
                Mensaje.Error.mostrar();
                return false;
            }
        }
        Fecha = {};
        Fecha.Id = $("#IdFecha").val();
        Fecha.FechaInicio = $("#fechaInicioCalendario").val();
        Fecha.FechaFin = $("#fechaFinCalendario").val();
        Fecha.Hora = $("#Hora").val();
        if ($("#divEtapa").is(":visible")) {
            Fecha.EtapaId = $("#Etapa option:selected").val();
            Fecha.Etapa = $("#Etapa option:selected").text();
        }
        if ($("#divProceso").is(":visible")) {
            Fecha.ProcesoId = $("#Proceso option:selected").val();
            Fecha.Proceso = $("#Proceso option:selected").text();
        }
        if ($("#divConcepto").is(":visible")) {
            Fecha.Concepto = $("#Concepto").val();
        }

        Fecha.EsHabil = $("#EsHabil").attr("checked");

        var temp = $("#listFechas").data("fechas") || [];
        var datos = temp;
        if (Fecha.Id != 0) {
            for (var i = 0; i < datos.length; i++) {
                if (datos[i].Id == Fecha.Id) {
                    datos[i] = Fecha;
                    break;
                }
            }
        }
        else {
            Fecha.Id = datos.length;
            datos[datos.length] = Fecha;
        }
        
        temp = datos;
        $("#listFechas").data("fechas", temp);
        $("#listFechas").clearGridData();
        for (var item = 0; item < datos.length; item++) {
            $("#listFechas").addRowData(item.Id, datos[item], "last");
        }
        return true;
    }
    else {
        Mensaje.Error.texto = "Los campos son obligatorios.";
        Mensaje.Error.mostrar();
        return false;
    }
}

    //Elimina una fecha
    CalendarioEscolar.QuitarFechaClick = function () {
        var gr = jQuery("#listFechas").jqGrid('getGridParam', 'selrow');
        if (gr != null) {
            //Variable que contiene el registro seleccionado de la grilla de vinculos        
            var seleccion = $("#listFechas").getRowData(parseInt($("#listFechas").getGridParam("selrow")));

            //Variable que contiene todos los datos temporales de la grilla
            var registro = $("#listFechas").data("fechas") || [];
            //Variable en la que se va a guardar todos los datos temporales menos el que se elimina
            var temp = [];
            //Comparo los datos temporales que estan en la grilla y elimino el que este seleccionado
            for (var i = 0; i < registro.length; i++) {
                if (registro[i].Id != seleccion.Id) {
                    temp[temp.length] = registro[i];
                }
            }
            //Borro visualmente de la grilla el seleccionado
            jQuery("#listFechas").delRowData(gr);
            //Cargo en el data de la grilla los nuevos datos temporales sin el que se elimino
            $("#listFechas").data("fechas", temp);
        }
        else {
            Mensaje.Error.texto = "Seleccione una fecha para eliminar.";
            Mensaje.Error.mostrar();
        }
    };

    //Carga los campos de fecha cuando quiero editar
    CalendarioEscolar.CargarCamposFecha = function () {
        var gr = jQuery("#listFechas").jqGrid('getGridParam', 'selrow');
        if (gr != null) {
            var seleccion = $("#listFechas").getRowData(parseInt($("#listFechas").getGridParam("selrow")));

            $("#IdFecha").val(seleccion.Id);
            $("#fechaInicioCalendario").val(seleccion.FechaInicio);
            $("#fechaFinCalendario").val(seleccion.FechaFin);
            $("#Hora").val(seleccion.Hora);
            $("#Proceso").val(seleccion.ProcesoId);
            $("#Etapa").val(seleccion.EtapaId);
            $("#Concepto").val(seleccion.Concepto);
            if (seleccion.EsHabil == "Si")
                $("#EsHabil").val(["EsHabil"]);
        }
        else {
            Mensaje.Error.texto = "Seleccione una fecha para editar.";
            Mensaje.Error.mostrar();
        }
    };

    CalendarioEscolar.preEnviarModelo = AbmcMixto.preEnviarModelo;
    AbmcMixto.preEnviarModelo = function (data) {
        if (CalendarioEscolar.preEnviarModelo != null)
            CalendarioEscolar.preEnviarModelo(data);
        //Bindeo datos de fechas
        var datosFechas = $("#listFechas").data("fechas") || [];
        //cambio los No por false
        for (var i = 0; i < datosFechas.length; i++) {
            if (datosFechas[i].EsHabil == "No") {
                datosFechas[i].EsHabil = false;
            }
            if (datosFechas[i].EsHabil == "Si") {
                datosFechas[i].EsHabil = true;
            }
        }
        $.formatoModelBinder(datosFechas, data, "ItemsCalendario");
        data.push({ name: 'AñoCiclo', value: $("#A_oCiclo option:selected").val() });

    }
    CalendarioEscolar.postEnviarModelo = AbmcMixto.postEnviarModelo;
    AbmcMixto.postEnviarModelo = function (data) {
        if (data && data.status) {
            $('#grid_button_delete').hide();
            $('#grid_button_editar').hide();
        }
      
    }

    CalendarioEscolar.Registrar = function () {
        //Inicializacion de los combos en cascada
        $("#Proceso").CascadingDropDown("#NivelEducativoId", $.getUrl('/CalendarioEscolar/GetProcesosByNivelEducativo'), { promptText: 'SELECCIONE',
            postData: function () {
                return { idNivelEducativo: $("#NivelEducativoId").val() };
            }
        }).attr("disabled", "disabled");
        $("#Etapa").CascadingDropDown("#NivelEducativoId", $.getUrl('/CalendarioEscolar/GetEtapasByNivelEducativo'), { promptText: 'SELECCIONE',
            postData: function () {
                return { idNivelEducativo: $("#NivelEducativoId").val() };
            }
        }).attr("disabled", "disabled");
    }

    CalendarioEscolar.Ver = function () {
        $('#grid_button_delete').hide();
        $('#grid_button_editar').hide();
        $("#divCamposFechasCalendario").hide();
    }

    CalendarioEscolar.Editar = function () {
        //Cargo las fechas del calendario en el editar
        //Valido que se se puede editar
        var temp = $("#listFechas").getRowData();
        $("#listFechas").data("fechas", temp);
        var actual = new Date().getFullYear();
        if ($("#A_oCiclo option:selected").val() < actual) {
            Mensaje.Error.texto = "El ciclo lectivo no es editable";
            Mensaje.Error.mostrar();
            $("#CicloLectivo").attr("disabled", "disabled");
            $('#grid_button_delete').hide();
            $('#grid_button_editar').hide();
            $("#divCamposFechasCalendario").hide();
            $("#divAsignacionInstrumentoLegal").hide();

        }
        if ($("#A_oCiclo option:selected").val() == actual) {
            $("#CicloLectivo").attr("disabled", "disabled");
        }

        var base = $("#A_oCiclo option:selected").val();
        var inicio = base - 1;
        $("#fechaInicioCalendario").datepicker("option", "yearRange", inicio + ":" + (inicio + 2));
        $("#fechaFinCalendario").datepicker("option", "yearRange", inicio + ":" + (inicio + 2));

        

    }

    CalendarioEscolar.Eliminar = function () {
        var actual = new Date().getFullYear();
        $('#grid_button_delete').hide();
        $('#grid_button_editar').hide();
        $("#divCamposFechasCalendario").hide();
        if ($("#A_oCiclo option:selected").val() <= actual) {
            Mensaje.Error.texto = "El ciclo lectivo no se puede eliminar";
            Mensaje.Error.mostrar();
            $("#CicloLectivo").attr("disabled", "disabled");
            $("#divAsignacionInstrumentoLegal").hide();
        }
    }


    CalendarioEscolar.CargarGrillaFechas = function () {
        $("#listFechas").setGridParam({
            url: $.getUrl("/CalendarioEscolar/GetFechasByCicloLectivo"),
            datatype: 'json',
            postData: { ciclo: $("#Id").val() }
        });
        $('#listFechas').trigger('reloadGrid');
        $('#listFechas').setGridParam({ gridComplete: CalendarioEscolar.CargarDataFechas });
    };
    CalendarioEscolar.CargarDataFechas = function () {
        var temp = $("#listFechas").getRowData();
        $("#listFechas").data("fechas", temp);
    };