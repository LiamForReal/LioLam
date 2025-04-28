var isFormValid;

function checkForm()
{
    isFormValid = true;
    checkId();
    checkUserName();
    checkPassword();
    checkEmail();
    checkPhone();
    //checkCity();
    //checkStreet();
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
    var password = document.getElementById("password").value;
    var lbl = document.getElementById("passwordError");
    lbl.style.visibility = "hidden";
    var isFormValid = true;

    // Check if password is empty
    if (password == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password can not be empty";
        isFormValid = false;
        return isFormValid;
    }

    // Check password length (at least 8 characters)
    if (password.length < 8) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must be at least 8 characters long";
        isFormValid = false;
        return isFormValid;
    }

    // Check for at least one uppercase letter
    var upperCasePattern = /[A-Z]/;
    if (!upperCasePattern.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one uppercase letter";
        isFormValid = false;
        return isFormValid;
    }

    // Check for at least one lowercase letter
    var lowerCasePattern = /[a-z]/;
    if (!lowerCasePattern.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one lowercase letter";
        isFormValid = false;
        return isFormValid;
    }

    // Check for at least one number
    var numberPattern = /[0-9]/;
    if (!numberPattern.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one number";
        isFormValid = false;
        return isFormValid;
    }

    // Check for at least one special character
    var specialCharPattern = /[!@#$%^&*(),.?":{}|<>]/;
    if (!specialCharPattern.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one special character";
        isFormValid = false;
        return isFormValid;
    }

    return isFormValid;
}

function checkHouse()
{
    var houseNumber = document.getElementById("houseNumber").value;
    var lbl = document.getElementById("houseNumberError");
    lbl.style.visibility = "hidden";
    if (houseNumber == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "House number can not be empty";
        isFormValid = false;
        return;
    }
    var regex = /^[1-9][0-9]?$/
    if (regex.test(houseNumber) == false) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "House number need to be number between 1 to 99";
        isFormValid = false;
    }

}
function checkPhone() {
    var phone = document.getElementById("phone").value;
    var lbl = document.getElementById("phoneError");
    lbl.style.visibility = "hidden";
    if (phone == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Phone can not be empty";
        isFormValid = false;
        return;
    }
    var regex = /^05[0-9]{8}$/
    if (regex.test(phone) == false) {
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
    if (regex.test(userName) == false) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Username is invalid";
        isFormValid = false;
    }
}

function checkEmail() {
    var email = document.getElementById("email");
    var lbl = document.getElementById("emailError");
    lbl.style.visibility = "hidden";
    if (email == "") {
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

    for (var option in options)
    {
        if (select.textContent == option.textContent) {
            flag = true;
            break;
        }
    }

    if (!flag) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Choose city from the list";
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

    for (var option in options)
    {
        if (select.textContent == option.textContent)
        {
            flag = true;
            break;
        }
    }

    if (!flag) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Choose street from the list";
        isFormValid = false;
    }
}