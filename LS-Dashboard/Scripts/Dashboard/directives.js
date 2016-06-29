var dir = angular.module("directives", [])

// Directive to update property using ng-model just like an <input> using a contentEditable div
dir.directive("contenteditable", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, element, attrs, ngModel) {
            function read() {
                ngModel.$setViewValue(element.html());
            }

            ngModel.$render = function () {
                element.html(ngModel.$viewValue || "");
            };

            element.bind("keyup change", function () {
                scope.$apply(read);
            });
        }
    };
});