function updatePrice() {
    var id = document.getElementById("tour_id").value;
    $.ajax({
        type:"POST",
        url: "/tourGia/GetAllPricesByID",
        data: JSON.stringify({ id: id }),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var prices = JSON.parse(data);
            var table = document.getElementById("tablePrices");
            var temp = "";
            for (var i = 0; i < prices.length; i++) {
                temp += `<tr>
                    <td>
                        ${i+1}
                    </td>
                    <td>
                        ${numberWithCommas(prices[i].gia_sotien)} VND
                    </td>
                    <td>
                        ${getDateIfDate(prices[i].gia_tungay)}
                    </td>
                    <td>
                        ${getDateIfDate(prices[i].gia_denngay)}
                    </td>
                    <td>
                        <a href="/tourGia/Edit/${prices[i].gia_id}">Sửa</a>
                    </td>
                </tr>`
            }
            table.innerHTML = temp;
        },
        error: function () {
            alert("Error occured!!");
        }  
    });
}
function getDateIfDate(d) {
    var m = d.match(/\/Date\((\d+)\)\//);
    return m ? (new Date(+m[1])).toLocaleDateString('en-GB', { month: '2-digit', day: '2-digit', year: 'numeric' }) : d;
}
function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}
window.onload = function () {
    updatePrice();
}
