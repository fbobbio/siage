<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.AsignacionInstrumentoLegalModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;
%>

<fieldset>
    <legend><%: estadoText %> asignación de instrumento legal</legend>
    <%: Html.HiddenFor(model => model.Id) %>
    
    <div id="divInstrumentoLegal">
        <%: Html.Partial("ConsultarInstrumentoLegalEditor", new InstrumentoLegalModel()) %>
    </div>

    <%: Html.AbmcTextControlFor(model => model.Observaciones, estadoId, Abmc.TextControl.TextArea) %>
    
    <div id="divFechaNotificacion" style="display:none;">
        <%: Html.AbmcTextControlFor(model => model.FecNotificacion, estadoId, Abmc.TextControl.Calendar) %>
    </div>
</fieldset>
