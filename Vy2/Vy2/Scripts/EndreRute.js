
function gjorTilInputs(event) {
    var lagret = sjekkOmLagret();
    if (event.target.textContent != "Lagre") {
        if (!lagret) {
            visFeilMelding();
            return;
        }
    }
 
    var radId = "#rad_" + event.target.id;
    var tilsvarendeRad = document.querySelector(radId);
    var tabellCeller = tilsvarendeRad.children;
    var verdier = [];
    var inputFields = [];
    if (event.target.classList.contains("rediger")) {

        event.target.innerHTML = "Lagre";
        event.target.classList.remove("rediger");
        event.target.classList.add("lagre");

        //tar her i < length-2 for å ikke ta med cellen hvor knappen ligger eller RuteID cellen
        for (var i = 0; i < tabellCeller.length - 2; i++) {            
            verdier.push(tabellCeller[i].textContent);
           
            tabellCeller[i].innerHTML = "<input type='text' id='input_" + i + "' class='input' style='text-align:left;'/>";
            inputFields.push($("#input_" + i));
            inputFields[i].val(verdier[i]);
            if (i == 2 || i == 3) {
                $("#input_"+i).prop("disabled", true);
            }
        }

    } else if (event.target.classList.contains("lagre")) {
        event.target.innerHTML = "Rediger";
        event.target.classList.remove("lagre");
        event.target.classList.add("rediger");  
        for (var i = 0; i < tabellCeller.length - 2; i++) {
            verdier.push($("#input_" + i).val());
            tabellCeller[i].innerHTML = verdier[i];
        }

        var togRute = {
            RuteId: event.target.id,
            AvgangTid: verdier[0],
            AnkomstTid: verdier[1],
            Platform: verdier[4],
            Pris: verdier[5]
        };
        
        $.ajax({
            url: "/EndreRuter",
            method: "POST",
            data: JSON.stringify(togRute),
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
        else if (knapper[i].classList.contains("rediger")) {
            ok = true;
           
        }
    }
   
 return ok;
}
$(".rediger").on("click", gjorTilInputs);


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