var GrupoMab = {
    initGrillaCodigosMovimientoMab: function () {
        $("#gridCodigosMovimientoMab").jqGrid({
            url: $.getUrl("/CodigoMovimientoMab/GetAllCodigosMovimientoMab"),
            datatype: 'json',
            mtype: 'GET',
            colNames: ['Id', 'Codigo', 'Descripcion', 'Uso', 'GrupoMabId'],
            multiselect: true,
            colModel: [
                            { name: 'Id', index: 'Id', width: 55, key: true, jsonmap: 'Id', hidden: true },
                            { name: 'Codigo', index: 'Codigo', width: 150, sortable: false, jsonmap: 'Codigo' },
                            { name: 'Descripcion', index: 'Descripcion', width: 150, sortable: false, jsonmap: 'Descripcion' },
                            { name: 'Uso', index: 'Uso', width: 150, sortable: false, jsonmap: 'Uso' },
                            { name: 'GrupoMabId', index: 'GrupoMabId', width: 150, sortable: false, jsonmap: 'GrupoMabId', hidden: true }
                          ],
            pager: '#pager1',
            rowNum: 10,
            sortname: 'invid',
            sortorder: 'desc',
            viewrecords: true,
            width: '100%',
            caption: 'Códigos de Movimiento Mab',
            height: "100%"
        });
        //fin definicion de la grilla1

    },
    initGrillaEstadosAntrioresPuestoTrabajo: function () {
        $("#estadosAnterioresAccionesPuestoTrabajo").jqGrid({
        datatype:'local',
        colNames: ['Id', 'Codigo', 'Descripcion', 'Uso', 'GrupoMabId'],
        colModel: [
                            { name: 'Id', index: 'Id', width: 55, key: true, jsonmap: 'Id', hidden: true },
                            { name: 'Codigo', index: 'Codigo', width: 150, sortable: false, jsonmap: 'Codigo' },
                            { name: 'Descripcion', index: 'Descripcion', width: 150, sortable: false, jsonmap: 'Descripcion' },
                            { name: 'Uso', index: 'Uso', width: 150, sortable: false, jsonmap: 'Uso' },
                            { name: 'GrupoMabId', index: 'GrupoMabId', width: 150, sortable: false, jsonmap: 'GrupoMabId', hidden: true }
        });

        $("#estadosAnterioresAccionesPuestoTrabajoAnterior").jqGrid({});
    },
    initGrillaCodigoMovimientoParaGrupoMAB: function () {
        $("#grillaCodigoMovimientoParaGrupoMAB").jqGrid({
            datatype: 'local',
            multiselect: false,
            colNames: ['Id', 'Codigo', 'Descripcion', 'Uso', 'GrupoMabId'],
            colModel: [
                            { name: 'Id', index: 'Id', width: 55, key: true, jsonmap: 'Id', hidden: true },
                            { name: 'Codigo', index: 'Codigo', width: 150, sortable: false, jsonmap: 'Codigo' },
                            { name: 'Descripcion', index: 'Descripcion', width: 150, sortable: false, jsonmap: 'Descripcion' },
                            { name: 'Uso', index: 'Uso', width: 150, sortable: false, jsonmap: 'Uso' },
                            { name: 'GrupoMabId', index: 'GrupoMabId', width: 150, sortable: false, jsonmap: 'GrupoMabId', hidden: true }
                          ],
            pager: '#pager2',
            rowNum: 10,
            sortname: 'invid',
            sortorder: 'desc',
            viewrecords: true,
            width: '100%',
            caption: 'Códigos de Movimiento Mab del Grupo MAB seleccionado',
            height: "100%"
        });
    },
    initCodigosMovimientoMabSeleccionados: function () {
        $("#gridCodigosMeMovimientoSeleccionados").jqGrid({
            datatype: 'local',
            multiselect: true,
            colNames: ['Id', 'Codigo', 'Descripcion', 'Uso', 'GrupoMabId'],
            colModel: [
                            { name: 'Id', index: 'Id', width: 55, key: true, jsonmap: 'Id', hidden: true },
                            { name: 'Codigo', index: 'Codigo', width: 150, sortable: false, jsonmap: 'Codigo' },
                            { name: 'Descripcion', index: 'Descripcion', width: 150, sortable: false, jsonmap: 'Descripcion' },
                            { name: 'Uso', index: 'Uso', width: 150, sortable: false, jsonmap: 'Uso' },
                            { name: 'GrupoMabId', index: 'GrupoMabId', width: 150, sortable: false, jsonmap: 'GrupoMabId', hidden: true }
                          ],
            pager: '#pager2',
            rowNum: 10,
            sortname: 'invid',
            sortorder: 'desc',
            viewrecords: true,
            width: '100%',
            caption: 'Códigos de Movimiento Mab seleccionados',
            height: "100%"
        });
    },
    initBotonAgregarQuitar: function () {
        $("#btnQuitar").click(function () {
            var data = $("#gridCodigosMeMovimientoSeleccionados").jqGrid("getGridParam", "selarrrow");
            var aux = null;
            var longitud = data.length;
            for (var i = 0; i < longitud; i++) {
                aux = $("#gridCodigosMeMovimientoSeleccionados").getRowData(data[0]);
                //$("#gridCodigosMeMovimientoSeleccionados").delRowData(data[0]);
                $("#gridCodigosMovimientoMab").addRowData(aux.Id, aux);
                $("#gridCodigosMeMovimientoSeleccionados").delRowData(aux.Id);
            }
        }); //END BTN QUITAR
        //---------
        $("#btnAgregar").click(function () {
            var data = $("#gridCodigosMovimientoMab").jqGrid("getGridParam", "selarrrow");
            var aux = null;
            var longitud = data.length;
            for (var i = 0; i < longitud; i++) {
                aux = $("#gridCodigosMovimientoMab").jqGrid("getRowData", data[0]);
                $("#gridCodigosMeMovimientoSeleccionados").addRowData(aux.Id, aux);
                $("#gridCodigosMovimientoMab").delRowData(aux.Id);
            }
        }); //END BTN AGREGAR
    }
};

GrupoMab.init = function (estadoText) {

    GrupoMab.estado = estadoText;
    GrupoMab.initCodigosMovimientoMabSeleccionados();
    GrupoMab.initBotonAgregarQuitar();

    switch (estadoText) {
        case 'Eliminar':
            $('#divAbmc #TipoGrupo').attr('disabled', true);
            $('#btnAgregar, #btnQuitar').attr('disabled', true);
            GrillaUtil.setUrl($('#gridCodigosMeMovimientoSeleccionados'), $.getUrl('/GrupoMab/GetCodigosMovimientoByGrupoMabId/' + '?grupoMabId=' + parseInt($('#Id').val())));
            $('#gridCodigosMeMovimientoSeleccionados').setGridParam({ datatype: "json", mtype: 'GET', multiselect: false });
            $('#gridCodigosMeMovimientoSeleccionados').trigger('reloadGrid');
            break;
        case 'Registrar':
            GrupoMab.initGrillaCodigosMovimientoMab();

            break;
        case 'Editar':
            GrupoMab.initGrillaCodigosMovimientoMab();
            GrillaUtil.setUrl($('#gridCodigosMeMovimientoSeleccionados'), $.getUrl('/GrupoMab/GetCodigosMovimientoByGrupoMabId/' + '?grupoMabId=' + parseInt($('#Id').val())));
            $('#gridCodigosMeMovimientoSeleccionados').setGridParam({ datatype: "json", mtype: 'GET' });
            $('#gridCodigosMeMovimientoSeleccionados').trigger('reloadGrid');
            $('#divAbmc #TipoGrupo').attr('disabled', true);
            break;
        case 'Ver':
            $('.soloRegistro').hide();
            $('#btnAgregar, #btnQuitar').hide();
            $('#divAbmc #TipoGrupo').attr('disabled', true);
            $('#btnAgregar, #btnQuitar').attr('disabled', true);
            $("#btnAgregarQuitar").hide();
            //$('#gridCodigosMeMovimientoSeleccionados').setGridParam({url: $.getUrl('/GrupoMab/GetCodigosMovimientoByGrupoMabId/' + '?grupoMabId=' + parseInt($('#Id').val())}); 

            GrillaUtil.setUrl($('#gridCodigosMeMovimientoSeleccionados'), $.getUrl('/GrupoMab/GetCodigosMovimientoByGrupoMabId/' + '?grupoMabId=' + parseInt($('#Id').val())));
            $('#gridCodigosMeMovimientoSeleccionados').setGridParam({ datatype: "json", mtype: 'GET' });
            $('#gridCodigosMeMovimientoSeleccionados').trigger('reloadGrid');
            //            $.getJSON($.getUrl('/GrupoMab/GetCodigosMovimientoByGrupoMabId/' + '?grupoMabId=' + parseInt($('#Id').val())),
            //                null,
            //                function (data) {
            //                    for (var i = 0; i < data.rows.length; i++) {
            //                        console.log(data.rows[i].cell[0]);
            //                        $("#gridCodigosMeMovimientoSeleccionados").addRowData(data.rows[i].cell[0], data.rows[i], "last");
            //                    };
            //                }
            //            );
            break;

        default:
            break;
    };
};

Abmc.preEnviarModelo = function (datos) {
    var unidades = $("#gridCodigosMeMovimientoSeleccionados").getRowData();
    var codigosDeMovimiento = [];
    $.each(unidades, function (ind, val) {
        codigosDeMovimiento[ind] = {};
        codigosDeMovimiento[ind].Id = val.Id
        codigosDeMovimiento[ind].Codigo = val.Codigo;
        codigosDeMovimiento[ind].Descripcion = val.Descripcion;
        codigosDeMovimiento[ind].Uso = val.Uso;
        codigosDeMovimiento[ind].GrupoMabId = val.GrupoMabId;
    });

    $.formatoModelBinder(codigosDeMovimiento, datos, "CodigosMovimientoMab");
};

Abmc.postEnviarModelo = function (data) {
    if (data && data.status && GrupoMab.estado !== 'Editar') {
        $("#gridCodigosMeMovimientoSeleccionados").clearGridData(false);
        $("#gridCodigosMovimientoMab").setGridParam({page: 1});
        $("#gridCodigosMovimientoMab").trigger('reloadGrid');
    }
};

GrupoMab.estado = null;
