$(function(){
    $("#tlf").keypress(function (event) {
        var nummer = $("#tlf").val();
        var tlf = nummer.split("");
        console.log(tlf);
        var formatertTlf = "";
        var hei = "//";

        for (i = 0; i < tlf.length; i++) {
            formatertTlf += tlf[i];
            console.log(tlf.length);
            console.log("når i er " + i + " er formatertTlf " + formatertTlf);

        }
        //sjekker om knappen trykket ikke er backspace
        //dette gjør at man faktisk kan fjerne nummeret om man skrev feil
        if (event.keyCode != 8) {
            if (tlf.length == 3 || tlf.length == 6) {
                formatertTlf += " ";
            }
        }

        $("#tlf").val(formatertTlf);

    });
});


