$(function () {
    getProductGroups();
    getTaxGroups();
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