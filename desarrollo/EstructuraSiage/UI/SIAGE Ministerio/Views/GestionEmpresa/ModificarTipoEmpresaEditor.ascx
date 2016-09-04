<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EscuelaModificarTipoEmpresaModel>" %>

<%
    string vista = (string)ViewData[Constantes.VistaEmpresa] ?? VistaEmpresa.ModificarTipoEmpresaEditor.ToString();
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Consultar";
    int estadoId = (estadoText != "Consultar") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Consultar;

    using (Html.BeginForm()) {
%>
    <%:Html.HiddenFor(model => model.Id) %>
    <fieldset>
        <%: Html.AbmcTextControlFor(model => model.TipoEmpresa, estadoId, Abmc.TextControl.DropDownListEnum) %>
        <div id="EscuelaMadre">
            <%: Html.AbmcCheckControlFor(model => model.EsRaiz, estadoId, Abmc.CheckControl.CheckBox) %>
            <p><%: Html.BtnGenerico(Botones.ButtonType.button, "Buscar escuela raiz", string.Empty, string.Empty, "btnBuscarEscuelaRaiz") %></p>
        </div>
        <div id="divBuscarEscuelaRaiz">
            <% Html.RenderPartial("ConsultarEmpresaEditor"); %>
        </div>
        <div id="EscuelaAnexo">
            <%: Html.AbmcTextControlFor(model => model.NumeroAnexo, estadoId, Abmc.TextControl.TextBox) %>
            <p><%: Html.BtnGenerico(Botones.ButtonType.button, "Buscar escuela madre", string.Empty, string.Empty, "btnBuscarEscuelaMadre") %></p>
        </div>
        <div id="divBuscarEscuelaMadre">
            <% Html.RenderPartial("ConsultarEmpresaEditor");%>
        </div>

        <p>
            <%:Html.AbmcTextControlFor(model => model.Nombre, estadoId, Abmc.TextControl.TextBox) %>
            <%:Html.BtnGenerico(Botones.ButtonType.button, "Sugerir nombre", string.Empty, string.Empty, "btnSugerirNombre") %>
        </p>

        <p class="botones">
            <%: Html.BtnGenerico(Botones.ButtonType.button, "Aceptar", string.Empty, string.Empty, "btnAceptarModificacion") %>
            <%: Html.BtnGenerico(Botones.ButtonType.button, "Cancelar", string.Empty, string.Empty, "btnCancelarModificacion") %>
        </p>
    </fieldset>
<% } %>

<script type="text/javascript">
    $(document).ready(function () {
        ModificarTipoEmpresa.init('SinVista', editorConsulta);
    });    
</script>