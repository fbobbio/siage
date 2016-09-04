<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaReactivacionModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<% 
    
    var ordenDePago = Html.CreateSelectList<OrdenDePagoModel>(Siage.Base.ViewDataKey.ORDEN_DE_PAGO.ToString(), "Id", "LineaCombo", Model.OrdenDePago);
    var programaPresupuestario = Html.CreateSelectList<ProgramaPresupuestarioModel>(Siage.Base.ViewDataKey.PROGRAMA_PRESUPUESTARIO.ToString(), "Id", "LineaCombo", Model.ProgramaPresupuestario);
    
    using (Html.BeginForm())
    {
%>
    <div id="divReactivacionEmpresa">
    
        <%: Html.HiddenFor(model => model.Id)%>
    <fieldset>
        <div id="divVinculos">
            <fieldset>
                <legend>Vinculación Empresa a Edificio</legend>
                <p>
                    <%:Html.Label("Registrar nuevo vínculo de Empresa a Edificio:")%>
                    <%:Html.CheckBox("VincularEdificioCheck", false)%>
                </p>
                <div id="divVincularEdificioAEmpresa" style="display: none">
                    <% Html.RenderPartial("RegistroEmpresa/VinculoEmpresaEdificioEditor"); %>        
                </div>                    
            </fieldset>
        </div>

        <!-- Esto no va en Fase 1 ni Fase 2 Por lo que solo se agrega el check 
        <div id="divIngresarPaquetePresupuestario" >
        <fieldset>
            <legend>Paquete Presupuestario</legend> 
            <p>
                < %:Html.Label("Ingresar Paquete Presupuestario:")%>
                < %:Html.CheckBox("IngrearPaquetePresupuestarioCheck", false)%>
            </p>
            <div id="divAreaPaquetePresupuestario" style="display:none">
                 < % Html.RenderPartial("RegistrarPaquetePresupuestadoEditor"); %> 
            </div>
        </fieldset>
        </div> -->

        <div id="divInstrumentoLegal">
            <fieldset>
                <legend>Instrumento Legal</legend> 
                <p>
                    <%:Html.Label("Ingresar Instrumento Legal:")%>
                    <%:Html.CheckBox("InstrumentoLegalCheck", false)%>
                </p>
                <div id="divInstrumentoLegalEditor" style="display:none" >     
                    <% Html.RenderPartial("AsignacionInstrumentoLegalEditor", new AsignacionInstrumentoLegalModel());%>
                </div>
                <div id="divAsignacionInstrumentoLegal" style="display:none">
                  <fieldset>
                    <legend>Asignación del instrumento legal</legend>
                    <p>
                        <%:Html.Label("Fecha de Notificación:")%>
                        <%:Html.DateTimeTextBox("FechaNotificacionAsignacionInstrumentoLegal")%></p>
                    <p>
                        <%:Html.Label("Observaciones:")%>
                        <%:Html.TextArea("ObservacionesAsignacionInstrumentoLegal")%></p>
                  </fieldset>
                </div>
            </fieldset>
        </div>

            <div id="divSeleccionarEscuelaMadre" style="display:none" >
            <fieldset>
                <legend>Selección de Escuela Madre</legend>
                <div id="divGrillaEscuelaMadre" style="display:none;">
                    <% Html.RenderPartial("ConsultarEmpresaEditor"); %>
                </div>
                <%: Html.Label("Nombre Sugerido:") %> 
                <%: Html.TextBox("NombreSugerido", "")%>
                <%:Html.BtnGenerico(Botones.ButtonType.button, "Sugerir Nombre", string.Empty, string.Empty,
                                            "btnSugerirNombre")%>
            </fieldset>
            </div>
            
            <!-- Si NO es Escuela tengo que pedir que se cargue el programa presupuestario y la orden de pago -->
            <div id="divNoEsEscuela">
            <fieldset>
                <legend>Programa Presupuestario y Orden de Pago</legend>
                <%: Html.Label("Programa Presupuestario:") %> 
                <%: Html.DropDownList("ProgramaPresupuestario", programaPresupuestario, Constantes.Seleccione)%>

                <%: Html.Label("Orden de Pago:") %>
                <%: Html.DropDownList("OrdenDePago", ordenDePago, Constantes.Seleccione) %>
            </fieldset>
            </div>
            
            <div id="divEstructuraEscolar" style="display:none">
            <fieldset>
                <legend>Estructura Escolar</legend> 
                <p>
                    <%:Html.Label("Registrar Estructura Escolar:")%>
                    <%:Html.CheckBox("RegistrarEstructuraEscolarCheck", false)%>
                </p>
  
                <div id="divAreaEstructuraEscolar" style="display:none">
                <% if(Model.EstructuraEscolar != null)%> <% { %>
                        <%: Html.Editor("Model.EstructuraEscolar", "EstructuraEscuelaRegistrarEditor")%>
                <% } %>
                <% else{ %>
                       <%: Html.EditorFor(model => model.EstructuraEscolar) %>
                <% } %> 
                 </div> 
                <div id="grillaEstructura">
                <table id="listaEstructura" cellpadding="0" cellspacing="0"></table>
                </div> 
            </fieldset>
            </div>
            
            <div id="divPlanDeEstudios" style="display:none">
            <fieldset>
                <legend>Plan de Estudios</legend> 
                <p>
                    <%:Html.Label("Asignar Plan de Estudios:")%>
                    <%:Html.CheckBox("PlanDeEstudiosCheck", false)%>
                </p>
                <div id="divAreaPlanDeEstudios" style="display:none">
                    <div id="divPlanDeEstudiosSuperior" style="display:none">
                <%: Html.Label("divPlanDeEstudiosSuperior")%> 
                        <!--  < % Html.RenderPartial("AsignarPlanDeEstudioSuperiorEditor"); %> -->
                    </div>
                    <div id="divPlanDeEstudiosEscuela" style="display:none">
                <%: Html.Label("divPlanDeEstudiosEscuela")%> 
                        <!--  < % Html.RenderPartial("AsignarPlanDeEstudioEscuelaEditor"); %> -->
                    </div>
                </div>
            </fieldset>
            </div>
            
            <div id="divEmpresaInspeccion" style="display:none">
                <fieldset>
                    <legend>Asignar Empresa de Inspección</legend>
                    <%: Html.Label("Empresa de Inspección:") %> 
                    <div id="divConsultarEmpresaInspeccion" style="display:none;">
                        <% Html.RenderPartial("ConsultarEmpresaEditor"); %>
                    </div>
                </fieldset>
            </div>

      </fieldset>

        <p style="text-align:center">
            <%:Html.BtnGenerico(Botones.ButtonType.button, "Aceptar", string.Empty, string.Empty,
                                               "btnAceptar1")%>
            <%:Html.BtnGenerico(Botones.ButtonType.button, "Cancelar", string.Empty, string.Empty,
                                               "btnCancelar")%>
        </p>

        </div>

<%
    }
%>

<script type="text/javascript">
    $(document).ready(function () {
        ReactivacionEmpresa.init(Empresa.Registrar.consulta);
    });
   
</script>