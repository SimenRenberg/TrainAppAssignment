

$(function () {

    var fra_input = $("#fra");
    var til_section = $("#til-section");
    var til_input = $("#til");
    var når_section = $("#når-section");
    fra_input.focus();

    $("#type").hide();
    $("#billett").hide();
    $("#til-overskrift").css("opacity", "0");
    $("#når-overskrift").css("opacity", "0");
    $("#retur-section").hide();
    $("#retur-overskrift").css("opacity", "0");
    til_section.hide();
    når_section.hide();

    $("#tur-retur").click(function () {
        $("#retur-input").prop('required', true); //fjerner mulighet til å returnere null i dato input
        $("#retur-input").val(date);
        $("#når-section").show(600, "swing", function () {
            $("#når-overskrift").css("opacity", "1");
        });
        $("#retur-section").show(600, "swing", function () {
            $("#retur-overskrift").css("opacity", "1");
        });
    });
    $("#en-vei").click(function () {
        $("#retur-input").prop('required', false); //gjør det mulig å submitte form fordi retur-input ikke lenger blir required
        $("#når-section").show(600, "swing", function () {
            $("#når-overskrift").css("opacity", "1");
        });
        if ($("#en-vei:checked")) {
            $("#retur-section").hide(600, "swing", function () {
                $("#retur-overskrift").css("opacity", "0");
            });
        }
    });




    var fraStasjoner = $("#fraStasjoner");
    var tilStasjoner = $("#tilStasjoner");
    fraStasjoner.hide();
    tilStasjoner.hide();

    $("#fra").keyup(function () {
        var sokFra = $("#fra").val();

        fraStasjoner.show();
        if (sokFra.length < 1) {
            fraStasjoner.hide();
            return;
        }

        
        $.getJSON("/ajax?sok=" + sokFra, function (stasjoner) {

            

            var ut = "";

            if (stasjoner.length == 0) {
                ut += "Søket oppga ingen stasjoner";
            }
            else {
                for (i = 0; i < stasjoner.length; i++) {
                    if (sokFra == stasjoner[i]) {
                        til_section.show(600, "swing", function () {
                            $("#til-overskrift").css("opacity", "1");
                        });
                    }
                    ut += "<div class='valgtStasjon'>" + stasjoner[i] + "</div>";
                }

            }

            fraStasjoner.html(ut);

            var hei = document.querySelectorAll(".valgtStasjon");
            

          
            for (i = 0; i < hei.length; i++) {
                hei[i].addEventListener("click", velgStasjon);
            }

        });
    });
    $("#til").keyup(function () {
        var sokTil = $("#til").val();
        tilStasjoner.show();

        if (sokTil.length < 1) {
            tilStasjoner.hide();
            return;
        }

       
        $.getJSON("/ajax?sok=" + sokTil, function (stasjoner) {

            

            var ut = "";

            if (stasjoner.length == 0) {
                ut += "Søket oppga ingen stasjoner";
            }
            else {
                for (i = 0; i < stasjoner.length; i++) {
                    if (sokTil == stasjoner[i]) {
                        $("#billett").show(600, "swing");
                        $("#type").show(600, "swing");
                    }
                    if (stasjoner[i] == fra_input.val()) {
                        stasjoner[i].remove();
                    }
                    ut += "<div class='valgtStasjon'>" + stasjoner[i] + "</div>";
                }

            }

            tilStasjoner.html(ut);

            var stasjonsListe = document.querySelectorAll(".valgtStasjon");
            


            for (i = 0; i < stasjonsListe.length; i++) {
                stasjonsListe[i].addEventListener("click", velgStasjon);
            }

        });

    });


    function velgStasjon(event) {
        
        var parentId = event.path[1].id;
        var stasjon = event.target.textContent;
        if (parentId == "fraStasjoner") {

            $("#fra").val(stasjon);

            til_section.show(600, "swing", function () {
                $("#til-overskrift").css("opacity", "1");
            });
            fraStasjoner.hide();
        }
        else if (parentId == "tilStasjoner") {

            $("#til").val(stasjon);

            $("#billett").show(600, "swing");
            $("#type").show(600, "swing");
            tilStasjoner.hide();
        }
    }

    var StasjonRegex = /^(Oslo sentralstasjon|Bodø stasjon|Kristiansand stasjon|Trondheim sentralstasjon|Bergen jernbanestasjon)$/;
    $("#fra").change(function () {
        validerFra();
    });
    $("#fraStasjoner").click(function () {
        validerFra();
    });
    $("#til").change(function () {
        validerTil();
    });
    $("#tilStasjoner").click(function () {
        validerTil();
    });


    function validerFra() {
        var ok = StasjonRegex.test($("#fra").val());
        if (!ok) {
            $("#feilMelding").html("Vennligst fyll inn en gyldig stasjon");
            return false;
        } else {
            $("#feilMelding").html("");
            return true;
        }
    }
    function validerTil() {
       
        var ok = StasjonRegex.test($("#til").val());
        if (!ok) {
            $("#feilMelding2").html("Vennligst fyll inn en gyldig stasjon");
            return false;
        } else {
            $("#feilMelding2").html("");
            return true;
        }
    }
    

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

    function validerAlt() {
        okTil = validerTil();
        okFra = validerFra();
        okDatoTil = validerDatoTil();
        okDatoFra = validerDatoFra();
        
        if (okTil && okFra && okDatoFra && okDatoTil) {
            return true;
        }
        return false;
    }
    $("#form").submit(function (hendelse) {
        return validerAlt();   
    });



    //Setter verdien i dato inputfeltene til dagens dato og tid
    var today = new Date();
    
    var month = today.getMonth() + 1;
    
    //lagrer dagens dato og tid, og samtidig sjekker på om minuttene er mindre enn 10, dersom de er mindre enn 10 legger vi på en 0 forran slik at det fremdeles funker
    //når minuttene er mellom 00 og 09
    var date = today.getFullYear() + "-" + (month < 10 ? '0' : '') + month + "-" + (today.getDate() < 10 ? '0' : '') + today.getDate() + "T" + (today.getHours() < 10 ? '0' : '') + today.getHours() + ":" + (today.getMinutes() < 10 ? '0' : '') + today.getMinutes();
  
    $("#når-input").val(date);
});
