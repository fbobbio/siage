var EmpresaVisarActivacionEmpresa = {};

EmpresaVisarActivacionEmpresa.init = function (instancia) {

    $(".val-DateTime").mask("99/99/9999", { placeholder: " " });
    $(".val-DateTime").datepicker({ currentText: 'Now', dateFormat: 'dd/mm/yy' });

    var id = $(instancia.prefijo + "Id").val();
    if (id) {
        //ocultar consulta y mostrar en modo solo lectura
    }

    EmpresaVisarActivacionEmpresa.AccionesVisadoAQuitar();
    EmpresaVisarActivacionEmpresa.ManejoDeEventos(instancia);
};

EmpresaVisarActivacionEmpresa.AccionesVisadoAQuitar = function () {
    $.get($.getUrl("/GestionEmpresa/AccionesVisadoAQuitar/"), {}, function (data) {
        if (data) {
            for (var i = 0; i < data.length; i++) {
                $('#Accion option[value=' + data[i] + ']').remove();

            }
        }
    });
};

EmpresaVisarActivacionEmpresa.ManejoDeEventos = function (instancia) {
    $("#btnAceptarVisadoActivacion").click(function () {
        if (!Validacion.validar()) {
            return;
        }
        $.post($.getUrl("/GestionEmpresa/VisarActivacion"), $("form :input").serializeArray(),
                function (data) {
                    if (data.status) {
                        Mensaje.Exito.mostrar();
                        $('#ObservacionesVisarActivacion').attr('disabled', 'disabled');
                        $('#Accion').attr('disabled', 'disabled');
                        $('#btnAceptarVisadoActivacion').hide();
                        $('#btnCancelarVisadoActivacion').val('Volver');
                    }
                    else {
                        for (var i = 0; i < data.details.length; i++) {
                            Mensaje.Error.agregarError(data.details[i]);
                        }
                    }
                }, "json");
    });

    $("#btnCancelarVisadoActivacion").click(function () {
        $("#divAbmc").hide();
        $(instancia.prefijo + "divDatosGeneralesEmpresa").hide();
        $(instancia.prefijo + "divFiltrosDeBusqueda").show();
        if ($('#btnCancelarVisadoActivacion').val() === 'Volver') {
            GrillaUtil.actualizar($(instancia.prefijo + "list"));
        }
    });
};