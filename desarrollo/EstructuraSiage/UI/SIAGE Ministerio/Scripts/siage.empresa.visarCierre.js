var EmpresaVisarCierre = {};

EmpresaVisarCierre.init = function () {
    $("label[for='Id']").html("Codigo pedido(*)");
    $("label[for='FechaCierreEmpresa']").html("Fecha de cierre empresa(*)");
    $("#DesvincularEdificio").removeAttr('disabled');
    $('#DesvincularEdificio').attr('checked', true);
    $("#divDesvincularEdificio").hide();

    //Si el valor del parametro booleano desvincular edificio es "SI" entonces, marco el check y lo deshabilito.
    EmpresaVisarCierre.VerificarValorBooleano();

    //Navegabilidad del div VerificarEstado
    $("#divVerificarEstado").hide();
    $("#Rechazado").removeAttr("disabled");
    $("#ObservacionesRechazo").removeAttr("disabled");
    $("label[for='Rechazado']").html("Autorizada");
    $("#divObsrRechazo").hide();

    if ($("#EstadoPedido").val() == "GENERADO") {
        $("#divDesvincularEdificio").show();
        $("#btnAceptarVisado").removeAttr("disabled");
    }
    else {
        $("#divDesvincularEdificio").hide();
    }

    if ($("#EstadoEmpresa").val() == "EN_PROCESO_DE_CIERRE") {
        $("#divVerificarEstado").show();
    }
    $("#divVerificarEstado").find("input[type='radio']").changePatch(function () {
        if ($("#rdbRechazada").attr("checked")) {
            $("#divObsrRechazo").show();
        }
        else {
            $("#divObsrRechazo").hide();
        }
    });

    $("#btnCancelarVisado").click(function () {
        $("#divAbmc").hide();
        $("#divConsulta").show();
        Mensaje.ocultar();
    });

    $("#btnAceptarVisado").click(function () {
        $("form :input").removeAttr("disabled");
        if (!Validacion.validar()) {
            //$("form :input").attr("disabled", "disabled");
            //$("form :input").setEnabled(false);
            $("#divAbmc").setEnabled(false);
            $('#btnAceptarVisado').removeAttr('disabled');
            $('#btnCancelarVisado').removeAttr('disabled');
            return;
        }
        var data = $("form").serializeArray();
        var rechazada = false;
        if ($("#rdbRechazada").attr("checked")) {
            rechazada = true;
        }
        var idPedido = $("#Id").val();
        data.push({ name: "Rechazado", value: rechazada });
        data.push({ name: "IdPedido", value: idPedido });
        $.post($.getUrl("/PedidoAutorizacionCierre/VisarCierreEmpresa"), data,
                function (data) {
                    if (data.status) {
                        $("form :input").attr("disabled", "disabled");
                        if ($("#rdbRechazada").attr("checked") == true) {
                            Mensaje.Exito.texto = "La operación se completo con éxito. <br /> El estado de la solicitud de cierre: APROBADO por lo tanto no se registra la desvinculación de edificio/s con la empresa.";
                        }
                        else {
                            Mensaje.Exito.texto = "La operación se completo con éxito.";
                        }
                        Mensaje.Exito.mostrar();
                        $("#btnCancelarVisado").removeAttr("disabled");
                        $('#btnAceptarVisado').hide();
                        $('#btnCancelarVisado').val('Volver');
                        $('#btnLimpiar').click();
                    }
                    else {
                        for (var i = 0; i < data.details.length; i++) {
                            Mensaje.Error.agregarError(data.details[i]);
                        }
                        $("form :input").attr("disabled", "disabled");
                        $("#btnCancelarVisado").removeAttr("disabled");
                        $("#divDesvincularEdificio :input").attr("disabled", false);
                        $("#divVerificarEstado :input").attr("disabled", false);
                        $('#btnAceptarVisado').attr("disabled", false);
                    }
                }, "json");
    });

};

//Si el valor del parametro booleano desvincular edificio es "SI" entonces, marco el check y lo deshabilito.
EmpresaVisarCierre.VerificarValorBooleano = function () {
    $.get($.getUrl("/PedidoAutorizacionCierre/GetParametroDesvinculacionEdificio"), {},
            function (data) {
                if (data != null) {
                    if (data === "Y") {
                        $("#DesvincularEdificio").attr("checked", true);
                        $("#DesvincularEdificio").attr("disabled", true);
                    }
                }
            }, "json");
};