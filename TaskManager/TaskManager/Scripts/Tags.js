var i = 1;
var y = 1;
var wholeTags = "";
var tagId = 0;
// function for (Moving of focus)
function ActOptionTag() {
    $(".optionTag:nth-child(" + y + ")").removeClass("focus");   
    $(".optionTag:nth-child(" + i + ")").addClass("focus");
}

// Selecting of tag with mouseClick
$(".optionTag").live("click", function () {
    idtag = $(this).attr("tabindex");
    title = $(this).html();
    $(".ForTags").append("<span class='tag' id=" + idtag + ">" + title + "</span>");

    arrayOfTags.push(new tag());
    wholeTags = wholeTags + ", " + title;
    $(".inputTag").focus().val("");
});

// Hiding block of options
$("body").click(function () {
    $(".optionTag").fadeOut(100);
});

// Moving of focus for beginning
$(".inputTag").keyup(function (event) {
    if (event.keyCode == 40) {
        $(".sel-list").focus();
        setTimeout(function () {
            $(".optionTag:nth-child(1)").focus();
            ActOptionTag();
        }, 50);
    }
});

// Moving of focus on options with up/down buttons
$(".sel-list").live("keyup", function (event) {
    if (event.keyCode == 40) {
        i++;
        y = i - 1;
        if ($(".optionTag:nth-child(" + i + ")").html() != undefined) {
            $(".optionTag:nth-child(" + i + ")").focus();
            ActOptionTag();

        }
        else {
            i = i - 1;
            y = i + 1;
        }
    }
    if (event.keyCode === 38) {
        i--;
        y = i + 1;
        if ($(".optionTag:nth-child(" + i + ")").html() != undefined) {
            $(".optionTag:nth-child(" + i + ")").focus();
            ActOptionTag();
        }
        else {
            $(".inputTag").focus();
            $(".optionTag:nth-child(" + y + ")").removeClass("focus");
            i = 1;
            y = 1;
        }
    }
    
    // Get event of tag with button "Enter" 
    if (event.keyCode === 13 && $(".focus")) {
        idtag = $(".focus").attr("tabindex");
        title = $(".focus").html();
        $(".ForTags").append("<span class='tag' id=" + idtag + ">" + title + "</span>");
        arrayOfTags.push(new tag());

        //console.log(arrayOfTags);
        $(".optionTag").fadeOut(100);
        wholeTags = wholeTags + ", " + title;
        $(".inputTag").focus().val("");
    }
});

// Deleting tag
$(".tag").live("click", function () {
    idtag = $(this).attr("id");
    arrayOfTags = $.grep(arrayOfTags, function (el) { return el.idtag != idtag });
    console.log(arrayOfTags);

    //console.log(arrayOfTags);
    $(this).fadeOut(300);
    var that = $(this);
    setTimeout(function () {
        that.remove();
    }, 1000);
});