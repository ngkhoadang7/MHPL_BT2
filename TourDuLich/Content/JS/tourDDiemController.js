function updateLocation() {
    var city = document.getElementById("thanhPho").value;
    $.ajax({
        type:"POST",
        url: "/tourDDiem/GetAllLocationsByCity",
        data: JSON.stringify({ city: city }),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var location = JSON.parse(data);
            var table = document.getElementById("tableLocation");
            var temp = "";
            for (var i = 0; i < location.length; i++) {
                temp += `<tr>
                    <td>
                        ${i+1}
                    </td>
                    <td>
                        ${location[i].dd_ten}
                    </td>
                    <td>
                        ${location[i].dd_mota}
                    </td>
                    <td>
                        <a href="/tourDDiem/Edit/${location[i].dd_id}">Sửa</a>
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
window.onload = function () {
    updateLocation();
}
