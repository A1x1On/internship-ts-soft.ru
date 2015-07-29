// Initialazing of tags prototype 
var idtag = 0;
var title = "";
var arrayOfTags = [];
var tag = function () {
    this.idtag = idtag;
    this.title = title;
    return this;
};
