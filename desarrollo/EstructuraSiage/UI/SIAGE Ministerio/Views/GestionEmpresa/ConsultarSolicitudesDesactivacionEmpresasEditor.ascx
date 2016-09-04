<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.ConsultarSolicitudesDesactivacionEmpresasModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    string vista = (string)ViewData[Constantes.VistaEmpresa] ?? "ConsultarSolicitudesDesactivacionEmpresasEditor";
    string usuarioLogueado = (string)ViewData[Constantes.TipoUsuarioLogueado];
    var departamentoProvincial = Html.CreateSelectList<DepartamentoProvincialModel>(Siage.Base.ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString());
    var localidad = Html.CreateSelectList<LocalidadModel>(Siage.Base.ViewDataKey.LOCALIDAD.ToString());
%>

<div id="divFiltroConsultarSolicitudDesactivacionEmpresa">
    <fieldset id="FiltroConsultarSolicitudesActivacionEmpresa">
        <legend>Consultar solicitud desactivación empresas</legend>
        <p>
            <%: Html.Label("CUE") %>
            <%: Html.TextBox("FiltroCUE") %></p>
        <p>
            <%: Html.Label("Código empresa") %>
            <%: Html.TextBox("FiltroCodigoEmpresa") %></p>
        <p>
            <%: Html.Label("Número escuela") %>
            <%: Html.TextBox("FiltroNumeroEscuela") %></p>
        <p>
            <%: Html.Label("Número de pedido de autorización") %>
            <%: Html.TextBox("FiltroNumeroPedidoAutorizacion") %></p>
        <p>
            <%: Html.Label("Fecha alta desde") %>
            <%: Html.DateTimeTextBox("FiltroFechaAltaDesde") %></p>
        <p>
            <%: Html.Label("Fecha alta hasta") %>
            <%: Html.DateTimeTextBox("FiltroFechaAltaHasta") %></p>
        <%--<p>
            <%: Html.Label("Estado del pedido de autorización") %>
            <%: Html.DropDownListEnum("FiltroEstadoPedidoAutorizacion") %></p>--%>

        <fieldset>
            <legend>Domicilio</legend>
            <p>
                <%: Html.Label("Departamento Provincial:") %>
                <%: Html.DropDownList("FiltroDepartamentoProvincial", departamentoProvincial, Constantes.Seleccione) %></p>
            <p>
                <%: Html.Label("Localidad:") %>
                <%: Html.DropDownList("FiltroLocalidad", localidad, Constantes.Seleccione)%></p>
            <p>
                <%: Html.Label("Barrio:") %>
                <%: Html.TextBox("FiltroBarrioBasico") %></p>
            <p>
                <%: Html.Label("Calle:") %>
                <%: Html.TextBox("FiltroCalleBasico") %></p>
            <p>
                <%: Html.Label("Altura:") %>
                <%: Html.TextBox("FiltroAlturaBasico") %></p>
        </fieldset>

        <p class="botones">
            <input id="btnConsultar" type="button" value="Consultar" />
            <input id="btnLimpiar" type="button" value="Limpiar" />
        </p>
    </fieldset>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $("#FiltroLocalidad").CascadingDropDown("#FiltroDepartamentoProvincial", '/GestionEmpresa/CargarLocalidades', { promptText: 'SELECCIONE' });
    });    
</script>