$("document").ready(
    function () {
        $(".link").click(
            function () {
                let uri = "http://localhost:7287/Guest/GetDishList/?"
                if (this.hasAttribute("data-chefId")) {
                    let ext = "chefId=" + this.getAttribute("data-chefId");
                    uri = uri + ext;
                }
                if (this.hasAttribute("data-typeId")) {
                    let ext = "typeId=" + this.getAttribute("data-typeId");
                    uri = uri + ext;
                }
                if (this.hasAttribute("data-pageNumber")) {
                    let ext = "pageNumber=" + this.getAttribute("data-pageNumber");
                    uri = uri + ext;
                }
                $.ajax({
                    url: uri,
                    method: "GET",
                    dataType: "html",
                    beforeSend: function () {
                        let loader = "<div class=loader>" +
                            "<img src='..\..\Images\loader.png'/>" +
                            "</div>";
                        $("#body").html(loader);
                    },
                    error: function () {
                        // your code here;
                    },
                    success: function (data) {
                        setTimeout(function () {
                            
                            var dishData = $("#dishesData", data);
                            var paginationData = $("#paginationData", data);
                            $("#dishes").html(dishData); //dishes container id 
                            $("#pagination").html(paginationData);

                        }, 3000); //only to simulate real time getting

                    },
                    complete: function () {
                        // your code here;
                    }
                });
            }
        );
    });