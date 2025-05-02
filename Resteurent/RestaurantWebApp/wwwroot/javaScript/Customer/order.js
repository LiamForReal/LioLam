function updateQuantity(dishId, newQuantity) {
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch('https://localhost:7287/customer/UpdateDishInOrder', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({
            DishId: dishId,
            Quantity: parseInt(newQuantity)
        })
    })
        .then(response => {
            if (!response.ok) throw new Error("Server error");
            return response.json();
        })
        .then(data => {
            // Update price display for this dish
            const priceSpan = document.getElementById(`price-${dishId}`);
            if (priceSpan) {
                priceSpan.textContent = "$" + data.newTotalPrice;
            }
        })
        .catch(error => {
            console.error('Error updating quantity:', error);
            alert("There was a problem updating your order. Please try again.");
        });
}
