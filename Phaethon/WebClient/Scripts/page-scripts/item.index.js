//on load
$(function () {
    $("#newItem").click(function () {
        document.location.href = "Item/Edit/0";
    });
});

function getItem(id)
{
    document.location.href = "Item/Edit/" + id;
}