<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.AgenteModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;

    var sexos = Html.CreateSelectList<SexoModel>(Siage.Base.ViewDataKey.SEXO.ToString(), "Id", "TipoSexo");
    var estadosCiviles = Html.CreateSelectList<EstadoCivilModel>(Siage.Base.ViewDataKey.ESTADO_CIVIL.ToString());
    var tiposDocumento = Html.CreateSelectList<TipoDocumentoModel>(Siage.Base.ViewDataKey.TIPO_DOCUMENTO.ToString());
    var paises = Html.CreateSelectList<PaisModel>(Siage.Base.ViewDataKey.PAIS.ToString());
%>

<fieldset>
    <legend>Agente</legend>
    <%: Html.HiddenFor(model => model.Id) %>
     <%: Html.AbmcSelectControlFor(model => model.Persona.TipoDocumento, estadoId,Abmc.SelectControl.DropDownList, tiposDocumento) %>
    <%: Html.AbmcTextControlFor(model => model.Persona.NumeroDocumento, estadoId, Abmc.TextControl.TextBox)%>
     <%: Html.AbmcTextControlFor(model => model.Persona.Apellido, estadoId, Abmc.TextControl.TextBox)%>
    <%: Html.AbmcTextControlFor(model => model.Persona.Nombre, estadoId, Abmc.TextControl.TextBox)%>
    <%: Html.AbmcSelectControlFor(model => model.Persona.Sexo, estadoId, Abmc.SelectControl.DropDownList, sexos)%> 
    <%: Html.AbmcSelectControlFor(model => model.Persona.EstadoCivil, estadoId,Abmc.SelectControl.DropDownList, estadosCiviles) %>
    <%: Html.AbmcTextControlFor(model => model.Persona.FechaNacimiento, estadoId, Abmc.TextControl.Calendar) %>        
    <%: Html.AbmcSelectControlFor(model => model.Persona.IdPaisNacionalidad, estadoId, Abmc.SelectControl.DropDownList, paises) %>       
    <%: Html.HiddenFor(model => model.Persona.IdPaisEmisorDocumento) %>
   <%: Html.HiddenFor(model=>model.NumLegajoSiage)%>   
    <div id="divDomicilioPersonaFisica">
        <%--<%: Html.EditorFor(model => model.Persona.Domicilio) %>--%>
    </div>
</fieldset>


    