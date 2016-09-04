<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.DomicilioModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;

    var paises = Html.CreateSelectList<PaisModel>(Siage.Base.ViewDataKey.PAIS.ToString());
    var tipoCalle = Html.CreateSelectList<TipoCalleModel>(Siage.Base.ViewDataKey.TIPO_CALLE.ToString());

%>
<fieldset>
    <legend>
        <%: estadoText %>
        Domicilio</legend>
    <%: Html.HiddenFor(model => model.Id) %>
    <%-------------------------------------- INICIO AREA EDITABLE --------------------------------------%>
    
        <p>
            <label>País (*):</label>            
            <%: Html.DropDownList(Siage.Base.ViewDataKey.PAIS.ToString(), paises, Constantes.Seleccione) %></p>
        <div id="divCombos">
        <p>        
            <label>Provincia (*):</label>
             <%: Html.DropDownListFor(model => model.IdProvincia, Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
        <div id="divDepartamentoProvincial">
        <p>      
            <label>Departamento provincial:</label>
           
           <%: Html.DropDownListFor(model => model.IdDepartamentoProvincial, Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>   
        </div>
        <p>
            <label>Localidad (*):</label>
            <%: Html.DropDownListFor(model => model.IdLocalidad, Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
    
            <p>
            <label>Barrio (*):</label>
            <%: Html.DropDownListFor(model => model.IdBarrio, Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
            <%: Html.AbmcSelectControlFor(model => model.IdTipoCalle, estadoId,Abmc.SelectControl.DropDownList, tipoCalle) %>  
            <p>
            <label>Calle (*):</label>
            <%: Html.DropDownListFor(model => model.IdCalle, Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
            </div>
    <%: Html.AbmcTextControlFor(model => model.Altura, estadoId, Abmc.TextControl.TextBox) %>
    <%: Html.AbmcTextControlFor(model => model.Piso, estadoId, Abmc.TextControl.TextBox)%>
    <%: Html.AbmcTextControlFor(model => model.Departamento, estadoId, Abmc.TextControl.TextBox) %>
    <%: Html.AbmcTextControlFor(model => model.Torre, estadoId, Abmc.TextControl.TextBox)%>
    <%---------------------------------------- FIN AREA EDITABLE ---------------------------------------%>
    <%: Html.ValidationSummary("La operacion no pudo completarse") %>
</fieldset>