function updateLocation() {
    var city = document.getElementById("dd_thanhpho").value;
    $.ajax({
        type:"POST",
        url: "/tours/GetAllLocationByCity",
        data: JSON.stringify({ city: city }),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var locations = JSON.parse(data);
            $("#dd_diadiem").empty();
            for (var i = 0; i < locations.length; i++) {
                var option = new Option(locations[i].dd_ten, locations[i].dd_id);
                $("#dd_diadiem").append(option);
            }
        },
        error: function () {
            alert("Error occured!!");
        }  
    });
}
function addLocation() {
    var locationID = $("#dd_diadiem").val();
    var error = false;
    $("input[name='dd_id']").each(function () {
        if (this.value == locationID) {
            alert("Địa điểm đã được thêm");
            error = true;
            return;
        }
    });
    if (error) return;
    $.ajax({
        type: "POST",
        url: "/tours/GetLocationById",
        data: JSON.stringify({ id: locationID }),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var location = JSON.parse(data);
            var currentNumberofRows = $("#tableLocation >tr").length;
            var tmp = `<tr>
                        <th scope="row">
                            ${currentNumberofRows + 1}
                            <input class="form-control" id="dd_id" name="dd_id" type="hidden" value="${location[0].dd_id}">
                            <input class="form-control" id="dd_ten" name="dd_ten" type="hidden" value="${location[0].dd_ten}">
                            <input class="form-control" id="ct_thutu" name="ct_thutu" type="hidden" value="${currentNumberofRows + 1}">
                        </th>
                        <td>
                            ${location[0].dd_ten}
                        </td>
                        <td>
                            <span style="cursor: pointer" onclick="delLocation(this)"><a>Xóa</a></span>
                            <span style="cursor: pointer" onclick="increaseOrder(this)"><a>Tăng</a></span>
                            <span style="cursor: pointer" onclick="decreaseOrder(this)"><a>Giảm</a></span>
                        </td>
                    </tr>`;
            $("#tableLocation").append(tmp);
        },
        error: function () {
            alert("Error occured!!");
        }
    });
}
function delLocation(thisElement) {
    $(thisElement).closest("tr").remove();
    var dd_id = document.getElementsByName("dd_id");
    var dd_ten = document.getElementsByName("dd_ten");
    var tmp = "";
    for (var i = 0; i < dd_id.length; i++) {
        tmp += `<tr>
            <th scope="row">
                ${i + 1}
                <input class="form-control" id="dd_id" name="dd_id" type="hidden" value="${dd_id[i].value}">
                <input class="form-control" id="dd_ten" name="dd_ten" type="hidden" value="${dd_ten[i].value}">
                <input class="form-control" id="ct_thutu" name="ct_thutu" type="hidden" value="${i + 1}">
            </th>
            <td>
                ${dd_ten[i].value}
            </td>
            <td>
                <span style="cursor: pointer" onclick="delLocation(this)"><a>Xóa</a></span>
                <span style="cursor: pointer" onclick="increaseOrder(this)"><a>Tăng</a></span>
                <span style="cursor: pointer" onclick="decreaseOrder(this)"><a>Giảm</a></span>
            </td>
        </tr>`;
    }
    $("#tableLocation").empty();
    $("#tableLocation").append(tmp);
}
function increaseOrder(thisElement) {
    var currentIndex = $(thisElement).closest("tr").find("input[name='ct_thutu']").val() - 1;
    if (currentIndex == 0) return;

    var dd_id = document.getElementsByName("dd_id");
    var dd_ten = document.getElementsByName("dd_ten");

    var temp_id = dd_id[currentIndex - 1].value;
    dd_id[currentIndex - 1].value = dd_id[currentIndex].value;
    dd_id[currentIndex].value = temp_id;

    var temp_ten = dd_ten[currentIndex - 1].value;
    dd_ten[currentIndex - 1].value = dd_ten[currentIndex].value;
    dd_ten[currentIndex].value = temp_ten;

    var tmp = "";
    for (var i = 0; i < dd_id.length; i++) {
        tmp += `<tr>
            <th scope="row">
                ${i + 1}
                <input class="form-control" id="dd_id" name="dd_id" type="hidden" value="${dd_id[i].value}">
                <input class="form-control" id="dd_ten" name="dd_ten" type="hidden" value="${dd_ten[i].value}">
                <input class="form-control" id="ct_thutu" name="ct_thutu" type="hidden" value="${i + 1}">
            </th>
            <td>
                ${dd_ten[i].value}
            </td>
            <td>
                <span style="cursor: pointer" onclick="delLocation(this)"><a>Xóa</a></span>
                <span style="cursor: pointer" onclick="increaseOrder(this)"><a>Tăng</a></span>
                <span style="cursor: pointer" onclick="decreaseOrder(this)"><a>Giảm</a></span>
            </td>
        </tr>`;
    }
    $("#tableLocation").empty();
    $("#tableLocation").append(tmp);
}
function decreaseOrder(thisElement) {
    var currentIndex = $(thisElement).closest("tr").find("input[name='ct_thutu']").val() - 1;
    var currentNumberofRows = $("#tableLocation >tr").length;
    if (currentIndex == currentNumberofRows) return;

    var dd_id = document.getElementsByName("dd_id");
    var dd_ten = document.getElementsByName("dd_ten");

    var temp_id = dd_id[currentIndex].value;
    dd_id[currentIndex].value = dd_id[currentIndex + 1].value;
    dd_id[currentIndex + 1].value = temp_id;

    var temp_ten = dd_ten[currentIndex].value;
    dd_ten[currentIndex].value = dd_ten[currentIndex + 1].value;
    dd_ten[currentIndex + 1].value = temp_ten;

    var tmp = "";
    for (var i = 0; i < dd_id.length; i++) {
        tmp += `<tr>
            <th scope="row">
                ${i + 1}
                <input class="form-control" id="dd_id" name="dd_id" type="hidden" value="${dd_id[i].value}">
                <input class="form-control" id="dd_ten" name="dd_ten" type="hidden" value="${dd_ten[i].value}">
                <input class="form-control" id="ct_thutu" name="ct_thutu" type="hidden" value="${i + 1}">
            </th>
            <td>
                ${dd_ten[i].value}
            </td>
            <td>
                <span style="cursor: pointer" onclick="delLocation(this)"><a>Xóa</a></span>
                <span style="cursor: pointer" onclick="increaseOrder(this)"><a>Tăng</a></span>
                <span style="cursor: pointer" onclick="decreaseOrder(this)"><a>Giảm</a></span>
            </td>
        </tr>`;
    }
    $("#tableLocation").empty();
    $("#tableLocation").append(tmp);
}
window.onload = function () {
    updateLocation();
}
