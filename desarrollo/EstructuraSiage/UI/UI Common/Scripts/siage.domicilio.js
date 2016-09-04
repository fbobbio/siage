var Domicilio = {};


Domicilio.init = function (div, prefijo) {
    var instancia = {};
    instancia.div = div;
    if (prefijo) {
        $(div).agregarPrefijo(prefijo);
        instancia.prefijo = "#" + prefijo + "_";
    }
    else {
        instancia.prefijo = "#";
    }

    Domicilio.cargarComportamientoCombos(instancia);
    $(instancia.prefijo + "divCombos :input").attr("disabled", "disabled");
    $(instancia.prefijo + "Altura").numeric();
    return instancia;
};

Domicilio.cargarComportamientoCombos = function (instancia) {
    $(instancia.prefijo + "idPais").changePatch(function () {
        var pais = $(instancia.prefijo + "idPais").val();
        if (!pais) {
            return;
        }
        Domicilio.CargarProvinciaByPais(instancia, pais);
    });

    $(instancia.prefijo + "IdProvincia").changePatch(function () {
        var provincia = $(instancia.prefijo + "IdProvincia").val();
        if (!provincia) {
            return;
        }
        Domicilio.CargarDepartamentoLocalidadByProvincia(instancia, provincia);
    });

    $(instancia.prefijo + "IdLocalidad").changePatch(function () {
        var localidad = $(instancia.prefijo + "IdLocalidad").val();
        Domicilio.CargarBarrioByLocalidad(instancia, localidad);
    });

    $(instancia.prefijo + "IdTipoCalle").changePatch(function () {
        var localidad = $(instancia.prefijo + "IdLocalidad").val();
        var tipoCalle = $(instancia.prefijo + "IdTipoCalle").val();
        Domicilio.CargarCalleByTipoCalleYLocalidad(instancia, localidad, tipoCalle);
    });

    $(instancia.prefijo + "IdBarrio").changePatch(function () {
        $(instancia.prefijo + "IdTipoCalle").attr("disabled", false);
    });
};

Domicilio.CargarProvinciaByPais = function (instancia, id) {
    $.getJSON($.getUrl('/Domicilio/CargarProvinciaByPais'), { idPais: id }, function (provincias) {
        $(instancia.prefijo + "IdProvincia").cargarCombo(provincias, "Id", "Nombre");
        $(instancia.prefijo + "IdDepartamentoProvincial").val("").attr("disabled", "disabled");
        $(instancia.prefijo + "IdLocalidad").val("").attr("disabled", "disabled");
        $(instancia.prefijo + "IdBarrio").val("").attr("disabled", "disabled");
        $(instancia.prefijo + "IdCalle").val("").attr("disabled", "disabled");
        $(instancia.prefijo + "IdTipoCalle").val("").attr("disabled", "disabled");
    });
};

Domicilio.CargarDepartamentoLocalidadByProvincia = function (instancia, id) {
    $.getJSON($.getUrl('/Domicilio/CargarDepartamentoLocalidadByProvincia'), { idProvincia: id }, function (data) {
        $(instancia.prefijo + "IdDepartamentoProvincial").unbind("change");
        $(instancia.prefijo + "IdDepartamentoProvincial").cargarCombo(data.Departamentos, "Id", "Nombre");
        $(instancia.prefijo + "IdLocalidad").cargarCombo(data.Localidades, "Id", "Nombre");
        $(instancia.prefijo + "IdLocalidad").data("Localidades", data.Localidades);

        $(instancia.prefijo + "IdDepartamentoProvincial").changePatch(function () {
            var idDpto = $(instancia.prefijo + "IdDepartamentoProvincial").val();
            if (!idDpto) {
                return;
            }

            $.getJSON($.getUrl('/Domicilio/CargarLocalidadByDepartamentoProvincial'), { idDepartamento: idDpto }, function (localidades) {
                $(instancia.prefijo + "IdLocalidad").cargarCombo(localidades, "Id", "Nombre");
                $(instancia.prefijo + "IdLocalidad").data("Localidades", localidades);
            });
        });
    });
};

Domicilio.CargarDepartamentoLocalidadByProvinciaPorDefecto = function (instancia) {
    $.getJSON($.getUrl('/Domicilio/CargarDepartamentoLocalidadByProvinciaPorDefecto'), null, function (data) {
        $(instancia.prefijo + "idPais").val("ARG");
        $(instancia.prefijo + "IdProvincia").cargarCombo(data.Provincias, "Id", "Nombre");
        $(instancia.prefijo + "IdProvincia").val("X");
        $(instancia.prefijo + "IdDepartamentoProvincial").unbind("change");
        $(instancia.prefijo + "IdDepartamentoProvincial").cargarCombo(data.Departamentos, "Id", "Nombre");

        $(instancia.prefijo + "IdDepartamentoProvincial").changePatch(function () {
            var idDpto = $(instancia.prefijo + "IdDepartamentoProvincial").val();
            if (!idDpto) {
                return;
            }

            $.getJSON($.getUrl('/Domicilio/CargarLocalidadByDepartamentoProvincial'), { idDepartamento: idDpto }, function (localidades) {
                $(instancia.prefijo + "IdLocalidad").cargarCombo(localidades, "Id", "Nombre");
                $(instancia.prefijo + "IdLocalidad").data("Localidades", localidades);
            });
        });
    });
};

Domicilio.CargarBarrioByLocalidad = function (instancia, idLocalidad) {
    $.getJSON($.getUrl('/Domicilio/CargarBarrioByLocalidad'), { idLocalidad: idLocalidad }, function (dataBarrio) {
        $(instancia.prefijo + "IdBarrio").cargarCombo(dataBarrio, "Id", "Nombre");
    });
};

Domicilio.CargarCalleByTipoCalleYLocalidad = function (instancia, idLocalidad, idTipoCalle) {
    $.getJSON($.getUrl('/Domicilio/CargarCalleByTipoCalleYLocalidad'), { idLocalidad: idLocalidad, idTipoCalle: idTipoCalle }, function (dataCalle) {
        $(instancia.prefijo + "IdCalle").cargarCombo(dataCalle, "Id", "Nombre");
    });
};

Domicilio.Cargar = function (instancia, idDomicilio) {
    $.getJSON($.getUrl('/Domicilio/GetDomicilioById'), { id: idDomicilio }, function (domicilio) {
        $.formatoFormulario(domicilio, instancia.prefijo + "Domicilio_");
    });
};

Domicilio.cargarDomicilio = function (instancia, id, estado) {
    if (!id || id == 0) {
        return;
    }

    $.get($.getUrl("/Domicilio/GetDomicilioCompletoById"), { id: id }, function (domicilio) {
        $(instancia.prefijo + "IdProvincia").cargarCombo(domicilio.Provincias, "Id", "Nombre");
        $(instancia.prefijo + "IdDepartamentoProvincial").cargarCombo(domicilio.Departamentos, "Id", "Nombre");
        $(instancia.prefijo + "IdLocalidad").cargarCombo(domicilio.Localidades, "Id", "Nombre");
        $(instancia.prefijo + "IdBarrio").cargarCombo(domicilio.Barrios, "Id", "Nombre");
        $(instancia.prefijo + "IdCalle").cargarCombo(domicilio.Calles, "Id", "Nombre");

        $.formatoFormulario(domicilio, (instancia.prefijo).substring(1));

        switch (estado) {
            case "Registrar":
            case "Editar":
                $(":input").regex("id", new RegExp("^" + (instancia.prefijo).substring(1) + ".*")).removeAttr("disabled");
                break;

            case "Eliminar":
            case "Ver":
                $(":input").regex("id", new RegExp("^" + (instancia.prefijo).substring(1) + ".*")).attr("disabled", "disabled");
                break;
        }
    });
};