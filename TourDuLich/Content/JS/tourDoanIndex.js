function updateGroup() {
    var id = document.getElementById("tour_id").value;
    $.ajax({
        type:"POST",
        url: "/tourDoan/GetAllGroupByTourID",
        data: JSON.stringify({ id: id }),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var group = JSON.parse(data);
            var tmp = "";
            for (var i = 0; i < group.length; i++) {
                tmp += `<tr>
                    <td>
                        ${group[i].tour_doan.doan_name}
                    </td>
                    <td>
                        ${getDateIfDate(group[i].tour_doan.doan_ngaydi)}
                    </td>
                    <td>
                        ${getDateIfDate(group[i].tour_doan.doan_ngayve)}
                    </td>
                    <td>
                        ${numberWithCommas(group[i].tour_gia.gia_sotien)} VND
                    </td>
                    <td>
                        <a href="/tourDoan/Edit/${group[i].tour_doan.doan_id}">Sửa</a> |
                        <a href="/tourDoan/Details/${group[i].tour_doan.doan_id}">Chi tiết</a>
                    </td>
                </tr>`; 
            }
            document.getElementById("tableGroup").innerHTML = tmp;
        },
        error: function () {
            alert("Error occured!!");
        }  
    });
}
window.onload = function () {
    updateGroup();
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}

function getDateIfDate(d) {
    var m = d.match(/\/Date\((\d+)\)\//);
    return m ? (new Date(+m[1])).toLocaleDateString('en-GB', { month: '2-digit', day: '2-digit', year: 'numeric' }) : d;
}