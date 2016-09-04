var AbmcUtil = {};

AbmcUtil.limpiarFormulario = function (div) {
    var campos = div + " fieldset :input";

    $(campos).each(function () {
        var type = this.type;
        var tag = this.tagName.toLowerCase();

        if (type === "text" || type === "password" || type === "hidden" || tag === "textarea") {
            this.value = "";
        }
        else if (type === "checkbox" || type === "radio") {
            this.checked = false;
        }
        else if (tag === "select") {
            this.selectedIndex = 0;
        }
    });
};

AbmcUtil.submitOnEnter = function (e) {
    var key, datepicker, textarea;
    var habilidado = $("#btnAceptar").is(":enabled");

    if (e.originalTarget) {
        var datepicker = e.originalTarget.classList.contains("val-DateTime");
        var textarea = e.originalTarget.type === "textarea";
    }
    else if(e.target) {
        var datepicker = e.target.className.indexOf("val-DateTime") != -1;
        var textarea = e.target.type === "textarea";
    }

    if (window.event) {
        key = window.event.keyCode; //IE   
    }
    else {
        key = e.which; //FF
    }
    if (key === 13 && !datepicker && !textarea && habilidado) {
        //$("#btnAceptar").click();
        return false;
    }
    else {
        return true;
    }
};

AbmcUtil.formToJSON = function (selector) {
    var arr = $(selector).serializeArray();
    var json = "";

    jQuery.each(arr, function () {
        jQuery.each(this, function (i, val) {
            if (i === "name") {
                json += '"' + val + '" : ';
            } else if (i === "value") {
                json += '"' + val.replace(/"/g, '\\"') + '",';
            }
        });
    });

    json = "{" + json.substring(0, json.length - 1) + "}";
    return jQuery.parseJSON(json);
};

AbmcUtil.mensajeSeleccion = function () {
    Mensaje.Advertencia.botones = false;
    Mensaje.Advertencia.texto = "Debe seleccionar algún elemento de la grilla para poder realizar esta operación."
    Mensaje.Advertencia.mostrar();
};