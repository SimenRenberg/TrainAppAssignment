function Ajax(event) {
    console.log("hei");
    $.ajax({
        url: "/SlettAdmin?AnsattNr=" + event.target.id,
        method: "POST",
        success: function (data) { }
    });
    location.reload();
}
$(".SlettAdmin").on("click", Ajax);

function gjorTilInputs(event) {
    console.log(event);
    var lagret = sjekkOmLagret();
    if (event.target.textContent != "Lagre") {
        if (!lagret) {
            //visFeilMelding();
            return;
        }
    }

    var radId = "#rad_" + event.target.id;
    var tilsvarendeRad = document.querySelector(radId);
    var tabellCeller = tilsvarendeRad.children;
    var verdier = [];
    var inputFields = [];
    if (event.target.classList.contains("EndreAdmin")) {

        event.target.innerHTML = "Lagre";
        event.target.classList.remove("EndreAdmin");
        event.target.classList.add("lagre");

        //tar her i < length-2 for å ikke ta med cellen hvor knappen ligger eller RuteID cellen
        for (var i = 0; i < tabellCeller.length - 2; i++) {
            verdier.push(tabellCeller[i].textContent);
            console.log(verdier);
            tabellCeller[i].innerHTML = "<input type='text' id='input_" + i + "' class='input' style='text-align:left;'/>";
            inputFields.push($("#input_" + i));
            inputFields[i].val(verdier[i]);
            if (i==0) {
                $("#input_" + i).prop("disabled", true);
            }
        }

    } else if (event.target.classList.contains("lagre")) {
        event.target.innerHTML = "Rediger";
        event.target.classList.remove("lagre");
        event.target.classList.add("EndreAdmin");
        for (var i = 0; i < tabellCeller.length - 2; i++) {
            verdier.push($("#input_" + i).val());
            tabellCeller[i].innerHTML = verdier[i];
        }

        var Admin = {
            AnsattNr: event.target.id,
            Rolle: verdier[1],
            Epost: verdier[2]
        };
        console.log(Admin);
        $.ajax({
            url: "/EndreAdmin",
            method: "POST",
            data: JSON.stringify(Admin),
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                alert("Endringer er lagret i databasen");

            }

        });

    }
}
function sjekkOmLagret() {
    var knapper = document.querySelectorAll("button");
    var ok;
    for (var i = 1; i < knapper.length; i++) {
        if (knapper[i].classList.contains("lagre")) {
            ok = false;
            return ok;
        }
        else if (knapper[i].classList.contains("EndreAdmin")) {
            ok = true;

        }
    }

    return ok;
}
function visFeilMelding() {


    var knapper = document.querySelectorAll("button");
    var linje;
    for (var i = 1; i < knapper.length; i++) {
        if (knapper[i].classList.contains("lagre")) {
            linje = knapper[i].id;
        }
    }

    $("#linje").html(linje);
    $("#feilMelding").show();
    window.setTimeout(() => {
        $("#feilMelding").hide();
    }, 2500);
}

$(".EndreAdmin").on("click", gjorTilInputs);
$(".lagre").on("click", gjorTilInputs);