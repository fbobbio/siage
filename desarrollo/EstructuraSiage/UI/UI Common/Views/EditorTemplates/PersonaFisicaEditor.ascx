<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.PersonaFisicaModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;

    var estadosCivil = Html.CreateSelectList<EstadoCivilModel>(Siage.Base.ViewDataKey.ESTADO_CIVIL.ToString(), Model != null ? Model.EstadoCivil : string.Empty);
    var tiposDocumento = Html.CreateSelectList<TipoDocumentoModel>(Siage.Base.ViewDataKey.TIPO_DOCUMENTO.ToString(), Model != null ? Model.TipoDocumento : string.Empty);
    var sexos = Html.CreateSelectList<SexoModel>(Siage.Base.ViewDataKey.SEXO.ToString(), "Id", "TipoSexo", Model != null ? Model.Sexo : string.Empty);
    var organismoEmisor = Html.CreateSelectList<OrganismoEmisorDocumentoModel>(Siage.Base.ViewDataKey.ORGANISMO_EMISOR.ToString(), Model != null ? Model.OrganismoEmisorDocumento : string.Empty);
    var pais = Html.CreateSelectList<PaisModel>(Siage.Base.ViewDataKey.PAIS.ToString(), Model != null ? Model.IdPaisEmisorDocumento : string.Empty);
    var listaVacia = Html.CreateEmptySelectList(); 
%>

<div id="divConsultaPF">
    <fieldset>
        <legend>Buscar persona</legend>

        <%: Html.Hidden("txtId") %>
        <p><%: Html.Label("Tipo documento (*): ") %><%: Html.DropDownList("cmbTipoDocumento", tiposDocumento, Constantes.Seleccione) %></p>
        <p><%: Html.Label("Número documento (*): ") %><%: Html.TextBox("txtNumeroDocumento") %></p>
        <p><%: Html.Label("Sexo (*): ") %><%: Html.DropDownList("cmbSexo", sexos, Constantes.Seleccione) %></p>
        
        <p class="botones">
            <button id="btnBuscarPF" type="button">Buscar</button>
            <button id="btnNuevoPF" type="button">Nuevo</button>
        </p>
    </fieldset>
</div>


<div id="divFormularioPF" style="display:none;">
    <fieldset>
        <legend>Persona</legend>   
        <%: Html.HiddenFor(model => model.Id) %>
        <%: Html.CheckBox("EsDeRCivil")%>
        <%--<p><%: Html.Label("Indocumentado: ") %><%: Html.CheckBox("ckIndocumentado")%></p>--%>

        <%: Html.AbmcSelectControlFor(model => model.TipoDocumento, estadoId,Abmc.SelectControl.DropDownList, tiposDocumento) %>
        <%: Html.AbmcTextControlFor(model => model.NumeroDocumento, estadoId, Abmc.TextControl.TextBox) %>  
        <%: Html.AbmcTextControlFor(model => model.Apellido, estadoId, Abmc.TextControl.TextBox)%>
        <%: Html.AbmcTextControlFor(model => model.Nombre, estadoId, Abmc.TextControl.TextBox) %>
        <%: Html.AbmcTextControlFor(model => model.CUIL, estadoId, Abmc.TextControl.TextBox) %>
        <%: Html.AbmcTextControlFor(model => model.FechaNacimiento, estadoId, Abmc.TextControl.Calendar) %>
        <%: Html.AbmcSelectControlFor(model => model.EstadoCivil, estadoId,Abmc.SelectControl.DropDownList, estadosCivil) %>
        <%: Html.AbmcTextControlFor(model => model.Observaciones, estadoId, Abmc.TextControl.TextArea) %>
        <%: Html.AbmcSelectControlFor(model => model.OrganismoEmisorDocumento, estadoId, Abmc.SelectControl.DropDownList, organismoEmisor) %>
        <%--<%: Html.AbmcTextControlFor(model => model.Clase, estadoId, Abmc.TextControl.DropDownListEnum) %>--%>
        <%: Html.AbmcSelectControlFor(model => model.Sexo, estadoId, Abmc.SelectControl.DropDownList, sexos)%>
        <%: Html.AbmcSelectControlFor(model => model.IdPaisEmisorDocumento, estadoId, Abmc.SelectControl.DropDownList, pais) %>
        <%: Html.AbmcSelectControlFor(model => model.IdPaisNacionalidad, estadoId, Abmc.SelectControl.DropDownList, pais) %>
        <%: Html.AbmcSelectControlFor(model => model.IdPaisOrigen, estadoId,Abmc.SelectControl.DropDownList, pais) %>
        <%: Html.AbmcSelectControlFor(model => model.ProvinciaNacimiento, estadoId, Abmc.SelectControl.DropDownList, listaVacia) %>
        <%: Html.AbmcSelectControlFor(model => model.DepartamentoProvincialNacimiento, estadoId, Abmc.SelectControl.DropDownList, listaVacia) %>
        <%: Html.AbmcSelectControlFor(model => model.LocalidadNacimiento, estadoId, Abmc.SelectControl.DropDownList, listaVacia) %>
    
        <div id="divDomicilioCheckBox" style="display: none;">
            <p><%: Html.Label("Registrar domicilio: ") %><%: Html.CheckBox("ckRegistrarDomicilioPF") %></p>
        </div>
    
        <div id="divDomicilioPF">
            <%: Html.EditorFor(model => model.Domicilio) %>
        </div>
        <% if(estadoId != 1 && estadoId != 4 && estadoId !=6)
           {%>
        <p class="botones">
            <button type="button" id="btnLimpiarPF">Limpiar</button>
            <button type="button" id="btnEditarPF">Editar</button>
        </p>
        <%
           }%>
    </fieldset>
</div>