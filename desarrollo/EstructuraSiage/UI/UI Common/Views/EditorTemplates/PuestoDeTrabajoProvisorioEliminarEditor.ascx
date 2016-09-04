<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.PuestoDeTrabajoProvisorioVerModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;
%>
<div id="divGeneral">
    <%: Html.Hidden("Id") %>
    <%: Html.AbmcTextControlFor(model=> model.Empresa,estadoId,Abmc.TextControl.TextBox)%>
    <%: Html.AbmcTextControlFor(model=> model.Cargo,estadoId,Abmc.TextControl.TextBox)%>
    <%: Html.AbmcTextControlFor(model=> model.TipoPuesto,estadoId,Abmc.TextControl.TextBox)%>
    <%: Html.AbmcTextControlFor(model=> model.TipoPuestoProvisorio,estadoId,Abmc.TextControl.TextBox)%>
    <fieldset>
        <legend>Asignación</legend>
         <%: Html.AbmcTextControlFor(model=> model.AsignacionFechaInicio,estadoId,Abmc.TextControl.TextBox)%>           
         <%: Html.AbmcTextControlFor(model=> model.AsignacionFechaFin,estadoId,Abmc.TextControl.TextBox)%>   
         <%: Html.AbmcTextControlFor(model=> model.SituacionRevista,estadoId,Abmc.TextControl.TextBox)%>         
    </fieldset>
    <fieldset>
        <legend>Detalle asignación</legend>
         <%: Html.AbmcTextControlFor(model=> model.DetalleAsignacionFechaDesde,estadoId,Abmc.TextControl.TextBox)%>            
         <%: Html.AbmcTextControlFor(model=> model.DetalleAsignacionFechaHasta,estadoId,Abmc.TextControl.TextBox)%>            
    </fieldset>
</div>
<div id="divTareaPasiva" style="display:none;">
    <fieldset>
        <legend>MAB</legend>
         <%: Html.AbmcTextControlFor(model=> model.MabTipoMovimiento,estadoId,Abmc.TextControl.TextBox)%>            
         <%: Html.AbmcTextControlFor(model=> model.MabCodigoNovedad,estadoId,Abmc.TextControl.TextBox)%>
         <%: Html.AbmcTextControlFor(model=> model.MabCodigoBarra,estadoId,Abmc.TextControl.TextBox)%>            
    </fieldset>           
</div>
<div id="divMaestraIntegradora" style="display:none;">
    <fieldset>
        <legend>Inscripcion</legend>
          <%: Html.AbmcTextControlFor(model=> model.NombreEstudiante,estadoId,Abmc.TextControl.TextBox)%>   
          <%: Html.AbmcTextControlFor(model=> model.Año,estadoId,Abmc.TextControl.TextBox)%>   
          <%: Html.AbmcTextControlFor(model=> model.Seccion,estadoId,Abmc.TextControl.TextBox)%>    
          <%: Html.AbmcTextControlFor(model=> model.Turno,estadoId,Abmc.TextControl.TextBox)%> 
    </fieldset>          
</div>
<div id="divProfesorItinerante" style="display:none;">
       <fieldset>
        <legend>MAB</legend>
         <%: Html.AbmcTextControlFor(model=> model.MabTipoMovimiento,estadoId,Abmc.TextControl.TextBox)%>            
         <%: Html.AbmcTextControlFor(model=> model.MabCodigoNovedad,estadoId,Abmc.TextControl.TextBox)%>
         <%: Html.AbmcTextControlFor(model=> model.MabCodigoBarra,estadoId,Abmc.TextControl.TextBox)%>            
        </fieldset>    
        <fieldset>
            <legend>Puesto de trabajo padre</legend>
             <%: Html.AbmcTextControlFor(model=> model.Cargo,estadoId,Abmc.TextControl.TextBox)%>
             <div id="divPuestoPadre" style="display:none;">
                 <%: Html.Label("Materia: ")%><%: Html.TextBox("VerMateria", string.Empty, new { disabled = true })%>  
                 <%: Html.Label("Horas: ")%><%: Html.TextBox("VerHoras", string.Empty, new { disabled = true })%>  
                 <%: Html.Label("Situación de revista: ")%><%: Html.TextBox("VerSR", string.Empty, new { disabled = true })%> 
             </div>
        </fieldset>                       
</div>
<div id="divOtrasJurisdicciones" style="display:none;">
        <fieldset>
            <legend>Puesto de trabajo padre</legend>
             <%: Html.Label("Código de tipo cargo: ")%><%: Html.TextBox("VerCodigoTipoCargo", string.Empty, new { disabled = true })%>  
             <%: Html.Label("Nombre del tipo cargo: ")%><%: Html.TextBox("VerNombreTipoCargo", string.Empty, new { disabled = true })%>  
             <div id="div2" style="display:none;">
                 <%: Html.Label("Materia: ")%><%: Html.TextBox("VerMateria", string.Empty, new { disabled = true })%>  
                 <%: Html.Label("Horas: ")%><%: Html.TextBox("VerHoras", string.Empty, new { disabled = true })%>  
                 <%: Html.Label("Situación de revista: ")%><%: Html.TextBox("VerSR", string.Empty, new { disabled = true })%> 
             </div>
        </fieldset>                       
</div>
<div id="divEditable" >
 <%: Html.AbmcTextControlFor(model => model.FechaHasta , (int)EstadoABMC.Editar, Abmc.TextControl.TextBox) %>
 </div>
  <p />
 <p class="botones">
    <%:Html.BtnGenerico(Botones.ButtonType.button, "Aceptar", string.Empty, string.Empty, "btnAceptar")%>
    <%:Html.BtnGenerico(Botones.ButtonType.button, "Cancelar", string.Empty, string.Empty, "btnSalir")%>

</p>
<script type="text/javascript">
   $(document).ready(PuestoDeTrabajoProvisorio.Eliminar.init);
</script>   

