var EmpresaRegistrar = {};
EmpresaRegistrar.init = function (estadoText) {

    EmpresaRegistrar.obtenerParrafos('TipoEducacion');
    EmpresaRegistrar.obtenerParrafos('TipoEscuela');
    EmpresaRegistrar.obtenerParrafos('NivelEducativoId');

    EmpresaRegistrar.ocultarTodos();
    EmpresaRegistrar.mostrarCapas();

    //funcionalidad al btn aceptar dependiendo si es modificar o registrar
    switch (estadoText) {
        case "Registrar":
            $("#btnAceptar").click(function () {
                $.post("/GestionEmpresa/Registrar",
                    $("#divVista form").serialize(),
                    function (data) { $("#divVista").html(data); },
                    "html");
            });
            $("#TipoEmpresa").attr("disabled", "disabled");
            $("#TipoEmpresa").CascadingDropDown("#TipoGestion", '/GestionEmpresa/CargarTipoEmpresa', { promptText: 'SELECCIONE' });
            break;

        case "Editar":
            $("#btnAceptar").click(function () {
                $.post("/GestionEmpresa/GetCampos", null, function (data) {
                    $.each(data, function (index, value) {
                        $("#lista").tmpl(value).appendTo("#CamposSinInstrumentoLegal");
                    });
                });
            });
            EmpresaRegistrar.escuelaPrivada();
            var tipoGestionSelec = $("#TipoGestion option:selected").text()
            $("#TipoGestion").attr("disabled", "disabled");
            var tipoEmpresaSelec = $("#TipoEmpresa option:selected").val();
            EmpresaRegistrar.mostrarCampos(tipoEmpresaSelec);
            $("#TipoEmpresa option").remove();
            $.getJSON('/GestionEmpresa/CargarTipoEmpresa', { TipoGestion: tipoGestionSelec }, function (data) {
                var html = '';
                var len = data.length;
                for (var i = 0; i < len; i++) {
                    html += '<option value="' + data[i].Value + '">' + data[i].Text + '</option>';
                    //$('#TipoEmpresa').append('<option value="' + data[i].Id + '">' + data[i].Nombre + '</option>');
                }
                $("#TipoEmpresa").append(html);
                $("#TipoEmpresa").val(tipoEmpresaSelec);
            });
            break;
        case "Revisar":
            $("#btnAceptar").hide();
            $("#btnCancelar").val("Volver");
            break;
    } //fin funcionalidad
    //funcionalidad al boton cancelar
    $("#btnCancelar").click(function () {
        EmpresaRegistrar.limpiarCampos();
        $("#divVista").hide();
        $("#divConsulta").show();
    }); //fin funcionalidad

    EmpresaRegistrar.deshabilitarCampos();

    //    $("#ConjuntoEmpresaPadre").hide();
    //    $("#ConjuntoPaquete").hide();
    //    $("#ConjuntoEscuelaRaiz").hide();
    //    $("#ConjuntoEscuelaMadre").hide();
    //    $("#ConjuntoEmpresaSupervisora").hide();

    //habilita/deshabilita btn buscar escuela raiz
    $("#btnEscuelaRaiz").attr("disabled", false);
    $("#EsRaiz").changePatch(function () {
        if ($("#EsRaiz").is(":checked"))
            $("#btnEscuelaRaiz").attr("disabled", "disabled");
        else
            $("#btnEscuelaRaiz").attr("disabled", false);
    });

    $("#btnEscuelaRaiz").click(function () {
        $("#divVista").hide();
        $("#divConsulta").show();
    }); //fin btn buscar escuela raiz

    $("#btnEscuelaMadre").click(function () {
        $("#divVista").hide();
        $("#divConsulta").show();
    });

    $("#btnEmpresaPadre").click(function () {
        $("#divVista").hide();
        $("#divConsulta").show();
    });

    $("#ZonaDesfavorable").changePatch(function () {
        if ($("#ZonaDesfavorable").val() != "")
            $("#btnInstrumento").attr("disabled", false);
        else
            $("#btnInstrumento").attr("disabled", "disabled")
    });
}

//oculta todas las divs
EmpresaRegistrar.ocultarTodos = function () {
    $(".DireccionNivel").hide();
    $(".EscuelaMadreAnexo").hide();
    $(".EscuelaMadre").hide();
    $(".EscuelaAnexo").hide();
    $(".Inspeccion").hide();
    $(".EsPrivado").hide();
}

//muestra/oculta divs dependiendo q tipo de empresa se selecciona
EmpresaRegistrar.mostrarCapas = function () {
    EmpresaRegistrar.ocultarTodos();
    $("#TipoEmpresa").changePatch(function () {

        $("#pTipoEscuela").remove();
        $("#pNivelEducativoId").remove();
        $("#pTipoEducacion").remove();

        switch ($("#TipoEmpresa").val()) {
            case "DIRECCION_DE_NIVEL":
                EmpresaRegistrar.ocultarTodos();
                var empresa = "DN";
                EmpresaRegistrar.cargarParrafos(empresa);
                $(".DireccionNivel").show();
                break;
            case "ESCUELA_MADRE":
                EmpresaRegistrar.ocultarTodos();
                var empresa = "EMA";
                EmpresaRegistrar.cargarParrafos(empresa);
                $(".EscuelaMadreAnexo").show();
                $(".EscuelaMadre").show();
                EmpresaRegistrar.escuelaPrivada();
                break;
            case "ESCUELA_ANEXO":
                EmpresaRegistrar.ocultarTodos();
                var empresa = "EMA";
                EmpresaRegistrar.cargarParrafos(empresa);
                $(".EscuelaMadreAnexo").show();
                $(".EscuelaAnexo").show();
                EmpresaRegistrar.escuelaPrivada();
                break;
            case "INSPECCION":
                EmpresaRegistrar.ocultarTodos();
                $(".Inspeccion").show();
                $("#TipoInspeccion").changePatch(function () {
                    if ($("#TipoInspeccion").val() != "GENERAL" && $("#TipoInspeccion").val() != "")
                        $("#btnEmpresaSupervisora").attr("disabled", false);
                    else
                        $("#btnEmpresaSupervisora").attr("disabled", "disabled")
                });

                break;
            default: EmpresaRegistrar.ocultarTodos();
                break;
        }
    });
}

EmpresaRegistrar.escuelaPrivada = function () {
    $("#Privado").changePatch(function () {
        if ($("#Privado").is(":checked"))
            $(".EsPrivado").show();
        else
            $(".EsPrivado").hide();
    });
}

EmpresaRegistrar.limpiarCampos = function () {
    $(":input[type=text]").val("");
    $(":input[type=textarea]").val("");
    $(":input[type=checkbox]").attr("checked", false);
}

EmpresaRegistrar.deshabilitarCampos = function () {
    $("#btnEdificios").attr("disabled", "disabled");
    $("#btnEmpresaPadre").attr("disabled", "disabled");
    $("#btnPaquete").attr("disabled", "disabled");
    $("#btnEstructura").attr("disabled", "disabled");
    $("#btnEmpresaSupervisora").attr("disabled", "disabled");
    $("input[name=EmpresaPadre]").attr("disabled", "disabled");
    $("input[name=EscuelaRaiz]").attr("disabled", "disabled");
    $("input[name=EscuelaMadre]").attr("disabled", "disabled");
    $("input[name=EmpresaSupervisora]").attr("disabled", "disabled");
    $("input[name=PaquetePresupuestado]").attr("disabled", "disabled");
    $("#divVista #CodigoEmpresa").attr("disabled", "disabled");
}

EmpresaRegistrar.mostrarCampos = function (tipoEmpresaSelec) {
    switch (tipoEmpresaSelec) {
        case "ESCUELA_MADRE":
            EmpresaRegistrar.cargarParrafos("EMA");
            $(".EscuelaMadreAnexo").show();
            $(".EscuelaMadre").show();
            break;
        case "ESCUELA_ANEXO":
            EmpresaRegistrar.cargarParrafos("EMA");
            $(".EscuelaMadreAnexo").show();
            $(".EscuelaAnexo").show();
            break;
        case "INSPECCION":
            $(".Inspeccion").show();
            break;
        case "DIRECCION_DE_NIVEL":
            EmpresaRegistrar.cargarParrafos("DN");
            $(".DireccionNivel").show();
            break;
        default:
            break;
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
    alert(idInput +", " + $("#p" + idInput).html());
}

EmpresaRegistrar.cargarParrafos = function (empresa) { 
    //recibe la empresa por parametro, 
    //y con eso seleccionamos el div y le hacemos 
    //append obteniendo del data el <p> q corresponde
    $("#TipoEscuela" + empresa).append($('body').data("pTipoEscuela"));
    $("#NivelEducativo" + empresa).append($('body').data("pNivelEducativoId"));
    $("#TipoEducacion" + empresa).append($('body').data("pTipoEducacion"));
}