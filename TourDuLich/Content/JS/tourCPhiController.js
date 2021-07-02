function addCost() {
    var loaiChiPhi = document.getElementById("loaiChiPhiOption");
    var loaiChiPhiText = loaiChiPhi.options[loaiChiPhi.selectedIndex].text;

    var hoaDon = document.getElementById("hoaDon");
    if (hoaDon.value == "") {
        alert("Nhập hóa đơn");
        hoaDon.focus();
        return;
    }

    var noiDung = document.getElementById("noiDung");
    if (noiDung.value == "") {
        alert("Nhập nội dung hóa đơn");
        noiDung.focus();
        return;
    }

    var ngay = document.getElementById("ngay");
    if (ngay.value == "") {
        alert("Chọn ngày thanh toán");
        ngay.focus();
        return;
    }

    var soTien = document.getElementById("soTien");
    if (soTien.value == "") {
        alert("Nhập số tiền");
        soTien.focus();
        return;
    }

    document.getElementById("costDetailTable").innerHTML += `<tr>
        <td>
            ${hoaDon.value}
            <input type="hidden" value="${hoaDon.value}" name="hoaDon"/>
        </td>
        <td>
            ${loaiChiPhiText}
            <input type="hidden" value="${loaiChiPhi.value}" name="loaiChiPhi"/>
        </td>
        <td>
            ${noiDung.value}
            <input type="hidden" value="${noiDung.value}" name="noiDung"/>
        </td>
        <td>
            ${formatDate(ngay.value)}
            <input type="hidden" value="${ngay.value}" name="ngay"/>
        </td>
        <td>
            ${numberWithCommas(soTien.value)} VND
            <input type="hidden" value="${soTien.value}" name="soTien"/>
        </td>
        <th><a href="#" onclick="removeCost(this)">Xóa</a></th>
    </tr>`;

    hoaDon.value = "";
    noiDung.value = "";
    ngay.value = "";
    soTien.value = "";

    calculateTotal();
}

function removeCost(thisElement) {
    $(thisElement).closest("tr").remove();
    calculateTotal()
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}

function formatDate(date) {
    var today = new Date(date);
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    return dd + '/' + mm + '/' + yyyy;
}

function calculateTotal() {
    var costs = document.getElementsByName("soTien");
    var total = 0;
    for (var i = 0; i < costs.length; i++) {
        total += parseFloat(costs[i].value);
    }
    document.getElementById("totalShow").innerHTML = numberWithCommas(total);
    document.getElementById("chiphi_total").value = total;
}