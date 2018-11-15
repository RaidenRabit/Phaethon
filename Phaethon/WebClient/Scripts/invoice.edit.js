﻿var senderElement = "Sender";
var receiverElement = "Receiver";

$(function () {//on code load
    CompanyChange(receiverElement);
    CompanyChange(senderElement);
    
    RepresentativeChange(receiverElement);
    RepresentativeChange(senderElement);
    
    $("#newItemRow").click(function () {
        var lastRow = $('#itemTable tbody tr:last');
        var lastRowValue = parseInt(lastRow.find('input').attr("name").split('[')[1].split(']')[0]) + 1;
        lastRow.after("<tr><input data-val='true' data-val-number='The field Invoice_ID must be a number.' data-val-required='The Invoice_ID field is required.' id='Elements_" + lastRowValue + "__Invoice_ID' name='Elements[" + lastRowValue + "].Invoice_ID' type='hidden' value='0'>" +
            "<input data-val='true' data-val-number='The field Item_ID must be a number.' data-val-required='The Item_ID field is required.' id='Elements_" + lastRowValue + "__Item_ID' name='Elements[" + lastRowValue + "].Item_ID' type='hidden' value='0'>" +
            "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + lastRowValue + "__Item_ID' name='Elements[" + lastRowValue + "].Item.ID' type='hidden' value='0'>" +
            "<input data-val='true' data-val-number='The field Product_ID must be a number.' id='Elements_" + lastRowValue + "__Item_Product_ID' name='Elements[" + lastRowValue + "].Item.Product_ID' type='hidden' value=''>" +
            "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + lastRowValue + "__Item_Product_ID' name='Elements[" + lastRowValue + "].Item.Product.ID' type='hidden' value='0'>" +
            "<input data-val='true' data-val-number='The field ProductGroup_ID must be a number.' id='Elements_" + lastRowValue + "__Item_Product_ProductGroup_ID' name='Elements[" + lastRowValue + "].Item.Product.ProductGroup_ID' type='hidden' value=''>" +
            "<input data-val='true' data-val-number='The field ID must be a number.' data-val-required='The ID field is required.' id='Elements_" + lastRowValue + "__Item_Product_ProductGroup_ID' name='Elements[" + lastRowValue + "].Item.Product.ProductGroup.ID' type='hidden' value='1'>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Product name field is required.' id='Elements_" + lastRowValue + "__Item_Product_Name' name='Elements[" + lastRowValue + "].Item.Product.Name' required='required' type='text' value=''></td>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Barcode field is required.' id='Elements_" + lastRowValue + "__Item_Product_Barcode' name='Elements[" + lastRowValue + "].Item.Product.Barcode' required='required' type='text' value=''></td>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Serial number field is required.' id='Elements_" + lastRowValue + "__Item_SerNumber' name='Elements[" + lastRowValue + "].Item.SerNumber' required='required' type='text' value=''></td>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-required='The Product group name field is required.' id='Elements_" + lastRowValue + "__Item_Product_ProductGroup_Name' name='Elements[" + lastRowValue + "].Item.Product.ProductGroup.Name' required='required' type='text' value=''></td>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-number='The field Price must be a number.' data-val-required='The Price field is required.' id='Elements_" + lastRowValue + "__Item_Price' min='0' name='Elements[" + lastRowValue + "].Item.Price' required='required' step='0.01' type='number' value='0'></td>" +
            "<td><input class='form-control text-box single-line' data-val='true' data-val-number='The field Product group tax must be a number.' data-val-required='The Product group tax field is required.' id='Elements_" + lastRowValue + "__Item_Product_ProductGroup_Tax' name='Elements[" + lastRowValue + "].Item.Product.ProductGroup.Tax' required='required' type='number' value='0'></td>" +
            "<td><input type='number' class='form-control' value='0'></td>" +
            "<td><input type='button' class='btn btn-danger btn-block' value='Delete' onclick='window.location.href='https://stackoverflow.com''></td></tr>");
    });

    $(".date-picker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-20:+0", // You can set the year range as per as your need
        dateFormat: "dd-M-yy"
    });

    $("#Transport").keydown(function (e) {
        if (!((e.keyCode > 95 && e.keyCode < 106)
            || (e.keyCode > 47 && e.keyCode < 58)
            || e.keyCode == 8)) {
            return false;
        }
        return true;
    });
});

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