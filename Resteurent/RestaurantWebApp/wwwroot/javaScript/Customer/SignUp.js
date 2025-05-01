var isFormValid;

function checkForm()
{
    isFormValid = true;
    checkId();
    checkUserName();
    checkPassword();
    checkEmail();
    checkPhone();

    if (isFormValid == false) {
        event.preventDefault();
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
    var formAction = document.querySelector("form").action
    lbl.style.visibility = "hidden";

    if (formAction.includes("EditAccount") && password == "")
        return;

    // Check if password is empty
    if (password == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password can not be empty";
        isFormValid = false;
        return;
    }

    // Check password length (at least 8 characters)
    if (password.length < 8) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must be at least 8 characters long";
        isFormValid = false;
        return;
    }

    // Check for at least one uppercase letter
    var upperCasePattern = /[A-Z]/;
    if (!upperCasePattern.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one uppercase letter";
        isFormValid = false;
        return;
    }

    // Check for at least one lowercase letter
    var lowerCasePattern = /[a-z]/;
    if (!lowerCasePattern.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one lowercase letter";
        isFormValid = false;
        return;
    }

    // Check for at least one number
    var numberPattern = /[0-9]/;
    if (!numberPattern.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one number";
        isFormValid = false;
        return;
    }

    // Check for at least one special character
    var specialCharPattern = /[!@#$%^&*(),.?":{}|<>]/;
    if (!specialCharPattern.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one special character";
        isFormValid = false;
        return;
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
    var regex = /^[A-Za-z][A-Za-z0-9_.-]*$/
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