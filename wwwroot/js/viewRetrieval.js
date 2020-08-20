
function showRequests() {
    var x = document.getElementsByClassName("requestdetail");
    var i;
    for (i = 0; i < x.length; i++) {
        if (x[i].style.visibility === "collapse")
            x[i].style.visibility = "visible";
        else x[i].style.visibility = "collapse";
    }
}

function viewDisDetail() {
    document.getElementByClass("disbursementDetail").style.display = "block";
}

function openDetail() {
    document.getElementById("myDetail").style.width = "300px";
    document.getElementById("All").style.marginRight = "250px";
    document.getElementById("Outstanding").style.marginRight = "250px";
    document.getElementById("New").style.marginRight = "250px";
}

/* Set the width of the sidebar to 0 and the left margin of the page content to 0 */
function closeDetail() {
    document.getElementById("myDetail").style.width = "0";
    document.getElementById("All").style.marginRight = "auto";
    document.getElementById("Outstanding").style.marginRight = "auto";
    document.getElementById("New").style.marginRight = "auto";
}

function showQty() {
    document.getElementById = ("changeQty").style.display = "block";
    document.getElementById = "editQtybtn".style.display = "none";
}

function showChangeStatus() {
    document.getElementById("changeStatus").style.display = "block";
    document.getElementById("originalStatus").style.display = "none";
}

document.getElementById("defaultOpen").click();

function openReq(evt, cityName) {
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

document.querySelector('.close').addEventListener("click", function () {
    document.querySelector('.bg-modal').style.display = "none";
});