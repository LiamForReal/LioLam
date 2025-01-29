var isFormValid;

function checkForm() {
    isFormValid = true;
    checkId();
    checkUserName();
    checkEmail();
    checkCity();
    chackProgramerKind();
    checkFreeDays();
    checkMoreInfo();

    if (isFormValid == true) {
        document.forms[0].submit();
    }
}

function checkId() {
    var id = document.getElementById("userId").value;
    var lbl = document.getElementById("idError");
    lbl.style.visibility = "hidden";
    if (id == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Id can not be empty";
        isFormValid = false;
        return;
    }
    var regex = /^[0-9]{9}/
    if (regex.test(id) == false) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Id is invalid";
        isFormValid = false;
    }


}

function checkUserName() {
    var userName = document.getElementById("userName").value;
    var lbl = document.getElementById("userNameError");
    lbl.style.visibility = "hidden";
    if (userName == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "User Name can not be empty";
        isFormValid = false;
        return;
    }
    var regex = /^[A-Z]{1}[a-z]{1,20}/
    if (regex.test(userName) == false) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "User name is invalid";
        isFormValid = false;
    }
}

function checkEmail() {
    var email = document.getElementById("email");
    var lbl = document.getElementById("emailError");
    lbl.style.visibility = "hidden";
    if (email.value == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Email can not be empty";
        isFormValid = false;
        return;
    }

    if (!email.checkValidity()) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Email is invalid";
        isFormValid = false;
    }
}

function checkCity() {
    var input = document.getElementById("cityInput").value;
    var datalist = document.getElementById("cities");
    var lbl = document.getElementById("cityError");
    var options = datalist.getElementsByTagName("option");
    var flag = false;
    lbl.style.visibility = "hidden";

    if (input == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "City can not be empty";
        isFormValid = false;
        return;
    }

    for (var i = 0; i < options.length; i++) {
        if (input === options[i].value) {
            flag = true;
        }
    }

    if (!flag) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Choose city from the list";
        isFormValid = false;
    }

}

function chackProgramerKind() {
    var gender = document.getElementsByName("programmerkind");
    var lbl = document.getElementById("programmerKindError");
    lbl.style.visibility = "hidden";
    for (var i = 0; i < gender.length; i++) {
        if (gender[i].checked) {
            return;
        }
    }
    lbl.style.visibility = "visible";
    lbl.innerHTML = "You must choose one option";
    isFormValid = false;
}

function checkFreeDays() {
    var freeDays = document.getElementsByName("freeDays");
    var lbl = document.getElementById("freeDaysError");
    var counter = 0;
    lbl.style.visibility = "hidden"

    for (var i = 0; i < freeDays.length; i++) {
        if (freeDays[i].checked)
            counter++;
    }

    if (counter < 2) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "You must choose at list two";
        isFormValid = false;
    }

}

function checkMoreInfo() {
    var moreInfo = document.getElementById("moreInfo").value;
    var lbl = document.getElementById("moreInfoError");
    var max = 500;
    var min = 50;
    lbl.style.visibility = "hidden";

    if (moreInfo == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "This input can not be empty";
        isFormValid = false;
        return;
    }

    if (moreInfo.length > max || moreInfo.length < min) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Write between " + min + " - " + max + " characters";
        isFormValid = false;
    }
}


