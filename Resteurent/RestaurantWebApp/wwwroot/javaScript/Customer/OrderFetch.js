function updateQuantity(dishId, quantity)
{
    fetch('/Customer/UpdateDishQuantity',
    {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({ dishId: dishId, quantity: quantity })
    })
        .then(response => {
            if (!response.ok) throw new Error("Failed to update");
        })
        .catch(error => console.error(error));
}