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
   
});




