//numpad listeners

var numInput = $(".inputWrapper input");
var operators = ['X', '/', '-', '+'];
var stackChar = "";

$(() => {
    $(".numPad button.num").click(function () {
        var buttonsClass = $(this).attr('class').split(' ');
        buttonsClass = buttonsClass[1];
        buttonsClass = buttonsClass.split('_');
        buttonsClass = buttonsClass[1];
        numInput.val(numInput.val() + buttonsClass);
    })

    $(".operationsWrapper button:not(.sum)").click(function () {
        var opVal = $(this).text();
        var inputVal = numInput.val();
        var lastChar = inputVal.slice(inputVal.length - 1);
        var containsOperator = false;
        operators.forEach(x => containsOperator |= (inputVal.indexOf(x) > -1));
        if (operators.indexOf(lastChar) > -1) {
            numInput.val(inputVal.slice(0, -1) + opVal);
        }
        else {
            numInput.val(inputVal + opVal);
        }
    })

    $(".numPad button.back").click(function () {
        numInput.val(numInput.val().slice(0, -1));
    })

    $(".numPad button.clear").click(function () {
        halt(false);
        numInput.val("");
    })

    $(".operationsWrapper button.sum").click(function () {
        numInput.val(calculate(numInput.val()));
    })

});

function halt(mode) {
    if (mode) {
        $("button").prop('disabled', true);
        $(".numPad button.clear").prop('disabled', false);
    }
    else {
        $("button").prop('disabled', false);
    }
}
