$(function () {
    $(".chosen-select").chosen();
});

const draggables = document.querySelectorAll('.draggable')
const containers = document.querySelectorAll('.match-container')
const matchtiles = document.querySelectorAll('.matchtileform')
matchtiles.forEach(matchtile => {
    $(matchtile).hide();
})

draggables.forEach(draggable => {
    draggable.addEventListener('dragstart', () => {
        draggable.classList.add('dragging')
    })

    draggable.addEventListener('dragend', async (e) => {
        var id = $(e.target).attr("id");
        var weekId = $(e.target.parentElement).attr("weekId");
        var datetime = $("#" + id + " .matchtiledatetime")[0].innerText;

        var element = $("#" + id + " .matchtiledatetime");
        var jsonData = { action: "MoveTile", id: id, datetime: datetime, weekId: weekId }
        edit(jsonData, "Manager", "MoveTile", moveTile, element);
        
        draggable.classList.remove('dragging')

    })
})

containers.forEach(container => {
    container.addEventListener('dragover', e => {
        e.preventDefault()

        const afterElement = getDragAfterElement(container, e.clientY)
        const draggable = document.querySelector('.dragging')
        if (afterElement == null) {
            container.appendChild(draggable)
        } else {
            container.insertBefore(draggable, afterElement)
        }
    })
})

function getDragAfterElement(container, y) {
    const draggableElements = [...container.querySelectorAll('.draggable:not(.dragging)')]

    return draggableElements.reduce((closest, child) => {
        const box = child.getBoundingClientRect()
        const offset = y - box.top - box.height / 2
        if (offset < 0 && offset > closest.offset) {
            return { offset: offset, element: child }
        } else {
            return closest
        }
    }, { offset: Number.NEGATIVE_INFINITY }).element
}


function edit(jsonData, controllerName, functionName, callback, element) {
    //console.log("EDIT AJAX")
    $.ajax({
        method: "POST",
        url: `@Url.Action("Edit", "Manager")`,
        data: jsonData,
        dataType: "json",
        success: function (response) {
            //alert(response.week);
            //alert(response.dateTime);
            callback(response, element);
        },
        error: function (response) {
            alert("Error: " + response);
        }
    }).done(function (msg) {
        //alert("Data Saved: " + msg);
    });
    
}




moveTile = function (response, element) {

    let date = new Date(response.dateTime);

    $(element).text(date.toLocaleString('en-NZ'));
}

changeTile = function (response, element) {

    let date = new Date(response.dateTime);

    $(element).children(".matchtiledatetime").text(date.toLocaleString('en-NZ'));
    $(element).children(".matchtilefieldkey").text(response.fieldId);
}


function myFunction(id) {
    var myId = "#" + id
    var selector = myId + " .matchtileform"

    $(selector).toggle();
}



function getAvaialableFields(id) {
    console.log("getAvaialableFields")
    var jsonData = { id: id }
    $.ajax({
        method: "GET",
        url: "../../Manager/AvailableFields",
        data: jsonData,
        dataType: "json",
        success: function (response) {
            console.log(response)
            //alert(response.week);
            //alert(response.dateTime);
            callback(response, element);
        },
        error: function (response) {
            alert("Error: " + response);
        }
    }).done(function (msg) {
        //alert("Data Saved: " + msg);
    });

}



function saveMatchTile(id) {
    console.log("save match tile");
    console.log(id);

    var myId = "#" + id
    var input = myId + " .matchtileform" + " input"
    var select = myId + " .matchtileform" + " select"
    var element = myId + " .matchtiledatetime"
    
    var element = $(myId + " .matchdetails");

    var datetime = $(input).val();
    var fieldId = $(select).find(":selected").val();

    var jsonData = { action: "ChangeDate", id: id, datetime: datetime, fieldId: fieldId }

    console.log(jsonData);
    edit(jsonData, "Manager", "ChangeDate", changeTile, element);

}