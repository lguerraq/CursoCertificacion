var config = {
    iniGrids: {
        GrillaEventos: function () {
            var width = $("#roNombre").width();
            var tableCursos = $('#gridEventos').DataTable({
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
                            return meta.row + 1;;
                        }, "bSortable": false, "sClass": 'text-left'
                    },
                    {
                        "data": function (row, type, set, meta) {
                            return '<div title="' + row.Fecha + '" style="overflow:hidden;white-space:nowrap;text-overflow: ellipsis;">' + row.NombreEvento + '</div>';
                        }, "bSortable": false, "sClass": 'text-left'
                    },
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
                            var IrContenido = '<a data-toggle="tooltip1" data-placement="bottom" title="Ir a contenido" onclick="Eventos.IrContenidoVirtual(\'' + row.rowid + '\')"><i class="uil uil-eye" aria-hidden="true"></i></a>';
                            return IrContenido;
                        }, "sClass": 'text-center'
                    }
                ],
                "drawCallback": function (settings) {
                    $(".rowNombreEvento").width(width);
                    $('[data-toggle="tooltip1"]').tooltip();
                }
            });

            tableCursos.on('order.dt search.dt', function () {
                debugger;
                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        },
    }
}
var Eventos = {
    CargarPdfCertificadoUtomatico: function (IdEvento, NombreCertificadoImprimir) {

        var documentos = new Object();
        documentos.DocumentoCertificadoImprimir = NombreCertificadoImprimir;
        documentos.IdEvento = IdEvento;

        DBR.blockUIStar();

        $.ajax({
            url: urlCargarPdfCertificadoUtomatico,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "documentos": documentos }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    window.location.href = urlDescargarPdfGenerado + "?NombreDocumento=" + response.Message;
                } else {
                    DBR.ToastError(response.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    IrContenidoVirtual: function (RowId) {
        window.location.href = urlIrContenidoEvento + "/" + RowId;
    }
}

$(document).ready(function () {
    //config.iniGrids.GrillaEventos();
    $("#gridEventos").on("click", ".btnCertificado", function () {
        var IdEvento = $(this).data("idevento");
        var nombrecertificado = $(this).data("nombrecertificado");
        Eventos.CargarPdfCertificadoUtomatico(IdEvento, nombrecertificado);
    });
});