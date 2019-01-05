$(function () {
    GetItems();

    //on search option change get corresponding items
    $("#serialNumber, #productName, #barcode").change(function () {
        GetItems();
    });
});

//gets items
function GetItems() {
    $.ajax({
        type: "GET",
        url: "/Item/GetItems",
        data: {
            serialNumber: $("#serialNumber").val(),
            productName: $("#productName").val(),
            barcode: $("#barcode").val(),
            showAll: $("#showAll").val()
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<tr>" +
                    "<input id='Items_" + i + "__ID'  type='hidden' value='" + data[i].ID + "'>" +
                    "<td>" + data[i].Quantity + "</td>" +
                    "<td>" + data[i].SerNumber + "</td>" +
                    "<td>" + data[i].Product.Name + "</td>" +
                    "<td>" + data[i].Product.Barcode + "</td>" +
                    "<td>" + data[i].Price + "</td>" +
                    "<td><input id='Items_" + i + "__Select' type='button' class='btn btn-success btn-block' value='"+select+"'/></td>" +
                    "</tr>";
            }
            $("#itemTable tbody").html(htmlText);
            $("#itemTable tbody tr").each(function () {
                var row = $(this).find("input").attr("id").split("_")[1].split("__")[0];
                $("#Items_" + row + "__Select").click(function () {
                    getItem($("#Items_" + row + "__ID").val());
                });
            });
        },
        error: function () {
            $("#itemTable tbody").html("");
        }
    });
}