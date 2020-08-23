/*Change table into JSON*/


function openDetail(id, department) {
    console.log(department, id);
    document.getElementById("myDetail").style.width = "300px";
    document.getElementById("All").style.marginRight = "250px";

    $.ajax({
        url: "/Requisition/GetRequisitionDetail/",
        method: "POST",
        data: { reqId: id },
        dataType: 'json',
        success: function (response) {
            console.log("hello detail");
            var jsonObject = JSON.parse(response);
            var p = document.createElement('p');
            p.innerHTML = jsonObject.Department.DeptName;
            document.getElementById('reqDeptName').appendChild(p);

            var xo = document.createElement('p');
            xo.innerHTML = jsonObject.DateSubmitted;
            document.getElementById('reqDate').appendChild(xo);

            var titleDetail = document.createElement('h1');
            titleDetail.innerHTML = jsonObject.Id;
            var reId = document.getElementById('reqId');
            reId.appendChild(titleDetail);

            var os = document.getElementById('originalStatus');
            os.innerHTML = ` ${jsonObject.status} < button class="editbtn" onclick = "showChangeStatus()" > <i class="far fa-edit"></i></button >`;


        }
    });
}

/* Set the width of the sidebar to 0 and the left margin of the page content to 0 */
function closeDetail() {
    document.getElementById("myDetail").style.width = "0";
    document.getElementById("All").style.marginRight = "auto";
    document.getElementById("Outstanding").style.marginRight = "auto";
    document.getElementById("New").style.marginRight = "auto";
}

function showChangeStatus() {
    document.getElementById("changeStatus").style.display = "block";
    document.getElementById("originalStatus").style.display = "none";
}

document.getElementById("defaultOpen").click();

function openReq(evt, cityName) {
    // Declare all variables
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("requisitioncontainer");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}
/*
$(document).ready(function () {
    $('#dt-multi-checkbox').dataTable({

        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'multi',
            selector: 'td:first-child'
        }
    });
});*/