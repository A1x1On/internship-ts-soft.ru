$(document).ready(function() {

    // Change the value of TASKTERM(@Class = "valDate") to the value of <input type="date" id="myDate"
    $("#myDate").change(function() {
        var val = $(this).val();
        $(".valDate").val(val);
    });

    // Adding current date to value of TASKTERM(@Class = "valDate") and to <input type="date" id="myDate"
    var date = new Date();
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();
    if (month < 10) month = "0" + month;
    if (day < 10) day = "0" + day;
    var today = year + "-" + month + "-" + day;
    $("#myDate").attr("value", today);
    $(".valDate").val(today);


    var i = 1;
    var y = 1;
    $(".optionTag").live("click", function() {

        $(this).css("color", "blue");
    }
);
    $(".inputTag").keyup(function (event) {
        if (event.keyCode == 40) {
            $(".sel-list").focus();
            setTimeout(function () {
                $(".optionTag:nth-child(" + y + ")").removeClass("focus");
                $(".optionTag:nth-child(1)").focus();
                $(".optionTag:nth-child(1)").css("color", "red");
                $(".optionTag:nth-child(" + i + ")").addClass("focus");
            }, 50); 
        }
    });
   
    $(".sel-list").live("keyup", function (event) {
        if (event.keyCode == 40) {
            i++;
            y = i - 1;
            if ($(".optionTag:nth-child(" + i + ")").html() != undefined) {
                $("#er").html("down: " + i);
                $(".optionTag:nth-child(" + y + ")").css("color", "#333");
                $(".optionTag:nth-child(" + y + ")").removeClass("focus");
                $(".optionTag:nth-child(" + i + ")").focus();
                $(".optionTag:nth-child(" + i + ")").css("color", "red");
                $(".optionTag:nth-child(" + i + ")").addClass("focus");
            }
            else
            {
                i = i - 1;
                y = i + 1;  
            }
        }
        if (event.keyCode == 38) {
            i--;
            y = i + 1;
            if ($(".optionTag:nth-child(" + i + ")").html() != undefined) {
                $("#er").html("up: " + i);
                $(".optionTag:nth-child(" + y + ")").css("color", "#333");
                $(".optionTag:nth-child(" + y + ")").removeClass("focus");
                $(".optionTag:nth-child(" + i + ")").focus();
                $(".optionTag:nth-child(" + i + ")").css("color", "red");
                $(".optionTag:nth-child(" + i + ")").addClass("focus");
            }
            else
            {
                $(".inputTag").focus();
            }
        }
        if (event.keyCode == 13 && $(".focus")) {
            
            $(".ForTags").append('<span>' + $(".focus").html() + '</span>');
            $(".optionTag").fadeOut(100);
        }


    });


});






