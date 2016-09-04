<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaRegistrarModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Consultar";
    int estadoId = (estadoText != "Consultar") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Consultar;

    var tipoEscuelasPermitidas = Html.CreateSelectList<TipoEscuelaModel>(
        Siage.Base.ViewDataKey.TIPO_ESCUELA_PERMITIDA_EMPRESA.ToString(), Model.TipoEscuela.HasValue ? Model.TipoEscuela.Value : -1);

    var nivelEducativoDireccion = Html.CreateSelectList<NivelEducativoModel>(Siage.Base.ViewDataKey.NIVEL_EDUCATIVO.ToString(), Model.NivelEducativoId.HasValue ? Model.NivelEducativoId.Value : -1);
    
    using (Html.BeginForm())
    {
%>
<fieldset>    

    <%-- Agrego una grilla de carga rápida para seleccionar los tipo de escuelas de la Direccion de nivel --%>
    <div id="divTipoEscuelas">
        <%: Html.AbmcSelectControlFor(model => model.TipoEscuela, estadoId, Abmc.SelectControl.DropDownList, tipoEscuelasPermitidas, Constantes.Seleccione)%>
        <p class="botones">
        <% if(estadoText!="Ver"){%>
            <%:Html.BtnGenerico(Botones.ButtonType.button, "Agregar", string.Empty, string.Empty, "btnAgregarTE")%>
            <%:Html.BtnGenerico(Botones.ButtonType.button, "Eliminar", string.Empty, string.Empty,"btnEliminarTE")%>
            <%}%>
        </p>
        <table id="listTE" cellpadding="0" cellspacing="0">
        </table>
        <br />
    </div>

    <%-- Agrego una grilla de carga rápida de las tuplas niveles educativo por tipo educación --%>
    <div id="divNivelEducativoPorTipoEducacion">
        <%: Html.AbmcTextControlFor(model => model.TipoEducacion, estadoId, Abmc.TextControl.DropDownListEnum) %>
        <%: Html.AbmcSelectControlFor(model => model.NivelEducativoId, estadoId, Abmc.SelectControl.DropDownList, nivelEducativoDireccion, Constantes.Seleccione)%>
        <p class="botones">
            <% if (estadoText != "Ver") {%>
                <%: Html.BtnGenerico(Botones.ButtonType.button, "Agregar", string.Empty, string.Empty, "btnAgregarNETE") %>
                <%: Html.BtnGenerico(Botones.ButtonType.button, "Eliminar", string.Empty, string.Empty, "btnEliminarNETE") %>
            <% } %>
        </p>
        <table id="listNETE" cellpadding="0" cellspacing="0">
        </table>
        <br />
    </div>

    <%: Html.AbmcTextControlFor(model => model.Sigla, estadoId, Abmc.TextControl.TextBox, "DireccionNivel")%>

</fieldset>
<% } %>