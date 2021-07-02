/*window.onload = function () {
    alert(test);
};*/

function removeElement(array, elem) {
    var index = array.indexOf(elem);
    if (index > -1) {
        array.splice(index, 1);
    }
}

function getDateIfDate(d) {
    var m = d.match(/\/Date\((\d+)\)\//);
    return m ? (new Date(+m[1])).toLocaleDateString('en-GB', { month: '2-digit', day: '2-digit', year: 'numeric' }) : d;
}

/*============== Staff =============================================*/
if (typeof id_staffs  === 'undefined') {
    var rootStaffs = new Array();
} else {
    var rootStaffs = id_staffs.slice();
}

var tempStaffs;

function toggleStaff(checkbox, id) {
    if (checkbox.checked == true) {
        tempStaffs.push(id);
    } else {
        removeElement(tempStaffs, id);
    }
}

function showStaffPopup() {
    $("#staffPopup").modal('show');
    tempStaffs = rootStaffs.slice();
    showStaff(1, 10);
}

function showStaff(currentPage, itemsInPage) {
    staffPagination(currentPage, itemsInPage);
    $.ajax({
        type: "POST",
        url: "/tourNDi/getStaffPagination",
        data: JSON.stringify({ currentPage: currentPage, itemsInPage: itemsInPage }),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var staff = JSON.parse(data);

            var tmp = "";
            for (var i = 0; i < staff.length; i++) {
                tmp += `<tr>
                            <th scope="row">${staff[i].nv_id}</th>
                            <td>${staff[i].nv_ten}</td>
                            <td>${getDateIfDate(staff[i].nv_ngaysinh)}</td>
                            <td>${staff[i].nv_nhiemvu}</td>
                            <td>
                                <input type="checkbox" onchange="toggleStaff(this,${staff[i].nv_id})" ${(tempStaffs.includes(staff[i].nv_id))?"checked":""} />
                            </td>
                        </tr>`;
            }
            document.getElementById("staffTableModal").innerHTML = tmp;
        },
        error: function () {
            alert("Error occured!!");
        }
    });
}

function staffPagination(currentPage, itemsInPage) {
    $.ajax({
        type: "POST",
        url: "/tourNDi/countAllStaff",
        data: JSON.stringify({ currentPage: currentPage, itemsInPage: itemsInPage }),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var allItems = JSON.parse(data);
            var tmp = "";
            var numOfPage = Math.ceil(allItems / itemsInPage);

            if (currentPage == 1) {
                tmp += `<li class="page-item disabled"><a class="page-link" href="#">Trước</a></li>`;
            } else {
                tmp += `<li class="page-item"><a class="page-link" href="#" onclick="showStaff(${currentPage - 1}, ${itemsInPage})">Trước</a></li>`;
            }

            for (var i = 1; i <= numOfPage; i++) {
                if (i == currentPage) {
                    tmp += `<li class="page-item active"><a class="page-link" href="#">${i}</a></li>`;
                } else {
                    tmp += `<li class="page-item"><a class="page-link" href="#" onclick="showStaff(${i}, ${itemsInPage})">${i}</a></li>`;
                }
            }

            if (currentPage == numOfPage) {
                tmp += `<li class="page-item disabled"><a class="page-link" href="#">Sau</a></li>`;
            } else {
                tmp += `<li class="page-item"><a class="page-link" href="#" onclick="showStaff(${currentPage + 1}, ${itemsInPage})">Sau</a></li>`;
            }


            document.getElementById("paginationStaff").innerHTML = tmp;
        },
        error: function () {
            alert("Error occured!!");
        }
    });
}

function acceptStaffModal() {
    rootStaffs = tempStaffs.slice();
    $.ajax({
        type: "POST",
        url: "/tourNDi/getStaffsByIds",
        data: JSON.stringify({ rootStaffs: rootStaffs}),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var staffs = JSON.parse(data);

            var tmp = "";
            for (var i = 0; i < staffs.length; i++) {
                tmp += `<tr>
                            <th scope="row">
                                ${staffs[i].nv_id}
                                <input type="hidden" value="${staffs[i].nv_id}" name="nv_id" />
                            </th>
                            <td>${staffs[i].nv_ten}</td>
                            <td>${getDateIfDate(staffs[i].nv_ngaysinh)}</td>
                            <td>${staffs[i].nv_nhiemvu}</td>
                        </tr>`;
            }
            document.getElementById("staffTable").innerHTML = tmp;
            $("#staffPopup").modal('hide');
        },
        error: function () {
            alert("Error occured!!");
        }
    });
}
/*=====================================================================*/

/*============== Customer =============================================*/
if (typeof id_customers === 'undefined') {
    var rootCustomers = new Array();
} else {
    var rootCustomers = id_customers.slice();
}

var tempCustomers;

function toggleCustomer(checkbox, id) {
    if (checkbox.checked == true) {
        tempCustomers.push(id);
    } else {
        removeElement(tempCustomers, id);
    }
}

function showCustomerPopup() {
    $("#costumerPopup").modal('show');
    tempCustomers = rootCustomers.slice();
    showCustomer(1, 10);
}


function showCustomer(currentPage, itemsInPage) {
    customerPagination(currentPage, itemsInPage);
    $.ajax({
        type: "POST",
        url: "/tourNDi/getCustomerPagination",
        data: JSON.stringify({ currentPage: currentPage, itemsInPage: itemsInPage }),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var customer = JSON.parse(data);
            var tmp = "";
            for (var i = 0; i < customer.length; i++) {
                tmp += `<tr>
                            <th scope="row">${customer[i].kh_id}</th>
                            <td>${customer[i].kh_ten}</td>
                            <td>${getDateIfDate(customer[i].kh_ngaysinh)}</td>
                            <td>${customer[i].kh_cmnd}</td>
                            <td>
                                <input type="checkbox" onchange="toggleCustomer(this,${customer[i].kh_id})" ${(tempCustomers.includes(customer[i].kh_id)) ? "checked" : ""}/>
                            </td>
                        </tr>`;
            }
            document.getElementById("customerTableModal").innerHTML = tmp;
        },
        error: function () {
            alert("Error occured!!");
        }
    });
}

function customerPagination(currentPage, itemsInPage) {
    $.ajax({
        type: "POST",
        url: "/tourNDi/countAllCustomer",
        data: JSON.stringify({ currentPage: currentPage, itemsInPage: itemsInPage }),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var allItems = JSON.parse(data);
            var tmp = "";
            var numOfPage = Math.ceil(allItems / itemsInPage);

            if (currentPage == 1) {
                tmp += `<li class="page-item disabled"><a class="page-link" href="#">Trước</a></li>`;
            } else {
                tmp += `<li class="page-item"><a class="page-link" href="#" onclick="showCustomer(${currentPage - 1}, ${itemsInPage})">Trước</a></li>`;
            }

            for (var i = 1; i <= numOfPage; i++) {
                if (i == currentPage) {
                    tmp += `<li class="page-item active"><a class="page-link" href="#">${i}</a></li>`;
                } else {
                    tmp += `<li class="page-item"><a class="page-link" href="#" onclick="showCustomer(${i}, ${itemsInPage})">${i}</a></li>`;
                }
            }

            if (currentPage == numOfPage) {
                tmp += `<li class="page-item disabled"><a class="page-link" href="#">Sau</a></li>`;
            } else {
                tmp += `<li class="page-item"><a class="page-link" href="#" onclick="showCustomer(${currentPage + 1}, ${itemsInPage})">Sau</a></li>`;
            }

            document.getElementById("paginationCustomer").innerHTML = tmp;
        },
        error: function () {
            alert("Error occured!!");
        }
    });
}

function acceptCustomerModal() {
    rootCustomers = tempCustomers.slice();
    $.ajax({
        type: "POST",
        url: "/tourNDi/getCustomersByIds",
        data: JSON.stringify({ rootCustomers: rootCustomers }),
        contentType: 'application/json; charset=utf-8',
        datatype: "json",
        success: function (data) {
            var customer = JSON.parse(data);

            var tmp = "";
            for (var i = 0; i < customer.length; i++) {
                tmp += `<tr>
                            <th scope="row">
                                ${customer[i].kh_id}
                                <input type="hidden" value="${customer[i].kh_id}" name="kh_id" />
                            </th>
                            <td>${customer[i].kh_ten}</td>
                            <td>${getDateIfDate(customer[i].kh_ngaysinh)}</td>
                            <td>${customer[i].kh_cmnd}</td>
                        </tr>`;
            }
            document.getElementById("customerTable").innerHTML = tmp;
            $("#costumerPopup").modal('hide');
        },
        error: function () {
            alert("Error occured!!");
        }
    });
}
/*===========================================================*/

