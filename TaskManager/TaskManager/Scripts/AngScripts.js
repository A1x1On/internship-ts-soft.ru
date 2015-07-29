(function () {

    // Including of tags prototype 
    $.getScript("Scripts/ArrayOfTags.js");

    //////////////////////////////////////
    // Initialazing of module AngularManager
    var ModuleManager = angular.module("AngularManager", ["ngRoute"]);

    /////////////////////////////////////////////////////////////////
    // Initialazing of controller DinamicTag and factory TagService
    ModuleManager.controller("DinamicTag", function ($scope, TagService) {

        $scope.TagListToDOM = function (name) {
            // Name is value of tag
            key = name;
            if (name != "") {
                // Use TagService for getting data tags
                TagService.GetTags().then(function (d) {
                    $scope.Tags = d.data;
                }, function () {
                    alert("Failed");
                });
            }
            else {
                $scope.Tags = "";
            }
        }
    });

    ModuleManager.factory("TagService", function ($http) {
        var fac = {};
        fac.GetTags = function () {
            return $http({ method: "GET", url: "/Manager/GetTags/", params: { "name": key } });
        }
        return fac;

    });


    ////////////////////////////////////////////////////////////////
    // Initialazing of controller OpenTask and factory OpenTaskService
    ModuleManager.controller("OpenTask", function ($scope, OpenTaskService) {

        // Clicking on the task at the left panel
        $scope.ClickedTask = function (TaskId) {
            // TaskId is value of task
            key = TaskId;
            // forming of interface for selected task
            $(".RemoveTask").fadeIn();
            
            $(".saveTask").val("Сохранить");

            // Use TagService for getting data tags from current task without reloaded page
            OpenTaskService.GetTask().then(function (t) {
                $scope.Task = t.data;

                if ($scope.Task[3] !== "Завершен") {
                    $(".finishtag").fadeIn();
                } else {
                    $(".finishtag").fadeOut();
                }

                $(".t-title").val($scope.Task[0]);
                $(".TASKID").val($scope.Task[5]);
                $(".finishFromTask").val($scope.Task[5]);
                $(".RemoveTask").attr("id", $scope.Task[5]);

                $(".t-disc").val($scope.Task[1]);
                $(".valDate").val($scope.Task[2]);
                $("#myDate").attr("value", $scope.Task[2]);
                $(".TASKSTATUS").val($scope.Task[3]);
                $(".HIDDDENTAGS").val($scope.Task[4]);

                // setting to null for the Prototype
                arrayOfTags = $.grep(arrayOfTags, function (el) { return null });

                var re = /[a-zа-я0-9-]+/gi;
                var str = $scope.Task[4];
                var html = "";
                var id100 = 100;
                $(".ForTags").html("");
                str = str.match(re) || [];
                str.forEach(function (item, i) {
                    // forming of DOM elemets(tags)
                    $(".ForTags").append("<span class='tag' id='" + id100 + "'>" + item);
                    idtag = id100;
                    title = item;
                    id100++;
                    // Pushing to The prototype
                    arrayOfTags.push(new tag());
                });
                //console.log(arrayOfTags);
            }, function () {
                alert('Failed');
            });
        }
    });

    ModuleManager.factory("OpenTaskService", function ($http) {
        var fac2 = {};
        fac2.GetTask = function () {
            return $http({ method: 'GET', url: "/Manager/OpenTask/", params: { 'TaskId': key } });
        }
        return fac2;
    });

})();


