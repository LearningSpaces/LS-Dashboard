//JavaScript File

var SERVICENOW_REFRESH_INTERVAL = 1000*60;
var WIW_REFRESH_INTERVAL = 1000*60;
var CARTSTATUS_REFRESH_INTERVAL = 1000*30;
var TECHCHECK_REFRESH_INTERVAL = 1000*5;
var DATETIME_REFRESH_INTERVAL = 1000*1;

var timers = {};

function updateIncidentTable(cases,table,icon_path){
	// Initialize the row variable
	rows = "";
	// Iterate through all of the returned cases
	switch (table){
		case (document.getElementById("faculty_assists")):
			for(var i=0; i < cases.length; i++){
				rows += "<tr onclick=\"window.open('https://ounew.service-now.com/nav_to.do?uri=incident.do?sys_id="+cases[i].sys_id+"')\">";
				// Inset the case information into the row
				rows += "<td>"+cases[i].combined_location+"</td><td>"+cases[i].number+"</td><td>"+cases[i].time_available+"</td><td>"+cases[i].dv_assigned_to+"</td></tr>\n";
			}
			rows += "<tr></tr>";
			break;
		default:
			for(var i=0; i < cases.length; i++){
				rows += "<tr onclick=\"window.open('https://ounew.service-now.com/nav_to.do?uri=incident.do?sys_id="+cases[i].sys_id+"')\">";
				// Inset the case information into the row
				rows += "<td>"+cases[i].combined_location+"</td><td>"+cases[i].number+"</td><td>"+cases[i].time_available+"</td><td>"+getPriority(cases[i].priority)+"</td></tr>\n";
			}
			rows += "<tr></tr>";
	}
	// Get the tbody element of the table
	var tbody =	table.tBodies.item(0);
	// Check if the tbody element exists
	if(tbody == null){
		// Create a tbody element attached to the table
		tbody = table.appendChild(document.createElement('tbody'));
	}
	// Set the row data for the table
	tbody.innerHTML = rows;
	table.getElementsByTagName("h3").item(0).innerHTML = cases.length;
	table.getElementsByTagName("img").item(0).src = icon_path;
}

//Callback to update the Shift workers table
function updateShiftWorkers(results){
	try{
		var currentTable = document.getElementById('currentWorkers');
		var nextTable = document.getElementById('nextWorkers');
		var currentRows = "";
		var nextRows = "";
		for(var i=0; i < results.current.length; i++){
			currentRows += "<tr><td>"+results.current[i].first_name+" "+results.current[i].last_name+"</td></tr>\n";
		}
		tbody =	currentTable.tBodies.item(0);
		if(tbody == null){
			tbody = currentTable.appendChild(document.createElement('tbody'));
		}
		tbody.innerHTML = currentRows;
		for(i=0; i< results.next.length; i++){
			nextRows += "<tr><td>"+results.next[i].first_name+" "+results.next[i].last_name+"</td></tr>\n";
		}
		tbody =	nextTable.tBodies.item(0);
		if(tbody == null){
			tbody = nextTable.appendChild(document.createElement('tbody'));
		}
		tbody.innerHTML = nextRows;
	} catch(e){
		console.log(e);
	}
	timers.wiw = setTimeout(function(){getAsyncShifts('now',updateShiftWorkers)},WIW_REFRESH_INTERVAL)
}

// Callback to update the TechCheck pie charts
function updateTechChecks(results){
	try {
		// Static variable: first
		// Used to keep the charts from animating from 0 for every update
		if ( typeof updateTechChecks.first == 'undefined' ) {
			updateTechChecks.first = true;
		}
		var asCtx = document.getElementById("asChecksCanvas").getContext("2d");
		var engCtx = document.getElementById("engChecksCanvas").getContext("2d");
		var resCtx = document.getElementById("resChecksCanvas").getContext("2d");
		var asData = [
			{
				// Completed
				value: results.asLabs.completed,
				color: "#fdaf33",
				highlight: "#fdaf33",
				label: "completed"
			},
			{
				// Remaining
				value: results.asLabs.remaining,
				color: "#2a2e30",
				highlight: "#2a2e30",
				label: "remaining"
			}
		]
		var engData = [
			{
				// Completed
				value: results.engLabs.completed,
				color: "#609eb5",
				highlight: "#609eb5",
				label: "completed"
			},
			{
				// Remaining
				value: results.engLabs.remaining,
				color: "#2a2e30",
				highlight: "#2a2e30",
				label: "remaining"
			}
		]
		var resData = [
			{
				// Completed
				value: results.resLabs.completed,
				color: "#96a0a8",
				highlight: "#96a0a8",
				label: "completed"
			},
			{
				// Remaining
				value: results.resLabs.remaining,
				color: "#2a2e30",
				highlight: "#2a2e30",
				label: "remaining"
			}
		]
		if(updateTechChecks.first){
			// Makes new charts with TechCheck data
			console.log("Generating Doughnut Charts");
			updateTechChecks.asLabChart = new Chart(asCtx).Doughnut(asData, {segmentStrokeColor: "e39d2d", percentageInnerCutout : 70, animationEasing : "easeInOutCubic"});
			updateTechChecks.engLabChart = new Chart(engCtx).Doughnut(engData, {segmentStrokeColor: "e39d2d", percentageInnerCutout : 70, animationEasing : "easeInOutCubic" });
			updateTechChecks.resLabChart = new Chart(resCtx).Doughnut(resData, {segmentStrokeColor: "e39d2d", percentageInnerCutout : 70, animationEasing : "easeInOutCubic" });
			updateTechChecks.first = false;
		} else {
			// Updates the existing charts
			updateTechChecks.asLabChart.segments[0].value = asData[0].value;
			updateTechChecks.asLabChart.segments[1].value = asData[1].value;
			updateTechChecks.asLabChart.update();
			updateTechChecks.engLabChart.segments[0].value = engData[0].value;
			updateTechChecks.engLabChart.segments[1].value = engData[1].value;
			updateTechChecks.engLabChart.update();
			updateTechChecks.resLabChart.segments[0].value = resData[0].value;
			updateTechChecks.resLabChart.segments[1].value = resData[1].value;
			updateTechChecks.resLabChart.update();
		}
	} catch(e) {
		console.log(e);
	}
	timers.techchecks = setTimeout(function(){getAsyncTechChecks(updateTechChecks);},TECHCHECK_REFRESH_INTERVAL);
}

// Callback to update the Cart Status table
function updateCartStatus(results){
	var table = document.getElementById("cart_status");
	var rows = "";
	results.carts.sort(function(a,b){
		return a.cart.length-b.cart.length;
	});
	for(var i=0; i < results.carts.length; i++){
		rows += "<tr><td>"+results.carts[i].cart+"</td><td>"+results.carts[i].person+"</td><td>"+getStatusIcon(results.carts[i].status_code)+"</td></tr>\n";
	}
	table.tBodies.item(0).innerHTML = rows;
	
	timers.cart_status = setTimeout(function(){getAsyncCartsStatus(updateCartStatus);},CARTSTATUS_REFRESH_INTERVAL);
}

// Returns an html img tag containing the paths to
// the red,yellow,red icons given a status code
function getStatusIcon(status_code){
	switch(status_code){
		case("0"):
			return "<img src='res/images/priority_icons/2_high_yellow.gif'>";
		case("1"):
			return "<img src='res/images/priority_icons/3_low_green.gif'>";
		case("2"):
			return "<img src='res/images/priority_icons/1_crit_red.gif'>";
		default:
			return "";
	}
}

// Returns an image tag with the location of icons to represent the different priorities
function getPriority(priority){
	switch(priority){
		case ("1"):
			return "<img src='res/images/priority_icons/1_crit_red.gif'>";
		case ("2"):
			return "<img src='res/images/priority_icons/2_high_yellow.gif'>";
		case ("3"):
			return "<img src='res/images/priority_icons/3_low_green.gif'>";
		case ("4"):
			return "<img src='res/images/priority_icons/3_low_green.gif'>";
		default:
			return "";
	}
}

// Function used as the callback in the setInterval to update
// all of the info on the page
function updateCaseInfo(){
	try {
		// Update Actionable table
		document.getElementById("actionable").getElementsByTagName("img").item(0).src = "res/images/header_icons/lloader.gif";
		document.getElementById("faculty_assists").getElementsByTagName("img").item(0).src = "res/images/header_icons/lloader.gif"
		document.getElementById("class_in_session").getElementsByTagName("img").item(0).src = "res/images/header_icons/lloader.gif"
		getAsyncCases("incident_state<6^short_description>=~^ORcategory=Non-Case",				//actionable query
			true,																				//get display values
			updateIncidentTable,
			document.getElementById("actionable"),
			"res/images/header_icons/Actionables-icon.gif"
		);
		// Update Class in Session table
		getAsyncCases("incident_state<6^u_class_in_session=Yes",								// CiS query
			true,																				// Get display values
			updateIncidentTable,
			document.getElementById("class_in_session"),
			"res/images/header_icons/Urgent-Icon.gif"
		);
		// Update Faculty Assist table
		getAsyncCases("incident_state<6^subcategory=Faculty Assist",							// Faculty Assist query
			true,																				// Get display values
			updateIncidentTable,
			document.getElementById("faculty_assists"),
			"res/images/header_icons/faculty-assist-icon.gif"
		);
		
	} catch(e) {
		console.log(e);
	}
	
	timers.service_now = setTimeout(updateCaseInfo,SERVICENOW_REFRESH_INTERVAL);
}
// Pulls the current date/time and applies it to the #date_time div
function updateDateTime(){
	try {
		// Get the #date_time div object
		var elem = document.getElementById("date_time");
		// Get current date/time
		var d = new Date();
		// Apply the formatted date to the #date_time div
		elem.innerHTML = getMonthString(d.getMonth())+" "+(d.getDate()<10?"0":"")+d.getDate()+", "+d.getFullYear()+" - "+(d.getHours()%12==0?12:d.getHours()%12)+":"+(d.getMinutes()<10?"0":"")+d.getMinutes()+" "+(d.getHours < 12 ? "AM" : "PM");
		
		// Translates the number returned from JavaScript's 
		function getMonthString(month){
			switch(month){
				case(0):
					return "January";
				case(1):
					return "February";
				case(2):
					return "March";
				case(3):
					return "April";
				case(4):
					return "May";
				case(5):
					return "June";
				case(6):
					return "July";
				case(7):
					return "August";
				case(8):
					return "September";
				case(9):
					return "October";
				case(10):
					return "November";
				case(11):
					return "December";
			}
		}
	} catch(e) {
		console.log(e);
	}
	
	timers.datetime = setTimeout(updateDateTime, DATETIME_REFRESH_INTERVAL);
}

// Callback loaded when all of the elements on the page has loaded
window.onload = function (){
	// Update all of the main page information
	updateCaseInfo();
	// Updates the shift information
	getAsyncShifts('now',updateShiftWorkers);
	//Updates Date/Time
	updateDateTime();
	//Update Tech Checks
	getAsyncTechChecks(updateTechChecks);
	//Update Cart Status
	getAsyncCartsStatus(updateCartStatus);
};