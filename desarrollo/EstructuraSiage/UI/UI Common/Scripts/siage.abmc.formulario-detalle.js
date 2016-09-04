var AbmcDetalle = {};
AbmcDetalle.controller = null;
AbmcDetalle.estadoText = "Ver";
AbmcDetalle.sinConsulta = true;
AbmcDetalle.grilla = null;

AbmcDetalle.divAbmc = "#divAbmc";
AbmcDetalle.divConsulta = "#divConsulta";

AbmcDetalle.btnConsultar = "#btnConsultar";
AbmcDetalle.btnLimpiar = "#btnLimpiar";
AbmcDetalle.btnAgregar = "#btnAgregar";
AbmcDetalle.btnAceptar = "#btnAceptar";
AbmcDetalle.btnVolver = "#btnVolver";

AbmcDetalle.init = function (controlador, grilla) {

    AbmcDetalle.controller = controlador;
    AbmcDetalle.grilla = grilla;

    $(AbmcDetalle.btnConsultar).click(function () {
        AbmcDetalle.grilla.setGridParam({ page: 1 });
        AbmcDetalle.grilla.actualizar();
        AbmcDetalle.sinConsulta = false;
    });

    $(AbmcDetalle.btnLimpiar).click(function () {
        var cargando = $("load_" + AbmcDetalle.grilla.id_limpio).is(":visible");
        if (!cargando) {
            AbmcUtil.limpiarFormulario(AbmcDetalle.divConsulta);
            AbmcDetalle.grilla.limpiar();
            AbmcDetalle.sinConsulta = true;
        }
    });
};

AbmcDetalle.cargarModelo = function (estado, id) {
    if (AbmcDetalle.preCargarModelo) {
        if (!AbmcDetalle.preCargarModelo(estado, id)) {
            return;
        }
    }

    AbmcUtil.limpiarFormulario(AbmcDetalle.divAbmc);

    AbmcDetalle.estadoText = estado;
    var urlGet = $.getUrl("/" + AbmcDetalle.controller + "/" + AbmcDetalle.estadoText + "/" + id);

	// Bloqueo la pantalla
    Formulario.Procesando.bloquear();
	
    $.get(urlGet, {}, function (data) {
        $(AbmcDetalle.divAbmc).html(data);

        if (AbmcDetalle.postCargarModelo) {
            AbmcDetalle.postCargarModelo(estado, id);
        }
        
		$(AbmcDetalle.btnAceptar).click(AbmcDetalle.enviarModelo);
        $(AbmcDetalle.btnVolver).click(AbmcDetalle.mostrarDivConsulta);

        AbmcDetalle.mostrarDivAbmc();
        $("form").keypress(AbmcUtil.submitOnEnter);
		
		// Desbloqueo la pantalla
        Formulario.Procesando.desbloquear();
    }, "html");
};

AbmcDetalle.enviarModelo = function () {
    if (!Validacion.validar()) {
        return;
    }

    var urlPost = $.getUrl("/" + AbmcDetalle.controller + "/" + AbmcDetalle.estadoText);

    var campos = $(AbmcDetalle.divAbmc + " :input:disabled");
    $(campos).removeAttr("disabled");

    var datos = $("form").serializeArray();
    datos.push({ name: "idPadre", value: AbmcDetalle.grilla.Subgrilla.idExtendido });

    // Metodo que se llama antes de hacer el POST
    if (AbmcDetalle.preEnviarModelo) {
        AbmcDetalle.preEnviarModelo(datos);
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

                $(AbmcDetalle.btnAceptar).hide();
                $(AbmcDetalle.btnVolver).val("Volver");
                
				$(AbmcDetalle.divAbmc + " :input").not(AbmcDetalle.btnVolver).attr("disabled", "disabled");
            }
            else {
                $(campos).attr("disabled", "disabled");
				
                for (var i=0; i < data.details.length; i++) {
                    Mensaje.Error.agregarError(data.details[i]);
                }
            }

            // Metodo que se llama despues de hacer el POST
            if (AbmcDetalle.postEnviarModelo) {
                AbmcDetalle.postEnviarModelo(data);
            }
			
			// Desbloqueo la pantalla
			Formulario.Procesando.desbloquear();
        },
        "json");
};

AbmcDetalle.preCargarModelo = null;
AbmcDetalle.postCargarModelo = null;
AbmcDetalle.preEnviarModelo = null;
AbmcDetalle.postEnviarModelo = null;

AbmcDetalle.mostrarDivAbmc = function () {
    $(AbmcDetalle.divConsulta).hide();
    $(AbmcDetalle.divAbmc).show();

	Mensaje.ocultar();
    Validacion.init();
    Formulario.init();
};

AbmcDetalle.mostrarDivConsulta = function () {
    $(AbmcDetalle.divConsulta).show();
    $(AbmcDetalle.divAbmc + ", " + AbmcDetalle.divGrillaAbmc).hide();
    Mensaje.ocultar();

    if (!AbmcDetalle.sinConsulta) {
        AbmcDetalle.grilla.actualizar();
    }
};