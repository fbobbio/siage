<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.RegistrarVinculoEmpresaEdificioModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;

    var funcionEdificio = Html.CreateSelectList<FuncionEdificioModel>(Siage.Base.ViewDataKey.FUNCION_EDIFICIO.ToString());
    var departamentoProvincial = Html.CreateSelectList<DepartamentoProvincialModel>(Siage.Base.ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString());
    var localidad = Html.CreateSelectList<LocalidadModel>(Siage.Base.ViewDataKey.LOCALIDAD.ToString());
%>
<fieldset>
       <legend><%: estadoText %> Vínculo empresa edificio</legend>
    <%: Html.HiddenFor(model => model.Id) %>

    <%-------------------------------------- INICIO AREA EDITABLE --------------------------------------%>
    <%---------------------------------------- Registrar ---------------------------------------%>
    <%-------------------------------------- SELECICON DE EMPRESA --------------------------------------%>
    
    <div id="divVinculoEmpresaEdificio">
        <fieldset>
            <legend>Selección Empresa</legend>
            <% Html.RenderPartial("ConsultarEmpresaEditor", new { vista = VistaEmpresa.SinVista, id = 0 }); %>
        </fieldset>
    </div>

    <%-------------------------------------- SELECCION MULTIPLE DE EDIFICIOS --------------------------------------%>
    <div id="divEdificio">
        <fieldset>
            <legend>Selección Edificio</legend>
            <div id="divConsultaEdificio">                
                
                <p><%: Html.Label("Tipo de edificio:") %> <%: Html.DropDownListEnum("TipoEdificioConsulta", typeof(Siage.Base.TipoEdificioEnum))%></p>
                <p><%: Html.Label("Identificador edificio:") %> <%: Html.TextBox("IdentificadorEdificioConsulta")%></p>
                <p><%: Html.Label("Funcion de edificio:") %> <%: Html.DropDownList("FuncionEdificioConsulta", funcionEdificio, Constantes.Seleccione)%></p>

                <p><%: Html.Label("Identificador predio:") %> <%: Html.TextBox("IdentificadorPredioConsultaEdificio")%></p>
                <p><%: Html.Label("Descripcion predio:") %> <%: Html.TextBox("DescripcionPredioConsultaEdificio")%></p>
                <p><%: Html.Label("Nombre casa habitación:") %> <%: Html.TextBox("NombreCasaHabitacionConsulta")%></p>

                <fieldset>
                <legend>Domicilio</legend>
                <p><%: Html.Label("Departamento:")%> <%: Html.DropDownList("FiltroDepartamentoProvincial", departamentoProvincial, Constantes.Seleccione)%>
                <p><%: Html.Label("Localidad:")%> <%: Html.DropDownList("FiltroLocalidad", localidad, Constantes.Seleccione)%>
                <p><%: Html.Label("Barrio:")%> <%: Html.DropDownList("FiltroBarrio", Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
                <p><%: Html.Label("Calle:")%> <%: Html.DropDownList("FiltroCalle", Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
                <p><%: Html.Label("Altura:")%> <%: Html.TextBox("FiltroAltura") %></p>
                </fieldset>      
                              
                <p><input id="btnConsultarEdificio" type="button" value="Consultar"/></p>
            </div>
            <table id="seleccionEdificio" cellpadding="0" cellspacing="0"></table>
            <div id="pagerEdificio"></div>
                        
        </fieldset>
    </div>
   <div id="divEdificioTemplate"></div>

    <div id="divVinculo">
    <%: Html.AbmcTextControlFor(model => model.FechaDesde, estadoId, Abmc.TextControl.TextBox) %>
    <%: Html.AbmcTextControlFor(model => model.Observacion, estadoId, Abmc.TextControl.TextArea) %>
    <%---------------------------------------- FIN AREA EDITABLE ---------------------------------------%>
   
    <%
        
        if (estadoId == (int)EstadoABMC.Ver || estadoId == (int)EstadoABMC.Eliminar)
        {%>
        <%: Html.AbmcTextControlFor(model => model.FechaHasta, (int)EstadoABMC.Ver, Abmc.TextControl.TextBox)%>  
    <%: Html.AbmcTextControlFor(model => model.Motivo, estadoId, Abmc.TextControl.TextBox)%>
    <%: Html.AbmcTextControlFor(model => model.Estado, (int)EstadoABMC.Ver, Abmc.TextControl.TextBox)%> 
    
    <% } %>
    <%---------------------------------------- FIN AREA EDITABLE ---------------------------------------%>
    </div>
    <p><%: Html.AjaxAbmcBotones(estadoId) %></p>
    
</fieldset>

<script type="text/javascript">
    $(document).ready(function () {
        //ConsultarEmpresa.init('SinVista', "#divVinculoEmpresaEdificio", "vincularEdificio", false);
    });
</script>

