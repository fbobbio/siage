<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AjaxAbmc.Master" Inherits="System.Web.Mvc.ViewPage<Siage.Services.Core.Models.MabModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TituloContent" runat="server">MABs
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="PersonalizableContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FiltrosBusquedaContent" runat="server">

    <fieldset>
    <legend>Seleccionar forma de búsqueda</legend>
    <p>
        <label>Por código de barra</label>
        <%: Html.RadioButton("radioBtn", "Código de barra", false, new { id = "rdbCodigoBarra" })%></p>
    <p>
        <label>Por agente</label>
        <%: Html.RadioButton("radioBtn", "Agente", false, new { id = "rdbAgente" })%></p>
</fieldset>

<div id="divFiltroPorCodigoBarra" style="display:none">
    <p><%: Html.Label("Código de barra: ") %> <%: Html.TextBox("FiltroCodigoBarra") %> </p>
</div>

<div id="divFiltroPorAgente" style="display:none">
    <div id="divConsultarAgenteMabIndex">
        <% Html.RenderPartial("ConsultarAgenteEditor");%>
    </div>
    <div id="divFiltroFechasAgente">
        <fieldset>
            <legend>Fechas de registro</legend>
            <p> <%: Html.Label("Fecha desde: (*)") %> <%: Html.DateTimeTextBox("FiltroFechaInicial") %> </p>    
            <p> <%: Html.Label("Fecha hasta: (*)") %> <%: Html.DateTimeTextBox("FiltroFechaFinal") %> </p>
        </fieldset>
    </div>
</div>
    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%: Url.Content("~/Scripts/jquery.tmpl.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.cascadingDropDown.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.abmc.formulario.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.abmc.grilla.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.abmc.util.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.mab.js") %>" type="text/javascript"></script>    
    <script src="<%: Url.Content("~/Scripts/siage.grillas.js") %>" type="text/javascript"></script>    
    <script src="<%: Url.Content("~/Scripts/siage.personaFisica.js") %>" type="text/javascript"></script>    
    <script src="<%: Url.Content("~/Scripts/siage.instrumentoLegal.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.asignacionInstrumentoLegal.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.expediente.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.agente.consultar.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.puestoDeTrabajo.consultar.js") %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $(".val-DateTime").datepicker({ currentText: 'Now', dateFormat: 'dd/MM/yy' });
            $('#btnConsultar').attr('disabled', 'disabled');

            /** Inicializo el script del consultar agente */
            var instanciaAgente = AgenteConsultar.init("#divConsultarAgenteMabIndex", "MabIndex");
            /** Oculto el boton volver de agente */
            $('#MabIndex_btnVolverAgente').hide();
            var idAgente = null;

            if (!Validacion.validar()) {
                return;
            }

            /** Eventos de selección de tipo de filtro */
            $("#rdbCodigoBarra").changePatch(function () {
                $("#divFiltroPorCodigoBarra").show();
                $("#divFiltroPorAgente").hide();
                $('#FiltroFechaInicial').val('');
                $('#FiltroFechaFinal').val('');
                $('#btnConsultar').removeAttr('disabled');
                $('#btnConsultar').show();
                $('#btnLimpiar').show();
            });
            $("#rdbAgente").changePatch(function () {
                $("#divFiltroPorCodigoBarra").hide();
                $("#divFiltroPorAgente").show();
                $("#MabIndex_divGrillaSeleccionAgente").show();
                GrillaUtil.limpiar($("#MabIndex_seleccionAgenteList"));
                $('#FiltroCodigoBarra').val('');
                $('#btnConsultar').removeAttr('disabled');

                /** Si se selecciona un agente, que permita ingresar las fechas de búsqueda y consulte el/los Mabs asociados a ese agente */
                $('#divFiltroFechasAgente').hide();
                $('#btnConsultar').hide();
                $('#btnLimpiar').hide();
            });

            instanciaAgente.successSeleccion = function () {
                idAgente = instanciaAgente.seleccion;
                if (idAgente != null) {
                    $('#rdbCodigoBarra').attr('disabled', true);
                    $('#rdbAgente').attr('disabled', true);
                }
                $('#btnConsultar').show();
                $('#btnLimpiar').show();
                $('#divFiltroFechasAgente').show();
                $('#MabIndex_btnVolver').hide();
            };

            /** Cargo la grilla con los nombre de las columnas y los tipo de datos con los que se carga */
            var controller = "Mab";
            var orderBy = "Id";
            var titulos = ['Id', 'Tipo de novedad', 'Tipo doc.', 'Nro. doc.', 'Apellido', 'Nombre', 'Fecha novedad inicio', 'Fecha novedad fin', 'Codigo novedad', 'Nombre empresa', 'Codigo empresa', 'Cargo', 'Horas', 'Materia', 'Turno', 'Grado/Año', 'Sección/División'];
            var propiedades = ['Id', 'TipoNovedad', 'TipoDocumentoNombreAgente', 'NumeroDocumentoAgente', 'ApellidoAgente', 'NombreAgente', 'FechaNovedadDesde', 'FechaNovedadHasta', 'CodigoDeNovedad', 'EmpresaNombre', 'EmpresaCodigo', 'CodigoCargo', 'HorasCargo', 'Materia', 'Turno', 'GradoAno', 'SeccionDivision'];
            var tipos = ['integer', null, null, null, null, null, 'date', 'date', null, null, null, null, null, null, null, null, null];
            var botones = ["Reactivar", "Ver", "Eliminar", "Editar", "Agregar"]; // en orden inverso al que se mostraran
            var key = 'Id';

            var grilla = AbmcGrilla.init("#list", controller, titulos, propiedades, tipos, key, orderBy, botones);
            Abmc.init(controller, grilla);
            /** Cargo los parámetros para realizar la búsqueda */
            $("#divConsulta :input").changePatch(function () {
                var parametros = "&filtroCodigoBarra=" + $("#FiltroCodigoBarra").val() +
                "&filtroIdAgente=" + idAgente +
                "&filtroFechaInicial=" + $("#FiltroFechaInicial").val() +
                "&filtroFechaFinal=" + $("#FiltroFechaFinal").val();
                grilla.agregarParametros(parametros);
            });

            /** Oculto todos los divs */
            $("#btnLimpiar").click(function () {
                $("#divFiltroPorCodigoBarra").hide();
                $("#divFiltroPorAgente").hide();
                $('#btnConsultar').attr('disabled', 'disabled');
                $("#rdbAgente").attr('disabled', false);
                $("#rdbCodigoBarra").attr('disabled', false);
                $('#MabIndex_divDatosAgente').hide();
                $('#MabIndex_divFiltrosDeBusqueda').show();
            });

            /** Valido que no pueda consultar sin haber ingresado el código de barra o la fecha inicial y final */
            $("#btnConsultar").unbind("click");
            $('#btnConsultar').click(function () {
                if ($("#rdbCodigoBarra").is(':checked') === true) {
                    if ($('#FiltroCodigoBarra').val() == '') {
                        Mensaje.Error.texto = "Debe ingresar el código de barra";
                        Mensaje.Error.mostrar();
                    }
                    else {
                        Abmc.grilla.actualizar();
                        Abmc.sinConsulta = false;
                    }
                }
                else {
                    if ($("#rdbAgente").is(':checked') === true) {
                        if ($("#FiltroFechaInicial").val() == '' || $("#FiltroFechaFinal").val() == '') {
                            Mensaje.Error.texto = "Debe ingresar ambas fechas para poder realizar la búsqueda";
                            Mensaje.Error.mostrar();
                        }
                        else {
                            Abmc.grilla.actualizar();
                            Abmc.sinConsulta = false;
                        }
                    }
                }
            });
        });

    </script>
</asp:Content>