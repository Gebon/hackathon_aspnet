String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};

$(document).ready(function () {
    $("#yolo").click(function () {
        var inputs = $("input:text");
        var arr = [];
        for (var i = 0; i < inputs.length; i++) {
            arr.push({ "fieldName": inputs[i].id, "fieldValue": inputs[i].value });
        }
        console.log(arr);
        $.ajax({
            url: "/Character/Search",
            type: "POST",
            data: {
                'filterData': JSON.stringify(arr)
            },
            dataType: "json",
            success: function (data) {
                var innerHtml = "";
                for (var i = 0; i < data.length; i++) {
                    innerHtml +=
                        "<div class='aligner'>" +
                            "<div class='flip-container'>" +
                                "<div class='flipper'>" +
                                    "<div class='front center'>" +
                                        "<h3>NAME</h3>" +
                                        "<img class='image' src='/Content/Images/TITLE.jpg' alt='NAME' />" +
                                    "</div>" +
                                    "<div class='back center'>" +
                                        "<p>Born BORN</p>" +
                                        "<p>Gender: GENDER</p>" +
                                        "<p>Character is STATE</p>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>";
                    innerHtml = innerHtml
                        .replaceAll("NAME", data[i].Name)
                        .replaceAll("TITLE", data[i].Name.replaceAll(" ", ""))
                        .replaceAll("GENDER", data[i].Gender === 1 ? "female" : "male")
                        .replaceAll("BORN", data[i].Born)
                        .replaceAll("STATE", data[i].Died ? "dead" : "alive");
                }
                $(".content").html(innerHtml);
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
});