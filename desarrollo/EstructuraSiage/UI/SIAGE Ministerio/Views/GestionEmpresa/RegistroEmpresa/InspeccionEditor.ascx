<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaRegistrarModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Consultar";
    int estadoId = (estadoText != "Consultar") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Consultar;

    var inspeccionIntermedia =
        Html.CreateSelectList<TipoInspeccionIntermediaComboModel>(
            Siage.Base.ViewDataKey.INSPECCION_INTERMEDIA.ToString(), "Id", "Descripcion",
            Model.TipoInspeccionIntermediaId != null ? Model.TipoInspeccionIntermediaId : -1);
   
    using (Html.BeginForm())
    {
%>

<fieldset>

    <div id="divTipoInspeccionLista" style="display:none;">
        <%:Html.AbmcSelectControlFor(model => model.TipoInspeccion, estadoId, Abmc.SelectControl.DropDownList, inspeccionIntermedia)%>
    </div>
 

  <div id="divTipoInspeccionEnum"  style="display:none;">
        <%:Html.AbmcTextControlFor(model => model.TipoInspeccionEnum, estadoId, Abmc.TextControl.DropDownListEnum)%>
    </div>
   

    <%: Html.BtnGenerico(Botones.ButtonType.button, "Empresa supervisora", string.Empty, string.Empty, "btnEmpresaSupervisora") %>

    <div id="divSeleccionEmpresaSupervisora" style="display: none;">
        <% Html.RenderPartial("ConsultarEmpresaEditor"); %>
    </div>
    <div id="divMensajeEmpresaSupervisora" style="display: none;">
        <b><%:Html.Label("Seleccionar tipo de inspección antes de este paso")%></b>
    </div>
    <div style="clear: both;">
    <table id="listEmpresasInspeccionadas"  cellpadding="0" cellspacing="0" width="50%"></table>
    <div id="pagerEmpresasInspeccionadas"></div>
    
    </div>
</fieldset>
<% } %> 