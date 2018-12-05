$(document).bind({
    ajaxStart: function () {
        $(".modal").show();
    },
    ajaxStop: function () {
        $(".modal").hide();
    }
});