function validerDatoTil() {
    if ($("#når-input").val() >= date) {
        $("#datoFeil").html("");
        return true;
    } else {
        $("#datoFeil").html("Datoen for avreise kan ikke være før dagens dato.");
        return false;
    }
}
function validerDatoFra() {
    if ($("#retur-input").val() === "") {
        $("#returDatoFeil").html("");
        return true;
    } else if ($("#retur-input").val() < $("#når-input").val()) {
        $("#returDatoFeil").html("Datoen for retur kan ikke være før datoen for avreise.");
        return false;
    } else if ($("#retur-input").val() >= date) {
        $("#returDatoFeil").html("");
        return true;
    } else {
        $("#returDatoFeil").html("Datoen for retur kan ikke være før dagens dato.");
        return false;
    }
}

//Setter verdien i dato inputfeltene til dagens dato og tid
var today = new Date();

var month = today.getMonth() + 1;

//lagrer dagens dato og tid, og samtidig sjekker på om minuttene er mindre enn 10, dersom de er mindre enn 10 legger vi på en 0 forran slik at det fremdeles funker
//når minuttene er mellom 00 og 09
var date = today.getFullYear() + "-" + (month < 10 ? '0' : '') + month + "-" + (today.getDate() < 10 ? '0' : '') + today.getDate() + "T" + (today.getHours() < 10 ? '0' : '') + today.getHours() + ":" + (today.getMinutes() < 10 ? '0' : '') + today.getMinutes();

$("#når-input").val(date);
$("#retur-input").val(date);


function validerStartStasjon() {
    var regex = /^[a-zæøåA-ZÆØÅ ]{5,30}$/;
    var ok = false;
    if (regex.test($("#startstasjon").val())) {
        $("#feilStart").html("");
        ok = true;
    }
    else {
        ok = false;
        $("#feilStart").html("Ugyldig togstasjon");
    }
    return ok;
}
function validerEndeStasjon() {
    var regex = /^[a-zæøåA-ZÆØÅ ]{5,30}$/;
    var ok = false;
    if (regex.test($("#endestasjon").val())) {
        ok = true;
        $("#feilEnde").html("");
    }
    else {
        ok = false;
        $("#feilEnde").html("Ugyldig togstasjon");
    }
    return ok;
}
function validerPlatform() {
    
    var regex = /^[0-9]{1,2}$/;
    var ok = false;
    if ($("#platform").val() < 1) {
        $("#platformFeil").html("Platformen må være mer enn 0");
        return false;
    }
    if (regex.test($("#platform").val())) {
        ok = true;
        $("#platformFeil").html("");
    }
    else {
        ok = false;
        $("#platformFeil").html("Ugyldig platform");
    }
    return ok;
}
function validerPris() {
    var regex = /^[0-9]{1,4}$/;
    var ok = false;
    if ($("#pris").val() < 50) {
        $("#prisFeil").html("Pris kan ikke være mindre enn 50 kr");
        return false;
    }
    else if ($("#pris").val() > 5000) {
        $("#prisFeil").html("Pris kan ikke være høyere enn 5000 kr");
        return false;
    }
    if (regex.test($("#platform").val())) {
        $("#prisFeil").html("");
        ok = true;
        
    }
    else {
        ok = false;
        $("#prisFeil").html("Ugyldig pris");
    }
    return ok;
}
function validerAlt() {
    validerDatoFra();
    validerDatoTil();
    validerStartStasjon();
    validerEndeStasjon();
    validerPlatform();
    validerPris();

    if (validerDatoFra() && validerDatoTil() && validerStartStasjon() && validerEndeStasjon() && validerPlatform() && validerPris()) {
        return true;
    } else {
        return false;
    }
}

$("#når-input").on("change", validerDatoFra);
$("#fra-input").on("change", validerDatoTil);
$("#startstasjon").on("change", validerStartStasjon);
$("#endestasjon").on("change", validerEndeStasjon);
$("#platform").on("change", validerPlatform);
$("#pris").on("change", validerPris);

$("#skjema").on("submit", validerAlt);
