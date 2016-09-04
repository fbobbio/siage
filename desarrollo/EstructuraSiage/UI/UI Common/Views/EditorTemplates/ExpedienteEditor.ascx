<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.ExpedienteModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;
%>

<fieldset>
    <legend>Expediente</legend>
    <%: Html.HiddenFor(model => model.Id)%>

    <div id="divConsulta">
        <p><%: Html.Label("Número de expediente: ") %><%: Html.TextBox("FiltroNumero") %></p>       
    
        <p class="botones">
            <button type="button" id="btnBuscar" >Buscar</button>
            <button type="button" id="btnNuevo" >Nuevo</button>
        </p>
    </div>

    <div id="divVista" style="display:none;">
        <%: Html.AbmcTextControlFor(model => model.Numero, estadoId, Abmc.TextControl.TextBox)%>
        <%: Html.AbmcTextControlFor(model => model.FechaInicio, estadoId, Abmc.TextControl.Calendar)%>   
        <%: Html.AbmcTextControlFor(model => model.Asunto, estadoId, Abmc.TextControl.TextBox)%>
            
        <div id="divAgente">
            <% Html.RenderPartial("ConsultarAgenteEditor", new AgenteModel());%>
        </div>

        <p class="botones">
            <button type="button" id="btnBusqueda" >Buscar expediente</button>
        </p>
    </div>
</fieldset>

