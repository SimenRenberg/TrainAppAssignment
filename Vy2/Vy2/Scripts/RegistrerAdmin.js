

function regexEpost() {
    
    var epost = $("#Epost").val();
    var ok = false;
    var regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if (regex.test(epost)) {
        $("#feilEpost").html("");
        ok = true;
    }
    else {
        ok = false;
        $("#feilEpost").html("Ugyldig Epost");
    }
    return ok;
}

function validerPassord() {
    
    var passord = $("#Passord").val();
   
    if (passord.length < 4) {
        $("#feilPassord").html("Minst 4 karakterer");
        return false;
    }
    else {
        $("#feilPassord").html("");
        return true;
    }
    
}

function validerAlt() {
    validerPassord();
    regexEpost();
    var ok = false;
    if (validerPassord() && regexEpost()) {
        ok = true;
    }
    else {
        ok = false;
    }
    return ok;
}

$("#Passord").on("change", validerPassord);
$("#Epost").on("change", regexEpost);
$("#skjema").on("submit", validerAlt);

