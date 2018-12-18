//On load
$(function () {
    $("#deleteInvoice").click(function () {
        $("#dialog").html("");
        $("#dialog").dialog({
            title: deleteLabel,
            autoOpen: true,
            modal: true,
            buttons: {
                Delete: function () { DeleteInvoice(); },
                Cancel: function () { $(this).dialog("close"); }
            }
        });
    });
});

function DeleteInvoice() {
    $.ajax({
        url: "/Invoice/Delete/" + $("#ID").val(),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json"
    }).done(function (res) {
        window.location.href = res.newUrl;
    }).fail(function (xhr, a, error) {
        console.log(error);
    });
}