var senderElement = "Sender";
var receiverElement = "Receiver";

$(function () {//on code load
    //clears fields for company and representative
    NewCompany(senderElement);
    NewCompany(receiverElement);
    
    //gets all companies
    $.ajax({
        type: "GET",
        url: "/Company/GetCompanies",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i].Name + "' " +
                    "data-ID='" + data[i].ID + "'" +
                    "data-RegNumber='" + data[i].RegNumber + "'" +
                    "data-Location='" + data[i].Location + "'" +
                    "data-Address='" + data[i].Address + "'" +
                    "data-BankNumber='" + data[i].BankNumber + "'/>";
            }
            $("#Companies").html(htmlText);
        },
        error: function () {
            alert("There was a issue getting companies");
        }
    });

    //Gets selected companies name
    CompanyChange(receiverElement);
    CompanyChange(senderElement);

    //gets representatives of company
    GetRepresentatives(receiverElement);
    GetRepresentatives(senderElement);
});

function NewCompany(element) {
    $("#reset" + element).click(function () {
        $("#" + element + "_ID").val(0);
        $("#" + element + "_Name").val("");
        $("#" + element + "_Company_ID").val(0);
        $("#" + element + "_Company_Name").val("");
        $("#" + element + "_Company_BankNumber").val("");
        $("#" + element + "_Company_RegNumber").val("");
        $("#" + element + "_Company_Address").val("");
        $("#" + element + "_Company_Location").val("");
    });
}

function CompanyChange(element) {
    $("#" + element + "_Company_Name").change(function () {
        var option = $("#Companies option[value='" + $(this).val() + "']");
        $("#" + element + "_Company_ID").val(option.data('id'));
        $("#" + element + "_Company_Name").val(option.val());
        $("#" + element + "_Company_BankNumber").val(option.data('banknumber'));
        $("#" + element + "_Company_RegNumber").val(option.data('regnumber'));
        $("#" + element + "_Company_Address").val(option.data('address'));
        $("#" + element + "_Company_Location").val(option.data('location'));
        GetRepresentatives(element);
    });
}

function GetRepresentatives(element) {
    $.ajax({
        type: "GET",
        url: "/Representative/GetRepresentatives?id=" + $("#" + element + "_Company_ID").val(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i].Name + "'" +
                    "data-ID='" + data[i].ID + "'/>";
            }
            $("#" + element + "Representatives").html(htmlText);
            $("#" + element + "_Name").val(document.getElementById(element + "Representatives").options[0].value);
        },
        error: function () {
            alert("There was a issue getting representatives");
        }
    });
}