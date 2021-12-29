$(function () {
    
    //Prevents right click from bringing up default menu 
    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });

    //removes form submitting on the gameboard. Submitting the form has been depracated
    $(".game-form").submit(function (e) {
        e.preventDefault();
    });

    //handles all mouse down events
    $(document).on("mousedown", ".game-button", function (event) {
        switch (event.buttons) {
            case 1:
                var buttonNumber = $(this).val();
                console.log("Button number " + buttonNumber + " was left clicked");
                doButtonUpdate(buttonNumber, "/game/HandleButtonLeftClick", updateCells);
                break;
            case 2:
                var buttonNumber = $(this).val();
                console.log("Button number " + buttonNumber + " was right clicked");
                doButtonUpdate(buttonNumber, "/game/HandleButtonRightClick", updateCells);
                break;
            default:
                console.log("That control is not supported!")
        }
    });
});

//processes a button left or right click
function doButtonUpdate(buttonNumber, urlString, cellCallback) {
    $.ajax({
        dataType: "json",
        url: urlString,
        data: {
            "buttonNumber": buttonNumber
        },
        success: function (data) {
            console.log(data);
            cellCallback(data);
        },
        error: function (jq, textError, errorMsg) {
            console.log(textError + " : " + errorMsg);
        }
    });
};

//callback method to process all revealed cells for a given move. Must be preformed before win/loss logic can proceed.
function updateCells(cells) {

    //setup for detecting completion of updates
    var promises = [];

    //perform all partial update ajax calls
    cells.forEach(function (cell) {
        promises.push(updateCell(cell));
    });

    //Once all partial loads have been complete, process a win check.
    Promise.all(promises).then(checkWinCondition);
}

//this method produces a partial page load on the given button.
function updateCell(buttonNumber) {
    return $.ajax({
            type: 'GET',
            dataType: "text",
            url: "/game/UpdateOneCell",
            data: {
                "buttonNumber": buttonNumber
            },
            success: function (data) {
                return data;
            },
            error: function (jq, textError, errorMsg) {
                console.log(textError + " : " + errorMsg);
            },
            timeout: 3000
        })
        .then(function (data) {
            console.log("Cell: " + buttonNumber + " was Updated!");
            console.log(data);
            $('#' + buttonNumber).html(data);
        });
}

//checks for a win condition on the game board
function checkWinCondition() {
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
