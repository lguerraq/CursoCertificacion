var GrillaDocumentos;
var config = {
    iniGrids: {
        GrillaDocumentos: function () {
            GrillaDocumentos =
                $('#gridDocumentos').DataTable({
                    "searching": false,
                    "ordering": false,
                    "processing": true,
                    "serverSide": true,
                    "destroy": true,
                    "responsive": true,
                    "stateSave": true,
                    "paging": false,
                    "language": {
                        url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                    },
                    "ajax": {
                        "url": urlListDocumentoPaginado,
                        "type": 'POST',
                        "datatype": "json",
                        "contentType": 'application/json; charset=UTF-8',
                        "data": function () {
                            let request = {
                                IdEmpresa: $("#cboEmpresas").val(),
                                IdDocumentoPadre: $("#txtIdDocumentoPadre").val()
                            };
                            return JSON.stringify({ "request": request });
                        }
                    },
                    "columns": [
                        {
                            "data": function (row, type, set, meta) {
                                let Nombre = "";
                                switch (row.Tipo) {
                                    case 0:
                                        Nombre = '<a title="Raíz" onclick="Funciones.VerCarpeta(0)" class="folder-personalizado"></i><i class="fa fa-folder fa-folder-open"></i> [..Raíz..] \\</a>' + row.NombreOriginal + ' <i class="fa fa-reply"></i>';
                                        break;
                                    case 1:
                                        Nombre = '<a title="Abrir" onclick="Funciones.VerCarpeta(' + row.IdDocumento + ')" class="folder-personalizado"><i class="fa fa-folder"></i> ' + row.NombreOriginal + '</a>';
                                        break;
                                    case 2:
                                        if (row.Extension == "PDF") {
                                            Nombre = '<a class="file-personalizado"><i class="fa fa-file-pdf-o"></i> ' + row.NombreOriginal + "</a>";
                                        } else if (row.Extension == "DOC" || row.Extension == "DOCX") {
                                            Nombre = '<a class="file-personalizado"><i class="fa fa-file-word-o"></i> ' + row.NombreOriginal + "</a>";
                                        } else if (row.Extension == "ZIP") {
                                            Nombre = '<a class="file-personalizado"><i class="fa fa-file-zip-o"></i> ' + row.NombreOriginal + "</a>";
                                        } else if (row.Extension == "XLSX" || row.Extension == "XLS") {
                                            Nombre = '<a class="file-personalizado"><i class="fa fa-file-excel-o"></i> ' + row.NombreOriginal + "</a>";
                                        } else if (row.Extension == "PPTX" || row.Extension == "PPT") {
                                            Nombre = '<a class="file-personalizado"><i class="fa fa-file-powerpoint-o"></i> ' + row.NombreOriginal + "</a>";
                                        } else if (row.Extension == "PNG" || row.Extension == "JPG") {
                                            Nombre = '<a class="file-personalizado"><i class="fa fa-file-image-o"></i> ' + row.NombreOriginal + "</a>";
                                        } else {
                                            Nombre = '<a class="file-personalizado"><i class="fa fa-file-text-o"></i> ' + row.NombreOriginal + "</a>";
                                        }
                                        break;
                                }
                                return Nombre;
                            }, "sClass": 'text-left'
                        },
                        {
                            "data": function (row, type, set, meta) {
                                return parseJsonDate(row.FechaCreacion) + " " + parseJsonTime(row.FechaCreacion);
                            }, "sClass": 'text-left'
                        },
                        {
                            "data": function (row, type, set, meta) {
                                return parseJsonDate(row.FechaModificacion) + " " + parseJsonTime(row.FechaModificacion);
                            }, "sClass": 'text-left'
                        },
                        {
                            "data": function (row, type, set, meta) {
                                if (row.Tamaño > 0) {
                                    if (row.Tamaño > (1024 * 1024)) {
                                        return (row.Tamaño / (1024 * 1024)).toFixed(2) + "Mb";
                                    } else {
                                        return (row.Tamaño / (1024)).toFixed(2) + "Kb";
                                    }
                                } else {
                                    return "";
                                }
                            }, "sClass": 'text-right'
                        },
                        {
                            "data": function (row, type, set, meta) {
                                let result;
                                switch (row.Tipo) {
                                    case 0:
                                        result = "";
                                        break;
                                    case 1:
                                        var descargar = '<a data-toggle="tooltip1" data-placement="bottom" title="Descargar carpeta" onclick="Eventos.DownloadCarpeta(' + row.IdDocumento + ')" class="btn btn-primary btn-xs"><span class="fa fa-cloud-download" aria-hidden="true"></span></a>';
                                        result = descargar;
                                        break;
                                    case 2:
                                        var descargar = '<a data-toggle="tooltip1" data-placement="bottom" title="Descargar documento" onclick="Eventos.DownloadDocumento(' + row.IdDocumento + ')" class="btn btn-primary btn-xs"><span class="fa fa-cloud-download" aria-hidden="true"></span></a>';
                                        result = descargar;
                                        break;
                                }
                                return result;
                            }, "sClass": 'text-center'
                        }
                    ],
                    "drawCallback": function (settings) {
                        $('[data-toggle="tooltip1"]').tooltip({ container: "body" });
                    }
                });
        },
    }
}
var Eventos = {
    GetEmpresaByUsuario: function () {
        DBR.blockUIStar();
        $.ajax({
            url: urlGetEmpresaByUsuario,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({}),
            success: function (response) {
                DBR.blockUIStop();
                $("#lblEmpresa").html("EMPRESA: " + response.RazonSocial);             
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
                DBR.blockUIStop();
            }
        });
    },
    DownloadDocumento: function (IdDocumento) {
        window.location.href = urlDownloadDocumento + "?IdDocumento=" + IdDocumento;
    },
    DownloadCarpeta: function (IdDocumento) {
        window.location.href = urlDownloadCarpeta + "?IdDocumento=" + IdDocumento;
    }
}

var Funciones = {
    VerCarpeta: function (IdDocumento) {
        $("#txtIdDocumentoPadre").val(IdDocumento);
        GrillaDocumentos.ajax.reload();
    }
}
$(document).ready(function () {
    Eventos.GetEmpresaByUsuario();
    config.iniGrids.GrillaDocumentos();
    //ACTUALIZAR DATOS
    $("#btnActualizar").on("click", function () {
        GrillaDocumentos.ajax.reload();
    });
});