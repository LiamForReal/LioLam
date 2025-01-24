function HomePage() {
    let content = `
        <p class="bodyTitle">LioLam Restaurant<br />Fresh & Tasty</p>
        <p class="bodyDescription">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis vitae<br />tincidunt neque. Fusce sodales accumsan interdum. Phasellus velit enim, maximus</p>
        <img src="../images/Icon.png" alt="restaurantIcon" class="bodyImg">
        <div class="bodyExamples">
            <i class="fas fa-leaf bodyIcon"></i>
            <p class="bodyExamplesTitle">Quality Ingredients</p>
            <p class="bodyExampleDescription">Lorem ipsum dolor sit amet<br />consectetur adipiscing elit.</p>
        </div>
        <div class="bodyExamples">
            <i class="fas fa-carrot bodyIcon"></i>
            <p class="bodyExamplesTitle">Fresh Food</p>
            <p class="bodyExampleDescription">Lorem ipsum dolor sit amet<br />consectetur adipiscing elit.</p>
        </div>
        <div class="bodyExamples">
            <i class="fas fa-carrot bodyIcon"></i>
            <p class="bodyExamplesTitle">Master Chefs</p>
            <p class="bodyExampleDescription">Lorem ipsum dolor sit amet<br />consectetur adipiscing elit.</p>
        </div>
    `;

    let body = document.querySelector('.body');

    // Check if the content is already present
    if (body.innerHTML.trim() !== content.trim()) {
        body.innerHTML = content; // Update the content only if different
    }
}
