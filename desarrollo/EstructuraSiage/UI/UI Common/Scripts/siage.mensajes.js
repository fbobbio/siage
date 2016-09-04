var Mensaje = {};
Mensaje.classDiv = ".confirmacion";
Mensaje.classBtn = ".confirmacion-btnCerrar";

Mensaje.ocultar = function (btnCerrar) {
    if (btnCerrar) {
        $(btnCerrar).parent("div").parent("div").slideUp();
        $(Mensaje.Error.div + " ul").html("");
    }
    else {
        $(Mensaje.classDiv).slideUp();
        $(Mensaje.Error.div + " ul").html("");
    }
};


Mensaje.mostrarSinOcultar = function (div,texto) { 
    if (div) {
        $(div + " span").html(texto);
        $("html, body").animate({ scrollTop: 0 }, "slow");

        $(div).attr("style", "display: none;");
        $(div).fadeIn();
    }
}

Mensaje.mostrar = function (div, texto) {
    Mensaje.ocultar();

    if (div) {
        $(div + " span").html(texto);
        $("html, body").animate({ scrollTop: 0 }, "slow");

        $(div).attr("style", "display: none;");
        $(div).fadeIn();
    }
};

Mensaje.cargando = {
    _html: (function () {
        var img = $.getUrl("/Content/images/24min.gif");
        var div = document.createElement('div');

        div.estaAgregado = false;
        div.className = 'mensajeCargando';
        div.innerHTML = '<img src="' + img + '"/><label>Cargando...</label>';
        return div;
    })(),
    mostrar: function () {
        if (!Mensaje.cargando._html.estaAgregado) {
            document.body.appendChild(Mensaje.Cargando._html);
            Mensaje.cargando._html.estaAgregado = true;
        }
        Mensaje.cargando._html.style.display = 'none';
    },
    ocultar: function () {
        Mensaje.cargando._html.style.display = 'false';
    }
}
//====================================================== MENSAJE DE EXITO ======================================================= //

Mensaje.Exito = {};
Mensaje.Exito.div = "#divMensajeExito";
Mensaje.Exito.texto = "La operación ha finalizado exitosamente";

Mensaje.Exito.mostrar = function () {
    Mensaje.ocultar();
    Mensaje.mostrar(Mensaje.Exito.div, Mensaje.Exito.texto);
};

//====================================================== MENSAJE DE ERROR ======================================================= //

Mensaje.Error = {};
Mensaje.Error.div = "#divMensajeError";
Mensaje.Error.texto = "La operación no se ha completado.";

Mensaje.Error.mostrar = function () {
    Mensaje.mostrar(Mensaje.Error.div, Mensaje.Error.texto);
};

Mensaje.Error.limpiar = function () {
    $(Mensaje.Error.div + " ul").html("");
    $(Mensaje.Error.div).hide();
};

Mensaje.Error.agregarError = function (texto) {
    if ($(Mensaje.Error.div).is(":hidden")) {
		
        Mensaje.Error.mostrar();
    }
    $(Mensaje.Error.div + " ul").append("<li>" + texto + "</li>");
};

Mensaje.Error.agregarArrayError = function (array) {
    $.each(array, Mensaje.Error.agregarError);
};

//================================================== MENSAJE DE CONFIRMACION ==================================================== //

Mensaje.Advertencia = {};
Mensaje.Advertencia.div = "#divMensajeAdvertencia";
Mensaje.Advertencia.texto = "¿Confirma que desea realizar esta operación?";
Mensaje.Advertencia.botones = true;

Mensaje.Advertencia.mostrar = function () {
    
    if (Mensaje.Advertencia.botones)
        $(".confirmacion-botones").show();
    else
        $(".confirmacion-botones").hide();

    Mensaje.mostrar(Mensaje.Advertencia.div, Mensaje.Advertencia.texto);
};

Mensaje.Advertencia.aceptar = null;
Mensaje.Advertencia.cancelar = null;