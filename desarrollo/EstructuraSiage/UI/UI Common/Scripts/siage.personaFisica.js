var PersonaFisica = {};
PersonaFisica.validarPersona = null // funcion que debe ser implementada por quien use este control, solo si es necesario
PersonaFisica.trajoDatosRCivil;

PersonaFisica.init = function (div, prefijo) {
    var instancia = {};
    instancia.div = div;

    if (prefijo) {
        $(div).agregarPrefijo(prefijo);
        instancia.prefijo = "#" + prefijo + "_";
    }
    else {
        instancia.prefijo = "#";
    }

    PersonaFisica.inicializarBotones(instancia);
    PersonaFisica.inicializarValidaciones(instancia);
    PersonaFisica.inicializarCombos(instancia);
    PersonaFisica.inicializarDomicilio(instancia);
    PersonaFisica.inicializarCheckbox(instancia);

    return instancia;
};

PersonaFisica.inicializarValidaciones = function (instancia) {
    $(instancia.prefijo + "NumeroDocumento").numeric();
    $(instancia.prefijo + "Apellido").alpha({ allow: " .,'" });
    $(instancia.prefijo + "Nombre").alpha({ allow: " .,'" });
};

PersonaFisica.inicializarBotones = function (instancia) {
    $(instancia.prefijo + "btnBuscarPF").click(function () {
        var id = $(instancia.prefijo + "txtId").val();
        var tipoDocumento = $(instancia.prefijo + "cmbTipoDocumento").val();
        var nroDocumento = $(instancia.prefijo + "txtNumeroDocumento").val();
        var sexo = $(instancia.prefijo + "cmbSexo").val();

        if (PersonaFisica.validarPersona) {
            if (!PersonaFisica.validarPersona(instancia, id, nroDocumento, tipoDocumento)) {
                return;
            }
        }

        if ((tipoDocumento && nroDocumento) || id) {
            $.getJSON($.getUrl('/PersonaFisica/GetPersonaFisicaByFiltros'),
            {
                'id': id,
                'tipoDocumento': tipoDocumento,
                'nroDocumento': nroDocumento,
                'sexo': sexo
            },
            function (data) {
                if (!data) {
                    PersonaFisica.trajoDatosRCivil = false;
                    Mensaje.Advertencia.botones = false;
                    Mensaje.Advertencia.texto = "La búsqueda de persona no produjo ningún resultado.";
                    Mensaje.Advertencia.mostrar();
                }
                else {
                    PersonaFisica.trajoDatosRCivil = true;
                    PersonaFisica.cargarPersonaFisica(instancia, data);
                }
            });
        }
        else {
            Mensaje.Advertencia.botones = false;
            Mensaje.Advertencia.texto = "No ha ingresado los datos requeridos para una búsqueda de persona";
            Mensaje.Advertencia.mostrar();
        };
    });

    $(instancia.prefijo + "btnNuevoPF").click(function () {
        $(instancia.prefijo + "divFormularioPF").show();
        $(instancia.prefijo + "divConsultaPF").hide();
        $(instancia.prefijo + "divDomicilioCheckBox").show();
        PersonaFisica.nuevaPF(instancia);
        PersonaFisica.habilitarCampos(instancia);
    });

    $(instancia.prefijo + "btnEditarPF").click(function () {
        PersonaFisica.habilitarCampos(instancia);
        $(instancia.prefijo + "TipoDocumento").attr('disabled', 'disabled');
        $(instancia.prefijo + "NumeroDocumento").attr('disabled', 'disabled');
        $(instancia.prefijo + "Sexo").attr('disabled', 'disabled');
        $(instancia.prefijo + "IdPaisNacionalidad").attr('disabled', 'disabled');
        $(instancia.prefijo + "divDomicilioCheckBox").show();
    });

    $(instancia.prefijo + "btnLimpiarPF").click(function () {
        PersonaFisica.limpiarCampos(instancia);
        PersonaFisica.modoConsulta(instancia);
    });
};

PersonaFisica.modoConsulta = function (instancia) {
    $(instancia.prefijo + "divConsultaPF").show();
    $(instancia.prefijo + "divFormularioPF").hide();
    $(instancia.prefijo + "cmbTipoDocumento").val("");
    $(instancia.prefijo + "txtNumeroDocumento").val("");
    $(instancia.prefijo + "cmbSexo").val("");
};

PersonaFisica.inicializarCombos = function (instancia) {

    //Combo provincia depende del valor de pais de origen
    $(instancia.prefijo + "ProvinciaNacimiento").CascadingDropDown(instancia.prefijo + "IdPaisOrigen",
        $.getUrl('/PersonaFisica/CargarProvinciaByPaisOrigen'), {
            promptText: 'SELECCIONE',
            postData: function () {
                return { idPais: $(instancia.prefijo + "IdPaisOrigen").val() };
            }
        });

    //Combo departamento depende del valor de provincia
    $(instancia.prefijo + "DepartamentoProvincialNacimiento").CascadingDropDown(instancia.prefijo + "ProvinciaNacimiento",
        $.getUrl('/PersonaFisica/CargarDepartamentoProvincialByProvincia'), {
            promptText: 'SELECCIONE',
            postData: function () {
                return { idProvincia: $(instancia.prefijo + "ProvinciaNacimiento").val() };
            }
        });

    //Combo localidad depende del valor de departamento
    $(instancia.prefijo + "LocalidadNacimiento").CascadingDropDown(instancia.prefijo + "DepartamentoProvincialNacimiento",
        $.getUrl('/PersonaFisica/CargarLocalidadByDepartamentoProvincial'), {
            promptText: 'SELECCIONE',
            postData: function () {
                return { idDepartamento: $(instancia.prefijo + "DepartamentoProvincialNacimiento").val() };
            }
        });
}

PersonaFisica.inicializarDomicilio = function (instancia) {
    htmlDomicilio = $(instancia.prefijo + "divDomicilioPF").html();
    if (htmlDomicilio) {
        instancia.htmlDomicilio = htmlDomicilio;
    }
    $(instancia.prefijo + "divDomicilioPF").html("");
};

PersonaFisica.inicializarCheckbox = function (instancia) {
    //Check domicilio
    $(instancia.prefijo + "ckRegistrarDomicilioPF").changePatch(function () {
        if ($(instancia.prefijo + "ckRegistrarDomicilioPF").is(":checked")) {
            $(instancia.prefijo + "divDomicilioPF").html(instancia.htmlDomicilio);
            $(instancia.prefijo + "divDomicilioPF").show();

            Domicilio.init("#divDomicilioPF", instancia.prefijo.replace("#", "") + "Domicilio");
        }
        else {
            PersonaFisica.inicializarDomicilio(instancia);
        }
    });

    //Check indocumentado
    $(instancia.prefijo + "ckIndocumentado").changePatch(function () {
        if ($(instancia.prefijo + "ckIndocumentado").is(":checked")) {
            $(instancia.prefijo + "TipoDocumento").attr("disabled", "disabled").val('INDOCUMENTADO'); //recupera de gobierno
            $(instancia.prefijo + "NumeroDocumento").attr("disabled", "disabled").val(9999);
            $(instancia.prefijo + "OrganismoEmisorDocumento").attr("disabled", "disabled").val("");
        }
        else {
            $(instancia.prefijo + "TipoDocumento").removeAttr("disabled").val("");
            $(instancia.prefijo + "NumeroDocumento").removeAttr("disabled").val("");
            $(instancia.prefijo + "OrganismoEmisorDocumento").removeAttr("disabled").val("");
        }
    });

    //Chek trajo registro de rcivil
    $(instancia.prefijo + "EsDeRCivil").hide();
}

PersonaFisica.cargarPersonaFisica = function (instancia, data) {
    $.formatoFormulario(data, instancia.prefijo.substring(1));
    $(instancia.prefijo + "ProvinciaNacimiento").cargarCombo([data.Provincia], "Id", "Nombre");
    $(instancia.prefijo + "DepartamentoProvincialNacimiento").cargarCombo([data.DepartamentoProvincial], "Id", "Nombre");
    $(instancia.prefijo + "LocalidadNacimiento").cargarCombo([data.Localidad], "Id", "Nombre");

    $(instancia.prefijo + "ProvinciaNacimiento").val(data.Provincia.Id);
    $(instancia.prefijo + "DepartamentoProvincialNacimiento").val(data.DepartamentoProvincial.Id);
    $(instancia.prefijo + "LocalidadNacimiento").val(data.Localidad.Id);

    $(instancia.prefijo + "divConsultaPF").hide();
    $(instancia.prefijo + "divFormularioPF").show();
    $(instancia.prefijo + "divFormularioPF p.botones").show();
    $(instancia.prefijo + "EsDeRCivil").attr("checked", data.EsDeRCivil);

    if (data.Domicilio) {
        $(instancia.prefijo + "divDomicilioCheckBox").show();
        $(instancia.prefijo + "ckRegistrarDomicilioPF").attr("checked", "checked").changePatch();
        $(instancia.prefijo + "ckRegistrarDomicilioPF").attr("disabled", "disabled");
        var instanciaDomicilio = {};
        instanciaDomicilio.prefijo = instancia.prefijo + "Domicilio_";
        if (data.Domicilio.Id && data.Domicilio.Id > 0) {
            Domicilio.cargarDomicilio(instanciaDomicilio, data.Domicilio.Id, "Ver");
        }
        else if (data.Domicilio > 0) {
            Domicilio.cargarDomicilio(instanciaDomicilio, data.Domicilio, "Ver");
        }
    };

    PersonaFisica.deshabilitarCampos(instancia);
};

PersonaFisica.cargarPersonaById = function (div, prefijo, estado, id) {
    $.get($.getUrl("/PersonaFisica/GetPersonaPartialViewById"),
		{ id: id, estadoText: estado },
		function (data) {
		    $(div).html(data);
            var instanciaPersona = PersonaFisica.init(div, prefijo);
            $(div).data("persona", instanciaPersona);
		    $(instanciaPersona.prefijo + "divConsultaPF").hide();
		    $(instanciaPersona.prefijo + "divFormularioPF").show();
		    $(prefijo + "divConsultaPF").hide();
		    $(prefijo + "divFormularioPF").show();
		},
		"html"
	);
}

PersonaFisica.limpiarCampos = function (instancia) {
    $(instancia.prefijo + "divFormularioPF :input").val("");
    $(instancia.prefijo + "Id").val("");
    $(instancia.prefijo + "ckRegistrarDomicilioPF").attr("checked", false).changePatch();
};

PersonaFisica.nuevaPF = function (instancia) {
    $(instancia.prefijo + "divFormularioPF :input").val("");
    $(instancia.prefijo + "Id").val("0");
};

PersonaFisica.habilitarCampos = function (instancia) {
    $(instancia.prefijo + "divFormularioPF :input").removeAttr("disabled");
};

PersonaFisica.deshabilitarCampos = function (instancia) {
    $(instancia.prefijo + "divFormularioPF :input[type!='button']").attr("disabled", "disabled");
    $(instancia.div + " button").removeAttr("disabled");
};