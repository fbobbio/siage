<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteLogueado.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gestion asignación por Mab
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TituloContent" runat="server">
    Gestion asignación por Mab
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>Mabs de ausentismo con fecha desde igual a la actual</legend>
        <div id="divMabsAusentismoFechaDesde" style="clear:both;">
            <table id="listaMabAusentismoFechaDesde" cellpadding="0" cellspacing="0"></table>
            <div id="pagerMabAusentismoFechaDesde"></div>
        </div>
    </fieldset>
    
    <fieldset>
        <legend>Mabs de ausentismo con fecha hasta igual a la actual</legend>
        <div id="divMabAusentismoFechaHasta" style="clear:both;">
            <table id="listaMabAusentismoFechaHasta" cellpadding="0" cellspacing="0"></table>
            <div id="pagerMabAusentismoFechaHasta"></div>
        </div>
    </fieldset>
    
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">
 <script src="<%: Url.Content("~/Scripts/jquery.tmpl.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.maskedinput-1.2.2.js") %>" type="text/javascript"></script> 
    <script src="<%: Url.Content("~/Scripts/siage.grillas.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.abmc.formulario.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.abmc.grilla.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.abmc.util.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.gestionarAsignacionPorMab.js") %>" type="text/javascript"></script>

     <script type="text/javascript">
        $(document).ready(function () {
            GestionarAsignacionPorMab.init();
        });
    </script>
</asp:Content>
