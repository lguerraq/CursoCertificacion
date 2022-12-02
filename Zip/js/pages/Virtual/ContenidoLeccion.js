var Leccion = {
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
        $("#player").html(content);
        let player0 = videojs('VideoJs0');
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

    if ($("#hdfVideo").val() != null && $("#hdfVideo").val() != "") {
        var data = new Object();
        data.Url = $("#hdfVideo").val();
        data.MostrarVideo = true;
        Leccion.FillVideo(data);
    }

    $(".login-btn").on("click", function (e) {
        e.preventDefault();
        window.location.href = urlLeccion + "/" + $(this).data("id") + "?rowId=" + $(this).data("curso");
    });
});

