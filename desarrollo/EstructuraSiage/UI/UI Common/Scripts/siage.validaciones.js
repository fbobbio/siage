var Validacion = {};

Validacion.Mensaje = {};
Validacion.Mensaje.required = "El campo {0} es requerido";
Validacion.Mensaje.range = "El campo {0} está fuera del rango permitido";
Validacion.Mensaje.formatoNoValidoCampoFecha = "Formato de fechas no valido para el campo {0}";

Validacion.init = function () {
    Mensaje.Error.limpiar();

    // Validaciones de campos numericos (enteros)
    $(".val-Int16, .val-Int32, .val-Int64").numeric({ ichars: " ñ<>,.{´+}¿'|;:_[*¨]¡?=)(&%$#!°^~`¬/@\"\\áéýúíóäëÿüïö⅓⅔⅛⅜⅝⅞¼¾½" });

    // Validaciones de campos numericos (decimales)
    $(".val-Single, .val-Double, .val-Float, .val-Decimal").numeric({ ichars: " ,ñ<>{´+}¿'|;:_[*¨]¡?=)(&%$#!°^~`¬/@\"\\áéýúíóäëÿüïö⅓⅔⅛⅜⅝⅞¼¾½" });

    // Validaciones de campos de fecha
    $(".val-DateTime").mask("99/99/9999", { placeholder: " " });
    $(".val-DateTime").datepicker({
        currentText: 'Now',
        dateFormat: 'dd/mm/yy',
        changeYear: true,
        yearRange: (new Date().getFullYear() - 80) + ":" + (new Date().getFullYear() + 80)
    });

    // Validaciones de campos de hora
    $(".val-Time").mask("99:99", { placeholder: " " });

    // Validaciones de campos de texto
    $(".val-String").alphanumeric({ ichars: "<>&" });
    $(".val-PhoneNumber").numeric({ ichars: "ñ<>&" });
    $(".val-EmailAddress").alphanumeric({ allow: "-_./@", ichars: "<>&" });
    $(".val-Url").alphanumeric({ ichars: "<>" });
	
	// Validaciones de largo de cadena
    $(":input").regex("class", new RegExp(".*val-MaxLength.*")).each(function (index, input) {
        var clases = $(this).attr("class");
        var maxlength = clases.substring(clases.indexOf("val-MaxLength") + 13, clases.length);

        $(this).attr("maxlength", maxlength);
    });
	
	// Elimino los ENTER de las areas de texto
	$("textarea").focusout(function () {
		var texto = $(this).val();
		texto = texto.replace(/\n/g, " ");
		texto = texto.replace(/  /g, "");
		$(this).val(texto);
	});
};

Validacion.validar = function (div) {
    Mensaje.Error.limpiar();

    // Validaciones de campos numericos (enteros)
    if (div) {
        div = div + " ";
    }
    else {
        div = "";
    }
    $(div + ".val-Int16").each(function () {
        Validacion.range(this, -32768, 32767);
    });

    $(div + ".val-Int32").each(function () {
        Validacion.range(this, -2147483648, 2147483647);
    });

    $(div + ".val-Int64").each(function () {
        Validacion.range(this, -9223372036854775808, 9223372036854775807);
    });

    // Validaciones de campos numericos (decimales)
    $(div + ".val-Single").each(function () {
        Validacion.range(this, -9999999, 9999999);
    });

    $(div + ".val-Double").each(function () {
        Validacion.range(this, -9999999, 9999999);
    });

    $(div + ".val-Float").each(function () {
        Validacion.range(this, -9999999, 9999999);
    });

    $(div + ".val-Decimal").each(function () {
        Validacion.range(this, -9999999, 9999999);
    });

    // Validaciones de campos requeridos
    $(div + ".val-Required").not(":hidden").each(function () {
        Validacion.required(this);
    });
    $(div + ".val-DateTime").not(":hidden").each(function () {

        if (!Validacion.Fecha(this) && $(this).val() != "") {
            Validacion.formatError(Validacion.Mensaje.formatoNoValidoCampoFecha, this);
        }
    });
    return $(Mensaje.Error.div + " li").length == 0;
};

Validacion.addError = function (mensaje) {
    mensaje = mensaje.replace(" 12:00:00 a.m.", "");
    mensaje = mensaje.replace(" 00:00:00", "");

    Mensaje.Error.agregarError(mensaje);
};

Validacion.formatError = function (mensaje, campo) {
    var label = $("label[for='" + campo.id + "']").html();

    if (label === null) {
        return;
        //label = $("label[for='" + campo.id.replace("A_o", "Año") + "']").html();
    }

    label = label.replace(" (*): ", "");
    Validacion.addError(mensaje.replace("{0}", label));
};

Validacion.required = function (campo) {
    var valor = campo.value;
    if (!valor) {
        Validacion.formatError(Validacion.Mensaje.required, campo);
    }
};

Validacion.range = function (campo, minimo, maximo) {
    var valor = campo.value;
    if (valor < minimo || valor > maximo) {
        Validacion.formatError(Validacion.Mensaje.range, campo);
    }
};

Validacion.Fecha = function (fecha) {



    function __comprobarSiBisisesto(anio) {
        if ((anio % 100 != 0) && ((anio % 4 == 0) || (anio % 400 == 0))) {
            return true;
        }
        else {
            return false;
        }
    }
    if (typeof fecha == "string") { fecha = { value: fecha} };

    fecha.value = fecha.value.substring(0, 10);

    if (fecha != undefined && fecha.value != "") {

        var expR = /\d{2}\/\d{2}\/\d{4}$/;
        if (!expR.test(fecha.value)) {

            return false;
        }
        var dia = parseInt(fecha.value.substring(0, 2), 10);
        var mes = parseInt(fecha.value.substring(3, 5), 10);
        var anio = parseInt(fecha.value.substring(6), 10);

        switch (mes) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                numDias = 31;
                break;
            case 4: case 6: case 9: case 11:
                numDias = 30;
                break;
            case 2:
                if (__comprobarSiBisisesto(anio)) { numDias = 29 } else { numDias = 28 };
                break;
            default:

                return false;
        }

        if (dia > numDias || dia == 0) {

            return false;
        }
        return true;
    }
}

