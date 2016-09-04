<%@ Page Title="Union de edificios" Language="C#" MasterPageFile="~/Views/Shared/AjaxAbmc.Master" Inherits="System.Web.Mvc.ViewPage<Siage.Services.Core.Models.RegistrarVinculoEmpresaEdificioModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TituloContent" runat="server">Vínculo Empresa Edificio
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FiltrosBusquedaContent" runat="server">
    <p><%: Html.Label("Código de empresa:")%> <%: Html.TextBox("FiltroCodigoEmpresa")%></p>
    <p><%: Html.Label("Nombre empresa:")%> <%: Html.TextBox("FiltroNombreEmpresa")%></p>
    <p><%: Html.Label("Código de edificio:")%> <%: Html.TextBox("FiltroCodigoEdificio")%></p>
    <p><%: Html.Label("Estado vínculo:")%> <%: Html.DropDownListEnum("FiltroEstadoVinculo", typeof(Siage.Base.EstadoVinculoEmpresaEdificioEnum))%></p>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContent" runat="server">

    <script src="<%: Url.Content("~/Scripts/jquery.cascadingDropDown.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.tmpl.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.empresa.consultar.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.grillas.js") %>" type="text/javascript"></script>  
    <script src="<%: Url.Content("~/Scripts/siage.vinculoEmpresaEdificio.js") %>" type="text/javascript"></script>  

<script type="text/javascript">
    $(document).ready(function () {
        VinculoEmpresaEdificio.init("vincularEdificio");
    });   
</script>

<script id="edificioTemplate" type="text/x-jquery-tmpl">    
       <div>
            <fieldset>        
                <legend>Edificio</legend>  
                <p><label>Identificador edificio:</label><span>${edificio.IdentificadorEdificio}</span></p>
                <p><label>Tipo edificio:</label><span>${tipoEdificio}</span></p>
                <p><label>Estado:</label><span>${estadoEdificio}</span></p>
                <p><label>Función edificio:</label><span>${funcionEdificio}</span></p>
                <p><label>Tipo adquisición:</label><span>${tipoAdquisicion}</span></p>
                <fieldset>        
                    <legend>Predio</legend>  
                    <p><label>Identificador predio:</label><span>${predio.IdentificadorPredio}</span></p>
                    <p><label>Fecha:</label><span>${fechaAlta}</span></p>
                    <p><label>Código CUI:</label><span>${predio.CodCuiNacion}</span></p>
                 
                    <p><label>Localidad:</label><span>${localidad}</span></p>
                    <p><label>Barrio:</label><span>${predio.Barrio}</span></p>
                    <p><label>Calle:</label><span>${predio.Calle}</span></p>
                    <p><label>Altura:</label><span>${predio.Altura}</span></p>
                </fieldset>
            </fieldset>
        </div>      
</script>

</asp:Content>
