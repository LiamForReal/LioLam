function checkForm() {
    const cardNumber = document.getElementById('cardNumber').value.trim();
    const cardCVC = document.getElementById('cardCVC').value.trim();
    const cardDate = document.getElementById('cardDate').value.trim();
    const cardName = document.getElementById('cardName').value.trim();

    var nameError = document.getElementById('nameError');
    var numberError = document.getElementById('numberError');
    var cvcError = document.getElementById('cvcError');
    var dateError = document.getElementById('dateError');

    nameError.style.visibility = "hidden";
    numberError.style.visibility = "hidden";
    cvcError.style.visibility = "hidden";
    dateError.style.visibility = "hidden";

    let isFormValid = true;

    if (!isNumeric(cardNumber) || cardNumber.length < 12 || cardNumber.length > 19) {
        numberError.style.visibility = "visible";
        numberError.innerHTML = "card number must contains 12 - 19 numeric characters";
        isFormValid = false;
    }

    if (!isNumeric(cardCVC) || cardCVC.length !== 3) {
        cvcError.style.visibility = "visible";
        cvcError.innerHTML = "cvc must contains 3 numeric characters";
        isFormValid = false;
    }

    if (cardName == "") {
        nameError.style.visibility = "visible";
        nameError.innerHTML = "card name cant be empty"
        isFormValid = false;
    }

    if (!cardDate.match(/^(0[1-9]|1[0-2])\/\d{2}$/)) {
        dateError.style.visibility = "visible";
        dateError.innerHTML = "date not in the requaiyed format";
        isFormValid = false;
    }

    if (isFormValid == true) {
        document.forms[0].submit(); 
    }
}

function isNumeric(value) {
    return /^\d+$/.test(value);
}
