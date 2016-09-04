<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AjaxAbmc.Master" Inherits="System.Web.Mvc.ViewPage<Siage.Services.Core.Models.FuncionEdificioModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TituloContent" runat="server">Función edificio
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FiltrosBusquedaContent" runat="server">
    <p><%: Html.Label("Nombre: ") %> <%: Html.TextBox("FiltroNombre") %></p>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            var controller = "FuncionEdificio";
            var orderBy = "Nombre";
            var titulos = ['Id', 'Nombre', 'Descripción',' Predefinido', 'Mostrar'];
            var propiedades = ['Id', 'Nombre', 'Descripcion', 'Predefinido', 'Mostrar'];
            var tipos = ['integer', null, null, null];
            var botones = [ "Eliminar", "Editar", "Agregar"]; // en orden inverso al que se mostraran
            var key = 'Id';

            var grilla = AbmcGrilla.init("#list", controller, titulos, propiedades, tipos, key, orderBy, botones);
            Abmc.init(controller, grilla);
            $("#list").hideCol("Mostrar");

            $("#divConsulta :input").changePatch(function () {
                var parametros = "&filtroNombre=" + $("#FiltroNombre").val();
                grilla.agregarParametros(parametros);
            });

            //Oculta las funciones de edificio que son predefinidas
            $("#list").setGridParam({ onSelectRow: function (id) {
                var fila = GrillaUtil.getFila($("#list"), id).Mostrar;
                if (fila === "True") {
                    $("td[title='Editar']").hide();
                }
                else {
                    $("td[title='Editar']").show();
                }
            }
            });
    });
    Abmc.postCargarModelo = function (estado, id) {
        $("#listRecientes").setGridWidth($("#divGrillaAbmc").width() - 10, true);
    };  
    </script>
</asp:Content>
