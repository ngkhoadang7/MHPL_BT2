function updatePrice() {
    var id = document.getElementById("tour_id").value;
    $.ajax({
        type:"POST",
        url: "/tourDoan/GetAllPricesByTourID",
        data: JSON.stringify({ id: id }),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var prices = JSON.parse(data);
            $("#gia_id").empty();
            for (var i = 0; i < prices.length; i++) {
                var option = new Option(prices[i].gia_sotien, prices[i].gia_id);
                $("#gia_id").append(option);
            }
        },
        error: function () {
            alert("Error occured!!");
        }  
    });
}
window.onload = function () {
    if(window.location.pathname == "/tourDoan/Create")
        updatePrice();
}
