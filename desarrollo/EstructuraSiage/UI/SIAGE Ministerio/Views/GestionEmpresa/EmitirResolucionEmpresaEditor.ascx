<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.ResolucionModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%: Html.RenderPartial("EmitirResolucionEmpresaEditor", new ResolucionModel())%>

<p class="botones">
    <%: Html.BtnGenerico(Botones.ButtonType.button, "Aceptar", string.Empty, string.Empty, "btnAceptarResolucion") %>
    <%: Html.BtnGenerico(Botones.ButtonType.button, "Cancelar", string.Empty, string.Empty, "btnCancelarResolucion")%>
</p>