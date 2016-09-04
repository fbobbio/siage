
/************************************************************** VARIABLES ***********************************************************/ 

var ConsultarEmpresa = {};
ConsultarEmpresa.vista = "";
ConsultarEmpresa.Grilla = {};
ConsultarEmpresa.Grilla.id                  = "#list";
ConsultarEmpresa.Grilla.botones             = ['Ver', 'Seleccionar', 'Editar', 'Agregar'];
ConsultarEmpresa.Grilla.idSeleccionado      = 0;
ConsultarEmpresa.Grilla.accionListar        = "/GestionEmpresa/ProcesarBusquedaBasico";
ConsultarEmpresa.Grilla.accionVer           = "Ver";
ConsultarEmpresa.Grilla.accionEliminar      = "Eliminar";
ConsultarEmpresa.Grilla.accionEditar        = "Editar";
ConsultarEmpresa.Grilla.accionRegistrar     = "Registrar";
ConsultarEmpresa.Grilla.accionReactivar     = "Reactivar";
ConsultarEmpresa.Grilla.accionSeleccionar   = "Seleccionar";

/************************************************************* FORMULARIO ***********************************************************/

ConsultarEmpresa.init = function (nombreVista) {
    ConsultarEmpresa.vista = nombreVista;
    ConsultarEmpresa.visualizarDivs();
    ConsultarEmpresa.Grilla.accionListar += "?vista=" + nombreVista;
    ConsultarEmpresa.Grilla.init();

    $("#btnConsultarBasico, #btnConsultarAvanzado").click(function () {
        ConsultarEmpresa.Grilla.actualizar();
        $(document).ajaxStop(ConsultarEmpresa.Grilla.mostrarBotones);
    });

    /*
    Le doy funcionalidad a los botones Limpiar basico y Avanzado
    */
    $("#btnLimpiarBasico").click(function () {
        ConsultarEmpresa.Grilla.accionListar = "/GestionEmpresa/ProcesarBusquedaBasico" + "?vista=" + nombreVista;
        ConsultarEmpresa.limpiar();
        ConsultarEmpresa.Grilla.limpiar();
    });
    $("#btnLimpiarAvanzado").click(function () {
        if (ConsultarEmpresa.esEscuela())
            ConsultarEmpresa.Grilla.accionListar = "/GestionEmpresa/ProcesarBusquedaAvanzadaEscuela" + "?vista=" + nombreVista;
        else
            ConsultarEmpresa.Grilla.accionListar = "/GestionEmpresa/ProcesarBusquedaAvanzadaEmpresa" + "?vista=" + nombreVista;
        ConsultarEmpresa.limpiar();
        ConsultarEmpresa.Grilla.limpiar();
    });

    /*
    Cuando estamos en el busqueda basico, al hacer clic en busqueda avanzada
    me oculta el boton consultar, y hasta q no se seleccione un tipo de empresa
    no aparece el boton consultar
    */
    $("#btnBusquedaAvanzada").click(function () {
        $("#divFiltroAvanzado").show();
        $("#divFiltroBasico").hide();
        $("#btnConsultarAvanzado").hide();
    });

    /*
    Muestra y oculta divs de acuerdo a si el tipo empresa es escuela o no.
    Si la empresa es una direccion de nivel, me muestra los combos de nivel educativo y 
    direccion de nivel. Si es otra empresa, los oculta, y le seteo el valor del combo a -1.
    */
    $("#FiltroTipoEmpresa").changePatch(function () {
        if (ConsultarEmpresa.esEscuela()) {
            $("#divFiltroEmpresa").hide();
            $("#divFiltroEscuela").show();
            ConsultarEmpresa.Grilla.accionListar = "/GestionEmpresa/ProcesarBusquedaAvanzadaEscuela";
        }
        else {
            $("#divFiltroEscuela").hide();
            $("#divFiltroEmpresa").show();
            if (ConsultarEmpresa.esDireccionNivel()) {
                $("#divTipoEducacionNivelEducativo").show();
            }
            else {
                $("#FiltroTipoEducacionEmpresa").val(-1);
                $("#FiltroNivelEducativoEmpresa").val(-1);
                $("#divTipoEducacionNivelEducativo").hide();
            }
            ConsultarEmpresa.Grilla.accionListar = "/GestionEmpresa/ProcesarBusquedaAvanzadaEmpresa";
        }
        ConsultarEmpresa.Grilla.accionListar += "?filtroTipoEmpresa=" + $("#FiltroTipoEmpresa").val() + "&vista=" + nombreVista;
        ConsultarEmpresa.Grilla.limpiarUrl();
        $("#btnConsultarAvanzado").show();
    });

    $("#btnBusquedaBasica").click(function () {
        $("#divFiltroAvanzado").hide();
        $("#divFiltroBasico").show();
        ConsultarEmpresa.Grilla.accionListar = "/GestionEmpresa/ProcesarBusquedaBasico" + "?vista=" + nombreVista;
        ConsultarEmpresa.Grilla.limpiarUrl();
    });

    /*
    Paso por url, el valor de los parametros ingresados en la busqueda al metodo ProcesarBusquedaBasico
    */
    $("#divFiltroBasico :input").changePatch(function () {
        ConsultarEmpresa.Grilla.accionListar = "/GestionEmpresa/ProcesarBusquedaBasico";
        var url = "filtroNombreEmpresa=" + $("#FiltroNombreEmpresa").val() +
            "&filtroCUE=" + $("#FiltroCUE").val() +
            "&filtroCodigoEmpresa=" + $("#FiltroCodigoEmpresa").val() +
            "&filtroDepartamentoProvincialBasico=" + $("#FiltroDepartamentoProvincialBasico").val() +
            "&filtroLocalidadBasico=" + $("#FiltroLocalidadBasico").val() +
            "&filtroBarrioBasico=" + $("#FiltroBarrioBasico").val() +
            "&filtroCalleBasico=" + $("#FiltroCalleBasico").val() +
            "&filtroAlturaBasico=" + $("#FiltroAlturaBasico").val() +
            "&vista=" + nombreVista;
        ConsultarEmpresa.Grilla.parametrosUrl(url);
    });

    /*
    Paso por url, el valor de los parametros ingresados en la busqueda al metodo ProcesarBusquedaAvanzadaEmpresa
    */
    $("#divFiltroEmpresa :input").changePatch(function () {
        ConsultarEmpresa.Grilla.accionListar = "/GestionEmpresa/ProcesarBusquedaAvanzadaEmpresa";
        var url = "filtroTipoEmpresa=" + $("#FiltroTipoEmpresa").val() +
            "&filtroFechaDesde=" + $("#FiltroFechaDesdeEmpresa").val() +
            "&filtroFechaHasta=" + $("#FiltroFechaHastaEmpresa").val() +
            "&filtroFechaDesdeInicioActividad=" + $("#FiltroFechaDesdeInicioActividad").val() +
            "&filtroFechaHastaInicioActividad=" + $("#FiltroFechaHastaInicioActividad").val() +
            "&filtroProgramaPresupuestarioEmpresa=" + $("#FiltroProgramaPresupuestarioEmpresa").val() +
            "&filtroEstadoEmpresa=" + $("#FiltroEstadoEmpresa").val() +
            "&filtroTipoEducacionEmpresa=" + $("#FiltroTipoEducacionEmpresa").val() +
            "&filtroNivelEducativoEmpresa=" + $("#FiltroNivelEducativoEmpresa").val() +

            "&filtroDepartamentoProvincialEmpresa=" + $("#FiltroDepartamentoProvincialEmpresa").val() +
            "&filtroLocalidadEmpresa=" + $("#FiltroLocalidadEmpresa").val() +
            "&filtroBarrioEmpresa=" + $("#FiltroBarrioEmpresa").val() +
            "&filtroCalleEmpresa=" + $("#FiltroCalleEmpresa").val() +
            "&filtroAlturaEmpresa=" + $("#FiltroAlturaEmpresa").val() +
            "&vista=" + nombreVista;

        ConsultarEmpresa.Grilla.parametrosUrl(url);
    });

    /*
    Paso por url, el valor de los parametros ingresados en la busqueda al metodo ProcesarBusquedaAvanzadaEscuela
    */
    $("#divFiltroEscuela :input").changePatch(function () {
        ConsultarEmpresa.Grilla.accionListar = "/GestionEmpresa/ProcesarBusquedaAvanzadaEscuela";
        var url = "filtroTipoEmpresa=" + $("#FiltroTipoEmpresa").val() +
            "&filtroNumeroEscuela=" + $("#FiltroNumeroEscuela").val() +
            "&filtroFechaDesdeEscuela=" + $("#FiltroFechaDesdeEscuela").val() +
            "&filtroFechaHastaEscuela=" + $("#FiltroFechaHastaEscuela").val() +
            "&filtroFechaDesdeInicioActividadEscuela=" + $("#FiltroFechaDesdeInicioActividadEscuela").val() +
            "&filtroFechaHastaInicioActividadEscuela=" + $("#FiltroFechaHastaInicioActividadEscuela").val() +
            "&filtroTipoEscuelaEnum=" + $("#FiltroTipoEscuela").val() +
            "&filtroProgramaPresupuestarioEscuela=" + $("#FiltroProgramaPresupuestarioEscuela").val() +
            "&filtroCategoriaEscuelaEnum=" + $("#FiltroTipoCategoria").val() +
            "&filtroTipoEducacionEnum=" + $("#FiltroTipoEducacion").val() +
            "&filtroNivelEducativoEnum=" + $("#FiltroNivelEducativo").val() +
            "&filtroDependenciaEnum=" + $("#FiltroDependencia").val() +
            "&filtroAmbitoEscuelaEnum=" + $("#FiltroAmbito").val() +
            "&filtroEsReligioso=" + $("#FiltroReligioso").attr("checked") +
            "&filtroEsArancelado=" + $("#FiltroArancelado").attr("checked") +
            "&filtroTipoInspeccionEnum=" + $("#FiltroTipoInspeccion").val() +
            "&filtroEstadoEmpresa=" + $("#FiltroEstadoEmpresaEscuela").val() +
            "&filtroObraSocial=" + $("#FiltroObraSocial").val() +
            "&filtroPeriodoLectivo=" + $("#FiltroPeriodoLectivo").val() +
            "&filtroTurno=" + $("#FiltroTurno").val() +

            "&filtroDepartamentoProvincialEscuela=" + $("#FiltroDepartamentoProvincialEscuela").val() +
            "&filtroLocalidadEscuela=" + $("#FiltroLocalidadEscuela").val() +
            "&filtroBarrioEscuela=" + $("#FiltroBarrioEscuela").val() +
            "&filtroCalleEscuela=" + $("#FiltroCalleEscuela").val() +
            "&filtroAlturaEscuela=" + $("#FiltroAlturaEscuela").val() +
            "&vista=" + nombreVista;

        ConsultarEmpresa.Grilla.parametrosUrl(url);
    });

    /*
    Pregunto si la escuela es privada o no, para poder filtrar por la obra social
    */
    $("#Privada").changePatch(function () {
        if ($("#Privada").is(":checked"))
            $("#EsPrivada").show();
        else {
            $("#FiltroObraSocial").val(-1);
            $("#EsPrivada").hide();
        }
    });
};

/*
Pregunto si la empresa seleccionada en el combo de tipo empresa es una escuela (madre o anexo)
*/
ConsultarEmpresa.esEscuela = function () {
    var escuelaMadre = "ESCUELA_MADRE";
    var escuelaAnexo = "ESCUELA_ANEXO";
    var comboTipoEmpresa = $("#FiltroTipoEmpresa").val();

    return (comboTipoEmpresa == escuelaMadre || comboTipoEmpresa == escuelaAnexo);
};

/*
Pregunto si la empresa seleccionada en el combo de tipo empresa es una empresa de tipo dirección de nivel
*/
ConsultarEmpresa.esDireccionNivel = function () {
    var empresaDireccionNivel = "DIRECCION_DE_NIVEL";
    var comboTipoEmpresa = $("#FiltroTipoEmpresa").val();
    return (comboTipoEmpresa == empresaDireccionNivel);
};

/*
Limpia todo el formulario, oculta los divs y el boton consultar avanzado.
*/
ConsultarEmpresa.limpiar = function () {
    ConsultarEmpresa.Grilla.limpiarUrl();

    $("#divFiltrosDeBusqueda fieldset :input").each(function () {
        var type = this.type;
        var tag = this.tagName.toLowerCase();

        if (type == "text" || type == "password" || type == "hidden" || tag == "textarea")
            this.value = "";
        else if (type == "checkbox" || type == "radio")
            this.checked = false;
        else if (tag == "select")
            this.selectedIndex = 0;
    });
    $("#divFiltroEscuela").hide();
    $("#divFiltroEmpresa").hide();
    $("#btnConsultarAvanzado").hide();
    $("#EsPrivada").hide();

};

/*
Selecciono una empresa, y muestro el valor de los datos generales de la empresa.
*/
ConsultarEmpresa.seleccionarEmpresa = function (idEmpresa) {
    $.get("/GestionEmpresa/ProcesarSeleccion/", { id: idEmpresa, vistaActiva: ConsultarEmpresa.vista },
        function (data) { $("#divVista").html(data); },
        "html");

    $("#divFiltrosDeBusqueda").hide();
    $("#divDatosGeneralesEmpresa").show();
    $("#divVista").show();

    var empresa = getFila(idEmpresa, ConsultarEmpresa.Grilla.id);
    $("#Id").val(empresa.Id);
    $("#VerCodigoEmpresa").val(empresa.CodigoEmpresa);
    $("#VerNombreEmpresa").val(empresa.NombreEmpresa);
    $("#VerCueEmpresa").val(empresa.CUE);
    $("#VerTipoEducacion").val(empresa.TipoEducacion);
    $("#VerNivelEducativo").val(empresa.NivelEducativo);
    $("#VerTipoEmpresa").val(empresa.TipoEmpresa);
    $("#VerEstadoEmpresa").val(empresa.EstadoEmpresa);
};

/*
Funcionalidad a los combos en cascada. De acuerdo al departamento provincial, me muestra una localidad.
*/
$(document).ready(function () {
    $("#EsPrivada").hide();
    $(".datepicker").datepicker({ currentText: 'Now' });
    $("#FiltroLocalidadBasico").CascadingDropDown("#FiltroDepartamentoProvincialBasico", '/GestionEmpresa/CargarLocalidadesBasico', { promptText: 'SELECCIONE' });
    $("#FiltroLocalidadEmpresa").CascadingDropDown("#FiltroDepartamentoProvincialEmpresa", '/GestionEmpresa/CargarLocalidadesAvanzadoEmpresa', { promptText: 'SELECCIONE' });
    $("#FiltroLocalidadEscuela").CascadingDropDown("#FiltroDepartamentoProvincialEscuela", '/GestionEmpresa/CargarLocalidadesAvanzadoEscuela', { promptText: 'SELECCIONE' });
});

/************************************************************** GRILLA **************************************************************/ 

ConsultarEmpresa.Grilla.init = function () {
    var grilla = ConsultarEmpresa.Grilla.id;

    $(grilla).jqGrid({
        url: ConsultarEmpresa.Grilla.accionListar,
        datatype: "local",
        mtype: "GET",
        colNames: ['Id', 'Fecha alta', 'Código empresa', 'CUE', 'Nombre empresa', 'Tipo empresa', 'Estado empresa', 'Nivel Educativo', 'Tipo Educacion'],
        colModel: [
            { name: 'Id',               index: 'Id',                align: 'left',      key: true,      hidden: true },
            { name: 'FechaAlta',        index: 'FechaAlta',         align: 'left' },
            { name: 'CodigoEmpresa',    index: 'CodigoEmpresa',     align: 'left' },
            { name: 'CUE',              index: 'CUE',               align: 'left' },
            { name: 'NombreEmpresa',    index: 'NombreEmpersa',     align: 'left' },
            { name: 'TipoEmpresa',      index: 'TipoEmpresa',       align: 'left' },
            { name: 'EstadoEmpresa',    index: 'EstadoEmpresa',     align: 'left' },
            { name: 'NivelEducativo',   index: 'NivelEducativo',    align: 'left' },
            { name: 'TipoEducacion',    index: 'TipoEducacion',     align: 'left'}],
        onSelectRow: function (id) { idSeleccionado = $(grilla).getCell(id, "Id"); },
        pager: "#pager",
        toppager: true,
        rowNum: 10,
        rowList: [5, 10, 20, 50],
        sortname: "CUE",
        sortorder: "asc",
        viewrecords: true,
        autowidth: true,
        width: document.body.offsetWidth - 650,
        height: "100%",
        loadtext: "Cargando, espere por favor",
        emptyrecords: ""
    });

    $(grilla).navGrid("#pager",
        { cloneToTop: true, view: false, add: false, edit: false, del: false, search: false, refresh: false },  // actualizar grilla
        {}, // editar registro
        {}, // insertar registro
        {}, // eliminar registro
        {}, // buscar registros (ventana modal)
        {}  // ver registro
    );

    // Creo los botones de la barra de herramientas
    ConsultarEmpresa.Grilla.crearBotones();

    // Agrego div para mostrar mensajes personalizado
    divGrilla = grilla.replace("#", "");
    $("#gview_" + divGrilla).append("<div id='sinConsulta' class='ui-mensaje'>Aún no se ha realizado ningúna búsqueda</div>");
    $("#gview_" + divGrilla).append("<div id='sinRegistros' class='ui-mensaje'>No se encontraron resultados con los datos de búsqueda ingresados</div>");

    // Limpio los datos de la consulta para el primer ingreso y oculto los botones que no sean necesarios
    $("#sinConsulta").show(); /// <reference path="MicrosoftMvcAjax.debug.js" />

    ConsultarEmpresa.Grilla.mostrarBotones();
    ConsultarEmpresa.Grilla.validarBotones();
};

ConsultarEmpresa.Grilla.actualizar = function () {
    actualizarGrilla(ConsultarEmpresa.Grilla.id);
};

ConsultarEmpresa.Grilla.limpiar = function () {
    limpiarGrilla(ConsultarEmpresa.Grilla.id, ConsultarEmpresa.Grilla.accionListar, ConsultarEmpresa.Grilla.botones);
};

ConsultarEmpresa.Grilla.limpiarUrl = function () {
    editarURL(ConsultarEmpresa.Grilla.id, ConsultarEmpresa.Grilla.accionListar);
};

ConsultarEmpresa.Grilla.parametrosUrl = function (parametros) {
    editarURL(ConsultarEmpresa.Grilla.id, ConsultarEmpresa.Grilla.accionListar + "?" + parametros);
};

ConsultarEmpresa.Grilla.mostrarBotones = function () {
    mostrarBotones(ConsultarEmpresa.Grilla.id, ConsultarEmpresa.Grilla.botones);
};

ConsultarEmpresa.Grilla.cambiarBotones = function (nuevosBotones) {
    ConsultarEmpresa.botones = nuevosBotones;
    ConsultarEmpresa.Grilla.mostrarBotones();
};

ConsultarEmpresa.Grilla.crearBotones = function () {
    var grilla = ConsultarEmpresa.Grilla.id;
    var botones = ConsultarEmpresa.Grilla.botones;

    // Elimino la paginacion en la barra de arriba
    $(grilla + "_toppager_center", grilla + "_toppager").remove();
    $(".ui-paging-info", grilla + "_toppager").remove();

    //Seteado de botones
    switch (ConsultarEmpresa.vista) {
        case "RegistrarEmpresaEditor":
            ConsultarEmpresa.Grilla.botones = ['Ver', 'Editar', 'Agregar'];
            break;
        case "VisarActivacionEmpresaEditor":
        case "ActivacionCodigoEmpresaEditor":
        case "CerrarEmpresaEditor":
        case "VisarCierreEmpresaEditor":
        case "ReactivarEmpresaEditor":
        case "SinVista":
            ConsultarEmpresa.Grilla.botones = ['Seleccionar'];
            break;
    }

    for (var i = 0; i < botones.length; i++) {

        //Creacion de botones (en realidad: de NO creacion)
        if (ConsultarEmpresa.vista != "RegistrarEmpresaEditor") { //Si es de Visados
            if (botones[i] != "Seleccionar") { // Y no es el boton "Seleccionar"
                continue; //No crearlo
            }
        } else {
            if (botones[i] == "Seleccionar") {
                continue;
            }
        }

        switch (botones[i]) {
            case "Seleccionar":
                $(grilla).navButtonAdd(grilla + "_toppager_left",
                {
                    position: "first",
                    caption: "Seleccionar",
                    title: "Seleccionar",
                    buttonicon: "ui-icon-pin-s",
                    onClickButton: function () {
                        var seleccion = filaSeleccionada(grilla);

                        if (seleccion) {
                            ConsultarEmpresa.seleccionarEmpresa(seleccion);

                        } else {
                            viewModal("#alertmod", { gbox: "#gbox_", jqm: true });
                        }
                    }
                });
                break;
            case "Ver":
                $(grilla).navButtonAdd(grilla + "_toppager_left",
                {
                    position: "first",
                    caption: "Ver",
                    title: "Ver",
                    buttonicon: "ui-icon-search",
                    onClickButton: function () {
                        var seleccion = filaSeleccionada(grilla);

                        if (seleccion)
                            $.get("/GestionEmpresa/Ver/" + seleccion, {},
                                    function (data) { $("#divVista").html(data); },
                                    "html");
                        else
                            viewModal("#alertmod", { gbox: "#gbox_", jqm: true });
                    }
                });
                break;
            case "Agregar":
                $(grilla).navButtonAdd(grilla + "_toppager_left",
                {
                    position: "first",
                    caption: "Agregar",
                    title: "Agregar",
                    buttonicon: "ui-icon-plus",
                    onClickButton: function () {
                        $("#estadoVista").val("Registrar");

                        $.get("/GestionEmpresa/Registrar", {},
                            function (data) { $("#divVista").html(data); },
                            "html");
                        ConsultarEmpresa.visualizarDivs();

                    }
                });
                break;
            case "Editar":
                $(grilla).navButtonAdd(grilla + "_toppager_left",
                {
                    position: "first",
                    caption: "Editar",
                    title: "Editar",
                    buttonicon: "ui-icon-pencil",
                    onClickButton: function () {
                        $("#estadoVista").val("Editar");
                        var seleccion = filaSeleccionada(grilla);
                        if (seleccion) {
                            $.get("/GestionEmpresa/Editar/" + seleccion, {},
                                    function (data) { $("#divVista").html(data); },
                                    "html");
                            ConsultarEmpresa.visualizarDivs();
                        }
                        else
                            viewModal("#alertmod", { gbox: "#gbox_", jqm: true });
                    }
                });
                break;
        }

    }
};

ConsultarEmpresa.Grilla.validarBotones = function () {
    var grilla = ConsultarEmpresa.Grilla.id;

    switch (ConsultarEmpresa.vista) {

        case "ValorDeEnum": // [MODIFICABLE] El valor del Enum que identifica a X vista
            // Esto hace que todos los items de la grilla, ejecuten esta funcion cuando sean seleccionados
            $(grilla).jqGrid().bind(
                "onSelectRow",
                function (rowid, status) {
                    var estado = $(ConsultarEmpresa.Grilla.id).jqGrid().getCell(rowid, "EstadoEmpresa");
                    // [COPIABLE]
                    if (estado == "CERRADA") { // [MODIFICABLE] "CERRADA", es el valor que tiene que tener el EstadoEmpresa para que entre por el IF
                        $("td[title='Seleccionar']").hide(); // [MODIFICABLE] 'Seleccionar', es el valor del titulo del boton a Mostrar/Ocultar
                    } else {
                        $("td[title='Seleccionar']").show();
                    }
                    // FIN: [COPIABLE]

                });
            break;

        //////////////////////////////////////////////////////////////////////////////////////      
        // Por cada valor de ENUM, copiar el CASE anterior, y configurar su funcionabilidad //      
        //////////////////////////////////////////////////////////////////////////////////////       
        case "RegistrarEmpresaEditor":
            $(grilla).jqGrid().bind(
                "onSelectRow",
                function (rowid) {
                    console.log('asd');
                    var estado = $(ConsultarEmpresa.Grilla.id).jqGrid().getCell(rowid, "EstadoEmpresa");
                    alert(estado);
                    alert(rowid);
                    alert(status);
                    // [COPIABLE]
                    //if (estado == "CERRADA") { // [MODIFICABLE] "CERRADA", es el valor que tiene que tener el EstadoEmpresa para que entre por el IF
                    //$("td[title='Seleccionar']").hide(); // [MODIFICABLE] 'Seleccionar', es el valor del titulo del boton a Mostrar/Ocultar
                    //} else {
                    //$("td[title='Seleccionar']").show();
                    //}
                    // FIN: [COPIABLE]

                });
            break;
        default:
            break;
    }
};

ConsultarEmpresa.visualizarDivs = function () {
    if ($("#estadoVista").val() == "Consultar") {
        $("#divConsulta").show();
        $("#divVista").hide();
    }
    else {
        $("#divConsulta").hide();
        $("#divVista").show();
    }
};