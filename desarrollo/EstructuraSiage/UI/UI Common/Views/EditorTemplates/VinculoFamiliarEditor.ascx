<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.VinculoFamiliarModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;
%>
    <fieldset>
    
    <legend>Vinculo Familiar</legend>
    <div id="divPersonaFisicaVinculo">
        <% Html.RenderPartial("PersonaFisicaEditor", new PersonaFisicaModel());%>
    </div>
    <div id="divCamposVinculo">
    <%: Html.HiddenFor(model => model.Id) %>
    <% var tipoVinculos = Html.CreateSelectList<TipoVinculoModel>(Siage.Base.ViewDataKey.TIPO_VINCULO.ToString()); %>
    <%: Html.AbmcSelectControlFor(model => model.VinculoId, estadoId, Abmc.SelectControl.DropDownList, tipoVinculos)%>
    <%: Html.AbmcCheckControlFor(model => model.Vive, estadoId, Abmc.CheckControl.CheckBox)%>
    <%: Html.AbmcTextControlFor(model => model.Ocupacion, estadoId, Abmc.TextControl.TextBox)%>
    <%: Html.AbmcTextControlFor(model => model.Telefono, estadoId, Abmc.TextControl.TextBox)%>
    <%: Html.AbmcCheckControlFor(model => model.PermisoRetiro, estadoId, Abmc.CheckControl.CheckBox)%>
    </div>
    <p class="botones">
        <input id="btnAddVinculo" type="button" value="Aceptar" onclick="Vinculo.AgregarVinculo()" />
        <input id="btnCleanVinculo" type="button" value="Volver" onclick="Vinculo.CancelarVinculo()" />
    </p>
    <div id="divVinculoFamiliar">
    <table id="listaVinculos" cellpadding="0" cellspacing="0"></table>
    </div>
    </fieldset>    

    <script type="text/javascript">
        $(document).ready(function () {
            var pariente = PersonaFisica.init("#divPersonaFisicaVinculo", "Vinculos");
        });
    </script>
