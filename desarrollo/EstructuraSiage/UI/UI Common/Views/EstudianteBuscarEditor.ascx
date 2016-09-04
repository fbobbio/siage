<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    var sexos = Html.CreateSelectList<SexoModel>(Siage.Base.ViewDataKey.SEXO.ToString(), "Id", "TipoSexo");
%>

<fieldset>
    <legend>Estudiante</legend>

    <div id="divConsultarEstudiante">        
        <p><%: Html.Label("Número de documento - Sexo: ")%><%: Html.RadioButton("radioButton", "filtroDS", true, new { id = "radioFiltro1" })%></p>
        <p><%: Html.Label("Nombre - Apellido: ")%><%: Html.RadioButton("radioButton", "filtroNA", false, new { id = "radioFiltro2" })%></p>

        <div id="divFiltro1">
            <p><%: Html.Label("Número Documento:") %> <%: Html.TextBox("FiltroDniEstudianteConsultar")%></p>
            <p><%: Html.Label("Sexo:") %> <%: Html.DropDownList("FiltroSexoEstudianteConsultar", sexos, Constantes.Seleccione)%></p>
        </div>
        
        <div id="divFiltro2" style="display: none;">
            <p><%: Html.Label("Nombre:") %> <%: Html.TextBox("FiltroNombreEstudianteConsultar")%></p>
            <p><%: Html.Label("Apellido:") %> <%: Html.TextBox("FiltroApellidoEstudianteConsultar")%></p>
        </div>

        <p class="botones">
            <input id="btnConsultarEstudiante" type="button" value="Aceptar" />
            <input id="btnLimpiarEstudiante" type="button" value="Limpiar" />
        </p>

        <div id="DivGrilla">
            <table id="listEstudiantes"></table>
            <div id="pagerEstudiantes"></div>
        </div>
    </div>

    <div id="divVistaEstudiantes" style="display: none;">
        <%: Html.Hidden("VerEstudiante_Id") %>
        <p><%: Html.Label("Nombre: ")%><%: Html.TextBox("VerEstudiante_Persona_Nombre", string.Empty, new { disabled = true })%></p>
        <p><%: Html.Label("Apellido: ")%><%: Html.TextBox("VerEstudiante_Persona_Apellido", string.Empty, new { disabled = true })%></p>
        <p><%: Html.Label("Sexo: ")%><%: Html.TextBox("VerEstudiante_Persona_SexoNombre", string.Empty, new { disabled = true })%></p>
        <p><%: Html.Label("Tipo documento: ")%><%: Html.TextBox("VerEstudiante_Persona_TipoDocumento", string.Empty, new { disabled = true })%></p>
        <p><%: Html.Label("Número documento: ")%><%: Html.TextBox("VerEstudiante_Persona_NumeroDocumento", string.Empty, new { disabled = true })%></p>
        <p><%: Html.Label("Edad: ")%><%: Html.TextBox("Edad", string.Empty, new { disabled = true })%></p>
        <p class="botones">
            <button id="btnCambiarEstudiante" type="button">Cambiar estudiante</button>
        </p>
    </div>
</fieldset>
