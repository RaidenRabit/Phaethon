//On load
$(function () {
    //sets action listener to company
    CompanyChange("Receiver");
    CompanyChange("Sender");

    //allows only int values for all number fields created in right now
    onlyNumbers("");

    //initializes datepicker
    $(".date-picker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-20:+0", // You can set the year range as per as your need
        dateFormat: "dd-M-yy"
    });
});

function CompanyChange(element) {
    //gets all companies
    getCompanies();

    //sets info to create new company
    $("#reset" + element).click(function () {
        //representative
        $("#" + element + "_ID").val(0);
        $("#" + element + "_Name").val("");
        //company
        $("#" + element + "_Company_ID").val(0);
        $("#" + element + "_Company_Name").val("");
        $("#" + element + "_Company_BankNumber").val("");
        $("#" + element + "_Company_RegNumber").val("");
        //Address
        $("#" + element + "_Company_Address_ID").val(0);
        $("#" + element + "_Company_Address_City").val("");
        $("#" + element + "_Company_Address_Street").val("");
        $("#" + element + "_Company_Address_Number").val("");
        $("#" + element + "_Company_Address_Extra").val("");
        //Location
        $("#" + element + "_Company_Location_ID").val(0);
        $("#" + element + "_Company_Location_City").val("");
        $("#" + element + "_Company_Location_Street").val("");
        $("#" + element + "_Company_Location_Number").val("");
        $("#" + element + "_Company_Location_Extra").val("");

        $("#" + element + "_ID").change();
        $("#" + element + "_Company_ID").change();
    });

    //Colors company if new will be added
    $("#" + element + "_Company_ID").change(function () {
        if ($(this).val() == 0) {
            //company
            $('label[for="' + element + "_Company_Name" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_BankNumber" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_RegNumber" + '"]').addClass("text-success");
            //Address
            $('label[for="' + element + "_Company_Address_City" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_Address_Street" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_Address_Number" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_Address_Extra" + '"]').addClass("text-success");
            //Location
            $('label[for="' + element + "_Company_Location_City" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_Location_Street" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_Location_Number" + '"]').addClass("text-success");
            $('label[for="' + element + "_Company_Location_Extra" + '"]').addClass("text-success");
        } else {
            //company
            $('label[for="' + element + "_Company_Name" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_BankNumber" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_RegNumber" + '"]').removeClass("text-success");
            //Address
            $('label[for="' + element + "_Company_Address_City" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_Address_Street" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_Address_Number" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_Address_Extra" + '"]').removeClass("text-success");
            //Location
            $('label[for="' + element + "_Company_Location_City" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_Location_Street" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_Location_Number" + '"]').removeClass("text-success");
            $('label[for="' + element + "_Company_Location_Extra" + '"]').removeClass("text-success");
        }
        $("#" + element + "_ID").change();
    });

    //sets selected companies companies info
    $("#" + element + "_Company_Name").change(function () {
        var option = $("#companies option[value='" + $(this).val() + "']");
        if (option.length !== 0) {
            getCompany(element, option.data("id"));
        } else {
            $("#" + element + "_ID").val(0);
            $("#" + element + "_Company_ID").val(0);
            $("#" + element + "Representatives").html("");

            $("#" + element + "_Company_ID").change();
        }
    });

    //sets action listener to representative
    RepresentativeChange(element);
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

//Get info
function getCompanies() {
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
}

function getCompany(element, id) {
    $.ajax({
        type: "GET",
        url: "/Api/Company/GetCompany",
        data: {
            id: id
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //company
            $("#" + element + "_Company_ID").val(data.ID);
            $("#" + element + "_Company_Name").val(data.Name);
            $("#" + element + "_Company_BankNumber").val(data.BankNumber);
            $("#" + element + "_Company_RegNumber").val(data.RegNumber);
            //Address
            $("#" + element + "_Company_Address_ID").val(data.Address.ID);
            $("#" + element + "_Company_Address_City").val(data.Address.City);
            $("#" + element + "_Company_Address_Street").val(data.Address.Street);
            $("#" + element + "_Company_Address_Number").val(data.Address.Number);
            $("#" + element + "_Company_Address_Extra").val(data.Address.Extra);
            //Location
            $("#" + element + "_Company_Location_ID").val(data.Location.ID);
            $("#" + element + "_Company_Location_City").val(data.Location.City);
            $("#" + element + "_Company_Location_Street").val(data.Location.Street);
            $("#" + element + "_Company_Location_Number").val(data.Location.Number);
            $("#" + element + "_Company_Location_Extra").val(data.Location.Extra);

            //sets the companies representatives
            var htmlText = "";
            for (var i = 0; i < data.Representatives.length; i++) {
                htmlText += "<option value='" + data.Representatives[i].Name + "'" + "data-ID='" + data.Representatives[i].ID + "'/>";
            }
            $("#" + element + "Representatives").html(htmlText);

            //selects the selected representative
            var option = $("#" + element + "Representatives option[data-id='" + $("#" + element + "_ID").val() + "']");
            if (option.length === 0) {
                option = $("#" + element + "Representatives option");
                $("#" + element + "_Name").val(option.val());
                $("#" + element + "_ID").val(option.data("id"));
            }

            $("#" + element + "_Company_ID").change();
        },
        error: function () {
            //representative
            $("#" + element + "_ID").val(0);
            $("#" + element + "_Name").val("");
            //company
            $("#" + element + "_Company_ID").val(0);
            $("#" + element + "_Company_Name").val("");
            $("#" + element + "_Company_BankNumber").val("");
            $("#" + element + "_Company_RegNumber").val("");
            //Address
            $("#" + element + "_Company_Address_ID").val(0);
            $("#" + element + "_Company_Address_City").val("");
            $("#" + element + "_Company_Address_Street").val("");
            $("#" + element + "_Company_Address_Number").val("");
            $("#" + element + "_Company_Address_Extra").val("");
            //Location
            $("#" + element + "_Company_Location_ID").val(0);
            $("#" + element + "_Company_Location_City").val("");
            $("#" + element + "_Company_Location_Street").val("");
            $("#" + element + "_Company_Location_Number").val("");
            $("#" + element + "_Company_Location_Extra").val("");

            $("#" + element + "Representatives").html("");

            $("#" + element + "_Company_ID").change();
        }
    });
}