$(document).ready(function () {


    
    // Change the value of TASKTERM(@Class = "valDate") to the value of <input type="date" id="myDate"
    $("#myDate").change(function () {
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



    ///////////////////////////////
    // Including of tags prototype 
    $.getScript("Scripts/ArrayOfTags.js");

    // Including of Tags script
    $.getScript("Scripts/Tags.js");
 
    // Getting tags from prototype, .inputTag control in one .HIDDDENTAGS and Submit form
    $(".saveTask").click(function () {
        var str_tags = "";
        var comma = "";
        for (var c = 0; arrayOfTags.length !== c; c++) {
            str_tags = str_tags + ", " + arrayOfTags[c].title;
        }
        str_tags = str_tags.substring(2, str_tags.length);
        if (str_tags+$(".inputTag").val()) {
            comma = ", ";
        }
        $(".HIDDDENTAGS").val(str_tags + comma + $(".inputTag").val());
        var that = $(this);
        setTimeout(function () {
            that.parent("form").submit();
        }, 1000);
    });

});






