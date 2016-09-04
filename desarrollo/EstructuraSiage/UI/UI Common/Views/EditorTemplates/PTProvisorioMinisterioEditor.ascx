<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.PuestoDeTrabajoProvisorioModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<div id="divDatosVer"></div>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;
%>
<fieldset>
    <legend>Tareas pasivas</legend>
    <div id="tabs">
            <ul>
                <li id="liSolapaDatosAgente"><a href="#divSolapaDatosAgente">Agente</a></li>
                <li id="liSolapaOrigen"><a href="#divSolapaOrigen">Origen</a></li>
                <li id="liSolapaDestino"><a href="#divSolapaDestino">Destino</a></li>
                <li id="liSolapaDatos"><a href="#divSolapaDatos">Datos</a></li>
            </ul>
        <div id="divSolapaDatosAgente" >
                <%: Html.Hidden("IdAgente") %>
                <%: Html.Label("Legajo: ")%><%: Html.TextBox("VerLegajo", string.Empty, new { disabled = true })%>
                <%: Html.Label("Nombre: ")%><%: Html.TextBox("VerNombre", string.Empty, new { disabled = true })%>
                <%: Html.Label("Apellido: ")%><%: Html.TextBox("VerApellido", string.Empty, new { disabled = true })%>
                <%: Html.Label("Sexo: ")%><%: Html.TextBox("VerSexo", string.Empty, new { disabled = true })%>
                <%: Html.Label("Tipo agente: ")%><%: Html.TextBox("VerTipoAgente", string.Empty, new { disabled = true })%>
                <%: Html.Label("Tipo documento: ")%><%: Html.TextBox("VerTipoDocumento", string.Empty, new { disabled = true })%>
                <%: Html.Label("Nro. documento: ")%><%: Html.TextBox("VerNroDocumento", string.Empty, new { disabled = true })%>

            <p></p>
        </div>
        <div id="divSolapaOrigen" >
                <%: Html.Hidden("IdPT") %>
                <%: Html.Label("Código de cargo: ") %><%: Html.TextBox("VerCodigoCargo", string.Empty, new { disabled = true }) %>
                <%: Html.Label("Nombre de cargo: ") %><%: Html.TextBox("VerNombreCargo", string.Empty, new { disabled = true }) %>
                <%: Html.Label("Horas: ") %><%: Html.TextBox("VerHoras", string.Empty, new { disabled = true }) %>
                <%: Html.Label("Plan de estudio: ") %><%: Html.TextBox("VerPlanEstudio", string.Empty, new { disabled = true }) %>
                <%: Html.Label("Materia: ") %><%: Html.TextBox("VerMateria", string.Empty, new { disabled = true }) %>
                <%: Html.Label("Turno: ") %><%: Html.TextBox("VerTurno", string.Empty, new { disabled = true }) %>
                <%: Html.Label("Grado / Año: ") %><%: Html.TextBox("VerGradoAnio", string.Empty, new { disabled = true }) %>
                <%: Html.Label("Sección / División: ") %><%: Html.TextBox("VerSeccionDivision", string.Empty, new { disabled = true }) %>
                <%: Html.Label("CUPOF: ") %><%: Html.TextBox("VerCupof", string.Empty, new { disabled = true }) %>
            <p></p>
        </div>
        <div id="divSolapaDestino" >
            <fieldset>
                <legend>Consulta de empresa</legend>
                <div id="divEmpresa">
                <% Html.RenderPartial("ConsultarEmpresaEditor"); %>     
                </div>              
            </fieldset>           
            <p></p>
        </div>
        <div id="divSolapaDatos" >
            <%: Html.AbmcTextControlFor(model => model.FechaInicio, estadoId, Abmc.TextControl.TextBox) %>
            <%: Html.AbmcTextControlFor(model => model.FechaHasta , estadoId, Abmc.TextControl.TextBox) %>
            <p></p>
        </div> 
    </div>
    <p class="botones">
                      <%-- <%: Html.AjaxAbmcBotones(estadoId) %>--%>
           <%:Html.BtnGenerico(Botones.ButtonType.button, "Aceptar", string.Empty, string.Empty, "btnAceptar")%>
           <%:Html.BtnGenerico(Botones.ButtonType.button, "Cancelar", string.Empty, string.Empty, "btnSalir")%>

    </p>
</fieldset>
        
        
<script type="text/javascript">
    $(document).ready(function () {
        PuestoDeTrabajoProvisorio.RegistrarTareasPasivas.init();
        $("#tabs").tabs();
    });
</script>


