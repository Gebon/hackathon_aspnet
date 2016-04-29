$(document).ready(function() {
    $("#yolo").click(function() {
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
            success: function(data) {
                var innerHtml = "";
                for (var i = 0; i < data.length; i++) {
                    innerHtml += 
                        "<div class='aligner'>" +
                            "<div class='flip-container'>" + 
                                "<div class='flipper'>" +
                                    "<div class='front center'>" + 
                                        "<h3>NAME</h3>" +
                                        "<img class='image' src='~/Content/Images/NAME_S.jpg' alt='NAME' />" +
                                    "</div>" +
                                    "<div class='back center'>" +
                                        "<p>Born BORN</p>" +
                                        "<p>Gender: GENDER</p>" +
                                        "<p>Character is STATE)</p>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>"
                    .replace("NAME", data[i].name)
                    .replace("NAME_S", data[i].name.replace(" ", ""))
                    .replace("GENDER", data[i].gender.toLowerCase())
                    .replace("BORN", data[i].born)
                    .replace("STATE", data[i].died ? "dead" : "alive");
                }
                $(".content").html(innerHtml);
            },
            error: function(error) {
                console.log(error);
            }
        });
    });
});