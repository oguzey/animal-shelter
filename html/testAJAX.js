
function sendRequest(type) {
	  var xhttp = new XMLHttpRequest();
	    xhttp.onreadystatechange = function() {
			    if (xhttp.readyState == 4 && xhttp.status == 200) {
					     document.getElementById("demo").innerHTML = xhttp.responseText;
						     }
				  };
		  xhttp.open(type, "/data", true);
		    xhttp.send();
}

function sendGet() {
	sendRequest("GET");
}

function sendPost() {
	sendRequest("POST");
}
