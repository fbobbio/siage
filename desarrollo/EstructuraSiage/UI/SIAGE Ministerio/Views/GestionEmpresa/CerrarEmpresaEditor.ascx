<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaCierreModel>" %>

<% 
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Registrar";
    int estadoId = (estadoText != "Registrar") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Registrar;
    using (Html.BeginForm())
    {
%>

<div id="divCierreEmpresa">
  <fieldset>
    <legend>Cierre de Empresa</legend>  
         <%: Html.HiddenFor(model => model.IdEmpresa)%>
    <p>
        <%:Html.AbmcTextControlFor(model => model.FechaCierre, estadoId, Abmc.TextControl.Calendar)%>
    </p>

    <div id="divEmitirResolucionCheck">
        <p>
            <%:Html.Label("Emitir resolución de cierre:")%>    
            <%:Html.CheckBox("EmitirResolucionDeCierre")%>
        </p>
    </div>
    
    <div id="divAsignacionInstrumentoLegalCierre">
    <%: Html.EditorForModel("AsignacionInstrumentoLegalEditor")%>
    </div>

    <div id="divResolucionCierre" style="display:none">
    <%: Html.EditorForModel("EmitirResolucionEmpresaEditor") %>
    </div>

    <div id="divDatosAsignacionILCierre">
    <fieldset>
    <legend>Datos asignación instrumento legal</legend>
        <%:Html.AbmcTextControlFor(model => model.FechaNotificacion, estadoId, Abmc.TextControl.Calendar)%>
        <%:Html.AbmcTextControlFor(model => model.ObservacionesAsignacion, estadoId, Abmc.TextControl.TextArea)%>
    </fieldset>
    </div>

  </fieldset>
</div>
<div id="divBotonesCierre">
    
    <p class="botones">
        <%:Html.BtnGenerico(Botones.ButtonType.button, "Aceptar", string.Empty, string.Empty,
                                           "btnAceptar1")%>
        <%:Html.BtnGenerico(Botones.ButtonType.button, "Cancelar", string.Empty, string.Empty,
                                           "btnCancelar")%>
        <%:Html.BtnGenerico(Botones.ButtonType.button, "Volver", string.Empty, string.Empty,
                                           "btnVolver")%>
    </p>
    </div>
<%
    }
%>

<script type="text/javascript">
    $(document).ready(function () {
        CierreEmpresa.init(Empresa.Registrar.consulta);
    });
   
</script>