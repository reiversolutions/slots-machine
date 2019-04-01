var slots = (function () {

    let balance = 0;
    let stake = 0;

    const _alert = Alerts();
    const thisObj = this;

    _initEvents();

    function _initEvents() {
        $('#deposit-btn').on('click', _handleDeposit);
        $('#spin-btn').on('click', _handleSpin);
    }

    function _handleDeposit() {
        _alert.close();
        $('#slots-container').addClass('d-none');
        const deposit = parseFloat($('#deposit-input').val());
        if (isNaN(deposit) || deposit <= 0) {
            _alert.show("Your current balance is empty. Please deposit more funds to continue playing.");
            return false;
        }

        _alert.close();
        $('#welcome-container').addClass('d-none');
        $('#stake-container').removeClass('d-none');

        _updateBalance(deposit);
        $('#balance-container').removeClass('d-none');
    }

    function _updateBalance(newBalance) {
        thisObj.balance = newBalance;
        $('#balance-text').text(thisObj.balance.toFixed(2));
    }

    function _handleSpin() {
        _alert.close();
        const stake = parseFloat($('#stake-input').val());
        if (isNaN(stake) || stake <= 0) {
            _alert.show("Please stake a bet of £1 or higher.");
            return false;
        }

        if (stake > thisObj.balance) {
            _alert.show(`You can not stake more than your current balance. Please add a stake less than ${thisObj.balance.toFixed(2)}.`);
            return false;
        }

        _alert.close();
        thisObj.stake = stake;
        _getSpinResult();
    }

    function _getSpinResult() {
        var request = {
            'balance': thisObj.balance,
            'stake': thisObj.stake
        };

        $.ajax({
            type: "POST",
            url: "https://localhost:44356/api/",
            contentType: 'application/json',
            dataType: "json",
            data: JSON.stringify(request),
            success: function (data) {
                if (data.success) {
                    _displaySpin(data.data);
                } else {
                    _alert.show(data.failureMessage);
                }
            },
            error: function (jqXHR, status, err) {
                _alert.show(status);
            },
        });
    }

    function _displaySpin(data) {
        _updateBalance(data.balance);
        _updateSlotMachine(data.rows);
        if (data.winnings > 0) {
            _alert.show(`Congratulations! You won £${data.winnings.toFixed(2)}`);
        } else {
            _alert.show("No luck this time! Try your luck again.");
        }
        $('#slots-container').removeClass('d-none');

        if (data.balance <= 0) {
            _endGame();
        } 
    }

    function _updateSlotMachine(rows) {
        var cells = $('.symbol');
        if (rows.length !== cells.length) {
            _alert.show("An error occured returning slot data.");
            return false;
        }

        for (var i = 0; i < rows.length; i++) {
            $(cells[i]).text(rows[i]);
        }
    }

    function _endGame() {
        _alert.close();
        $('#welcome-container').removeClass('d-none');
        $('#stake-container').addClass('d-none');
        $('#balance-container').addClass('d-none');
        _alert.show("GAME OVER - Your balance reached zero. Deposit more funds to continue.");
        thisObj.balance = 0;
    }
    
    return {

    }
})();