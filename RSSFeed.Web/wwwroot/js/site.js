// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//$('#getData').on("change", function () {
//    GetPosts();
//});

//function GetPosts() {
//    var radioValue = $("input[name='source']:checked").val();
//    var page = $('#pageNumber').val();
//    if (radioValue == "date") {
//        var data = {
//            "pageNumber": page,
//            "sortType": radioValue,
//            "source": $('#source').val()
//        };
//        $.ajax({
//            url: '/Home/Index',
//            type: 'GET',
//            cache: false,
//            async: true,
//            dataType: "html",
//            data: data
//        })
//            .done(function (result) {
//                $("input[name='source']:checked").prop('checked', true);
//                $('#posts').html(result);
//            }).fail(function (xhr) {
//                console.log('error : ' + xhr.status + ' - '
//                    + xhr.statusText + ' - ' + xhr.responseText);
//            });
//    }
//    else {
//        data = {
//            "pageNumber": page,
//            "sortType": radioValue,
//            "source": $('#source').val()
//        };
//        $.ajax({
//            url: '/Home/Index',
//            type: 'GET',
//            cache: false,
//            async: true,
//            dataType: "html",
//            data: data
//        })
//            .done(function (result) {
//                $("input[name='source']:checked").prop('checked', true);
//                $('#posts').html(result);
//            }).fail(function (xhr) {
//                console.log('error : ' + xhr.status + ' - '
//                    + xhr.statusText + ' - ' + xhr.responseText);
//            });
//    }

//}

function PostSeen(postId) {
    var id = $(postId).val();
    $.ajax({
        url: "/Home/PostSeen/",
        type: "POST",
        data: {
            "postId": id
        },
        cache: false,
        success: function (result) {
            console.log('success');
            return;
        },
        error: function (XMLHttpRequest) {
            console.log('failure');
            return;
        }
    });
}