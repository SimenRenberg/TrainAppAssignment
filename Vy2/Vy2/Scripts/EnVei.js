$(function () {
    var radios = document.getElementsByName("RuteId");
    for (i = 0; i < radios.length; i++) {
        radios[i].checked = false; 
    }
    
    
    var ruter = document.querySelectorAll(".ruter");
    for (i = 0; i < ruter.length; i++) {
        ruter[i].addEventListener("click", ruteHandler);
    }
    /*console.log(ruter);*/
    function ruteHandler(event) {
        /*console.log("inne i funksjon");
        console.log(event.path);*/
        radId = event.path[1].id;

        if ($("#radio-" + radId).prop("checked")) {
            $("#radio-" + radId).prop("checked", false);
            $("#radio-" + radId).parents("tr").css("background-color", "white");
            $("#radio-" + radId).parents("tr").siblings().css("background-color", "white");
        } else {
            $("#radio-" + radId).prop("checked", true);
            $("#radio-" + radId).parents("tr").css("background-color", "#31b81a");
            $("#avreiseFeil").html("");
            $("#radio-" + radId).parents("tr").siblings().css("background-color", "white");
        }
        /*console.log($("#radio-" + radId).prop("checked"));*/
    }

});

var fra = document.getElementsByName("RuteId");

function sjekkOmValgtFra() {
    var ok = false;
    for (i = 0; i < fra.length; i++) {
        if (fra[i].checked) {
            ok = true;
            return true;
        }

    }
    $("#avreiseFeil").html("Du er nødt til å velge en rute");
    return false;
}

$("#form").submit(function (hendelse) {
    return sjekkOmValgtFra();
})