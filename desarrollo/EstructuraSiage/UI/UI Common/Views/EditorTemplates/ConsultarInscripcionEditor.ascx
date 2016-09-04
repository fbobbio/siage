<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;

    var sexos = Html.CreateSelectList<SexoModel>(Siage.Base.ViewDataKey.SEXO.ToString(), "Id", "TipoSexo");    
%>

<fieldset>
    <legend>Inscripto</legend>
    <div id="divConsultaEmpresaInscripcion">
        <% Html.RenderPartial("ConsultarEmpresaEditor"); %>
    </div>
    <div id="divBusquedaInscripciones">
        <div id="divFiltrosInscripcion">
            <fieldset>
                <legend>Filtros de búsqueda inscripciones</legend>
                <p><%: Html.Label("Número Documento:") %> <%: Html.TextBox("FiltroNroDocumentoEstudianteConsultarInscripcion")%></p>        
                <p><%: Html.Label("Sexo:") %> <%: Html.DropDownList("FiltroSexoEstudianteConsultarInscripcion", sexos, Constantes.Seleccione)%></p>
                <p><%: Html.Label("Grado / Año:") %> <%: Html.DropDownList("FiltroGradoAnioConsultarInscripcion", Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
                <p><%: Html.Label("Turno:") %> <%: Html.DropDownList("FiltroTurnoConsultarInscripcion", Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
                <p><%: Html.Label("División:") %> <%: Html.DropDownList("FiltroDivisionConsultarInscripcion", Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
                <p><%: Html.Label("Especialidad:") %> <%: Html.DropDownList("FiltroEspecialidadConsultarInscripcion", Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
                <p><%: Html.Label("Período de Inscripción Desde:") %> <%: Html.DateTimeTextBox("FiltroPeriodoInscripcionDesdeConsultarInscripcion")%></p>
                <p><%: Html.Label("Período de Inscripción Hasta:") %> <%: Html.DateTimeTextBox("FiltroPeriodoInscripcionHastaConsultarInscripcion")%></p>
                <p><%: Html.Label("Ciclo Lectivo:") %> <%: Html.TextBox("FiltroCicloLectivoConsultarInscripcion")%></p>
            </fieldset>
        </div>
        <p class="botones">
                <input id="btnConsultarInscripcion" type="button" value="Consultar" /> 
                <input id="btnLimpiarConsultarInscripcion" type="button" value="Limpiar" />
        </p>
    </div>
    <div id="divGrillaInscripciones">
        <table id="listInscripciones" cellpadding="0" cellspacing="0"></table>
        <div id="pagerInscripciones"></div>
    </div>
    <div id="divDatosInscripcion">
        <fieldset>
            <legend>Datos inscripción</legend>
            <p><%: Html.Hidden("VerInscripcion_Id")%></p>
            <p><%: Html.Label("Nombre: ")%><%: Html.TextBox("VerInscripcion_Persona_Nombre", string.Empty, new { disabled = true })%></p>
            <p><%: Html.Label("Apellido: ")%><%: Html.TextBox("VerInscripcion_Persona_Apellido", string.Empty, new { disabled = true })%></p>
            <p><%: Html.Label("Sexo: ")%><%: Html.TextBox("VerInscripcion_Persona_SexoNombre", string.Empty, new { disabled = true })%></p>
            <p><%: Html.Label("Tipo documento: ")%><%: Html.TextBox("VerInscripcion_Persona_TipoDocumento", string.Empty, new { disabled = true })%></p>
            <p><%: Html.Label("Número documento: ")%><%: Html.TextBox("VerInscripcion_Persona_NumeroDocumento", string.Empty, new { disabled = true })%></p>
            <p><%: Html.Label("Grado / Año:") %> <%: Html.TextBox("VerInscripcion_GradoAnio", string.Empty, new { disabled = true })%></p>
            <p><%: Html.Label("Turno:") %> <%: Html.TextBox("VerInscripcion_Turno", string.Empty, new { disabled = true })%></p>
            <p><%: Html.Label("División:") %> <%: Html.TextBox("VerInscripcion_Division", string.Empty, new { disabled = true })%></p>
            <p class="botones">
                <input id="btnCambiarInscripcion" type="button" value="Volver" /> 
            </p>
        </fieldset>
    </div>
</fieldset>