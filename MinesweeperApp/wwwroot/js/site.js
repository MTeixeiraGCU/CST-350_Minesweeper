$(function () {
    
    //Prevents right click from bringing up default menu 
    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });

    //removes form submitting on the gameboard
    $(".game-form").submit(function (e) {
        e.preventDefault();
    });

    //handles all mouse down events
    $(document).on("mousedown", ".game-button", function (event) {
        switch (event.buttons) {
            case 1:
                var buttonNumber = $(this).val();
                console.log("Button number " + buttonNumber + " was left clicked");
                doButtonUpdate(buttonNumber, "/game/HandleButtonLeftClick");
                break;
            case 2:
                var buttonNumber = $(this).val();
                console.log("Button number " + buttonNumber + " was right clicked");
                doButtonUpdate(buttonNumber, "/game/HandleButtonRightClick");
                break;
            default:
                console.log("That control is not supported!")
        }
    });

});

//processes a button left or right click
function doButtonUpdate(buttonNumber, urlString) {
    $.ajax({
        dataType: "json",
        url: urlString,
        data: {
            "buttonNumber": buttonNumber
        },
        success: function (data) {
            console.log(data);
            for (var i = 0; i < data.length; i++) {
                updateCell(data[i]);
            }
            checkWinCondition(buttonNumber);
        },
        error: function (jq, textError, errorMsg) {
            console.log(textError + " : " + errorMsg);
        }
    });
};

//this method will cycle through and update all the cells images
function updateCell(buttonNumber) {
    
    $.ajax({
        dataType: "text",
        url: "/game/UpdateOneCell",
        data: {
            "buttonNumber": buttonNumber
        },
        success: function (data) {
            console.log("Cell: " + buttonNumber + " was Updated!");
            console.log(data);
            $('#' + buttonNumber).html(data);
        },
        error: function (jq, textError, errorMsg) {
            console.log(textError + " : " + errorMsg);
        }
    });
}

//checks for a win condition on the game board
function checkWinCondition(buttonNumber) {
    $.ajax({
        type: 'GET',
        dataType: "text",
        url: '/game/CheckGrid',
        success: function (result) {
            console.log("Game status is : " + result);
            
            if (result == "0") {
                var url = "Winner";
                alert("Congrats!");
                $(location).attr('href', url);
            } else if (result == "1") {
                var url = "EndGame";
                alert("You stepped on a mine!");
                $(location).attr('href', url);
            }
        },
        error: function (jq, textError, errorMsg) {
            console.log(textError + " : " + errorMsg);
        }
    });
};
