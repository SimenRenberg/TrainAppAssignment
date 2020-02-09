
window.setTimeout(() => {
    document.querySelector(".modal").style.opacity = "1";
}, 500);


$(".modal").hide();
function handleModal() {
    
    if ($(".modal").hasClass("hidden")) {
        $(".modal").removeClass("hidden");
        $(".modal").addClass("visible");
        $(".modal").show(175, "linear");
        $("body").css("background-color", "rgba(0,0,0,0.6)");
        
    }
    else if ($(".modal").hasClass("visible")) {
        $(".modal").removeClass("visible");
        $(".modal").addClass("hidden");
        $(".modal").hide(175, "linear");
        $("body").css("background-color", "white");
        

    }
}
function lukkModal(event) {
    
    var trykketLogIn = event.path.includes($("#login")[0]);
    var trykketModal = event.path.includes($(".modal")[0]);
    if (trykketLogIn) return;
    if (trykketModal) return;
    else {
        $(".modal").removeClass("visible");
        $(".modal").addClass("hidden");
        $(".modal").hide(175, "linear");
        $("body").css("background-color", "white");
    }
}
document.addEventListener("click", lukkModal);
$("#login").on("click", handleModal);
$(".kryss").on("click", handleModal);
/*
var epost = $("#brukerNavn").val();
var passord = $("#passord").val();

var url = "/Logginn";

function loggInn() {
    console.log("inne i funskjonen");
        $.ajax({
            url: url,
            dataType: "JSON",
            data: {Passord: passord, Epost: epost},
            type: "POST",
            contentType: "application/json; charset=utf8",
            success: function (resultat) {
                console.log(resultat);
            }
        });
}

$("#loggInn").on("click", loggInn);*/

