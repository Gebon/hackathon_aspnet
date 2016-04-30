//$(document).ready(function () {
//    $(".vote").click(function () {
//        var id = $(this).data("id");
      
//        console.log(id);
//        $.ajax({
//            url: "/Character/Vote",
//            type: "POST",
//            data: {
//                'id': id
//            },
//            success: function (data) {
//                window.location.reload();
//            },
//            error: function (error) {
//                console.log(error);
//            }
//        });
//    });

//    $(".unvote").click(function () {
//        var id = $(this).data("id");

//        console.log(id);
//        $.ajax({
//            url: "/Character/Unvote",
//            type: "POST",
//            data: {
//                'id': id
//            },
//            success: function (data) {
//                window.location.reload();
//            },
//            error: function (error) {
//                console.log(error);
//            }
//        });
//    });
//});