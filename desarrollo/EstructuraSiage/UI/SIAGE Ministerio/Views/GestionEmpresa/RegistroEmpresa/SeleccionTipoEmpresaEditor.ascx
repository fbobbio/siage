<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaRegistrarModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<%@ Import Namespace="Siage.Base" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Consultar";
    int estadoId = (estadoText != "Consultar") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Consultar;
    
    using (Html.BeginForm())
    {
%>



<fieldset>
    <legend>Registrar Empresa</legend>
    <script>
       
    </script>
    <%: Html.HiddenFor(model => model.Id) %>
    <%:Html.AbmcTextControlFor(model => model.TipoGestion, estadoId, Abmc.TextControl.DropDownListEnum)%>
    <%:Html.AbmcTextControlFor(model => model.TipoEmpresa, estadoId, Abmc.TextControl.DropDownListEnum)%>
    
    <div id="divCodigoEmpresa" style="display:none">
        <%: Html.AbmcTextControlFor(model => model.CodigoEmpresa, estadoId, Abmc.TextControl.TextBox)%>
    </div>

    <p class="botones">
        <%:Html.BtnGenerico(Botones.ButtonType.button, "Seleccionar", string.Empty, string.Empty,"btnSeleccionarTipoEmpresa")%>
    </p>
</fieldset>

<div id="divRegistrarEmpresa" />

<% } %>

<script type="text/javascript">
    $(document).ready(Empresa.Registrar.initEditorRegistrar('<%: (string)ViewData[Constantes.VistaEmpresa] %>', "<%: estadoText %>"));
</script>