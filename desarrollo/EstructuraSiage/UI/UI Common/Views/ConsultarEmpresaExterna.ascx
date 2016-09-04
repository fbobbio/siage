<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaExternaModel>" %>

<div id="divConsulta" >
    <fieldset>
        <legend id="lgdFiltroBusqueda">Empresa Externa</legend>

        <p><%: Html.Label("Nombre/Razón Social: ") %> <%: Html.TextBox("filtroNombre") %></p>
        <p><%: Html.Label("Cuit/Cuil: ") %> <%: Html.TextBox("filtroCuitCuil") %></p>
        <p><%: Html.Label("Tipo de Empresa: ") %> <%: Html.DropDownListEnum("filtroTipoEmpresa", typeof(Siage.Base.TipoEmpresaExternaEnum))%></p>
        <p><%: Html.Label("Incluir dados de baja: ") %> <%: Html.CheckBox("filtroEliminadas")%></p>
    
        <p class="botones">
            <button id="btnConsultar" type="button" >Consultar</button>
            <button id="btnLimpiar" type="button" >Limpiar</button>
        </p>
    </fieldset>

    <table id="list" cellpadding="0" cellspacing="0"></table>
    <div id="pager"></div>
</div>

<div id="divDatos" style="display:none;" >
    <fieldset>
        <legend>Empresa Externa</legend>
        <%: Html.Hidden("Id") %>
        <p><%: Html.Label("Nombre: ")%><%: Html.TextBox("Nombre", string.Empty, new { disabled = "disabled" })%></p>
        <p><%: Html.Label("Teléfono: ")%><%: Html.TextBox("Telefono", string.Empty, new { disabled = "disabled" })%></p>
        <p><%: Html.Label("E-mail: ")%><%: Html.TextBox("Email", string.Empty, new { disabled = "disabled" })%></p>
        <p><%: Html.Label("Tipo empresa: ")%><%: Html.TextBox("TipoEmpresaExterna", string.Empty, new { disabled = "disabled" })%></p>
        <p><%: Html.Label("Estado: ")%><%: Html.TextBox("Estado", string.Empty, new { disabled = "disabled" })%></p>

        <%--<div id="divDomicilio">
            <p>Poner aca el editor de domicilio</p>
        </div>--%>

        <p class="botones">
            <button id="btnVolver" type="button" >Volver</button>
        </p>
    </fieldset>
</div>