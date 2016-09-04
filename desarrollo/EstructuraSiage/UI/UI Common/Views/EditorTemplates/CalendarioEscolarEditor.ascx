<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.CicloLectivoModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;
    var nivelEducativo = Html.CreateSelectList<NivelEducativoModel>(Siage.Base.ViewDataKey.NIVEL_EDUCATIVO.ToString(), Model.NivelEducativoId);
    var PeriodoLectivoList = Html.CreateSelectList<PeriodoLectivoModel>(Siage.Base.ViewDataKey.PERIODO_LECTIVO.ToString(), Model.PeriodoLectivoId);
    var aniosList = Html.CreateSelectList<ComboModel>(Siage.Base.ViewDataKey.ANIO.ToString(), Model.AñoCiclo);
%>
<fieldset>
    <legend><%: estadoText %> Calendario escolar</legend>
    <%: Html.HiddenFor(model => model.Id) %>
    <div id="CicloLectivo">
        <fieldset>
        <legend>Ciclo lectivo</legend>
            <%: Html.AbmcSelectControlFor(model => model.AñoCiclo, estadoId, Abmc.SelectControl.DropDownList, aniosList)%>
            <%: Html.AbmcTextControlFor(model => model.FechaInicio , estadoId, Abmc.TextControl.Calendar) %>
            <%: Html.AbmcTextControlFor(model => model.FechaFin , estadoId, Abmc.TextControl.Calendar) %>
            <%: Html.AbmcSelectControlFor(model => model.PeriodoLectivoId , estadoId, Abmc.SelectControl.DropDownList, PeriodoLectivoList )%>
            <%: Html.AbmcSelectControlFor(model => model.NivelEducativoId, estadoId, Abmc.SelectControl.DropDownList, nivelEducativo)%>
        </fieldset>
    </div>
    <br />

    <fieldset>
        <legend>Fechas</legend>
        <div id="divCamposFechasCalendario">
            <%: Html.Hidden("IdFecha")%>
            <p><%: Html.Label("Fecha inicio (*):") %> <%: Html.DateTimeTextBox ("fechaInicioCalendario")%></p>
            <p><%: Html.Label("Fecha fin (*):") %> <%: Html.DateTimeTextBox("fechaFinCalendario")%></p>
            <p><%: Html.Label("Hora :") %> <%: Html.TextBox("Hora")%></p>
            <div id="divOpcionesExcluyentes">
            <fieldset>
                <legend>Seleccione una opción (*):</legend>
                <p>
                    <label>Etapa</label>
                    <%: Html.RadioButton("rdb", false, new { id = "rdbEtapa" })%></p>
                <p>
                    <label>Proceso</label>
                    <%: Html.RadioButton("rdb", false, new { id = "rdbProceso" })%></p>
                    <p>
                    <label>Otro concepto</label>
                    <%: Html.RadioButton("rdb", false, new { id = "rdbOtroConcepto" })%></p>
            </fieldset>
            </div>
            <div id="divEtapa"  style="display:none;"> 
                <p><%: Html.Label("Etapa (*):") %> <%: Html.DropDownList("Etapa", Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
            </div>
            <div id="divProceso"  style="display:none;"> 
                <p><%: Html.Label("Proceso (*):") %> <%: Html.DropDownList("Proceso", Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
            </div>
            <div id="divConcepto"  style="display:none;"> 
                <p><%: Html.Label("Concepto (*):") %> <%: Html.TextArea("Concepto")%></p>
            </div>
            <p><%: Html.Label("Es Hábil:") %> <%: Html.CheckBox("EsHabil")%></p>
            <p><input id="btnAgregarFecha" type="button" value="Agregar" />
            <input id="btnLimpiarFecha" type="button" value="Limpiar" /></p>
        </div>
        <div id="divFechasCalendario" style="clear: both;">
           <table id="listFechas"></table>
        </div>
    </fieldset>
    <% if (estadoText != EstadoABMC.Ver.ToString() ) { %>
        <div id='divCheckRegistrarInstrumento'>
            <%: Html.Label("Registrar Instrumento Legal: ")%>
            <%: Html.CheckBox("checkRegistrarInstrumento", false)%>
        </div>
    <% } %>
    
    <%: Html.HiddenFor(model => model.AsignacionInstrumentoLegal.Id) %>
    <div id="divAsignacionInstrumentoLegal" style="display:none;">
        <% Html.RenderPartial("AsignacionInstrumentoLegalEditor", new AsignacionInstrumentoLegalModel()); %>
    </div>
    <p><%: Html.AjaxAbmcBotones(estadoId) %></p>
</fieldset>

<script type="text/javascript">
    $(document).ready(function () {
        CalendarioEscolar.EstadoEditor = "<%: estadoText %>";
        CalendarioEscolar.init();
    });
   
</script>