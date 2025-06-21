let currentChefId = null;
let currentTypeId = null;

$(document).ready(function () {
    $(document).on("click", ".link", function () {
        let uri = "https://localhost:7287/Guest/GetDishList/?";

        if (this.hasAttribute("data-chefId")) {
            currentTypeId = null;
            currentChefId = this.getAttribute("data-chefId");
            uri += "chefId=" + currentChefId + "&";

        }
        if (this.hasAttribute("data-typeId")) {
            currentChefId = null;
            currentTypeId = this.getAttribute("data-typeId");
            uri += "typeId=" + currentTypeId + "&";
        }
        if (this.hasAttribute("data-pageNumber")) {

            if (currentChefId != null) 
                uri += "chefId=" + currentChefId + "&";
            if (currentTypeId != null)
                uri += "typeId=" + currentTypeId + "&";

            uri += "pageNumber=" + this.getAttribute("data-pageNumber");;
        }

        // Optional: remove trailing '&' if needed
        uri = uri.replace(/[&?]$/, '');

        $.ajax({
            url: uri,
            method: "GET",
            dataType: "html",
            beforeSend: function () {
                let loader = "<div class=loader><img src='../../Images/loader.gif'/></div>";
                $("#dishes").html(loader);
            },
            success: function (data) {
                setTimeout(function () {
                    const parser = new DOMParser();
                    const doc = parser.parseFromString(data, 'text/html');

                    const dishesData = doc.querySelector("#dishes").innerHTML;
                    const paginationData = doc.querySelector("#pagination").innerHTML;

                    $("#dishes").html(dishesData);
                    $("#pagination").html(paginationData);
                });
            },
            error: function () {
                console.error("Failed to fetch dishes.");
            }
        });
    });
});
