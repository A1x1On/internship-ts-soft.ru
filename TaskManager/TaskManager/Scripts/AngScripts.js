(function () {

    // Including of tags prototype 
    $.getScript("Scripts/ArrayOfTags.js");

    // Initializing of module AngularManager
    var ModuleManager = angular.module("AngularManager", ["ngRoute"]);

    // id of task
    var idTask;
 
    // Initializing of controller DinamicTag and factory TService
    ModuleManager.controller("TaskFrom", function ($scope, TService) {

        // Select tasks on home page by current user without filters
        $scope.init = function () {
            TService.GetTasks().then(function(d) {
                $scope.Tasks = d.data;
                $scope.tagValue = "";
                $scope.dateValue = "";
            }, function() {
                alert("Fail of deleting task");
            });
        }

        // Deleting of task
        $scope.DeleteTask = function (obj) {
            idTask = obj.currentTarget.id;
            if (idTask != 0) {
                TService.DeleteTask().then(function (d) {
                    $("i#" + idTask).fadeOut();
                    $(".blockInsertTask").fadeOut();
                }, function () {
                    alert("Fail of deleting task");
                });
            }
        }

        // Getting of tags to DOM like select tag by keyword
        $scope.TagListToDOM = function (name) {
            key = name;
            if (name != "") {
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

        // Turn off activity of task(status)
        $scope.EndActTask = function (obj) {
            idTask = obj.currentTarget.id;
            TService.EndActTask().then(function (d) {
                $scope.Tags = d.data;
                $(".finishtag").css("display", "none");
                $(".Cancelfinishtag").css("display", "block");
                $("i#" + idTask).children(".d-status").html("<b>Cтатус: </b>Завершен");

            }, function () {
                alert("Fail of getting tags");
            });
        }

        // Turn on activity of task(status)
        $scope.BeginActTask = function (obj) {
            idTask = obj.currentTarget.id;
            TService.BeginActTask().then(function (d) {
                $scope.Tags = d.data;
                $(".finishtag").css("display", "block");
                $(".Cancelfinishtag").css("display", "none");
                $("i#" + idTask).children(".d-status").html("<b>Cтатус: </b>Активный");
            }, function () {
                alert("Fail of getting tags");
            });
        }

        // Filter tasks by tag and date
        $scope.filterBy = function (obj) {
            if (obj.currentTarget.className == "fDate") {
                $scope.dateValue = obj.currentTarget.innerHTML;
                $(".Dates").children("span").css("color", "#FFF");
                $(".Dates").children("#" + obj.currentTarget.id).css("color", "red");
            }
            if (obj.currentTarget.className == "fTag") {
                $scope.tagValue = obj.currentTarget.innerHTML;
                $(".Tags").children("span").css("color", "#FFF");
                $(".Tags").children("#"+obj.currentTarget.id).css("color", "red");
            }
            if (obj.currentTarget.className == "fResetDate") {
                $scope.dateValue = "";
                $(".Dates").children("span").css("color", "#FFF");
            }
            if (obj.currentTarget.className == "fResetTag") {
                $scope.tagValue = "";
                $(".Tags").children("span").css("color", "#FFF");
            }
        }

        // By pressing the button to show memo for fill in of task
        $scope.InsertTask = function (obj) {
            key = obj.currentTarget.id;
            $(".blockInsertTask").fadeIn();
            $(".finishtag").css("display", "none");
            $(".RemoveTask").css("display", "none");
            $(".TASKID").val("");
            $(".t-title").val("");
            $(".t-disc").val("");
            $(".inputTag").val("");
            $(".ForTags").html("");
            arrayOfTags = $.grep(arrayOfTags, function (el) { return null })
        }

        // By pressing the task to show memo of detail information
        $scope.OpenTask = function (obj) {
            key = obj.currentTarget.id;
                if ($(".blockInsertTask").css("display") === "none") {
                    $(".blockInsertTask").css("display", "none");
                    $(".blockInsertTask").fadeIn();
                }
            // Getting of values of task to show memo
            TService.GetValuesTask().then(function (t) {
                $scope.Task = t.data;
                if ($scope.Task[3] !== "1") {
                    $(".finishtag").fadeIn();
                    $(".RemoveTask").fadeIn();
                    $(".Cancelfinishtag").css("display", "none");
                } else {
                    $(".finishtag").fadeOut();
                    setTimeout(function () {
                        $(".Cancelfinishtag").fadeIn();
                    }, 500);
                }
                $(".t-title").val($scope.Task[0]);
                $(".TASKID").val($scope.Task[5]);
                $(".finishtag").attr("id", $scope.Task[5]);
                $(".Cancelfinishtag").attr("id", $scope.Task[5]);
                $(".t-disc").val($scope.Task[1]);
                $(".RemoveTask").attr("id", $scope.Task[5]);
                $(".TASKSTATUS").val($scope.Task[3]);
                $(".HIDDDENTAGS").val($scope.Task[4]);

                var date = $scope.Task[2].substring(0, 2);
                date = $scope.Task[2].substring(3, 5) + "-" + date;
                date = $scope.Task[2].substring(6, 10) + "-" + date;

                $(".valDate").val(date);
                $("#myDate").attr("value", date);

                // setting to null for the Prototype
                arrayOfTags = $.grep(arrayOfTags, function (el) { return null });
                var re = /[a-zа-я0-9-]+/gi;
                var str = $scope.Task[4];
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
        };

    });

    // Factory "TService" for WEB API services TagsController and TaskController 
    ModuleManager.factory("TService", function ($http) {
        var fac = {};
        fac.GetTags = function () {
            return $http({ method: "GET", url: "/api/Tags/GetTags/", params: { "name": key } });
        }
        fac.DeleteTask = function () {
            return $http({ method: "GET", url: "/api/Tasks/DeleteTask/", params: { 'idTask': idTask } });
        }
        fac.BeginActTask = function () {
            return $http({ method: "POST", url: "/api/Tasks/SetActiveStatus/", params: { 'idTask': idTask } });
        }
        fac.EndActTask = function () {
            return $http({ method: "POST", url: "/api/Tasks/SetEndStatus/", params: { 'idTask': idTask } });
        }
        fac.GetTasks = function () {
            return $http({ method: "Get", url: "/api/Tasks/GetTasks/" });
        }
        fac.GetValuesTask = function () {
            return $http({ method: 'GET', url: "/api/Tasks/GetValuesTask", params: { 'id': key } });
        }
        return fac;
    });

})();


