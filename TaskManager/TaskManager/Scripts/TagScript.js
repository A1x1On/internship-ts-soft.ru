(function () {

    // Init module for actions of tags
    var ModuleTag = angular.module("AngularManager", ["ngRoute"]);

    // Init controller DinamicTag
    ModuleTag.controller("DinamicTag", function($scope, TagService) {
        $scope.TagListToDOM = function(name) {
            $scope.mad = name + " :: " + name.length;
            word = name;

            // Use TagService for getting data tags
            TagService.GetTags().then(function(d) {
                $scope.Tags = d.data; // передаем в массив JS
            }, function() {

                alert('Failed');
            });
        }
    });

    // Init TagService
    ModuleTag.factory('TagService', function ($http) {
        var fac = {};
        fac.GetTags = function () {
            return $http({ method: 'GET', url: '/Manager/GetTags/', params: { 'name': word } });
        }
        return fac;

    });

})();


