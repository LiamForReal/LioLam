async function updateQuantity(dishId) {
    try {
        const quantityInput = document.getElementById("dish-quantity_" + dishId);
        const SearchQuantity = quantityInput?.value ?? '';

        const data = {
            dishId: dishId,
            Quantity: SearchQuantity
        };

        const response = await fetch('https://localhost:7287/customer/UpdateDishInOrder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error("Server error");
        }

        const result = await response.json();

        // Update price display for this dish
        const priceSpan = document.getElementById(`total_price_${dishId}`);
        if (priceSpan) {
            priceSpan.textContent = result.totalPrice;
        }

        let sum = 0;
        document.querySelectorAll(".dish-price").forEach(el => {
            sum += parseFloat(el.textContent) || 0;
        });
        console.log("Total sum:", sum);

        const sumx = document.getElementById(`sum-box`);
        if (sumx) {
            sumx.textContent = "Sum: $" + sum;
        }

        console.log("Total sum:", sum);

    } catch (error) {
        console.error('Error updating quantity:', error);
        alert("There was a problem updating your order. Please try again.");
    }
}
