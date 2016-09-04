<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaExternaModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;
    var condicionIva = Html.CreateSelectList<CondicionIvaModel>(Siage.Base.ViewDataKey.CONDICION_IVA.ToString(), "Id", "Nombre");
%>
<%--
<fieldset>
    <legend><%: estadoText %>Empresa Externa</legend>--%>
  <%--  <%: Html.HiddenFor(model => model.Id) %>   --%>

    <%-------------------------------------- INICIO AREA EDITABLE --------------------------------------%>
      
    <div id="tabs1">
        <ul>
            <li id="uno"><a href="#divGestionarPersona"><span>Persona</span></a></li> 
            <li ><a href="#divDatosEmpresaExterna"><span>Empresa Externa</span></a></li>                               
        </ul>                  
       
    <div id="divGestionarPersona">
     <p><%: Html.Label(" Persona Fisica:") %><input id="RadioPersonaFisica" name="persona" value="personaFisica" type="radio" /></p> 
     <p><%: Html.Label("Persona Juridica:") %><input id="RadioPersonaJuridica" name="persona" value="personaJuridica" type="radio" /></p> 

      <div id="divPrincipalPersonaFisica" style="display:none">
      <div id="divConsultarEmpresaExternaPF" style="display:block">
      <fieldset>
            <legend>Consultar Persona Física Asociada a una Empresa</legend>
            <p> <%: Html.AbmcTextControlFor(model =>model.Cuil, estadoId, Abmc.TextControl.TextBox)%> </p>
            <input id="botonConsultarEmpresaExternaPF" name="persona" value="Buscar" type="button" /> 
      </fieldset>
      </div>

      <div id="divPersonaFisicaEE" style="display:none">
        <fieldset>
            <legend>Gestionar Persona Física</legend>
            <%: Html.EditorFor(model => model.Referente)%>
        </fieldset>
      </div>

      </div>

      <div id="divPrincipalPersonaJuridica" style="display:none">
      
      <div id="divConsultarEmpresaExternaPJ" style="display:block">
      <fieldset>
            <legend>Consultar Persona Juridica Asociada a una Empresa</legend>
            <p> <%: Html.AbmcTextControlFor(model =>model.Cuit, estadoId, Abmc.TextControl.TextBox)%> </p>
            <input id="botonConsultarEmpresaExternaPJ" name="persona" value="Buscar" type="button" /> 
       </fieldset>
      </div>

      <div id="divPersonaJuridicaYreferenteEmpresa" style="display:none;"> 
        <fieldset>
            <legend>Gestionar Persona Juridica y Referente de la Empresa</legend>
                <div id="divPersonaJuridica">
                <fieldset>
                <legend>Persona Juridica</legend>
                 <%: Html.EditorFor(model => model.PersonaJuridica)%> 
                </fieldset>
                </div>
        
            <div id="divPersonaReferenteEmpresa">
            <fieldset>
            <legend>Referente de la Empresa</legend>
            <%: Html.EditorFor(model => model.ReferenteEmpresa)%>
            </fieldset>
        </div>
        </fieldset>
            </div>


      </div>



    </div>

   <div id="divDatosEmpresaExterna">
   <fieldset>
            <legend>Gestionar Empresa Externa</legend>
            <%: Html.HiddenFor(model => model.Id) %>
            <p> <%: Html.AbmcTextControlFor(model =>model.Nombre, estadoId, Abmc.TextControl.TextBox)%> </p>
            <p> <%: Html.AbmcTextControlFor(model =>model.Sucursal, estadoId, Abmc.TextControl.TextBox)%> </p>
            <p> <%: Html.AbmcTextControlFor(model =>model.NumeroAnses, estadoId, Abmc.TextControl.TextBox)%> </p>
            <p> <%: Html.AbmcTextControlFor(model =>model.NumeroIngBrutos, estadoId, Abmc.TextControl.TextBox)%> </p>
            <p> <%: Html.AbmcTextControlFor(model =>model.Telefono, estadoId, Abmc.TextControl.TextBox)%> </p>
            <p> <%: Html.AbmcTextControlFor(model =>model.Fax, estadoId, Abmc.TextControl.TextBox)%> </p>
            <p> <%: Html.AbmcTextControlFor(model =>model.Email, estadoId, Abmc.TextControl.TextBox)%> </p>
            <p> <%: Html.AbmcTextControlFor(model =>model.Descripcion, estadoId, Abmc.TextControl.TextBox)%> </p>
            <p> <%: Html.AbmcTextControlFor(model =>model.Observaciones, estadoId, Abmc.TextControl.TextArea)%> </p>
            <%  if (estadoText == "Eliminar" || estadoText == "Reactivar")
            {%>
             <p><%: Html.AbmcTextControlFor(model =>model.MotivoBaja, estadoId, Abmc.TextControl.TextArea)%></p>
    
             <%  }%>

             <div id="divDatosAdicionlesEmpresaExterna" style="display:block;">
        <fieldset>
          <legend>Datos Adicionales</legend>
            <p> <%: Html.AbmcSelectControlFor(model => model.CondicionIva, estadoId, Abmc.SelectControl.DropDownList, condicionIva)%></p>
            <p> <%: Html.AbmcTextControlFor(model => model.TipoEmpresaExterna, estadoId, Abmc.TextControl.DropDownListEnum)%></p>
            <p> <%: Html.AbmcTextControlFor(model =>model.FechaCreacion, estadoId, Abmc.TextControl.Calendar)%> </p>
            
            <div id="divAsignarDomicilioPF" style="display:block;">
            <%: Html.AbmcCheckControlFor(model => model.AsignarDomiPersonaF, estadoId, Abmc.CheckControl.CheckBox)%>
            </div>
            <div id="divAsignarDomicilioPJ" style="display:block;">
               <%: Html.AbmcCheckControlFor(model => model.AsignarDomiPersonaJ, estadoId, Abmc.CheckControl.CheckBox)%>
            </div>
            
            <div id="divRegistrarDomicilio" style="display:block;">
            <%: Html.AbmcCheckControlFor(model => model.RegistarDomicilioEmpresa, estadoId, Abmc.CheckControl.CheckBox)%>
           </div>
           
            <div id="divDomicilioEmpresaExterna" style="display:none;">
             <fieldset>
                  <legend>Registrar Domicilio Empresa Externa</legend>
                   <%: Html.EditorFor(model => model.Domicilio, Model != null? Model.Domicilio: new DomicilioModel())%>
            </fieldset>
            </div>



    </fieldset>
   
   </div>

     </div>
      </div> 
    <%---------------------------------------- FIN AREA EDITABLE ---------------------------------------%>
    
    <p><%: Html.AjaxAbmcBotones(estadoId) %></p>


<script type="text/javascript">
    $(document).ready(function () {
        EmpresaExterna.initEditor();
    });

</script>    




