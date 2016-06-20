var ctrl = angular.module("controllers", ["services"]);

ctrl.controller('IncidentController', ['$scope', 'signalRFactory', function ($scope, signalRFactory) {
    var bind = this;
    this.incidentHub = signalRFactory.connection.hub.createHubProxy("IncidentHub");
    this.incidents = [];
    this.table = "";

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
            console.log("ServerHub connection success")
            bind.update(1000 * 60);
        });
    }

    this.keyDown = function (incident, event) {
        if (event.keyCode == 13) {
            this.submit(incident, event);
            return;
        }
        if (event.keyCode == 27) {
            this.cancel(incident, event);
            return;
        }
    }

    this.cancel = function (incident, evt) {
        incident.update_availability = incident.update_availability.replace(/(<br>|<br\/>|<div>|<\/div>)/g, "");
        incident.update_notes = incident.update_notes.replace(/(<br>|<br\/>|<div>|<\/div>)/g, "");
        if (incident.update_availability != incident.availability || incident.update_notes != incident.notes) {
            $(evt.target).parent().addClass("danger");
        } else {
            $(evt.target).parent().removeClass("danger");
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
        var inc = 0;
        console.log("updating " + number, bind.table);
        bind.incidents.forEach(function (inc, i, arr) {
            if (inc.number == number) {
                arr[i].notes = notes;
                arr[i].availability = availability;
                inc++;
            }
        });
        console.log("updated " + inc.number, inc + " items updated", bind.table);
        $scope.$apply();
    });
}]);