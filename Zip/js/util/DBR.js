function IsNull(value, result) {
    if (value == null) {
        return result;
    } else {
        return value;
    }
}
function strPad(i, l, s) {
    var o = i.toString();
    if (!s) { s = '0'; }
    while (o.length < l) {
        o = s + o;
    }
    return o;
}

function parseJsonDate(jsonDate) {
    if (jsonDate == null)
        return '';
    var value = new Date(parseInt(jsonDate.substr(6)));
    var ret = strPad(value.getDate(), 2) + "/" + strPad(value.getMonth() + 1, 2) + "/" + value.getFullYear();
    return ret;
}
function parseToday() {
    var value = new Date();
    var ret = strPad(value.getDate(), 2) + "/" + strPad(value.getMonth() + 1, 2) + "/" + value.getFullYear();
    return ret;
}
function parseJsonTime(jsonDate) {
    if (jsonDate == null)
        return '';
    var value = new Date(parseInt(jsonDate.substr(6)));
    var formt24 = "AM";
    var hora = value.getHours();
    if (hora >= 12) {
        formt24 = "PM";
        if (hora == 12) {
            hora = hora;
        } else {
            hora = hora - 12;
        }      
    }
    if (hora == 0) {
        hora = hora + 12;
    }
    var ret = strPad(hora, 2) + ":" + strPad(value.getMinutes(), 2) + " " + formt24;
    return ret;
}
function parseIntMes(numMes)
{
    var Mes = "";
    switch (numMes) {
        case 1:
            Mes = "ENERO";
            break;
        case 2:
            Mes = "FEBRERO";
            break;
        case 3:
            Mes = "MARZO";
            break;
        case 4:
            Mes = "ABRIL";
            break;
        case 5:
            Mes = "MAYO";
            break;
        case 6:
            Mes = "JUNIO";
            break;
        case 7:
            Mes = "JULIO";
            break;
        case 8:
            Mes = "AGOSTO";
            break;
        case 9:
            Mes = "SEPTIEMBRE";
            break;
        case 10:
            Mes = "OCTUBRE";
            break;
        case 11:
            Mes = "NOVIEMBRE";
            break;
        case 12:
            Mes = "DICIEMBRE";
            break;
        default:
            Mes = "";
    }
    return Mes;
}

var DBR = {   
    validarCampo: function (Control) {
        /// <summary>
        /// Valida un campo obligatorio
        /// </summary>
        /// <param name="#IdControl" type="String">Id del control a aplicar</param>
        tipo = $(Control);
        if (tipo[0].localName == "input") {
            if ($(Control).next().hasClass("input-group-addon") || $(Control).next().hasClass("input-group")) {
                if ($.trim($(Control).val()) == "") {
                    $(Control).parent().parent().addClass("has-error");
                    $(Control).parent().next().removeClass("hide");
                    return false;
                } else {
                    $(Control).parent().parent().removeClass("has-error");
                    $(Control).parent().next().addClass("hide");
                    return true;
                }
            } else {
                if ($.trim($(Control).val()) == "") {
                    $(Control).parent().addClass("has-error");
                    $(Control).next().removeClass("hide");
                    return false;
                } else {
                    $(Control).parent().removeClass("has-error");
                    $(Control).next().addClass("hide");
                    return true;
                }
            }           
        }
        if (tipo[0].localName == "textarea") {
            if ($.trim($(Control).val()) == "") {
                $(Control).parent().addClass("has-error");
                $(Control).next().removeClass("hide");
                return false;
            } else {
                $(Control).parent().removeClass("has-error");
                $(Control).next().addClass("hide");
                return true;
            }
        }
        if (tipo[0].localName == "select") {
            if ($(Control).hasClass("select2")) {
                if ($(Control).attr("multiple") == "multiple") {
                    if ($(Control).val().length == 0) {
                        $(Control).parent().addClass("has-error");
                        $(Control).next().next().removeClass("hide");
                        return false;
                    } else {
                        $(Control).parent().removeClass("has-error");
                        $(Control).next().next().addClass("hide");
                        return true;
                    }
                } else {
                    if ($(Control).val() == 0) {
                        $(Control).parent().addClass("has-error");
                        $(Control).next().next().removeClass("hide");
                        $(Control).next().children().children().addClass("select2-error");
                        return false;
                    } else {
                        $(Control).parent().removeClass("has-error");
                        $(Control).next().next().addClass("hide");
                        $(Control).next().children().children().removeClass("select2-error");
                        return true;
                    }
                }
                
            } else {
                if ($(Control).val() == 0) {
                    $(Control).parent().addClass("has-error");
                    $(Control).next().removeClass("hide");
                    return false;
                } else {
                    $(Control).parent().removeClass("has-error");
                    $(Control).next().addClass("hide");
                    return true;
                }
            }

        }

    },
    validarCheckIcon: function (Control) {
        if ($(Control).prop("checked")) {           
            $(Control).next().removeClass("check-error");
            return true;
        } else {
            $(Control).next().addClass("check-error");
            return false;
        }
    },
    validarConjuntoCampos: function (ControlContenedor) {
        /// <summary>
        /// valida un conjunto de campos dentro de un contenedor
        /// </summary>
        /// <param name="#IdControl" type="String">Id del control a aplicar</param>
        var contador = 0;
        var Validar = ControlContenedor + " .validar";
        $(Validar).each(function () {
            tipo = $(this);
            Control = this;
            if (tipo[0].localName == "input") {
                if ($.trim($(Control).val()) == "") {
                    $(Control).parent().addClass("has-error");
                    $(Control).next().removeClass("hide");
                    contador++;
                } else {
                    $(Control).parent().removeClass("has-error");
                    $(Control).next().addClass("hide");
                }
            }
            if (tipo[0].localName == "select") {
                if ($(Control).hasClass("select2")) {
                    if ($(Control).attr("multiple") == "multiple") {
                        if ($(Control).val() == null) {
                            $(Control).parent().addClass("has-error");
                            $(Control).next().next().removeClass("hide");
                            contador++;
                        } else {
                            $(Control).parent().removeClass("has-error");
                            $(Control).next().next().addClass("hide");
                        }
                    } else {
                        if ($(Control).val() == 0) {
                            $(Control).parent().addClass("has-error");
                            $(Control).next().next().removeClass("hide");
                            $(Control).next().children().children().addClass("select2-error");
                            contador++;
                        } else {
                            $(Control).parent().removeClass("has-error");
                            $(Control).next().next().addClass("hide");
                            $(Control).next().children().children().removeClass("select2-error");
                        }
                    }

                } else {
                    if ($(Control).val() == 0) {
                        $(Control).parent().addClass("has-error");
                        $(Control).next().removeClass("hide");
                        contador++;
                    } else {
                        $(Control).parent().removeClass("has-error");
                        $(Control).next().addClass("hide");
                    }
                }

            }
        });
        return (contador == 0);
    },
    limpiarConjuntoCampos: function (ControlContenedor) {
        /// <summary>
        /// Limpiar un conjunto de campos dentro de un contenedor
        /// </summary>
        /// <param name="#IdControl" type="String">Id del control a aplicar</param>
        $(ControlContenedor + " .validar").each(function () {
            tipo = $(this);
            Control = this;
            if (tipo[0].localName == "input") {
                $(Control).val("");
                $(Control).parent().removeClass("has-error");
                $(Control).next().addClass("hide");
            }
            if (tipo[0].localName == "select") {
                if ($(Control).hasClass("select2")) {
                    $(Control).val(0).trigger("change");
                    $(Control).parent().removeClass("has-error");
                    $(Control).next().next().addClass("hide");
                } else {
                    $(Control).val(0);
                    $(Control).parent().removeClass("has-error");
                    $(Control).next().addClass("hide");
                }
            }
        });
    },
    limpiarCampo: function (Control) {
        /// <summary>
        /// Limpia un campo
        /// </summary>
        /// <param name="#IdControl" type="String">Id del control a aplicar</param>
        tipo = $(Control);
        if (tipo[0].localName == "input") {
            if ($(Control).next().hasClass("input-group-addon") || $(Control).next().hasClass("input-group")) {
                $(Control).val("");
                $(Control).parent().parent().removeClass("has-error");
                $(Control).parent().next().addClass("hide");
            } else {
                $(Control).val("");
                $(Control).parent().removeClass("has-error");
                $(Control).next().addClass("hide");
            }
        }
        if (tipo[0].localName == "textarea") {
            $(Control).val("");
            $(Control).parent().removeClass("has-error");
            $(Control).next().addClass("hide");
        }
        if (tipo[0].localName == "select") {
            if ($(Control).hasClass("select2")) {
                if ($(Control).attr("multiple") == "multiple") {
                    $(Control + " > option").removeAttr("selected");
                    $(Control).val('').trigger("change");
                    $(Control).parent().removeClass("has-error");
                    $(Control).next().next().addClass("hide");
                } else {
                    $(Control).val(0).trigger("change");
                    $(Control).parent().removeClass("has-error");
                    $(Control).next().next().addClass("hide");
                    $(Control).next().children().children().removeClass("select2-error");
                }

            } else {
                $(Control).val(0);
                $(Control).parent().removeClass("has-error");
                $(Control).next().addClass("hide");
            }

        }

    },
    limpiarCheckIcon: function (Control) {
        $(Control).next().removeClass("check-error");
        $(Control).prop("checked", false);
    },
    ToUpperCase: function (IdControl) {
        /// <summary>
        /// Convierte a Mayúsculas el texto ingresado en un control
        /// </summary>
        /// <param name="IdControl" type="String">Id del control a aplicar</param>
        var objControl = $("[id$=" + IdControl + "]");

        objControl.keydown(function (e) {
            blnAlfanumericoCGR = false;
            e = e || window.event;
            var iKey = e.keycode || e.which || 0;
            if (iKey == 9 || iKey == 35 || iKey == 36 || iKey == 39 || iKey == 46 || iKey == 37) {
                //Tab, fin, inicio, Left, Right, Supr                                 
                blnAlfanumericoCGR = true;
                return true;
            }
        });
        objControl.keypress(function (e) {
            if (e.charCode >= 16 & e.charCode <= 17 || e.charCode >= 65 & e.charCode <= 90 || e.charCode >= 97 & e.charCode <= 122 || e.charCode == 241 || blnAlfanumericoCGR == true) {
                var pst = e.currentTarget.selectionStart;
                var string_start = e.currentTarget.value.substring(0, pst);
                var string_end = e.currentTarget.value.substring(pst, e.currentTarget.value.length);

                if (blnAlfanumericoCGR == true) {
                    return true;
                } else {
                    e.currentTarget.value = string_start + String.fromCharCode(e.charCode).toUpperCase() + string_end;
                    e.currentTarget.selectionEnd = pst + 1;
                    return false;
                }
            }
        });
    },
    MensajeError: function (Control, Mensaje, Mostrar) {
        if (Mostrar) {
            $(Control).removeClass("hide");
            $(Control).html('<span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span> ' + Mensaje);
        } else {
            $(Control).addClass("hide");
            $(Control).html("");
        }
    },
    MensajeInformativoSmall: function (Mensaje,Title) {
        var dialog =new BootstrapDialog({
            title: Title,
            closeByBackdrop: false,
            message: Mensaje,
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Aceptar',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });
        //dialog.realize();
        //dialog.getModalHeader().hide();
        dialog.open();
        //$("#contenidoModalInformativo").html(Mensaje);
        $("#ModalInformativo").modal("show");

    },
    MensajeInformativoNormal: function (Mensaje, Title) {
        var dialog = new BootstrapDialog({
            title: Title,
            closeByBackdrop: false,
            message: Mensaje,
            size: BootstrapDialog.SIZE_NORMAL,
            buttons: [
                {
                    label: 'Aceptar',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });

        dialog.open();
    },
    MensajeInformativoWide: function (Mensaje, Title) {
        var dialog = new BootstrapDialog({
            title: Title,
            closeByBackdrop: false,
            message: Mensaje,
            size: BootstrapDialog.SIZE_WIDE,
            buttons: [
                {
                    label: 'Aceptar',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });

        dialog.open();
    },
    MensajeErrorModal: function (Mensaje) {
        $("#contenidoModalError").html(Mensaje);
        $("#ModalError").modal("show");
    },
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
            Opciones = Opciones + '<option value="' + i.Value + '">' + i.Descripcion + '</option>';
        });
        $(control).html(Opciones);
        if (EsSelect2) {
            $(control).select2();
        }
    },
    MostarCargando: function (Mostrar) {
        if (Mostrar) {
            $("#container_loadin_fondo").show();
            $("#container_loadin_imagen").show();
        } else {
            $("#container_loadin_fondo").hide();
            $("#container_loadin_imagen").hide();
        }
    },
    ToastError: function(Mensaje) {
        setTimeout(function () {
            $.toast({
                text: Mensaje,
                position: 'top-right',
                loaderBg: '#bf441d',
                icon: 'error',
                hideAfter: 4000,
                stack: 1
            });
        }, 250);
    },
    ToastSuccess: function (Mensaje) {
        setTimeout(function () {
            $.toast({
                text: Mensaje,
                position: 'top-right',
                loaderBg: '#5ba035',
                icon: 'success',
                hideAfter: 3000,
                stack: 1
            });
        }, 250);
    },
    ToastWarning: function (Mensaje) {
        setTimeout(function () {
            $.toast({
                text: Mensaje,
                position: 'top-right',
                loaderBg: '#da8609',
                icon: 'warning',
                hideAfter: 3000,
                stack: 1
            });
        }, 250);
    },
    ToastWarningTimeOut: function (Mensaje, TimeOut) {
        setTimeout(function () {
            $.toast({
                text: Mensaje,
                position: 'top-right',
                loaderBg: '#da8609',
                icon: 'warning',
                hideAfter: TimeOut,
                stack: 1
            });
        }, 250);
    },
    ToastInfo: function (Mensaje) {
        setTimeout(function () {
            $.toast({
                text: Mensaje,
                position: 'top-right',
                loaderBg: '#3b98b5',
                icon: 'info',
                hideAfter: 3000,
                stack: 1
            });
        }, 250);
    },
    blockUIStar: function (Message) {
        if (Message == null || typeof Message === 'undefined') {
            Message = "<div style='color:#FFF'>Cargando ...</div>";
        } else {
            Message = "<div style='color:#FFF'>" + Message + "</div>";
        }
        $.blockUI({
            message: Message,
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                color: '#fff',
                width: '160px',
                left: 'calc(50% - 80px)',
                "font-size": "20px",
                "font-weight": "700"
            }
        });
    },
    blockUIStop: function () {
        $.unblockUI();
    },
}

var setIdiomaSpanish = {
    "sProcessing": "Procesando...",
    "sLengthMenu": "Mostrar _MENU_ registros",
    "sZeroRecords": "No se encontraron resultados",
    "sEmptyTable": "Ningún dato disponible en esta tabla",
    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
    "sInfoPostFix": "",
    "sSearch": "Buscar:",
    "sUrl": "",
    "sInfoThousands": ",",
    "sLoadingRecords": "Cargando...",
    "oPaginate": {
        "sFirst": "Primero",
        "sLast": "Último",
        "sNext": "Siguiente",
        "sPrevious": "Anterior"
    },
    "oAria": {
        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
    }
}