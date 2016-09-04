<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaRegistrarModel>" %>

<div id="divDatosGeneralesEmpresa" style="display: none">
        <fieldset>
            <legend>Datos Generales</legend>
            <p>
                <%--:  Html.Hidden("Id")--%>
                <%: Html.HiddenFor(model => model.Id)%>
                </p>

            <p>
                <%: Html.Label("Código Empresa:") %>
            
                <%: Html.TextBox("VerCodigoEmpresa", string.Empty, new { disabled = "disabled" })%>
                
                </p>
            <p>
                <%: Html.Label("Nombre:") %>
                <%: Html.TextBox("VerNombreEmpresa", string.Empty, new { disabled = "disabled" })%></p>
            <p>
                <%: Html.Label("CUE:") %>
                <%: Html.TextBox("VerCueEmpresa", string.Empty, new { disabled = "disabled" })%>
                <%: Html.Label("-") %>
                <%: Html.TextBox("VerCueAnexoEmpresa", string.Empty, new { disabled = "disabled"} )%></p>
            <p>
                <%: Html.Label("Nivel Educativo:") %>
                <%: Html.TextBox("VerNivelEducativo", string.Empty, new { disabled = "disabled" })%></p>
            <p>
                <%: Html.Label("Tipo Educacion") %>
                <%: Html.TextBox("VerTipoEducacion",string.Empty ,  new { disabled = "disabled" })%></p>
            <p>
                <%: Html.Label("Tipo Empresa:") %>
                <%: Html.TextBox("VerTipoEmpresa", string.Empty, new { disabled = "disabled" })%></p>
            <p>
                <%: Html.Label("Estado Empresa:") %>
                <%: Html.TextBox("VerEstadoEmpresa", string.Empty, new { disabled = "disabled" })%></p>
        </fieldset>
    </div>