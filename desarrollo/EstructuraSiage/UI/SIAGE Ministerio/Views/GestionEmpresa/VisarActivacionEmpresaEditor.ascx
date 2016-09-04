<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaVisarModel>" %>
<%@ Import Namespace="Siage.Base" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Registrar";
    int estadoId = (estadoText != "Registrar") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Registrar;
    
    using (Html.BeginForm())
    {
%>
<fieldset>
    <legend>Visado de Empresa</legend>
    <%: Html.HiddenFor(model => model.Id) %>
    <p><%: Html.LabelFor(model => model.ObservacionesVisarActivacion) %> <%: Html.TextAreaFor(model => model.ObservacionesVisarActivacion)%></p> 
    <p><%: Html.AbmcTextControlFor(model => model.Accion, estadoId, Abmc.TextControl.DropDownListEnum)%></p>
    
    <p class="botones">
        <%: Html.BtnGenerico(Botones.ButtonType.button, "Aceptar", string.Empty, string.Empty, "btnAceptarVisadoActivacion") %>
        <%: Html.BtnGenerico(Botones.ButtonType.button, "Cancelar", string.Empty, string.Empty, "btnCancelarVisadoActivacion") %>
    </p>
</fieldset>

<% } %>

<script type="text/javascript">
    $(document).ready(function () {
        EmpresaVisarActivacionEmpresa.init(Empresa.Registrar.consulta);
    }); 
</script>
