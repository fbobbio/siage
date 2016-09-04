<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.InstrumentoLegalModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%  
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;

    var tipos = Html.CreateSelectList<TipoInstrumentoLegalModel>(Siage.Base.ViewDataKey.TIPO_INSTRUMENTO_LEGAL.ToString());
%>

<fieldset>
    <legend>Instrumento legal</legend>
        
    <div id="divConsulta">
		<p><%: Html.Label("Número: ") %><%: Html.TextBox("FiltroNumeroIL", string.Empty, new { maxlength = 14 }) %></p>
		
		<p class="botones">
            <button type="button" id="btnBuscar" >Buscar</button>
            <button type="button" id="btnNuevo" >Nuevo</button>
        </p>
    </div>

    <div id="divVista" style="display:none;">
        <%: Html.HiddenFor(model => model.Id) %>
        <%: Html.HiddenFor(model => model.FechaAlta) %>
        <%: Html.AbmcTextControlFor(model => model.NroInstrumentoLegal, estadoId, Abmc.TextControl.TextBox)%>
        <%: Html.AbmcTextControlFor(model => model.FechaEmision, estadoId, Abmc.TextControl.Calendar) %>   
        <%: Html.AbmcSelectControlFor(model => model.IdTipoInstrumentoLegal, estadoId, Abmc.SelectControl.DropDownList, tipos)%>
        <%: Html.AbmcTextControlFor(model => model.EmisorInstrumentoLegal, estadoId, Abmc.TextControl.DropDownListEnum)%>
        <%: Html.AbmcTextControlFor(model => model.Observaciones, estadoId, Abmc.TextControl.TextArea)%>
		
        <p>
	        <%: Html.Label("Registrar expediente:") %>
	        <%: Html.CheckBoxFor(model => model.RegistrarExpediente)%>
        </p>
    
        <div id="divExpediente" style="display:none;">
            <%: Html.Partial("ExpedienteEditor", new ExpedienteModel()) %>
        </div>
         
		<p class="botones">
			<button type="button" id="btnBusqueda" >Buscar instrumento legal</button>
		</p>
    </div>
</fieldset>