<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteLogueado.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Siage.Base" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server" />
<asp:Content ID="Content5" ContentPlaceHolderID="TituloContent" runat="server">Empresa</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    <div id="divConsulta" > 
        <% Html.RenderPartial("ConsultarEmpresaEditor"); %>
    </div>

    <% Html.BeginForm(); %>
        <div id="divAbmc" style="display: none;" />
    <% Html.EndForm(); %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.alphanumeric.pack.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.maskedinput-1.2.2.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.maxlength.js") %>" type="text/javascript"></script>

    <script src="<%: Url.Content("~/Scripts/jquery.cascadingDropDown.js") %>" type="text/javascript"></script>

    <script src="<%: Url.Content("~/Scripts/siage.abmc.formulario.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.abmc.grilla.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.abmc.util.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.validaciones.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.tmpl.min.js") %>" type="text/javascript"></script>    
    <%--<script src="<%: Url.Content("~/Scripts/jquery.alphanumeric.js") %>" type="text/javascript"></script> --%>
    <script src="<%: Url.Content("~/Scripts/jquery.cascadingDropDown.js") %>" type="text/javascript"></script>
    
    <script src="<%: Url.Content("~/Scripts/siage.grillas.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.grillas.util.js") %>" type="text/javascript"></script>

    <script src="<%: Url.Content("~/Scripts/siage.expediente.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.validaciones.js") %>" type="text/javascript"></script>

    <script src="<%: Url.Content("~/Scripts/siage.empresa.emitirResolucionPuestosDeTrabajo.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.resolucion.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.asignacionInstrumentoLegal.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.instrumentoLegal.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.expediente.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.agente.consultar.js") %>" type="text/javascript"></script>

    <script src="<%: Url.Content("~/Scripts/siage.empresa.consultar.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.empresa.historiales.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.empresa.cierreEmpresa.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.empresa.emitirResolucionPuestosDeTrabajo.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.empresa.reactivacionEmpresa.js") %>" type="text/javascript"></script>     
    <script src="<%: Url.Content("~/Scripts/siage.empresa.registrar2.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.empresa.vinculoEmpresaEdificio.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.empresa.modificarTipoEmpresa.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.empresa.activarCodigo.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.empresa.visarActivacionEmpresa.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.estructuraEscuelaRegistrar.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.personaFisica.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.domicilio.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.modificarInstrumentoLegalEmpresa.js") %>" type="text/javascript"></script>
        
    <script type="text/javascript">
        $(document).ready(function () {        

        Empresa.Registrar.TipoEmpresaUsuarioLogueado="<%: ViewData[ViewDataKey.TIPO_EMPRESA_USUARIO_LOGUEADO.ToString()] %>";
            <%  
                
                string vista = (string)ViewData[Constantes.VistaEmpresa] ?? "RegistrarEmpresaEditor";
                string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Consultar";
             %>
            Empresa.Registrar.initConsultar('<%: vista %>');
        });
    </script>
</asp:Content>