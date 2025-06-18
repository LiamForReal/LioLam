function checkForm(event) {
    var isFormValid = true;

    if (!checkId()) isFormValid = false;
    if (!checkUserName()) isFormValid = false;
    if (!checkPassword()) isFormValid = false;
    if (!checkEmail()) isFormValid = false;
    if (!checkPhone()) isFormValid = false;

    if (!isFormValid) {
        if (event) event.preventDefault();
        return false; 
    }

    return true; // Allow submission
}

function checkId() {
    var id = document.getElementById("userId").value;
    var lbl = document.getElementById("idError");
    lbl.style.visibility = "hidden";

    if (id == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Id can not be empty";
        return false;
    }

    var regex = /^[0-9]{9}$/;
    if (!regex.test(id)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Id is invalid";
        return false;
    }

    return true;
}

function checkPassword() {
    var password = document.getElementById("password").value;
    var lbl = document.getElementById("passwordError");
    var formAction = document.querySelector("form").action;
    lbl.style.visibility = "hidden";

    if (formAction.includes("EditAccount") && password == "")
        return true; // Password can be empty when editing

    if (password == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password can not be empty";
        return false;
    }

    if (password.length < 8) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must be at least 8 characters long";
        return false;
    }

    if (!/[A-Z]/.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one uppercase letter";
        return false;
    }

    if (!/[a-z]/.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one lowercase letter";
        return false;
    }

    if (!/[0-9]/.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one number";
        return false;
    }

    if (!/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Password must contain at least one special character";
        return false;
    }

    return true;
}

function checkPhone() {
    var phone = document.getElementById("phone").value;
    var lbl = document.getElementById("phoneError");
    lbl.style.visibility = "hidden";

    if (phone == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Phone can not be empty";
        return false;
    }

    var regex = /^05[0-9]{8}$/;
    if (!regex.test(phone)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Phone is invalid";
        return false;
    }

    return true;
}

function checkUserName() {
    var userName = document.getElementById("userName").value;
    var lbl = document.getElementById("userNameError");
    lbl.style.visibility = "hidden";

    if (userName == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "User Name can not be empty";
        return false;
    }

    var regex = /^[A-Za-z][A-Za-z0-9_.-]*$/;
    if (!regex.test(userName)) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Username is invalid";
        return false;
    }

    return true;
}

function checkEmail() {
    var email = document.getElementById("email");
    var lbl = document.getElementById("emailError");
    lbl.style.visibility = "hidden";

    if (email.value == "") {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Email can not be empty";
        return false;
    }

    if (!email.checkValidity()) {
        lbl.style.visibility = "visible";
        lbl.innerHTML = "Email is invalid";
        return false;
    }

    return true;
}
