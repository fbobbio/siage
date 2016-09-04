<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%
    string vista = (string)ViewData[Constantes.VistaEmpresa] ?? VistaEmpresa.ActivacionCodigoEmpresaEditor.ToString();    
    using (Html.BeginForm())
    {
%>
<div id="divActivacionCodigoEmpresa">
    <p class="botones">
        <%: Html.BtnGenerico(Botones.ButtonType.button, "Activar", string.Empty, string.Empty, "btnAceptar")%>
        <%: Html.BtnGenerico(Botones.ButtonType.button, "Cancelar", string.Empty, string.Empty, "btnCancelar")%></p>
</div>

<% } %>

<script type="text/javascript">
    $(document).ready(function () {
        EmpresaActivarCodigo.init(Empresa.Registrar.consulta);
    });  
</script>