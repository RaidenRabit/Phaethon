var url = "http://localhost:64007";
var senderElement = "Sender";
var receiverElement = "Receiver";

var ProductGroups;
var TaxGroups;
var transport;//saves already saved transport cost for calculations

//On load
$(function () {
    //sets initial transport cost, to know what was added
    transport = parseFloat($("#Transport").val());

    //sets action listener to company
    CompanyChange(receiverElement);
    CompanyChange(senderElement);

    //sets action listener to representative
    RepresentativeChange(receiverElement);
    RepresentativeChange(senderElement);

    //allows only int values for all fields filled in right now
    onlyNumbers("");

    //initializes datepicker
    $(".date-picker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-20:+0", // You can set the year range as per as your need
        dateFormat: "dd-M-yy"
    });

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
        var rowValue;
        if ($("#itemTable tbody tr").length == 0) {
            rowValue = 0;
        } else {
            rowValue = parseInt($("#itemTable tbody tr:last").find("input").attr("name").split("[")[1].split("]")[0]) + 1;
        }
        $("#itemTable tbody").append(addNewElement(rowValue, 0, 0, "", "", "", 0));

        //search for product by barcode
        ItemChange(rowValue);
        $("#Elements_" + rowValue + "__ProductGroup").change();
        $("#Elements_" + rowValue + "__IncomingTaxGroup").change();
    });

    //if transport cost changes
    $("#Transport").change(function () {
        totalAmount();
    });

    //on tax group label click will open dialog
    $("#taxGroupLabel").click(function () {
        $("#dialog").dialog({ title: "Tax group", autoOpen: false, modal: true, buttons: { "Save": function () { taxGroupForm(); } } });
        getTaxGroupForm();
    });

    //on product group label click will open dialog
    $("#productGroupLabel").click(function () {
        $("#dialog").dialog({ title: "Product group", autoOpen: false, modal: true, buttons: { "Save": function () { productGroupForm(); } } });
        getProductGroupForm();
    });
}

function ItemChange(rowValue) {
    elementSetUp(rowValue);

    //on barcode change get corresponding info in database for product, product group and item
    $("#Elements_" + rowValue + "__Item_Product_Barcode").change(function () {
        getProduct(rowValue, this);
    });

    //changes product group
    $("#Elements_" + rowValue + "__ProductGroup").change(function () {
        var productId = $("#Elements_" + rowValue + "__Item_Product_ID").val();
        var productGroupId = $(this).val();
        $("#itemTable tbody tr").each(function () {
            var row = $(this).find("input").attr("name").split("[")[1].split("]")[0];
            if (productId == $("#Elements_" + row + "__Item_Product_ID").val()) {
                $("#Elements_" + row + "__ProductGroup").val(productGroupId);
            }
        });
    });
    
    //changes tax group
    $("#Elements_" + rowValue + "__IncomingTaxGroup").change(function () {
        $("#Elements_" + rowValue + "__Item_IncomingPrice").change();
    });
    
    //Price without tax changed
    $("#Elements_" + rowValue + "__Price").change(function () {
        var option = $("#Elements_" + rowValue + "__IncomingTaxGroup option[value='" + $("#Elements_" + rowValue + "__IncomingTaxGroup").val() + "']");
        var procent = option.data("tax") / 100;
        var val = parseFloat($(this).val());
        var price = val + val * procent;
        $("#Elements_" + rowValue + "__Item_IncomingPrice").val(price.toFixed(2));
        totalAmount();
    });

    //Incoming price changed
    $("#Elements_" + rowValue + "__Item_IncomingPrice").change(function () {
        var option = $("#Elements_" + rowValue + "__IncomingTaxGroup option[value='" + $("#Elements_" + rowValue + "__IncomingTaxGroup").val() + "']");
        var procent = option.data("tax") / 100 + 1;
        var val = parseFloat($(this).val());
        var price = val / procent;
        $("#Elements_" + rowValue + "__Price").val(price.toFixed(2));
        totalAmount();
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
    getCompanies();

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
            getCompany(element, option);
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

//Gets info
function getProductGroups() {
    return $.ajax({
        type: "GET",
        url: url + "/ProductGroup/GetProductGroups",
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
        url: url + "/TaxGroup/GetTaxGroups",
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

function getCompanies() {
    $.ajax({
        type: "GET",
        url: url + "/Company/GetCompanies",
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
}

function getProduct(rowValue, obj) {
    $.ajax({
        type: "GET",
        url: url + "/Product/GetProduct",
        data: {
            barcode: $(obj).val()
        },
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

                var productId = $("#Elements_" + rowValue + "__Item_Product_ID").val();
                $("#itemTable tbody tr").each(function () {
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
        url: url + "/Element/GetInvoiceElements",
        data: {
            id: $("#ID").val()
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $("#itemTable tbody").append(addNewElement(i, data[i].Item.ID, data[i].Item.Product.ID, data[i].Item.SerNumber, data[i].Item.Product.Name, data[i].Item.Product.Barcode, data[i].Item.IncomingPrice));
                ItemChange(i);
                $("#Elements_" + i + "__ProductGroup").val(data[i].Item.Product.ProductGroup.ID);
                $("#Elements_" + i + "__IncomingTaxGroup").val(data[i].Item.IncomingTaxGroup.ID);
                $("#Elements_" + i + "__Item_IncomingPrice").change();
            }
            totalAmount();
        }
    });
}

function getCompany(element, option) {
    $.ajax({
        type: "GET",
        url: url + "/Company/GetCompany",
        data: {
            id: option.data("id")
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
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
        error: function () {
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
}

function getTaxGroupForm() {
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

//Posts info
function productGroupForm() {
    $.ajax({
        type: "POST",
        url: "/ProductGroup/Create",
        data: $("#productGroupForm").serialize(),
        success: function () {
            $("#dialog").html("");
            $("#dialog").dialog("close");
            $.when(getProductGroups()).done(function () {
                $("#itemTable tbody tr").each(function () {
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
                $("#itemTable tbody tr").each(function () {
                    var rowValue = $(this).find("input").attr("name").split("[")[1].split("]")[0];
                    var taxGroupId = $("#Elements_" + rowValue + "__IncomingTaxGroup").val();
                    $("#Elements_" + rowValue + "__IncomingTaxGroup").html(ProductGroups);
                    $("#Elements_" + rowValue + "__IncomingTaxGroup").val(taxGroupId);
                });
            });
        }
    });
}

//Methods
function elementSetUp(rowValue) {
    $("#Elements_" + rowValue + "__IncomingTaxGroup").html(TaxGroups);
    $("#Elements_" + rowValue + "__ProductGroup").html(ProductGroups);

    //allows only int values in last row
    onlyNumbers("#itemTable tbody tr:last ");
}

function totalAmount() {
    var amount = parseFloat(0);
    $("#itemTable tbody tr").each(function () {
        amount = amount + parseFloat($("#Elements_" + $(this).find("input").attr("name").split("[")[1].split("]")[0] + "__Item_IncomingPrice").val());
    });
    $("#totalAmount").val(parseFloat(amount + parseFloat($("#Transport").val()) - transport).toFixed(2));
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

//Table row info
function addNewElement(rowValue, itemId, productId, serNumber, productName, barcode, incomingPrice) {
    return "<tr>" +
        "<input data-val='true' data-val-number='The field Invoice_ID must be a number.' data-val-required='The Invoice_ID field is required.' id='Elements_" + rowValue + "__Invoice_ID' name='Elements[" + rowValue + "].Invoice_ID' type='hidden' value='" + $("#ID").val() + "'>" +
        "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_ID' name='Elements[" + rowValue + "].Item.ID' type='hidden' value='" + itemId + "'>" +
        "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_Product_ID' name='Elements[" + rowValue + "].Item.Product.ID' type='hidden' value='" + productId + "'>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Serial number field is required.' id='Elements_" + rowValue + "__Item_SerNumber' name='Elements[" + rowValue + "].Item.SerNumber' type='text' value='" + serNumber + "'></td>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Product name field is required.' id='Elements_" + rowValue + "__Item_Product_Name' name='Elements[" + rowValue + "].Item.Product.Name' required='required' type='text' value='" + productName + "'></td>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Barcode field is required.' id='Elements_" + rowValue + "__Item_Product_Barcode' name='Elements[" + rowValue + "].Item.Product.Barcode' required='required' type='number' value='" + barcode + "'></td>" +
        "<td><input class='form-control' id='Elements_" + rowValue + "__Price' name='Price' required='required' step='0.01' type='number'></td>" +
        "<td><select class='form-control' data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__IncomingTaxGroup' name='Elements[" + rowValue + "].Item.IncomingTaxGroup_ID' required='required'></select></td>" +
        "<td><select class='form-control' data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__ProductGroup' name='Elements[" + rowValue + "].Item.Product.ProductGroup_ID' required='required'></select></td>" +
        "<td><input class='form-control text-box single-line' data-val='true' data-val-number='The field IncomingPrice must be a number.' data-val-required='The IncomingPrice field is required.' id='Elements_" + rowValue + "__Item_IncomingPrice' min='0' name='Elements[" + rowValue + "].Item.IncomingPrice' required='required' step='0.01' type='number' value='" + incomingPrice + "'></td>" +
        "<td><input type='button' class='btn btn-block' id='Elements_" + rowValue + "__Delete' value='Delete' title='This button removes item, action cant be canceled.'></td>" +
        "</tr>";
}