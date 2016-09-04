<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.HistorialesEmpresaModel>" %>

<%
    int estadoId = (int)EstadoABMC.Ver;
%>

<table id="HistorialEmpresa" cellpadding="0" cellspacing="0"></table>

<div id="detalle">
    
</div>

<p>
    <%: Html.BtnGenerico(Botones.ButtonType.button, "Volver", string.Empty, string.Empty, "btnVolver") %>
</p>
