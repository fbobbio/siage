var AbmcMixto = {};
AbmcMixto.controller = null;
AbmcMixto.estadoText = "Ver";
AbmcMixto.sinConsulta = true;
AbmcMixto.grilla = null;

AbmcMixto.divAbmc = "#divAbmc";
AbmcMixto.divAbmcDetalle = "#divAbmcDetalle";
AbmcMixto.divConsulta = "#divConsulta";

AbmcMixto.btnConsultar = "#btnConsultar";
AbmcMixto.btnLimpiar = "#btnLimpiar";
AbmcMixto.btnAgregar = "#btnAgregar";
AbmcMixto.btnAceptar = "#btnAceptar";
AbmcMixto.btnVolver = "#btnVolver";

AbmcMixto.init = function (controlador, grilla) {

    AbmcMixto.controller = controlador;
    AbmcMixto.grilla = grilla;

    $(AbmcMixto.btnConsultar).click(function () {
        AbmcMixto.grilla.setGridParam({ page: 1 });
        AbmcMixto.grilla.actualizar();
        AbmcMixto.sinConsulta = false;
    });

    $(AbmcMixto.btnLimpiar).click(function () {
        var cargando = $("load_" + AbmcMixto.grilla.id_limpio).is(":visible");
        if (!cargando) {
            AbmcUtil.limpiarFormulario(AbmcMixto.divConsulta);
            AbmcMixto.grilla.limpiar();
            AbmcMixto.sinConsulta = true;
        }
    });
};

AbmcMixto.cargarModelo = function (estado, id, detalle) {
    if (AbmcMixto.preCargarModelo) {
        if (!AbmcMixto.preCargarModelo(estado, id, detalle)) {
            return;
        }
    }

	var div;
	if(detalle) {
		div = AbmcMixto.divAbmcDetalle;
		$(AbmcMixto.divAbmc).html("");
	}
	else {
		div = AbmcMixto.divAbmc;
		$(AbmcMixto.divAbmcDetalle).html("");
	}
    AbmcUtil.limpiarFormulario(div);

    AbmcMixto.estadoText = estado;
    var urlGet = $.getUrl("/" + AbmcMixto.controller + "/" + AbmcMixto.estadoText + "/" + id);

	// Bloqueo la pantalla
    Formulario.Procesando.bloquear();
	
    $.get(urlGet, { detalle: detalle }, function (data) {
        $(div).html(data);

        if (AbmcMixto.postCargarModelo) {
            AbmcMixto.postCargarModelo(estado, id);
        }
		
        $(AbmcMixto.btnAceptar).click(function () { AbmcMixto.enviarModelo(detalle); });
        $(AbmcMixto.btnVolver).click(AbmcMixto.mostrarDivConsulta);

        AbmcMixto.mostrarDivAbmc(div);
        $("form").keypress(AbmcUtil.submitOnEnter);
		
		// Desbloqueo la pantalla
        Formulario.Procesando.desbloquear();
    }, "html");
};

AbmcMixto.enviarModelo = function (detalle) {
    if (!Validacion.validar()) {
        return;
    }

    var sufijo = detalle ? "Detalle" : "";
    var urlPost = $.getUrl("/" + AbmcMixto.controller + "/" + AbmcMixto.estadoText + sufijo);

    var campos = $(AbmcMixto.divAbmc + sufijo + " :input:disabled");
    $(campos).removeAttr("disabled");

    var datos = $("form").serializeArray();
    if (detalle) {
        datos.push({ name: "idPadre", value: AbmcMixto.grilla.Subgrilla.idExtendido });
    }

    // Metodo que se llama antes de hacer el POST
    if (AbmcMixto.preEnviarModelo) {
        AbmcMixto.preEnviarModelo(datos);
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

                $(AbmcMixto.btnAceptar).hide();
                $(AbmcMixto.btnVolver).val("Volver");
				
				$(AbmcMixto.divAbmc + " :input").not(AbmcMixto.btnVolver).attr("disabled", "disabled");
            }
            else {
                $(campos).attr("disabled", "disabled");
				
                for (var i = 0; i < data.details.length; i++) {
                    Mensaje.Error.agregarError(data.details[i]);
                }
            }

            // Metodo que se llama despues de hacer el POST
            if (AbmcMixto.postEnviarModelo) {
                AbmcMixto.postEnviarModelo(data);
            }
			
			// Desbloqueo la pantalla
			Formulario.Procesando.desbloquear();
        },
        "json");
};

AbmcMixto.preCargarModelo = null;
AbmcMixto.postCargarModelo = null;
AbmcMixto.preEnviarModelo = null;
AbmcMixto.postEnviarModelo = null;

AbmcMixto.mostrarDivAbmc = function (div) {
    $(AbmcMixto.divConsulta).hide();
    $(div).show();

	Mensaje.ocultar();
    Validacion.init();
    Formulario.init();
};

AbmcMixto.mostrarDivConsulta = function () {
    $(AbmcMixto.divConsulta).show();
    $(AbmcMixto.divAbmc + ", " + AbmcMixto.divAbmcDetalle).html("");
    Mensaje.ocultar();

    if (!AbmcMixto.sinConsulta) {
        AbmcMixto.grilla.actualizar();
    }
};