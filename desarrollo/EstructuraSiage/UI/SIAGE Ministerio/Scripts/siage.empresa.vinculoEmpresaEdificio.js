var VinculoEmpresaEdificio = {};
VinculoEmpresaEdificio.seleccionNuevoDomicilio = false;
VinculoEmpresaEdificio.grillaVinculos = {};

VinculoEmpresaEdificio.init = function (vista, div, prefijo) {
    div = div || "";
    prefijo = prefijo || "";

    // Modificaciones de los id de los campos del editor
    $(div).agregarPrefijo(prefijo);

    var instancia = {};
    instancia.vista = vista;
    instancia.prefijo = "#" + prefijo + "_";

    VinculoEmpresaEdificio.domiciliosAgregados = new Array();
    VinculoEmpresaEdificio.edificiosAgregados = new Array();
    VinculoEmpresaEdificio.initVinculos(instancia);
    VinculoEmpresaEdificio.initEdificios(instancia);

    //ocultamos botones de aceptar/volver para domicilio
    $(instancia.prefijo + "btnVolverDomicilio, " + instancia.prefijo + "btnAceptarNuevoDomicilio").hide();

    VinculoEmpresaEdificio.initFuncionalidadAgregarBorrarVinculo(instancia);
    VinculoEmpresaEdificio.manejoDeEventos(instancia);

    return instancia;

};

//grilla vinculos
VinculoEmpresaEdificio.initVinculos = function (instancia) {
    var titulos = ['Id', 'Id Edificio', 'Identificador edificio', 'Tipo edificio', 'Fecha desde', 'Observación', 'Determina domicilio','Estado vínculo'];
    var propiedades = ['Id', 'IdEdificio', 'IdentificadorEdificio', 'IdTipoEstructuraEdilicia', 'FechaDesde', 'Observacion', 'DeterminaDomicilio', 'EstadoVinculo'];
    var tipos = ['integer', 'integer', 'string', 'string', 'string', 'string', 'bool','string'];
    var caption = 'Nuevas vinculaciones de edificios a empresa';
    var key = 'Id';

    instancia.grillaVinculos = $(instancia.prefijo + "listVinculos").jqGrid({
        datatype: 'local',
        colNames: titulos,
        colModel: GrillaUtil.crearColumnas(propiedades, tipos, key),
        pager: instancia.prefijo + "pagerVinculos",
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: 730,
        height: "100%",
        emptyrecords: "",
        caption: caption,
        loadComplete: function (data) {
            $("#TipoEmpresa").changePatch();
            VinculoEmpresaEdificio.initDomicilios(instancia);
            if (VinculoEmpresaEdificio.grillaVinculos.loadComplete) {
                VinculoEmpresaEdificio.grillaVinculos.loadComplete();
            }
        }
    });

    //fin grilla//
    $(instancia.prefijo + "listVinculos").hideCol('IdEdificio');
    $(instancia.prefijo + "listVinculos").hideCol('DeterminaDomicilio');
    $(instancia.prefijo + "TD[title='Seleccionar']").hide(); // Borro al choto el botón de selección
    $(instancia.prefijo + "listVinculos").setGridWidth(730, true);
    $(instancia.prefijo + 'divGrillaVinculos').show(); //Muestro la grilla
};

//Inicializo grilla de Domicilios
VinculoEmpresaEdificio.initDomicilios = function (instancia) {
    //Inicio grilla//
    //var controller = "GestionEmpresa";
    //var orderBy = "Id";
    var titulos = ['Id', 'Identificador edificio', 'Calle', 'Altura', 'Barrio', 'Localidad', 'Departamento provincial'];
    var propiedades = ['Id', 'EntidadId', 'Calle', 'Altura', 'Barrio', 'Localidad', 'DepartamentoProvincial'];
    var tipos = ['integer', null, null, null, null, null, null];
    var key = 'Id';
    var url = null;

    //metodo q se ejecuta cuando se hace clic en el btn Seleccionar de la grilla de domicilios
    //muestra un div con los datos del domicilio seleccionado
    VinculoEmpresaEdificio.seleccionDomicilio = function (id) {
        VinculoEmpresaEdificio.seleccionNuevoDomicilio = false;
        $(instancia.prefijo + "DivGrillaDomicilio").hide();
        $(instancia.prefijo + "divDatosGeneralesDomicilio").show();
        $(instancia.prefijo + "divDatosGeneralesDomicilio :input[type!='button']").attr("disabled", "disabled");
        var seleccion = GrillaUtil.getSeleccionFilas(grillaDomicilio, false);
        var fila = GrillaUtil.getFila(grillaDomicilio, seleccion);
        $(instancia.prefijo + "Calle").val(fila.Calle);
        $(instancia.prefijo + "Altura").val(fila.Altura);
        $(instancia.prefijo + "Barrio").val(fila.Barrio);
        $(instancia.prefijo + "Localidad").val(fila.Localidad);
        $(instancia.prefijo + "DepartamentoProvincial").val(fila.DepartamentoProvincial);

        //muestro el btn para q se pueda cancelar la seleccion
        $(instancia.prefijo + "btnVolverDomicilio").show();
    };

    //funcionalidad al btn volver de domicilio, para seleccionar un domicilio distinto
    $(instancia.prefijo + "btnVolverDomicilio").click(function () {
        $(instancia.prefijo + "DivGrillaDomicilio").show();
        $(instancia.prefijo + "divDatosGeneralesDomicilio").hide();
        $(instancia.prefijo + "divNuevoDomicilio").hide();
        $(instancia.prefijo + "btnVolverDomicilio").hide();
        $(instancia.prefijo + "btnAceptarNuevoDomicilio").hide();
    });

    //Funcionalidad para el btn Aceptar Nuevo domicilio, en dnd se selecciona la calle e ingresa la altura
    VinculoEmpresaEdificio.funcionalidadBtnNuevoDomicilio = function (domicilio) {

        $(instancia.prefijo + "btnAceptarNuevoDomicilio").click(function () {
            //oculto lo q se selecciona y muestro los datos generales del domicilio con los datos nuevos

            if ($(instancia.prefijo + "comboCalles").val() === "" || $(instancia.prefijo + "AlturaNueva").val() === "") {
                Mensaje.Error.texto = "Ingrese ambos datos de domicilio.";
                Mensaje.Error.mostrar();
                return;
            }
            $(instancia.prefijo + "divNuevoDomicilio").hide();
            $(instancia.prefijo + "divDatosGeneralesDomicilio").show();


            $(instancia.prefijo + "divDatosGeneralesDomicilio :input[type!='button']").attr("disabled", "disabled");
            $(instancia.prefijo + "Calle").val($(instancia.prefijo + "comboCalles option:selected").text());
            $(instancia.prefijo + "Altura").val($(instancia.prefijo + "AlturaNueva").val());
            $(instancia.prefijo + "Barrio").val(domicilio.Barrio);
            $(instancia.prefijo + "Localidad").val(domicilio.Localidad);
            $(instancia.prefijo + "DepartamentoProvincial").val(domicilio.DepartamentoProvincial);
        });
    };
    //si el boton seleccionar no esta cargado
    if ($("#gview_" + instancia.prefijo.substring(1) + "listDomicilio td[title='Seleccionar']").length == 0) {
                                                                                
        var grillaDomicilio = Grilla.Seleccion.init(instancia.prefijo + "listDomicilio", titulos, propiedades, tipos, key, url, "Domicilio", VinculoEmpresaEdificio.seleccionDomicilio, false, "#pagerDomicilio");
        //fin grilla//
        $(grillaDomicilio.id).setGridWidth(730, true);
        $(grillaDomicilio.id).navButtonAdd(grillaDomicilio.id + "_toppager_left",
    {
        position: "last",
        caption: "Nuevo domicilio",
        title: "Nuevo domicilio",
        buttonicon: "ui-icon-check",
        onClickButton: function () {
            //obtengo el id de la fila seleccionada
            var seleccion = GrillaUtil.getSeleccionFilas(grillaDomicilio, false);
            if (seleccion) {
                VinculoEmpresaEdificio.seleccionNuevoDomicilio = true;
                //obtengo la fila seleccionada
                var filaActual = GrillaUtil.getFila(grillaDomicilio, seleccion);
                //cargar el mini editor
                $(instancia.prefijo + "DivGrillaDomicilio").hide();
                $(instancia.prefijo + "divNuevoDomicilio").show();
                $(instancia.prefijo + "comboCalles").attr("disabled", "disabled");
                $(instancia.prefijo + "AlturaNueva").numeric();
                //paso por parametro el id del edificio, q es la columna idEntidad
                //muestro el btn volver por si quiere volver a seleccionar un domicilio
                $(instancia.prefijo + "btnVolverDomicilio").show();
                $.get($.getUrl("/GestionEmpresa/GetCallesPredioByIdEdificio"), { idEdificio: filaActual.EntidadId },
                    function (data) {
                        //cargo el combo con las calles correspondientes
                        //TODO: VER DE HACER ESTA CARGA DE DATOS CON EL MÉTODO DE LA VICKY cargarCombo();
                        //TODO: PERITIR QUE APAREZCA EL BOTÓN VOLVER ANTES DE QUE SE EJECUTE EL GET POR SI FALLA O POR SI QUIERO VOLVER A SELECCIONAR SIN DARLE ACEPTAR
                        $(instancia.prefijo + "comboCalles").html("");
                        $(instancia.prefijo + "comboCalles").append("<option value=''>SELECCIONE</option>");
                        $(instancia.prefijo + "comboCalles").append("<option value=" + data.CalleDomicilio + ">" + data.CalleDomicilio + "</option>");
                        $(instancia.prefijo + "comboCalles").append("<option value=" + data.CalleNorte + ">" + data.CalleNorte + "</option>");
                        $(instancia.prefijo + "comboCalles").append("<option value=" + data.CalleSur + ">" + data.CalleSur + "</option>");
                        $(instancia.prefijo + "comboCalles").append("<option value=" + data.CalleEste + ">" + data.CalleEste + "</option>");
                        $(instancia.prefijo + "comboCalles").append("<option value=" + data.CalleOeste + ">" + data.CalleOeste + "</option>");

                        $(instancia.prefijo + "comboCalles").removeAttr("disabled");
                        $(instancia.prefijo + "AlturaNueva").val("");
                        $(instancia.prefijo + "btnAceptarNuevoDomicilio").show();
                        VinculoEmpresaEdificio.funcionalidadBtnNuevoDomicilio(filaActual);
                    }, "json");
            }
            else {
                AbmcUtil.mensajeSeleccion();
            }
        }
    });
    }
    //fin btn nuevo domicilio

};

//Cargo los combos en cascada de vinculo edifico
VinculoEmpresaEdificio.CargarCombosVinculoEdificio = function (instancia) {
    $(instancia.prefijo + "FiltroLocalidad").CascadingDropDown(instancia.prefijo + "FiltroDepartamentoProvincial", $.getUrl('/Edificio/CargarLocalidadByDepartamentoProvincial'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idDepartamento: $(instancia.prefijo + "FiltroDepartamentoProvincial").val() };
        }
    });

    $(instancia.prefijo + "FiltroBarrio").CascadingDropDown(instancia.prefijo + "FiltroLocalidad", $.getUrl('/Edificio/CargarBarrioByLocalidad'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idLocalidad: $(instancia.prefijo + "FiltroLocalidad").val() };
        }
    });
    $(instancia.prefijo + "FiltroCalle").CascadingDropDown(instancia.prefijo + "FiltroLocalidad", $.getUrl('/Edificio/CargarCalleByLocalidad'), { promptText: 'SELECCIONE',
        postData: function () {
            return { idLocalidad: $(instancia.prefijo + "FiltroLocalidad").val() };
        }
    });
};

//Inicializo grilla de Edificios
VinculoEmpresaEdificio.initEdificios = function (instancia) {
    //Cargo los combos en cascada.
    VinculoEmpresaEdificio.CargarCombosVinculoEdificio(instancia);

    var controller = "VinculoEmpresaEdificio";
    var titulos = ['Id', 'Identificador edificio', 'Tipo edificio', 'Estado', 'Función edificio'];
    var propiedades = ['Id', 'IdentificadorEdificio', 'TipoEdificio', 'Estado', 'FuncionEdificio'];
    var tipos = ['integer', 'string', 'string', 'string', 'string'];
    var key = 'Id';
    var caption = "Selección Edificio";
    var url = $.getUrl("/VinculoEmpresaEdificio/ProcesarBusquedaEdificio/");

    var grillaEdificios = Grilla.Detalle.init(instancia.prefijo + "listEdificios", titulos, propiedades, tipos, key, url, caption, parseInt($("#Id").val()), instancia.prefijo + "pagerDetalleEdificio");

    grillaEdificios.id_limpio = grillaEdificios.id.replace("#", "");
    var divMensajes = "#gview_" + grillaEdificios.id_limpio;
    $(divMensajes).append("<div id='" + grillaEdificios.id_limpio + "_sinConsulta' class='ui-mensaje'>Aún no se ha realizado ninguna búsqueda</div>");
    $(divMensajes).append("<div id='" + grillaEdificios.id_limpio + "_sinRegistros' class='ui-mensaje'>No se encontraron resultados con los datos de búsqueda ingresados</div>");

    //fin grilla//
    $(instancia.prefijo + 'divConsultaEdificio :input').changePatch(function () {
        //var parametrosEdificio = "&" + $(instancia.prefijo + "divConsultaEdificio :input[type!='button']").getFiltros();
        var parametrosEdificio = "&TipoEdificioConsulta=" + $(instancia.prefijo + "TipoEdificioConsulta").val()
        + "&IdentificadorEdificioConsulta=" + $(instancia.prefijo + "IdentificadorEdificioConsulta").val()
        + "&FuncionEdificioConsulta=" + $(instancia.prefijo + "FuncionEdificioConsulta").val()
        + "&IdentificadorPredioConsultaEdificio=" + $(instancia.prefijo + "IdentificadorPredioConsultaEdificio").val()
        + "&DescripcionPredioConsultaEdificio=" + $(instancia.prefijo + "DescripcionPredioConsultaEdificio").val()
        + "&NombreCasaHabitacionConsulta=" + $(instancia.prefijo + "NombreCasaHabitacionConsulta").val()
        + '&FiltroDepartamentoProvincial=' + $(instancia.prefijo + 'FiltroDepartamentoProvincial').val()
        + '&FiltroLocalidad=' + $(instancia.prefijo + 'FiltroLocalidad').val()
        + '&FiltroBarrio=' + $(instancia.prefijo + 'FiltroBarrio').val()
        + '&FiltroCalle=' + $(instancia.prefijo + 'FiltroCalle').val()
        + '&FiltroAltura=' + $(instancia.prefijo + 'FiltroAltura').val();

        parametrosEdificio += "&funcionEdificio=" + $(instancia.prefijo + "FuncionEdificioConsulta").val() + "&identificadorPredio=" + $(instancia.prefijo + "IdentificadorPredioConsultaEdificio").val();
        GrillaUtil.setUrl(grillaEdificios, url + parametrosEdificio);
    });
    $(instancia.prefijo + "btnConsultarEdificio").click(function () {
        if ($(instancia.prefijo + "TipoEdificioConsulta").val() +
            $(instancia.prefijo + "IdentificadorEdificioConsulta").val() +
            $(instancia.prefijo + "FuncionEdificioConsulta").val() +
            $(instancia.prefijo + "IdentificadorPredioConsultaEdificio").val() +
            $(instancia.prefijo + "DescripcionPredioConsultaEdificio").val() +
            $(instancia.prefijo + "NombreCasaHabitacionConsulta").val() +
            $(instancia.prefijo + 'FiltroDepartamentoProvincial').val() +
            $(instancia.prefijo + 'FiltroLocalidad').val() +
            $(instancia.prefijo + 'FiltroBarrio').val() +
            $(instancia.prefijo + 'FiltroCalle').val() +
            $(instancia.prefijo + 'FiltroAltura').val() == "") {
            Mensaje.Error.texto = "Se debe filtrar por al menos un criterio";
            Mensaje.Error.mostrar();
            return;
        }
        grillaEdificios.setGridParam({ datatype: "json", loadComplete: VinculoEmpresaEdificio.funcionLoadComplete });
        grillaEdificios.trigger("reloadGrid");
        $(instancia.prefijo + "listEdificios").show();
    });
    $(instancia.prefijo + "listEdificios").setGridWidth(730, true);
    $(instancia.prefijo + "TD[title='Seleccionar']").hide(); // Borro al choto el botón de selección
    $(instancia.prefijo + 'divGrillaEdificios').show(); //Muestro la grilla
    $(instancia.prefijo + "btnLimpiarConsultaEdificio").click(function () {
        GrillaUtil.limpiar($(instancia.prefijo + "listEdificios"));
        $(instancia.prefijo + "divFiltrosDeConsultaEdificio :input").val("");
    });

    VinculoEmpresaEdificio.funcionLoadComplete = function (data) {
        $(grillaEdificios.id + "_sinRegistros").hide();
        if (data.records === 0) {
            $(grillaEdificios.id + "_sinRegistros").show();
        }
    };
};

VinculoEmpresaEdificio.agregarVinculo = function (instancia, idEdificio, idVin, tipoEdificio, identificadorEdificioGrilla, fecDesde, Obs, estado) {
    var idVinculo = idVin;
    var tipoEdificio = tipoEdificio;
    var identificadorEdificio = identificadorEdificioGrilla;
    var fechaDesde = fecDesde;
    var observacion = Obs;
    var est = estado;
    var data = { Id: idVinculo, IdEdificio: idEdificio, IdentificadorEdificio: identificadorEdificio,
        IdTipoEstructuraEdilicia: tipoEdificio, FechaDesde: fechaDesde, Observacion: observacion, EstadoVinculo: est
    };
    if (fechaDesde == null || fechaDesde == "") { // Checkeamos obligatoriamente la fecha
        Mensaje.Error.texto = "Ingrese la 'Fecha Desde' del Vínculo Empresa a Edificio";
        Mensaje.Error.mostrar();
        return;
    } else {

        if (!Validacion.Fecha({ value: fechaDesde })) {
            Mensaje.Error.texto = "La 'Fecha Desde' del Vínculo Empresa a Edificio tiene un formato invalido";
            Mensaje.Error.mostrar();
            return;
        }
    }
    var nroColumnaRepetida = -1;

    for (var i = 0; i < VinculoEmpresaEdificio.edificiosAgregados.length; i++) {
        if (VinculoEmpresaEdificio.edificiosAgregados[i] == idEdificio) {
            Mensaje.Advertencia.texto = "No se puede vincular, ya que se encuentra vinculado el edificio";
            Mensaje.Advertencia.botones = false;
            Mensaje.Advertencia.mostrar();

            nroColumnaRepetida = i;
            i = VinculoEmpresaEdificio.edificiosAgregados.length;
            return;
        }
    }
    VinculoEmpresaEdificio.edificiosAgregados[VinculoEmpresaEdificio.edificiosAgregados.length] = idEdificio;
    $(instancia.prefijo + "listVinculos").jqGrid().addRowData(idVinculo, data);
    //    if (nroColumnaRepetida == -1) {
    //        $(instancia.prefijo + "listVinculos").jqGrid().addRowData(idVinculo, data);
    //        VinculoEmpresaEdificio.edificiosAgregados[VinculoEmpresaEdificio.edificiosAgregados.length] = idEdificio;
    //    }
    //    else {
    //        $(instancia.prefijo + "listVinculos").jqGrid().setCell(nroColumnaRepetida, "FechaDesde", data.FechaDesde);
    //        $(instancia.prefijo + "listVinculos").jqGrid().setCell(nroColumnaRepetida, "Observacion", data.Observacion);
    //    }


    //Limpio los campos
    $(instancia.prefijo + "listVinculos").jqGrid().resetSelection();
    $(instancia.prefijo + "listEdificios").jqGrid().resetSelection();
    $(instancia.prefijo + "FechaDesdeVinculo").val("");
    $(instancia.prefijo + "ObservacionVinculo").val("");

    //Agrego el Domicilio
    VinculoEmpresaEdificio.agregarDomicilio(instancia, idEdificio);
};

/** Método que se encarga de borrar de la lista de vínculos alguno seleccionado */
VinculoEmpresaEdificio.borrarVinculo = function (instancia) {
    // Traigo el identificador del edificio
    var identificador = GrillaUtil.getFila($(instancia.prefijo + "listVinculos"), GrillaUtil.getSeleccionFilas($(instancia.prefijo + "listVinculos"))).IdentificadorEdificio;

    // Con el identificador traigo el Id de la otra grilla para poder ir a buscar el domicilio
    var idEdificio = GrillaUtil.getFila($(instancia.prefijo + "listVinculos"), GrillaUtil.getSeleccionFilas($(instancia.prefijo + "listVinculos"))).IdEdificio;
    if (idEdificio == -1) {
        Mensaje.Error.texto = "No se encontró el edificio correspondiente al vínculo. No se pudo borrar.";
        Mensaje.Error.mostrar();
        return;
    }
    // Borro el vínculo
    $(instancia.prefijo + "listVinculos").jqGrid().delRowData(GrillaUtil.getSeleccionFilas($(instancia.prefijo + "listVinculos"), false));
    //borro el id del edificio q estaba en la lista de edificios vinculados
    for (var i = 0; i < VinculoEmpresaEdificio.edificiosAgregados.length; i++) {
        if (VinculoEmpresaEdificio.edificiosAgregados[i] == idEdificio) {
            VinculoEmpresaEdificio.edificiosAgregados.shift(i);
            break;
        }
    }
    //si se ha seleccionado el domicilio, lo deselecciono
    if ($(instancia.prefijo + "divDatosGeneralesDomicilio").is(":visible")) {
        $(instancia.prefijo + "btnVolverDomicilio").click();
    }
    //Mando a borrar el domicilio, si es que fue cargado "manualmente"
    if (VinculoEmpresaEdificio.domiciliosAgregados.length > 0) // Si hay domicilios cargados manualmente
    {
        VinculoEmpresaEdificio.borrarDomicilio(instancia, idEdificio);
    }
};

/** Método que trae un domicilio mediante un GET a partir de un edificio y lo inserta en la lista de domicilios si corresponde */
VinculoEmpresaEdificio.agregarDomicilio = function (instancia, idEdificio) {
    $.get($.getUrl("/GestionEmpresa/FindDomicilioDeEdificio"), { idEdificio: idEdificio },
 function (data) {
     var domGrilla = GrillaUtil.getFila($(instancia.prefijo + "listDomicilio"), data.Id);
     if (domGrilla == null || domGrilla == "" || jQuery.isEmptyObject(domGrilla)) { //Si el domicilio del edificio no se encuentra en la grilla, lo agrego.
         var id = data.Id;
         var calle = data.Calle;
         var altura = data.Altura;
         var barrio = data.Barrio;
         var EdificioId = idEdificio;
         var departamentoProvincial = data.NombreDepartamento;
         var localidad;
         $.get($.getUrl("/GestionEmpresa/GetLocalidadString"), { idLocalidad: data.Localidad }, // Traigo el string de la localidad
 function (ret) {
     localidad = ret;
     var data = { Id: id, Calle: calle, Altura: altura, Barrio: barrio, Localidad: localidad, DepartamentoProvincial: departamentoProvincial, EntidadId: EdificioId }
     $(instancia.prefijo + "listDomicilio").jqGrid().addRowData(id, data);
     // Guardo el Id de los domicilios que haya agregado por acá, para poder borrarlos si quito los vínculos
     VinculoEmpresaEdificio.domiciliosAgregados[VinculoEmpresaEdificio.domiciliosAgregados.length] = id;
 }, "json");
     }
 }, "json");
};

/** Método llamado desde borrar vínculo que borra un domicilio de la lista si este fue cargado manualmente en este UC, si lo trajo de db a partir de los vínculos no lo borra */
VinculoEmpresaEdificio.borrarDomicilio = function (instancia, idEdificio) {
    $.get($.getUrl("/GestionEmpresa/FindDomicilioDeEdificio"), { idEdificio: idEdificio }, // Traigo el domicilio
         function (data) {
             for (var i = 0; i < VinculoEmpresaEdificio.domiciliosAgregados.length; i++) { // Si el id del domicilio se encuentra en la lista de agregados, tengo q borrarlo
                 if (data.Id == VinculoEmpresaEdificio.domiciliosAgregados[i]) {
                     $(instancia.prefijo + "listDomicilio").jqGrid().delRowData(data.Id); // Borro el domicilio
                     $("#DomicilioId").val("");
                     VinculoEmpresaEdificio.domiciliosAgregados[i] = null;
                     return;
                 };
             };
         }, "json");
};

//Seteo de domicilio en la grilla de domicilio
VinculoEmpresaEdificio.seleccionarDomicilioEmpresa = function () {
    
    var vinculos = $(instancia.prefijo + "listVinculos").getRowData();
    var domicilios = $(instancia.prefijo + "listDomicilio").getRowData();
    var identificadorEdificio = null;
    for (var i = 0; i < vinculos.length; i++) {
        if (vinculos[i].DeterminaDomicilio == 'true') {
            identificadorEdificio = vinculos[i].IdEdificio;
        }
    }
    for (var i = 0; i < domicilios.length; i++) {
        if (domicilios[i].EntidadId === identificadorEdificio) {
            $(instancia.prefijo + "listDomicilio").setSelection(domicilios[i].Id);
        }
    }
};

//funcionalidad a agregar y borrar vinculo
VinculoEmpresaEdificio.initFuncionalidadAgregarBorrarVinculo = function (instancia) {
    $(instancia.prefijo + "btnVincularEdificio").click(function () {
        var edificioId = GrillaUtil.getSeleccionFilas($(instancia.prefijo + "listEdificios"), false); // Trae el id del edificio
        if (edificioId == null || edificioId == "") {
            Mensaje.Error.texto = "Seleccione el edificio de la lista que quiere vincular a la empresa";
            Mensaje.Error.mostrar();
        }
        else {
            var idVinculo = $(instancia.prefijo + "listVinculos").getGridParam("reccount") * (-1); //Fila a insertar, en número negativo para que no haya problemas de id con los otros objetos
            var filaActual = GrillaUtil.getFila($(instancia.prefijo + "listEdificios"), edificioId);
            var tipoEdificio = filaActual.TipoEdificio; //Traigo el tipo de edificio de la grilla;
            var identificadorEdificioGrilla = filaActual.IdentificadorEdificio; //Traigo el identificador de edificio de la grilla;
            var fechaDesde = $(instancia.prefijo + "FechaDesdeVinculo").val();
            var observacion = $(instancia.prefijo + "ObservacionVinculo").val();
            var estado = "PROVISORIO";
            VinculoEmpresaEdificio.agregarVinculo(instancia, edificioId, idVinculo, tipoEdificio, identificadorEdificioGrilla, fechaDesde, observacion, estado);
        }
    });

    $(instancia.prefijo + "btnBorrarVinculo").click(function () {
        var vinculoId = GrillaUtil.getSeleccionFilas($(instancia.prefijo + "listVinculos"), false); // Trae el id del edificio
        if (vinculoId == null || vinculoId == "") {
            Mensaje.Error.texto = "Seleccione el vínculo de la lista que desea borrar";
            Mensaje.Error.mostrar();
        }
        else {
            VinculoEmpresaEdificio.borrarVinculo(instancia);
        }
    });
};

//Método que setea los vínculos empresa-edificio y carga todos los domicilios en las grillas
VinculoEmpresaEdificio.setearVinculosYDomicilios = function (instancia, vinculos, domicilio) {
    for (var i = 0; i < vinculos.length; i++) {
        VinculoEmpresaEdificio.agregarVinculo(
                                                instancia,
                                                vinculos[i].IdEdificio,
                                                vinculos[i].Id,
                                                vinculos[i].IdTipoEstructuraEdilicia,
                                                vinculos[i].IdentificadorEdificio,
                                                vinculos[i].FechaDesde,
                                                vinculos[i].Observacion,
                                                vinculos[i].EstadoVinculo
                                            );
    }

    //Activo el check para q mueste los vinculos, domicilios, etc
    $("#VincularEdificioCheck").attr("checked", "checked").changePatch();
    //Seteo visualmente el domicilio de la empresa
    VinculoEmpresaEdificio.setearDomicilioPorId(instancia,domicilio);
};

//Método que setea el domicilio y muestra los datos
VinculoEmpresaEdificio.setearDomicilioPorId = function (instancia, domicilio) {    
    $(instancia.prefijo + "Calle").val(domicilio.Calle);
    $(instancia.prefijo + "Altura").val(domicilio.Altura);
    $(instancia.prefijo + "Barrio").val(domicilio.Barrio);
    $(instancia.prefijo + "Localidad").val(domicilio.Localidad);
    $(instancia.prefijo + "DepartamentoProvincial").val(domicilio.DepartamentoProvincial);

    //oculto la grilla de domicilio
    $(instancia.prefijo + "DivGrillaDomicilio").hide();
    //muestro los datos generales y el btn volver
    $(instancia.prefijo + "divDatosGeneralesDomicilio, " + instancia.prefijo + "btnVolverDomicilio").show();
    //deshabilitos los campos
    $(instancia.prefijo + "divDatosGeneralesDomicilio :input").attr("disabled", "disabled");
};

VinculoEmpresaEdificio.manejoDeEventos = function (instancia) {
    $(instancia.prefijo + "IdentificadorEdificioConsulta").numeric();
    $(instancia.prefijo + "IdentificadorEdificioConsulta").attr("maxLength", 10);

    $(instancia.prefijo + "IdentificadorEdificioConsulta").blur(function () {
        if ($(instancia.prefijo + "IdentificadorEdificioConsulta").val() !== "") {
            if (isNaN($(instancia.prefijo + "IdentificadorEdificioConsulta").val())) {
                Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Ingresar solo números en el campo identificador edificio.");
            }
        }
    });

};