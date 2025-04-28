function updateQuantity(dishId, newQuantity) {
    const token = document.getElementById('antiForgeryToken').value;

    fetch('https://localhost:7287/customer/UpdateDishQuantity', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token // important!
        },
        body: JSON.stringify({ DishId: dishId, Quantity: newQuantity })
    })
    .then(response => {
        console.log(response);
        return response.json();
    })
    .then(data => {
        console.log(data);
        document.getElementById('total-price').textContent = "$" + data.newTotalPrice;
    })
    .catch(error => {
        console.error('Error:', error);
    });
}