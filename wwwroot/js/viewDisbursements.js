
function tableToJson(table) {
    console.log("converting table to JSON");
    var data = [];

    // first row needs to be headers
    var headers = [];
    for (var i = 0; i < table.rows[0].cells.length; i++) {
        headers[i] = table.rows[0].cells[i].innerHTML.toLowerCase().replace(/ /gi, '');
    }

    // go through cells
    for (var i = 1; i < table.rows.length; i++) {

        var tableRow = table.rows[i];
        var rowData = {};

        for (var j = 0; j < tableRow.cells.length; j++) {

            rowData[headers[j]] = tableRow.cells[j].innerHTML;
        }

        data.push(rowData);
    }
    return data;
}

/*Change table into JSON*/
var pretable = document.getElementById('Pendingtable');
var tableJson = JSON.stringify(tableToJson(pretable));

$('#deptPending').change('keyup', function () {
    /*get the value selected*/
    var value = $(this).val();
    /* search the html table for the matching dept name */
    if (value.length < 2) {
        console.log("All dept selected");
        var data1 = getAll(tableJson);
        console.log("Data to be passed in to buildTable: " + data1);
        buildTable(data1, "pendingUnfiltered");
    }
    else if (value.length > 1) {
        console.log(value + " selected");
        var data2 = searchTable(value, tableJson);
        console.log("Data to be passed in to buildTable: " + data2);
        buildTable(data2, "pendingUnfiltered");
    }
});

$('#dateCollectionPending').change('keyup', function () {
    var value = $(this).val();
    var inputDate = new Date(value);
    console.log(inputDate);
    var dataByDate = searchTableByCollDate(inputDate, tableJson);
    buildTable(dataByDate, "pendingUnfiltered");
    $(this).value = "";
});

$('#dateCompiledPending').change('keyup', function () {
    var value = $(this).val();
    var inputDate = new Date(value);
    console.log(inputDate);
    var dataByDate = searchTableByComDate(inputDate, tableJson);
    buildTable(dataByDate, "pendingUnfiltered");
    $(this).value = "";
});

function searchTable(value, data) {

    console.log("Reached searchTable");
    console.log("Printing value:", value);
    console.log("Printing data: ", data);
    var obj = JSON.parse(data);
    var filteredData = [];
    for (var i = 0; i < obj.length; i++) {
        var name = obj[i].department;
        console.log('Department from JSON:', name);
        if (name == value) {
            filteredData.push(obj[i]);
        }
    }
    return filteredData;
}

function searchTableByCollDate(inputDate, data) {
    var obj = JSON.parse(data);
    console.log("Date input:", inputDate);
    var filteredData = [];
    for (var i = 0; i < obj.length; i++) {
        var name = obj[i].collectiondate;
        var rowDate = new Date(name);
        console.log('Collection date from JSON:', rowDate);
        if (rowDate.toDateString() == inputDate.toDateString()) {
            console.log("found a record with matching date");
            filteredData.push(obj[i]);
        }
    }
    return filteredData;
}

function searchTableByComDate(inputDate, data) {
    var obj = JSON.parse(data);
    console.log("Date input:", inputDate);
    var filteredData = [];
    for (var i = 0; i < obj.length; i++) {
        var name = obj[i].collectiondate;
        var rowDate = new Date(name);

        if (rowDate.toDateString() == inputDate.toDateString()) {
            console.log("found a record with matching date");
            filteredData.push(obj[i]);
        }
    }
    return filteredData;
}

function getAll(alldata) {
    var obj = JSON.parse(alldata);
    var filteredData = [];
    for (var i = 0; i < obj.length; i++) {
        filteredData.push(obj[i]);
    }
    return filteredData;
}

var comTable = document.getElementById('Completedtable');
var comTableJSON = JSON.stringify(tableToJson(comTable));

$('#deptCompleted').change('keyup', function () {
    /*get the value selected*/
    var value = $(this).val();
    /* search the html table for the matching dept name */
    if (value.length < 2) {
        console.log("All dept selected");
        var data1 = getAll(comTableJSON);
        buildTable(data1, "completedUnfiltered");
    }
    else if (value.length > 1) {
        console.log(value + " selected");
        var data2 = searchTable(value, comTableJSON);
        buildTable(data2, "completedUnfiltered");
    }
});

$('#completedCompiledDate').change('keyup', function () {
    var value = $(this).val();
    var inputDate = new Date(value);
    console.log(inputDate);
    var dataByDate = searchTableByComDate(inputDate, comTableJSON);
    buildTable(dataByDate, "pendingUnfiltered");
    $(this).value = "";
});

$('#completedCollDate').change('keyup', function () {
    var value = $(this).val();
    var inputDate = new Date(value);
    console.log(inputDate);
    var dataByDate = searchTableByComDate(inputDate, comTableJSON);
    buildTable(dataByDate, "pendingUnfiltered");
    $(this).value = "";
});

function buildTable(data, tableBodyId) {
    var tableOld = document.getElementById(tableBodyId);
    tableOld.innerHTML = "";
    for (var i = 0; i < data.length; i++) {
        console.log(data[i]);
        var row = `<tr>
                <td>${data[i].collectiondate}</td>
                <td>${data[i].department}</td>
                <td>${data[i].compiledon}</td>
                <td>Adam Smith</td>
                <td><button onclick="" style="color:#1f5f89"><i class="fa fa-arrow-right" aria-hidden="true"></i></button></td>
                </tr>`;
        tableOld.innerHTML += row;
    }
}

document.getElementById("defaultOpen").click();

function openReq(evt, cityName) {
    var n, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("requisitioncontainer");
    for (n = 0; n < tabcontent.length; n++) {
        tabcontent[n].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (n = 0; n < tablinks.length; n++) {
        tablinks[n].className = tablinks[n].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
};

function viewDisDetail(e) {
    $.ajax({
        type: 'POST',
        url: '/StationeryStore/GetEmployeeTest',
        data: { 'id': e },
        dataType: "json",
        success: function (response) {
            var jsonObject = JSON.parse(response);
            var collDate = new Date(jsonObject.CollectionDate);
            var collDateString = collDate.toISOString().substr(0, 10);
            $("#collDateSpec").val(collDateString);
            $("#collPtSpec").val(jsonObject.Departments.CollectionPoint.Location);
            var p = document.createElement('p');
            p.innerHTML = jsonObject.Departments.Representative + '<br>';
            document.getElementById('repSpec').appendChild(p);
            var items = jsonObject.DisbursementDetails;
            var tabledisQty = document.getElementById('disbDetailsQty');
            for (var i = 0; i < items.length; i++) {
                var distributedQty = parseInt(items[i].RequisitionDetail.DistributedQty);
                var rowItem = `<tr>
                <td>${items[i].RequisitionDetail.Inventory.description}</td>
                <td>${items[i].RequisitionDetail.RequestedQty}</td>
                <td>${distributedQty}</td>
                <input type="hidden" id="rqdId" value=${jsonObject.Id}/>
                </tr>`;
                tabledisQty.innerHTML += rowItem;
            }
        }
    });

    document.querySelector('.bg-modal').style.display = "flex";
}

/*document.getElementById('.close').addEventListener("click", function () {
        document.querySelector('.bg-modal').style.display = "none";
    });*/

function submitChangesDisb() {
    var newDate = $("#collDateSpec").val();
    var newLocation = $("#collPtSpec").val();
    var rqId = $("#rqdId").val();
    console.log("Printing:" + newDate);
    $.ajax({
        type: 'POST',
        url: '/StationeryStore/SaveChangesToDisb',
        data: { 'newDate': newDate, 'newColl': newLocation, 'rqId': rqId },
        success: function (result) {
            console.log("wow" + result);
        }
    });
    alert("Method REached");
    var clearTable = document.getElementById("Pending");
    clearTable.innerHTML = "";
}