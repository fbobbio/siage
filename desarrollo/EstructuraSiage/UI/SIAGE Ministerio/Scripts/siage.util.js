// Function que permite resolver el ID de un control HTML cuando se utilizan vistas parciales dentro de otras y los nombres se modifican
//$.resolverId("Modelo", "Propiedad").click(...);
jQuery.resolverId = function (modelo, propiedad) {
    var selector = $(":input").regex("id", new RegExp("^.*" + modelo + "_" + propiedad + "$"));
    if(selector.length == 0)
        selector = $("#" + propiedad);

    return selector;
};

// Retorna true si el objeto pasado por parametros es un objeto (no array ni atributo simple)
jQuery.isObject = function(o) {
    return (o && (typeof o === 'object' || $.isFunction(o))) || false;
};

jQuery.endsWith = function (texto, patron) {
    var d = texto.length - patron.length;
    return d >= 0 && texto.lastIndexOf(patron) === d;
};


// Convierte un objeto en formato JSON a un array con el formato correcto para ser interpretado por el ModelBinder de MVC
// Es importante que las propiedades del objeto JSON tengan el mismo nombre que en el Model
jQuery.formatoModelBinder = function (json, contenedor, prefijo) {
    prefijo = prefijo || "";

    if ($.isArray(json)) {
        $.each(json, function (keyProp, valueProp) {
            if (json.hasOwnProperty(keyProp)) {
                prefijo = $.endsWith(prefijo, ".") ? prefijo.substr(0, prefijo.length - 1) : prefijo;
                $.formatoModelBinder(valueProp, contenedor, prefijo + "[" + keyProp + "].");
            }
        });
    }
    else if ($.isObject(json)) {
        $.each(json, function (keyProp, valueProp) {
            if (json.hasOwnProperty(keyProp)) {
                $.formatoModelBinder(valueProp, contenedor, prefijo + keyProp + ".");
            }
        });
    }
    else {
	
	
    var name= prefijo.substr(0, prefijo.length - 1);
	var esRepetida=false;
                            //aca se valida q no se repita
                        for(var i=0;i<contenedor.length;i++){						
                            if(contenedor[i].name==name){        
                            contenedor[i].value=json;
							esRepetida=true;
                            break;
                            }//emd if
                        }//end for
						if(!esRepetida){
						contenedor.push({ name:name, value: json });
						}
    }
};


jQuery.formatoFormulario = function (json, prefijo) {
    if (!json) {
        return;
    }

    prefijo = prefijo || "";

    if ($.isArray(json)) {
        $.each(json, function (keyProp, valueProp) {
            if (json.hasOwnProperty(keyProp)) {
                prefijo = $.endsWith(prefijo, "_") ? prefijo.substr(0, prefijo.length - 1) : prefijo;
                $.formatoFormulario(valueProp, prefijo + "[" + keyProp + "]_");
            }
        });
    }
    else if ($.isObject(json)) {
        $.each(json, function (keyProp, valueProp) {
            if (json.hasOwnProperty(keyProp)) {
                $.formatoFormulario(valueProp, prefijo + keyProp + "_");
            }
        });
    }
    else {
        prefijo = "#" + prefijo.substr(0, prefijo.length - 1);
        $(prefijo).val(json);
    }
};

jQuery.fn.formatoJson = function () {
    // creo un nuevo objeto que contendra los datos del formulario
    var entidad = {};

    //recorro los campos del formulario
    $(this.selector + " :input[id!=''][type!='button']").each(function (index, input) {
        // si la propiedad no tiene valor no continuo con el procedimiento
        if (input.value) {
			
            // evaluo si es una propiedad de tipo simple (no tiene puntos en el name)
            if (input.name.indexOf(".") >= 0) {
                try {
                    $.valuarPropiedad(entidad, input.name, input.value);
                }
                catch (err) { }                
            }
            else {
                if ($(this).is(":disabled")) {
                    $(this).attr("disabled", false);
                    entidad[input.name] = input.value;
                    $(this).attr("disabled", true);
                }
                else {
                    entidad[input.name] = input.value;
                }
            }
        }
    });

    return entidad;
};

jQuery.valuarPropiedad = function (entidad, propiedades, valor) {
    var propiedades_arr = propiedades.split(".");

    if (propiedades_arr.length === 1) {

        if ($("#" + propiedades).is(":disabled")) {
            $("#" + propiedades).attr("disabled", false);
            entidad[propiedades_arr[0]] = valor;
            $("#" + propiedades).attr("disabled", true);
        }
        else {
            entidad[propiedades_arr[0]] = valor;
        }
    }
    else {
        if (!entidad[propiedades_arr[0]]) {
            entidad[propiedades_arr[0]] = {};
        }
        var propNuevas = propiedades.substring(propiedades.indexOf(".") + 1, propiedades.length);
        $.valuarPropiedad(entidad[propiedades_arr[0]], propNuevas, valor);
    }
};

jQuery.fn.cargarCombo = function (items, value, text, seleccione, deshabilitarCombo) {
    if (!seleccione) {
        seleccione = "SELECCIONE";
    }

    var option = "<option value='{0}'>{1}</option>";

    this.html("");
    this.append(option.replace("{0}", "").replace("{1}", seleccione));

    for (var i = 0; i < items.length; i++) {
        this.append(option.replace("{0}", items[i][value]).replace("{1}", items[i][text]));
    }
    if (!deshabilitarCombo) {
        this.removeAttr("disabled");
    }
};

jQuery.fn.limpiarCombo = function (seleccione) {
    if (!seleccione) {
        seleccione = "SELECCIONE";
    }

    this.html("");
    this.append("<option value=''>" + seleccione + "</option>");
    this.attr("disabled", "disabled");
};

jQuery.formatoFechaJson = function(datetime) {    
    var dateString = datetime.replace(/[^+0-9]*/img, ""); 
    var dateObj = new Date(parseInt(dateString, 10));
    var curr_date = ""+dateObj.getDate();
    var curr_month = dateObj.getMonth() + 1;
    var curr_year = ""+dateObj.getFullYear();
    curr_month = "" + curr_month;
    if (curr_date.trim().length == 1) curr_date = "0" + curr_date;
    if (curr_month.trim().length == 1) curr_month = "0" + curr_month;
    return curr_date + "/" + curr_month + "/" + curr_year;    
};

jQuery.fn.agregarPrefijo = function (prefijo) {
    var div = this.selector;
    $(div + " :input, " + div + " div, " + div + " table, " + div + " button, " + div + " fieldset").each(function (value) {
        var id = $(this).attr("id");
        var name = $(this).attr("name");
        
        if (id && id.indexOf(prefijo) !== 0) {
            id = prefijo + '_' + id;

            $(this).attr("id", id);

            if (name) {
                name = prefijo + '.' + name;
                name = name.replace(/_/g, ".");

                $(this).attr("name", name);    
                $(this).parent("p").children("label").attr("for", id);
            }
        }
    });
}

jQuery.fn.agregarPrefijoSoloInput = function (prefijo) {
    var selector = this.selector + " :input";
    $(selector).each(function (value) {
        if (this.type === "button" || this.type === "submit") {
            return;
        }

        var id = $(this).attr("id");
        var name = $(this).attr("name");
        if (id) {
			id = prefijo + '_' + id;
			name = prefijo + '.' + name;
			name = name.replace(/_/g, ".");
			
            $(this).attr("id", id);
            $(this).attr("name", name);
            $(this).parent("p").children("label").attr("for", id);
        }
    });
}

jQuery.getUrl = function (url) {
    var urlReal = location.pathname.split("/");
    var urlParcial = url.split("/");

    var separador = (url.indexOf("?") !== -1) ? "&" : "?";
    url += separador + "sid=" + Math.random();

    if (urlReal.length > 2 && urlParcial[1] !== urlReal[1]) {
        return "/" + urlReal[1] + url;
    }
    return url;
};

jQuery.fn.getFiltros = function () {
    var filtros = [];
    $(this).each(function (ind, val) {
        if (val.value) {
            filtros.push(val.id + "=" + val.value);
        }
    });
    return filtros.join("&");
};

jQuery.fn.changePatch = function (handler) {
    if (handler) {
        $(this).bind($.browser.msie && $.browser.version != "9.0" ? 'propertychange' : 'change', handler);
    }
    else {
        $(this).trigger($.browser.msie && $.browser.version != "9.0" ? 'propertychange' : 'change');
    }
    return $(this);
};

jQuery.fn.editorOpcional = function (check, inversa) {
	var div = $(this);
	inversa = inversa || false;

	$(check).changePatch(function () {
        var checked = $(check).is(":checked")
        if ((checked && inversa === false) || (!checked && inversa === true)) {
	        $.recuperarDiv(div);			
		}
	    else {
	        $.guardarDiv(div);
		} 		
	});
};

jQuery.fn.comboOpcional = function (opcionDiv) {
    $(this).changePatch(function () {
        var seleccionado = $(this).children("option:selected").text();

        $.each(opcionDiv, function (keyProp, valueProp) {
            if (keyProp != seleccionado && valueProp != null) {
                $.guardarDivCombo(valueProp);
            }
        });

        $.recuperarDiv(opcionDiv[seleccionado]);
    });
};

jQuery.guardarDiv = function (div) {
    var json = $(div).formatoJson();
    $(div).data("editor", json);
    var requeridos = $(div.selector + " .val-Required").removeClass("val-Required");
    $(div).data("requeridos", requeridos);
    $(div.selector + " :input[type!='button']").val("");
    $(div).hide();
};

jQuery.guardarDivCombo = function (div) {
    var json = $(div).formatoJson();
    $(div).data("editor", json);
    var requeridos = $(div + " .val-Required").removeClass("val-Required");
    $(div).data("requeridos", requeridos);
    $(div + " :input[type!='button']").val("");
    $(div).hide();
};

jQuery.recuperarDiv = function (div) {
    var json = $(div).data("editor");
    $.formatoFormulario(json, "");
    var requeridos = $(div).data("requeridos");
    $(requeridos).addClass("val-Required");
    $(div).show();
};

// Comportamiento similar al original serializeArray() con la capacidad de recuperar sólo los campos visibles, incluso
// aquellos que están deshabilitados
jQuery.fn.serializeArrayPatch = function () {
    var campos = [];
    $(this.selector + " :input[type!='button']:visible, " + this.selector + " div:visible > input[type='hidden']").each(function (ind, val) {
        var valor = null;
        var name = $(val).attr("name");
        var type = $(val).attr("type");

        if (type == "checkbox") {
            valor = $(val).is(":checked");
        }
        else {
            valor = $(val).val();
        }
        
        if (valor && name) {
            campos.push({ name: name, value: valor });
        }
    });
    return campos;
};

jQuery.fn.serializeArrayPatchDiv = function () {
    var campos = [];
    $(this.selector + " :input[type!='button'][type!='hidden']:visible").each(function (ind, val) {
        var valor = null;
        var name = $(val).attr("name");
        var type = $(val).attr("type");

        if (type == "checkbox") {
            valor = $(val).is(":checked");
        }
        else {
            valor = $(val).val();
        }

        if (valor && name) {
            campos.push({ name: name, value: valor });
        }
    });
    $(this.selector + " div:visible").each(function (indice, value) {
        var div = $(value).attr("id");
        $("#" + div + " input[type='hidden']").each(function (indi, valueHidden) {
            var valor = null;
            var name = $(valueHidden).attr("name");
            var type = $(valueHidden).attr("type");
            valor = $(valueHidden).val();
            if (valor && name) {
                campos.push({ name: name, value: valor });
            }
        });
    });
    return campos;
};