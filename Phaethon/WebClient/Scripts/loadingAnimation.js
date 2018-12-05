$(document).bind({
    ajaxStart: function () {
        $(".modal").show();
    },
    ajaxStop: function () {
        $(".modal").hide();
    },
    ajaxError: function() {
        $(".modal").hide();
    }
});