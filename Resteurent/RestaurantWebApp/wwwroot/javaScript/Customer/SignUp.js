var isFormValid;

function checkForm()
{
    isFormValid = true;
    checkId();
    checkUserName();
    checkPassword();
    checkEmail();
    checkPhone();
    checkCity();
    checkStreet();
    checkHouse();

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

function checkPassword() {
    var id = document.getElementById("password").value;
    var lbl = document.getElementById("passwordError");
    lbl.style.visibility = "hidden";
    if (id == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password can not be empty";
        isFormValid = false;
        return;
    }
}

function checkHouse()
{
    var id = document.getElementById("houseNumber").value;
    var lbl = document.getElementById("houseNumberError");
    lbl.style.visibility = "hidden";
    if (id == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "House number can not be empty";
        isFormValid = false;
        return;
    }
    var regex = /^[1-9][0-9]?$/
    if (regex.test(id) == false) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "House number need to be number between 1 to 99";
        isFormValid = false;
    }

}
function checkPhone() {
    var id = document.getElementById("phone").value;
    var lbl = document.getElementById("phoneError");
    lbl.style.visibility = "hidden";
    if (id == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Phone can not be empty";
        isFormValid = false;
        return;
    }
    var regex = /^0(5[1-9])[0-9]{7}$/
    if (regex.test(id) == false) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Phone is invalid";
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
    var regex = /^[A-Za-z][A-Za-z0-9]*$/
    if (regex.test(id) == false) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Username is invalid";
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
    var select = document.getElementById("cities");
    var lbl = document.getElementById("cityError");
    var options = select.getElementsByTagName("option");
    var flag = false;
    lbl.style.visibility = "hidden";

    if (select.textContent == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Street can not be empty";
        isFormValid = false;
        return;
    }

    for (var i = 0; i < options.length; i++) {
        if (select.textContent == options[i].value) {
            flag = true;
        }
    }

    if (!flag) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Choose street from the list";
        isFormValid = false;
    }
}

function checkStreet() {
    var select = document.getElementById("streets");
    var lbl = document.getElementById("streetError");
    var options = select.getElementsByTagName("option");
    var flag = false;
    lbl.style.visibility = "hidden";
    
    if (select.textContent == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Street can not be empty";
        isFormValid = false;
        return;
    }

    for (var i = 0; i < options.length; i++) {
        if (select.textContent == options[i].value) {
            flag = true;
        }
    }

    if (!flag) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Choose street from the list";
        isFormValid = false;
    }
}
