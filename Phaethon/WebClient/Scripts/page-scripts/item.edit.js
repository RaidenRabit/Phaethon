//on load
$(function () {
    getProductGroups();
    //on barcode change get corresponding info in database for product, product group and item
    $("#Product_Barcode").change(function () {
        getProduct($(this).val());
    });
});

function getProduct(barcode) {
    $.ajax({
        type: "GET",
        url: "/Product/GetProductAjax",
        data: {
            barcode: barcode
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data !== null) {
                if (data.Items.length != 0) {
                    $("#Price").val(data.Items[0].Price);
                }
                $("#Product_Name").val(data.Name);
                $("#Product_ID").val(data.ID);
                $("#ProductGroup_ID").val(data.ProductGroup_ID);
            } else {
                $("#Product_ID").val(0);
            }
        }
    });
}

function getProductGroups(){
    $.ajax({
        type: "GET",
        url: "/ProductGroup/GetProductGroupsAjax",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i].ID + "' " +
                    "data-Margin='" + data[i].Margin + "'>" +
                    data[i].Name +
                    "</option>";
            }
            $("#ProductGroup_ID").html(htmlText);
        },
        error: function () {
            $("#ProductGroup_ID").html("");
        }
    });
}