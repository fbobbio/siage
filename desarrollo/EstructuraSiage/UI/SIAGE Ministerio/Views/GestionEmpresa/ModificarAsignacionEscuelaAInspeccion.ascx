<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<table id="listModificarAsignacion" cellpadding="0" cellspacing="0"></table>
<div id="pagerSelect"></div>

<script type="text/javascript">

    $(document).ready(function () { 

        var titulos = ['Id', 'Fecha alta', 'Código empresa', 'CUE', 'Nombre empresa', 'Tipo empresa', 'Estado empresa', 'Nivel Educativo', 'Tipo Educacion'];
        var propiedades = ['Id', 'FechaAlta', 'CodigoEmpresa', 'CUE', 'Nombre', 'TipoEmpresa', 'EstadoEmpresa', 'NivelEducativo', 'TipoEducacion'];
        var tipos = ['int', 'date', "string", "string", "string", "string", "string", "string", "string"];
        var key = 'Id';
        Grilla.Seleccion.init("#listModificarAsignacion", titulos, propiedades, tipos, key, '/GestionEmpresa/ProcesarBusquedaAsignacionEscuela', 'Listado escuelas', SeleccionarFilas, true);
    });

    var SeleccionarFilas = function (id) {
        var bandera = false;
        var escuelasTodas = $("#listModificarAsignacion").getRowData();
        for (var i = 0; i < escuelasTodas.lenght; i++) {
            bandera = false;
            for (var j = 0; j < id.lenght; j++) {
                if (escuelasTodas[i] == id[j]) {
                    bandera = true;
                    break;
                }
            }
            if (!bandera) {
                $("#listModificarAsignacion").delRowData(i);
            }
        }        
    };
</script>