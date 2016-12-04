
$(document).ready(function () {
    var idList = $('[name="idList"]');
    var hidden = $("#hidOperate").val();
    if (typeof (hidden) != 'undefined' && hidden != '') {
        var reparray = hidden.split(",");
        for (var i = 0; i < idList.length; i++) {
            for (var j = 0; j < reparray.length; j++) {
                if (idList[i].value == reparray[j]) {
                    idList[i].checked = true;
                }
            }
        }
    }
});

function setChildMenu(id) {
    var father = document.getElementById("childouter_" + id);
    var checks = father.getElementsByTagName("input");
    var fathercheck = document.getElementById("input_" + id);
    for (var i = 0; i < checks.length; i++) {
        checks[i].checked = fathercheck.checked;
    }
}
function setParentMenu(id) {
    var father = document.getElementById("childouter_" + id);
    var checks = father.getElementsByTagName("input");
    document.getElementById("input_" + id).checked = false;
    for (var i = 0; i < checks.length; i++) {
        if (checks[i].checked) {
            document.getElementById("input_" + id).checked = true;
            break;
        }
    }
}