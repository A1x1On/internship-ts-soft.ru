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
    var wholeTags = "";
    var tagId = 0;



   
 
    var idtag = 0;
    var title = "";
    var arrayOfTags = [];
    var tag = function () {
        this.idtag = idtag;
        this.title = title;
        return this;
    };


    $(".optionTag").live("click", function() {
        idtag = $(this).attr("tabindex");
        title = $(this).html();
        $(".ForTags").append("<span class='tag' id=" + idtag + ">" + title + "</span>");

        arrayOfTags.push(new tag());
     

        wholeTags = wholeTags + ", " + title;
        $(".inputTag").focus().val("");
    });
    $("body").click(function () {
        $(".optionTag").fadeOut(100);  
    });

    $(".inputTag").keyup(function (event) {
        if (event.keyCode == 40) {
            $(".sel-list").focus();
            setTimeout(function () {
                $(".optionTag:nth-child(" + y + ")").removeClass("focus");
                $(".optionTag:nth-child(1)").focus();
                $(".optionTag:nth-child(" + i + ")").addClass("focus");
            }, 50); 
        }
    });
   
    $(".sel-list").live("keyup", function (event) {
        if (event.keyCode == 40) {
            i++;
            y = i - 1;
            if ($(".optionTag:nth-child(" + i + ")").html() != undefined) {
                $(".optionTag:nth-child(" + y + ")").removeClass("focus");
                $(".optionTag:nth-child(" + i + ")").focus();
                $(".optionTag:nth-child(" + i + ")").addClass("focus");
            }
            else
            {
                i = i - 1;
                y = i + 1;  
            }
        }
        if (event.keyCode === 38) {
            i--;
            y = i + 1;
            if ($(".optionTag:nth-child(" + i + ")").html() != undefined) {
                $(".optionTag:nth-child(" + y + ")").removeClass("focus");
                $(".optionTag:nth-child(" + i + ")").focus();
                $(".optionTag:nth-child(" + i + ")").addClass("focus");
            }
            else
            {
                $(".inputTag").focus();
                $(".optionTag:nth-child(" + y + ")").removeClass("focus");
                i = 1;
                y = 1;
            }
        }
        if (event.keyCode === 13 && $(".focus")) {

            idtag = $(".focus").attr("tabindex");
            title = $(".focus").html();
            
            $(".ForTags").append("<span class='tag' id=" + idtag + ">" + title + "</span>");

            arrayOfTags.push(new tag());
            console.log(arrayOfTags);

            $(".optionTag").fadeOut(100);
            wholeTags = wholeTags + ", " + title;
            $(".inputTag").focus().val("");
        }


    });

    //Добавление задачи submit
    $(".saveTask").click(function () {
        var str_tags = "";
        for (var c = 0; arrayOfTags.length !== c; c++) {
            str_tags = str_tags + ", " + arrayOfTags[c].title;
        }
        str_tags = str_tags.substring(2, str_tags.length);
        $(".HIDDDENTAGS").val(str_tags);

        var that = $(this);
        setTimeout(function () {
            that.parent("form").submit();
        }, 1000);

    });


    //Удаление тега
    $(".tag").live("click", function () {

        alert($(this).attr("id"));

        $(this).fadeOut(300);
        var that = $(this);
        setTimeout(function() {
            that.remove();
        }, 1000);
        



    });


    

});






