function loadData() {
    var items = [];

    $.getJSON("http://localhost:11341/api/hobbit", function (data) {
        $.each(data, function (key, val) {
            var lineItem = "<tr>\n" + 
                "<td>" + val.Name + "</td>\n" +
                "<td>" + val.FamilyName + "</td>\n" +
                "<td>" + val.BirthYear + "</td>\n" +
                "<td>" + val.DeathYear + "</td>\n";
            $("#hobbitsList").append(lineItem);
        });
    });
};

$(document).ready(function () {
    loadData();
});
