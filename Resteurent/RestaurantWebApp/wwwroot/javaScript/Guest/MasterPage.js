function HomePage() {
    let content = `
        <img src="~/images/Icon.png" alt="restaurantIcon" class="restaurantMainImg" />


        <p class="restaurantTitle">LioLam Dining</p>
        <p class="restaurantTagline">Get ready for something tasty — your next meal starts here.</p>


        <div class="restaurantFeatures">
            <div class="featureChunk">
                <h3>Elegant Atmosphere</h3>
                <p>A perfect blend of style and comfort — dine in luxury every visit.</p>
            </div>
            <div class="featureChunk">
                <h3>Award-Winning Menu</h3>
                <p>Savor dishes crafted by master chefs with fresh, quality ingredients.</p>
            </div>
            <div class="featureChunk">
                <h3>Delivery & Dine-In</h3>
                <p>Your cravings delivered or served — experience LioLam your way.</p>
            </div>
        </div>
    `;

    let body = document.querySelector('.body');

    // Check if the content is already present
    if (body.innerHTML.trim() !== content.trim()) {
        body.innerHTML = content; // Update the content only if different
    }
}
