<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.HistorialesEmpresaModel>" %>

<%
    int estadoId = (int)EstadoABMC.Ver;
%>


<div id="divHistorialEmpresa" style="display:none">
    <fieldset>
        <legend>Historial</legend>
        <%: Html.HiddenFor(model => model.Id) %>
        <%: Html.AbmcTextControlFor(model => model.FechaModificacion, estadoId, Abmc.TextControl.TextBox) %>
        <%: Html.AbmcTextControlFor(model => model.NombreAgenteModificacion, estadoId, Abmc.TextControl.TextBox) %>
        <%: Html.AbmcTextControlFor(model => model.AsignacionInstrumentoLegal.FecNotificacion, estadoId, Abmc.TextControl.TextBox) %>
        <%: Html.AbmcTextControlFor(model => model.AsignacionInstrumentoLegal.InstrumentoLegal.NroInstrumentoLegal, estadoId, Abmc.TextControl.TextBox) %>
        <%: Html.AbmcTextControlFor(model => model.NombreEmpresa, estadoId, Abmc.TextControl.TextBox)%>
        <%: Html.AbmcTextControlFor(model => model.NombreEmpresaPadreOrganigrama, estadoId, Abmc.TextControl.TextBox) %>
        <%: Html.AbmcTextControlFor(model => model.TipoEmpresa, estadoId, Abmc.TextControl.DropDownListEnum)%>
        <%: Html.AbmcTextControlFor(model => model.Telefono, estadoId, Abmc.TextControl.TextBox)%>
        <%: Html.AbmcTextControlFor(model => model.Domicilio, estadoId, Abmc.TextControl.TextBox)%>
        <%: Html.AbmcTextControlFor(model => model.Observaciones, estadoId, Abmc.TextControl.TextBox)%>
        <%: Html.AbmcTextControlFor(model => model.FechaInicioActividades, estadoId, Abmc.TextControl.TextBox)%>
        <%: Html.AbmcTextControlFor(model => model.FechaNotificacion, estadoId, Abmc.TextControl.TextBox)%>
    

        <div id="divHistorialEscuela">
            <%: Html.AbmcTextControlFor(model => model.NumeroAnexo, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcCheckControlFor(model => model.Religioso, estadoId, Abmc.CheckControl.CheckBox)%>
            <%: Html.AbmcCheckControlFor(model => model.Arancelado, estadoId, Abmc.CheckControl.CheckBox)%>
            <%: Html.AbmcCheckControlFor(model => model.Albergue, estadoId, Abmc.CheckControl.CheckBox)%>
            <%: Html.AbmcTextControlFor(model => model.CUE, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcTextControlFor(model => model.HorarioDeFuncionamiento, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcCheckControlFor(model => model.ContextoDeEncierro, estadoId, Abmc.CheckControl.CheckBox)%>
            <%: Html.AbmcCheckControlFor(model => model.Hospitalaria, estadoId, Abmc.CheckControl.CheckBox)%>
            <%: Html.AbmcTextControlFor(model => model.NombreEscuelaMadre, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcCheckControlFor(model => model.EsRaiz, estadoId, Abmc.CheckControl.CheckBox)%>
            <%: Html.AbmcTextControlFor(model => model.NombreEscuelaRaiz, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcTextControlFor(model => model.EscuelaPrivada.RepresentanteLegal.Nombre, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcTextControlFor(model => model.TipoCooperadora, estadoId, Abmc.TextControl.DropDownListEnum)%>
            <%: Html.AbmcTextControlFor(model => model.TipoCategoria, estadoId, Abmc.TextControl.DropDownListEnum)%>
            <%: Html.AbmcTextControlFor(model => model.NombreZonaDesfavorable, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcTextControlFor(model => model.Ambito, estadoId, Abmc.TextControl.DropDownListEnum)%>
            <%: Html.AbmcTextControlFor(model => model.Dependencia, estadoId, Abmc.TextControl.DropDownListEnum)%>
            <%: Html.AbmcTextControlFor(model => model.NombreTipoEscuela, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcTextControlFor(model => model.TipoEducacionEscuela, estadoId, Abmc.TextControl.DropDownListEnum)%>
            <%: Html.AbmcTextControlFor(model => model.NombreModalidadJornada, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcTextControlFor(model => model.NombrePeriodoLectivo, estadoId, Abmc.TextControl.TextBox)%>
            <%--: Html.AbmcSelectControlFor(model => model.Turnos, estadoId, Abmc.SelectControl.List, new SelectList(Model.Turnos, "Id", "Nombre")) --%>
        </div>

        <div id="divHistorialInspeccion">
            <%: Html.AbmcTextControlFor(model => model.NombreInspeccion, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcTextControlFor(model => model.NombreInspeccionSupervisa, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcTextControlFor(model => model.TipoInspeccion, estadoId, Abmc.TextControl.DropDownListEnum)%>
            <%: Html.AbmcTextControlFor(model => model.NombreTipoInspeccionIntermedia, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcTextControlFor(model => model.NombreInspeccionAsignacion, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcTextControlFor(model => model.NombreEscuelaAsignacion, estadoId, Abmc.TextControl.TextBox)%>
        </div>

        <div id="divHistorialDireccionDeNivel">
            <%: Html.AbmcTextControlFor(model => model.Sigla, estadoId, Abmc.TextControl.TextBox)%>
            <%: Html.AbmcTextControlFor(model => model.TipoEducacionDN, estadoId, Abmc.TextControl.DropDownListEnum)%>
            <%--: Html.AbmcSelectControlFor(model => model.NivelesEducativo, estadoId, Abmc.SelectControl.List, new SelectList(Model.NivelesEducativo, "Id", "Nombre")) %>
            <%: Html.AbmcSelectControlFor(model => model.TipoEscuelaACrear, estadoId, Abmc.SelectControl.List, new SelectList(Model.TipoEscuelaACrear, "Id", "Nombre")) --%>
            <%-- Faltan Niveles Educativos y Tipo Escuelas a crear --%>
        </div>
    </fieldset>
</div>

<script type="text/javascript">
    $(document).ready(function () {
           
    });
</script>