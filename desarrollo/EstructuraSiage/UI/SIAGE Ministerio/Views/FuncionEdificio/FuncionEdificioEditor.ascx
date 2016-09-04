<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.FuncionEdificioModel>" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;
%>
<fieldset>
    <legend><%: estadoText %> Función Edificio </legend>
    <%: Html.HiddenFor(model => model.Id) %>
    <%--<%: Html.HiddenFor(model => model.Version) %>--%>
    <%: Html.AbmcTextControlFor(model => model.Nombre, estadoId, Abmc.TextControl.TextBox) %>
    <%: Html.AbmcTextControlFor(model => model.Descripcion, estadoId, Abmc.TextControl.TextArea) %>
    <% //Cuando elimino debo seleccionar el motivo
    
    if(estadoId == (int)EstadoABMC.Eliminar) {%> 
        <%: Html.AbmcTextControlFor(model => model.MotivoBaja, estadoId, Abmc.TextControl.DropDownListEnum) %>
        <script type="text/javascript">
            $("#MotivoBaja").attr("disabled", false);
            $("#MotivoBaja").addClass("val-Required");
        </script>
    <% } %>

    <p><%: Html.AjaxAbmcBotones(estadoId) %></p>
</fieldset>