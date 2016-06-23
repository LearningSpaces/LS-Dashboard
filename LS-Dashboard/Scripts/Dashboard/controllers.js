var ctrl = angular.module("controllers", ["services"]);

ctrl.controller('IncidentController', ['$scope', 'signalRFactory', function ($scope, signalRFactory) {
    var bind = this;
    this.incidentHub = signalRFactory.connection.hub.createHubProxy("IncidentHub");
    this.incidents = [];
    this.table = "";
    this.timeout = -1;

    this.init = function (table) {
        this.table = table;
        console.log(table);
        console.log('Connecting to hub: IncidentHub');

        this.update = function (interval) {
            console.log("updating cases for: " + bind.table);
            bind.incidentHub.server.GetIncidents().done(
                function (result) {
                    var new_incidents = result[bind.table];
                    console.log(result);
                    bind.incidents = new_incidents;
                    bind.incidents.forEach(function (incident, i, arr) {
                        incident.update_notes = incident.notes;
                        incident.update_availability = incident.availability;
                    });
                    $scope.$apply();
                    console.log(bind.incidents);
                    setTimeout(bind.update, interval, interval);
                }).fail(
                function (error) {
                    console.log("Get Incidents Failed: ");
                    console.log(error);
                    setTimeout(bind.update, interval, interval);
                });
        }

        signalRFactory.start(function () {
            console.log("IncidentHub connection success: " + bind.table);
            bind.update(1000 * 60);
        });
    }

    this.keyUp = function (incident, event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            this.submit(incident, event);
        }
        if (event.keyCode == 27) {
            event.preventDefault();
            var field = $(event.target).attr("ng-model").replace("incident.", "");
            incident[field] = incident[field.replace("update_", "")];
            $(event.target).blur();
        }
    }

    this.cancel = function (incident, evt) {
        incident.update_availability = incident.update_availability.replace(/(<br>|<br\/>|<div>|<\/div>)/g, "");
        incident.update_notes = incident.update_notes.replace(/(<br>|<br\/>|<div>|<\/div>)/g, "");
        if (incident.update_availability != incident.availability || incident.update_notes != incident.notes) {
            $(evt.target).parent().addClass("danger");
            $(evt.target).parent().attr("title", "Changes not saved (Enter to save) (Esc to cancel)")
        } else {
            $(evt.target).parent().removeClass("danger");
            $(evt.target).parent().attr("title", "");
        }

        console.log("edit " + incident.number + " not submitted");
    }

    this.submit = function (incident, evt) {
        console.log("submitting " + incident.number);
        incident.update_availability = incident.update_availability.replace(/(<br>|<br\/>|<div>|<\/div>)/g, "");
        incident.update_notes = incident.update_notes.replace(/(<br>|<br\/>|<div>|<\/div>)/g, "");
        $(evt.target).parent().parent().children().removeClass("danger");
        this.incidentHub.server.NewIncidentUpdate(incident.number, incident.update_notes, incident.update_availability).done(
            function (result) {
                console.log("submitted " + incident.number);
            }).fail(
            function (error) {
                console.log("submission failed for " + incident.number, error);
            });
    }

    this.incidentHub.on("updateIncident", function (number, notes, availability) {
        var incs = 0;
        console.log("updating " + number, bind.table);
        bind.incidents.forEach(function (inc, i, arr) {
            if (inc.number == number) {
                arr[i].notes = notes;
                arr[i].availability = availability;
                incs++;
            }
        });
        console.log("updated " + number, incs + " items updated", bind.table);
        $scope.$apply();
    });
}]);

ctrl.controller("WIWController", ["$scope", "signalRFactory", function ($scope, signalRFactory) {
    var bind = this;
    this.wiwHub = signalRFactory.connection.hub.createHubProxy("WiWHub");
    this.current_shifts = [];
    this.next_shifts = [];

    this.update = function (interval) {
        console.log("updating shifts");
        bind.wiwHub.server.GetWorkers().done(
            function (result) {
                console.log(result);
                bind.current_shifts = result.current_shifts;
                bind.next_shifts = result.next_shifts;
                $scope.$apply();
                setTimeout(bind.update, interval, interval);
            }).fail(
            function (error) {
                console.log("Get Worker Shifts Failed: ");
                console.log(error);
                setTimeout(bind.update, interval, interval);
            });
    }

    signalRFactory.start(function () {
        console.log("WiWHub connection success")
        bind.update(1000 * 60 * 5);
    });
}]);

ctrl.controller("TechCheckController", ["$scope", "signalRFactory", function ($scope, signalRFactory) {
    var bind = this;
    this.techCheckHub = signalRFactory.connection.hub.createHubProxy("TechCheckHub");
    this.eng = {};
    this.res = {};
    this.as = {};

    this.update = function (interval) {
        console.log("updating tech checks");
        bind.techCheckHub.server.GetTechChecks().done(
            function (result) {
                console.log(result);
                bind.eng = (result.filter(function (sector) {
                    return sector.name == "Engineering Sector";
                }))[0];
                bind.res = (result.filter(function (sector) {
                    return sector.name == "Residential Sector";
                }))[0];
                bind.as = (result.filter(function (sector) {
                    return sector.name == "Arts & Sciences Sector";
                }))[0];
                $scope.$apply();
                setTimeout(bind.update, interval, interval);
            }).fail(
            function (error) {
                console.log("Get Tech Checks Failed: ");
                console.log(error);
                setTimeout(bind.update, interval, interval);
            });
    }

    signalRFactory.start(function () {
        console.log("TechCheck connection success")
        bind.update(1000 * 30);
    });
}]);

ctrl.controller("CartStatusController", ["$scope", "signalRFactory", function ($scope, signalRFactory) {
    var bind = this;
    this.cartsHub = signalRFactory.connection.hub.createHubProxy("CartsHub");
    this.carts = [];

    this.update = function (interval) {
        console.log("updating cart statuses");
        bind.cartsHub.server.GetCartStatuses().done(
            function (result) {
                console.log(result);
                bind.carts = result;
                $scope.$apply();
                setTimeout(bind.update, interval, interval);
            }).fail(
            function (error) {
                console.log("Get Cart Statuses Failed: ");
                console.log(error);
                setTimeout(bind.update, interval, interval);
            });
    }

    signalRFactory.start(function () {
        console.log("CartsHub connection success")
        bind.update(1000 * 30);
    });
}]);