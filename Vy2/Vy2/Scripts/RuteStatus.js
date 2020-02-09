function Ajax(event) {
    
    if (event.target.className == "InstillRute") {
        var url = "/InstillRute?RuteId=" + event.target.id;
    } else if (event.target.className == "Gjenopprett") {
        var url = "/GjenopprettRute?RuteId=" + event.target.id;
    }
    $.ajax({
        url: url,
        method: "POST",
        success: function (data) { }      
    });
    window.setTimeout(() => {
        location.reload();
    }, 500);
}

$(".InstillRute").on("click", Ajax);
$(".Gjenopprett").on("click", Ajax);

