$(function () {

    var radios = document.getElementsByName("RuteId");
    for (i = 0; i < radios.length; i++) {
        radios[i].checked = false;
    }

    var radiosRetur = document.getElementsByName("RuteId");
    for (i = 0; i < radiosRetur.length; i++) {
        radiosRetur[i].checked = false;
    }

    var ruter = document.querySelectorAll(".ruter");
    var returRuter = document.querySelectorAll(".returRuter");
    console.log(ruter);
    console.log(returRuter);
    
        
    for (i = 0; i < ruter.length; i++) {
        ruter[i].addEventListener("click", ruteHandler);
    }
    for (i = 0; i < returRuter.length; i++) {
        returRuter[i].addEventListener("click", returRuteHandler);
    }
        
        
   
    
    
    function ruteHandler(event) {
        console.log(event.path[1].id);
        radId = event.path[1].id;

        if ($("#radio-" + radId).prop("checked")) {
            $("#radio-" + radId).prop("checked", false);
            $("#radio-" + radId).parents("tr").css("background-color", "white");
            $("#radio-" + radId).parents("tr").siblings().css("background-color", "white");
        } else {
            $("#radio-" + radId).prop("checked", true);
            $("#radio-" + radId).parents("tr").css("background-color", "#31b81a");
            $("#returAvreiseFeil").html("");
            $("#radio-" + radId).parents("tr").siblings().css("background-color", "white");
        }
        console.log($("#radio-" + radId).prop("checked"));
    }
    function returRuteHandler(event) {
        console.log(event.path[1].id);
        radId = event.path[1].id;
        
        if ($("#radio-" + radId).prop("checked")) {
            $("#radio-" + radId).prop("checked", false);
            $("#radio-" + radId).parents("tr").css("background-color", "white");
            $("#radio-" + radId).parents("tr").siblings().css("background-color", "white");
        } else {
            $("#radio-" + radId).prop("checked", true);
            $("#radio-" + radId).parents("tr").css("background-color", "#31b81a");
            $("#returAvreiseFeil").html("");
            $("#radio-" + radId).parents("tr").siblings().css("background-color", "white");
        }
        console.log($("#radio-" + radId).prop("checked"));
    }
    
});
var fra = document.getElementsByName("RuteId");
var til = document.getElementsByName("ReturRuteId");



console.log(til);
console.log(fra);
console.log(til.checked);

function sjekkOmValgtTil() {
    for (i = 0; i < til.length; i++) {
        if (til[i].checked) {
             return true;
        }   
    }  
    return false;
}
function sjekkOmValgtFra() {
    for (i = 0; i < fra.length; i++) {
        if (fra[i].checked) {
            return true;
        }
    } 
    return false;
}
function sjekkOmValgt() {
    var fra = sjekkOmValgtFra();
    var til = sjekkOmValgtTil();
    if (fra && til) {
        return true;
    }
    $("#returAvreiseFeil").html("Du må velge en avreise og en retur-rute for å gå videre");

    return false;
}
$("#form").submit(function (hendelse) {
    

    return sjekkOmValgt();
   
});


