var config = {
    iniGrids: {
        GrillaEventos: function () {
            var width = $("#roNombre").width();
            $('#gridEventos').DataTable({
                "searching": false,
                "ordering": false,
                "processing": true,
                "serverSide": true,
                "destroy": true,
                "responsive": true,
                "language": {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                "ajax": {
                    "url": urlListEventoUsuarioPaged,
                    "type": 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    "data": function (oPaged) {
                        return JSON.stringify({ "page": oPaged });
                    },
                },
                "columns": [
                            {
                                "data": function (row, type, set, meta) {
                                    return '<div class="rowNombreEvento" title="' + row.NombreEvento + '" style="overflow:hidden;white-space:nowrap;text-overflow: ellipsis;">' + row.NombreEvento + '</div>';
                                }, "bSortable": false, "sClass": 'text-left'
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return parseJsonDate(row.Fecha);
                                }, "bSortable": false, "sClass": 'text-center'
                            },
                            { "data": "Expositor" },
                            {
                                "data": function (row, type, set, meta) {
                                    var IrContenido = '<a data-toggle="tooltip1" data-placement="bottom" title="Ir a contenido" onclick="Eventos.IrContenidoVirtual(\'' + row.rowid + '\')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-share-alt" aria-hidden="true"></span></a>';
                                    return IrContenido;
                                }, "sClass": 'text-center'
                            }
                ],
                "drawCallback": function (settings) {
                    $(".rowNombreEvento").width(width);
                    $('[data-toggle="tooltip1"]').tooltip();
                }
            });
        },
    }
}
var Eventos = {
    IrContenidoVirtual: function (RowId) {
        window.location.href = urlIrContenidoEvento + "/" + RowId;
    }
}

$(document).ready(function () {   
    config.iniGrids.GrillaEventos();
});