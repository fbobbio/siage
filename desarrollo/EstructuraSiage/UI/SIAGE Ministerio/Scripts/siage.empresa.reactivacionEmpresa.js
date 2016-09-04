var ReactivacionEmpresa = {};
ReactivacionEmpresa.instanciaVinculoEdificio = {};
ReactivacionEmpresa.init = function (instancia, prefijo) {
    ReactivacionEmpresa.prefijo = prefijo || "";
    ReactivacionEmpresa.EscuelaEsSuperior = false;
    $(".val-DateTime").datepicker({ currentText: 'Now', dateFormat: 'dd/mm/yy' });

    $("#btnAceptar1").click(ReactivacionEmpresa.enviarModelo);

    $("#btnCancelar").live("click", function () {
        $("#divReactivacionEmpresa").hide();
        $(instancia.prefijo + "divDatosGeneralesEmpresa").hide();
        $(instancia.prefijo + "divFiltrosDeBusqueda").show();
        $(instancia.grilla).trigger('reloadGrid');
        $(ConsultarEmpresa.divVista).hide();
        $("#divConsultar").show();
    });

    $("#btnSugerirNombre").click(function () {
        ReactivacionEmpresa.getNombreSugerido(ReactivacionEmpresa.editorConsultarMadre.seleccion);
    });

    $("#VincularEdificioCheck").changePatch(function () {
        if ($("#VincularEdificioCheck").is(":checked")) {
            $("#divVincularEdificioAEmpresa").show();
            $(ReactivacionEmpresa.instanciaVinculoEdificio.prefijo + "divVincularEdificioAEmpresa").show();
        }
        else {
            $("#divVincularEdificioAEmpresa").hide();
        }
    });

    /* Esta funcionalidad no entra en fase 1 ni en fase 2
    $("#IngrearPaquetePresupuestarioCheck").changePatch(function () {
        if ($("#IngrearPaquetePresupuestarioCheck").is(":checked")) {
            $("#divAreaPaquetePresupuestario").show();
        }
        else {
            $("#divAreaPaquetePresupuestario").hide();
        }
    });*/

    $("#InstrumentoLegalCheck").changePatch(function () {

        if ($("#InstrumentoLegalCheck").is(":checked")) {

            $("#divInstrumentoLegalEditor").html($("#InstrumentoLegalCheck").data("editorIL"));

            $("#divInstrumentoLegalEditor").show();
            $("#divAsignacionInstrumentoLegal").show();
        }
        else {
            $("#divInstrumentoLegalEditor").hide();
            $("#divAsignacionInstrumentoLegal").hide();

            $("#InstrumentoLegalCheck").data("editorIL", $("#divInstrumentoLegalEditor").html());
            $("#divInstrumentoLegalEditor").html("");

        }
    });

    $("#PlanDeEstudiosCheck").changePatch(function () {
        if ($("#PlanDeEstudiosCheck").is(":checked")) {
            $("#divAreaPlanDeEstudios").show();
        }
        else {
            $("#divAreaPlanDeEstudios").hide();
        }
    });

    /*$("#EscuelaMadreCombo").changePatch(function () {
    //TODO: CHECKEAR QUE LA SELECCIÓN SEA MADRE MEDIANTE REGLA
    ReactivacionEmpresa.getNombreSugerido(ReactivacionEmpresa.editorConsultarMadre.seleccion);
    });*/

    //Edificios y Domicilio    
    ReactivacionEmpresa.instanciaVinculoEdificio = VinculoEmpresaEdificio.init("", "#divVincularEdificioAEmpresa", "Empresa");

    //Verifico si la empresa tiene vínculos a edificios activos
    ReactivacionEmpresa.hasVinculosActivos($("#Id").val());
    //Verifico si la empresa es escuela
    ReactivacionEmpresa.getValidacionEscuela($("#Id").val());

    //InstrumentoLegal.consultarInstrumentoPorNumero = true;
    InstrumentoLegal.init("Instrumento_");

    ReactivacionEmpresa.EstructuraEscolar();
    ReactivacionEmpresa.HabilitarCamposEstructura(instancia);
    $("#IngrearPaquetePresupuestarioCheck").attr("disabled", true);

    $("#PlanDeEstudiosCheck").attr("disabled", true);

    //para q no tire el error de los datepicker
    $(ConsultarEmpresa.divVista + " .val-DateTime").each(function (input) {
        var data = $(this).data("datepicker");

        if (data) {
            data.id = this.id;
            $(this).data("datepicker", data);
        }
    });
};

//Habilito los campos de la estructura escolar que misteriosamente aparecen deshabilitados
ReactivacionEmpresa.HabilitarCamposEstructura = function (instancia) {
    $("#EstructuraEscolar_Turno").attr("disabled", false);
    $("#EstructuraEscolar_GradoAnio").attr("disabled", false);
    $("#EstructuraEscolar_Division").attr("disabled", false);
    $("#EstructuraEscolar_Cupo").attr("disabled", false);
    $("#EstructuraEscolar_FechaApertura").attr("disabled", false);
}

// Función que verifica si la empresa tiene algún vínculo a edificio en estado Activo. Si no posee se llama al registrar Vínculos empresa a edificio
ReactivacionEmpresa.hasVinculosActivos = function (id) {
    $.get($.getUrl("/GestionEmpresa/EmpresaHasVinculosActivos"), { IdEmpresa: id },
        function (data) {
            if (data == true) { // Habilitar el check de vinculación a edificio
                $("#VincularEdificioCheck").attr("disabled", false);

                //busco los vinculos de la empresa, y cargo la grilla con los mismos
                $.get($.getUrl("/GestionEmpresa/GetVinculosEmpresaEdificio"),
                { empresaId: id },
                function (datos) {
                    VinculoEmpresaEdificio.setearVinculosYDomicilios(ReactivacionEmpresa.instanciaVinculoEdificio, datos[0].vin, datos[0].domJson);
                    if (Abmc.estadoText === "Ver") {
                        //Oculto el btnVolver del domicilio seleccionado
                        $(ReactivacionEmpresa.instanciaVinculoEdificio.prefijo + "btnVolverDomicilio").hide();
                    }
                },
                "json");
            }
            else { // Checkeo y deshabilito el check de vinculación a edificio y se muestra "Registrar Vínculo a edificio"
                $("#VincularEdificioCheck").attr("checked", true);
                $("#VincularEdificioCheck").changePatch();
                $("#VincularEdificioCheck").attr("disabled", true);
            }
        }, "json");
};

//Función que checkea si es escuela o no y opera según eso
    ReactivacionEmpresa.getValidacionEscuela = function (id) {
        $.get($.getUrl("/GestionEmpresa/CheckEscuela"), { IdEmpresa: id },
        function (data) {
            if (data.EsEscuela == true) { //Mostrar el sector de: "Si es Escuela" y "Si su Escuela Madre está Cerrada (seleccionar escuela madre)"
                ReactivacionEmpresa.EscuelaEsSuperior = data.Superior; // actualizo la variable que indica si la escuela es de nivel superior o no
                //La orden de pago y el programa presupuestario correspondiente a la dirección de nivel a la que pertenece el usuario logueado lo cargo en la Regla.
                $("#divNoEsEscuela").hide();
                if (data.EsMadre != true) { // Si la escuela no es madre
                    if (data.MadreCerrada == true) { // Si la madre está cerrada ofrezco asignar una nueva, si no dejo la que estaba
                        $("#divSeleccionarEscuelaMadre").show();
                        ReactivacionEmpresa.seleccionarMadre();
                    }
                }
                $("#divEstructuraEscolar").show();
                $("#divPlanDeEstudios").show(); //TODO: No entra en fase 1
                if (ReactivacionEmpresa.EscuelaEsSuperior)//Si el nivel educativo es Superior llamo a asignar plan de estudios superior
                {
                    $("#divPlanDeEstudiosSuperior").show();
                }
                else { //Si no es superior llamo a asignar plan de estudios a una escuela
                    $("#divPlanDeEstudiosEscuela").show();
                }
                //Selecciono empresa de inspección según tipo de dirección de nivel del usuario logueado
                $("#divEmpresaInspeccion").show();
                ReactivacionEmpresa.seleccionarInspeccion();
            } else { //No es escuela por lo que dejo que se seleccione la orden de pago y el programa presupuestario
                $("#divNoEsEscuela").show();
                $("#divEmpresaInspeccion").hide();
            }
        }, "json");
    };

    ReactivacionEmpresa.seleccionarMadre = function () {
        ReactivacionEmpresa.editorConsultarMadre = ConsultarEmpresa.init('SinVista', '#divGrillaEscuelaMadre', 'buscarEmpresaMadre', false);
        $('#divGrillaEscuelaMadre').show();
        $(ConsultarEmpresa.divVista).show();
//        $("#divSeleccionarEscuelaMadre").width(650)
//        $(ReactivacionEmpresa.editorConsultarMadre.grilla.id).setGridWidth($("#divSeleccionarEscuelaMadre").width() - 10, true);
        $(ReactivacionEmpresa.editorConsultarMadre.grilla.id + "sinRegistros").hide();
    };

    ReactivacionEmpresa.seleccionarInspeccion = function () {
        ReactivacionEmpresa.editorConsultarInspeccion = ConsultarEmpresa.init('BusquedaPorInspeccionUnica', '#divConsultarEmpresaInspeccion', 'buscarEmpresaInspeccion', false);
        $('#divConsultarEmpresaInspeccion').show();
        $(ConsultarEmpresa.divVista).show();
//        $("#divEmpresaInspeccion").width(650)
//        $(ReactivacionEmpresa.editorConsultarInspeccion.grilla.id).setGridWidth($("#divEmpresaInspeccion").width() - 10, true);
        $(ReactivacionEmpresa.editorConsultarInspeccion.grilla.id + "sinRegistros").hide();
        
    }

    ReactivacionEmpresa.getNombreSugerido = function (id) {
        $.get($.getUrl("/GestionEmpresa/SugerirNombre"), { IdEmpresa: id },
        function (data) {
            var nombre = data;
            $("#NombreSugerido").val(nombre);
        }, "json");
    };

    ReactivacionEmpresa.EstructuraEscolar = function () {
        $("#divEstructuraEscolar").hide();
        $("#TipoEmpresa").changePatch(function () {
            if ($('#TipoGestion option:selected').text() == 'ESCUELA' && $('#TipoEmpresa option:selected').text() != "SELECCIONE") {
                $.get($.getUrl('/GestionEmpresa/ParametroEstructura'), {},
                function (data) {
                    if (data == "True") {
                        $("#divEstructuraEscolar").show();
                    }
                    else {
                        $("#divEstructuraEscolar").hide();
                    }
                }
            );
            }
        });

        $("#RegistrarEstructuraEscolarCheck").changePatch(function () {
            if ($("#RegistrarEstructuraEscolarCheck").is(":checked")) {
                ReactivacionEmpresa.cargarComboTurnoEstructuraEscolar();
                $("#grillaEstructura").show();
            }
            else {
                $("#divAreaEstructuraEscolar").hide();
                $("#grillaEstructura").hide();
            }
        });
    };

    ReactivacionEmpresa.cargarComboTurnoEstructuraEscolar = function () {
        $.getJSON($.getUrl('/GestionEmpresa/GetTurnosByEscuelaId?escuelaId=' + parseInt($('#Id').val())), null,
        function (datos) {
            if (datos) {
                $("#EstructuraEscolar_Turno").cargarCombo(datos, "id", "turnos");
                ReactivacionEmpresa.cargarComboGradoAnio();
            }
        }
    );
    }

    ReactivacionEmpresa.cargarComboGradoAnio = function () {
        $.getJSON($.getUrl('/GestionEmpresa/GetAllGradoAñoByNivelEducativoByEscuelaId?escuelaId=' + parseInt($('#Id').val())), null,
        function (datos) {
            if (datos) {
                $("#EstructuraEscolar_GradoAnio").cargarCombo(datos, "Value", "Text");
            }
        }
    );
    }

    /* Método que envía el modelo mediante post al server */
ReactivacionEmpresa.enviarModelo = function () {
    //Deshabilito expediente para que levante los datos
    var model = $(ConsultarEmpresa.divVista).formatoJson();

    if (!$("#InstrumentoLegalCheck").is(":checked")) {
        delete model.Instrumento;
    }

    model.IdEmpresa = $("#Id").val();
    model.Domicilio = GrillaUtil.getSeleccionFilas($(ReactivacionEmpresa.instanciaVinculoEdificio.prefijo + "listDomicilio"), false); // Trae el id del domicilio
    if (!model.Domicilio) {
        model.Domicilio = 0;
    }

    if (ReactivacionEmpresa.editorConsultarMadre) {
        model.NuevaEscuelaMadre = ReactivacionEmpresa.editorConsultarMadre.seleccion;
    }

    model.NombreSugerido = $("#NombreSugerido").val();
    if (!ReactivacionEmpresa.editorConsultarInspeccion) {
    } else {
        if (!ReactivacionEmpresa.editorConsultarInspeccion.seleccion) {
            model.IdEmpresaInspeccion = -1;
        }
        else {
            model.IdEmpresaInspeccion = ReactivacionEmpresa.editorConsultarInspeccion.seleccion;
        }
    }

    //Levanto todos los vínculos de la grilla para insertarlos en el model
    var datosVinculos = $(ReactivacionEmpresa.instanciaVinculoEdificio.prefijo + "listVinculos").getRowData();
    model.VinculoEmpresaEdificio = [];
    for (var i = 0; i < datosVinculos.length; i++) {
        console.log(datosVinculos[i]);
        model.VinculoEmpresaEdificio[i] = {};
        model.VinculoEmpresaEdificio[i].Edificio = datosVinculos[i].IdEdificio;
        model.VinculoEmpresaEdificio[i].Observacion = datosVinculos[i].Observacion;
        model.VinculoEmpresaEdificio[i].FechaDesde = datosVinculos[i].FechaDesde;
    }
    //Levanto todos los datos de estructura escuela de la grilla para insertarlos en el model
    var datosEstructura = $("#listaEstructura").getRowData();
    model.EstructuraEscolar = [];
    for (var i = 0; i < datosEstructura.length; i++) {
        model.EstructuraEscolar[i] = {};
        model.EstructuraEscolar[i].Id = datosEstructura[i].Id;
        if (ReactivacionEmpresa.EscuelaEsSuperior) {
            model.EstructuraEscolar[i].CarreraNombre = datosEstructura[i].Carrera;
        }
        model.EstructuraEscolar[i].Turno = datosEstructura[i].Turno;
        model.EstructuraEscolar[i].GradoAnio = datosEstructura[i].GradoAnio;
        model.EstructuraEscolar[i].Division = datosEstructura[i].Division;
        model.EstructuraEscolar[i].Cupo = datosEstructura[i].Cupo;
        model.EstructuraEscolar[i].FechaApertura = datosEstructura[i].FechaApertura;
        model.EstructuraEscolar[i].Estado = 1; //Seteo el estado de pecho
        model.EstructuraEscolar[i].Escuela = $("#Id").val();
    }

    var datos = [];
    $.formatoModelBinder(model, datos, "");


    //TODO: Falta Plan de estudio

    $.post($.getUrl("/GestionEmpresa/ReactivarEmpresa"), datos,
        function (data) {
            if (data.status) {
                Mensaje.Exito.mostrar();
                $("#divReactivacionEmpresa").show();
                $("#divReactivacionEmpresa form :input, #divReactivacionEmpresa form textarea").attr("disabled", "disabled");
                $("#btnCancelar").val("Volver");
            }
            else {
                for (var i = 0; i < data.details.length; i++) {
                    Mensaje.Error.agregarError(data.details[i]);
                }
            }
        }, "json");
};

Abmc.postEnviarModelo = function (data) {
    if (data.status) {
        $(ConsultarEmpresa.divVista).hide();
        $("#divConsulta").show();
    }
};