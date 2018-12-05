var invoiceType;
var incoming;

var ProductGroups;
var TaxGroups;
var transport;//saves already saved transport cost for calculations

//On load
$(function () {
    //sets if invoice is incoming or outgoing
    if ($("#Incoming").val() == "True") {
        incoming = true;
        invoiceType = "Incoming";
    } else {
        incoming = false;
        invoiceType = "Outgoing";
    }

    //sets initial transport cost, to know what was added
    transport = parseFloat($("#Transport").val());
    
    //Adds action listener to element table header and foot
    elementTableChange();

    //gets getProductGroups and getTaxGroups, than adds all Items in invoice to table
    $.when(getProductGroups(), getTaxGroups()).done(function () {
        getInvoiceItems();
    });
});

//Change listeners
function elementTableChange() {
    //adds new item row to table
    $("#newItemRow").click(function () {
        if (incoming) {
            var rowValue;
            if ($("#elementTable tbody tr").length == 0) {
                rowValue = 0;
            } else {
                rowValue = parseInt($("#elementTable tbody tr:last").find("input").attr("name").split("[")[1].split("]")[0]) + 1;
            }
            addNewElement(rowValue, 0, 0, 1, "", "", "", 0, 0, "", 0, 0, "True");
        } else {
            getSelectItemForm();
        }
    });

    //if transport cost changes
    $("#Transport").change(function () {
        totalAmount();
    });

    //on tax group label click will open dialog
    $("#taxGroupLabel").click(function () {
        getTaxGroupForm();
    });

    //on product group label click will open dialog
    $("#productGroupLabel").click(function () {
        getProductGroupForm();
    });
}

function ItemChange(rowValue) {
    elementSetUp(rowValue);
    
    //changes product group
    $("#Elements_" + rowValue + "__ProductGroup").change(function () {
        var productId = $("#Elements_" + rowValue + "__Item_Product_ID").val();
        var productGroupId = $(this).val();
        $("#elementTable tbody tr").each(function () {
            var row = $(this).find("input").attr("name").split("[")[1].split("]")[0];
            if (productId == $("#Elements_" + row + "__Item_Product_ID").val()) {
                $("#Elements_" + row + "__ProductGroup").val(productGroupId);
                if (!incoming) {
                    $("#Elements_" + row + "__Price").change();
                }
            }
        });
    });
    
    //Price without tax changed
    $("#Elements_" + rowValue + "__Price").change(function () {
        //gets tax in %
        var tax = $("#Elements_" + rowValue + "__" + invoiceType + "TaxGroup option[value='" + $("#Elements_" + rowValue + "__" + invoiceType + "TaxGroup").val() + "']").data("tax") / 100;
        var val = parseFloat($(this).val());
        if (!incoming) {
            //if invoice is outgoing add companies margin
            val = val + val * ($("#Elements_" + rowValue + "__ProductGroup option[value='" + $("#Elements_" + rowValue + "__ProductGroup").val() + "']").data("margin") / 100);
        }
        //add tax to item
        var price = val + val * tax;
        $("#Elements_" + rowValue + "__Item_" + invoiceType + "Price").val(price.toFixed(2));
        totalAmount();
    });

    if (incoming) {
        //on barcode change get corresponding info in database for product, product group and item
        $("#Elements_" + rowValue + "__Item_Product_Barcode").change(function () {
            getProduct(rowValue, $(this).val());
        });

        //Price, tax or quantity changed
        $("#Elements_" + rowValue + "__Item_" + invoiceType + "Price, #Elements_" + rowValue + "__" + invoiceType + "TaxGroup, #Elements_" + rowValue + "__Item_Quantity").change(function () {
            //gets tax in % and than adds 1, so tax can be removed from item
            var tax = $("#Elements_" + rowValue + "__" + invoiceType + "TaxGroup option[value='" + $("#Elements_" + rowValue + "__" + invoiceType + "TaxGroup").val() + "']").data("tax") / 100 + 1;
            var val = parseFloat($("#Elements_" + rowValue + "__Item_" + invoiceType + "Price").val());
            //removes tax from value
            var price = val / tax;
            $("#Elements_" + rowValue + "__Price").val(price.toFixed(2));
            totalAmount();
        });
    }
    else {
        //!! quantity should be added for outgoing

        //tax or quantity changed
        $("#Elements_" + rowValue + "__" + invoiceType + "TaxGroup").change(function () {
            $("#Elements_" + rowValue + "__Price").change();
        });

        //Price, tax or quantity changed
        $("#Elements_" + rowValue + "__Item_" + invoiceType + "Price").change(function () {
            totalAmount();
        });
    }

    //delete button clicked 
    $("#Elements_" + rowValue + "__Delete").click(function () {
        if ($(this).data("deletable") == "True") {
            var row = $(this).closest("tr");
            row.remove();
        }
        else {
            var obj = $("#Elements_" + rowValue + "__Item_Delete");
            if (obj.val() == "false") {
                obj.val("true");
                $(this).addClass("btn-danger");
            } else {
                obj.val("false");
                $(this).removeClass("btn-danger");
            }
        }
    });
}

//Gets info
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
            TaxGroups = htmlText;
        },
        error: function () {
            TaxGroups = "";
        }
    });
}

function getProduct(rowValue, barcode) {
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
                    var price = $("#Elements_" + rowValue + "__Item_" + invoiceType + "Price");
                    if (incoming) {
                        price.val(data.Items[0].IncomingPrice);
                    } else {
                        price.val(data.Items[0].OutgoingPrice);
                    }
                    $("#Elements_" + rowValue + "__Item_" + invoiceType + "Price").change();
                }
                $("#Elements_" + rowValue + "__Item_Product_Name").val(data.Name);
                $("#Elements_" + rowValue + "__Item_Product_ID").val(data.ID);

                var productId = $("#Elements_" + rowValue + "__Item_Product_ID").val();
                $("#elementTable tbody tr").each(function () {
                    var row = $(this).find("input").attr("name").split("[")[1].split("]")[0];
                    if (productId == $("#Elements_" + row + "__Item_Product_ID").val()) {
                        var productGroupId = $("#Elements_" + row + "__ProductGroup").val();
                        $("#Elements_" + rowValue + "__ProductGroup").val(productGroupId);
                    }
                });
            } else {
                $("#Elements_" + rowValue + "__Item_Product_ID").val(0);
            }
        }
    });
}

function getInvoiceItems() {
    $.ajax({
        type: "GET",
        url: "/Api/Element/GetInvoiceElements",
        data: {
            id: $("#ID").val()
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var price;
                var taxGroup;
                var noTaxPrice;
                var readonly;
                if (incoming) {
                    price = data[i].Item.IncomingPrice;
                    taxGroup = data[i].Item.IncomingTaxGroup_ID;
                    noTaxPrice = 0;
                    readonly = "";
                } else {
                    price = data[i].Item.OutgoingPrice;
                    taxGroup = data[i].Item.OutgoingTaxGroup_ID;
                    noTaxPrice = data[i].Item.IncomingPrice;
                    readonly = "readonly";
                }
                addNewElement(i,
                    data[i].Item.ID,
                    data[i].Item.Product.ID,
                    data[i].Item.Quantity,
                    data[i].Item.SerNumber,
                    data[i].Item.Product.Name,
                    data[i].Item.Product.Barcode,
                    price,
                    noTaxPrice,
                    readonly,
                    data[i].Item.Product.ProductGroup_ID,
                    taxGroup,
                    "False");
            }
            totalAmount();
        }
    });
}

function getItem(id) {
    var exist = false;
    $("#elementTable tbody tr").each(function () {
        var row = $(this).find("input").attr("name").split("[")[1].split("]")[0];
        if ($("#Elements_" + row + "__Item_ID").val() == id) {
            exist = true;
        }
    });

    if (!exist) {
        var rowValue;
        if ($("#elementTable tbody tr").length == 0) {
            rowValue = 0;
        } else {
            rowValue =
                parseInt($("#elementTable tbody tr:last").find("input").attr("name").split("[")[1].split("]")[0]) + 1;
        }

        $.ajax({
            type: "GET",
            url: "/Api/Item/GetItem",
            data: {
                id: id
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(data) {
                addNewElement(rowValue,
                    data.ID,
                    data.Product.ID,
                    data.Quantity,
                    data.SerNumber,
                    data.Product.Name,
                    data.Product.Barcode,
                    data.OutgoingPrice,
                    data.IncomingPrice,
                    "readonly",
                    data.Product.ProductGroup_ID,
                    data.OutgoingTaxGroup_ID,
                    "True");
                totalAmount();
            }
        });
    }
}

//Get form
function getTaxGroupForm() {
    $("#dialog").dialog({
        title: "Tax group",
        autoOpen: false,
        modal: true,
        buttons: { "Save": function () { taxGroupForm(); } }
    });

    $.ajax({
        type: "GET",
        url: "/TaxGroup/Create",
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $("#dialog").html(data);
            $("#dialog").dialog("open");
        }
    });
}

function getProductGroupForm() {
    $("#dialog").dialog({
        title: "Product group",
        autoOpen: false,
        modal: true,
        buttons: { "Save": function () { productGroupForm(); } }
    });

    $.ajax({
        type: "GET",
        url: "/ProductGroup/Create",
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $("#dialog").html(data);
            $("#dialog").dialog("open");
        }
    });
}

function getSelectItemForm() {
    $("#dialog").dialog({
        title: "Item selection",
        autoOpen: false,
        modal: true,
        height: 500,
        width: 1000
    });

    $.ajax({
        type: "GET",
        url: "/Item/Select",
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (data) {
            $("#dialog").html(data);
            $("#dialog").dialog("open");
        }
    });
}

//Posts form
function productGroupForm() {
    $.ajax({
        type: "POST",
        url: "/ProductGroup/Create",
        data: $("#productGroupForm").serialize(),
        success: function () {
            $("#dialog").html("");
            $("#dialog").dialog("close");
            $.when(getProductGroups()).done(function () {
                $("#elementTable tbody tr").each(function () {
                    var rowValue = $(this).find("input").attr("name").split("[")[1].split("]")[0];
                    var productGroupId = $("#Elements_" + rowValue + "__ProductGroup").val();
                    $("#Elements_" + rowValue + "__ProductGroup").html(ProductGroups);
                    $("#Elements_" + rowValue + "__ProductGroup").val(productGroupId);
                });
            });
        }
    });
}

function taxGroupForm() {
    $.ajax({
        type: "POST",
        url: "/TaxGroup/Create",
        data: $("#taxGroupForm").serialize(),
        success: function () {
            $("#dialog").html("");
            $("#dialog").dialog("close");
            $.when(getTaxGroups()).done(function () {
                $("#elementTable tbody tr").each(function () {
                    var rowValue = $(this).find("input").attr("name").split("[")[1].split("]")[0];
                    var taxGroupId = $("#Elements_" + rowValue + "__" + invoiceType + "TaxGroup").val();
                    $("#Elements_" + rowValue + "__" + invoiceType + "TaxGroup").html(ProductGroups);
                    $("#Elements_" + rowValue + "__" + invoiceType + "TaxGroup").val(taxGroupId);
                });
            });
        }
    });
}

//Methods
function elementSetUp(rowValue) {
    $("#Elements_" + rowValue + "__" + invoiceType + "TaxGroup").html(TaxGroups);
    $("#Elements_" + rowValue + "__ProductGroup").html(ProductGroups);

    //allows only int values in last row
    onlyNumbers("#elementTable tbody tr:last ");
}

function totalAmount() {
    var amount = parseFloat(0);
    var amountNoTax = parseFloat(0);
    $("#elementTable tbody tr").each(function () {
        var row = $(this).find("input").attr("name").split("[")[1].split("]")[0];
        var quantity = parseFloat($("#Elements_" + row + "__Item_Quantity").val());
        amount = amount + parseFloat($("#Elements_" + row + "__Item_" + invoiceType + "Price").val()) * quantity;
        amountNoTax = amountNoTax + parseFloat($("#Elements_" + row + "__Price").val()) * quantity;
    });
    $("#totalAmount").val(parseFloat(amount + parseFloat($("#Transport").val()) - transport).toFixed(2));
    $("#totalNoTax").val(amountNoTax.toFixed(2));
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

function addNewElement(rowValue, itemId, productId, quantity, serNumber, productName, barcode, price, noTaxPrice, readonly, productGroupId, taxGroupId, deletable) {
    $("#elementTable tbody").append("<tr>" +
        "<input data-val='true' data-val-number='The Delete must be a boolean.' data-val-required='The Delete field is required.' id='Elements_" + rowValue + "__Item_Delete' name='Elements[" + rowValue + "].Item.Delete' type='hidden' value='false'>" +
        "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_ID' name='Elements[" + rowValue + "].Item.ID' type='hidden' value='" + itemId + "'>" +
        "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_Product_ID' name='Elements[" + rowValue + "].Item.Product.ID' type='hidden' value='" + productId + "'>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Quantity field is required.' id='Elements_" + rowValue + "__Item_Quantity' name='Elements[" + rowValue + "].Item.Quantity' type='number' min='0' value='" + quantity + "'></td>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Serial number field is required.' id='Elements_" + rowValue + "__Item_SerNumber' name='Elements[" + rowValue + "].Item.SerNumber' type='text' value='" + serNumber + "' " + readonly + "></td>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Product name field is required.' id='Elements_" + rowValue + "__Item_Product_Name' name='Elements[" + rowValue + "].Item.Product.Name' required='required' type='text' value='" + productName + "' " + readonly + "></td>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Barcode field is required.' id='Elements_" + rowValue + "__Item_Product_Barcode' name='Elements[" + rowValue + "].Item.Product.Barcode' required='required' type='number' value='" + barcode + "' " + readonly + "></td>" +
        "<td><input class='form-control' id='Elements_" + rowValue + "__Price' name='Price' required='required' step='0.01' type='number' value='" + noTaxPrice + "' " + readonly + "></td>" +
        "<td><select class='form-control' data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__" + invoiceType + "TaxGroup' name='Elements[" + rowValue + "].Item." + invoiceType + "TaxGroup_ID' required='required'></select></td>" +
        "<td><select class='form-control' data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__ProductGroup' name='Elements[" + rowValue + "].Item.Product.ProductGroup_ID' required='required'></select></td>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-number='The field " + invoiceType + "Price must be a number.' data-val-required='The " + invoiceType + "Price field is required.' id='Elements_" + rowValue + "__Item_" + invoiceType + "Price' name='Elements[" + rowValue + "].Item." + invoiceType + "Price' required='required' min='0' step='0.01' type='number' value='" + price + "'></td>" +
        "<td><input type='button' class='btn btn-block' id='Elements_" + rowValue + "__Delete' value='Delete' title='This button removes item, action cant be canceled.' data-deletable='" + deletable + "'></td>" +
        "</tr>");
    ItemChange(rowValue);
    $("#Elements_" + rowValue + "__ProductGroup").val(productGroupId);
    $("#Elements_" + rowValue + "__" + invoiceType + "TaxGroup").val(taxGroupId);
    if (incoming) {
        $("#Elements_" + rowValue + "__Item_" + invoiceType + "Price").change();
    } else {
        if (price == 0) {
            $("#Elements_" + rowValue + "__Price").change();
        }
    }
}