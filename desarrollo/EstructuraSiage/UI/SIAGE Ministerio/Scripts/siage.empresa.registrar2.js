Empresa = {};
Empresa.Registrar = {};
Empresa.Registrar.instanciaInstrumentoLegal;
Empresa.Registrar.instanciaInstrumentoLegalParaZonaDesfavorable;

// Método que inicializa el RenderPartial de consulta de empresa en el index
Empresa.Registrar.initConsultar = function (vista) {
    Empresa.Registrar.consulta = ConsultarEmpresa.init(vista, "#divConsulta", 'consultaIndex', false,"",true);

    /* Esto se hace porq el div del formulario en los demas editores de empresa tiene como id="divVista". Al utilizar el abm de Vicky
    el div contenedor del formulario tiene como id="divAbmc" por lo que hay q sobrescribir la variable para q tome toda la funcionalidad */
    ConsultarEmpresa.divVista = "#divAbmc";

    Abmc.init("GestionEmpresa", Empresa.Registrar.consulta.grilla);
    $(Empresa.Registrar.consulta.prefijo + "btnVolver").remove();

};

//Inicializador de la funcionalidad al registrar empresa una vez q se selecciona el Tipo empresa
Empresa.Registrar.initEditorRegistrar = function (vista, estadoText) {
    Empresa.estadoText = estadoText;

    Empresa.Registrar.initFuncionalidadComboSeleccion();

    Empresa.Registrar.initBotones();

    if (Empresa.estadoText === "Editar" || Empresa.estadoText === "Ver") {
        $("#btnSeleccionarTipoEmpresa").click();
        $("#btnSeleccionarTipoEmpresa").hide();        
    }
};

//Funcionalidad de cascada para la selección del tipo de empresa
Empresa.Registrar.initFuncionalidadComboSeleccion = function () {
    $("#TipoEmpresa").attr("disabled", "disabled");
    $("#TipoEmpresa").CascadingDropDown("#TipoGestion", $.getUrl('/GestionEmpresa/CargarTipoEmpresa'), { promptText: 'SELECCIONE', onLoaded: function () {
        if (Empresa.Registrar.TipoEmpresaUsuarioLogueado == "MINISTERIO" && Empresa.estadoText == "Registrar") {
            $("#TipoEmpresa [value='INSPECCION']").remove();
        } else {
            if (Empresa.Registrar.TipoEmpresaUsuarioLogueado == "DIRECCION_DE_NIVEL" && Empresa.estadoText == "Registrar") {
                $("#TipoEmpresa [value='DIRECCION_DE_NIVEL']").remove();
            } 
        }
    }
    });

if (Empresa.Registrar.TipoEmpresaUsuarioLogueado == "MINISTERIO" && Empresa.estadoText == "Registrar") {
        $("#TipoGestion [value='ESCUELA']").remove();
    } else {
        if (Empresa.Registrar.TipoEmpresaUsuarioLogueado == "DIRECCION_DE_NIVEL" && Empresa.estadoText == "Registrar") {
            $("#TipoGestion [value='GESTION_ADMINISTRATIVA'],#TipoGestion [value='DIRECCION_DE_NIVEL']").remove();
        }
    }
};

//Evento de selección de tipo de empresa
Empresa.Registrar.initBotones = function () {
    $("#btnSeleccionarTipoEmpresa").click(Empresa.Registrar.eventoClickSeleccionarEmpresa);
};

//Funcionalidad del evento click del seleccionar empresa
Empresa.Registrar.eventoClickSeleccionarEmpresa = function () {
    var tipoEmpresa = $("#TipoEmpresa").val();
    if (tipoEmpresa) {
        //Deshabilito los combos
        $("#TipoEmpresa, #TipoGestion").attr("disabled", "disabled");
        var url = $.getUrl("/GestionEmpresa/SeleccionTipoEmpresaAbmc");
        $.get(url, { tipo: tipoEmpresa, id: $("#Id").val(), estado: Empresa.estadoText }, function (html) {
            Empresa.Registrar.cargarEditores(html);
        }, "html");
        $("#btnSeleccionarTipoEmpresa").hide();
    }
};



//Método que carga el html de los editores y realiza las inicializaciones
Empresa.Registrar.cargarEditores = function (html) {
    //Seteo el html
    $("#divRegistrarEmpresa").html(html);

    //Oculto el botón
    $("#btnSeleccionarTipoEmpresa").hide();
    //Muestro el código de empresa
    $("#divCodigoEmpresa").show();
    
    //Inicializo las pestañas
    $("#tabs").tabs();

    Empresa.Registrar.initEditoresComunes();

    Empresa.Registrar.initFuncionalidadEstadoTextEmpresa(Empresa.estadoText);

    Empresa.Registrar.manejoDeEventos();

    //Funcionalidad del script de acuerdo al tipo de empresa seleccionado
    Empresa.Registrar.procesarTipoEmpresa();

};

//Método que inicializa los editores y consultas generales del registro de empresa
Empresa.Registrar.initEditoresComunes = function () {

    //Edificios y Domicilio
    Empresa.Registrar.instanciaVinculo = VinculoEmpresaEdificio.init("", "#divVincularEdificioAEmpresa", "Empresa");
    
    //Instrumento Legal
    Empresa.Registrar.instanciaInstrumentoLegal = AsignacionInstrumentoLegal.init("#divInstrumentoLegalGeneral", "InstrumentosLegales");

    //Seleccionar Empresa Padre
    Empresa.Registrar.editorConsultarPadre = ConsultarEmpresa.init(Empresa.Registrar.getNuevaVistaEnum(), '#divSeleccionEmpresaPadre', 'buscarEmpresaPadre', false, "empresa padre", true);
    Empresa.Registrar.editorConsultarPadre.onSelect = function (empresaId) {
        $("#EmpresaPadreOrganigramaId").val(empresaId);
        $(Empresa.Registrar.editorConsultarPadre.prefijo + "btnVolver").val("Buscar empresa padre");
    }


    $(Empresa.Registrar.editorConsultarPadre.grilla.id).setGridWidth(730, true);

    //Seleccionar Empresa Supervisora
    Empresa.Registrar.editorConsultarEmpresaSupervisora = ConsultarEmpresa.init(Empresa.Registrar.getNuevaVistaEnum(), '#divSeleccionEmpresaSupervisora', 'BuscarInspeccionesZonal', false, "empresa supervisora", true);
    $(Empresa.Registrar.editorConsultarEmpresaSupervisora.grilla.id).setGridWidth(730, true);

    Empresa.Registrar.editorConsultarEmpresaSupervisora.onSelect = function (empresaId) {
        $("#EmpresaInspeccionSupervisoraId").val(empresaId);
        var parametro = $("#parametroJerarquiaDeInspeccionIgualAOrganigrama").val();
        //si el parametro jerarquia inspeccion padre organigrama es SI, entonces nuestra empresa padre va a ser nuestra empresa supervisora
        if (parametro == "True") {
            Empresa.Registrar.editorConsultarPadre.seleccion = empresaId;
            ConsultarEmpresa.seleccionarEmpresa(Empresa.Registrar.editorConsultarPadre);
            $(Empresa.Registrar.editorConsultarPadre.prefijo + "btnVolver").hide();            
            $("#liSolapa4").show();
        }

    }
    //oculto el boton de empresa supervisora en empresa tipo impeccion
    $("#btnEmpresaSupervisora").hide();

    //esto se hace porq de por si el script del consultar empresa al inicializar oculta el div del formulario,
    //y muestra el div q contiene los filtros junto a la grilla
    //por ende, se hace lo contrario: se muestra el formulario con los tabs y se oculta la busqueda PRINCIPAL
    $(ConsultarEmpresa.divVista).show();
    $("#divConsulta").hide();


    //Inicializo a mano los botones de aceptar y cancelar
    Empresa.Registrar.initBotonesAceptarCancelar();

    //les saco a los editores de asignacion y de instrumento legal los Id que te traen xq sino te agrega los de la Id de la empresa
    $("#InstrumentosLegales_Id").val(0);
    $("#InstrumentosLegales_InstrumentoLegal_Id").val(0);

};

//Método que inicializa los editores y pestañas según el estadoText
Empresa.Registrar.initFuncionalidadEstadoTextEmpresa = function (estadoText) {
    switch (estadoText) {
        case "Registrar":
            //saco el valor por defecto q VS le pone al ser dato requerido
            $("#FechaInicioActividades").val("");
            break;
        case "Editar":
        case "Ver":
            //carga el valor de tipo gestion
            Empresa.Registrar.cargarTipoGestion();

            //Vínculo Empresa-Edificio y Domicilio
            $.get($.getUrl("/GestionEmpresa/GetVinculosEmpresaEdificio"),
                { empresaId: $("#Id").val() },
                function (data) {
                    VinculoEmpresaEdificio.setearVinculosYDomicilios(Empresa.Registrar.instanciaVinculo, data[0].vin, data[0].domJson);
                    if (estadoText == "Ver") {
                        //Oculto el btnVolver del domicilio seleccionado
                        $(Empresa.Registrar.instanciaVinculo.prefijo + "btnVolverDomicilio").hide();
                    }
                },
                "json");

            //Busco y cargo el usuario que solicito el cierre y la fecha de cierre
            $.get($.getUrl("/GestionEmpresa/GetUsuarioDeCierreEmpresa"), { empresaId: $("#Id").val() },
                    function (data) {
                        if (data) {
                            $("#UsuarioCierre").val(data.UsuarioCierre);
                            $("#FechaCierre").val(data.FechaCierre);
                        }
                    },
                "json");

            if ($("#TipoEmpresa").val() !== "MINISTERIO") {
                Empresa.Registrar.editorConsultarPadre.seleccion = $("#EmpresaPadreOrganigramaId").val();
                ConsultarEmpresa.seleccionarEmpresa(Empresa.Registrar.editorConsultarPadre);
            }

            //Instrumento Legal
            Empresa.Registrar.cargarGrillaInstrumentosLegales();

            if (estadoText === "Editar") {
                $("#divInstrumentoLegalGeneral").show();
                Empresa.Registrar.datosEnModoSoloLectura();
                //Vínculo empresa-edificio
                $("#divVinculos :input").attr("disabled", "disabled");
                $(Empresa.Registrar.instanciaVinculo.prefijo + "divDomicilio :input").removeAttr("disabled");
                $(Empresa.Registrar.instanciaVinculo.prefijo + "btnVincularEdificio").hide();
                $(Empresa.Registrar.instanciaVinculo.prefijo + "btnBorrarVinculo").hide();
                $(Empresa.Registrar.instanciaVinculo.prefijo + "divGrillaEdificios").hide();
                $(Empresa.Registrar.instanciaVinculo.prefijo + "divDatosDelVinculo").hide();
            }

            //si es VER deshabilitos los campos del vinculo empresa - edificio
            if (estadoText === "Ver") {
                $("#divVinculos :input, #divSeleccionEmpresaPadre :input").attr("disabled", "disabled");
                $("#Empresa_divGrillaEdificios, #Empresa_divDatosDelVinculo, #buscarEmpresaPadre_btnVolver").hide();
                $("#FechaAlta").unmask("99/99/9999");
                //Funcionalidad del btnVolver
                Empresa.Registrar.funcionalidadBotonVolver();
            }

            //acorto las fecha
            $("#FechaNotificacion").val($("#FechaNotificacion").val().substring(0, 10));
            $("#FechaInicioActividades").val($("#FechaInicioActividades").val().substring(0, 10));

            break;
        default:
            break;
    }
    if ($("#TipoGestion option:selected").text() === "GESTION_ADMINISTRATIVA") {
        Empresa.Registrar.empresasTipoGestionAdministrativa();
    }
};

Empresa.Registrar.funcionalidadBotonVolver = function () {
    $(Abmc.btnVolver + "[value = 'Volver']").hide();

    //Evento click del btnVolver
    $("#btnVolverEmpresa").click(function () {
        Abmc.mostrarDivConsulta();
        Abmc.registrando = false;
    });

    $("#divBtnVolver").show();
};

//carga los turnos y periodos lectivos en la escuela madre
Empresa.Registrar.cargarTurnosYPeriodosLetivosEnEscuela = function () {
    
};

//Inicializo la grilla con los instrumentos legales asociados a la empresa seleccionada
Empresa.Registrar.cargarGrillaInstrumentosLegales = function () {
    var grid = $("#listInstrumentosLegales").jqGrid({
        datatype: 'json',
        url: $.getUrl("/GestionEmpresa/GetInstrumentosLegalesByEmpresaId/?empresaId=" + $("#Id").val()),
        colNames: ['Id', 'Número', 'Emisor', 'Fecha emisión', 'Observaciones', 'Tipo instrumento legal', 'Tipo movimiento', 'Fecha Notificación'],
        colModel: [
                    { name: 'Id', index: 'Id', width: 55, key: true, jsonmap: 'Id', hidden: true },
                    { name: 'Numero', index: 'Numero', width: 70, sortable: true, jsonmap: 'Numero' },
                    { name: 'Emisor', index: 'Emisor', width: 70, sortable: true, jsonmap: 'Emisor' },
                    { name: 'FechaEmision', index: 'FechaEmision', width: 70, sortable: true, jsonmap: 'FechaEmision', formatter: 'date', formatoptions: { srcformat: "d-m-Y", newformat: "d/m/Y"} },
                    { name: 'Observaciones', index: 'Observaciones', width: 70, sortable: true, jsonmap: 'Observaciones' },
                    { name: 'TipoInstrumentoLegal', index: 'TipoInstrumentoLegal', width: 70, sortable: true, jsonmap: 'TipoInstrumentoLegal' },
                    { name: 'TipoMovimiento', index: 'TipoMovimiento', width: 70, sortable: true, jsonmap: 'TipoMovimiento' },
                    { name: 'FechaNotificacion', index: 'FechaNotificacion', width: 70, sortable: true, jsonmap: 'FechaNotificacion'  }

                  ],
        pager: '#pagerInstrumentoLegal',
        rowNum: 10,        
        sortorder: 'desc',
        sortname: 'FechaNotificacion',
        viewrecords: true,
        autowidth: false,
        shrinkToFit: false,
        width: 730,
        caption: "Instrumentos legales",
        height: "100%"
    });
    grid.setGridWidth(730, true);
};

//podria recibir por parametro el tipo de empresa, asi se puede hacer un switch
//y ejecutar codigo dependiendo de lo q se intenta registrar
Empresa.Registrar.manejoDeEventos = function () {
    //cuando hacems click en la seleccion de empresa supervisora
    $("#btnEmpresaSupervisora").click(function () {
        //mostramos el filtro de busqueda
        $("#divSeleccionEmpresaSupervisora").toggle();

    });

    /** GENERALES */
    //Check de Vínculo empresa-edificio
    $("#VincularEdificioCheck").changePatch(function () {
        if ($("#VincularEdificioCheck").is(":checked")) {
            $("#divVincularEdificioAEmpresa").show();
        }
        else {
            $("#divVincularEdificioAEmpresa").hide();
        }
    });

    //le saco los val-Required del editor de instrumento legal para que no me los pida al momento de registrar
    $("#divAsignacionInstrumentoLegalGeneral").editorOpcional("#checkRegistrarInstrumento");
    $("#checkRegistrarInstrumento").changePatch();

    //$(Empresa.Registrar.instanciaInstrumentoLegal.prefijo + "InstrumentoLegal_RegistrarExpediente").changePatch();

    //Seteo el máximo de caracteres para algunos campos.
    $("#Sigla").attr("maxLength", 10);
    $("#CUE").attr("maxLength", 10);
    $("#CUEAnexo").attr("maxLength", 2);
    $("#NumeroAnexo").attr("maxlength", 2);
    $("#CodigoEmpresa").attr("maxlength", 9);
    $("#NumeroEscuela").attr("maxlength", 5);
    $("#CodigoInspeccion").attr("maxlength", 6);
    $("#NumeroEscuela").attr("disabled","disabled");
    $("#Nombre, #Observaciones").attr("maxlength", 200);
    $("#PorcentajeAporteEstado").attr("maxlength", 3);
    $("#Telefono").attr("maxlength", 12);
    $("#Nombre, #HorarioDeFuncionamiento").attr("maxlength", 50);
    $("#NumeroCuentaBancaria, #Colectivos").attr("maxlength", 30);
    $("#Telefono, #PorcentajeAporteEstado, #NumeroAnexo, #CUEAnexo, #CUE").numeric();

    //Valido que solo se ingresen numeros en campos.
    Empresa.Registrar.validarIngresoSoloNumeros("Telefono", "Telefono");
    Empresa.Registrar.validarIngresoSoloNumeros("NumeroAnexo", "Numero anexo");
    Empresa.Registrar.validarIngresoSoloNumeros("CUEAnexo", "CUE Anexo");
    Empresa.Registrar.validarIngresoSoloNumeros("CUE", "CUE");
    Empresa.Registrar.validarIngresoSoloNumeros("PorcentajeAporteEstado", "Porcentaje aporte estado");

    $("#PorcentajeAporteEstado").blur(function () {
        if ($("#PorcentajeAporteEstado").val() > 100) {
            Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Ingresar un valor menor a 100 en el campo Porcentaje aporte estado");
            $("#PorcentajeAporteEstado").val("");
        }
    });

    if (Empresa.estadoText === "Registrar") {
        //Agrego máscara a los DateTime
        $(".val-DateTime").mask("99/99/9999");
    }
};

//Valido que solo se ingresen numeros en el campo pasado por parámetro
Empresa.Registrar.validarIngresoSoloNumeros = function (selector, campo) {
    $("#" + selector).blur(function () {
        if ($("#" + selector).val() !== "") {
            if (isNaN($("#" + selector).val())) {
                Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Ingresar solo números en el campo " + campo);
            }
        }
    });
};

//Método que inicializa el script de los botones aceptar y cancelar del ABM
Empresa.Registrar.initBotonesAceptarCancelar = function () {
    $(Abmc.btnAceptar).click(Abmc.enviarModelo);
    //porque sino agarra el btnVolver de otro editor incluido
    $(Abmc.btnVolver + "[value = 'Cancelar']").click(function () {
        Abmc.mostrarDivConsulta();
        Abmc.registrando = false;
    });
};

//Devuelve un string con la vista q se va a usar en la consulta de empresa correspondiente
Empresa.Registrar.getNuevaVistaEnum = function () {
    var filtroTipoEmpresa;
    switch ($('#TipoEmpresa option:selected').text()) {
        case 'DIRECCION_DE_INFRAESTRUCTURA':
        case 'DIRECCION_DE_RECURSOS_HUMANOS':
        case 'DIRECCION_DE_SISTEMAS':
        case 'DIRECCION_DE_TESORERIA':
            filtroTipoEmpresa = 'SoloMinisterio';
            break;
        case 'SECRETARIA':
            filtroTipoEmpresa = 'BusquedaPorSecretaria';
            break;
        case 'SUBSECRETARIA':
            filtroTipoEmpresa = 'BusquedaPorSubSecretaria';
            break;
        case 'APOYO_ADMINISTRATIVO':
            filtroTipoEmpresa = 'BusquedaPorApoyoAdm';
            break;
        case 'INSPECCION':
            var selector = "TipoInspeccionEnum"
            if ($("#TipoInspeccion option").length > 3) {
                selector = "TipoInspeccion";
            }
            if ($("#" + selector + " option:selected").text() != "SELECCIONE") {                
                $.ajax({
                    url: $.getUrl('/GestionEmpresa/GetFiltroEmpresaPadreInspeccion/?tipoInspeccion=' + $('#' + selector + '  option:selected').text()),
                    type: "GET",
                    async: false,
                    success: function (data) {
                        filtroTipoEmpresa = data;
                    }
                });
            } else {
               
                filtroTipoEmpresa = "CualquierInspeccionDeDireccionDeNivelDelUsuarioLogueado";
            }
            break;
        case 'DIRECCION_DE_NIVEL':
            filtroTipoEmpresa = 'SoloMinisterio';
            break;
        case 'ESCUELA_MADRE':
            filtroTipoEmpresa = 'BusquedaPorEscuelaMadre';
            break;
        case 'ESCUELA_ANEXO':
            filtroTipoEmpresa = 'BusquedaPorEscuelaAnexo';
            break;
        default:
            break;
    }
    return filtroTipoEmpresa;
};

//Elimino la solapa ingresada por parametro
Empresa.Registrar.eliminarSolapas = function (solapa) {
    $("#div" + solapa).html("");
    $("#li" + solapa).remove();
};

//Funcionalidad del script de acuerdo al tipo de empresa seleccionado
Empresa.Registrar.procesarTipoEmpresa = function () {
    switch ($('#TipoEmpresa option:selected').text()) {
        case 'MINISTERIO':
            Empresa.Registrar.empresaMinisterio(); 
            break;
        case 'APOYO_ADMINISTRATIVO':
        case 'DIRECCION_DE_INFRAESTRUCTURA':
        case 'DIRECCION_DE_RECURSOS_HUMANOS':
        case 'DIRECCION_DE_SISTEMAS':
        case 'DIRECCION_DE_TESORERIA':
        case 'SECRETARIA':
        case 'SUBSECRETARIA':
            break;

        case 'DIRECCION_DE_NIVEL':
            Empresa.Registrar.empresaDireccionDeNivel();
            break;

        case 'ESCUELA_MADRE':
        case 'ESCUELA_ANEXO':
            //Inicializo las pestañas
            $("#escuelaTabs").tabs();
            Empresa.Registrar.empresaEscuela();
            break;

        case 'INSPECCION':
            Empresa.Registrar.empresaInspeccion();
            break;

        default:
            break;
    }
};


/**************************************************** AREA DIRECCION DE NIVEL ****************************************************/
Empresa.Registrar.empresaDireccionDeNivel = function () {

    //Inicialización y funcionalidad de la grilla de tipo escuelas
    Empresa.Registrar.cargarGrillaTipoEscuelas();

    //Inicialización y funcionalidad de la grilla nivele educativo por tipo educacion
    Empresa.Registrar.cargarGrillaNivelesEducativosPorTipoEducacion();

    //Manejo de eventos
    Empresa.Registrar.manejoDeEventosDireccionDeNivel();
};

//Manejo de eventos
Empresa.Registrar.manejoDeEventosDireccionDeNivel = function () {
    var NivelSeleccionado;
    $("#NivelEducativoId").focus(function () {
        NivelSeleccionado = $("#NivelEducativoId option:selected").val();
    });
    //Funcionalidad del combo em cascada de tipo educacion (padre) y nivel educativo (hijo)
    $("#NivelEducativoId").CascadingDropDown("#TipoEducacion", $.getUrl("/GestionEmpresa/CargarNivelEducativo"), { promptText: 'SELECCIONE', loadingText: "Cargando.." });

    
    //Ejecuto de  nuevo el metodo para que se cargue el combo de niveles educativos
    $("#TipoEducacion").changePatch();

    //Funcionalidad del evento change del combo tipo educacion.
    $("#TipoEducacion").changePatch(function () {
        $("#NivelEducativoId").attr("disabled", false);
        $("#listNETE").clearGridData();
    });

    if (Empresa.estadoText == "Ver") {
        $("#TipoEscuela,label[for='TipoEscuela'] ").hide();
        $("#NivelEducativoId,label[for='NivelEducativoId'] ").hide();
        $(Empresa.Registrar.instanciaVinculo.prefijo + "btnVolverDomicilio").hide();
    }
};

//Inicializacion y funcionalidad de la grilla tipo de escuelas
Empresa.Registrar.cargarGrillaTipoEscuelas = function () {
    //Inicializo la grilla de tipo escuelas
    var grid = $("#listTE").jqGrid({
        datatype: "local",
        colNames: ["id", "IdTE", "Tipos de escuelas"],
        colModel: [
                { key: true, name: "id", index: "id", align: "left", hidden: true },
                { key: false, name: "idTE", index: "idTE", align: "left", hidden: true },
                { key: false, name: "tipoEscuela", index: "tipoEscuela", align: "left" }
            ],
        rowNum: 10,
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: 730,
        height: "100%",
        loadComplete: function () {
            var TiposDeEscuela = Empresa.Registrar.DireccionDeNivel.TiposDeEscuelas;
            for (var i = 0; i < TiposDeEscuela.length; i++) {
                var te = {
                    idTE: TiposDeEscuela[i].idTE,
                    tipoEscuela: TiposDeEscuela[i].tipoEscuela
                };
                $("#listTE").addRowData(i, te, "last");
            }
        }
    });

    //Evento click del boton agregar tipo escuela a la grilla
    $("#btnAgregarTE").click(function () {
        if ($('#TipoEscuela option:selected').text() == "SELECCIONE") {
            Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Debe seleccionar un tipo de escuela");
            return;
        }
        var TE = {
            id: grid.getGridParam("reccount") + 1,
            idTE: $("#TipoEscuela").val(),
            tipoEscuela: $('#TipoEscuela option:selected').text()
        };
        if ($("#TipoEscuela").val() != "") {
            var data = grid.getRowData();
            for (i = 0; i < data.length; i++) {
                if (data[i].idTE === $("#TipoEscuela").val()) {
                    Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("El tipo de escuela ya se encuentra cargado");
                    return;
                }
            }
            grid.addRowData(TE.id, TE, "last");
            $('#TipoEscuela').val(-1);
            grid.show();
        }
    });

    //Evento click del boton eliminar tipo escuela de la grilla
    $("#btnEliminarTE").click(function () {
        var seleccion = GrillaUtil.getSeleccionFilas(grid, false);
        if (seleccion && seleccion.lenght !== 0) {
            grid.delRowData(seleccion);

            if (grid.getGridParam("reccount") === 0) {
                grid.hide();
            }
            else {
                var data = grid.getRowData();
                var json = {};
                json.total = json.page = 1;
                json.records = data.length;
                json.rows = [];

                grid.clearGridData();
                for (i = 0; i < data.length; i++) {
                    data[i].id = i + 1;
                    grid.addRowData(data[i].id, data[i], "last");
                }
            }
        }
        else {
            AbmcUtil.mensajeSeleccion();
        }
    });
};

//Inicializacion y funcionalidad de la grilla nivel educativo por tipo educacion
Empresa.Registrar.cargarGrillaNivelesEducativosPorTipoEducacion = function () {
    //Inicializacion de la grilla nivele educativo por tipo educacion
    var yaPasoPorAca = false;
    var grid = $("#listNETE").jqGrid({
        datatype: "local",
        colNames: ["id", "IdNE", "Nivel Educativo", "Tipo Educación"],
        colModel: [
                { key: true, name: "id", index: "id", align: "left", hidden: true },
                { key: false, name: "idNE", index: "idNE", align: "left", hidden: true },
                { key: false, name: "nivelEducativo", index: "nivelEducativo", align: "left" },
                { key: false, name: "tipoEducacion", index: "tipoEducacion", align: "left", hidden: true }
            ],
        rowNum: 10,
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: 730,
        height: "100%",
        loadComplete: function () {
            var NivelEducativo = Empresa.Registrar.DireccionDeNivel.NivelEducativo;
            for (var i = 0; i < NivelEducativo.length; i++) {
                var nete = {
                    idNE: NivelEducativo[i].idNE,
                    nivelEducativo: NivelEducativo[i].nivelEducativo,
                    tipoEducacion: NivelEducativo[i].tipoEducacion
                };
                $("#listNETE").addRowData(i, nete, "last");
            }
        }
    });

    //Evento click del boton agregar nivel educativo por tipo educacion a la grilla
    $("#btnAgregarNETE").click(function () {
        //si es la primera vez q se agrega, guardar el tipo de educacion
        if (!yaPasoPorAca) {
            Empresa.Registrar.TipoEducacion = $('#TipoEducacion').val();
            yaPasoPorAca = true;
        }

        //si el tipo de educacion cambió y se intenta agregar otro nivel educativo, limpiar la grilla
        if ($('#TipoEducacion').val() != "SELECCIONE" && $('#TipoEducacion').val() != Empresa.Registrar.TipoEducacion) {
            $('#listNETE').clearGridData(false);
            Empresa.Registrar.TipoEducacion = $('#TipoEducacion').val();
        }

        if ($('#TipoEducacion option:selected').text() == "SELECCIONE") {
            Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Debe seleccionar un nivel educativo y un tipo educación");
            return;
        }
        var NETE = {
            id: grid.getGridParam("reccount") + 1,
            idNE: $("#NivelEducativoId").val(),
            nivelEducativo: $('#NivelEducativoId option:selected').text(),
            tipoEducacion: $('#TipoEducacion option:selected').text()
        };
        //verifico q no se quiera agregar un nivel educativo repetido
        if ($("#NivelEducativoId").val() != "") {
            var data = grid.getRowData();
            for (i = 0; i < data.length; i++) {
                if (data[i].idNE === $("#NivelEducativoId").val() && data[i].tipoEducacion === $('#TipoEducacion option:selected').text()) {
                    Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Nivel educativo por tipo educación ya existe");
                    return;
                }
            }
            grid.addRowData(NETE.id, NETE, "last");
            $('#NivelEducativoId').val(-1);
            grid.show();
        }
    });

    //Evento click del boton eliminar nivel educativo por tipo educacion de la grilla
    $("#btnEliminarNETE").click(function () {
        var seleccion = GrillaUtil.getSeleccionFilas(grid, false);
        if (seleccion && seleccion.lenght !== 0) {
            grid.delRowData(seleccion);

            if (grid.getGridParam("reccount") === 0) {
                grid.hide();
            }
            else {
                var data = grid.getRowData();
                var json = {};
                json.total = json.page = 1;
                json.records = data.length;
                json.rows = [];

                grid.clearGridData();
                for (i = 0; i < data.length; i++) {
                    data[i].id = i + 1;
                    grid.addRowData(data[i].id, data[i], "last");
                }
            }
        }
        else {
            AbmcUtil.mensajeSeleccion();
        }
    });
};
/**************************************************** FIN AREA DIRECCION DE NIVEL ****************************************************/

Empresa.Registrar.empresasTipoGestionAdministrativa = function () {
    Empresa.Registrar.eliminarSolapas("Solapa5");
};


/**************************************************** AREA MINISTERIO ****************************************************/
Empresa.Registrar.empresaMinisterio = function () {
    //elimina el tab de seleccion de empresa padre
    Empresa.Registrar.eliminarSolapas("Solapa4");
};
/**************************************************** FIN AREA MINISTERIO ****************************************************/

/**************************************************** AREA ESCUELA  ****************************************************/
Empresa.Registrar.empresaEscuela = function () {

    //Inicializo los editores de consulta de empresas
    Empresa.Registrar.initEditoresConsultaEmpresas();

    //Inicializo la grilla de turnos
    Empresa.Registrar.initGrillaTurnos();

    //Inicialozo la grilla de periodos lectivos
    Empresa.Registrar.initGrillaPeriodoLectivo();

    //Inicializo el editor para Instrumento legal en zona desfavorable
    Empresa.Registrar.initInstrumentoLegalParaZonaDesfavorable();

    //Manejo de eventos
    Empresa.Registrar.manejoDeEventosEscuela();


    $("#divEsPrivado").editorOpcional("#Privado");
    //$("#Privado").changePatch();


    if (Empresa.estadoText !== "Registrar") {



        $("#btnEscuelaRaiz").hide();
        $("#RegistrarEstructuraEscolarCheck").click().changePatch();
        Empresa.Registrar.cargarGrillaEstructuraEscolar();
        //oculto los botones de la grilla, no se puede editar la estr. escolar desde empresa
        $("#listaEstructura_toppager td.ui-pg-button").hide();
        //deshabilito el check de estr definitiva
        $("#EstructuraDefinitiva").attr("disabled", "disabled");
        var divMensajes = "#gview_listaEstructura";
        $(divMensajes).append("<div id='listaEstructura_sinConsulta' class='ui-mensaje'>No se encontraron registros</div>");
        if ($("#listaEstructura").getDataIDs().length === 0)
            $("#listaEstructura_sinConsulta").show();

        //si tiene CUE y el valor de CUE anexo es 0, quiere decir q se ha registrado cue anexo 00
        //ya q no se permite solo un 0. si existe CUE, es obligatorio el CUE anexo
        if ($("#CUE").val() !== "") {
            if ($("#CUEAnexo").val() === "0") {
                $("#CUEAnexo").val("00");
            }
        }
        //si no tiene CUE le saco el 0 del CUE anexo
        else {
            $("#CUEAnexo").val("");
        }

    }
    else { // Si es registrar oculto
        //Lo comento porque en el ticket #1430 dicen que por mas que no se registren, en el ver los tiene que mostrar
        //$("#HorarioDeFuncionamiento").hide();
        //$("#Colectivos").hide();
        //$("#HorarioDeFuncionamiento,label[for='HorarioDeFuncionamiento']").hide();
        //$("#Colectivos,label[for='Colectivos']").hide();


    }
    $("#NumeroAnexo").numeric();
    $("#NumeroEscuela").numeric();



    if ($("#TipoEmpresa").val() == "ESCUELA_MADRE" || $("#TipoEmpresa").val() == "ESCUELA_ANEXO") {
        $("label[for='TipoEscuela']").text("Tipo escuela(*): ");

        if (Empresa.estadoText != "Registrar" && $("#NivelEducativoId option:selected").text() === "PRIMARIO") {
            $("#CodigoInspeccion,label[for='CodigoInspeccion']").show();
        } else {
            $("#CodigoInspeccion,label[for='CodigoInspeccion']").hide();
        }

        if (Empresa.estadoText != "Registrar" && $("#TipoEmpresa").val() == "ESCUELA_ANEXO") {
            Empresa.Registrar.editorConsultarPadre.seleccion = $("#EscuelaMadreId").val();
            ConsultarEmpresa.seleccionarEmpresa(Empresa.Registrar.editorConsultarPadre);
        }
    }
};

//Cargo la grilla estructura escolar
Empresa.Registrar.cargarGrillaEstructuraEscolar = function () {
    //ahora lo trae de la misma forma q trae los turnos, periodos lectivos, etc.
    //queda en stand by hasta q se pruebe bien, si llega a andar, borrar este get:
    //    $.get($.getUrl("/GestionEmpresa/GetEstructuraEscolar/"), { escuelaId: $("#Id").val() }, function (data) {

    //        if (data) {
    //            data = JSON.parse(data);
    //            for (var i = 0; i < data.length; i++) {
    //                $("#listaEstructura").addRowData(i,data[i],"last");
    //            }
    //        }
    //    });
    var estructura = Empresa.Registrar.Escuela.EstructuraEscolar;
    if (estructura) {
        for (var i = 0; i < estructura.length; i++) {
            $("#listaEstructura").addRowData(i, estructura[i], "last");
        }
    }

};

//validacion del parametro
Empresa.Registrar.validacionParametroPadreOrganigramaEmpresaEnEscuela = function () {
    var parametro = $("#parametroJerarquiaDeInspeccionIgualAOrganigrama").val();

    if (parametro == "True") {
        $("#buscarEmpresaPadre_btnVolver").hide();
        if (Empresa.estadoText == "Registrar") {
            $("#liSolapa4").hide();
            $("#divSolapa4").hide();
        }
    }

};

//Inicializo el instrumento legal para cuando la zona desfavorable es diferente de A
Empresa.Registrar.initInstrumentoLegalParaZonaDesfavorable = function () {
    Empresa.Registrar.instanciaInstrumentoLegalParaZonaDesfavorable = AsignacionInstrumentoLegal.init("#divInstrumentoLegalParaZonaDesfaforable", "AsignacionInstrumentoLegalZonaDesfavorable");
    $("#divInstrumentoLegalParaZonaDesfaforable .val-DateTime").each(function (input) {
        var data = $(this).data("datepicker");
        if (data) {
            data.id = this.id;
            $(this).data("datepicker", data);
        }
    });
};

//Inicalizacion de los editores
Empresa.Registrar.initEditoresConsultaEmpresas = function () {
    //Init editor consultar escuela raiz
    Empresa.Registrar.editorConsultaEscuelaRaiz = ConsultarEmpresa.init("BusquedaPorEscuelaRaiz", "#divSeleccionEscuelaRaiz", 'consultaEscuelaRaiz', false, " escuela raíz", true);
    $(Empresa.Registrar.editorConsultaEscuelaRaiz.grilla.id).setGridWidth(730);

    //Init editor consultar empresa inspeccion
    Empresa.Registrar.editorConsultarEInspeccion = ConsultarEmpresa.init('BuscarInspeccionesZonal', '#divSeleccionEmpresaInspeccion', 'buscarEInspeccion', false, "empresa inspección", true);
    $(Empresa.Registrar.editorConsultarEInspeccion.grilla.id).setGridWidth(730, true);


    if ($("#TipoGestion").val() == "ESCUELA") {

        Empresa.Registrar.editorConsultarEInspeccion.onSelect = function (seleccion) {
            Empresa.Registrar.editorConsultarPadre.seleccion = seleccion;
            ConsultarEmpresa.seleccionarEmpresa(Empresa.Registrar.editorConsultarPadre);
            //asignamos los id de las empresas a los hidden para que llege al model 
            $("#EmpresaPadreOrganigramaId").val(seleccion);
            $("#EmpresaInspeccionId").val(seleccion);
            //mostramos la consulta de empresa padre
            $("#liSolapa4").show();
            $("#divSolapa4").show();
        }
    }


    //Init editor consultar empresa escuela madre
    Empresa.Registrar.editorConsultarMadre = ConsultarEmpresa.init('BusquedaPorEscuelaAnexo', '#divSeleccionEscuelaMadre', 'buscarEscuelaMadre', false, "escuela madre", true);
    $(Empresa.Registrar.editorConsultarMadre.grilla.id).setGridWidth(730, true);
    //$(Empresa.Registrar.editorConsultarMadre.grilla.id + "sinRegistros").hide();
};

//init grilla periodos lectivos
Empresa.Registrar.initGrillaPeriodoLectivo = function () {
    $("#listPeriodosLectivos").jqGrid({
        datatype: 'local',
        colNames: ['Id', 'Periodo Lectivo', 'PeriodoLectivoId'],
        colModel: [
                        { name: 'id', index: 'id', width: 55, key: true, jsonmap: 'id', hidden: true },
                        { name: 'PeriodoLectivoText', index: 'PeriodoLectivoText', width: 250, sortable: false, jsonmap: 'PeriodoLectivoText' },
                        { name: 'PeriodoLectivoId', index: 'PeriodoLectivoId', sortable: false, jsonmap: 'PeriodoLectivoId', hidden: true }
                  ],
        rowNum: 10,
        sortname: 'invid',
        sortorder: 'desc',
        viewrecords: true,
        width: '100%',
        height: "100%",
        loadComplete: function () {
            var Periodos = Empresa.Registrar.Escuela.PeriodosLectivos;
      
            for (var i = 0; Periodos.length > i; i++) {
                var pl = { PeriodoLectivoText: Periodos[i].PeriodoLectivoText,
                    PeriodoLectivoId: Periodos[i].PeriodoLectivoId
                };
                $("#listPeriodosLectivos").addRowData(i, pl, "last");
            }
        }

    });

    //funcionalidad al boton Agregar
    $("#btnAgregarPeriodoLectivo").click(function () {
        if ($('#PeriodoLectivoId option:selected').text() == "SELECCIONE") {
            Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Debe seleccionar un periodo lectivo");
            return;
        }
        var peridoLectivo = {
            Id: $("#listPeriodosLectivos").getGridParam("reccount") + 1,
            PeriodoLectivoId: $("#PeriodoLectivoId :selected").val(),
            PeriodoLectivoText: $("#PeriodoLectivoId :selected").text()
        };

        //validar que el estado no este agregado
        if ($("#PeriodoLectivoId").val() != "") {
            var data = $("#listPeriodosLectivos").getRowData();

            for (i = 0; i < data.length; i++) {
                if (data[i].PeriodoLectivoText === $("#PeriodoLectivoId :selected").text()) {
                    Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("El periodo lectivo " + data[i].PeriodoLectivoText + " ya se encuentra agregado");
                    return;
                }
            }
        };

        $("#listPeriodosLectivos").addRowData(peridoLectivo.Id, peridoLectivo, 'last');
        $('#PeriodoLectivoId').val(-1);

    });

    //funcionalidad al btnEliminarPeriodoLectivo
    $("#btnEliminarPeriodoLectivo").click(function () {
        var data = $("#listPeriodosLectivos").jqGrid("getGridParam", "selrow");
        $("#listPeriodosLectivos").delRowData(data);
    });
};

//Init grilla turnos
Empresa.Registrar.initGrillaTurnos = function () {
    var grid = $("#listTurnos").jqGrid({
        datatype: "local",
        colNames: ["id", "IdTurnos", "Turnos"],
        colModel: [
                    { key: true, name: "id", index: "id", align: "left", hidden: true },
                    { key: false, name: "idTurnos", index: "idTurnos", align: "left", hidden: true },
                    { key: false, name: "turnos", index: "turnos", align: "left" }
                ],
        rowNum: 10,
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: 730,
        height: "100%",
        loadComplete: function () {
            for (var i = 0; Empresa.Registrar.Escuela.Turnos.length > i; i++) {
                var t = {
                    idTurnos: Empresa.Registrar.Escuela.Turnos[i].Id,
                    turnos: Empresa.Registrar.Escuela.Turnos[i].Nombre
                };                
                $("#listTurnos").addRowData(i, t, "last");
            }
        }
    });

    //Evento click del boton agregar turno a la grilla
    $("#btnAgregarTurno").click(function () {
        if ($('#TurnoId option:selected').text() == "SELECCIONE") {
            Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Debe seleccionar un turno");
            return;
        }
        var Turno = {
            id: grid.getGridParam("reccount") + 1,
            idTurnos: $("#TurnoId").val(),
            turnos: $('#TurnoId option:selected').text()
        };
        if ($("#TurnoId").val() != "") {
            var data = grid.getRowData();
            for (i = 0; i < data.length; i++) {
                if (data[i].idTurnos === $("#TurnoId").val()) {
                    Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("El turno " + data[i].turnos + " ya se encuentra agregado");
                    return;
                }
            }
            grid.addRowData(Turno.id, Turno, "last");
            $('#TurnoId').val(-1);
            grid.show();
        }

    });

    //Cargo el combo turno de estructura escuela
    $("#EstructuraEscolar_Turno").focus(function () {
        Empresa.Registrar.cargarComboTurnoEstructuraEscolar();
    });

    //Evento click del boton eliminar turno de la grilla de turnos
    $("#btnEliminarTurno").click(function () {
        var seleccion = GrillaUtil.getSeleccionFilas(grid, false);
        if (seleccion && seleccion.lenght !== 0) {
            var diagramaciones = $("#listaEstructura").getRowData();
            for (var i = 0; i < diagramaciones.length; i++)
                if (diagramaciones[i].Turno == seleccion) {
                    Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("No puede eliminarse el turno " + diagramaciones[i].TurnoNombre + " ya que se utiliza en estrucura escolar");
                    return;
                }
            grid.delRowData(seleccion);

            if (grid.getGridParam("reccount") === 0) {
                grid.hide();
            }
            else {
                var data = grid.getRowData();
                var json = {};
                json.total = json.page = 1;
                json.records = data.length;
                json.rows = [];

                grid.clearGridData();
                for (i = 0; i < data.length; i++) {
                    data[i].id = i + 1;
                    grid.addRowData(data[i].id, data[i], "last");
                }
            }
        }
        else {
            AbmcUtil.mensajeSeleccion();
        }
    });
};

//Cargo el combo de los turnos de la estructura escuela en base a los turnos seleccionados anteriormente.
Empresa.Registrar.cargarComboTurnoEstructuraEscolar = function () {
    var turnos = $("#listTurnos").getRowData();
    var html = "<option value='-1'>SELECCIONE</option>";
    for (var i = 0; i < turnos.length; i++) {
        html += "<option value='" + turnos[i].idTurnos + "'>" + turnos[i].turnos + "</option>";
    }
    $("#EstructuraEscolar_Turno").html(html);
};

//Manejo de eventos
Empresa.Registrar.manejoDeEventosEscuela = function () {

    if (Empresa.estadoText === "Registrar") {
        $("#NumeroEscuela").prev().hide();
        $("#NumeroEscuela").hide();
    }
    var NivelSeleccionado;
    $("#NivelEducativoId").focus(function () {
        NivelSeleccionado = $("#NivelEducativoId option:selected").val();
    });
    $("#NivelEducativoId").changePatch(function () {
        if ($("#NivelEducativoId option:selected").val() === "2") //Si el nivel educativo seleccionado es PRIMARIA
        {
            $("#CodigoInspeccion,label[for='CodigoInspeccion'] ").show();
        }
        else {
            $("#CodigoInspeccion,label[for='CodigoInspeccion'] ").hide();
        }
        if ($("#listaEstructura").getDataIDs().length > 0) {
            Mensaje.Advertencia.texto = "Si cambia el nivel educativo se perderán los datos de la estructura escolar. ¿Desea seguir de todas formas?";
            Mensaje.Advertencia.botones = true;
            Mensaje.Advertencia.mostrar();
            Mensaje.Advertencia.cancelar = function () {
                $("#divMensajeAdvertencia").hide();
                $("#NivelEducativoId").val(NivelSeleccionado);
            }
            Mensaje.Advertencia.aceptar = function () {
                $("#divMensajeAdvertencia").hide();
                $("#listaEstructura").clearGridData(false);
                $("#listaEstructura").data("listadoCompleto", null);
            }
        } //fin if        
    });

    //cargo el combo de tipo educacion, dependiendo de los de la dir de nivel del usuario logueado
    var tipoEducacion = $("#TipoEducacion option:selected").val();
    $.get($.getUrl("/GestionEmpresa/GetTiposEducacionByDireccionDeNivelUsuario"), {},
            function (data) {
                $("#TipoEducacion").html("");
                $("#TipoEducacion").append("<option value=-1>SELECCIONE</option>");
                for (var i = 0; i < data.length; i++) {
                    if (data[i].TipoEducacion == tipoEducacion) {
                        $("#TipoEducacion").append("<option selected='selected' value=" + data[i].TipoEducacion + ">" + data[i].TipoEducacion + "</option>");

                    } else {
                        $("#TipoEducacion").append("<option value=" + data[i].Id + ">" + data[i].TipoEducacion + "</option>");
                    }
                }
            },
            "json");

    //Funcionalidad del combo em cascada de tipo educacion (padre) y nivel educativo (hijo)
    $("#NivelEducativoId").CascadingDropDown("#TipoEducacion", $.getUrl("/GestionEmpresa/CargarNivelEducativo"), { promptText: 'SELECCIONE', loadingText: "Cargando.." });

    //Si la empresa es ESCUELA ANEXO muestro divBotonSugerirNombreEscuela, y divBtnSeleccionarEscuelaMadre
    if ($('#TipoEmpresa option:selected').text() === "ESCUELA_ANEXO" || $('#TipoEmpresa option:selected').text() === "ESCUELA_MADRE") {

        

        Empresa.Registrar.validacionParametroPadreOrganigramaEmpresaEnEscuela();
        //Evento click del boton sugerir nombre
        Empresa.Registrar.eventoClickSugerirNombre();
        $("#divBotonSugerirNombreEscuela").show();
        if ($('#TipoEmpresa option:selected').text() == "ESCUELA_ANEXO") {
            $("#divBtnSeleccionarEscuelaMadre").show();

            if (Empresa.estadoText !== "Registrar") {
                Empresa.Registrar.editorConsultarMadre.seleccion = $("#EscuelaMadreId").val();
                ConsultarEmpresa.seleccionarEmpresa(Empresa.Registrar.editorConsultarMadre);
                $("#divSeleccionEscuelaMadre").show();
            }
        }
        if ($('#TipoEmpresa option:selected').text() === "ESCUELA_MADRE") {
            //Le oculto el Label y Textbox de numero anexo y le elimino el val-required porque es un atributo de ambas escuela
            $("#NumeroAnexo").prev().hide();
            $("#NumeroAnexo").hide();
            $("#NumeroAnexo").removeClass("val-Required");
        }
    }

    //Si el tipo empresa es ESCUELA MADRE muestro los datos de escuela raiz y su funcionalidad.
    if ($('#TipoEmpresa option:selected').text() == "ESCUELA_MADRE") {

        $("#divEscuelaMadreRaiz").show();

        //oculto el editor de la consulta de empresa raiz
        $("#EsRaiz").changePatch(function () {
            if ($("#EsRaiz").is(":checked")) {
                $("#divSeleccionEscuelaRaiz, #btnEscuelaRaiz, #divBotonSugerirNombreEscuela").hide();
            } else {
                $("#btnEscuelaRaiz, #divBotonSugerirNombreEscuela").show();
            }
        });

        //Muestro/Oculto el editor de consulta escuela raiz de acuerdo al evento click del boton btnEscuelaRaiz
        $("#btnEscuelaRaiz").click(function () {
            $("#divSeleccionEscuelaRaiz").toggle();
        });
    }

    //si no es raiz y esta en editar/ver muestro la escuela raiz    
    if ($("#EsRaiz").is(":unchecked") && Empresa.estadoText !== "Registrar") {
        Empresa.Registrar.editorConsultaEscuelaRaiz.seleccion = $("#EscuelaRaizId").val();
        ConsultarEmpresa.seleccionarEmpresa(Empresa.Registrar.editorConsultaEscuelaRaiz);
        $("#divSeleccionEscuelaRaiz").show();
    }

    if ($("#EmpresaInspeccionId").val() != "" && $("#EmpresaInspeccionId").val() != "0") {
        Empresa.Registrar.editorConsultarEInspeccion.seleccion = $("#EmpresaInspeccionId").val();
        ConsultarEmpresa.seleccionarEmpresa(Empresa.Registrar.editorConsultarEInspeccion);
        $("#divSeleccionEmpresaInspeccion").show();
    }

    //cambio los valores del combo de categoria a numeros y no letras
    var categorias = $("#CategoriaEscuela option[value!='']");
    var val = ["UNO", "DOS", "TRES", "CUATRO"];
    var categoriaSeleccionada = $("#CategoriaEscuela option:selected").val();
    $("#CategoriaEscuela").html("<option value='-1'> SELECCIONE </option>");
    for (var i = 0; i < categorias.length; i++) {
        if (categoriaSeleccionada == val[i]) {
            $("#CategoriaEscuela").append("<option selected='selected' value='" + val[i] + "'>" + (i + 1) + "</option>");
        } else {
            $("#CategoriaEscuela").append("<option value='" + val[i] + "'>" + (i + 1) + "</option>");
        }
    }

    //Evento change del check Privado
    Empresa.Registrar.eventoChangeEscuelaPrivada();

    var divs = "#divContenedorIL";
    var jsonOpciones = {
        "A": null,
        "B": divs,
        "C": divs,
        "D": divs,
        "E": divs,
        "F": divs,
        "G": divs
    };
    $("#ZonaDesfavorableId").comboOpcional(jsonOpciones);
    $("#ZonaDesfavorableId").changePatch();
    //Oculto el div de instrmento legal para zona desfavorable != A para el ver o editar. Se mostrara siempre que se ejecute el change combo.
    if (Empresa.estadoText === "Editar" || Empresa.estadoText === "Ver") {
        $("#divInstrumentoLegalParaZonaDesfaforable").hide();
        $("#divFecNotificacionAsignacionILZD").hide();
    }

    if (Empresa.estadoText === "Registrar") {
        $("#AsignacionInstrumentoLegalZonaDesfavorable_InstrumentoLegal_FechaEmision").mask("99/99/9999");
    }

    //Muestro/Oculto el editor de consulta inspeccion de acuerdo al evento click del boton btnSelecEmpresaInspeccion
    $("#btnSelecEmpresaInspeccion").click(function () {
        $("#divSeleccionEmpresaInspeccion").toggle();
    });

    //Muestro/Oculto el editor de consulta escuela madre de acuerdo al evento click del boton btnEscuelaMadre
    $("#btnEscuelaMadre").click(function () {
        $("#divSeleccionEscuelaMadre").toggle();
    });

    //Evento change del check de registro estructura escolar
    $("#RegistrarEstructuraEscolarCheck").changePatch(function () {
        Empresa.Registrar.cargarComboTurnoEstructuraEscolar();
        if ($("#RegistrarEstructuraEscolarCheck").is(":checked")) {
            $("#grillaEstructura").show();
            $("#divEstructuraDefinitiva").show();
            $("#listaEstructura").setGridWidth(730);
        }
        else {
            $("#divAreaEstructuraEscolar").hide();
            $("#grillaEstructura").hide();
            $("#divEstructuraDefinitiva").hide();
            $("#listaEstructura").setGridWidth(730);
        }
    });


};

//Evento change del check Privado
Empresa.Registrar.eventoChangeEscuelaPrivada = function () {
    $("#Privado").changePatch(function () {
        if ($("#Privado").is(":checked")) {
            $("#divEsPrivado").show();
            var DirectorYRep = null;
            if (Abmc.estadoText === 'Editar' || Abmc.estadoText === 'Ver') {
                $.ajax({
                    type: 'GET',
                    url: $.getUrl("/GestionEmpresa/GetDatosEscuelaPrivada?empresaId=" + $("#Id").val()),
                    data: null,
                    async: false,
                    success: function (data) {
                        if (data != null) {
                            DirectorYRep = data;
                        }
                    },
                    dataType: 'json'
                });
            }

            //Cargo el Director en el div de director
            PersonaFisica.cargarPersonaById("#divDirector", "Director", Abmc.estadoText, null);
            $("#divDirector").one("ajaxStop", function () {
                $("#Director_divConsultaPF legend").html("Buscar Director");
                $("#Director_divFormularioPF legend").html("Director");
                //se oculta el boton de  nuevo agente, ya que no se puede hacer desde empresa
                /*$("#RepresentanteLegal_btnNuevoPF").hide();
                $("#RepresentanteLegal_btnEditarPF").hide();
                $("#Director_btnEditarPF").hide();
                $("#Director_btnNuevoPF").hide();
                
                $("#RepresentanteLegal_btnLimpiarPF").val("Volvar a buscar");
                $("#Director_btnLimpiarPF").val("Volvar a buscar");
                */
                if (DirectorYRep) {
                    PersonaFisica.cargarPersonaFisica($('#divDirector').data('persona'), DirectorYRep.director);
                }
                //Pongo el div de persona fisica (Director en modo búsqueda)
                if (Abmc.estadoText === "Registrar") {
                    $("#Director_btnLimpiarPF").click();
                }
            });

            //Cargo el RepresentanteLegal en el div de representante
            PersonaFisica.cargarPersonaById("#divRepresentanteLegal", "RepresentanteLegal", Abmc.estadoText, null);
            $("#divRepresentanteLegal").one("ajaxStop", function () {
                $("#RepresentanteLegal_divConsultaPF legend").html("Buscar Representante Legal");
                $("#RepresentanteLegal_divFormularioPF legend").html("Representante Legal");
                if (DirectorYRep) {
                    PersonaFisica.cargarPersonaFisica($('#divRepresentanteLegal').data('persona'), DirectorYRep.representante);
                }
                //Pongo el div de persona fisica (Director en modo búsqueda)
                if (Abmc.estadoText === "Registrar") {
                    $("#RepresentanteLegal_btnLimpiarPF").click();
                }
            });

        }
        else {
            $("#divEsPrivado").hide();
            $('#divDirector').html("");
            $('#divRepresentanteLegal').html("");
        }
    });

    $("#Privado").changePatch();
};

//Evento click del boton sugerir nombre
Empresa.Registrar.eventoClickSugerirNombre = function () {
    $('#btnNombreSugerido').click(function () {        
        var model = {};
        if (Empresa.Registrar.editorConsultaEscuelaRaiz) {
            if (Empresa.Registrar.editorConsultaEscuelaRaiz.seleccion) {
                $('#EscuelaRaizId').val(Empresa.Registrar.editorConsultaEscuelaRaiz.seleccion);
            }
        }
        if (Empresa.Registrar.editorConsultarMadre) {
            if (Empresa.Registrar.editorConsultarMadre.seleccion) {
                $('#EscuelaMadreId').val(Empresa.Registrar.editorConsultarMadre.seleccion);
            }
        }

        var idDomicilio = GrillaUtil.getSeleccionFilas($(Empresa.Registrar.instanciaVinculo.prefijo + "listDomicilio"), false);
        if (!idDomicilio) {
            Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Para sugerir nombre, se requiere que se seleccione un domicilio. ");
            return;
        }
        if ($("#TipoEscuela").val() == "") {
            Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Para sugerir nombre, se requiere que se seleccione el tipo de escuela.");
            return;
        }

        if ($("#TipoEmpresa").val() == "ESCUELA_ANEXO" && ($('#EscuelaMadreId').val() == "0" || $('#EscuelaMadreId').val() == undefined)) {
            Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Para sugerir nombre, se requiere que se seleccione la escuela madre.");
            return;
        }

        if ($("#TipoEmpresa").val() == "ESCUELA_MADRE" && !$("#EsRaiz").is(":checked") && ($('#EscuelaRaizId').val() == "0" || $('#EscuelaRaizId').val() == undefined)) {
            Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones("Para sugerir nombre, se requiere que se seleccione la escuela raiz. ");
            return;
        }

        var url = $.getUrl("/GestionEmpresa/SugerirNombre?domicilioId=" + idDomicilio + "&tipoEscuelaId=" + $("#TipoEscuela").val() + "&escuelaRaizId=" + $('#EscuelaRaizId').val() + "&escuelaMadreId=" + $('#EscuelaMadreId').val() + "&tipoEmpresa=" + $("#TipoEmpresa").val() + "&numeroEscuelaAnexo=" + $("#NumeroAnexo").val());
        $.post(url, {},
            function (data) {
                if (data.status) {
                    $('#Nombre').val(data.NombreEscuela);
                }
                else {
                    Mensaje.Error.limpiar();
                    Mensaje.Error.texto = "Error intentando sugerir nombre";
                    for (var i = 0; i < data.details.length; i++) {
                        Mensaje.Error.agregarError(data.details[i]);
                    }

                }
            }, 'json');
    });
};

//Metodo para mostrar el mensaje de advertencia
Empresa.Registrar.mensajeDeAdvertenciaDeValidaciones = function (mensaje) {
    Mensaje.Advertencia.texto = mensaje;
    Mensaje.Advertencia.botones = false;
    Mensaje.Advertencia.mostrar();
};

/**************************************************** FIN AREA ESCUELA  ****************************************************/



/**************************************************** AREA INSPECCION ****************************************************/
Empresa.Registrar.cargarGrillaEmpresasInspeccionadasEnInspeccion = function () {
    var grilla = $("#listEmpresasInspeccionadas").jqGrid({
        //url: $.getUrl("/GestionEmpresa/GetEmpresasInspeccionadasPorInspeccionId/?idInspeccion=" + $("#Id").val()),
        datatype: 'local',
        colNames: ["id", "Código empresa", "Nombre empresa", "Tipo empresa", "Estado empresa", "Nivel educativo", "Localidad", "Departamento", "Estado de asignacion", "Tipo inspección"],
        colModel: [
                { key: true, name: "id", index: "id", align: "left", hidden: true },
                { key: false, name: "CodigoEmpresa", index: "CodigoEmpresa" },
                { key: false, name: "NombreEmpresa", index: "NombreEmpresa" },
                { key: false, name: "TipoEmpresa", index: "TipoEmpresa" },
                { key: false, name: "EstadoEmpresa", index: "EstadoEmpresa" },
                { key: false, name: "NivelEducativo", index: "NivelEducativo" },
                { key: false, name: "Localidad", index: "Localidad" },
                { key: false, name: "Departamento", index: "Departamento" },
                { key: false, name: "EstadoAsignacion", index: "EstadoDeAsignacion" },
                { key: false, name: "TipoInspeccion", index: "TipoInspeccion" }
            ],
        pager: "#pagerEmpresasInspeccionadas",
        rowNum: 10,
        caption: "Empresas inspeccionadas",
        rowList: [5, 10, 20, 50],
        viewrecords: true,
        autowidth: true,
        sortorder: "asc",
        sortname: "",
        width: 730,
        height: "100%"

    });

    if ($("#TipoInspeccionEnum").val() != "ZONAL") {
        $("#listEmpresasInspeccionadas").hideCol("EstadoAsignacion");
        $("#listEmpresasInspeccionadas").trigger("reloadGrid");
    } else {
        $("#listEmpresasInspeccionadas").hideCol("TipoInspeccion");
        $("#listEmpresasInspeccionadas").trigger("reloadGrid");
    };

    $.get($.getUrl("/GestionEmpresa/GetEmpresasInspeccionadasPorInspeccionId"), { idInspeccion: $("#Id").val() }, function (datos) {
        for (var i = 0; i < datos.length; i++) {
            $("#listEmpresasInspeccionadas").addRowData(i, datos[i], "last");

            if ($("#listEmpresasInspeccionadas").getDataIDs().length > $("#listEmpresasInspeccionadas").getGridParam("rowNum")) {
                $("#listEmpresasInspeccionadas").trigger("reloadGrid");
            }

        };

    });

    $("#listEmpresasInspeccionadas").setGridWidth(730);
}
Empresa.Registrar.empresaInspeccion = function () {
    //seteo para el modificar / ver el tipo de inspeccion
    if (Empresa.Registrar.Inspeccion.TipoInspeccion != "") {
        $("#TipoInspeccion").val(Empresa.Registrar.Inspeccion.TipoInspeccion);
    }

    //Manejo de eventos empresa inspeccion
    Empresa.Registrar.manejoDeEventosInspeccion();

    if ($('#TipoInspeccion option').length > 3) {
        $('#divTipoInspeccionLista').show();
        $('#divTipoInspeccionEnum').hide();

    } else {
        $('#divTipoInspeccionLista').hide();
        $('#divTipoInspeccionEnum').show();
    }

    if (Empresa.estadoText != "Registrar") {
        Empresa.Registrar.cargarGrillaEmpresasInspeccionadasEnInspeccion();

        ConsultarEmpresa.setFiltroEmpresaId(Empresa.Registrar.editorConsultarEmpresaSupervisora, $("#Id").val());

        if ($("#TipoInspeccionEnum :selected").text() == "OTRA" && $("#TipoInspeccion option:selected").val()=="") {
            $("#divTipoInspeccionLista").hide();
            $("#divTipoInspeccionEnum").show();
        }
    }

};

//Manejo de eventos de Inspeccion
Empresa.Registrar.manejoDeEventosInspeccion = function () {
    //si ya esta seleccionado que me lo seleccione xq no lo hace por defecto
    $("#TipoInspeccion option[selected='selected']").attr("selected", true);

    if ($("#EmpresaInspeccionSupervisoraId").val() != "" && $("#EmpresaInspeccionSupervisoraId").val() != "0") {
        Empresa.Registrar.editorConsultarEmpresaSupervisora.seleccion = $("#EmpresaInspeccionSupervisoraId").val();
        ConsultarEmpresa.seleccionarEmpresa(Empresa.Registrar.editorConsultarEmpresaSupervisora);
        $("#divSeleccionEmpresaSupervisora").show();
    }
    if (Empresa.estadoText == "Editar" && $("#parametroJerarquiaDeInspeccionIgualAOrganigrama").val() == "True") {
        $("#buscarEmpresaPadre_btnVolver").hide();
    }


    Empresa.Registrar.editorConsultarEmpresaSupervisora.vista = "CualquierInspeccionNoZonalPertenecienteADireccionDeNivelDelUsuarioLogueado";

    var changePatch = function () {
        if ($('#TipoEmpresa').val() == "INSPECCION") {
            Empresa.Registrar.editorConsultarPadre.vista = Empresa.Registrar.getNuevaVistaEnum();

            //si el combo es seleccione, oculto la tab de Empresa Padre
            if ($("#" + this.id + " option:selected").text() === "SELECCIONE") {
                $("#liSolapa4, #divSolapa4").hide();
            }
            else {
                $("#liSolapa4, #divSolapa4").show();
            }

            //this.id porq depende si es el TipoInspeccion o TipoInspeccionEnum
            if ($("#" + this.id + " option:selected").text() !== "GENERAL" && $("#" + this.id + " option:selected").text() !== "SELECCIONE") {
                $("#btnEmpresaSupervisora").show();
                $("#BuscarInspeccionesZonal_divFiltroBasico").show();
                //aca valido el tema del parametro jerarquia organigrama
                var parametro = $("#parametroJerarquiaDeInspeccionIgualAOrganigrama").val();
                if (parametro == "True") {
                    $("#liSolapa4").hide();
                }
                
                if ($("#" + this.id + " option:selected").text() == "ZONAL") {
                    $("#BuscarInspeccionesZonal_fltTipoInspeccion").html('<option value="">SELECCIONE</option><option>GENERAL</option> <option>OTRA</option>');
                }
                $(Empresa.Registrar.editorConsultarPadre.prefijo + "fltTipoEmpresa").html("<option>SELECCIONE</option><option>INSPECCION</option>");
            }
            else {
                $(Empresa.Registrar.editorConsultarPadre.prefijo + "fltTipoEmpresa").html("<option>SELECCIONE</option><option>DIRECCION_DE_NIVEL</option>");
                $("#btnEmpresaSupervisora").hide();
                $("#BuscarInspeccionesZonal_divFiltroBasico").hide();
                //
                $("#liSolapa4").show();
            }
        }
    }

    //si tiene varios inspecciones intermedias la direccion de nivel del usario logeado 

    $('#TipoInspeccion').changePatch(changePatch);
    $('#TipoInspeccionEnum').changePatch(changePatch);


    //cosas q se ocultan
    if (Empresa.estadoText == "Ver" || Empresa.estadoText == "Editar") {
        $("#liSolapa4, #divSolapa4").show();

        if (Empresa.estadoText == "Ver") {
            $("#btnEmpresaSupervisora, #BuscarInspeccionesZonal_btnVolver").hide();
        }
    } else {
        $("#liSolapa4, #divSolapa4").hide();
    }
};

/**************************************************** FIN AREA INSPECCION ****************************************************/

//Redefinicion del metodo preEnviarModelo
Empresa.Registrar.preEnviarModelo = Abmc.preEnviarModelo;
Abmc.preEnviarModelo = function (datos) {

    var model = {};
    
    if (VinculoEmpresaEdificio.seleccionNuevoDomicilio) {
        if ( ($(Empresa.Registrar.instanciaVinculo.prefijo + "comboCalles").val() == "" || $(Empresa.Registrar.instanciaVinculo.prefijo + "AlturaNueva").val() == "" ) && !$(Empresa.Registrar.instanciaVinculo.prefijo + "divDatosGeneralesDomicilio").is(":visible")) {
            //si selecciono un nuevo domicilio y no le cargo
            
            for (var i = 0; i < datos.length; i++) {
                if (datos[i].name == "DomicilioId") {
                    delete datos[i].value;
                    break;
                }
            }
        }
        else {
            model.DomicilioId = GrillaUtil.getSeleccionFilas($(Empresa.Registrar.instanciaVinculo.prefijo + "listDomicilio"), false) || $("#DomicilioId").val();
            $("#DomicilioId").val(model.DomicilioId);
            model.CalleNuevoDomicilio = $(Empresa.Registrar.instanciaVinculo.prefijo + "comboCalles").val();
            $(Empresa.Registrar.instanciaVinculo.prefijo + "comboCalles").val(model.CalleNuevoDomicilio);
            model.AlturaNuevoDomicilio = $(Empresa.Registrar.instanciaVinculo.prefijo + "AlturaNueva").val();
            $(Empresa.Registrar.instanciaVinculo.prefijo + "AlturaNueva").val(model.AlturaNuevoDomicilio);
            
        }
    }
    else {
        model.DomicilioId = GrillaUtil.getSeleccionFilas($(Empresa.Registrar.instanciaVinculo.prefijo + "listDomicilio"), false) || $("#DomicilioId").val();
    }

    //Levanto todos los vínculos de la grilla para insertarlos en el model
    var datosVinculos = $(Empresa.Registrar.instanciaVinculo.prefijo + "listVinculos").getRowData();
    model.VinculoEmpresaEdificio = [];
    for (var i = 0; i < datosVinculos.length; i++) {
        model.VinculoEmpresaEdificio[i] = {};
        model.VinculoEmpresaEdificio[i].EdificioId = datosVinculos[i].IdEdificio;
        model.VinculoEmpresaEdificio[i].Observacion = datosVinculos[i].Observacion;
        model.VinculoEmpresaEdificio[i].FechaDesde = datosVinculos[i].FechaDesde;
    }

    //Si se selecciono la empresa padre
    if (Empresa.Registrar.editorConsultarPadre && Empresa.Registrar.editorConsultarPadre.seleccion) {
        model.EmpresaPadreOrganigramaId = Empresa.Registrar.editorConsultarPadre.seleccion;
    };

    $.formatoModelBinder(model, datos, "");

    switch ($("#TipoEmpresa").val()) {
        case "DIRECCION_DE_NIVEL":
            Empresa.Registrar.preEnviarModeloDireccionDeNivel(datos);
            break;
        case "ESCUELA_MADRE":
        case "ESCUELA_ANEXO":
            Empresa.Registrar.preEnviarModeloEscuela(datos);
            break;
        case "INSPECCION":
            Empresa.Registrar.preEnviarModeloInspeccion(datos);
            break;
        default:
            break;
    }
};

//preEnviarModelo para Direccion de nivel
Empresa.Registrar.preEnviarModeloDireccionDeNivel = function (datos) {
    //Bideo los atributos Nivel Educativo y Tipo Educacion con el model
   
    var NETETemporal = $("#listNETE").getRowData();
    var NETE = [];
    for (var i = 0; i < NETETemporal.length; i++) {
        var nete = NETETemporal[i];
        NETE[i] = {};
        NETE[i].NivelEducativo = {};
        NETE[i].NivelEducativo.Id = nete.idNE;
        NETE[i].NivelEducativo.Nombre = nete.nivelEducativo;
        NETE[i].TipoEducacion = nete.tipoEducacion;
    }
    if (NETETemporal != null) {
        $.formatoModelBinder(NETE, datos, "NivelEducativoPorTipoEducacion");
    }

    //Bideo los atributos Tipo Escuela con el model
    var TETemporal = $("#listTE").getRowData();
    var TE = [];
    for (var i = 0; i < TETemporal.length; i++) {
        var te = TETemporal[i];
        TE[i] = {};
        TE[i].Id = te.idTE;
        TE[i].Nombre = te.tipoEscuela;
    }
    if (TETemporal != null) {
        $.formatoModelBinder(TE, datos, "TiposEscuelas");
    }
};




//pre enviar modelo de una Escuela
Empresa.Registrar.preEnviarModeloEscuela = function (datos) {
    var model = {};
    if ($('#TipoEmpresa option:selected').text() == "ESCUELA_MADRE") {
        //Si se selecciono la escuela raiz
        if (Empresa.Registrar.editorConsultaEscuelaRaiz && Empresa.Registrar.editorConsultaEscuelaRaiz.seleccion) {
            model.EscuelaRaizId = Empresa.Registrar.editorConsultaEscuelaRaiz.seleccion;
        }
    }
    if ($('#TipoEmpresa option:selected').text() == "ESCUELA_ANEXO") {
        //Si se selecciono la escuela madre
        if (Empresa.Registrar.editorConsultarMadre && Empresa.Registrar.editorConsultarMadre.seleccion) {
            model.EscuelaMadreId = Empresa.Registrar.editorConsultarMadre.seleccion;
        }
        //exprecion regular para q valide q solo ingresa numeros        
    }

    //Si se selecciono la empresa inspeccion
    if (Empresa.Registrar.editorConsultarEInspeccion && Empresa.Registrar.editorConsultarEInspeccion.seleccion) {
        model.EmpresaInspeccionId = Empresa.Registrar.editorConsultarEInspeccion.seleccion;
    }

    if ($("#Privado").is(":checked") && $("#Director_Id").val() != "null" && $("#RepresentanteLegal_Id").val()!="null") {
        //Director
        model.Director = {};
        model.Director.Id = $("#Director_Id").val();

        //Representante Legal
        model.RepresentanteLegal = {};
        model.RepresentanteLegal.Id = $("#RepresentanteLegal_Id").val();
    }

    $.formatoModelBinder(model, datos, "");

    //Bindeo los turnos seleccionados con el model
    var turnos = $("#listTurnos").getRowData();
    var T = [];
    for (var i = 0; i < turnos.length; i++) {
        var turno = turnos[i];
        T[i] = {};
        T[i].Id = turno.idTurnos;
        T[i].Nombre = turno.turnos;
    }
    if (turnos != null) {
        $.formatoModelBinder(T, datos, "Turnos");
    }

    //Bindeo los periodos lectivos seleccionados con el model
    var periodosLectivos = $("#listPeriodosLectivos").getRowData();
    var P = [];
    for (var i = 0; i < periodosLectivos.length; i++) {
        var periodo = periodosLectivos[i];
        P[i] = {};
        P[i].Id = periodo.PeriodoLectivoId;
        P[i].Nombre = periodo.PeriodoLectivoText;
    }
    if (periodosLectivos != null) {
        $.formatoModelBinder(P, datos, "PeriodosLectivos");
    }

    //Levanto todos los datos de estructura escuela de la grilla para insertarlos en el model
    var datosEstructura = $("#listaEstructura").data("listadoCompleto");
    var EE = [];
    if (datosEstructura) {
        for (var i = 0; i < datosEstructura.length; i++) {
            EE[i] = {};
            EE[i].Id = datosEstructura[i].Id;
            //        if (ReactivacionEmpresa.EscuelaEsSuperior) {
            //            EstructuraEscolar[i].CarreraNombre = datosEstructura[i].Carrera;
            //        }
            EE[i].Turno = datosEstructura[i].Turno;
            EE[i].GradoAnio = datosEstructura[i].GradoAnio;
            EE[i].Division = datosEstructura[i].Division;
            EE[i].Cupo = datosEstructura[i].Cupo;
            EE[i].FechaApertura = datosEstructura[i].FechaApertura;
            EE[i].Estado = 1; //Seteo el estado de pecho
            EE[i].Escuela = $("#Id").val();
        }
        $.formatoModelBinder(EE, datos, "EstructuraEscolar");
    }    
};

//pre enviar modelo de una Inspeccion
Empresa.Registrar.preEnviarModeloInspeccion = function (datos) {
    var model = {};
    model.EmpresaInspeccionSupervisoraId = $("#EmpresaInspeccionSupervisoraId").val();
    $.formatoModelBinder(model, datos, "EmpresaInspeccionSupervisoraId");
};

//post enviar modelo:
//oculta el formulario y muestra la consulta actualizando la grilla
Abmc.postEnviarModelo = function (data) {

    if (data.status) {
        //unicamente para el registrar empresa
        if (Empresa.estadoText === "Registrar") {
            $(ConsultarEmpresa.divVista).hide();
            $("#divConsulta").show();
        }
    }
};

Empresa.Registrar.cargarTipoGestion = function () {
    var tipoEmpresa = $("#TipoEmpresa").val();
    switch (tipoEmpresa) {
        case "DIRECCION_DE_NIVEL":
        case "INSPECCION":
            $("#TipoGestion").val("GESTION_EDUCATIVA");
            break;
        case "ESCUELA_MADRE":
        case "ESCUELA_ANEXO":
            $("#TipoGestion").val("ESCUELA");
            break;
        default:
            $("#TipoGestion").val("GESTION_ADMINISTRATIVA");
            break;
    }
};

Empresa.Registrar.datosEnModoSoloLectura = function () {
    var tipoGestion = $("#TipoGestion").val();
    var tipoEmpresa = $("#TipoEmpresa").val();
    //campos q siempre se deshabilitan

    $("#VincularEdificioCheck").attr("disabled","disabled");
    $("#CodigoEmpresa").attr("disabled", "disabled");
    switch (tipoGestion) {
        case "ESCUELA":
            $("#NivelEducativoId, #Privado, ").attr("disabled", "disabled");
            
            break;
        default:
            break;
    }
    switch (tipoEmpresa) {
        case "ESCUELA_MADRE":
            if ($("#EstructuraDefinitiva").is(":checked")) {
                $("#EstructuraDefinitiva").attr("disabled", "disabled");
            }
            break;
        case "INSPECCION":
            $("#TipoInspeccion").attr("disabled", "disabled");
            $("#TipoInspeccionEnum").attr("disabled", "disabled");
    }
};


//Muestro mensaje de confirmación
Empresa.Registrar.enviarModelo = Abmc.enviarModelo;
Abmc.enviarModelo = function () {
    if (!Empresa.Registrar.validacionesPreEnviarModelo()) {
        Mensaje.Error.mostrar();
        return;
    }
    Mensaje.Advertencia.texto = "¿Desea continuar con la operación?";
    Mensaje.Advertencia.botones = true;
    Mensaje.Advertencia.mostrar();
    Mensaje.Advertencia.cancelar = function () {
        $("#divMensajeAdvertencia").hide();
    };
    Mensaje.Advertencia.aceptar = function () {
        Empresa.Registrar.enviarModelo();
    };
};

//Validaciones para el preEnviarModelo, retorna true si esta todo bien, y false, si hay algun error.
Empresa.Registrar.validacionesPreEnviarModelo = function () {
    var bandera = true;

    //TODO: Aca se podrían validar los datos requeridos de todo empresa.

    //Validaciones Director y Representante Legal    
    var mensajeDirector = Empresa.Registrar.validarPersonaFisica("divDirector", "Director");
    var mensajeReprecentanteLegal = Empresa.Registrar.validarPersonaFisica("divRepresentanteLegal", "Representante legal");
    Mensaje.Error.texto = mensajeDirector + mensajeReprecentanteLegal;
    delete mensajeDirector;
    delete mensajeReprecentanteLegal;

    if (mensajeDirector != "" || mensajeReprecentanteLegal != "") {
        bandera = false;
    }
    return bandera;
};


//Retorna un mensaje con los campos requeridos.
Empresa.Registrar.validarPersonaFisica = function (div, persona) {
    var requeridos = $("#" + div + " .val-Required");
    var mensajeSalida = "";    
    for (var i = 0; i < requeridos.length; i++) {
        //si no tiene valor
        if (requeridos[i].value == "") {
            mensajeSalida += "Faltan datos para " + persona + ". Verifique los campos requeridos (*). </br>";
            break;
        }
    }
    return mensajeSalida;
};