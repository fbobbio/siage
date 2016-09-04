<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaRegistrarModel>" %>
<%@ Import Namespace="Siage.Base" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Consultar";
    int estadoId = (estadoText != "Consultar") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Consultar;

    var parametroJerarquíaDeInspecciónIgualAOrganigrama = (bool)ViewData[Siage.Base.ViewDataKey.JERARQUIA_DE_INSPECCION_IGUAL_A_ORGANIGRAMA.ToString()];
    var ordenDePago = Html.CreateSelectList<OrdenDePagoModel>(Siage.Base.ViewDataKey.ORDEN_DE_PAGO.ToString(), "Id", "LineaCombo");
    var programaPresupuestario = Html.CreateSelectList<ProgramaPresupuestarioModel>(Siage.Base.ViewDataKey.PROGRAMA_PRESUPUESTARIO.ToString(), "Id", "LineaCombo");
    
%>

<script>
   
    Empresa.Registrar.Escuela = {};    
    Empresa.Registrar.Escuela.Turnos = [];  
    Empresa.Registrar.Escuela.PeriodosLectivos = [];
    Empresa.Registrar.Escuela.EstructuraEscolar = [];
    Empresa.Registrar.DireccionDeNivel = {};
    Empresa.Registrar.DireccionDeNivel.TiposDeEscuelas = []; 
    Empresa.Registrar.DireccionDeNivel.NivelEducativo = [];
    Empresa.Registrar.Inspeccion = {};
    Empresa.Registrar.Inspeccion.TipoInspeccion = "";
    

    <% if(Model.EstructuraEscolar != null){ %>
        <% foreach (var estructura in Model.EstructuraEscolar) {%>
            Empresa.Registrar.Escuela.EstructuraEscolar.push({Id:"<%:estructura.Id%>",Carrera:"<%:estructura.Carrera%>",CarreraNombre:"<%:estructura.CarreraNombre%>",Turno:"<%:estructura.Turno%>",TurnoNombre:"<%:estructura.TurnoNombre%>",GradoAnio:"<%:estructura.GradoAnio%>",GradoAnioNombre:"<%:estructura.GradoAnioNombre%>",Division:"<%:estructura.Division%>",Cupo:"<%:estructura.Cupo%>",FechaApertura:"<%:estructura.FechaApertura.ToShortDateString()%>"});       
        <%} 
    }%> 

    <% if(Model.Turnos != null){ %>
        <% foreach (var turno in Model.Turnos) {%>
            Empresa.Registrar.Escuela.Turnos.push({Id:"<%:turno.Id%>",Nombre:"<%:turno.Nombre %>".replace(/&#209;/g, "Ñ")});        
        <%} 
    }%> 

    <% if(Model.PeriodosLectivos != null){ %>
        <% foreach (var periodo in Model.PeriodosLectivos) {%>
            Empresa.Registrar.Escuela.PeriodosLectivos.push({PeriodoLectivoId:"<%:periodo.Id%>",PeriodoLectivoText:"<%:periodo.Nombre %>"});        
        <%} %> 
    <% } %> 

    <% if(Model.NivelEducativoPorTipoEducacion != null){ %>
        <% foreach (var NeTe in Model.NivelEducativoPorTipoEducacion) {%>
            Empresa.Registrar.DireccionDeNivel.NivelEducativo.push({idNE:"<%:NeTe.NivelEducativo.Id %>",nivelEducativo:"<%:NeTe.NivelEducativo.Nombre %>",tipoEducacion:"<%:NeTe.TipoEducacion.ToString() %>"});        
       <% } %> 
   <% } %> 

    <% if(Model.TiposEscuelas != null){ %>
        <% foreach (var tipoEscuela in Model.TiposEscuelas) {%>
            Empresa.Registrar.DireccionDeNivel.TiposDeEscuelas.push({idTE:"<%:tipoEscuela.Id%>",tipoEscuela:"<%:tipoEscuela.Nombre %>"});        
        <%} %> 
    <%} %> 
    
    <% if(Model.TipoInspeccion.HasValue){ %>
        Empresa.Registrar.Inspeccion.TipoInspeccion ="<%: Model.TipoInspeccion*(-10) %>";
    <%} %> 

</script>
<fieldset>
    <legend id="legendEstadoEmpresa"><%:estadoText%> Empresa</legend>
    <%:Html.Hidden("parametroJerarquiaDeInspeccionIgualAOrganigrama",parametroJerarquíaDeInspecciónIgualAOrganigrama)%>    
    <%:Html.HiddenFor(model => model.Id)%> 
    <%:Html.HiddenFor(model => model.DomicilioId)%>
    <%:Html.HiddenFor(model => model.EmpresaPadreCod)%>
    <%:Html.HiddenFor(model => model.EmpresaInspeccionId)%>
    <%:Html.HiddenFor(model => model.EmpresaPadreOrganigramaId) %>
    <%:Html.HiddenFor(model => model.EmpresaInspeccionSupervisoraId)%>
    <%:Html.HiddenFor(model => model.EscuelaRaizId)%>
    <%:Html.HiddenFor(model => model.EscuelaMadreId)%>
    
    

    <%-------------------------------------- INICIO AREA EDITABLE --------------------------------------%>

    <div id="tabs">
        <ul>
            <li id="liSolapa1"><a href="#divSolapa1">Generales</a></li>
            <li id="liSolapa2"><a href="#divSolapa2">Edificios</a></li>
            <li id="liSolapa3"><a href="#divSolapa3">Instrumento legal</a></li>
            <li id="liSolapa4"><a href="#divSolapa4">Empresa padre</a></li>
            <li id="liSolapa5"><a href="#divSolapa5">Particulares</a></li>
        </ul>
            
        <div id="divSolapa1">
        <fieldset>
            <legend>Datos generales de empresa</legend>
            <div id="divFechaNotificacion">
                <%:Html.AbmcTextControlFor(model => model.FechaNotificacion, estadoId, Abmc.TextControl.Calendar)%>
            </div>

            <%: Html.AbmcTextControlFor(model=>model.FechaInicioActividades,estadoId,Abmc.TextControl.Calendar) %>
            <%: Html.AbmcTextControlFor(model => model.Telefono, estadoId, Abmc.TextControl.TextBox, VisualizacionEmpresaEnum.A.ToString())%>
            <%: Html.AbmcTextControlFor(model => model.Observaciones, estadoId, Abmc.TextControl.TextArea, VisualizacionEmpresaEnum.C.ToString())%>
            <%: Html.AbmcSelectControlFor(model => model.OrdenDePagoId, estadoId, Abmc.SelectControl.DropDownList, ordenDePago, Constantes.Seleccione)%>
            <%: Html.AbmcSelectControlFor(model => model.ProgramaPresupuestarioId, estadoId, Abmc.SelectControl.DropDownList, programaPresupuestario, Constantes.Seleccione)%>
                        
            <% if(estadoText == EstadoABMC.Ver.ToString()) { %>
                <%: Html.AbmcTextControlFor(model => model.EstadoEmpresa, estadoId, Abmc.TextControl.TextBox) %>
                <%: Html.AbmcTextControlFor(model => model.FechaAlta, estadoId, Abmc.TextControl.TextBox) %>
                <%: Html.AbmcTextControlFor(model => model.FechaCierre, estadoId, Abmc.TextControl.TextBox) %>
                <%: Html.AbmcTextControlFor(model => model.UsuarioCierre, estadoId, Abmc.TextControl.TextBox) %>

                <fieldset>
                    <legend>Empresa que registró</legend>
                    <%: Html.AbmcTextControlFor(model=>model.CodigoEmpresaQueRegistro,estadoId,Abmc.TextControl.TextBox) %>
                    <%: Html.AbmcTextControlFor(model => model.NombreEmpresaQueRegistro, estadoId, Abmc.TextControl.TextBox)%>
                </fieldset>
            <% } %>

            <%: Html.AbmcTextControlFor(model => model.Nombre, estadoId, Abmc.TextControl.TextBox) %>     
            <div id="divBotonSugerirNombreEscuela" style="display:none;">
                <p class="botones">
                    <% if(estadoText != EstadoABMC.Ver.ToString()) { %>
                        <%: Html.BtnGenerico(Botones.ButtonType.button, "Sugerir Nombre", string.Empty, string.Empty, "btnNombreSugerido")%>
                    <% } %>
                </p>
            </div>           

        </fieldset>
        </div>
    
        <div id="divSolapa2">                 
            <div id="divVinculos">
                <fieldset>
                    <legend>Vinculación Empresa a Edificio</legend>                    
                        <p>
                            <%: Html.Label("Registrar nuevo vínculo de empresa a edificio (*): ")%>
                            <%: Html.CheckBox("VincularEdificioCheck", false)%>
                        </p>
                    <% Html.RenderPartial("RegistroEmpresa/VinculoEmpresaEdificioEditor"); %>                    
                </fieldset>
            </div>            
        </div>

        <div id="divSolapa3">
            <fieldset>
                <legend>Asignación de instrumento legal</legend>
                <% if (estadoText != EstadoABMC.Ver.ToString() || Model.InstrumentosLegales != null) { %>
                    <div id='divCheckRegistrarInstrumento'>
                        <%: Html.Label("Registrar Instrumento Legal: ")%>
                        <%: Html.CheckBox("checkRegistrarInstrumento", false)%>
                    </div>
                <% } %>
                
                <div id="divAsignacionInstrumentoLegalGeneral" style="display: none;">
                    <div id="divInstrumentoLegalGeneral">
                        <% Html.RenderPartial("AsignacionInstrumentoLegalEditor", new AsignacionInstrumentoLegalModel());%>
                    </div>
                    <%:Html.AbmcTextControlFor(model => model.FecNotificacionAsignacionIL, estadoId, Abmc.TextControl.Calendar)%>
                </div>

                <% if (estadoText == EstadoABMC.Ver.ToString() || estadoText == EstadoABMC.Editar.ToString())
                  { %>  
                    <div id="divInstrumentoLegalListado" style="clear: both;">
                        <table id="listInstrumentosLegales" cellpadding="0" cellspacing="0" width="50%" />
                        <div id="pagerInstrumentoLegal"/>
                    </div>
                <% } %>

            </fieldset>
        </div>

        <div id="divSolapa4">
            <div id="divSeleccionEmpresaPadre">
                <% Html.RenderPartial("ConsultarEmpresaEditor"); %>            
            </div>
        </div>
        <div id="divSolapa5">
        <fieldset>
            <legend>Datos particulares de empresa</legend>
        <%
        switch (Model.TipoEmpresa)
        {
            case TipoEmpresaEnum.ESCUELA_ANEXO:
            case TipoEmpresaEnum.ESCUELA_MADRE: %>
                    <% Html.RenderPartial("RegistroEmpresa/TipoGestionEscuelaEditor");%>
                <%
            break;
            case TipoEmpresaEnum.INSPECCION: %>
                    <% Html.RenderPartial("RegistroEmpresa/InspeccionEditor");%>
                <%
                break;
            case TipoEmpresaEnum.DIRECCION_DE_NIVEL:
                %>
                    <% Html.RenderPartial("RegistroEmpresa/DireccionDeNivelEditor");%>
                <%
                break;
                      
            default:
                break;
        } %>
        </fieldset>
        </div>

        <div id="divInstrumentoLegalModificacion" />
    </div>
</fieldset>

<p class="botones">
    <%: Html.AjaxAbmcBotones(estadoId) %>    
</p>
<div id="divBtnVolver" style="display:none;">
    <p class="botones">    
        <%: Html.BtnGenerico(Botones.ButtonType.button, "Volver", string.Empty, string.Empty, "btnVolverEmpresa") %>   
    </p>
 </div>