<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.GrupoMabModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<style type="text/css">
<!--
#TablaGrupos ,#TablaGrupos td{
	border: 1px solid #000000;
	height: 15px;
}
.encabezado{
	background-color: #E1E1E1;
}
-->
</style>
<% string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Consultar"; 
 int estadoId = (estadoText != "Consultar") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Consultar;
    
 
 var EstadoAsignacionId = Html.CreateSelectList<EstadoAsignacionModel>("EstadosAsignacion", "Id", "Valor", Model.EstadoAsignacionId);
 var EstadoAnteriorPtId = Html.CreateSelectList<EstadoPuestoModel>("EstadosPuesto", "Id", "Valor", Model.EstadoAnteriorPtId);
 var EstadoPosteriorPtId = Html.CreateSelectList<EstadoPuestoModel>("EstadosPuesto", "Id", "Valor", Model.EstadoPosteriorPtId);

 var EstadoAsignacionAnteriorId = Html.CreateSelectList<EstadoAsignacionModel>("EstadosAsignacion", "Id", "Valor", Model.EstadoAsignacionAnteriorId);
 var EstadoAnteriorPtAnteriorId = Html.CreateSelectList<EstadoPuestoModel>("EstadosPuesto", "Id", "Valor", Model.EstadoAnteriorPtAnteriorId);
 var EstadoPosteriorPtAnteriorId = Html.CreateSelectList<EstadoPuestoModel>("EstadosPuesto", "Id", "Valor", Model.EstadoPosteriorPtAnteriorId);
%>

<div id="divRegistrarGrupoMab">
 <%:Html.HiddenFor(model => model.Id)%>
 
 <fieldset>
    <legend><%: estadoText %> Grupo Mab</legend>
   <% if (estadoText == "Ver" || estadoText == "Editar" || estadoText=="Eliminar") {%> 

<p><%:Html.Label("Número Grupo: "+Model.NumeroGrupo)%></p>
<%:Html.HiddenFor(model=>model.NumeroGrupo) %>

<%}%>
 <p><%:Html.Label("Tipo Grupo(*)")%><%:Html.DropDownListEnumFor(model => model.TipoGrupo)%></p>
  
    <p><%:Html.AbmcCheckControlFor(model=>model.GeneraPtp,estadoId,Abmc.CheckControl.CheckBox)%></p>
    <!-- una lista que muestre todos los codigosMovimientoMab, que no este asignado a un grupoMab   -->
    <div id="divCodigoDeMovimiento"></div>

    <!--  accion en puesto de trabajo  -->
    <fieldset>
   
        <legend>Acción en puesto de trabajo</legend>
        <fieldset>
         <%:Html.HiddenFor(model=>model.EjecucionEnPuestoTrabajoId) %>
        <%: Html.AbmcCheckControlFor(model=>model.ModificaEstadoAsignacionPuesto,estadoId,Abmc.CheckControl.CheckBox) %>
        <%:Html.AbmcSelectControlFor(model => model.EstadoAsignacionId, estadoId, Abmc.SelectControl.DropDownList, EstadoAsignacionId)%>
        </fieldset>
        <fieldset>
          <%: Html.AbmcCheckControlFor(model=>model.ModificaEstadoPosteriorPuesto,estadoId,Abmc.CheckControl.CheckBox) %>     
        <%:Html.AbmcSelectControlFor(model => model.EstadoPosteriorPtId, estadoId, Abmc.SelectControl.DropDownList,EstadoPosteriorPtId)%>
        </fieldset>
       
        
        <fieldset>
         <%: Html.AbmcCheckControlFor(model=>model.ModificaEstadoAnteriorPuesto,estadoId,Abmc.CheckControl.CheckBox) %> 
       <%:Html.AbmcSelectControlFor(model => model.EstadoAnteriorPtId, estadoId, Abmc.SelectControl.DropDownList, EstadoAnteriorPtId)%>
       
       
        <p id='P1'><input id='btnAgregarEstadoPT1' type='button' value='Agregar'/>  <input id='btnQuitarEstadoPT1'  type='button' value='Quitar'/></p>
       <table id="grillaEstadosAnterioresAccionesPuestoTrabajo"></table>
        </fieldset>
  
         <%: Html.AbmcCheckControlFor(model=>model.Liquidacion,estadoId,Abmc.CheckControl.CheckBox) %>
        <%: Html.AbmcCheckControlFor(model=>model.GeneraVacante,estadoId,Abmc.CheckControl.CheckBox) %>
        <%: Html.AbmcCheckControlFor(model=>model.ModificaSitRev,estadoId,Abmc.CheckControl.CheckBox) %>

       

    </fieldset>
<!--  accion en puesto de trabajo anterior -->
        <fieldset> 
        <legend>Acción en puesto trabajo anterior</legend>  
        <fieldset>
         <%:Html.HiddenFor(model=>model.EjecucionEnPuestoTrabajoAnteriorId) %>
            <%: Html.AbmcCheckControlFor(model=>model.ModificaEstadoAsignacionPuestoAnterior,estadoId,Abmc.CheckControl.CheckBox) %>
        <%:Html.AbmcSelectControlFor(model => model.EstadoAsignacionAnteriorId, estadoId, Abmc.SelectControl.DropDownList, EstadoAsignacionAnteriorId)%>
        </fieldset>
        <fieldset>
        <%: Html.AbmcCheckControlFor(model=>model.ModificaEstadoAnteriorPuestoAnterior,estadoId,Abmc.CheckControl.CheckBox) %>     
        <%:Html.AbmcSelectControlFor(model => model.EstadoAnteriorPtAnteriorId, estadoId, Abmc.SelectControl.DropDownList,EstadoAnteriorPtAnteriorId)%>
            <p id="botones"><input id="btnAgregarEstadoPT2" type="button" value="Agregar"/>  <input id="btnQuitarEstadoPT2" type="button" value="Quitar"/></p>
           <table id="grillaEstadosAnterioresAccionesPuestoTrabajoAnterior"></table>
        </fieldset>
     <fieldset>   
     <%: Html.AbmcCheckControlFor(model=>model.ModificaEstadoPosteriorPuestoAnterior,estadoId,Abmc.CheckControl.CheckBox) %>
     <%:Html.AbmcSelectControlFor(model => model.EstadoPosteriorPtAnteriorId, estadoId, Abmc.SelectControl.DropDownList,EstadoPosteriorPtAnteriorId)%>

     </fieldset>
        <%: Html.AbmcCheckControlFor(model=>model.LiquidacionAnterior,estadoId,Abmc.CheckControl.CheckBox) %>
        <%: Html.AbmcCheckControlFor(model=>model.GeneraVacanteAnterior,estadoId,Abmc.CheckControl.CheckBox) %>
        <%: Html.AbmcCheckControlFor(model=>model.ModificaSitRevAnterior,estadoId,Abmc.CheckControl.CheckBox) %>

       
    
    

<%--<% if (estadoText == "Ver")
{
    
    var table = "<table id='TablaGrupos' width='728' height='402' border='' cellpadding='0' cellspacing='0'><tr class='encabezado'><td width='147'><strong>Grupo N°</strong>" + Model.NumeroGrupo + "</td><td colspan='6'><strong>Tipo Grupo: </strong>" + Model.TipoGrupo.ToString() + "</td></tr><tr class='encabezado'><td colspan='7'><strong>generaPTP: </strong>" + (Model.GeneraPtp ? "Si" : "No") + "</td></tr><tr class='encabezado'><td><div align='center'><strong>Codigo</strong></div></td><td colspan='3'><div align='center'><strong>Concepto</strong></div></td><td colspan='3'><div align='center'><strong>Observaciones</strong></div></td></tr>";
    for (int index = 0; index < Model.CodigosMovimientoMab.Count; index++)
    {
        var codigoMov = Model.CodigosMovimientoMab[index];
        table += "<tr id='contenido'><td>" + codigoMov.Codigo + "</td><td colspan='3'>" + codigoMov.Descripcion +
                 "</td><td colspan='3'></td></tr>";
    }

            table+="<tr class='encabezado'><td>&nbsp;</td><td width='90'><div align='center'><strong>Estado Prvio PT</strong></div></td><td width='98'><div align='center'><strong>Estado Nuevo PT</strong></div></td><td width='85'><div align='center'><strong>Liquidado</strong></div></td><td width='69'><div align='center'><strong>Estado Asignación</strong></div></td><td width='88'><div align='center'><strong>Genera Vacante</strong></div></td><td width='185'><div align='center'><strong>Modifica situacion de revista</strong></div></td></tr><tr><td><strong>Puesto de trabajo</strong></td><td colspan='3'>&nbsp;</td><td colspan='3'>&nbsp;</td></tr><tr><td height='206'><strong>Puesto de Trabajo Anterior</strong></td><td colspan='3'>&nbsp;</td><td colspan='3'>&nbsp;</td></tr></table>";



        %>
        <%=table%>

        <%
        }%>  --%>     

    </fieldset>
    <fieldset>
        <legend>Códigos de Movimiento</legend>
        <div class="soloRegistro">
            <table id="gridCodigosMovimientoMab"></table>
            <div id="pager1"></div>
        </div>
        <p class="botones" id="btnAgregarQuitar"><input type="button" value="Agregar" id="btnAgregar" /><input type="button" value="Quitar" id="btnQuitar" /></p>
        <table id="gridCodigosMeMovimientoSeleccionados"></table>
        <div id="pager2"></div>
    </fieldset>
        <p><%: Html.AjaxAbmcBotones(estadoId) %></p>
</fieldset>
</div>
    
<script type="text/javascript">
    //codigos de movimiento Mab

    $(document).ready(function () {
        GrupoMab.init("<%:estadoText %>");
        //        GrupoMab.initGrillaCodigosMovimientoMab();
        //        GrupoMab.initCodigosMovimientoMabSeleccionados();
        //        GrupoMab.initBotonAgregarQuitar();

        //        if ("<%:estadoText %>" != "Registrar") {
        //            //$("#gridCodigosMovimientoMab").hide();
        //            $('.soloRegistro').hide();
        //        }
        //        if ("<%:estadoText %>" === "Ver") {
        //            $('#btnAgregar, #btnQuitar').hide();
        //            $('#divAbmc #TipoGrupo').attr('disabled', true);
        //        }

    });
</script>