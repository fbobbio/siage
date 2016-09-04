<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AjaxAbmc.Master" Inherits="System.Web.Mvc.ViewPage<Siage.Services.Core.Models.GrupoMabModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TituloContent" runat="server">
    Grupo Mab
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FiltrosBusquedaContent" runat="server">
    <p>
        <%:Html.Label("Tipo Grupo") %><%:Html.DropDownListEnumFor(model=>model.TipoGrupo) %></p>
    <p>
        <%: Html.Label("Número Grupo") %><%: Html.TextBox("NumeroGrupoMab") %></p>
    <p>
        <%: Html.Label("Código movimiento Mab") %><%: Html.TextBox("CodigoMovimientoMab") %></p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%: Url.Content("~/Scripts/siage.grillas.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.grillas.util.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.abmc.grilla-detalle.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.abmc.formulario-detalle.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.grupoMab.js") %>" type="text/javascript"></script>
    <script type="text/javascript">


        $(document).ready(function () {
            //                var controller = "GrupoMab";
            //                var orderBy = "Codigo";
            //                var titulos = ['Id', 'Numero grupo', 'Tipo grupo ', 'N° de codigos de movimiento'];
            //                var propiedades = ['Id', 'nGrupo', 'tGrupo', 'nCodigosMovimiento'];
            //                var tipos = ["integer", null, null, null];
            //                var botones = ["Ver", "Eliminar", "Editar", "Agregar"]; // en orden inverso al que se mostraran
            //                var key = 'Id';

            //                var grilla = AbmcGrilla.init("#list", controller, titulos, propiedades, tipos, key, orderBy, botones);
            //                Abmc.init(controller, grilla);


            //                $("#divConsulta :input").changePatch(function () {
            //                    var parametros = "&filtroTipoGrupoMab=" + $("#TipoGrupo").val() + "&filtroNumeroGrupoMab=" + $("#NumeroGrupoMab").val() + "&filtroCodigoMovimientoMab=" + $("#CodigoMovimientoMab").val();
            //                    grilla.agregarParametros(parametros);
            //                });
            //            });


            //            var titulosDetalle = ['Id', 'Código', 'Descripción', 'Uso'];
            //            var propiedadesDetalle = ['Id', 'codigoMov', 'descripcionMov', 'usoMov'];
            //            var tiposDetalle = ['integer', 'integer', null, null];
            //            var controllerCodigoMoviento="CodigoMovimientoMab"
            //            var grilla = AbmcGrillaDetalle.init("#list", controllerCodigoMoviento, titulos, propiedades, tipos, key, orderBy, titulosDetalle, propiedadesDetalle, tiposDetalle, key, botones);
            //            AbmcDetalle.init(controller, grilla);
            //            grilla.agregarParametros(parametros);


            var controller = "GrupoMab";
            var orderBy = "Id";
            var titulos = ['Id', 'Número Grupo', 'Tipo Grupo', 'N° de codigos de movimiento'];
            var propiedades = ['Id', 'NumeroGrupo', 'TipoGrupo', 'nCodigosMovimiento'];
            var tipos = ["integer", "integer", null, null];
            var botones = ["Eliminar","Ver", "Editar", "Agregar" ]; // en orden inverso al que se mostraran
            var key = 'Id';

            var titulosDetalle = ['Id', 'Código', 'Descripción', 'Uso'];
            var propiedadesDetalle = ['Id', 'codigoMov', 'descripcionMov', 'usoMov'];
            var tiposDetalle = ['integer', null, null, null];

            var grilla = AbmcGrilla.init("#list", controller, titulos, propiedades, tipos, key, orderBy, botones);
            Abmc.init(controller, grilla);



            $("#divConsulta :input").changePatch(function () {
                var parametros = "&filtroTipoGrupoMab=" + $("#TipoGrupo option:selected ").val() + "&filtroNumeroGrupoMab=" + $("#NumeroGrupoMab").val() + "&filtroCodigoMovimientoMab=" + $("#CodigoMovimientoMab").val();
                grilla.agregarParametros(parametros); 
            });
        });
    </script>
</asp:Content>
