// JavaScript Document

var filter = {
	Eng: true,
	AS: true,
	Res: true
}
var cases = {
	Act: [],
	FA: [],
	CIS: []
}
var checks;
function filter_cases(cases, filter) {
	var _cases = {
		Act: [],
		FA: [],
		CIS: []
	}
	for(var i=0;i<cases.Act.length;i++){
		if((cases.Act[i].sector == 0 && filter.Eng) || (cases.Act[i].sector == 1 && filter.AS) || (cases.Act[i].sector == 2 && filter.Res)){
			_cases.Act.push(cases.Act[i]);
		}
	}
	for(var i=0;i<cases.FA.length;i++){
		if((cases.FA[i].sector == 0 && filter.Eng) || (cases.FA[i].sector == 1 && filter.AS) || (cases.FA[i].sector == 2 && filter.Res)){
			_cases.FA.push(cases.FA[i]);
		}
	}
	for(var i=0;i<cases.CIS.length;i++){
		if((cases.CIS[i].sector == 0 && filter.Eng) || (cases.CIS[i].sector == 1 && filter.AS) || (cases.CIS[i].sector == 2 && filter.Res)){
			_cases.CIS.push(cases.CIS[i]);
		}
	}
	return _cases;
}
function filter_checks(checks, filter){
	return {remaining: (filter.Eng?checks.engLabs.remaining:0)+(filter.Res?checks.resLabs.remaining:0)+(filter.AS?checks.asLabs.remaining:0),
		completed: (filter.Eng?checks.engLabs.completed:0)+(filter.Res?checks.resLabs.completed:0)+(filter.AS?checks.asLabs.completed:0)}
}
function updateIncidentTable(cases,table){
	// Initialize the row variable
	rows = "";
	// Iterate through all of the returned cases
	for(var i=0; i < cases.length; i++){
		rows += "<tr onclick=\"window.open('https://ounew.service-now.com/nav_to.do?uri=incident.do?sys_id="+cases[i].sys_id+"')\">";
		// Inset the case information into the row
		rows += "<td>"+cases[i].combined_location+"</td><td>"+cases[i].number+"</td><td>"+cases[i].time_available+"</td><td>"+getPriority(cases[i].priority)+"</td></tr>\n";
	}
	rows += "<tr></tr>";
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
}
function getPriority(priority){
	switch(priority){
		case ("1"):
			return "<img src='../res/images/priority_icons/1_crit_red.gif'>";
		case ("2"):
			return "<img src='../res/images/priority_icons/2_high_yellow.gif'>";
		case ("3"):
			return "<img src='../res/images/priority_icons/3_low_green.gif'>";
		case ("4"):
			return "<img src='../res/images/priority_icons/3_low_green.gif'>";
		default:
			return "";
	}
}
function updateIncidents(cases){
	updateIncidentTable(cases.Act,document.getElementById("actionable"));
	document.getElementById("act_leaf").innerHTML = cases.Act.length;
	updateIncidentTable(cases.CIS,document.getElementById("class_in_session"));
	document.getElementById("cis_leaf").innerHTML = cases.CIS.length;
	updateIncidentTable(cases.FA,document.getElementById("faculty_assist"));
	document.getElementById("fa_leaf").innerHTML = cases.FA.length;
}
function toggle_branch(elem){
	switch (elem.id){
		case "eng_leaf":
			filter.Eng = !filter.Eng;
			console.log("Engineeing Filter Toggled:"+filter.Eng);
			break;
		case "as_leaf":
			filter.AS = !filter.AS;
			console.log("Arts & Sciences Filter Toggled:"+filter.AS);
			break;
		case "res_leaf":
			filter.Res = !filter.Res;
			console.log("Residential Filter Toggled:"+filter.Res);
			break;
		default:
			console.log("Element ID not recognized");
			break;
	}
	updateLeafClasses();
	updateIncidents(filter_cases(cases,filter));
	updateTechChecks(filter_checks(checks,filter));
	updateCookies();
}
function updateLeafClasses(){
	var elem = document.getElementById("eng_leaf");
	if(filter.Eng){
		elem.classList.remove("leaf_unselected");
		elem.classList.add("leaf_selected");
	} else {
		elem.classList.remove("leaf_selected");
		elem.classList.add("leaf_unselected");
	}
	elem = document.getElementById("as_leaf");
	if(filter.AS){
		elem.classList.remove("leaf_unselected");
		elem.classList.add("leaf_selected");
	} else {
		elem.classList.remove("leaf_selected");
		elem.classList.add("leaf_unselected");
	}
	var elem = document.getElementById("res_leaf");
	if(filter.Res){
		elem.classList.remove("leaf_unselected");
		elem.classList.add("leaf_selected");
	} else {
		elem.classList.remove("leaf_selected");
		elem.classList.add("leaf_unselected");
	}
}
function updateInfo(){
	document.getElementById("actionable").getElementsByTagName("img").item(0).src = "../res/images/header_icons/lloader.gif";
	document.getElementById("faculty_assist").getElementsByTagName("img").item(0).src = "../res/images/header_icons/lloader.gif";
	document.getElementById("class_in_session").getElementsByTagName("img").item(0).src = "../res/images/header_icons/lloader.gif";
	document.getElementById("tech_check").getElementsByTagName("img").item(0).src = "../res/images/header_icons/lloader.gif";
	getAsyncCases("incident_state<6^short_description>=~^ORcategory=Non-Case",true,
		function(results){
			document.getElementById("actionable").getElementsByTagName("img").item(0).src = "../res/images/header_icons/Actionables-icon.gif";
			cases.Act = results;
			updateIncidents(filter_cases(cases, filter));
		}
	);
	getAsyncCases("incident_state<6^u_class_in_session=Yes",true,
		function(results){
			document.getElementById("class_in_session").getElementsByTagName("img").item(0).src = "../res/images/header_icons/Urgent-Icon.gif";
			cases.CIS = results;
			updateIncidents(filter_cases(cases, filter));
		}
	);
	getAsyncCases("incident_state<6^subcategory=Faculty Assist",true,
		function(results){
			document.getElementById("faculty_assist").getElementsByTagName("img").item(0).src = "../res/images/header_icons/faculty-assist-icon.gif";
			cases.FA = results;
			updateIncidents(filter_cases(cases, filter));
		}
	);
	getAsyncTechChecks(function(results){
		document.getElementById("tech_check").getElementsByTagName("img").item(0).src = "../res/images/header_icons/Tech-Check-Icons.gif";
		checks = results;
		updateTechChecks(filter_checks(checks,filter));
	});
}
function updateCookies(){
	setCookie("Eng",filter.Eng,24*60*60*1000);
	setCookie("AS",filter.AS,24*60*60*1000);
	setCookie("Res",filter.Res,24*60*60*1000);
}
// pulls the current date/time and applies it to the #date_time div
function updateDateTime(){
	// get the #date_time div object
	var elem = document.getElementById("date_time");
	// get current date/time
	var d = new Date();
	// apply the formatted date to the #date_time div
	elem.innerHTML = getMonthString(d.getMonth())+" "+(d.getDate()<10?"0":"")+d.getDate()+", "+d.getFullYear()+" - "+(d.getHours()%12==0?12:d.getHours()%12)+":"+(d.getMinutes()<10?"0":"")+d.getMinutes()+" "+(d.getHours < 12 ? "AM" : "PM");
	
	// translates the number returned from JavaScript's 
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
}
function getCookie(cname){
	var cookies = document.cookie.split(';');
	for(var i=0;i<cookies.length;i++){
		if(cookies[i].trim().indexOf(cname+"=") == 0){
			return cookies[i].trim().substring(cname.length+1, cookies[i].trim().length);
		}
	}
	return null
}
function setCookie(cname,cvalue,exp_time_length){
	var exp_date = new Date();
	exp_date.setTime(exp_date.getTime()+exp_time_length);
	document.cookie = cname + "=" + cvalue + "; " + "expires=" + exp_date.toUTCString();
}
// Callback to update the TechCheck pie charts
function updateTechChecks(checks){
	document.getElementById("techcheck_leaf").innerHTML = checks.remaining;
	// Static variable: first
	// Used to keep the charts from animating from 0 for every update
	if ( typeof updateTechChecks.first == 'undefined' ) {
        updateTechChecks.first = true;
    }
	var techCheckCtx = document.getElementById("tech_check_cvs").getContext("2d");
	checks.total = checks.completed+checks.remaining;
	if(checks.completed==0&&checks.remaining==0) checks.remaining=1;
	var techCheckData = [
		{
			// Completed
			value: checks.completed,
			color: "#6eb4ce",
			highlight: "#6eb4ce",
			label: "completed"
		},
		{
			// Remaining
			value: checks.remaining,
			color: "#FFFFFF",
			highlight: "#FFFFFF",
			label: "remaining"
		}
	]
	if(updateTechChecks.first){
		// Makes new charts with TechCheck data
		console.log("Generating Doughnut Charts");
		updateTechChecks.techCheckChart = new Chart(techCheckCtx).Doughnut(techCheckData, { showTooltips: false, segmentStrokeColor: "e39d2d", percentageInnerCutout : 70, animationEasing : "easeInOutCubic"});
		updateTechChecks.first = false;
	} else {
		// Updates the existing charts
		updateTechChecks.techCheckChart.segments[0].value = techCheckData[0].value;
		updateTechChecks.techCheckChart.segments[1].value = techCheckData[1].value;
		updateTechChecks.techCheckChart.update();
	}
	document.getElementById("tech_check").getElementsByTagName("h3").item(0).innerHTML = checks.completed+" / "+checks.total;
}
window.onload = function(){
	updateInfo();
	updateDateTime();
	if(getCookie("Eng") == "true" || getCookie("Eng") == "false") {
		filter.Eng = (getCookie("Eng") == "true");
	}
	if(getCookie("AS") == "true" || getCookie("AS") == "false") {
		filter.AS = (getCookie("AS") == "true");
	}
	if(getCookie("Res") == "true" || getCookie("Res") == "false") {
		filter.Res = (getCookie("Res") == "true");
	}
	updateCookies();
	updateLeafClasses();
	console.log(filter);
	console.log(document.cookie);
	setInterval(updateInfo, PAGE_REFRESH_TIME);
	setInterval(updateDateTime, DATE_REFRESH_TIME);
}