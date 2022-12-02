var GrillaDocumentos;
var contenidoHtml = "";
var config = {
    iniGrids: {
        GrillaDocumentos: function () {
            GrillaDocumentos = $('#gridDocumentos').DataTable({
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
                            IdDocumentoPadre: $("#txtIdDocumentoPadre").val(),
                            Estado: $("#chckVerEliminados").prop("checked")
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
                            return parseJsonDate(row.FechaDescarga) + " " + parseJsonTime(row.FechaDescarga);
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
                        }, "sClass": 'text-right' },
                    {
                        "data": function (row, type, set, meta) {
                            if (row.Tipo == 0) {
                                return "";
                            } else {
                                if (row.EstadoDocumento == 1) {
                                    return "Pendiente";
                                } else {
                                    return "Descargado";
                                }
                            }
                        }, "sClass": 'text-center' },
                    {
                        "data": function (row, type, set, meta) {
                            let result;
                            var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteDocumento(' + row.IdDocumento + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                            var eliminarDoc = '<a style="background-color: #dd4b39;border-color: #c52d1a;" data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteFisicoDocumento(' + row.IdDocumento + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                            var restaurar = '<a style="background-color: #dd4b39;border-color: #c52d1a;" data-toggle="tooltip1" data-placement="bottom" title="Restaurar" onclick="Eventos.RestoreDocumento(' + row.IdDocumento + ')" class="btn btn-primary btn-xs"><span class="fa fa-history" aria-hidden="true"></span></a>';
                            switch (row.Tipo) {
                                case 0:
                                    result = "";
                                    break;
                                case 1:
                                    var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetCarpeta(' + row.IdDocumento + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';                           
                                    var descargar = '<a data-toggle="tooltip1" data-placement="bottom" title="Descargar carpeta" onclick="Eventos.DownloadCarpeta(' + row.IdDocumento + ')" class="btn btn-primary btn-xs"><span class="fa fa-cloud-download" aria-hidden="true"></span></a>';
                                    if (!row.Estado) {
                                        result = eliminarDoc;
                                    } else {
                                        result = descargar + " " + editar + " " + eliminar;
                                    }
                                    break;
                                case 2:
                                    var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetDocumento(' + row.IdDocumento + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                                    var descargar = '<a data-toggle="tooltip1" data-placement="bottom" title="Descargar documento" onclick="Eventos.DownloadDocumento(' + row.IdDocumento + ')" class="btn btn-primary btn-xs"><span class="fa fa-cloud-download" aria-hidden="true"></span></a>';
                                    if (!row.Estado) {
                                        result = restaurar + " " + eliminarDoc;
                                    } else {
                                        result = descargar + " " + editar + " " + eliminar;
                                    }
                                    break;
                            }
                            return result;
                        }, "sClass": 'text-center'
                    }
                ],
                createdRow: function (row, data, dataIndex) {
                    console.log(data.IdDocumento);
                    if (data.Tipo == 0) {
                        $('td:eq(0)', row).attr('colspan', 7);
                        $('td:eq(1)', row).css('display', 'none');
                        $('td:eq(2)', row).css('display', 'none');
                        $('td:eq(3)', row).css('display', 'none');
                        $('td:eq(4)', row).css('display', 'none');
                        $('td:eq(5)', row).css('display', 'none');
                        $('td:eq(6)', row).css('display', 'none');
                        $('td:eq(7)', row).css('display', 'none');
                    }
                },
                "drawCallback": function (settings) {
                    $('[data-toggle="tooltip1"]').tooltip({ container: "body" });
                }
            });
        },
    }
}
var Eventos = {
    ListEmpresas: function () {
        DBR.blockUIStar();
        $.ajax({
            url: urlListEmpresaCombo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({}),
            success: function (response) {
                DBR.blockUIStop();
                Funciones.LlenarCombo(response.response, "#cboEmpresas", true, false, "Seleccione empresa");
                let Tamaño = (response.Tamaño / (1024 * 1024)).toFixed(2) + "Mb / 10Gb";
                $("#lblConsumo").html(Tamaño);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
                DBR.blockUIStop();
            }
        });
    },
    GetSizeFolder: function () {
        $.ajax({
            url: urlGetSizeFolder,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({}),
            success: function (response) {
                let Tamaño = (response / (1024 * 1024)).toFixed(2) + "Mb / 10Gb";
                $("#lblConsumo").html(Tamaño);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
                DBR.blockUIStop();
            }
        });
    },
    CrearCarpeta: function () {
        DBR.blockUIStar();
        let request = {
            IdDocumento: $("#txtIdDocumento1").val(),
            IdDocumentoPadre: $("#txtIdDocumentoPadre").val(),
            IdEmpresa: $("#cboEmpresas").val(),
            Nombre: $("#NombreCarpeta").val()
        };

        $.ajax({
            url: urlSaveCarpeta,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                if (response.IsSuccess) {
                    DBR.ToastSuccess(response.Message);
                    GrillaDocumentos.ajax.reload();
                    $("#modalCrearCarpeta").modal("hide");
                } else {
                    DBR.ToastError(response.Message);
                }
                DBR.blockUIStop();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
                DBR.blockUIStop();
            }
        });
    },
    GetCarpeta: function (IdDocumento) {
        DBR.blockUIStar();
        let request = {
            IdDocumento: IdDocumento,
        };
        $.ajax({
            url: urlGetDocumento,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {                
                DBR.blockUIStop();
                $("#txtIdDocumento1").val(response.IdDocumento);
                $("#titleNuevaCarpeta").html("Editar carpeta");
                DBR.limpiarCampo("#NombreCarpeta");
                $("#NombreCarpeta").val(response.NombreOriginal);
                $("#modalCrearCarpeta").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
                DBR.blockUIStop();
            }
        });
    },
    SaveDocumento: function () {
        var input1 = document.getElementById("fileDocumento");

        var formData = new FormData();
        formData.append("documento", input1.files[0]);
        formData.append("IdDocumento", $("#txtIdDocumento2").val());
        formData.append("IdDocumentoPadre", $("#txtIdDocumentoPadre").val());
        formData.append("IdEmpresa", $("#cboEmpresas").val());

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveDocumento,
            type: "POST",
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            data: formData
        }).done(function (response) {
            if (response.IsSuccess) {
                DBR.ToastSuccess(response.Message);
                GrillaDocumentos.ajax.reload();
                Eventos.GetSizeFolder();
                $("#modalDocumento").modal("hide");
            } else {
                DBR.ToastError(response.Message);
            }
            DBR.blockUIStop();
        }).fail(function (XMLHttpRequest) {
            DBR.blockUIStop();
            console.log(XMLHttpRequest);
        });
    },
    GetDocumento: function (IdDocumento) {
        $("#fileDocumento").filestyle('clear');
        $("#fileDocumento").parent().removeClass("has-error");
        $("#fileDocumento").next().next().addClass("hide");
        $("#lblTitleAgregarDocumento").html("Editar documento");
        $("#txtIdDocumento2").val(IdDocumento);
        $("#modalDocumento").modal("show");
    },
    DeleteDocumento: function (IdDocumento) {
        var request = new Object();
        request.IdDocumento = IdDocumento;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: '¿Esta seguro de eliminar el registro?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {

                        DBR.blockUIStar();
                        $.ajax({
                            url: urlDeleteDocumento,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    GrillaDocumentos.ajax.reload();
                                    DBR.ToastSuccess(response.Message);
                                } else {
                                    DBR.ToastError(response.Message);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                DBR.blockUIStop();
                                console.log(textStatus);
                            }
                        });
                    }
                },
                {
                    label: 'No',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });
    },
    DownloadDocumento: function (IdDocumento) {
        window.location.href = urlDownloadDocumento + "?IdDocumento=" + IdDocumento;
    },
    DownloadCarpeta: function (IdDocumento) {
        window.location.href = urlDownloadCarpeta + "?IdDocumento=" + IdDocumento;
    },
    EnviarCorreo: function () {
        DBR.blockUIStar("Enviando");
        let request = {
            Asunto: $("#txtAsuntoMensaje").val(),
            CorreoOrigen: $("#txtCorreoOrigen").val(),
            NombreOrigen: $("#txtNombreOrigen").val(),
            CorreoDestino: $("#txtCorreoDestino").val(),
            Mensaje: contenidoHtml,
        };
        $.ajax({
            url: urlNotificarEmpresaCorreo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    DBR.ToastSuccess(response.Message);
                    GrillaDocumentos.ajax.reload();
                    $("#modalNotificarCorreo").modal("hide");
                } else {
                    DBR.ToastError(response.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
                DBR.blockUIStop();
            }
        });
    },
    DeleteFisicoDocumento: function (IdDocumento) {
        var request = new Object();
        request.IdDocumento = IdDocumento;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: '¿Esta seguro de eliminar permanentemente el registro?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {

                        DBR.blockUIStar();
                        $.ajax({
                            url: urlDeleteFisicoDocumento,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    GrillaDocumentos.ajax.reload();
                                    Eventos.GetSizeFolder();
                                    DBR.ToastSuccess(response.Message);
                                } else {
                                    DBR.ToastError(response.Message);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                DBR.blockUIStop();
                                console.log(textStatus);
                            }
                        });
                    }
                },
                {
                    label: 'No',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });
    },
    RestoreDocumento: function (IdDocumento) {
        var request = new Object();
        request.IdDocumento = IdDocumento;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: '¿Esta seguro de restaurar el registro?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {

                        DBR.blockUIStar();
                        $.ajax({
                            url: urlRestoreDocumento,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    GrillaDocumentos.ajax.reload();
                                    DBR.ToastSuccess(response.Message);
                                } else {
                                    DBR.ToastError(response.Message);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                DBR.blockUIStop();
                                console.log(textStatus);
                            }
                        });
                    }
                },
                {
                    label: 'No',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });
    },
    //Correo
    SaveFileServer: function () {

        var input = document.getElementById("inputFileServer");

        var formData = new FormData();
        formData.append("documento", input.files[0]);

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveFileServer,
            type: "POST",
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            data: formData
        }).done(function (response) {
            DBR.blockUIStop();
            $("#inputFileServer").filestyle('clear');
            if (response.IsSuccess) {
                $("#txtLink").val(response.Message);
            } else {
                $("#txtLink").val("");
                GISSAT.ToastError(response.Message);
            }
        }).fail(function (XMLHttpRequest) {
            DBR.blockUIStop();
            console.log(XMLHttpRequest);
        });
    },
}

var Funciones = {
    LlenarCombo: function (data, control, EsSelect2, EsMultiple, TextoInicial) {
        $(control).html("");
        var Opciones = '';
        if (TextoInicial != null && TextoInicial != undefined) {
            Opciones = '<option value="0">' + TextoInicial + '</option>';
        }

        if (EsMultiple) {
            Opciones = "";
        }
        $.each(data, function (e, i) {
            Opciones = Opciones + '<option data-Informacion="' + i.Informacion +'" value="' + i.Value + '">' + i.Descripcion + '</option>';
        });
        $(control).html(Opciones);
        if (EsSelect2) {
            $(control).select2();
        }
    },
    VerCarpeta: function (IdDocumento) {
        $("#txtIdDocumentoPadre").val(IdDocumento);
        GrillaDocumentos.ajax.reload();
    },
    LimpiarControles: function () {
        DBR.limpiarCampo("#txtAsuntoMensaje");
        DBR.limpiarCampo("#txtCorreoOrigen");
        DBR.limpiarCampo("#txtNombreOrigen");
        $("#inputFileServer").filestyle('clear');
        $("#txtLink").val("");
        CKEDITOR.instances.txtMensajeCompleto.setData("");
        contenidoHtml = "";
    },
}
$.fn.SoloLetrasNumerosDatPer = function () {
    $(this).keydown(function (e) {
        switch (true) {
            case ((e.keyCode == 186)):
                e.preventDefault();
                e.stopPropagation();
                break;
            case ((e.keyCode == 111)):
                e.preventDefault();
                break;
            case ((e.shiftKey) && (e.keyCode == 35)):
                e.preventDefault();
                break;
            case ((e.shiftKey) && (e.keyCode == 36)):
                e.preventDefault();
                break;
            case (e.shiftKey || e.ctrlKey || e.altKey):
                e.preventDefault();
                break;
            case ((e.keyCode >= 186) && (e.keyCode <= 187)):
                e.preventDefault();
                break;
            case ((e.keyCode >= 193) && (e.keyCode <= 218)):
                e.preventDefault();
                break;
            case (e.keyCode >= 220):
                e.preventDefault();
                break;
            case (e.keyCode == 191):
                e.preventDefault();
                break;
            default:
                var n = e.keyCode;
                if (
                    !((n >= 48 && n <= 57) || //numero teclado funcion
                        (n >= 96 && n <= 105) || //numeros 0..9 teclado numerico
                        (n >= 109 && n <= 111) || // MENOS y PUNTO y Slash
                        (n >= 65 && n <= 90) || //letras may
                        (n >= 8 && n <= 9) ||  //BACK SPACE y TAB
                        (n >= 12 && n <= 13) ||  //ENTER
                        (n == 32) || (n == 46) || //Space y SUPR
                        (n >= 35 && n <= 40) ||
                        (n == 192) || // Ñ
                        (n == 219) || //comilla simple
                        (n <= 1)) // Ñ en firefox
                ) {
                    e.preventDefault();
                }
        }
        //alert(e.keyCode);
    });
}
$(document).ready(function () {
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };
    Eventos.ListEmpresas();
    config.iniGrids.GrillaDocumentos();
    /*$("#NombreCarpeta").SoloLetrasNumerosDatPer();*/
    $('input').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
        increaseArea: '20%' // optional
    });
    //NOTIFICAR
    $("#btnNotificarCorreo").on("click", function () {
        if (!DBR.validarCampo("#cboEmpresas")) {
            DBR.ToastWarning("Seleccione una empresa");
            return false;
        }
        Funciones.LimpiarControles();
        $("#txtCorreoOrigen").val(CorreoOrigen);
        $("#txtNombreOrigen").val(NombreCorreoOrigen);
        $("#txtCorreoDestino").val($("#cboEmpresas option:selected").data("informacion"));
        $("#txtNombreDestino").val($("#cboEmpresas option:selected").text());
        $("#modalNotificarCorreo").modal("show");
    });
    //CREAR CARPETA
    $("#btnAgregarCarpeta").on("click", function () {
        if (!DBR.validarCampo("#cboEmpresas")) {
            DBR.ToastWarning("Seleccione una empresa");
            return false;
        }
        $("#titleNuevaCarpeta").html("Nueva carpeta");
        $("#txtIdDocumento1").val(0);
        DBR.limpiarCampo("#NombreCarpeta");
        $("#modalCrearCarpeta").modal("show");
    });
    $("#btnCrearCarpeta").on("click", function () {
        if (DBR.validarCampo("#NombreCarpeta")) {
            Eventos.CrearCarpeta();
        }
    });
    //SUBIR ARCHIVO
    $("#btnSubirArchivo").on("click", function () {
        if (!DBR.validarCampo("#cboEmpresas")) {
            DBR.ToastWarning("Seleccione una empresa");
            return false;
        }
        $("#fileDocumento").filestyle('clear');
        $("#fileDocumento").parent().removeClass("has-error");
        $("#fileDocumento").next().next().addClass("hide");
        $("#lblTitleAgregarDocumento").html("Subir documento");
        $("#txtIdDocumento2").val(0);
        $("#modalDocumento").modal("show");
    });
    $("#btnGuardarDucumento").on("click", function () {
        let validar = true;
        var input = document.getElementById("fileDocumento");
        if (input.files.length == 0) {
            $("#fileDocumento").parent().addClass("has-error");
            $("#fileDocumento").next().next().removeClass("hide");
            validar = false;
        } else {
            $("#fileDocumento").parent().removeClass("has-error");
            $("#fileDocumento").next().next().addClass("hide");
            if (input.files[0].size > 10485760) {
                DBR.ToastWarning("Verifique que el archivo adjunto no supere el 10MB");
                validar = false;
            }
        }
        if (validar) {
            Eventos.SaveDocumento();
        }
    });
    //ACTUALIZAR DATOS
    $("#btnActualizar").on("click", function () {
        GrillaDocumentos.ajax.reload();
    });
    $("#cboEmpresas").on("change", function () {
        $("#txtIdDocumentoPadre").val(0)
        GrillaDocumentos.ajax.reload();
    });
    $('#chckVerEliminados').on('ifChanged', function () {
        GrillaDocumentos.ajax.reload();
    });


    var controlMensComp = CKEDITOR.replace('txtMensajeCompleto', {
        filebrowserBrowseUrl: urlFileServerBrowse,
        filebrowserUploadUrl: urlSaveFileServerCkEditor
    });
    controlMensComp.on('change', function (evt) {
        contenidoHtml = evt.editor.getData();
    });
    $("#btnGuardarCorreo").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#txtAsuntoMensaje")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtCorreoOrigen")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtNombreOrigen")) {
            validar = false;
        }
        if ($.trim(contenidoHtml).length == 0) {
            contenidoHtml = "<p></p>";
        }
        if (validar) {
            Eventos.EnviarCorreo();
        }
    });
    //Correo
    $("#inputFileServer").on("change", function () {

        var input = document.getElementById("inputFileServer");
        if (input.files.length == 0) {
            GISSAT.ToastWarning("No se selecciono ningún archivo");
            return false;
        }

        if (input.files[0].size > 2097152) {
            GISSAT.ToastWarning("Verifique que el archivo no supere los 2MB");
            $("#inputFileServer").filestyle('clear');
            $("#txtLink").val("");
            return false;
        }
        Eventos.SaveFileServer();
    });
});