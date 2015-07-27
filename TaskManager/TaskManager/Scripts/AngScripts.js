(function () {

    // Init module for actions of tags
    var ModuleManager = angular.module("AngularManager", ["ngRoute"]);

   
    // Init controller DinamicTag
    ModuleManager.controller("DinamicTag", function ($scope, TagService) {

        $scope.TagListToDOM = function(name) {
            //$scope.mad = name;
            key = name;
            if (name != "") {
                // Use TagService for getting data tags
                TagService.GetTags().then(function(d) {
                    $scope.Tags = d.data;
                }, function() {

                    alert('Failed');
                });
            }
            else {
                $scope.Tags = "";
            }
        }
    });

    // Init TagService
    ModuleManager.factory('TagService', function ($http) {
        var fac = {};
        fac.GetTags = function () {
            return $http({ method: 'GET', url: '/Manager/GetTags/', params: { 'name': key } });
        }
        return fac;

    });





    ModuleManager.controller("OpenTask", function ($scope, OpenTaskService) {
        $scope.ClickedTask = function (TaskId) {
            key = TaskId;
            
            
           

            // Use TagService for getting data tags
            OpenTaskService.GetTask().then(function (t) {
                $scope.Task = t.data;
             
                $(".t-title").val($scope.Task[0]);
                $(".TASKID").val($scope.Task[5]);
                $(".t-disc").val($scope.Task[1]);
                $(".valDate").val($scope.Task[2]);
                $("#myDate").attr("value", $scope.Task[2]);
                $(".TASKSTATUS").val($scope.Task[3]);
                
                   
          
                

            }, function () {

                alert('Failed');
            });

          
          


        }
    });

    // Init TagService
    ModuleManager.factory("OpenTaskService", function ($http) {
        var fac2 = {};
        fac2.GetTask = function () {
            return $http({ method: 'GET', url: "/Manager/OpenTask/", params: { 'TaskId': key } });
        }
        return fac2;

    });




})();


