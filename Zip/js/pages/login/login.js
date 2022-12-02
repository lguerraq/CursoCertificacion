var config = {
    ini: function () {
        $.ajaxSetup({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {}
        });

    }
}
var Eventos = {
    ValidarUsuario: function () {

        grecaptcha.ready(function () {
            grecaptcha.execute(reCapchaKeyWeb, { action: 'LoginIntranet' }).then(function (token) {

                var request = new Object();
                request.Login = $("#txtUsuario").val();
                request.Password = $("#txtPassword").val();
                request.Capcha = token;

                DBR.blockUIStar()

                $.ajax({
                    url: urlValidarUsuario,
                    type: "POST",
                    data: JSON.stringify({ 'request': request }),
                    success: function (response) {
                        DBR.blockUIStop()
                        $("#btnIniciarSesion").prop("disabled", false);
                        if (response.IsSuccess) {
                            $("#lblMensajeGeneral").html("");
                            if (response.Informacion != "" && response.Informacion != null) {
                                window.location.href = response.Informacion;
                            } else {
                                window.location.href = urlUsuario;
                            }
                        } else {
                            $("#lblMensajeGeneral").html(response.Message);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        DBR.blockUIStop()
                        $("#btnIniciarSesion").prop("disabled", false);
                        console.log(XMLHttpRequest.responseText);
                    }
                });
                
            });
        });
        
    }
}
$(document).ready(function () {
    config.ini();
    $("#txtUsuario").keypress(function (e) {
        if (e.which == 13) {
            $("#txtPassword").focus();
        }
    });
    $("#txtPassword").keypress(function (e) {
        if (!DBR.validarCampo("#txtUsuario")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtPassword")) {
            validar = false;
        }
        if (validar) {
            Eventos.ValidarUsuario();
        }
    });
    //$("#txtCapcha").keypress(function (e) {
    //    if (e.which == 13) {
    //        var validar = true;
    //        if (!DBR.validarCampo("#txtUsuario")) {
    //            validar = false;
    //        }
    //        if (!DBR.validarCampo("#txtPassword")) {
    //            validar = false;
    //        }
    //        if (!DBR.validarCampo("#txtCapcha")) {
    //            validar = false;
    //        }
    //        if (validar) {
    //            Eventos.ValidarUsuario();
    //        }
    //    }
    //});
    $("#btnIniciarSesion").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#txtUsuario")) {
            validar = false;                
        }
        if (!DBR.validarCampo("#txtPassword")) {
            validar = false;
        }
        if (validar) {
            Eventos.ValidarUsuario();
        }
    });
    $("#btnRefrescarCapcha").on("click", function () {
        $("#txtCapcha").val("");
        var ran = Math.floor(Math.random() * 10) + 1;
        $('#imgCapcha').attr('src', urlGetCaptchaImage + "?v=" + ran);
    });
});