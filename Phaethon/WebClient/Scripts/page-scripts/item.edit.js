$(function () {
    getProductGroups();
    getTaxGroups();

    //on barcode change get corresponding info in database for product, product group and item
    $("#Product_Barcode").change(function () {
        getProduct($(this).val());
    });
});

function getProductGroups() {
    return $.ajax({
        type: "GET",
        url: "/Api/ProductGroup/GetProductGroups",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i].ID + "' " +
                    "data-Margin='" + data[i].Margin + "'>" +
                    data[i].Name + " " + data[i].Margin + "%" +
                    "</option>";
            }
            $("#ProductGroup").html(htmlText);
        },
        error: function () {
            $("#ProductGroup").html("");
        }
    });
}

function getTaxGroups() {
    return $.ajax({
        type: "GET",
        url: "/Api/TaxGroup/GetTaxGroups",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i].ID + "' " +
                    "data-Tax='" + data[i].Tax + "'>" +
                    data[i].Name + " " + data[i].Tax + "%" +
                    "</option>";
            }
            $("#IncomingTaxGroup").html(htmlText);
            $("#OutgoingTaxGroup").html(htmlText);
        },
        error: function () {
            $("#IncomingTaxGroup").html("");
            $("#OutgoingTaxGroup").html("");
        }
    });
}

function getProduct(barcode) {
    $.ajax({
        type: "GET",
        url: "/Api/Product/GetProduct",
        data: {
            barcode: barcode
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data !== null) {
                if (data.Items.length != 0) {
                    $("#IncomingPrice").val(data.Items[0].IncomingPrice);
                    $("#OutgoingPrice").val(data.Items[0].OutgoingPrice);
                }
                $("#Product_Name").val(data.Name);
                $("#Product_ID").val(data.ID);
                $("#ProductGroup").val(data.ProductGroup_ID);
            } else {
                $("#Product_ID").val(0);
            }
        }
    });
}