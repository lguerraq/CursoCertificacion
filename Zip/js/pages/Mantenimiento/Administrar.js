var EditarDocumento1 = false;
var EditarDocumento2 = false;
var EditarDocumento3 = false;
var EditarDocumento4 = false;
var EditarDocumento5 = false;

var config = {
    ini: function () {
        $.ajaxSetup({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {}
        });
    },
    iniGrids: {
        GrillaEventos: function () {
            $('#gridEventos').DataTable({
                "searching": false,
                "ordering": false,
                "processing": true,
                "serverSide": true,
                "destroy": true,
                "responsive": true,
                "stateSave": true,
                "language": {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                "ajax": {
                    "url": urlListEvento,
                    "type": 'POST',
                    "datatype": "json",
                    "data": {},
                },
                "columns": [
                    {
                        "data": function (row, type, set, meta) {
                            return '<div title="' + row.NombreEvento + '" style="max-width:400px; overflow:hidden;white-space:nowrap;text-overflow: ellipsis;">' + row.NombreEvento + '</div>';
                            //return row.NombreEvento;
                        }, "bSortable": false, "sClass": 'text-left', "width": "30%"
                    },
                    {
                        "data": function (row, type, set, meta) {
                            return parseJsonDate(row.Fecha);
                        }, "bSortable": false, "sClass": 'text-center', "width": "08%"
                    },
                    { "data": "Expositor", "bSortable": false, "sClass": 'text-left', "width": "22%" },
                    {
                        "data": function (row, type, set, meta) {
                            if (row.Activo) {
                                return "SI";
                            } else {
                                return "NO";
                            }
                        }, "sClass": 'text-center', "width": "05%"
                    },
                    { "data": "Costo", "sClass": 'text-center', "width": "05%" },
                    {
                        "data": function (row, type, set, meta) {
                            return '<a href="../Documentos/' + row.ImagenEvento + '" target="_blank">FOTO' + (meta.row + 1) + '.JPG</a>';
                        }, "sClass": 'text-center', "width": "10%"
                    },
                    {
                        "data": function (row, type, set, meta) {
                            return '<a href="../Documentos/' + row.DocumentoCertificadoImprimir + '" target="_blank">Certificado' + (meta.row + 1) + '.PDF</a>';
                        }, "sClass": 'text-center', "width": "10%"
                    },
                    {
                        "data": function (row, type, set, meta) {
                            return '<a href="../Documentos/' + row.DocumentoCertificadoExpositor + '" target="_blank">Certificado' + (meta.row + 1) + '.PDF</a>';
                        }, "sClass": 'text-center', "width": "10%"
                    },
                    {
                        "data": function (row, type, set, meta) {
                            var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar evento" onclick="Eventos.GetEvento('
                                + row.IdEvento + ','
                                + (meta.row + 1)
                                + ')"><i class="uil uil-edit-alt"></i></a>';
                            var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar evento" onclick="Eventos.DeleteEvento(' + row.IdEvento + ')"><i class="uil uil-trash-alt" aria-hidden="true"></i></a>';
                            var activar = '<a data-toggle="tooltip1" data-placement="bottom" title="Activar" onclick="Eventos.UpdateEstadoEvento(' + row.IdEvento + ',' + 1 + ')"><span class="uil uil-check-circle" aria-hidden="true"></span></a>';
                            var desactivar = '<a data-toggle="tooltip1" data-placement="bottom" title="Desactivar" onclick="Eventos.UpdateEstadoEvento(' + row.IdEvento + ',' + 0 + ')"><span class="uil uil-ban" aria-hidden="true"></span></a>';
                            var modulos = '<a data-toggle="tooltip1" data-placement="bottom" title="Módulos" onclick="Eventos.Modulos(' + row.IdEvento + ')"><span class="uil uil-book-alt" aria-hidden="true"></span></a>';
                            if (row.Activo) {
                                activar = '';
                            } else {
                                desactivar = '';
                            }
                            return editar + " " + eliminar + " " + activar + " " + desactivar + " " + modulos;
                        }, "sClass": 'text-center', "width": "10%"
                    }
                ],
                "drawCallback": function (settings) {
                    $('[data-toggle="tooltip1"]').tooltip();
                }
            });
        },
        GrillaModulo: function () {
            $('#gridModulos').DataTable({
                "searching": true,
                "ordering": false,
                "processing": true,
                "serverSide": true,
                "destroy": true,
                "responsive": true,
                "autoWidth": false,
                "language": {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                "ajax": {
                    "url": urlListModuloPaged,
                    "type": 'POST',
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "data": function (oPaged) {
                        var request = new Object();
                        request.IdEvento = $("#txtIdEvento2").val();
                        return JSON.stringify({ "page": oPaged, "request": request });
                    },
                },
                "columns": [
                    { "data": "Nombre", "width": "40%" },
                    { "data": "Expositor", "width": "40%" },
                    { "data": "Horas", "sClass": 'text-center', "width": "10%" },
                    {
                        "data": function (row, type, set, meta) {
                            var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetModulo(' + row.IdModulo + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                            var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteModulo(' + row.IdModulo + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                            return editar + " " + eliminar;
                        }, "sClass": 'text-center', "width": "10%"
                    }
                ],
                "drawCallback": function (settings) {
                    $('[data-toggle="tooltip1"]').tooltip();
                }
            });
        },
    }
}
var Eventos = {
    SaveEvento: function () {
        var input1 = document.getElementById("fileFoto");
        //var input2 = document.getElementById("fileFotocheck");
        //var input3 = document.getElementById("fileCertificado");
        var input4 = document.getElementById("fileCertificadoImprimir");
        var input5 = document.getElementById("fileCertificadoExpositor");

        var formData = new FormData();
        formData.append("documento1", input1.files[0]);
        //formData.append("documento2", input2.files[0]);
        //formData.append("documento3", input3.files[0]);
        formData.append("documento4", input4.files[0]);
        formData.append("documento5", input5.files[0]);

        formData.append("IdEvento", $("#txtIdEvento").val());
        formData.append("NombreEvento", $("#txtNombreEvento").val().toUpperCase());
        formData.append("Fecha", $("#txtFecha").val());
        formData.append("Expositor", $("#txtExpositor").val().toUpperCase());
        formData.append("Horas", $("#txtHoras").val());
        formData.append("Costo", $("#cboCosto").val());

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveEvento,
            type: "POST",
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            data: formData
        }).done(function (response) {
            DBR.blockUIStop();
            if (response.IsSuccess) {
                $('#gridEventos').DataTable().ajax.reload();
                $("#modalEvento").modal("hide");
                DBR.ToastSuccess(response.Message);
            } else {
                DBR.ToastError(response.Message);
            }
        }).fail(function (XMLHttpRequest) {
            DBR.blockUIStop();
            console.log(XMLHttpRequest);
        });
    },
    SaveCurso: function () {

        var formData = new FormData();
        formData.append("IdEvento", $("#txtIdEvento").val());
        formData.append("NombreEvento", $("#txtNombreEvento").val().toUpperCase());
        formData.append("Fecha", $("#txtFecha").val());
        formData.append("Expositor", $("#txtExpositor").val().toUpperCase());
        formData.append("Horas", $("#txtHoras").val());
        formData.append("Costo", $("#cboCosto").val());

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveCurso,
            type: "POST",
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            async: false
        }).done(function (response) {
            DBR.blockUIStop();
            if (response.IsSuccess) {
                $('#gridEventos').DataTable().ajax.reload();
                $("#modalEvento").modal("hide");
                DBR.ToastSuccess(response.Message);
            } else {
                DBR.ToastError(response.Message);
            }

            return response.IsSuccess;

        }).fail(function (XMLHttpRequest) {
            DBR.blockUIStop();
            console.log(XMLHttpRequest);
            DBR.ToastError("Ocurrió un error al registrar la información.");
            return false;
        });
    },
    UpdateCursoFiles: function () {
        var input1 = document.getElementById("fileFoto");
        //var input2 = document.getElementById("fileFotocheck");
        //var input3 = document.getElementById("fileCertificado");
        var input4 = document.getElementById("fileCertificadoImprimir");
        var input5 = document.getElementById("fileCertificadoExpositor");

        var formData = new FormData();
        formData.append("documento1", input1.files[0]);
        //formData.append("documento2", input2.files[0]);
        //formData.append("documento3", input3.files[0]);
        formData.append("documento4", input4.files[0]);
        formData.append("documento5", input5.files[0]);

        formData.append("IdEvento", $("#txtIdEvento").val());

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveEvento,
            type: "POST",
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            async: false
        }).done(function (response) {
            DBR.blockUIStop();
            if (response.IsSuccess) {
                DBR.ToastSuccess(response.Message);
            } else {
                DBR.ToastError(response.Message);
            }

            return response.IsSuccess;
        }).fail(function (XMLHttpRequest) {
            DBR.blockUIStop();
            console.log(XMLHttpRequest);
            DBR.ToastError("Ocurrió un error al registrar la información.");
            return false;
        });
    },
    GetEvento: function (IdEvento, row) {
        //IdEvento, NombreEvento, Fecha, Expositor, Horas, row
        $("#lblTitleAgregarEvento").html("Editar");
        Funciones.LimpiarCamposEvento();

        var request = new Object();
        request.IdEvento = IdEvento;

        DBR.blockUIStar();
        $.ajax({
            url: urlGetEvento,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();

                $("#txtIdEvento").val(response.IdEvento);
                $("#txtNombreEvento").val(response.NombreEvento);
                $("#txtFecha").val(parseJsonDate(response.Fecha));
                $("#txtExpositor").val(response.Expositor);
                $("#cboCosto").val(response.Costo);
                $("#txtHoras").val(response.Horas);
                $($("#fileFoto").next().children()[0]).val("FOTO" + row + ".PDF");
                $($("#fileFotocheck").next().children()[0]).val("FOTOCHECK" + row + ".PDF");
                $($("#fileCertificado").next().children()[0]).val("CERTIFICADO" + row + ".PDF");
                $($("#fileCertificadoImprimir").next().children()[0]).val("CERTIFICADO-IMPRESION" + row + ".PDF");
                $($("#fileCertificadoExpositor").next().children()[0]).val("CERTIFICADO-EXPOSITOR" + row + ".PDF");
                EditarDocumento1 = false;
                EditarDocumento2 = false;
                EditarDocumento3 = false;
                EditarDocumento4 = false;
                EditarDocumento5 = false;
                $("#modalEvento").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });


    },
    DeleteEvento: function (IdEvento) {
        var request = new Object();
        request.IdEvento = IdEvento;

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
                            url: urlDeleteEvento,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridEventos').DataTable().ajax.reload();
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
    UpdateEstadoEvento: function (IdEvento, Activo) {
        var request = new Object();
        request.IdEvento = IdEvento;
        request.Activo = Activo;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: '¿Esta seguro de cambiar el estado del evento?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {

                        DBR.blockUIStar();
                        $.ajax({
                            url: urUpdateEstadoEvento,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridEventos').DataTable().ajax.reload();
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
    //Modulo
    Modulos: function (IdEvento) {
        $("#txtIdEvento2").val(IdEvento);
        $('#gridModulos').DataTable().ajax.reload();
        $("#modalModulosLista").modal("show");
    },
    SaveModulo: function () {

        var request = new Object();
        request.IdModulo = $("#txtIdModulo").val();
        request.IdEvento = $("#txtIdEvento2").val();
        request.Nombre = $("#txtModuloNombre").val();
        request.Descripcion = $("#txtModuloDescripcion").val();
        request.Expositor = $("#txtModuloExpositor").val();
        request.Horas = $("#txtModuloHoras").val();

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveModulo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    $('#gridModulos').DataTable().ajax.reload();
                    $("#modalModulo").modal("hide");
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
    },
    GetModulo: function (IdModulo) {

        $("#lblTitleModal").html("Editar");
        Funciones.LimpiarCamposModulo();

        var request = new Object();
        request.IdModulo = IdModulo;

        DBR.blockUIStar();
        $.ajax({
            url: urlGetModulo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();

                $("#txtIdModulo").val(response.IdModulo);
                $("#txtModuloNombre").val(response.Nombre);
                $("#txtModuloDescripcion").val(response.Descripcion);
                $("#txtModuloExpositor").val(response.Expositor);
                $("#txtModuloHoras").val(response.Horas);

                $("#modalModulo").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });


    },
    DeleteModulo: function (IdModulo) {
        var request = new Object();
        request.IdModulo = IdModulo;

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
                            url: urlDeleteModulo,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridModulos').DataTable().ajax.reload();
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
}
var Funciones = {
    LimpiarCamposEvento: function () {
        $("#fileFoto").filestyle('clear');
        $("#fileFoto").parent().removeClass("has-error");
        $("#fileFoto").next().next().addClass("hide");

        $("#fileFotocheck").filestyle('clear');
        $("#fileFotocheck").parent().removeClass("has-error");
        $("#fileFotocheck").next().next().addClass("hide");

        $("#fileCertificado").filestyle('clear');
        $("#fileCertificado").parent().removeClass("has-error");
        $("#fileCertificado").next().next().addClass("hide");

        $("#fileCertificadoImprimir").filestyle('clear');
        $("#fileCertificadoImprimir").parent().removeClass("has-error");
        $("#fileCertificadoImprimir").next().next().addClass("hide");

        $("#fileCertificadoExpositor").filestyle('clear');
        $("#fileCertificadoExpositor").parent().removeClass("has-error");
        $("#fileCertificadoExpositor").next().next().addClass("hide");

        DBR.limpiarCampo("#txtNombreEvento");
        DBR.limpiarCampo("#txtFecha");
        DBR.limpiarCampo("#txtExpositor");
        DBR.limpiarCampo("#txtHoras");
        DBR.limpiarCampo("#cboCosto");
    },
    LimpiarCamposModulo: function () {
        DBR.limpiarCampo("#txtModuloNombre");
        DBR.limpiarCampo("#txtModuloDescripcion");
        DBR.limpiarCampo("#txtModuloExpositor");
        DBR.limpiarCampo("#txtModuloHoras");
    }
}
$(document).ready(function () {
    //config.ini();
    config.iniGrids.GrillaEventos();
    config.iniGrids.GrillaModulo();

    $('#txtFecha').datepicker({
        autoclose: true,
        language: 'es',
        format: "dd/mm/yyyy",
        startDate: new Date()
    });

    $('#add-course-tab').steps({
        onStepChanging: function (event, currentIndex, newIndex) {

            if (newIndex < currentIndex) {
                return true;
            }

            if (currentIndex == 0) { //I suppose 0 is the first step
                return Eventos.SaveCurso();
            } else if (currentIndex == 1) {
                return Eventos.UpdateCursoFiles();
            } else if (currentIndex == 2) {
                if (modulosRegistrados > 0 && leccionesRegistradas > 0) {
                    return true;
                } else {
                    DBR.ToastError("Debe ingresar al menos un módulo y una lección.");
                }
            }
        },
        onFinish: function () {
            BDR.ToastSuccess("Curso registrado correctamente.");
            window.location.href = urlCursos;
        }
    });

    $("#txtHoras").numericInput({ allowFloat: false, allowNegative: false });
    $("#txtModuloHoras").numericInput({ allowFloat: false, allowNegative: false });

    $("#fileFoto").on("change", function () {
        EditarDocumento1 = true;
    });
    $("#fileFotocheck").on("change", function () {
        EditarDocumento2 = true;
    });
    $("#fileCertificado").on("change", function () {
        EditarDocumento3 = true;
    });
    $("#fileCertificadoImprimir").on("change", function () {
        EditarDocumento4 = true;
    });
    $("#fileCertificadoExpositor").on("change", function () {
        EditarDocumento5 = true;
    });
    $("#btnAgregarEvento").on("click", function () {
        EditarDocumento1 = true;
        EditarDocumento2 = true;
        EditarDocumento3 = true;
        EditarDocumento4 = true;
        EditarDocumento5 = true;
        $("#lblTitleAgregarEvento").html("Agregar");
        Funciones.LimpiarCamposEvento();
        $("#modalEvento").modal("show");
    });
    $("#btnGuardarEvento").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#txtNombreEvento")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtFecha")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtExpositor")) {
            validar = false;
        }
        if (!DBR.validarCampo("#cboCosto")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtHoras")) {
            validar = false;
        }
        if (EditarDocumento1) {
            var input = document.getElementById("fileFoto");
            if (input.files.length == 0) {
                $("#fileFoto").parent().addClass("has-error");
                $("#fileFoto").next().next().removeClass("hide");
                validar = false;
            } else {
                $("#fileFoto").parent().removeClass("has-error");
                $("#fileFoto").next().next().addClass("hide");
                if (input.files[0].size > 3145728) {
                    DBR.ToastWarning("Verifique que el archivo adjunto no supere el 3MB");
                    validar = false;
                }
            }
        }
        //if (EditarDocumento2) {
        //    var input2 = document.getElementById("fileFotocheck");
        //    if (input2.files.length == 0) {
        //        $("#fileFotocheck").parent().addClass("has-error");
        //        $("#fileFotocheck").next().next().removeClass("hide");
        //        validar = false;
        //    } else {
        //        $("#fileFotocheck").parent().removeClass("has-error");
        //        $("#fileFotocheck").next().next().addClass("hide");
        //        if (input2.files[0].size > 3145728) {
        //            DBR.ToastWarning("Verifique que el archivo adjunto no supere el 3MB");
        //            validar = false;
        //        }
        //    }
        //}
        //if (EditarDocumento3) {
        //    var input3 = document.getElementById("fileCertificado");
        //    if (input3.files.length == 0) {
        //        $("#fileCertificado").parent().addClass("has-error");
        //        $("#fileCertificado").next().next().removeClass("hide");
        //        validar = false;
        //    } else {
        //        $("#fileCertificado").parent().removeClass("has-error");
        //        $("#fileCertificado").next().next().addClass("hide");
        //        if (input3.files[0].size > 3145728) {
        //            DBR.ToastWarning("Verifique que el archivo adjunto no supere el 3MB");
        //            validar = false;
        //        }
        //    }
        //}
        if (EditarDocumento4) {
            var input4 = document.getElementById("fileCertificadoImprimir");
            if (input4.files.length == 0) {
                $("#fileCertificadoImprimir").parent().addClass("has-error");
                $("#fileCertificadoImprimir").next().next().removeClass("hide");
                validar = false;
            } else {
                $("#fileCertificadoImprimir").parent().removeClass("has-error");
                $("#fileCertificadoImprimir").next().next().addClass("hide");
                if (input4.files[0].size > 3145728) {
                    DBR.ToastWarning("Verifique que el archivo adjunto no supere el 3MB");
                    validar = false;
                }
            }
        }
        if (EditarDocumento5) {
            var input4 = document.getElementById("fileCertificadoExpositor");
            if (input4.files.length == 0) {
                $("#fileCertificadoExpositor").parent().addClass("has-error");
                $("#fileCertificadoExpositor").next().next().removeClass("hide");
                validar = false;
            } else {
                $("#fileCertificadoExpositor").parent().removeClass("has-error");
                $("#fileCertificadoExpositor").next().next().addClass("hide");
                if (input4.files[0].size > 3145728) {
                    DBR.ToastWarning("Verifique que el archivo adjunto no supere el 3MB");
                    validar = false;
                }
            }
        }

        if (validar) {
            Eventos.SaveEvento();
        }
    });
    //Modulo
    $("#btnAgregarModulo").on("click", function () {
        $("#lblTitleModal").html("Agregar");
        $("#txtIdModulo").val(0);
        Funciones.LimpiarCamposModulo();
        $("#modalModulo").modal("show");
    });
    $("#btnGuardarModulo").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#txtModuloNombre")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtModuloDescripcion")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtModuloExpositor")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtModuloHoras")) {
            validar = false;
        }
        if (validar) {
            Eventos.SaveModulo();
        }
    });
    //Eventos
    $("#modalModulo").on("hidden.bs.modal", function () {
        $("body").addClass("modal-open");
    });
});