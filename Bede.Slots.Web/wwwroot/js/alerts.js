var Alerts = function () {
    function show(message) {
        $('#alerts-container').append(`<div class="alert alert-warning alert-dismissible fade show" role="alert">
            <span id="alert-text"></span>
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>`);
        $('#alert-text').text(message);
        $('.alert').alert();
    }

    function close() {
        $('.alert').remove();
    }

    return {
        show: show,
        close: close
    }
};