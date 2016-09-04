var CierreEmpresa = {};
var InstanciaEditor = null;

CierreEmpresa.instanciaEmitirResolucion;
CierreEmpresa.instanciaAsignacionIL;

CierreEmpresa.htmlIL;
CierreEmpresa.htmlResolucion;

CierreEmpresa.init = function (instancia) {
    //CierreEmpresa.prefijo = prefijo || "";
    InstanciaEditor = instancia;

    if (GrillaUtil.getSeleccionFilas($(instancia.prefijo + "list"), false) != "")
        $("#IdEmpresa").val(GrillaUtil.getSeleccionFilas($(instancia.prefijo + "list"), false));

    $(".val-DateTime").datepicker({ currentText: 'Now', dateFormat: 'dd/mm/yy' });

    CierreEmpresa.instanciaEmitirResolucion = Resolucion.init("#divResolucionCierre", "Resolucion"); // EmpresaResolucionPuestosDeTrabajo.init(instancia, "Resolucion", "#divResolucionCierre");
    CierreEmpresa.instanciaAsignacionIL = AsignacionInstrumentoLegal.init("#divAsignacionInstrumentoLegalCierre", "AsignacionInstrumentoLegal");

    $("#btnVolver").hide();

    CierreEmpresa.manejoDeEventos();
}

//Método que contiene el manejo de los eventos
CierreEmpresa.manejoDeEventos = function () {
    $("#btnAceptar1").click(function () {
        CierreEmpresa.enviarModelo();
    });

    $("#btnCancelar").click(function () {
        $("#divCierreEmpresa").hide();
        $(instancia.prefijo + "divDatosGeneralesEmpresa").hide();
        $(instancia.prefijo + "divFiltrosDeBusqueda").show();
        GrillaUtil.actualizar(instancia.grilla);
    });

    $("#btnVolver").click(function () {
        $("#divCierreEmpresa").hide();
        $(instancia.prefijo + "divDatosGeneralesEmpresa").hide();
        $(instancia.prefijo + "divFiltrosDeBusqueda").show();
        GrillaUtil.actualizar(instancia.grilla);
    });

    $("#EmitirResolucionDeCierre").changePatch(function () {
        if ($("#EmitirResolucionDeCierre").is(":checked")) {
            $(CierreEmpresa.instanciaEmitirResolucion.prefijo + "divResolucion").show()
            InstrumentoLegal.cambiarEstado(CierreEmpresa.instanciaEmitirResolucion.asignacion.instrumento, "Registrar");
            $(CierreEmpresa.instanciaEmitirResolucion.asignacion.instrumento.prefijo + "btnBusqueda").hide();
            $(CierreEmpresa.instanciaEmitirResolucion.asignacion.instrumento.prefijo + "IdTipoInstrumentoLegal").val(3);
            $(CierreEmpresa.instanciaEmitirResolucion.asignacion.instrumento.prefijo + "IdTipoInstrumentoLegal").attr("disabled", "disabled");
        }
        else {
            $(CierreEmpresa.instanciaEmitirResolucion.asignacion.instrumento.prefijo + "IdTipoInstrumentoLegal").val("");
        }
    });

    $("#divResolucionCierre :input[type!='button']").val("");
    $("#divAsignacionInstrumentoLegalCierre :input[type!='button']").val("");

    $("#divResolucionCierre").editorOpcional("#EmitirResolucionDeCierre");
    $("#divAsignacionInstrumentoLegalCierre").editorOpcional("#EmitirResolucionDeCierre", true);
    $("#EmitirResolucionDeCierre").changePatch();
}

CierreEmpresa.enviarModelo = function () {
    if (!Validacion.validar()) {
        return;
    }

    if ($("#EmitirResolucionDeCierre").is(":checked")) {
        $(CierreEmpresa.instanciaEmitirResolucion.asignacion.instrumento.prefijo + "divVista :input").removeAttr("disabled");
    }
    else {
        $(CierreEmpresa.instanciaAsignacionIL.instrumento.prefijo + "divVista :input").removeAttr("disabled");
    }

    var datos = $("#divAbmc form").serializeArray();
    datos = datos.filter(function (campo) {
        return (campo.value !== "" && campo.value !== null && campo.value !== undefined);
    });

    if ($("#EmitirResolucionDeCierre").is(":checked")) {
        $(CierreEmpresa.instanciaEmitirResolucion.asignacion.instrumento.prefijo + "divVista :input").attr("disabled", "disabled");
    }
    else {
        $(CierreEmpresa.instanciaAsignacionIL.instrumento.prefijo + "divVista :input").attr("disabled", "disabled");
    }

    $.post($.getUrl("/GestionEmpresa/CerrarEmpresa"), datos,
        function (data) {
            if (data.status) {
                Mensaje.Exito.mostrar();
                $("#btnAceptar1").hide();
                $("#divCierreEmpresa").show();
                $("#divCierreEmpresa").attr("disabled", "disabled");
                $("#divCierreEmpresa form :input, #divCierreEmpresa form textarea").attr("disabled", "disabled");
                $("#btnVolver").show();
                $("#btnCancelar").hide();
            }
            else {
                //CierreEmpresa.restaurarDivs();
                Mensaje.Error.limpiar();

                for (var i = 0; i < data.details.length; i++) {
                    Mensaje.Error.agregarError(data.details[i]);
                }
            }
        }, "json");
};