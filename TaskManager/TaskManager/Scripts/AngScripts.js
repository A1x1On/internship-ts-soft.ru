(function () {

    // Including of tags prototype 
    $.getScript("Scripts/ArrayOfTags.js");

    // Initializing of module AngularManager
    var ModuleManager = angular.module("AngularManager", ["ngRoute"]);

    // id of task
    var idTask;
 
    // Initializing of controller DinamicTag and factory TService
    ModuleManager.controller("TaskFrom", function ($scope, TService) {

        // Deleting of task
        $scope.DeleteTask = function (obj) {
            idTask = obj.currentTarget.id;
            if (idTask != 0) {
                $("i#" + idTask).fadeOut();
                // Use TService for Deleting of task
                TService.DeleteTask().then(function (d) {
                    $("header").html(d.data);
                }, function () {
                    alert("Fail of deleting task");
                });
            }
        }

        // Getting of tags
        $scope.TagListToDOM = function (name) {
            key = name;
            if (name != "") {
                // Use TService for getting data tags
                TService.GetTags().then(function (d) {
                    $scope.Tags = d.data;
                }, function () {
                    alert("Fail of getting tags");
                });
            }
            else {
                $scope.Tags = "";
            }
        }
    });

    ModuleManager.factory("TService", function ($http) {
        var fac = {};
        fac.GetTags = function () {
            return $http({ method: "GET", url: "/Manager/GetTags/", params: { "name": key } });
        }
        fac.DeleteTask = function () {
            return $http({ method: "GET", url: "/Manager/Delete/", params: { 'idTask': idTask } });
        }
        return fac;

    });

    // Initializing of controller OpenTask and factory OpenTaskService
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
                    // forming of DOM elements(tags)
                    $(".ForTags").append("<span class='tag' id='" + id100 + "'>" + item);
                    idtag = id100;
                    title = item;
                    id100++;
                    // Pushing to The prototype
                    arrayOfTags.push(new tag());
                });
            }, function () {
                alert('Fail of forming tags');
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


