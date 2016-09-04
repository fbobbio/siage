HistorialEmpresa = {};
HistorialEmpresa.init = function () {
    //creo todos los datos q se necesitan para formar la grilla
    var titulos = ['Id', 'Nombre', 'Agente que modifica', 'Fecha'];
    var propiedades = ['Id', 'Nombre', 'NombreAgenteModificacion', 'FechaDesde'];
    var tipos = ['integer', null, null, 'date'];
    var key = 'Id';

    $('#btnVolver').click(function () {
        $('#divVista').hide();
        $('#divConsulta').show();
    });
    //Inicializo la grilla
    Grilla.Seleccion.init("#HistorialEmpresa", titulos, propiedades, tipos, key, '/GestionEmpresa/ProcesarHistorial/?id=' + GrillaUtil.getSeleccionFilas($('#consultaIndex_list'), false), 'Historial', HistorialEmpresa.seleccionarEmpresaHistorial, false);

};

//Para ver todos los datos de un registro de la grilla
HistorialEmpresa.seleccionarEmpresaHistorial = function (id) {
    $.get("/GestionEmpresa/VerDetalleHistorial/" + id, {},
            function (data) {
                $("#detalle").html(data);
                $('#TipoEmpresa').attr('disabled', false);
                var tipoEmpresa = $('#TipoEmpresa').val();
                $('#TipoEmpresa').attr('disabled', true);
                $('#divHistorialEmpresa').show();
                HistorialEmpresa.mostrarDatos(tipoEmpresa);
            },
            "html");
};

//Muestra y/u oculta divs segun el tipo de empresa.
HistorialEmpresa.mostrarDatos = function (tipoEmpresa) {
    switch (tipoEmpresa) {
        case 'MINISTERIO':
        case 'DIRECCION_DE_INFRAESTRUCTURA':
        case 'DIRECCION_DE_RECURSOS_HUMANOS':
        case 'DIRECCION_DE_SISTEMAS':
        case 'DIRECCION_DE_TESORERIA':
        case 'SECRETARIA':
        case 'SUBSECRETARIA':
        case 'APOYO_ADMINISTRATIVO':
        case 'DIRECCION_DE_NIVEL':
            $('#divHistorialEscuela').hide();
            $('#divHistorialInspeccion').hide();
            $('#divHistorialDireccionDeNivel').hide();
            break;

        case 'DIRECCION_DE_NIVEL':
            $('#divHistorialEscuela').hide();
            $('#divHistorialInspeccion').hide();
            $('#divHistorialDireccionDeNivel').show();
            break;

        case 'INSPECCION':
            $('#divHistorialEscuela').hide();
            $('#divHistorialInspeccion').show();
            $('#divHistorialDireccionDeNivel').hide();
            break;

        case 'ESCUELA_MADRE':
        case 'ESCUELA_ANEXO':
            $('#divHistorialEscuela').show();
            $('#divHistorialInspeccion').hide();
            $('#divHistorialDireccionDeNivel').hide();
            break;

        default:
            break;
    }
};