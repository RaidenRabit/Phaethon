var senderElement = "Sender";
var receiverElement = "Receiver";

$(function () {//on code load
    CompanyChange(receiverElement);
    CompanyChange(senderElement);
    
    RepresentativeChange(receiverElement);
    RepresentativeChange(senderElement);

    products();

    $(".date-picker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-20:+0", // You can set the year range as per as your need
        dateFormat: "dd-M-yy"
    });
    
    //allows only int values
    onlyNumbers("");
    
    //adds action listeners to existing input fields in items
    $("#itemTable tbody tr").each(function () {
        addActionToItem($(this).find('input').attr("name").split('[')[1].split(']')[0]);
    });
});

function products() {
    //gets all product groups
    $.ajax({
        type: "GET",
        url: "/Api/ProductGroup/GetProductGroups",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i].Name + "' " +
                    "data-ID='" + data[i].ID + "'" +
                    "label = '" + data[i].Margin + "'/>";
            }
            $("#ProductGroups").html(htmlText);
        },
        error: function () {
            $("#ProductGroups").html("");
        }
    });

    //gets all tax groups
    $.ajax({
        type: "GET",
        url: "/Api/TaxGroup/GetTaxGroups",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i].Name + "' " +
                    "data-ID='" + data[i].ID + "'" +
                    "label = '" + data[i].Tax + "'/>";
            }
            $("#TaxGroups").html(htmlText);
        },
        error: function () {
            $("#TaxGroups").html("");
        }
    });

    //adds new item row to table
    $("#newItemRow").click(function () {
        var rowValue;
        if ($('#itemTable tbody tr').length == 0) {
            rowValue = 0;
        } else {
            rowValue = parseInt($('#itemTable tbody tr:last').find('input').attr("name").split('[')[1].split(']')[0]) + 1;
        }
        $("#itemTable tbody").append("<tr>" +
            "<input data-val='true' data-val-number='The field Invoice_ID must be a number.' data-val-required='The Invoice_ID field is required.' id='Elements_" + rowValue + "__Invoice_ID' name='Elements[" + rowValue + "].Invoice.ID' type='hidden' value='" + $("#ID").val() + "'>" +
            "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_ID' name='Elements[" + rowValue + "].Item.ID' type='hidden' value='0'>" +
            "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_Product_ID' name='Elements[" + rowValue + "].Item.Product.ID' type='hidden' value='0'>" +
            "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_Product_ProductGroup_ID' name='Elements[" + rowValue + "].Item.Product.ProductGroup.ID' required='required' type='hidden'>" +
            "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + rowValue + "__Item_IncomingTaxGroup_ID' name='Elements[" + rowValue + "].Item.IncomingTaxGroup.ID' required='required' type='hidden'>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Serial number field is required.' id='Elements_" + rowValue + "__Item_SerNumber' name='Elements[" + rowValue + "].Item.SerNumber' type='text'></td>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Product name field is required.' id='Elements_" + rowValue + "__Item_Product_Name' name='Elements[" + rowValue + "].Item.Product.Name' required='required' type='text'></td>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Barcode field is required.' id='Elements_" + rowValue + "__Item_Product_Barcode' name='Elements[" + rowValue + "].Item.Product.Barcode' required='required' type='number'></td>" +
            "<td><input class='form-control' name='Price' required='required' step='0.01' type='number'></td>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-number='The Tax group name field is required.' data-val-required='The Tax group tax field is required.' id='Elements_" + rowValue + "__Item_IncomingTaxGroup_Name' name='Elements[" + rowValue + "].Item.IncomingTaxGroup.Name' list='TaxGroups' required='required' type='text'></td>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-number='The field IncomingPrice must be a number.' data-val-required='The IncomingPrice field is required.' id='Elements_" + rowValue + "__Item_IncomingPrice' min='0' name='Elements[" + rowValue + "].Item.IncomingPrice' required='required' step='0.01' type='number'></td>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Product group name field is required.' id='Elements_" + rowValue + "__Item_Product_ProductGroup_Name' name='Elements[" + rowValue + "].Item.Product.ProductGroup.Name' list='ProductGroups' required='required' type='text'></td>" +
            "<td><input type='button' class='btn btn-block' value='Delete' title='This button removes item, action cant be canceled.'></td>" +
            "</tr>");

        //search for product by barcode
        addActionToItem(rowValue);

        //allows only int values
        onlyNumbers("#itemTable tbody tr:last ");
    });
}

function addActionToItem(rowValue) {
    //on barcode change get corresponding info in database for product, product group and item
    $("#Elements_" + rowValue + "__Item_Product_Barcode").change(function () {
        $.ajax({
            type: "GET",
            url: "/Api/Product/GetProduct?barcode=" + $(this).val(),
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
                    $("#Elements_" + rowValue + "__Item_Product_ProductGroup_ID").val(data.ProductGroup.ID);
                    $("#Elements_" + rowValue + "__Item_Product_ProductGroup_Name").val(data.ProductGroup.Name);
                } else {
                    $("#Elements_" + rowValue + "__Item_Product_ID").val(0);
                }
            }
        });
    });

    //changes product group
    $("#Elements_" + rowValue + "__Item_Product_ProductGroup_Name").change(function () {
        var option = $("#ProductGroups option[value='" + $(this).val() + "']");
        if (option.length !== 0) {
            $("#Elements_" + rowValue + "__Item_Product_ProductGroup_ID").val(option.data('id'));
        } else {
            $("#Elements_" + rowValue + "__Item_Product_ProductGroup_ID").val(0);
        }
    });

    //changes tax group
    $("#Elements_" + rowValue + "__Item_IncomingTaxGroup_Name").change(function () {
        var option = $("#TaxGroups option[value='" + $(this).val() + "']");
        if (option.length !== 0) {
            $("#Elements_" + rowValue + "__Item_IncomingTaxGroup_ID").val(option.data('id'));
        } else {
            $("#Elements_" + rowValue + "__Item_IncomingTaxGroup_ID").val(0);
        }
    });
    
    //delete button clicked 
    $("#Elements_" + rowValue + "__Item_Product_ProductGroup_Name").closest("tr").find(":input[type='button']").click(function () {
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


    //do work here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    $("#Elements_" + rowValue + "__Item_Product_ProductGroup_Name").closest("tr").find("[name='Price']").change(function () {
        var procent = $("#TaxGroups option[value='" + $("#Elements_" + rowValue + "__Item_IncomingTaxGroup_Name").val() + "']").attr('label') / 100;
        var val = parseFloat($(this).val());
        var price = val + val * procent;
        $("#Elements_" + rowValue + "__Item_IncomingPrice").val(price.toFixed(2));
    });

    $("#Elements_" + rowValue + "__Item_IncomingPrice").change(function () {
        var procent = parseFloat($("#TaxGroups option[value='" + $("#Elements_" + rowValue + "__Item_IncomingTaxGroup_Name").val() + "']").attr('label')) / 100 + 1;
        var val = parseFloat($(this).val());
        var price = val / procent;
        $(this).closest("tr").find("[name='Price']").val(price.toFixed(2));
    });
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

function CompanyChange(element) {
    //gets all companies
    $.ajax({
        type: "GET",
        url: "/Api/Company/GetCompanies",
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
                url: "/Api/Company/GetCompany?id=" + option.data('id'),
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