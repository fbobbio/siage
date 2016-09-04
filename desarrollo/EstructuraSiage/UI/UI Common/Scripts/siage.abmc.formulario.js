var Abmc = {};
Abmc.controller = null;
Abmc.estadoText = "Ver";
Abmc.sinConsulta = true;
Abmc.registrando = false;
Abmc.grilla = null;
Abmc.grillaHistorial = null;

Abmc.divAbmc = "#divAbmc";
Abmc.divGrillaAbmc = "#divGrillaAbmc";
Abmc.divConsulta = "#divConsulta";
Abmc.divHistorial = "#divHistorial";

Abmc.btnConsultar = "#btnConsultar";
Abmc.btnLimpiar = "#btnLimpiar";
Abmc.btnAgregar = "#btnAgregar";
Abmc.btnAceptar = "#btnAceptar";
Abmc.btnVolver = "#btnVolver";
Abmc.btnVolverHistorial = "#btnVolverHistorial";

Abmc.init = function (controlador, grilla) {
    Abmc.controller = controlador;
    Abmc.grilla = grilla;

    $(Abmc.btnConsultar).click(function () {
        Abmc.grilla.setGridParam({ page: 1 });
        Abmc.grilla.actualizar();
        Abmc.sinConsulta = false;
    });

    $(Abmc.btnLimpiar).click(function () {
        var cargando = $("load_" + Abmc.grilla.id_limpio).is(":visible");
        if (!cargando) {
            AbmcUtil.limpiarFormulario(Abmc.divConsulta);
            Abmc.grilla.limpiar();
            Abmc.sinConsulta = true;
        }
    });
};

Abmc.mostrarHistorial = function (id) {
    $(Abmc.btnVolverHistorial).click(Abmc.mostrarDivConsulta);
    $(Abmc.divConsulta).hide();
    $(Abmc.divAbmc).hide;
    $(Abmc.divHistorial).show();
    if (Abmc.grillaHistorial) {
        Abmc.grillaHistorial(id);
    }
}

Abmc.cargarModelo = function (estado, id) {
    if (Abmc.preCargarModelo) {
        if (!Abmc.preCargarModelo(estado, id)) {
            return;
        }
    }

    AbmcUtil.limpiarFormulario(Abmc.divAbmc);

    Abmc.estadoText = estado;
    var urlGet = $.getUrl("/" + Abmc.controller + "/" + Abmc.estadoText + "/" + id);

    // Bloqueo la pantalla
    Formulario.Procesando.bloquear();

    $.get(urlGet, {}, function (data) {
        $(Abmc.divAbmc).html(data);

        if (Abmc.postCargarModelo) {
            Abmc.postCargarModelo(estado, id);
        }

        $(Abmc.btnAceptar).click(Abmc.enviarModelo);

        $(Abmc.btnVolver).click(function () {
            Abmc.mostrarDivConsulta();
            Abmc.registrando = false;
        });

        if (Abmc.estadoText === "Registrar" && !Abmc.registrando) {
            Abmc.registrando = true;
            Abmc.mostrarGrillaAbmc();
        }

        Abmc.mostrarDivAbmc();
        $("form").keypress(AbmcUtil.submitOnEnter);

        // Desbloqueo la pantalla
        Formulario.Procesando.desbloquear();
    }, "html");
};

Abmc.enviarModelo = function () {
    if (!Validacion.validar()) {
        return;
    }

    var urlPost = $.getUrl("/" + Abmc.controller + "/" + Abmc.estadoText);

    var campos = $(Abmc.divAbmc + " :input:disabled");
    $(campos).removeAttr("disabled");

    var json = AbmcUtil.formToJSON("form");
    var registro = Abmc.completarRegistroReciente(json);
    var datos = $("form").serializeArray();

    // Metodo que se llama antes de hacer el POST
    if (Abmc.preEnviarModelo) {
        Abmc.preEnviarModelo(datos);
    }

    datos = datos.filter(function (campo) {
        return (campo.value !== "" && campo.value !== null && campo.value !== undefined);
    });

    // Bloqueo la pantalla
    Formulario.Procesando.bloquear();

    $.post(urlPost, datos,
        function (data) {
            if (data.status) {
                Mensaje.Exito.mostrar();

                if (!Abmc.registrando) {
                    $(Abmc.btnAceptar).hide();
                    $(Abmc.btnVolver).val("Volver");

                    $(Abmc.divAbmc + " :input").not(Abmc.btnVolver).attr("disabled", "disabled");
                }
                else {
					if (Abmc.grilla.recientes)
						Abmc.grilla.recientes.agregarFila(registro);
                    AbmcUtil.limpiarFormulario(Abmc.divAbmc);

                    $("#Id, #Version").val("0");
                }
            }
            else {
                $(campos).attr("disabled", "disabled");

                for (var i = 0; i < data.details.length; i++) {
                    Mensaje.Error.agregarError(data.details[i]);
                }
            }

            // Metodo que se llama despues de hacer el POST
            if (Abmc.postEnviarModelo) {
                Abmc.postEnviarModelo(data);
            }

            // Desbloqueo la pantalla
            Formulario.Procesando.desbloquear();
        },
        "json");
};

Abmc.preCargarModelo = null;
Abmc.postCargarModelo = null;
Abmc.preEnviarModelo = null;
Abmc.postEnviarModelo = null;

Abmc.completarRegistroReciente = function (registro) {
    $(Abmc.divAbmc + " select").each(function (ind, val) {
        var texto = $(this).children("option:selected[value!='']").html();
        var name = $(this).attr("name");

        registro[name] = texto;
    });

    return registro;
};

Abmc.mostrarGrillaAbmc = function () {
    $(Abmc.divGrillaAbmc).show();
    if (Abmc.grilla.recientes) {
        Abmc.grilla.recientes.limpiar();
        Abmc.grilla.recientes.setGridWidth($(Abmc.divConsulta).width() - 10, true);
    }
};

Abmc.mostrarDivAbmc = function () {
    $(Abmc.divConsulta).hide();
    $(Abmc.divAbmc).show();
    $(Abmc.divHistorial).hide();

	Mensaje.ocultar();
    Validacion.init();
    Formulario.init();
};

Abmc.mostrarDivConsulta = function () {
    $(Abmc.divConsulta).show();
    $(Abmc.divAbmc + ", " + Abmc.divGrillaAbmc).hide();
    Mensaje.ocultar();

    if (!Abmc.sinConsulta) {
        Abmc.grilla.actualizar();
    }
};