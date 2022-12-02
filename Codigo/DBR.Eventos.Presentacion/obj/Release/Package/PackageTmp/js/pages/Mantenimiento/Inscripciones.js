var PrimeraCarga = 0;
var contenidoHtml = "";

var config = {
    iniGrids: {
        GrillaInscripciones: function () {
            $('#gridInscripciones').DataTable({
                "searching": true,
                "ordering": false,
                "processing": true,
                "serverSide": true,
                "destroy": true,
                "stateSave": true,
                "language": {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                "ajax": {
                    "url": urlListInscripcion,
                    "type": 'POST',
                    "datatype": "json",
                    "data": {
                        "IdEvento": function () {
                            return $("#cboEventosActivos").val();
                        }
                    },
                },
                "columns": [
                            //{ "data": "ApellidoPaterno" },
                            //{ "data": "ApellidoMaterno" },
                            {
                                "data": function (row, type, set, meta) {
                                    return row.TipoInscripcion == 1 ? "PART" : "EXPO";
                                }, "sClass": 'text-center'
                            },
                            { "data": "Nombre" },
                            { "data": "NumeroDocumento", "sClass": 'text-center' },
                            { "data": "Celular" },
                            //{ "data": "Correo" },
                            //{
                            //    "data": function (row, type, set, meta) {
                            //        if (row.EntregaCertificado) {
                            //            return '<i class="fa fa-check"></i>';
                            //        } else {
                            //            return '';
                            //        }
                            //    }, "sClass": 'text-center'
                            //},                            
                            { "data": "NombreEstadoPago" },
                            { "data": "NombreTipoPago" },
                            { "data": "Modalidad" },
                            {
                                "data": function (row, type, set, meta) {
                                    return row.Monto == null ? "" : row.Monto.toFixed(2);
                                }, "sClass": 'text-right'
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return parseJsonDate(row.FechaOperacion);
                                }, "sClass": 'text-center'
                            },
                            { "data": "NumeroOperacion", "sClass": 'text-center' },
                             {
                                 "data": function (row, type, set, meta) {
                                     if (row.Certificado == null) {
                                         return "Pendiente"
                                     } else {
                                         return "Cargado";
                                     }
                                 }, "sClass": 'text-center'
                             },
                             {
                                 "data": function (row, type, set, meta) {
                                     return row.Nota == null ? "" : row.Nota.toFixed(2);
                                 }, "sClass": 'text-center'
                             },
                             {
                                 "data": function (row, type, set, meta) {
                                     var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetInscripcion(' + row.IdInscripcion + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                                     var entregaCertificado = '<a data-toggle="tooltip1" data-placement="bottom" title="Entrega certificado" onclick="Eventos.EntregaCertificado(' + row.IdInscripcion + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span></a>';
                                     var accesousuario = '<a data-toggle="tooltip1" data-placement="bottom" title="Acceso usuario" onclick="Eventos.GetEventoUsuario(' + IsNull(row.IdUsuario, 0) + ',' + row.IdPersona + ')" class="btn btn-primary btn-xs"><span class="fa fa-user" aria-hidden="true"></span></a>';
                                     var enviarIndividual = '<a data-toggle="tooltip1" data-placement="bottom" title="Enviar correo" onclick="Eventos.EnviarCorreoIndividual(' + row.IdPersona + ',\'' + row.Nombres + ' ' + row.ApellidoPaterno + ' ' + row.ApellidoMaterno + '\',\'' + row.Correo + '\')" class="btn btn-primary btn-xs"><span class="fa fa-send" aria-hidden="true"></span></a>';
                                     var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteInscripcion(' + row.IdInscripcion + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                                     let CertificadoScrip = row.TipoInscripcion == 1 ? row.NombreCertificadoImprimir : row.NombreCertificadoExpositor;
                                     var verCertificado = '<a data-toggle="tooltip1" data-placement="bottom" title="Ver certificado" onclick="Eventos.VerCertificado(' +
                                         row.IdInscripcion + ',' +
                                         row.IdEvento + ',' +
                                         row.IdPersona + ',' +
                                         row.TipoInscripcion + ',' +
                                         row.NumeroCertificado + ',\'' +
                                         row.rowguid + '\',\'' +
                                         CertificadoScrip + '\',\'' +
                                         row.Nombres + '\',\'' +
                                         row.ApellidoPaterno + '\',\'' +
                                         row.ApellidoMaterno + '\',\'' +
                                         row.TipoOcupacionAbreviatura + '\',' +
                                         row.Nota + ',' +
                                         row.DetallarCertificado +
                                         ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span></a>';
                                     var CargarCertificado1 = '<a data-toggle="tooltip1" data-placement="bottom" title="Cargar automática" onclick="Eventos.CargarCertificadoUtomatico(' +
                                         row.IdInscripcion + ',' +
                                         row.IdEvento + ',' +
                                         row.IdPersona + ',' +
                                         row.TipoInscripcion + ',' +
                                         row.NumeroCertificado + ',\'' +
                                         row.rowguid + '\',\'' +
                                         CertificadoScrip + '\',\'' +
                                         row.Nombres + '\',\'' +
                                         row.ApellidoPaterno + '\',\'' +
                                         row.ApellidoMaterno + '\',\'' +
                                         row.TipoOcupacionAbreviatura + '\',' +
                                         row.Nota + ',' +
                                         row.DetallarCertificado +
                                         ')" class="btn btn-primary btn-xs"><span class="fa fa-file-pdf-o" aria-hidden="true"></span></a>';
                                     var CargarCertificado2 = '<a data-toggle="tooltip1" data-placement="bottom" title="Cargar manual" onclick="Eventos.CargarCertificado(' + row.IdInscripcion + ')" class="btn btn-primary btn-xs"><span class="fa fa-cloud-upload" aria-hidden="true"></span></a>';
                                     if (row.EntregaCertificado) {
                                         entregaCertificado = '';
                                         eliminar = '';
                                     }
                                     return entregaCertificado + " " + verCertificado + " " + CargarCertificado1 + " " + CargarCertificado2 + " " + accesousuario + " " + enviarIndividual +" " + editar + " " + eliminar;
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
    ListEventoCombo: function () {
        $.ajax({
            url: urlListEventoCombo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                DBR.LlenarCombo(response, "#cboEvento", true, false);
                var ultimoValor = response[response.length - 1].Value;
                $("#cboEvento").val(ultimoValor).trigger("change");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    ValoresIniciales: function () {

        DBR.blockUIStar();

        $.ajax({
            url: urlValoresIniciales,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                DBR.LlenarCombo(response.EstadoPago, "#cboEstadoPago", false, false, "Seleccione");
                DBR.LlenarCombo(response.TipoPago, "#cboTipoPago", false, false, "Seleccione");
                DBR.LlenarCombo(response.Ocupacion, "#cboTipoOcupacion", false, false, "Seleccione");
                DBR.LlenarCombo(response.Profesion, "#cboProfesion", false, false, "Seleccione");
                DBR.LlenarCombo(response.Evento, "#cboEventosActivos", true, false, "Seleccione evento");
                DBR.LlenarCombo(response.Modalidad, "#cboModalidad", true, false, "Seleccione");
                DBR.LlenarCombo(response.Pais, "#cboPais", true, false, "Seleccione");
                DBR.blockUIStop();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    SaveInscripcion: function () {
        var request1 = new Object();
        request1.IdInscripcion = $("#txtIdInscripcion").val();
        request1.IdEvento = $("#cboEventosActivos").val();
        request1.EstadoPago = $("#cboEstadoPago").val();
        request1.TipoPago = $("#cboTipoPago").val();
        request1.TipoModalidad = $("#cboModalidad").val();
        request1.Monto = $("#txtMonto").val();
        request1.NombreBanco = $.trim($("#txtNombreBanco").val().toUpperCase());
        request1.FechaOperacion = $("#txtFechaOperacion").val();
        request1.NumeroOperacion = $.trim($("#txtNumeroOperacion").val().toUpperCase());
        request1.Nota = $("#txtNota").val();
        request1.TipoInscripcion = $("#cboTipoInscripcion").val();
        request1.Ruc = $("#txtRuc").val();

        var request2 = new Object();
        request2.Nombres = $.trim($("#txtNombres").val().toUpperCase());
        request2.ApellidoPaterno = $.trim($("#txtApellidoPaterno").val().toUpperCase());
        request2.ApellidoMaterno = $.trim($("#txtApellidoMaterno").val().toUpperCase());
        request2.NumeroDocumento = $.trim($("#txtDni").val());
        request2.TipoOcupacion = $("#cboTipoOcupacion").val();
        request2.CIP = $.trim($("#txtCip").val());
        request2.Celular = $.trim($("#txtCelular").val());
        request2.Correo = $.trim($("#txtCorreo").val().toUpperCase());
        request2.DescripcionOcupacion = $.trim($("#txtDescripcionOcupacion").val().toUpperCase());
        request2.IdProfesion = $("#cboProfesion").val();
        request2.IdPais = $("#cboPais").val();
        request2.Ciudad = $("#txtCiudad").val().toUpperCase();

        DBR.blockUIStar();
        $.ajax({
            url: urlSaveInscripcion,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request1": request1, "request2": request2 }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    $('#gridInscripciones').DataTable().ajax.reload(null, false);
                    $("#modalInscripcion").modal("hide");
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
    DeleteInscripcion: function (IdInscripcion) {
        var request = new Object();
        request.IdInscripcion = IdInscripcion;

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
                            url: urlDeleteInscripcion,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridInscripciones').DataTable().ajax.reload(null, false);
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
    EntregaCertificado: function (IdInscripcion) {
        var request = new Object();
        request.IdInscripcion = IdInscripcion;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: '¿Marcar certificado entregado?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {

                        DBR.blockUIStar();
                        $.ajax({
                            url: urlUpdateEntregaCertificadoInscripcion,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridInscripciones').DataTable().ajax.reload(null, false);
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
    GetPersona: function () {
        var request = new Object();
        request.NumeroDocumento = $("#txtDni").val();

        DBR.blockUIStar();
        $.ajax({
            url: urlGetPersonaXdni,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.length > 0) {
                    $("#txtNombres").val(response[0].Nombres);
                    $("#txtApellidoPaterno").val(response[0].ApellidoPaterno);
                    $("#txtApellidoMaterno").val(response[0].ApellidoMaterno);
                    $("#cboTipoOcupacion").val(response[0].TipoOcupacion);
                    $("#txtCip").val(response[0].CIP);
                    $("#txtCelular").val(response[0].Celular);
                    $("#txtCorreo").val(response[0].Correo);
                    $("#txtDescripcionOcupacion").val(response[0].DescripcionOcupacion);
                    if (response[0].IdProfesion != null) {
                        $("#cboProfesion").val(response[0].IdProfesion).trigger("change");
                    } else {
                        $("#cboProfesion").val(0).trigger("change");
                    }
                    if (response[0].IdPais != null) {
                        $("#cboPais").val(response[0].IdPais).trigger("change");
                    } else {
                        $("#cboPais").val(0).trigger("change");
                    }
                    $("#txtCiudad").val(response[0].Ciudad);

                    DBR.ToastSuccess("Persona encontrada");
                } else {
                    $("#txtNombres").val("");
                    $("#txtApellidoPaterno").val("");
                    $("#txtApellidoMaterno").val("");
                    $("#txtOcupacion").val("");
                    $("#txtCip").val("");
                    $("#txtCelular").val("");
                    $("#txtCorreo").val("");
                    $("#txtDescripcionOcupacion").val("");
                    $("#cboProfesion").val(0).trigger("change");
                    $("#cboPais").val(0).trigger("change");
                    $("#txtCiudad").val("");
                    DBR.ToastWarning("No se encontro persona");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    GetPersonaXcip: function () {
        var request = new Object();
        request.Cip = $("#txtCip").val();

        DBR.blockUIStar();
        $.ajax({
            url: urlGetPersonaXcip,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.length > 0) {
                    $("#txtDni").val(response[0].NumeroDocumento);
                    $("#txtNombres").val(response[0].Nombres);
                    $("#txtApellidoPaterno").val(response[0].ApellidoPaterno);
                    $("#txtApellidoMaterno").val(response[0].ApellidoMaterno);
                    $("#cboTipoOcupacion").val(response[0].TipoOcupacion);
                    $("#txtCip").val(response[0].CIP);
                    $("#txtCelular").val(response[0].Celular);
                    $("#txtCorreo").val(response[0].Correo);
                    $("#txtDescripcionOcupacion").val(response[0].DescripcionOcupacion);
                    if (response[0].IdProfesion != null) {
                        $("#cboProfesion").val(response[0].IdProfesion).trigger("change");
                    } else {
                        $("#cboProfesion").val(0).trigger("change");
                    }
                    if (response[0].IdPais != null) {
                        $("#cboPais").val(response[0].IdPais).trigger("change");
                    } else {
                        $("#cboPais").val(0).trigger("change");
                    }
                    $("#txtCiudad").val(response[0].Ciudad);
                    DBR.ToastSuccess("Persona encontrada");
                } else {
                    $("#txtDni").val("");
                    $("#txtNombres").val("");
                    $("#txtApellidoPaterno").val("");
                    $("#txtApellidoMaterno").val("");
                    $("#txtOcupacion").val("");
                    $("#txtCelular").val("");
                    $("#txtCorreo").val("");
                    $("#txtDescripcionOcupacion").val("");
                    $("#cboProfesion").val(0).trigger("change");
                    $("#cboPais").val(0).trigger("change");
                    $("#txtCiudad").val("");
                    DBR.ToastWarning("No se encontro persona");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    GetInscripcion: function (IdInscripcion) {
        var request = new Object();
        request.IdInscripcion = IdInscripcion;

        Funciones.LimpiarCamposInscripcion();

        DBR.blockUIStar();
        $.ajax({
            url: urlGetInscripcion,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                $("#txtIdInscripcion").val(IdInscripcion);
                $("#txtDni").val(response[0].NumeroDocumento);
                $("#txtNombres").val(response[0].Nombres);
                $("#txtApellidoPaterno").val(response[0].ApellidoPaterno);
                $("#txtApellidoMaterno").val(response[0].ApellidoMaterno);
                $("#cboUniversidad").val(response[0].IdUniversidad).trigger("change");
                $("#cboTipoOcupacion").val(response[0].TipoOcupacion);
                $("#txtCip").val(response[0].CIP);
                $("#txtCelular").val(response[0].Celular);
                $("#txtCorreo").val(response[0].Correo);
                $("#txtDescripcionOcupacion").val(response[0].DescripcionOcupacion);
                if (response[0].IdPais != null) {
                    $("#cboPais").val(response[0].IdPais).trigger("change");
                } else {
                    $("#cboPais").val(0).trigger("change");
                }
                $("#txtCiudad").val(response[0].Ciudad);


                $("#cboEstadoPago").val(response[0].EstadoPago);
                $("#cboTipoPago").val(response[0].TipoPago);
                if (response[0].IdProfesion != null) {
                    $("#cboProfesion").val(response[0].IdProfesion).trigger("change");
                } else {
                    $("#cboProfesion").val(0).trigger("change");
                }
                $("#cboModalidad").val(response[0].TipoModalidad).trigger("change");
                $("#txtMonto").val(response[0].Monto == null ? "" : response[0].Monto.toFixed(2));
                $("#txtNombreBanco").val(response[0].NombreBanco);
                $("#txtFechaOperacion").datepicker("setDate", parseJsonDate(response[0].FechaOperacion));
                $("#txtNumeroOperacion").val(response[0].NumeroOperacion);
                $("#txtNota").val(response[0].Nota);
                $("#cboTipoInscripcion").val(response[0].TipoInscripcion);
                $("#txtRuc").val(response[0].Ruc);
                $("#modalInscripcion").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    VerCertificado: function (IdInscripcion, IdEvento, IdPersona, TipoInscripcion, NumeroCertificado, rowguid, NombreCertificadoImprimir, Nombres, ApellidoPaterno, ApellidoMaterno, TipoOcupacionAbreviatura, Nota, DetallarCertificado) {

        var request = new Object();
        request.IdInscripcion = IdInscripcion;
        request.IdEvento = IdEvento;
        request.IdPersona = IdPersona;
        request.NumeroCertificado = NumeroCertificado;
        request.rowguid = rowguid;
        request.Nota = Nota;
        request.TipoInscripcion = TipoInscripcion;

        var documentos = new Object();
        documentos.DocumentoCertificadoImprimir = NombreCertificadoImprimir;
        documentos.IdEvento = IdEvento;
        documentos.DetallarCertificado = DetallarCertificado;

        var persona = new Object();
        persona.ApellidoPaterno = ApellidoPaterno;
        persona.ApellidoMaterno = ApellidoMaterno;
        persona.Nombres = Nombres;
        persona.TipoOcupacionAbreviatura = TipoOcupacionAbreviatura;

        DBR.blockUIStar();
        $.ajax({
            url: urlVerPdfCertificado,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request, "documentos": documentos, "persona": persona }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    UrlViewPdfIframe = UrlViewPdfIframe.replace("/Mantenimiento", "");
                    var Url = UrlViewPdfIframe + "/pdfjs/web/viewer.html" + '?file=' + response.Message;
                    $("#visorPdf").attr('src', Url);
                    $("#modalVerDocumento").modal("show");
                } else {
                    DBR.ToastError(response.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(XMLHttpRequest.responseText);
            }
        });

    },
    CargarCertificadoUtomatico: function (IdInscripcion, IdEvento, IdPersona, TipoInscripcion, NumeroCertificado, rowguid, NombreCertificadoImprimir, Nombres, ApellidoPaterno, ApellidoMaterno, TipoOcupacionAbreviatura, Nota, DetallarCertificado) {

        var request = new Object();
        request.IdInscripcion = IdInscripcion;
        request.IdEvento = IdEvento;
        request.IdPersona = IdPersona;
        request.NumeroCertificado = NumeroCertificado;
        request.rowguid = rowguid;
        request.Nota = Nota;
        request.TipoInscripcion = TipoInscripcion;

        var documentos = new Object();
        documentos.DocumentoCertificadoImprimir = NombreCertificadoImprimir;
        documentos.IdEvento = IdEvento;
        documentos.DetallarCertificado = DetallarCertificado;

        var persona = new Object();
        persona.ApellidoPaterno = ApellidoPaterno;
        persona.ApellidoMaterno = ApellidoMaterno;
        persona.Nombres = Nombres;
        persona.TipoOcupacionAbreviatura = TipoOcupacionAbreviatura;

        DBR.blockUIStar();
        $.ajax({
            url: urlCargarPdfCertificadoUtomatico,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request, "documentos": documentos, "persona": persona }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    DBR.ToastSuccess(response.Message);
                } else {
                    DBR.ToastError(response.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(XMLHttpRequest.responseText);
            }
        });

    },
    CargarCertificado: function (IdInscripcion) {
        $("#txtIdInscripcionCertificado").val(IdInscripcion);
        $("#fileCertificadoImprimir").parent().removeClass("has-error");
        $("#fileCertificadoImprimir").next().next().addClass("hide");
        $("#fileCertificadoImprimir").filestyle("clear");
        $("#modalCargaCertificado").modal("show");
    },
    SaveCertificadoFirmado: function () {
        var input1 = document.getElementById("fileCertificadoImprimir");


        var formData = new FormData();
        formData.append("documento", input1.files[0]);
        formData.append("IdInscripcion", $("#txtIdInscripcionCertificado").val());

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveCertificadoFirmado,
            type: "POST",
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            data: formData
        }).done(function (response) {
            DBR.blockUIStop();
            if (response.IsSuccess) {
                $('#gridInscripciones').DataTable().ajax.reload(null, false);
                $("#modalCargaCertificado").modal("hide");
                DBR.ToastSuccess(response.Message);
            } else {
                DBR.ToastError(response.Message);
            }
        }).fail(function (XMLHttpRequest) {
            DBR.blockUIStop();
            console.log(XMLHttpRequest);
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
    EnviarCorreo: function () {
        var request1 = new Object();
        request1.IdEvento = $("#cboEventosActivos").val();

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: 'Se enviar correo a todas las personas inscritas </br></br>¿Esta seguro continuar?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {

                        DBR.blockUIStar("Enviando...");

                        $.ajax({
                            url: urlSaveCorreoInscritos,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request1": request1 }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
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
    EnviarCorreoIndividual: function (IdPersona, NombreCompleto, Correo) {
        var request1 = new Object();
        request1.IdEvento = $("#cboEventosActivos").val();

        var request = new Object();
        request.IdPersona = IdPersona;
        request.NombreCompleto = NombreCompleto;
        request.Correo = Correo;

        DBR.blockUIStar("Enviando...");

        $.ajax({
            url: urlSaveCorreoInscritosIndividual,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request1": request1, "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
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
    //Acceso por Usuario
    GetEventoUsuario: function (IdUsuario, IdPersona) {
        $("#txtIdUsuario").val(IdUsuario);
        if (IdUsuario == 0) {
            Eventos.CrarUsuarioNuevo(IdPersona);
        } else {
            var request = new Object();
            request.IdUsuario = IdUsuario;
            request.IdEvento = $("#cboEventosActivos").val();

            DBR.blockUIStar();
            $.ajax({
                url: urlGetEventoUsuario,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ "request": request }),
                success: function (response) {
                    DBR.blockUIStop();
                    Funciones.LimpiarCamposEventoUsuario();
                    if (response == "NULL") {
                        $("#txtIdEventoUsuario").val(0);
                        $('#txtFechaInicio').datepicker("setDate", "");
                        $('#txtFechaFin').datepicker("setDate", "");
                    } else {
                        $("#txtIdEventoUsuario").val(response.IdEventoUsuario);
                        $('#txtFechaInicio').datepicker("setDate", parseJsonDate(response.FechaInicio));
                        $('#txtFechaFin').datepicker("setDate", parseJsonDate(response.FechaFin));
                    }
                    $("#modalUsuarioEvento").modal("show");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    DBR.blockUIStop();
                    console.log(textStatus);
                }
            });
        }                
    },
    SaveEventoUsuario: function () {

        var request = new Object();
        request.IdEventoUsuario = $("#txtIdEventoUsuario").val();
        request.IdEvento = $("#cboEventosActivos").val();
        request.IdUsuario = $("#txtIdUsuario").val();
        request.FechaInicio = $("#txtFechaInicio").val();
        request.FechaFin = $("#txtFechaFin").val();
        console.log(request);
        var requestVideos = [];

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveEventoUsuario,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request, "requestVideos": requestVideos }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {                  
                    $("#modalUsuarioEvento").modal("hide");
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
    CrarUsuarioNuevo: function (IdPersona) {
        let request = new Object();
        request.IdPersona = IdPersona;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: 'No se encontro usuario registrado <br/><br/> ¿Desea crear un usuario para esta persona?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {
                      
                        DBR.blockUIStar()
                        $.ajax({
                            url: urlSaveUsuarioAutomativo,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                if (response.IsSuccess) {
                                    dialogItself.close();
                                    DBR.ToastSuccess(response.Message);
                                    $('#gridInscripciones').DataTable().ajax.reload(null, false);
                                    $("#txtIdUsuario").val(response.Codigo);
                                    $("#modalUsuarioEvento").modal("show");                                    
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
    //EventoCorreo
    GetEventoCorreo: function () {

        var request = new Object();
        request.IdEvento = $("#cboEventosActivos").val();

        DBR.blockUIStar();
        $.ajax({
            url: urlGeEventoCorreoByIdEvento,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response != "NULL") {
                    $("#txtIdEventoCorreo").val(response.IdEventoCorreo);
                    $("#txtAsuntoMensaje").val(response.Asunto);
                    $("#txtCorreoOrigen").val(response.Origen);
                    $("#txtNombreOrigen").val(response.NombreOrigen);
                    CKEDITOR.instances.txtMensajeCompleto.setData(response.Mensaje);
                    contenidoHtml = response.Mensaje;
                } else {
                    $("#txtIdEventoCorreo").val(0);
                    $("#txtAsuntoMensaje").val("");
                    $("#txtCorreoOrigen").val(CorreoOrigen);
                    $("#txtNombreOrigen").val(NombreCorreoOrigen);
                    CKEDITOR.instances.txtMensajeCompleto.setData("");
                    contenidoHtml = "";
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });
    },
    SaveEventoCorreo: function () {
        var request = new Object();
        request.IdEventoCorreo = $("#txtIdEventoCorreo").val();
        request.IdEvento = $("#cboEventosActivos").val();
        request.Asunto = $.trim($("#txtAsuntoMensaje").val().toUpperCase());
        request.Origen = $.trim($("#txtCorreoOrigen").val().toLowerCase());
        request.NombreOrigen = $.trim($("#txtNombreOrigen").val().toUpperCase());
        request.Mensaje = contenidoHtml;

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveEventoCorreo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    DBR.ToastSuccess(response.Message);
                    $("#modalAgregarCorreo").modal("hide");
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
}
var Funciones = {
    LimpiarCamposInscripcion: function () {
        $("#formInscripcion .validar").each(function () {
            DBR.limpiarCampo(this);
        });
        $("#cboTipoInscripcion").val(1);
        $("#txtCip").val("");
        $("#txtNombreBanco").val("");
        $("#txtFechaOperacion").datepicker("setDate", "");
        $("#txtNumeroOperacion").val("");
        $("#txtDescripcionOcupacion").val("");
        $("#txtCiudad").val("");
        $("#txtNota").val("");
        $("#txtRuc").val("");
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
    LimpiarCamposEventoUsuario: function () {
        DBR.limpiarCampo("#txtFechaInicio");
        DBR.limpiarCampo("#txtFechaFin");       
    },
}
$(document).ready(function () {
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };
    //Eventos.ListEventoCombo();
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };
    Eventos.ValoresIniciales();
    config.iniGrids.GrillaInscripciones();
    $('#txtFecha').datepicker({
        autoclose: true,
        language: 'es',
        format: "dd/mm/yyyy",
        startDate: new Date()
    });
    $('#txtFechaOperacion').datepicker({
        autoclose: true,
        language: 'es',
        format: "dd/mm/yyyy"
    });
    $("#txtDni").numericInput({ allowFloat: false, allowNegative: false });
    $("#txtCelular").numericInput({ allowFloat: false, allowNegative: false });
    $("#txtCip").numericInput({ allowFloat: false, allowNegative: false });
    $("#txtMonto").numericInput({ allowFloat: true, allowNegative: false });
    $("#txtNota").numericInput({ allowFloat: true, allowNegative: false, max: 20 });
    $("#txtRuc").numericInput({ allowFloat: true, allowNegative: false });
    $("#cboProfesion").select2();
    $("#cboPais").select2();

    $('#txtFechaInicio').datepicker({
        autoclose: true,
        language: 'es',
        format: "dd/mm/yyyy"
    });
    $('#txtFechaFin').datepicker({
        autoclose: true,
        language: 'es',
        format: "dd/mm/yyyy"
    });

    $("#txtDni").keypress(function (e) {
        if (e.which == 13) {
            Eventos.GetPersona();
        }
    });

    $("#txtCip").keypress(function (e) {
        if (e.which == 13) {
            Eventos.GetPersonaXcip();
        }
    });
    $("#btnBuscarPorDni").on("click", function () {
        var valor = $("#txtDni").val().length;
        if (valor >= 8) {
            Eventos.GetPersona();
        } else {
            DBR.ToastWarning("El dni no puede ser menos a 8 dígitos");
        }

    });
    $("#btnBuscarPorCip").on("click", function () {
        var valor = $("#txtCip").val().length;
        if (valor > 4) {
            Eventos.GetPersonaXcip();
        } else {
            DBR.ToastWarning("El CIP no puede ser menos a 4 dígitos");
        }
    });
    $("#cboEvento").on("change", function () {
        config.iniGrids.GrillaInscripciones();
    });
    $("#btnAgregarInscripcion").on("click", function () {
        $("#lblTitleAgregarInscripcion").html("Agregar");
        $("#txtIdInscripcion").val(0);
        if (DBR.validarCampo("#cboEventosActivos")) {
            Funciones.LimpiarCamposInscripcion();
            $("#modalInscripcion").modal("show");
        } else {
            DBR.ToastWarning("Seleccione un evento");
        }
    });
    $("#btnGuardarInscripcion").on("click", function () {
        var validar = true;
        $("#formInscripcion .validar").each(function () {
            if (!DBR.validarCampo(this)) {
                validar = false;
            }
        });
        if ($.trim($("#txtDni").val()) != "") {
            if ($.trim($("#txtDni").val()).length < 8) {
                DBR.ToastWarning("El dni no puede ser menos a 8 dígitos");
                validar = false;
            }
        }

        if (validar) {
            Eventos.SaveInscripcion();
        }
    });
    $("#btnDescargarExcel").on("click", function () {
        let IdEvento = $("#cboEventosActivos").val();
        let NombreEvento = $("#cboEventosActivos option:selected").text();
        window.location.href = urlDescargarPersonasInscritas + "?IdEvento=" + IdEvento + "&NombreEvento=" + NombreEvento;
    });
    $("#cboEventosActivos").on("change", function () {
        $('#gridInscripciones').DataTable().ajax.reload(null, false);
    });
    $("#btnGuardarCertificado").on("click", function () {
        var validar = true;
        var input = document.getElementById("fileCertificadoImprimir");
        if (input.files.length == 0) {
            $("#fileCertificadoImprimir").parent().addClass("has-error");
            $("#fileCertificadoImprimir").next().next().removeClass("hide");
            validar = false;
        } else {
            $("#fileCertificadoImprimir").parent().removeClass("has-error");
            $("#fileCertificadoImprimir").next().next().addClass("hide");
            if (input.files[0].length > 4194304) {
                DBR.ToastWarning("Verifique que el archivo adjunto no supere el 4MB");
                validar = false;
            }
        }
        if (validar) {
            Eventos.SaveCertificadoFirmado();
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
    $("#btnConfigurarCorreo").on("click", function () {
        if (DBR.validarCampo("#cboEventosActivos")) {
            Funciones.LimpiarControles();
            $("#txtCorreoOrigen").val(CorreoOrigen);
            $("#txtNombreOrigen").val(NombreCorreoOrigen);
            $("#modalAgregarCorreo").modal("show");
            Eventos.GetEventoCorreo();
        } else {
            DBR.ToastWarning("Seleccione un evento");
        }       
    });
    $("#btnEnviarCorreo").on("click", function () {
        if (DBR.validarCampo("#cboEventosActivos")) {
            Eventos.EnviarCorreo();
        } else {
            DBR.ToastWarning("Seleccione un evento");
        }
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
            Eventos.SaveEventoCorreo();
        }
    });
    //EventoUsuario
    $("#btnGuardarEventoUsuario").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#txtFechaInicio")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtFechaFin")) {
            validar = false;
        }
        if (validar) {
            Eventos.SaveEventoUsuario();
        }
    });
});