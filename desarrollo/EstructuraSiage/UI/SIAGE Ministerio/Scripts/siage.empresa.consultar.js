/*************************************************************** VARIABLES ***********************************************************/

var ConsultarEmpresa = {};
ConsultarEmpresa.Grilla = {};
ConsultarEmpresa.GrillaSeleccion = {};
ConsultarEmpresa.BusquedaBasica = "ProcesarBusquedaBasico";
ConsultarEmpresa.BusquedaAvanzadaEmpresa = "ProcesarBusquedaAvanzadaEmpresa";
ConsultarEmpresa.BusquedaAvanzadaEscuela = "ProcesarBusquedaAvanzadaEscuela";
ConsultarEmpresa.filtrosDeBusqueda = {};
ConsultarEmpresa.divVista = "#divVista";
ConsultarEmpresa.valorParametro=null;
/************************************************************** INICIALIZACION *******************************************************/

ConsultarEmpresa.init = function (vista, div, prefijo, seleccionMultiple, titulo, seConsultaDesdeRegistrarEmpresa) {
    div = div || "";
    prefijo = prefijo || "";
    //traigo el valor del parametro jeraquia organigrama
    if (ConsultarEmpresa.valorParametro == null) {
        $.get($.getUrl("/GestionEmpresa/ParametroJerarquiaSigueOrganigrama"), {}, function (valorParametro) {
            ConsultarEmpresa.valorParametro = valorParametro;
        });
    }
    
    // Modificaciones de los id de los campos del editor
    $(div).agregarPrefijo(prefijo);



    // Creacion de un objeto instancia
    var instancia = ConsultarEmpresa.inicializarVariables();
    instancia.vista = vista;
    instancia.Filtros = {};
    instancia.prefijo = "#" + prefijo + "_";

    instancia.seConsultaDesdeRegistrarEmpresa = seConsultaDesdeRegistrarEmpresa;
    if (titulo) {
        $(instancia.prefijo + "divDatosGeneralesEmpresa legend").html("Datos generales de " + titulo);
        $(instancia.prefijo + "btnVolver").val("Buscar " + titulo);
    }
    instancia.seleccionMultiple = seleccionMultiple || false;

    // Inicializacion de los controles del editor
    ConsultarEmpresa.Grilla.init(instancia);
    ConsultarEmpresa.GrillaSeleccion.init(instancia);

    ConsultarEmpresa.inicializarDivs(instancia);
    ConsultarEmpresa.inicializarBotones(instancia);

    // Comportamiento de los combos y checkbox cuando cambian su valor
    ConsultarEmpresa.inicializarComportamientoFiltros(instancia);
    // Paso por url, el valor de los parametros ingresados en la busqueda al metodo ProcesarBusquedaBasico
    ConsultarEmpresa.inicializarFiltroBasico(instancia);
    // Paso por url, el valor de los parametros ingresados en la busqueda al metodo ProcesarBusquedaAvanzadaEmpresa
    ConsultarEmpresa.inicializarFiltroAvanzadoEmpresa(instancia);
    // Paso por url, el valor de los parametros ingresados en la busqueda al metodo ProcesarBusquedaAvanzadaEscuela
    ConsultarEmpresa.inicializarFiltroAvanzadoEscuela(instancia);
    // Funcionalidad a los combos en cascada
    ConsultarEmpresa.inicializarCombos(instancia);
    $(instancia.prefijo + 'fltCodigoEmpresa').attr('maxLength', 9);
    return instancia;
};


ConsultarEmpresa.inicializarVariables = function (instancia) {
    instancia = {};

    // Nombre del controlador
    instancia.controlador = "GestionEmpresa";

    // Nombre de las acciones existentes en el controlador
    instancia.acciones = {
        Listar: "ProcesarBusquedaBasico",
        Seleccionar: "ProcesarSeleccion",
        Ver: "Ver",
        Eliminar: "Eliminar",
        Editar: "Editar",
        Registrar: "Registrar",
        Reactivar: "Reactivar",
		Historial: "MostrarVistaHistorial"
    };

    return instancia;
};

ConsultarEmpresa.inicializarDivs = function (instancia) {
    if ($(instancia.prefijo + "estadoVista").val() === "Consultar" ) {
        $("#divConsulta").show();
        $(ConsultarEmpresa.divVista).hide();
    }
    else {
        $("#divConsulta").hide();
        $(ConsultarEmpresa.divVista).show();
    }
};

ConsultarEmpresa.obtenerFiltrosBusqueda = function (instancia) {
    var model = $("#consultaIndex_divFiltrosDeBusqueda").formatoJson().consultaIndex;
    ConsultarEmpresa.filtrosDeBusqueda = model;
};

ConsultarEmpresa.cargarFiltrosBusqueda = function (instancia) {
    for (var i in ConsultarEmpresa.filtrosDeBusqueda) {
        $(instancia.prefijo + i).val('"' + ConsultarEmpresa.filtrosDeBusqueda[i] + '"');
        
    }
};

ConsultarEmpresa.inicializarBotones = function (instancia) {
    // Cuando presiono el boton Consultar se actualiza la grilla
    $(instancia.prefijo + "btnConsultarAvanzado").click(function () {        
        GrillaUtil.actualizar(instancia.grilla);
    });
    //Antes de ir al servidor, verifico q se haya utilizado al menos un filtro
    $(instancia.prefijo + "btnConsultarBasico").click(function () {
        var filtros = $(instancia.prefijo + "divFiltroBasico :input[type!=button][value!='']");
        if (filtros.length !== 0) {
            GrillaUtil.actualizar(instancia.grilla);
            Mensaje.ocultar();
        } else {
            Mensaje.Advertencia.botones = false;
            Mensaje.Advertencia.texto = "Se debe filtrar por al menos un criterio";
            Mensaje.Advertencia.mostrar();
        }
    });

    // Funcionalidad de los botones Limpiar basico y Avanzado
    $(instancia.prefijo + "btnLimpiarBasico").click(function () {
        instancia.acciones.Listar = ConsultarEmpresa.BusquedaBasica;
        ConsultarEmpresa.limpiar(instancia);
        ConsultarEmpresa.Grilla.limpiar(instancia);
    });

    $(instancia.prefijo + "btnLimpiarAvanzado").click(function () {
        if (ConsultarEmpresa.esEscuela(instancia)) {
            instancia.acciones.Listar = ConsultarEmpresa.BusquedaAvanzadaEscuela;
        }
        else {
            instancia.acciones.Listar = ConsultarEmpresa.BusquedaAvanzadaEmpresa;
        }
        ConsultarEmpresa.limpiar(instancia);
        ConsultarEmpresa.Grilla.limpiar(instancia);
    });

    // En la busqueda avanzada no muestra el boton Consultar a menos que seleccione un tipo de empresa
    $(instancia.prefijo + "btnBusquedaAvanzada").click(function () {
        $(instancia.prefijo + "divFiltroAvanzado").show();
        $(instancia.prefijo + "divFiltroBasico").hide();
        $(instancia.prefijo + "btnConsultarAvanzado").hide();
    });

    $(instancia.prefijo + "btnBusquedaBasica").click(function () {
        $(instancia.prefijo + "divFiltroAvanzado").hide();
        $(instancia.prefijo + "divFiltroBasico").show();
        instancia.acciones.Listar = ConsultarEmpresa.BusquedaBasica;
        ConsultarEmpresa.Grilla.limpiarUrl(instancia);
    });

    $(instancia.prefijo + "btnVolver").click(function () {
        $(instancia.prefijo + "divFiltrosDeBusqueda").show();
        $(instancia.prefijo + "divDatosGeneralesEmpresa").hide();
    });

};

ConsultarEmpresa.modoConsulta = function (instancia) {
    instancia.acciones.Listar = ConsultarEmpresa.BusquedaBasica;
    ConsultarEmpresa.limpiar(instancia);
    ConsultarEmpresa.Grilla.limpiar(instancia);
    $(instancia.prefijo + "divFiltrosDeBusqueda").show();
    $(instancia.prefijo + "divDatosGeneralesEmpresa").hide();
}

ConsultarEmpresa.inicializarCombos = function (instancia) {

    //elimino los  Tipo inspección intermedia  del combo que son general y zonal
    $(instancia.prefijo + "option[value='-10']," + instancia.prefijo + "option[value='-20']").remove();

    $(instancia.prefijo + "EsPrivada").hide();
    $(".val-DateTime").datepicker({ currentText: 'Now', dateFormat: 'dd/mm/yy' });
    $(instancia.prefijo + "fltCodigoInspeccion").numeric();
    $(instancia.prefijo + "fltCodigoInspeccion").attr("maxlength", "6");
    $(instancia.prefijo + "fltCodigoInspeccion").blur(function () {
        if (isNaN($(instancia.prefijo + "fltCodigoInspeccion").val())) {
            Mensaje.Advertencia.texto = "El campo código inspeccion permite solo números";
            Mensaje.Advertencia.botones = false;
            Mensaje.Advertencia.mostrar();
            $(instancia.prefijo + "fltCodigoInspeccion").val("");
            return;
        }
    });
    $(instancia.prefijo + "fltCodigoInspeccion").changePatch(function () {
        $(instancia.prefijo + "fltCodigoInspeccion")
    })

    $(instancia.prefijo + "fltNivelEducativo").changePatch(function () {
        if ($(instancia.prefijo + "fltNivelEducativo option:selected").text() == "PRIMARIO") {
            $(instancia.prefijo + "divFiltroCodigoInspeccion").show();
        } else {
            $(instancia.prefijo + "divFiltroCodigoInspeccion").hide();
        }
    })

    $(instancia.prefijo + "fltLocalidadBasico").CascadingDropDown(instancia.prefijo + "fltDepartamentoProvincialBasico", $.getUrl('/GestionEmpresa/CargarLocalidades'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idDepartamento: $(instancia.prefijo + "fltDepartamentoProvincialBasico").val() };
        }
    });
    $(instancia.prefijo + "fltLocalidadEmpresa").CascadingDropDown(instancia.prefijo + "fltDepartamentoProvincialEmpresa", $.getUrl('/GestionEmpresa/CargarLocalidades'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idDepartamento: $(instancia.prefijo + "fltDepartamentoProvincialEmpresa").val() };
        }
    });
    $(instancia.prefijo + "fltLocalidadEscuela").CascadingDropDown(instancia.prefijo + "fltDepartamentoProvincialEscuela", $.getUrl('/GestionEmpresa/CargarLocalidades'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idDepartamento: $(instancia.prefijo + "fltDepartamentoProvincialEscuela").val() };
        }
    });
};

ConsultarEmpresa.inicializarFiltroBasico = function (instancia) {
    $(instancia.prefijo + "divFiltroBasico :input").changePatch(function () {
        instancia.acciones.Listar = ConsultarEmpresa.BusquedaBasica;

        var url = $(instancia.prefijo + "divFiltroBasico :input[type!='button']").getFiltros() + "&vista=" + instancia.vista;

        url += "&seConsultaDesdeRegistrarEmpresa=" + (instancia.seConsultaDesdeRegistrarEmpresa || false);

        //modificar empresa obtengo el id de la empresa al que le estoy buscando algo
        var idEmpresa = $("legend:contains('Registrar Empresa') + input[type='hidden']").val();
        
        if (instancia.Filtros.idEmpresa) {
            idEmpresa = instancia.Filtros.idEmpresa;
        }
        if (idEmpresa && idEmpresa != "0") {
            url += "&idEmpresaDependientePadre=" + idEmpresa;
        };
        url = url.replace(new RegExp(instancia.prefijo.replace("#", ""), "g"), "");


        ConsultarEmpresa.Grilla.parametrosUrl(instancia, url);
    });
};

ConsultarEmpresa.setFiltroEmpresaId = function (instancia, idEmpresa) {
    instancia.Filtros.idEmpresa = idEmpresa;
}

ConsultarEmpresa.inicializarFiltroAvanzadoEmpresa = function (instancia) {

    $(instancia.prefijo + "divFiltroEmpresa :input").changePatch(function () {
        instancia.acciones.Listar = ConsultarEmpresa.BusquedaAvanzadaEmpresa;

        var url = $(instancia.prefijo + "divFiltroEmpresa :input[type!='button']").getFiltros()
                + "&fltTipoEmpresa=" + $(instancia.prefijo + "fltTipoEmpresa").val()
                + "&vista=" + instancia.vista + "&seConsultaDesdeRegistrarEmpresa=" + (instancia.seConsultaDesdeRegistrarEmpresa || false);

        url = url.replace(new RegExp(instancia.prefijo.replace("#", ""), "g"), "");

        ConsultarEmpresa.Grilla.parametrosUrl(instancia, url);
    });

    
};
	
ConsultarEmpresa.inicializarFiltroAvanzadoEscuela = function (instancia){
	$(instancia.prefijo + "divFiltroEscuela :input").changePatch(function () {
	    instancia.acciones.Listar = ConsultarEmpresa.BusquedaAvanzadaEscuela;

	    var url = $(instancia.prefijo + "divFiltroEscuela :input[type!='button']").getFiltros()
                + "&fltTipoEmpresa=" + $(instancia.prefijo + "fltTipoEmpresa").val()
                + "&vista=" + instancia.vista;
	    url += "&seConsultaDesdeRegistrarEmpresa=" + (instancia.seConsultaDesdeRegistrarEmpresa || false);
	    url = url.replace(new RegExp(instancia.prefijo.replace("#", ""), "g"), "");

        ConsultarEmpresa.Grilla.parametrosUrl(instancia, url);
    });
};

ConsultarEmpresa.inicializarComportamientoFiltros = function (instancia) {
    /*
    Muestra y oculta divs de acuerdo a si el tipo empresa es escuela o no.
    Si la empresa es una direccion de nivel, me muestra los combos de nivel educativo y 
    direccion de nivel. Si es otra empresa, los oculta, y le seteo el valor del combo a -1.
    */

    $(instancia.prefijo + "fltTipoEmpresa").changePatch(function () {
        if (ConsultarEmpresa.esEscuela(instancia)) {
            $(instancia.prefijo + "divFiltroEmpresa").hide();
            $(instancia.prefijo + "divFiltroEscuela").show();
            instancia.acciones.Listar = ConsultarEmpresa.BusquedaAvanzadaEscuela;
            if ($(instancia.prefijo + "fltTipoEmpresa").val() == "ESCUELA_MADRE") {
                $(instancia.prefijo + "fltNumeroEscuela").prev().hide();
                $(instancia.prefijo + "fltNumeroEscuela").hide();
                $(instancia.prefijo + "fltNumeroEscuela").val("");
            }
            else {
                $(instancia.prefijo + "fltNumeroEscuela").prev().show();
                $(instancia.prefijo + "fltNumeroEscuela").show();
            }
        }
        else {
            $(instancia.prefijo + "divFiltroEscuela").hide();
            $(instancia.prefijo + "divFiltroEmpresa").show();
            if (ConsultarEmpresa.esDireccionNivel(instancia)) {
                $(instancia.prefijo + "divTipoEducacionNivelEducativo").show();
            }
            else {
                $(instancia.prefijo + "fltTipoEducacionEmpresa").val(-1);
                $(instancia.prefijo + "fltNivelEducativoEmpresa").val(-1);
                $(instancia.prefijo + "divTipoEducacionNivelEducativo").hide();
            }
            instancia.acciones.Listar = ConsultarEmpresa.BusquedaAvanzadaEmpresa;
        }


        if ($(instancia.prefijo + "fltTipoEmpresa").val() === "INSPECCION") {
            
            if (ConsultarEmpresa.valorParametro=="True") {
                $(instancia.prefijo + "fltTipoInspeccion").show();
                $(instancia.prefijo + "fltTipoInspeccionIntermedia").hide();
            } else {
                $(instancia.prefijo + "fltTipoInspeccion").hide();
                $(instancia.prefijo + "fltTipoInspeccionIntermedia").show();
            }

            $(instancia.prefijo + "divFiltroTipoInspeccion").show();

        } else {
            $(instancia.prefijo + "divFiltroTipoInspeccion").hide();
        }

        var parametros = "fltTipoEmpresa=" + $(instancia.prefijo + "fltTipoEmpresa").val() + "&vista=" + instancia.vista;
            parametros += "&seConsultaDesdeRegistrarEmpresa=" + (instancia.seConsultaDesdeRegistrarEmpresa || false);
        ConsultarEmpresa.Grilla.parametrosUrl(instancia, parametros);

        $(instancia.prefijo + "btnConsultarAvanzado").show();
    });

    // Pregunto si la escuela es privada o no, para poder filtrar por la obra social
    $(instancia.prefijo + "Privada").changePatch(function () {
        if ($(instancia.prefijo + "Privada").is(":checked")) {
            $(instancia.prefijo + "EsPrivada").show();
        }
        else {
            $(instancia.prefijo + "fltObraSocial").val(-1);
            $(instancia.prefijo + "EsPrivada").hide();
        }
    });
};

// Pregunto si la empresa seleccionada en el combo de tipo empresa es una escuela (madre o anexo)
ConsultarEmpresa.esEscuela = function (instancia) {
    var escuelaMadre = "ESCUELA_MADRE";
    var escuelaAnexo = "ESCUELA_ANEXO";
    var comboTipoEmpresa = $(instancia.prefijo + "fltTipoEmpresa").val();

    return (comboTipoEmpresa === escuelaMadre || comboTipoEmpresa === escuelaAnexo);
};

// Pregunto si la empresa seleccionada en el combo de tipo empresa es una empresa de tipo dirección de nivel
ConsultarEmpresa.esDireccionNivel = function (instancia) {
    var empresaDireccionNivel = "DIRECCION_DE_NIVEL";
    var comboTipoEmpresa = $(instancia.prefijo + "fltTipoEmpresa").val();
    return (comboTipoEmpresa === empresaDireccionNivel);
};

// Limpia todo el formulario, oculta los divs y el boton consultar avanzado.
ConsultarEmpresa.limpiar = function (instancia) {
    $(instancia.prefijo + "divFiltrosDeBusqueda fieldset :input").each(function () {
        var type = this.type;
        var tag = this.tagName.toLowerCase();

        if (type === "text" || type === "password" || type === "hidden" || tag === "textarea") {
            this.value = "";
        }
        else {
            if (type === "checkbox" || type === "radio") {
                this.checked = false;
            }
            else {
                if (tag === "select") {
                    this.selectedIndex = 0;
                }
            }
        }
    });
    $(instancia.prefijo + "divFiltroEscuela").hide();
    $(instancia.prefijo + "divFiltroEmpresa").hide();
    $(instancia.prefijo + "btnConsultarAvanzado").hide();
    $(instancia.prefijo + "EsPrivada").hide();
};

// Selecciono una empresa, y muestro el valor de los datos generales de la empresa.
ConsultarEmpresa.seleccionarEmpresa = function (instancia) {
    $(ConsultarEmpresa.divVista).show();

    if (instancia.seleccionMultiple) {
        ConsultarEmpresa.seleccionarMultipleEmpresa(instancia);
    }
    else {
        ConsultarEmpresa.seleccionarUnicaEmpresa(instancia);
    }
};

ConsultarEmpresa.seleccionarMultipleEmpresa = function (instancia) {
    $(instancia.prefijo + "divDatosSeleccionMultiple").show();
    $(instancia.prefijo + "divDatosGeneralesEmpresa").hide();

    var escuelasSeleccionadas = $(instancia.prefijo + "listSeleccionadas").getDataIDs();
    instancia.seleccion = instancia.seleccion.filter(function (value) {
        return $.inArray(value, escuelasSeleccionadas) === -1;
    });

    for (var i = 0; i < instancia.seleccion.length; i++) {
        var datos = instancia.grilla.getRowData(instancia.seleccion[i]);
        instancia.grillaSeleccion.addRowData(instancia.seleccion[i], datos, "last");
        instancia.grilla.resetSelection();
    }
};

//Método que efectúa la selección de una empresa única
ConsultarEmpresa.seleccionarUnicaEmpresa = function (instancia) {
    $(instancia.prefijo + "divFiltrosDeBusqueda").hide();

    var ret = instancia.grilla.getRowData(instancia.seleccion);

    if (ret.Id) {
        ConsultarEmpresa.CargarEmpresa(instancia, ret);
    }
    else {
        $.get($.getUrl("/GestionEmpresa/GetEmpresaConsultaById"), { idEmpresa: instancia.seleccion },
        function (data) {
            if (data) {
                ConsultarEmpresa.CargarEmpresa(instancia,data);
            }
        }, "json");
    }
};

//Método que carga la empresa en la vista
ConsultarEmpresa.CargarEmpresa = function (instancia, empresa) {
    var url = $.getUrl("/" + instancia.controlador + "/" + instancia.acciones.Seleccionar);
    var datos = [];
    var model = {};
    model.id = instancia.seleccion;
    model.vistaActiva = instancia.vista;
    model.tipoEmpresa = empresa.TipoEmpresa;

    $.formatoModelBinder(model, datos, "");
    $.get(url, datos, function (data) { if (data) { $(ConsultarEmpresa.divVista).html(data); } }, "html");

    $(instancia.prefijo + "divDatosSeleccionMultiple").hide();
    $(instancia.prefijo + "divDatosGeneralesEmpresa").show();

    $(instancia.prefijo + "Id").val(empresa.Id);
    $(instancia.prefijo + "VerCodigoEmpresa").val(empresa.CodigoEmpresa);
    $(instancia.prefijo + "VerNombreEmpresa").val(empresa.NombreEmpresa);
    $(instancia.prefijo + "VerTipoEducacion").val(empresa.TipoEducacion);
    $(instancia.prefijo + "VerNivelEducativo").val(empresa.NivelEducativo);
    $(instancia.prefijo + "VerTipoEmpresa").val(empresa.TipoEmpresa);
    $(instancia.prefijo + "VerEstadoEmpresa").val(empresa.EstadoEmpresa);
    if (empresa.CUE) {
        if (empresa.CUE) {
            var cue = empresa.CUE.split("-")[0];
            var cueAnexo = empresa.CUE.split("-")[1];
            $(instancia.prefijo + "VerCueEmpresa").val(cue);
            if (cue != 0 && cueAnexo) {
                $(instancia.prefijo + "VerCueAnexoEmpresa").val(cueAnexo);
                //si el cue anexo es 0 y no nulo, significa q el mismo es 00, de lo contrario seria nulo.
                //no existe el cue anexo 0 (un solo 0)
                if (cueAnexo === "0") {
                    $(instancia.prefijo + "VerCueAnexoEmpresa").val("00");
                }
            }
        }
    }
};

/************************************************************** GRILLA **************************************************************/

ConsultarEmpresa.Grilla.init = function (instancia) {
    var id = instancia.prefijo + "list";
    var url = $.getUrl("/" + instancia.controlador + "/" + instancia.acciones.Listar + "?vista=" + instancia.vista);
    var pager = instancia.prefijo + "pager";

    instancia.grilla = $(id).jqGrid({
        url: url,
        datatype: "local",
        mtype: "GET",
        colNames: ['Id', 'Fecha alta', 'Código empresa', 'CUE', 'Nombre empresa', 'Tipo empresa', 'Estado empresa', 'Nivel Educativo', 'Tipo Educacion'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'FechaAlta', index: 'FechaAlta', align: 'left' },
            { name: 'CodigoEmpresa', index: 'CodigoEmpresa', align: 'left' },
            { name: 'CUE', index: 'CUE', align: 'left' },
            { name: 'NombreEmpresa', index: 'NombreEmpersa', align: 'left' },
            { name: 'TipoEmpresa', index: 'TipoEmpresa', align: 'left' },
            { name: 'EstadoEmpresa', index: 'EstadoEmpresa', align: 'left' },
            { name: 'NivelEducativo', index: 'NivelEducativo', align: 'left', hidden: true },
            { name: 'TipoEducacion', index: 'TipoEducacion', align: 'left', hidden: true}],
        //{ name: 'Raiz', index: 'Raiz', align: 'left', hiden: true }],
        pager: pager,
        toppager: true,
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        sortname: "CUE",
        sortorder: "asc",
        viewrecords: true,
        autowidth: true,
        //width: document.body.offsetWidth - 650,
        height: "100%",
        loadtext: "Cargando, espere por favor",
        emptyrecords: "",
        multiselect: instancia.seleccionMultiple
    });

    var id_limpio = id.replace("#", "");

    instancia.grilla.id = id;
    instancia.grilla.id_limpio = id_limpio;
    instancia.grilla.pager = pager;

    // Creo los botones de la barra de herramientas
    ConsultarEmpresa.Grilla.crearBotones(instancia);

    // Agrego div para mostrar mensajes personalizado
    $("#gview_" + id_limpio).append("<div id='" + id_limpio + "_sinConsulta' class='ui-mensaje'>Aún no se ha realizado ninguna búsqueda</div>");
    $("#gview_" + id_limpio).append("<div id='" + id_limpio + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados con los datos de búsqueda ingresados</div>");

    // Limpio los datos de la consulta para el primer ingreso y oculto los botones que no sean necesarios
    instancia.grilla.setGridParam({ loadComplete: function (data) { GrillaUtil.mostrarBotones(instancia.grilla); } }).trigger("reloadGrid");
};

ConsultarEmpresa.Grilla.limpiar = function (instancia) {
    var url = $.getUrl("/" + instancia.controlador + "/" + instancia.acciones.Listar + "?vista=" + instancia.vista);
    GrillaUtil.limpiar(instancia.grilla, url);
};

ConsultarEmpresa.Grilla.limpiarUrl = function (instancia) {
	var url = $.getUrl("/" + instancia.controlador + "/" + instancia.acciones.Listar + "?vista=" + instancia.vista);
	GrillaUtil.setUrl(instancia.grilla, url);	
};

ConsultarEmpresa.Grilla.parametrosUrl = function (instancia, parametros) {
	var url = $.getUrl("/" + instancia.controlador + "/" + instancia.acciones.Listar + "?" + parametros);
	GrillaUtil.setUrl(instancia.grilla, url);	
};

ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada = [];
ConsultarEmpresa.Grilla.crearBotones = function (instancia) {
    var grilla = instancia.grilla.id;

    // Elimino la paginacion en la barra de arriba
    $(grilla + "_toppager_center", grilla + "_toppager").remove();
    $(".ui-paging-info", grilla + "_toppager").remove();

    // Agrego la tabla que contiene los botones
    $(grilla + "_toppager_left").append('<table cellspacing="0" cellpadding="0" border="0" style="float: left; table-layout: auto;" class="ui-pg-table navtable"><tbody><tr></tr></tbody></table>');

    switch (instancia.vista) {

        case "RegistrarEmpresaEditor":
        case "VisarActivacionEmpresaEditor":
        case "ActivacionCodigoEmpresaEditor":
        case "CerrarEmpresaEditor":
        case "EmitirResolucionEmpresaEditor":
        case "VisarCierreEmpresaEditor":
        case "ReactivarEmpresaEditor":
        case "ConsultarEstructuraEditor":
        case "SinVista":
        case "AsignacionPlan":
        case "Seccion":
        case "ConsultarSolicitudesDesactivacionEmpresasEditor":
        case "SolicitudDePuestoDeTrabajo":
        case "ConsultarEstructuraEditor": ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada = ["APOYO_ADMINISTRATIVO", "DIRECCION_DE_INFRAESTRUCTURA", "DIRECCION_DE_NIVEL", "DIRECCION_DE_RECURSOS_HUMANOS", "DIRECCION_DE_SISTEMAS", "DIRECCION_DE_TESORERIA", "ESCUELA_ANEXO", "ESCUELA_MADRE", "INSPECCION", "MINISTERIO", "SECRETARIA", "SUBSECRETARIA"]; break;
        case ("AsignacionPlan"):
        case ("SoloEscuelas"):
        case ("ModificarTipoEmpresaEditor"):
        case ("ModificarAsignacionEscuelaAInspeccionEditor"):
        case ("ConsultarEstructuraEditor"):
        case ("Seccion"):
            ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada = ["ESCUELA_MADRE", "ESCUELA_ANEXO"];
            break;

        case ("SinVista"):
            break;
        case ("BuscarInspeccionesZonal"):
        case ("BusquedaPorInspeccionUnica"):
        case ("CualquierInspeccionDeDireccionDeNivelDelUsuarioLogueado"):
        case ("BuscarInspecciones"):
        case ("CualquierInspeccionNoZonalPertenecienteADireccionDeNivelDelUsuarioLogueado"):
            ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada = ["INSPECCION"];
            break;

        case ("SoloMinisterio"):
            ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada = ["MINISTERIO"];
            break;

        case ("BusquedaPorSecretaria"):
            ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada = ["MINISTERIO", "DIRECCION_DE_INFRAESTRUCTURA", "DIRECCION_DE_RECURSOS_HUMANOS", "DIRECCION_DE_SISTEMAS", "DIRECCION_DE_TESORERIA"];
            break;

        case ("BusquedaPorSubSecretaria"):
            ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada = ["SECRETARIA", "DIRECCION_DE_NIVEL"];
            break;
        case ("BusquedaPorApoyoAdm"):
        case ("BusquedaPorInspeccion"):
            ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada = ["DIRECCION_DE_NIVEL", "INSPECCION"];
            break;

        case ("DireccionDeNivel"):
        case ("DireccionDeNivelDelUsuarioLogeado"):
        case ("BusquedaPorEscuelaMadre"):
            ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada = ["DIRECCION_DE_NIVEL"];
            break;
        case ("BusquedaPorEscuelaRaiz"):
        case ("BusquedaPorEscuelaAnexo"):
            ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada = ["ESCUELA_MADRE"];
            break;

        default:
            break;
    }
   
    //agrego botones en las consultas
    if (instancia.vista == "RegistrarEmpresaEditor") {
        instancia.grilla.botones = [ 'Ver', 'Editar', 'Agregar'];
    } else {
        instancia.grilla.botones = ['Seleccionar'];
    }

    //primero borro las opciones en la consulta
    $(instancia.prefijo + "fltTipoEmpresa").html("<option>SELECCIONE</option>");
    //agrego los tipos de empresas permitidas en la consulta avanzada
    for (var i = 0; i < ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada.length; i++) {
        $(instancia.prefijo + "fltTipoEmpresa").append("<option>" + ConsultarEmpresa.TipoEmpresasEnConsultaAvanzada[i] + "</option>");
    }


    var botones = instancia.grilla.botones;
    if (botones) {
        for (var i = 0; i < botones.length; i++) {
            switch (botones[i]) {
                case "Seleccionar":
                    GrillaUtil.crearBotonSeleccion(instancia.grilla, "Seleccionar", "pin-s",
                    function () {
                        instancia.seleccion = GrillaUtil.getSeleccionFilas(instancia.grilla, instancia.seleccionMultiple);
                        if (instancia.onSelect && instancia.seleccion) {
                            instancia.onSelect(instancia.seleccion);
                        }

                        if (instancia.seleccion) {
                            ConsultarEmpresa.seleccionarEmpresa(instancia);

                        } else {
                            AbmcUtil.mensajeSeleccion();
                        }
                    });
                    break;

                case "Ver":
                    GrillaUtil.crearBotonSeleccion(instancia.grilla, "Ver", "search",
                    function () {
                        instancia.seleccion = GrillaUtil.getSeleccionFilas(instancia.grilla, instancia.seleccionMultiple);

                        $(instancia.prefijo + "estadoVista").val("Ver");
                        if (instancia.seleccion) {
                            var url = $.getUrl("/" + instancia.controlador + "/" + instancia.acciones.Ver + "/" + instancia.seleccion);

                            $.get(url, {},
                                function (data) {
                                    $(ConsultarEmpresa.divVista).show();
                                    $(ConsultarEmpresa.divVista).html(data);
                                    $("#divConsulta").hide();
                                },
                                "html");
                        } else {
                            AbmcUtil.mensajeSeleccion();
                        }
                        //instancia.grilla.jqGrid().clearGridData();
                        //GrillaUtil.limpiar(instancia.grilla, $.getUrl("/" + instancia.controlador + "/" + instancia.acciones.Ver + "/" + instancia.seleccion));

                    });
                    break;

                case "Agregar":
                    GrillaUtil.crearBotonSeleccion(instancia.grilla, "Agregar", "plus",
                    function () {
                        Abmc.cargarModelo("Registrar", null);
                        ConsultarEmpresa.inicializarDivs(instancia);
                    });
                    Mensaje.ocultar();
                    break;

                case "Editar":
                    GrillaUtil.crearBotonSeleccion(instancia.grilla, "Editar", "pencil",
                    function () {
                        instancia.seleccion = GrillaUtil.getSeleccionFilas(instancia.grilla, instancia.seleccionMultiple);
                        if (instancia.seleccion) {
                            $(instancia.prefijo + "estadoVista").val("Editar");
                            var valCelda = $(instancia.grilla).getCell(instancia.seleccion, "EstadoEmpresa");

                            if (instancia.seleccion && valCelda == "AUTORIZADA") {
                                Abmc.cargarModelo("Editar", instancia.seleccion);
                                ConsultarEmpresa.inicializarDivs(instancia);
                            }
                            else {
                                Mensaje.Error.texto = "No se puede editar la empresa seleccionada porque no se encuentra en estado AUTORIZADO";
                                Mensaje.Error.mostrar();
                            }
                        } else { AbmcUtil.mensajeSeleccion(); }
                    });
                    break;

                case "Historial":
                    GrillaUtil.crearBotonSeleccion(instancia.grilla, "Historial", "search",
                    function () {
                        instancia.seleccion = GrillaUtil.getSeleccionFilas(instancia.grilla, instancia.seleccionMultiple);
                        if (instancia.seleccion) {
                            $.ajax({
                                url: "/GestionEmpresa/" + instancia.acciones.Historial,
                                type: "GET",
                                async: false,
                                data: {},
                                success: function (data) { $(ConsultarEmpresa.divVista).html(data); HistorialEmpresa.init(); }
                            });
                            ConsultarEmpresa.inicializarDivs(instancia);
                        }
                        else {
                            AbmcUtil.mensajeSeleccion();
                        }
                    });
                    break;
            }
        }
    }
};

/************************************************************* GRILLA SELECCIONADAS ********************************************************************/

ConsultarEmpresa.GrillaSeleccion.init = function (instancia) {
    if (!instancia.seleccionMultiple) { return; }

    var id = instancia.prefijo + "listSeleccionadas";

    instancia.grillaSeleccion = $(id).jqGrid({
        datatype: "local",
        colNames: ['Id', 'Fecha alta', 'Código empresa', 'CUE', 'Nombre empresa', 'Tipo empresa', 'Estado empresa', 'Nivel Educativo', 'Tipo Educacion'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'FechaAlta', index: 'FechaAlta', align: 'left' },
            { name: 'CodigoEmpresa', index: 'CodigoEmpresa', align: 'left' },
            { name: 'CUE', index: 'CUE', align: 'left' },
            { name: 'NombreEmpresa', index: 'NombreEmpersa', align: 'left' },
            { name: 'TipoEmpresa', index: 'TipoEmpresa', align: 'left' },
            { name: 'EstadoEmpresa', index: 'EstadoEmpresa', align: 'left' },
            { name: 'NivelEducativo', index: 'NivelEducativo', align: 'left', hidden: true },
            { name: 'TipoEducacion', index: 'TipoEducacion', align: 'left', hidden: true }],
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        sortname: "CUE",
        sortorder: "asc",
        viewrecords: true,
        width: $("#divDatosSeleccionMultiple").width() - 30,
        autowidth: true,
        height: "100%",
        loadtext: "Cargando, espere por favor",
        emptyrecords: "",
        caption: "Empresas seleccionadas"
    });

    instancia.grillaSeleccion.id = id;
    $(instancia.prefijo + "listSeleccionadas").setGridWidth($("#divConsulta").width(), true);
};
