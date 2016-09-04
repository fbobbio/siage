<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Dictionary<string, Siage.Services.Core.Models.AsignacionInstrumentoLegalModel>>" %>

<% using(Html.BeginForm()) { %> 

    <p><%: Html.Label("Las siguientes campos modificados requieren que se registre un Intrumento Legal: ")%></p>
    <p><%: Html.DropDownList("cmbCampos", new SelectList(Model.Keys)) %></p>

    <div id="divAsignacion">
        <%:Html.Editor("AIL", "AsignacionInstrumentoLegalEditor")%>
    </div>

    <%: Html.BtnGenerico(Botones.ButtonType.button, "Aceptar", string.Empty, string.Empty, "btnAceptar")%>
<% } %>

<script type="text/javascript">
    $(document).ready(ModificarEmpresa.init);
</script>