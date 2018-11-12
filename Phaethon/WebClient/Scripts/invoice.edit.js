var senderElement = "Sender";
var receiverElement = "Receiver";

$(function () {//on code load
    CompanyChange(receiverElement);
    CompanyChange(senderElement);
    
    RepresentativeChange(receiverElement);
    RepresentativeChange(senderElement);

    //gets representatives of company
    GetRepresentatives(receiverElement);
    GetRepresentatives(senderElement);

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
            $("#companies").html(htmlText);
        },
        error: function () {
            alert("There was a issue getting companies");
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

    //Shows if new company will be added
    $("#" + element + "_Company_ID").change(function () {
        if ($(this).val() == 0) {
            $("#" + element + "_Company_Name").closest(".form-group").addClass("has-success");
            $("#" + element + "_Company_BankNumber").closest(".form-group").addClass("has-success");
            $("#" + element + "_Company_RegNumber").closest(".form-group").addClass("has-success");
            $("#" + element + "_Company_Address").closest(".form-group").addClass("has-success");
            $("#" + element + "_Company_Location").closest(".form-group").addClass("has-success");
        } else {
            $("#" + element + "_Company_Name").closest(".form-group").removeClass("has-success");
            $("#" + element + "_Company_BankNumber").closest(".form-group").removeClass("has-success");
            $("#" + element + "_Company_RegNumber").closest(".form-group").removeClass("has-success");
            $("#" + element + "_Company_Address").closest(".form-group").removeClass("has-success");
            $("#" + element + "_Company_Location").closest(".form-group").removeClass("has-success");
        }
    });

    //sets companies companies info
    $("#" + element + "_Company_Name").change(function () {
        var option = $("#companies option[value='" + $(this).val() + "']");
        $("#" + element + "_Company_ID").val(option.data('id'));
        $("#" + element + "_Company_Name").val(option.val());
        $("#" + element + "_Company_BankNumber").val(option.data('banknumber'));
        $("#" + element + "_Company_RegNumber").val(option.data('regnumber'));
        $("#" + element + "_Company_Address").val(option.data('address'));
        $("#" + element + "_Company_Location").val(option.data('location'));
        $("#" + element + "_Company_ID").change();
        GetRepresentatives(element);
    });
}

function RepresentativeChange(element) {
    //shows if new representative will be added
    $("#" + element + "_ID").change(function () {
        if ($(this).val() == 0) {
            $("#" + element + "_Name").closest(".form-group").addClass("has-success");
        } else {
            $("#" + element + "_Name").closest(".form-group").removeClass("has-success");
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
            var option = $("#" + element + "Representatives option[data-id='" + $("#" + element + "_ID").val() + "']");
            if (option.length === 0) {
                option = $("#" + element + "Representatives option");
                $("#" + element + "_Name").val(option.val());
                $("#" + element + "_ID").val(option.data("id"));
                $("#" + element + "_ID").change();
            }
        },
        error: function () {
            $("#" + element + "Representatives").html("");
        }
    });
}