var ModificarTipoEmpresa = {};
//TODO: Falta implementar las grillas para mostrar las escuelas(madres y anexo).

ModificarTipoEmpresa.init = function (vista, instancia) {

    var tipoEmpresaSeleccionada = $(instancia.prefijo + 'VerTipoEmpresa').val();
    $("#FiltroTipoEmpresa > option").remove();
    $('#FiltroTipoEmpresa').append("<option value=ESCUELA_ANEXO>ESCUELA_ANEXO</option>");
    $('#FiltroTipoEmpresa').append("<option value=ESCUELA_MADRE>ESCUELA_MADRE</option>");
    $('#btnBuscarEscuelaRaiz').show();
    $('#EsRaiz').attr('checked', false);
    $('#NumeroAnexo').removeAttr('disabled');
    $("label[for='NumeroAnexo']").html('Numero Anexo(*)');
    $('#Nombre').removeAttr('disabled');
    var idEmpresaSeleccionada = $('#Id').val();

    $('#EscuelaMadre').hide();
    $('#EscuelaAnexo').hide();
    $("#NumeroAnexo").numeric();
    document.getElementById('NumeroAnexo').maxLength = 2;

    $('#btnSugerirNombre').attr('disabled', 'disabled'); //TODO: Habilitar botón cuando funcione lo de domicilio

    //setea el value del combo, con el tipo de empresa de la escuela seleccionada
    if (tipoEmpresaSeleccionada == 'ESCUELA_MADRE') {
        $('#TipoEmpresa').val('ESCUELA_ANEXO');
        $('#EscuelaMadre').hide();
        $('#EscuelaAnexo').show();
    }
    else {
        $('#TipoEmpresa').val('ESCUELA_MADRE');
        $('#EscuelaAnexo').hide();
        $('#EscuelaMadre').show();
    }
    //fin seteo del combo

    $('#EsRaiz').attr('disabled', false);
    $('#NumeroAnexo').attr('disabled', false);
    $('#EsRaiz').changePatch(function () {
        if ($('#EsRaiz').is(':checked')) {
            $('#btnBuscarEscuelaRaiz').hide();
            $("#btnSugerirNombre").hide();
        }
        else {
            $('#btnBuscarEscuelaRaiz').show();
            $("#btnSugerirNombre").show();
        }
    });

    $("#divBuscarEscuelaRaiz").hide();
    $('#btnBuscarEscuelaRaiz').click(function () {
        $("#divBuscarEscuelaRaiz").show();
        ModificarTipoEmpresa.editorConsultarRaiz = ConsultarEmpresa.init(vista, "#divBuscarEscuelaRaiz", 'buscarEscuelaRaiz', false);

        //Filtro solo escuelas que son Madres
        var loadCompleteViejoRaiz = ModificarTipoEmpresa.editorConsultarMadre.grilla.getGridParam("loadComplete");
        ModificarTipoEmpresa.editorConsultarMadre.grilla.setGridParam({
            loadComplete: function (data) {
                loadCompleteViejoRaiz(data);
                var escuelas = ModificarTipoEmpresa.editorConsultarMadre.grilla.getRowData();
                var escuelas_madre = escuelas.filter(function (value) {
                    if (value.TipoEmpresa !== "ESCUELA_MADRE") {
                        ModificarTipoEmpresa.editorConsultarMadre.grilla.delRowData(value.Id);
                        return false;
                    }
                    return true;
                });
                //Elimino la empresa que se seleccionó primero, para q no me la muestre de nuevo
                ModificarTipoEmpresa.editorConsultarMadre.grilla.delRowData(instancia.seleccion);
            }
        });

        $("#divVista").show();
        $(ModificarTipoEmpresa.editorConsultarRaiz.grilla.id).setGridWidth($("#divConsulta").width() - 10, true);
        $(ModificarTipoEmpresa.editorConsultarRaiz.grilla.id + "sinRegistros").hide();
    });

    $("#divBuscarEscuelaMadre").hide();
    $('#btnBuscarEscuelaMadre').click(function () {
        $("#divBuscarEscuelaMadre").show();
        ModificarTipoEmpresa.editorConsultarMadre = ConsultarEmpresa.init(vista, "#divBuscarEscuelaMadre", 'buscarEscuelaMadre', false);

        //Filtro solo escuelas que son Madres
        var loadCompleteViejoMadre = ModificarTipoEmpresa.editorConsultarMadre.grilla.getGridParam("loadComplete");
        ModificarTipoEmpresa.editorConsultarMadre.grilla.setGridParam({
            loadComplete: function (data) {
                loadCompleteViejoMadre(data);
                var escuelas = ModificarTipoEmpresa.editorConsultarMadre.grilla.getRowData();
                var escuelas_madre = escuelas.filter(function (value) {
                    if (value.TipoEmpresa !== "ESCUELA_MADRE") {
                        ModificarTipoEmpresa.editorConsultarMadre.grilla.delRowData(value.Id);
                        return false;
                    }
                    return true;
                });
                //Elimino la empresa que se seleccionó primero, para q no me la muestre de nuevo
                ModificarTipoEmpresa.editorConsultarMadre.grilla.delRowData(instancia.seleccion);
            }
        });
        $("#divVista").show();
        $(ModificarTipoEmpresa.editorConsultarMadre.grilla.id).setGridWidth($("#divConsulta").width() - 10, true);
        $(ModificarTipoEmpresa.editorConsultarMadre.grilla.id + "sinRegistros").hide();
    });

    $("#btnCancelarModificacion").click(function () {
        $("#divVista").hide();
        $(instancia.prefijo + "divDatosGeneralesEmpresa").hide();
        $(instancia.prefijo + "divFiltrosDeBusqueda").show();
        GrillaUtil.actualizar($(instancia.prefijo + "list"));
    });

    $('#btnSugerirNombre').click(function () {
        $.get($.getUrl('/GestionEmpresa/SugerirNombre/?idEmpresa=' + $(instancia.prefijo + "Id").val()),
        {},
        function (data) { $('#Nombre').val(data); },
        'json');
    });

    $("#btnAceptarModificacion").click(function () {
        $("form :input").removeAttr("disabled");

        if ($('#TipoEmpresa').val() == "ESCUELA_ANEXO") {
            if ($('#NumeroAnexo').val() == "") {
                Mensaje.Error.text = 'El campo Numero anexo es requerido';
                Mensaje.Error.mostrar();
                return;
            }
        }

        var formulario = $("form").serializeArray();

        if (ModificarTipoEmpresa.editorConsultarRaiz) {
            formulario.push({
                name: "IdEscuelaRaiz",
                value: ModificarTipoEmpresa.editorConsultarRaiz.seleccion
            });
        }
        else {
            if ($('#TipoEmpresa').val() == "ESCUELA_MADRE" && $('#EsRaiz').attr('checked') == false) {
                Mensaje.Error.texto = 'El campo escuela raíz es requerido';
                Mensaje.Error.mostrar();
                return;
            }
        }
        if (ModificarTipoEmpresa.editorConsultarMadre) {
            formulario.push({
                name: "IdEscuelaMadre",
                value: ModificarTipoEmpresa.editorConsultarMadre.seleccion
            });
        }
        else {
            if ($('#TipoEmpresa').val() == "ESCUELA_ANEXO") {
                Mensaje.Error.texto = 'El campo escuela madre es requerido';
                Mensaje.Error.mostrar();
                return;
            }
        }
        $.post($.getUrl("/GestionEmpresa/ModificarTipoEmpresa"), formulario,
            function (data) {
                if (data.status) {
                    Mensaje.Exito.mostrar();
                    $("form :input").attr("disabled", "disabled");
                    $('#btnAceptarModificacion').hide();
                    $("#btnCancelarModificacion").removeAttr("disabled");
                    $("#btnCancelarModificacion").val("Volver");
                }
                else {
                    $("form :input").removeAttr("disabled", "disabled");
                    for (var i = 0; i < data.details.length; i++) {
                        Mensaje.Error.agregarError(data.details[i]);
                    }

                }
            }, "json");
    });
};

ModificarTipoEmpresa.empresaSeleccionada = function (empresa) {
    $("#consultaIndex_divFiltrosDeBusqueda").hide();
    $("#divVista").show();
    var url = $.getUrl('/GestionEmpresa/ProcesarSeleccion/?id=' + empresa.Id + '&vistaActiva=ModificarTipoEmpresaEditor');
    $.get(url, null,
                function (data) {

                    $("#ConsultaIndex_divDatosSeleccionMultiple").hide();

                    $("#divVista").html(data);
                    $('#divVista').prepend($('#consultaIndex_divDatosGeneralesEmpresa').html());

                    $("#divVista #consultaIndex_Id").val(empresa.Id);
                    $("#divVista #consultaIndex_VerCodigoEmpresa").val(empresa.CodigoEmpresa);
                    $("#divVista #consultaIndex_VerNombreEmpresa").val(empresa.NombreEmpresa);
                    $("#divVista #consultaIndex_VerCueEmpresa").val(empresa.CUE);
                    $("#divVista #consultaIndex_VerCueAnexoEmpresa").val(empresa.CUEAnexo);
                    $("#divVista #consultaIndex_VerTipoEducacion").val(empresa.TipoEducacion);
                    $("#divVista #consultaIndex_VerNivelEducativo").val(empresa.NivelEducativo);
                    $("#divVista #consultaIndex_VerTipoEmpresa").val(empresa.TipoEmpresa);
                    $("#divVista #consultaIndex_VerEstadoEmpresa").val("");

                    if (empresa.TipoEmpresa === "ESCUELA_MADRE") {
                        $('#TipoEmpresa').val("ESCUELA_ANEXO");
                        $('#EscuelaMadre').hide();
                        $('#EscuelaAnexo').show();
                    }
                    else {
                        $('#TipoEmpresa').val("ESCUELA_MADRE");
                        $('#EscuelaAnexo').hide();
                        $('#EscuelaMadre').show();
                    }

                    $('#btnCancelarModificacion').unbind('click');
                    $('#btnCancelarModificacion').click(function () {
                        $("#divVista").hide();
                        $("#divConsulta, #consultaIndex_divFiltrosDeBusqueda").show();
                        GrillaUtil.actualizar($("#consultaIndex_list"));
                    });

                }, "html");
};

////////////////////////////////////////- FUNCIONES DE SOPORTE -////////////////////////////////////////

ModificarTipoEmpresa.obtenerParametroUrl = function (nombreParametro) {
    //armo el string con la expresión regular
    var regexS = "[\\?&]" + nombreParametro + "=([^&#]*)";
    //ejecuto la función q interpreta el string anterior
    var regex = new RegExp(regexS);
    //obtengo la url completa de la barra de dirección
    var tmpURL = window.location.href;
    //obtengo un array de largo 2 con el nombre de la variable y su valor y devuelvo el mismo
    var results = regex.exec(tmpURL);
    if (results == null)
        return "";
    else
        return results[1];
};

