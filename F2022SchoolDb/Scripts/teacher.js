function AddTeacher() {

	//goal: send a request which looks like this:
	//POST : http://localhost:54398/api/TeacherData/AddTeacher
	//with POST data of teacherid, teacherFname, teacherLname, etc.

	var URL = "http://localhost:54398/api/TeacherData/AddTeacher/";

	var rq = new XMLHttpRequest();
	// where is this request sent to?
	// is the method GET or POST?
	// what should we do with the response?

	var teacherid = document.getElementById("teacherid").value;
	var teacherFname = document.getElementById('teacherFname').value;
	var teacherLname = document.getElementById('teacherLname').value;
	var employeenumber = document.getElementById('employeenumber').value;
	var hiredate = document.getElementById('hiredate').value;
	var salary = document.getElementById('salary').value;



	var TeacherData = {
		"teacherid": teacherid,
		"teacherFname": teacherFname,
		"teacherLname": teacherLname,
		"employeenumber": employeenumber,
		"hiredate": hiredate,
		"salary": salary
	};


	rq.open("POST", URL, true);
	rq.setRequestHeader("Content-Type", "application/json");
	rq.onreadystatechange = function () {
		//ready state should be 4 AND status should be 200
		if (rq.readyState == 4 && rq.status == 200) {
			//request is successful and the request is finished

			//nothing to render, the method returns nothing.


		}

	}
	//POST information sent through the .send() method
	rq.send(JSON.stringify(TeacherData));

}
Footer