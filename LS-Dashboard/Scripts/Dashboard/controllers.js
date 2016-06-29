// Creates the controllers submodule
var ctrl = angular.module("controllers", ["services", "chart.js"]);

// Adds the IncidentController to the module, requires the controller $scope and the signalRFactory directive
ctrl.controller('IncidentController', ['$scope', 'signalRFactory', function ($scope, signalRFactory) {
    // Add a reference to this controller scope so that anonymous functions can update variables
    var bind = this;
    var loadingIcon = "/Images/lloader.gif"
    var normalIcon = ""
    //Initialize icon path
    this.icon = window.location.pathname + normalIcon;
    // Connect to the signalR hub
    this.incidentHub = signalRFactory.connection.hub.createHubProxy("IncidentHub");
    // Tnitialize the incidents field
    this.incidents = [];
    // Initialize the table field (this get set by passing it into the init function from the HTML directive
    this.table = "";
    this.label = "";
    // Represents the refresh timeout interval reference
    this.timeout = 0;

    // Sets the table field, connects to the signalR hub
    // Onn connection starts the data refresh at a 60 second interval
    this.init = function (table, icon) {
        this.table = table;
        this.label = this.table.replace("_", " ");
        console.log(table);
        console.log('Connecting to hub: IncidentHub');
        normalIcon = icon;
        signalRFactory.start(function () {
            console.log("IncidentHub connection success: " + bind.table);
            bind.update();
        });
    }

    // Function to refresh the controller data at the specified interval
    this.update = function (interval) {
        if (!interval) interval = 1000 * 60;
        if (this.timeout) clearTimeout(this.timeout);
        this.timeout = 0;
        console.log("updating cases for: " + bind.table);
        this.icon = window.location.pathname + loadingIcon;
        // Call the server hub method "GetIncidents"
        bind.incidentHub.server.GetIncidents(bind.table).done(
            // After a successful response sets the controllers incidents
            function (result) {
                var new_incidents = result[bind.table];
                console.log(result);
                bind.incidents = new_incidents;
                // Resets the update_notes/availability, with the info stored in the database
                bind.incidents.forEach(function (incident, i, arr) {
                    incident.update_notes = incident.notes;
                    incident.update_availability = incident.availability;
                });
                bind.icon = window.location.pathname + normalIcon;
                $scope.$apply();
                console.log(bind.incidents);
                // Set a timeout to call this function again with the same interval
                bind.timeout = (bind.update, interval, interval);
            }).fail(
            function (error) {
                bind.icon = window.location.pathname + normalIcon;
                // Log the error
                console.log("Get Incidents Failed: ");
                console.log(error);
                $scope.$apply();
                // Set a timeout to call this function again with the same interval
                bind.timeout = (bind.update, interval, interval);
            });
    }

    // handles the keyup event for the content editable divs
    this.keyUp = function (incident, event) {
        // ENTER was pressed, so stop the event and submit the case info
        if (event.keyCode == 13) {
            event.preventDefault();
            this.submit(incident, event);
        }
        // ESC was pressed, so stop the event, reset the current edit to the stored value, unfocus the element
        if (event.keyCode == 27) {
            event.preventDefault();
            var field = $(event.target).attr("ng-model").replace("incident.", "");
            incident[field] = incident[field.replace("update_", "")];
            $(event.target).blur();
        }
    }

    // User canceled update submission
    this.cancel = function (incident, evt) {
        // Remove the page breaks that the DOM adds
        incident.update_availability = incident.update_availability.replace(/(<br>|<br\/>|<div>|<\/div>)/g, "");
        incident.update_notes = incident.update_notes.replace(/(<br>|<br\/>|<div>|<\/div>)/g, "");
        // If the user changed something, then add the danger class and add a tooltip to show that the changes were not submitted
        // Else remove the class and tooltip
        if (incident.update_availability != incident.availability || incident.update_notes != incident.notes) {
            $(evt.target).parent().addClass("danger");
            $(evt.target).parent().attr("title", "Changes not saved (Enter to save) (Esc to cancel)")
        } else {
            $(evt.target).parent().removeClass("danger");
            $(evt.target).parent().attr("title", "");
        }

        console.log("edit " + incident.number + " not submitted");
    }

    // User submitted changes
    this.submit = function (incident, evt) {
        console.log("submitting " + incident.number);
        // Remove those DOM page breaks
        incident.update_availability = incident.update_availability.replace(/(<br>|<br\/>|<div>|<\/div>)/g, "");
        incident.update_notes = incident.update_notes.replace(/(<br>|<br\/>|<div>|<\/div>)/g, "");
        // Remove any added danger class
        $(evt.target).parent().parent().children().removeClass("danger");
        // Call the "NewIncidentUpdate" function on the server using signalR
        this.incidentHub.server.NewIncidentUpdate(incident.number, incident.update_notes, incident.update_availability).done(
            function (result) {
                console.log("submitted " + incident.number);
            }).fail(
            function (error) {
                console.log("submission failed for " + incident.number, error);
            });
    }

    // Setup the callback for when the server calls "updateIncident" to update the incident in the table
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

// Add the controller for WhenIWork to the module. Requires the controller $scope and the signalRFactory directive
ctrl.controller("WIWController", ["$scope", "signalRFactory", function ($scope, signalRFactory) {
    // Add reference for subroutines
    var bind = this;
    // Connect to WhenIWork SignalR Hub
    this.wiwHub = signalRFactory.connection.hub.createHubProxy("WiWHub");
    // Initialize array of shifts
    this.current_shifts = [];
    this.next_shifts = [];
    this.timeout = 0;

    this.update = function (interval) {
        if (!interval) interval = 1000 * 60 * 5;
        if (this.timeout) clearTimeout(this.timeout);
        this.timeout = 0;

        console.log("updating shifts");
        bind.wiwHub.server.GetWorkers().done(
            function (result) {
                console.log(result);
                bind.current_shifts = result.current_shifts;
                bind.next_shifts = result.next_shifts;
                $scope.$apply();
                bind.timeout = (bind.update, interval, interval);
            }).fail(
            function (error) {
                console.log("Get Worker Shifts Failed: ");
                console.log(error);
                bind.timeout = (bind.update, interval, interval);
            });
    }

    signalRFactory.start(function () {
        console.log("WiWHub connection success")
        bind.update();
    });
}]);

ctrl.controller("TechCheckController", ["$scope", "signalRFactory", function ($scope, signalRFactory) {
    var bind = this;
    var loadingIcon = "/Images/lloader.gif"
    var normalIcon = "/Images/cart-Icon.gif"
    //Initialize icon path
    this.icon = window.location.pathname + normalIcon;
    this.labels = ["Completed", "Remaining"];
    this.eng = {};
    this.res = {};
    this.as = {};
    this.eng.options = {
        rotation: Math.random() * 2 * Math.PI,
        animationEasing: "easeInOutCubic",
        cutoutPercentage: 70
    }
    this.res.options = {
        rotation: Math.random() * 2 * Math.PI,
        animationEasing: "easeInOutCubic",
        cutoutPercentage: 70
    }
    this.as.options = {
        rotation: Math.random() * 2 * Math.PI,
        animationEasing: "easeInOutCubic",
        cutoutPercentage: 70
    }
    this.eng.colors = ["#609eb5", "#2a2e30"];
    this.res.colors = ["#96a0a8", "#2a2e30"];
    this.as.colors = ["#fdaf33", "#2a2e30"];
    this.eng.data = []
    this.res.data = []
    this.as.data = []
    this.techCheckHub = signalRFactory.connection.hub.createHubProxy("TechCheckHub");
    this.timeout = 0;

    this.update = function (interval) {
        if (!interval) interval = 1000 * 30;
        if (this.timeout) clearTimeout(this.timeout);
        this.timeout = 0;

        console.log("updating tech checks");
        bind.icon = window.location.pathname + loadingIcon;
        bind.techCheckHub.server.GetTechChecks().done(
            function (result) {
                console.log(result);
                var eng = (result.filter(function (sector) {
                    return sector.name == "Engineering Sector";
                }))[0];
                var res = (result.filter(function (sector) {
                    return sector.name == "Residential Sector";
                }))[0];
                var as = (result.filter(function (sector) {
                    return sector.name == "Arts & Sciences Sector";
                }))[0];
                bind.eng.data = [eng.completed, eng.remaining];
                bind.res.data = [res.completed, res.remaining];
                bind.as.data = [as.completed, as.remaining];
                bind.icon = window.location.pathname + normalIcon;
                $scope.$apply();
                bind.timeout = setTimeout(bind.update, interval, interval);
            }).fail(
            function (error) {
                console.log("Get Tech Checks Failed: ");
                console.log(error);
                bind.icon = window.location.pathname + normalIcon;
                $scope.$apply();
                bind.timeout = setTimeout(bind.update, interval, interval);
            });
    }

    signalRFactory.start(function () {
        console.log("TechCheck connection success")
        bind.update();
    });
}]);

ctrl.controller("CartStatusController", ["$scope", "signalRFactory", function ($scope, signalRFactory) {
    var bind = this;
    var loadingIcon = "/Images/lloader.gif"
    var normalIcon = "/Images/cart-Icon.gif"
    //Initialize icon path
    this.icon = window.location.pathname + normalIcon;
    this.cartsHub = signalRFactory.connection.hub.createHubProxy("CartsHub");
    this.carts = [];
    this.timeout = 0;
    this.update = function (interval) {
        if (!interval) interval = 1000 * 30;
        if (this.timeout) clearTimeout(this.timeout);
        this.timeout = 0;
        bind.icon = window.location.pathname + loadingIcon;
        console.log("updating cart statuses");
        bind.cartsHub.server.GetCartStatuses().done(
            function (result) {
                console.log(result);
                bind.carts = result;
                bind.icon = window.location.pathname + normalIcon;
                $scope.$apply();
                bind.timeout = setTimeout(bind.update, interval, interval);
            }).fail(
            function (error) {
                console.log("Get Cart Statuses Failed: ");
                console.log(error);
                bind.icon = window.location.pathname + normalIcon;
                $scope.$apply();
                bind.timeout = setTimeout(bind.update, interval, interval);
            });
    }

    signalRFactory.start(function () {
        console.log("CartsHub connection success")
        bind.update();
    });
}]);