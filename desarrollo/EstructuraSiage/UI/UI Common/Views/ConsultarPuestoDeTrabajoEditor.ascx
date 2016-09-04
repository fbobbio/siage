<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<%@ Import Namespace="Siage.Base" %>

<%
    var tiposDocumento = Html.CreateSelectList<TipoDocumentoModel>(Siage.Base.ViewDataKey.TIPO_DOCUMENTO.ToString());
    var nivelesEducativos = Html.CreateSelectList<NivelEducativoModel>(Siage.Base.ViewDataKey.NIVEL_EDUCATIVO.ToString());
    var situacionesRevista = Html.CreateSelectList<SituacionDeRevistaModel>(Siage.Base.ViewDataKey.SITUACION_REVISTA.ToString());
    var turnos = Html.CreateSelectList<TurnoModel>(Siage.Base.ViewDataKey.TURNO.ToString());
    var gradoAnios = Html.CreateSelectList<GradoAñoModel>(Siage.Base.ViewDataKey.GRADO_ANIO.ToString());
%>

<div id="divConsulta">
    <div id="divPTFiltroBasico">
        <fieldset>
            <legend>Filtro Básico</legend>

            <%-- Inicio Filtros Básicos--%>

            <p><%: Html.Label("Código de tipo cargo: ") %><%: Html.TextBox("fltCodigoTipoCargo") %></p>
            <p><%: Html.Label("Nombre de tipo cargo: ") %><%: Html.TextBox("fltNombreTipoCargo") %></p>
            <p><%: Html.Label("Código de agrupamiento: ") %><%: Html.TextBox("fltCodigoAgrupamiento") %></p>
            <p><%: Html.Label("Código de nivel de cargo: ") %><%: Html.TextBox("fltCodigoNivelCargo") %></p>
            <p><%: Html.Label("Estado de puesto de trabajo: ") %><%: Html.DropDownListEnum("fltEstadoPuestoDeTrabajo", typeof(EstadoPuestoDeTrabajoEnum))%></p>
            <p><%: Html.Label("Nombre de asignatura: ") %><%: Html.TextBox("fltNombreAsignatura") %></p>
            <p><%: Html.Label("Tipo de documento: ") %><%: Html.DropDownList("fltTipoDocumento", tiposDocumento, Constantes.Seleccione)%></p> 
            <p><%: Html.Label("Número de documento: ") %><%: Html.TextBox("fltNumeroDocumento") %></p>
            <p><%: Html.Label("Tipo de agente: ") %><%: Html.TextBox("fltTipoAgente") %></p>
            <p><%: Html.Label("Código de posición PN: ")%><%: Html.TextBox("fltCodigoPN", string.Empty, new { maxlength = 9 }) %></p>        

            <%-- Fin Filtros Básicos--%>

            <div class="botones">
                <button type="button" id="btnFiltroAvanzado">Filtro Avanzado</button>
                <button type="button" id="btnConsultarBasico">Consultar</button>
                <button type="button" id="btnLimpiarBasico">Limpiar</button>
            </div>
        </fieldset>
    </div>

    <div id="divPTFiltroAvanzado" style="display:none;">
        <fieldset>
            <legend>Filtro Avanzado</legend>

            <%-- Inicio Filtros Avanzado--%>

            <%-- Inicio Filtros Empresa--%>
            <fieldset>
                <legend>Empresa</legend>
                
                <p><%: Html.Label("CUE: ")%><%: Html.TextBox("fltCUE") %></p> 
                <p><%: Html.Label("Código de empresa: ")%><%: Html.TextBox("fltCodigoEmpresa") %></p>
                <p><%: Html.Label("Nombre de empresa: ")%><%: Html.TextBox("fltNombreEmpresa") %></p> 
                <p><%: Html.Label("Estado de empresa: ")%><%: Html.DropDownListEnum("fltEstadoEmpresa", typeof(EstadoEmpresaEnum)) %></p> 
                <p><%: Html.Label("Tipo de empresa: ")%><%: Html.DropDownListEnum("fltTipoEmpresa", typeof(TipoEmpresaEnum)) %></p> 
                <p><%: Html.Label("Escuela: ")%><%: Html.DropDownListEnum("fltEsEscuela", typeof(NoSiEnum)) %></p> 
                <p><%: Html.Label("Nivel educativo: ") %><%: Html.DropDownList("fltNivelEducativo", nivelesEducativos, Constantes.Seleccione)%></p> 
                <p><%: Html.Label("Nombre de programa presupuestado: ")%><%: Html.TextBox("fltProgramaPresupuestado") %></p>
            </fieldset>
            <%-- Fin Filtros Empresa--%>

            <%-- Inicio Filtros Puesto de Trabajo--%>
            <fieldset>
                <legend>Puesto de trabajo</legend>

                <p><%: Html.Label("Presupuestado: ")%><%: Html.DropDownListEnum("fltEsPresupuestado", typeof(NoSiEnum)) %></p> 
                <p><%: Html.Label("Liquidado: ")%><%: Html.DropDownListEnum("fltEsLiquidado", typeof(NoSiEnum)) %></p> 
                <p><%: Html.Label("Tipo de puesto: ") %><%: Html.DropDownListEnum("fltTipoPuesto", typeof(TipoPuestoEnum))%></p>
                <p><%: Html.Label("Estado de asignación: ")%><%: Html.DropDownListEnum("fltEstadoAsignacion", typeof(EstadoAsignacionEnum)) %></p> 
                <p><%: Html.Label("Situación de revista: ") %><%: Html.DropDownList("fltSituacionRevista", situacionesRevista, Constantes.Seleccione)%></p> 
                <p><%: Html.Label("Tipo de inspección: ") %><%: Html.DropDownListEnum("fltTipoInspeccion", typeof(TipoInscripcionEnum)) %></p>
                <p><%: Html.Label("Itinerante: ")%><%: Html.DropDownListEnum("fltEsItinerante", typeof(NoSiEnum)) %></p> 
            </fieldset>
            <%-- Fin Filtros Puesto de Trabajo--%>

            <%-- Inicio Filtros Asignatura--%>
            <fieldset>
                <legend>Asignatura</legend>
            
                <p><%: Html.Label("Código de asignatura: ")%><%: Html.TextBox("fltCodigoAsignatura") %></p> 
                <p><%: Html.Label("Especial: ")%><%: Html.DropDownListEnum("fltEsEspecial", typeof(NoSiEnum)) %></p> 
            </fieldset>
            <%-- Fin Filtros Asignatura--%>

            <%-- Inicio Filtros División--%>
            <fieldset>
                <legend>División</legend>

                <p><%: Html.Label("Turno: ") %><%: Html.DropDownList("fltTurno", turnos, Constantes.Seleccione)%></p>
                <p><%: Html.Label("Grado / Año: ") %><%: Html.DropDownList("fltGradoAnio", gradoAnios, Constantes.Seleccione)%></p>
                <p><%: Html.Label("División: ") %><%: Html.DropDownListEnum("fltDivision", typeof(DivisionEnum)) %></p> 
            </fieldset>
            <%-- Fin Filtros División--%>

            <%-- Inicio Filtros Carrera--%>
            <fieldset>
                <legend>Carrera</legend>
               
                <p><%: Html.Label("Nombre de carrera: ")%><%: Html.TextBox("fltNombreCarrera") %></p> 
            </fieldset>            
            <%-- Fin Filtros Carrera--%>

            <%-- Fin Filtros Avanzado--%>

            <div class = "botones">
                <button type="button" id="btnFiltroBasico">Filtro Básico</button>
                <button type="button" id="btnConsultarAvanzado">Consultar</button>
                <button type="button" id="btnLimpiarAvanzado">Limpiar</button>
            </div>
        </fieldset>
    </div>

    <table id="listPuestoDeTrabajo"></table>
    <div id="pagerPuestoDeTrabajo"></div>
</div>


<div id="divVista" style="display:none;">
    <%: Html.Hidden("Id") %>
    <%: Html.Label("Código de cargo: ") %><%: Html.TextBox("VerCodigoCargo", string.Empty, new { disabled = true }) %>
    <%: Html.Label("Nombre de cargo: ") %><%: Html.TextBox("VerNombreCargo", string.Empty, new { disabled = true }) %>
    <%: Html.Label("Horas: ") %><%: Html.TextBox("VerHoras", string.Empty, new { disabled = true }) %>
    <%: Html.Label("Plan de estudio: ") %><%: Html.TextBox("VerPlanEstudio", string.Empty, new { disabled = true }) %>
    <%: Html.Label("Materia: ") %><%: Html.TextBox("VerMateria", string.Empty, new { disabled = true }) %>
    <%: Html.Label("Turno: ") %><%: Html.TextBox("VerTurno", string.Empty, new { disabled = true }) %>
    <%: Html.Label("Grado / Año: ") %><%: Html.TextBox("VerGradoAnio", string.Empty, new { disabled = true }) %>
    <%: Html.Label("Sección / División: ") %><%: Html.TextBox("VerSeccionDivision", string.Empty, new { disabled = true }) %>
    <%: Html.Label("CUPOF: ") %><%: Html.TextBox("VerCupof", string.Empty, new { disabled = true }) %>

    <p class="botones"><button id="btnVolver" type="button">Volver</button></p>
</div>
