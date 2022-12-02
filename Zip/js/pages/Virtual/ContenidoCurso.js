var Eventos = {
    GetContenidoVirtualByEvento: function () {
        var request = new Object();
        request.IdEvento = $("#txtIdEvento").val();
        let urlDetalleHtmlCumplet = urlDetalleHtml + "?IdEvento=" + request.IdEvento;
        $('#ifrDetalle').attr("src", urlDetalleHtmlCumplet);
    },
    //VirtualEvento    
    ListVirtualVideoByEvento: function () {

        var request = new Object();
        request.IdEvento = $("#txtIdEvento").val();

        $.ajax({
            url: urlListVirtualVideoByEvento,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                Funciones.LlenarVirtualVideo(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    FillVideo: function (data) {
        var content = "";
        var contador = 0;
        var e = 0;

        var array = data.Url.split('&');
        array[0] = array[0].replace("https://onedrive.live.com/?", "https://onedrive.live.com/download.aspx?");
        array[2] = "res" + array[2];

        if (data.MostrarVideo) {

            content = content +
                '<video id="VideoJs' + contador + '" class="video-js vjs-fluid" controls preload="auto" width="640" height="360" poster="http://www.ingenierosx100.org/Images/Videos/VIDEO' + (e + 1) + '.jpg"> ' +
                '<source src="' + array[0] + '&' + array[2] + '&' + array[3] + '&' + array[4] + '" type="video/mp4"> ' +
                '<p class="vjs-no-js"> ' +
                'Para ver este video, habilite JavaScript y considere actualizar a un navegador web que admita video HTML5' +
                '</p> ' +
                '</video>';


            contador = contador + 1;
        }
        $(".preview_video").html(content);
        let player0 = videojs('VideoJs0');
    }
}

var Funciones = {
    LlenarVirtualVideo: function (data) {
        var content = "";
        var nave = "";
        var contador = 0;
        $.each(data, function (e, i) {
            var array = i.Url.split('&');
            array[0] = array[0].replace("https://onedrive.live.com/?", "https://onedrive.live.com/download.aspx?");
            array[2] = "res" + array[2];

            if (i.MostrarVideo) {
                if (contador == 0) {
                    nave = nave + '<li class="active"><a href="#tabVideo' + contador + '" data-toggle="tab">Video ' + (e + 1) + '</a></li>';
                } else {
                    nave = nave + '<li><a href="#tabVideo' + contador + '" data-toggle="tab">Video ' + (e + 1) + '</a></li>';
                }
                content = content + '<div class="tab-pane active" id="tabVideo' + contador + '">' +
                    '<div class="post">' +
                    '<div class="row">' +
                    '<div class="col-sm-12">' +
                    '<video id="VideoJs' + contador + '" class="video-js vjs-fluid" controls preload="auto" width="640" height="360" poster="http://www.ingenierosx100.org/Images/Videos/VIDEO' + (e + 1) + '.jpg"> ' +
                    '<source src="' + array[0] + '&' + array[2] + '&' + array[3] + '&' + array[4] + '" type="video/mp4"> ' +
                    '<p class="vjs-no-js"> ' +
                    'Para ver este video, habilite JavaScript y considere actualizar a un navegador web que admita video HTML5' +
                    '</p> ' +
                    '</video>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div>';
                contador = contador + 1;
            }

        });
        if (content == "") {
            content = content + '<tr><td colspan="2">No se encontró ningún video cargado</td></tr>';
        }
        $("#nav-tabs").html(nave);
        $("#tab-content").html(content);
        let player0 = videojs('VideoJs0');

        for (var i = 1; i < contador; i++) {
            let player1 = videojs('VideoJs' + i);
            $("#tabVideo" + i).removeClass("active");
        }
    }
}
$(document).ready(function () {
    $("body").on("contextmenu", function (e) {
        return false;
    });

    //Disable part of page
    $("#id").on("contextmenu", function (e) {
        return false;
    });
    Eventos.GetContenidoVirtualByEvento();
    //Eventos.ListVirtualVideoByEvento();

    $("#btnEditarContenido").on("click", function () {
        editor.setReadOnly(false);
        $(this).hide();
        $("#btnGuardarContenido").show();
    });
    $("#btnGuardarContenido").on("click", function () {
        Eventos.SaveVirtualContenido();
    });
    $("#cboEvento").on("change", function () {
        Eventos.GetContenidoVirtualByEvento();
        Eventos.ListVirtualVideoByEvento();
    });

    $(".preview-text").on("click", function () {
        $("#modalLeccion").modal("show");
    });

    if ($("#hdfVideo").val() != null && $("#hdfVideo").val() != "") {
        var data = new Object();
        data.Url = $("#hdfVideo").val();
        data.MostrarVideo = true;
        Eventos.FillVideo(data);
    }
});

