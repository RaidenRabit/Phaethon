var senderElement = "Sender";
var receiverElement = "Receiver";

$(function () {//on code load
    CompanyChange(receiverElement);
    CompanyChange(senderElement);
    
    RepresentativeChange(receiverElement);
    RepresentativeChange(senderElement);

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