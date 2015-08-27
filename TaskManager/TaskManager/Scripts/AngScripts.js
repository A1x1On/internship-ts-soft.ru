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
            $scope.mass = "ПРОБА";

            if (idTask != 0) {
                
                // Use TService for Deleting of task
                TService.DeleteTask().then(function (d) {
                    //$(".Tasks>i").remove();
                    $("i#" + idTask).fadeOut();

                    //$scope.Tasks = d.data; in View ng-repeat
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

        $scope.EndActTask = function (obj) {
            idTask = obj.currentTarget.id;
            TService.EndActTask().then(function (d) {
                $scope.Tags = d.data;
                $("i#" + idTask).children(".d-status").html("<b>Cтатус: </b>Завершен");
              
            }, function () {
                alert("Fail of getting tags");
            });
        }

        $scope.ChangeStatus = function (obj) {
            idTask = obj.currentTarget.id;
            TService.ChangeStatus().then(function (d) {
                $scope.Tags = d.data;
                $("i#" + idTask).children(".d-status").html("<b>Cтатус: </b>Активный");
            }, function () {
                alert("Fail of getting tags");
            });
        }

    });

    ModuleManager.factory("TService", function ($http) {
        var fac = {};
        fac.GetTags = function () {
            return $http({ method: "GET", url: "/api/Tags/GetTags/", params: { "name": key } });
        }
        fac.DeleteTask = function () {
            return $http({ method: "GET", url: "/api/Tasks/DeleteTask/", params: { 'idTask': idTask } });
        }
        fac.ChangeStatus = function () {
            return $http({ method: "POST", url: "/api/Tasks/SetActiveStatus/", params: { 'idTask': idTask } });
        }
        fac.EndActTask = function () {
            return $http({ method: "POST", url: "/api/Tasks/SetEndStatus/", params: { 'idTask': idTask } });
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
                if ($scope.Task[3] !== "1") {
                    $(".finishtag").fadeIn();
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
            return $http({ method: 'GET', url: "/api/Tasks/GetValuesTask", params: { 'id': key } });
        }
        return fac2;
    });

})();


