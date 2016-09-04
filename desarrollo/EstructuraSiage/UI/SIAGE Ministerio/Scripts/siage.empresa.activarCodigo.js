var EmpresaActivarCodigo = {};

EmpresaActivarCodigo.init = function (instancia) {
    $("#btnCancelar").click(function () {
        $("#divVista").hide();
        $("#divActivacionCodigoEmpresa").hide();
        $(instancia.prefijo + "divDatosGeneralesEmpresa").hide();
        $(instancia.prefijo + "divFiltrosDeBusqueda").show();
        GrillaUtil.actualizar($(instancia.prefijo + "list"));
    });

    $("#btnAceptar").click(function () {        
        $.post($.getUrl("/GestionEmpresa/ActivarCodigoEmpresa"), { id: instancia.seleccion, codigoActivado: true },
                function (data) {
                    if (data.status) {
                        $('#btnAceptar').hide();
                        $('#btnCancelar').val("Volver");
                        $('#CodigoActivado').attr('disabled', true);
                        Mensaje.Exito.mostrar();
                    }
                    else {
                        for (var i = 0; i < data.details.length; i++) {
                            Mensaje.Error.agregarError(data.details[i]);
                        }
                    }
                }, "json");
    });
    Validacion.init();

};