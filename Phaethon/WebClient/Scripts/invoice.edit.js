var senderElement = "Sender";
var receiverElement = "Receiver";

var ProductGroups;
var TaxGroups;
var transport;

$(function () {//on code load
    transport = Number($("#Transport").val());

    CompanyChange(receiverElement);
    CompanyChange(senderElement);
    
    RepresentativeChange(receiverElement);
    RepresentativeChange(senderElement);

    //allows only int values
    onlyNumbers("");

    $(".date-picker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-20:+0", // You can set the year range as per as your need
        dateFormat: "dd-M-yy"
    });

    products();
    
    dialogs();
});

function dialogs() {
    $("#dialog").dialog({
        autoOpen: false,
        modal: true
    });

    $("#TaxGroupLabel").click(function () {
        $("#dialog").dialog({ title: "Tax group", buttons: { "Save": function () { $("#taxGroupForm").submit(); } } });
        $.ajax({
            type: "GET",
            url: "http://localhost:49873/TaxGroup/Create",
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                $("#dialog").html(data);
                $("#taxGroupForm").submit(function (event) {
                    event.preventDefault();
                    $.ajax({
                        type: "POST",
                        url: "http://localhost:49873/TaxGroup/Create",
                        data: $("#taxGroupForm").serialize(),
                        success: function () {
                            $("#dialog").html("");
                            $("#dialog").dialog("close");
                            $.when(getTaxGroups()).done(function () {
                                $("#itemTable tbody tr").each(function () {
                                    var rowValue = $(this).find("input").attr("name").split("[")[1].split("]")[0];
                                    $("#Elements_" + rowValue + "__IncomingTaxGroup").html(TaxGroups);
                                    $("#Elements_" + rowValue + "__IncomingTaxGroup").find("option[data-id='" + $("#Elements_" + rowValue + "__Item_IncomingTaxGroup_ID").val() + "']").attr("selected", "selected");
                                    $("#Elements_" + rowValue + "__IncomingTaxGroup").change();
                                });
                            });
                        }
                    });
                });
                $("#dialog").dialog("open");
            }
        });
    });

    $("#ProductGroupLabel").click(function () {
        $("#dialog").dialog({ title: "Product group", buttons: { "Save": function () { $("#productGroupForm").submit(); } } });
        $.ajax({
            type: "GET",
            url: "http://localhost:49873/ProductGroup/Create",
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                $("#dialog").html(data);
                $("#productGroupForm").submit(function (event) {
                    event.preventDefault();
                    $.ajax({
                        type: "POST",
                        url: "http://localhost:49873/ProductGroup/Create",
                        data: $("#productGroupForm").serialize(),
                        success: function () {
                            $("#dialog").html("");
                            $("#dialog").dialog("close");
                            $.when(getProductGroups()).done(function () {
                                $("#itemTable tbody tr").each(function () {
                                    var rowValue = $(this).find("input").attr("name").split("[")[1].split("]")[0];
                                    $("#Elements_" + rowValue + "__ProductGroup").html(ProductGroups);
                                    $("#Elements_" + rowValue + "__ProductGroup").find("option[data-id='" + $("#Elements_" + rowValue + "__Item_Product_ProductGroup_ID").val() + "']").attr("selected", "selected");
                                    $("#Elements_" + rowValue + "__ProductGroup").change();
                                });
                            });
                        }
                    });
                });
                $("#dialog").dialog("open");
            }
        });
    });
}

//action listeners
function products() {
    $.when(getProductGroups(), getTaxGroups()).done(function (a1, a2) {
        //gets all Items in invoice
        $.ajax({
            type: "GET",
            url: "http://localhost:64010/Api/Element/GetInvoiceElements?id=" + $("#ID").val(),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    $("#itemTable tbody").append(addNewElement(i, data[i].Item.ID, data[i].Item.Product.ID, data[i].Item.Product.ProductGroup.ID, data[i].Item.IncomingTaxGroup.ID, data[i].Item.SerNumber, data[i].Item.Product.Name, data[i].Item.Product.Barcode, data[i].Item.IncomingPrice));
                    ItemChange(i);
                    $("#Elements_" + i + "__ProductGroup").find("option[data-id=" + data[i].Item.Product.ProductGroup.ID + "]").attr("selected", "selected");
                    $("#Elements_" + i + "__ProductGroup").change();
                    $("#Elements_" + i + "__IncomingTaxGroup").find("option[data-id='" + data[i].Item.IncomingTaxGroup.ID + "']").attr("selected", "selected");
                    $("#Elements_" + i + "__IncomingTaxGroup").change();
                }
                TotalAmount();
            }
        });

        //adds new item row to table
        $("#newItemRow").click(function () {
            var rowValue;
            if ($("#itemTable tbody tr").length == 0) {
                rowValue = 0;
            } else {
                rowValue = parseInt($("#itemTable tbody tr:last").find("input").attr("name").split("[")[1].split("]")[0]) + 1;
            }
            $("#itemTable tbody").append(addNewElement(rowValue, 0, null, null, null, "", "", null, 0));

            //search for product by barcode
            ItemChange(rowValue);
            $("#Elements_" + rowValue + "__ProductGroup").change();
            $("#Elements_" + rowValue + "__IncomingTaxGroup").change();
        });
    });
}

function ItemChange(rowValue) {
    $("#Elements_" + rowValue + "__IncomingTaxGroup").html(TaxGroups);
    $("#Elements_" + rowValue + "__ProductGroup").html(ProductGroups);

    //allows only int values in last row
    onlyNumbers("#itemTable tbody tr:last ");

    //on barcode change get corresponding info in database for product, product group and item
    $("#Elements_" + rowValue + "__Item_Product_Barcode").change(function () {
        $.ajax({
            type: "GET",
            url: "http://localhost:64010/Api/Product/GetProduct?barcode=" + $(this).val(),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data !== null) {
                    if (data.Items.length != 0) {
                        $("#Elements_" + rowValue + "__Item_IncomingPrice").val(data.Items[0].IncomingPrice);
                        $("#Elements_" + rowValue + "__Item_IncomingPrice").change();
                    }
                    $("#Elements_" + rowValue + "__Item_Product_Name").val(data.Name);
                    $("#Elements_" + rowValue + "__Item_Product_ID").val(data.ID);

                    $("#Elements_" + rowValue + "__ProductGroup option[data-id='" + data.ProductGroup.ID + "']").attr("selected", "selected");
                    $("#Elements_" + rowValue + "__ProductGroup").change();
                } else {
                    $("#Elements_" + rowValue + "__Item_Product_ID").val(0);
                }
            }
        });
    });

    //changes product group
    $("#Elements_" + rowValue + "__ProductGroup").change(function () {
        $("#Elements_" + rowValue + "__Item_Product_ProductGroup_ID").val($(this).find("option:selected").data("id"));
    });
    
    //changes tax group
    $("#Elements_" + rowValue + "__IncomingTaxGroup").change(function () {
        $("#Elements_" + rowValue + "__Item_IncomingTaxGroup_ID").val($(this).find("option:selected").data("id"));
        $("#Elements_" + rowValue + "__Item_IncomingPrice").change();
    });
    
    //Price without tax changed
    $("#Elements_" + rowValue + "__Price").change(function () {
        var procent = $(this).closest("tr").find("option:selected").val() / 100;
        var val = parseFloat($(this).val());
        var price = val + val * procent;
        $("#Elements_" + rowValue + "__Item_IncomingPrice").val(price.toFixed(2));
        TotalAmount();
    });

    //Incoming price changed
    $("#Elements_" + rowValue + "__Item_IncomingPrice").change(function () {
        var procent = $(this).closest("tr").find("option:selected").val() / 100 + 1;
        var val = parseFloat($(this).val());
        var price = val / procent;
        $("#Elements_" + rowValue + "__Price").val(price.toFixed(2));
        TotalAmount();
    });

    //delete button clicked 
    $("#Elements_" + rowValue + "__Delete").click(function () {
        var row = $(this).closest("tr");
        if ($("#Elements_" + rowValue + "__Item_ID").val() == 0) {
            row.remove();
        }
        else {
            if ($("#Elements_" + rowValue + "__Invoice_ID").val() == -1) {
                $("#Elements_" + rowValue + "__Invoice_ID").val($("#ID").val());
                $(this).removeClass("btn-danger");
            } else {
                $("#Elements_" + rowValue + "__Invoice_ID").val(-1);
                $(this).addClass("btn-danger");
            }
        }
    });
}

function CompanyChange(element) {
    //gets all companies
    $.ajax({
        type: "GET",
        url: "http://localhost:64010/Api/Company/GetCompanies",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i].Name + "' " +
                    "data-ID='" + data[i].ID + "'/>";
            }
            $("#companies").html(htmlText);
        },
        error: function () {
            $("#companies").html("");
        }
    });

    //if transport cost changes
    $("#Transport").change(function () {
        TotalAmount();
    });

    //sets info to create new company
    $("#reset" + element).click(function () {
        $("#" + element + "_ID").val(0);
        $("#" + element + "_Name").val("");
        $("#" + element + "_Company_ID").val(0);
        $("#" + element + "_Company_Name").val("");
        $("#" + element + "_Company_BankNumber").val("");
        $("#" + element + "_Company_RegNumber").val("");
        $("#" + element + "_Company_Address").val("");
        $("#" + element + "_Company_Location").val("");

        $("#" + element + "_ID").change();
        $("#" + element + "_Company_ID").change();
    });

    //Colors company if new will be added
    $("#" + element + "_Company_ID").change(function () {
        if ($(this).val() == 0) {
            $('label[for="' + element + "_Company_Name" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_BankNumber" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_RegNumber" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_Address" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_Location" + '"]').addClass("text-success");
        } else {
            $('label[for="' + element + "_Company_Name" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_BankNumber" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_RegNumber" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_Address" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_Location" + '"]').removeClass("text-success");
        }
        $("#" + element + "_ID").change();
    });

    //sets selected companies companies info
    $("#" + element + "_Company_Name").change(function () {
        var option = $("#companies option[value='" + $(this).val() + "']");
        if (option.length !== 0) {
            $.ajax({
                type: "GET",
                url: "http://localhost:64010/Api/Company/GetCompany?id=" + option.data("id"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    $("#" + element + "_Company_ID").val(data.ID);
                    $("#" + element + "_Company_Name").val(data.Name);
                    $("#" + element + "_Company_BankNumber").val(data.BankNumber);
                    $("#" + element + "_Company_RegNumber").val(data.RegNumber);
                    $("#" + element + "_Company_Address").val(data.Address);
                    $("#" + element + "_Company_Location").val(data.Location);
                    var htmlText = "";
                    for (var i = 0; i < data.Representatives.length; i++) {
                        htmlText += "<option value='" +
                            data.Representatives[i].Name +
                            "'" +
                            "data-ID='" +
                            data.Representatives[i].ID +
                            "'/>";
                    }
                    $("#" + element + "Representatives").html(htmlText);
                    option = $("#" +
                        element +
                        "Representatives option[data-id='" +
                        $("#" + element + "_ID").val() +
                        "']");
                    if (option.length === 0) {
                        option = $("#" + element + "Representatives option");
                        $("#" + element + "_Name").val(option.val());
                        $("#" + element + "_ID").val(option.data("id"));
                    }
                    
                    $("#" + element + "_Company_ID").change();
                },
                error: function() {
                    $("#" + element + "_ID").val(0);
                    $("#" + element + "_Name").val("");
                    $("#" + element + "_Company_ID").val(0);
                    $("#" + element + "_Company_Name").val("");
                    $("#" + element + "_Company_BankNumber").val("");
                    $("#" + element + "_Company_RegNumber").val("");
                    $("#" + element + "_Company_Address").val("");
                    $("#" + element + "_Company_Location").val("");
                    $("#" + element + "Representatives").html("");

                    $("#" + element + "_Company_ID").change();
                }
            });
        } else {
            $("#" + element + "_ID").val(0);
            $("#" + element + "_Company_ID").val(0);
            $("#" + element + "Representatives").html("");
            
            $("#" + element + "_Company_ID").change();
        }
    });
}

function RepresentativeChange(element) {
    //Colors representative if new will be added
    $("#" + element + "_ID").change(function () {
        if ($(this).val() == 0) {
            $('label[for="' + element + "_Name" + '"]').addClass("text-success");
        } else {
            $('label[for="' + element + "_Name" + '"]').removeClass("text-success");
        }
    });
    
    //sets corresponding id for representative
    $("#" + element + "_Name").change(function () {
        var option = $("#" + element + "Representatives option[value='" + $("#" + element + "_Name").val() + "']");
        if (option.length !== 0) {
            $("#" + element + "_Name").val(option.val());
            $("#" + element + "_ID").val(option.data("id"));
        } else {
            $("#" + element + "_ID").val(0);
        }
        $("#" + element + "_ID").change();
    });
}

//methods
function getProductGroups() {
    return $.ajax({
        type: "GET",
        url: "http://localhost:64010/Api/ProductGroup/GetProductGroups",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i].Margin + "' " +
                    "data-ID='" + data[i].ID + "'" +
                    "label = '" + data[i].Name + " " + data[i].Margin + "%'/>";
            }
            ProductGroups = htmlText;
        },
        error: function () {
            ProductGroups = "";
        }
    });
}

function getTaxGroups() {
    return $.ajax({
        type: "GET",
        url: "http://localhost:64010/Api/TaxGroup/GetTaxGroups",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i].Tax + "' " +
                    "data-ID='" + data[i].ID + "'" +
                    "label = '" + data[i].Name + " " + data[i].Tax + "%'/>";
            }
            TaxGroups = htmlText;
        },
        error: function () {
            TaxGroups = "";
        }
    });
}

function addNewElement(rowValue, itemId, productId, productGroupId, taxGroupId, serNumber, productName, barcode, incomingPrice) {
    return "<tr>" +
        "<input data-val='true' data-val-number='The field Invoice_ID must be a number.' data-val-required='The Invoice_ID field is required.' id='Elements_" + rowValue + "__Invoice_ID' name='Elements[" + rowValue + "].Invoice_ID' type='hidden' value='" + $("#ID").val() + "'>" +
        "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_ID' name='Elements[" + rowValue + "].Item.ID' type='hidden' value='" + itemId + "'>" +
        "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_Product_ID' name='Elements[" + rowValue + "].Item.Product.ID' type='hidden' value='" + productId + "'>" +
        "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_Product_ProductGroup_ID' name='Elements[" + rowValue + "].Item.Product.ProductGroup.ID' required='required' type='hidden' value='" + productGroupId + "'>" +
        "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_IncomingTaxGroup_ID' name='Elements[" + rowValue + "].Item.IncomingTaxGroup.ID' required='required' type='hidden' value='" + taxGroupId + "'>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Serial number field is required.' id='Elements_" + rowValue + "__Item_SerNumber' name='Elements[" + rowValue + "].Item.SerNumber' type='text' value='" + serNumber + "'></td>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Product name field is required.' id='Elements_" + rowValue + "__Item_Product_Name' name='Elements[" + rowValue + "].Item.Product.Name' required='required' type='text' value='" + productName + "'></td>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Barcode field is required.' id='Elements_" + rowValue + "__Item_Product_Barcode' name='Elements[" + rowValue + "].Item.Product.Barcode' required='required' type='number' value='" + barcode + "'></td>" +
        "<td><input class='form-control' id='Elements_" + rowValue + "__Price' name='Price' required='required' step='0.01' type='number'></td>" +
        "<td><select class='form-control' id='Elements_" + rowValue + "__IncomingTaxGroup'/></td>" +
        "<td><select class='form-control' id='Elements_" + rowValue + "__ProductGroup'/></td>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-number='The field IncomingPrice must be a number.' data-val-required='The IncomingPrice field is required.' id='Elements_" + rowValue + "__Item_IncomingPrice' min='0' name='Elements[" + rowValue + "].Item.IncomingPrice' required='required' step='0.01' type='number' value='" + incomingPrice + "'></td>" +
        "<td><input type='button' class='btn btn-block' id='Elements_" + rowValue + "__Delete' value='Delete' title='This button removes item, action cant be canceled.'></td>" +
        "</tr>";
}

function TotalAmount() {
    var amount = Number(0);
    $("#itemTable tbody tr").each(function () {
        amount = amount + Number($("#Elements_" + $(this).find("input").attr("name").split("[")[1].split("]")[0] + "__Item_IncomingPrice").val());
    });
    $("#TotalAmount").val(amount + Number($("#Transport").val()) - transport);
}

function onlyNumbers(path) {
    //on keypress checks if value is correct
    $(path + ":input[type='number']").keydown(function (e) {
        if (!((e.keyCode > 95 && e.keyCode < 106)
            || (e.keyCode > 47 && e.keyCode < 58)
            || e.keyCode == 8)) {
            return false;
        }
        return true;
    });
}