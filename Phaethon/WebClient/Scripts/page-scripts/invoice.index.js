$(function () {
    //creates date range picker
    $('.daterange').daterangepicker({
        "showDropdowns": true,
        "showWeekNumbers": true,
        "autoApply": true,
        "linkedCalendars": false,
        "startDate": "01/01/2001",
        locale: {
            format: 'DD/MMM/YYYY'
        }
    });
    
    GetCompanies();

    GetInvoices();

    //on search option change get corresponding invoices
    $("#numOfRecords, #regNumber, #invoiceNumber, #dateRange, #company, #sum").change(function() {
        GetInvoices();
    });

    $("#newInvoice").click(function () {
        $("#dialog").dialog({
            title: "Invoice type",
            autoOpen: true,
            modal: true,
            buttons: {
                incoming: function () { window.location.href = "Invoice/Edit/True"; },
                outgoing: function () { window.location.href = "Invoice/Edit/False"; }
            }
        });
    });
});

//gets existing companies
function GetCompanies() {
    $.ajax({
        type: "GET",
        url: "Api/Company/GetCompanies",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<option value='" + data[i].Name + "'/>";
            }
            $("#companies").html(htmlText);
        },
        error: function () {
            $("#companies").html("");
        }
    });
}

//gets invoices
function GetInvoices()
{
    $.ajax({
        type: "GET",
        url: "Api/Invoice/GetInvoices",
        data: {
            numOfRecords: $("#numOfRecords").val(),
            regNumber: $("#regNumber").val(),
            docNumber: $("#docNumber").val(),
            from: $("#dateRange").data('daterangepicker').startDate.format('DD/MM/YYYY'),
            to: $("#dateRange").data('daterangepicker').endDate.format('DD/MM/YYYY'),
            company: $("#company").val(),
            sum: $("#sum").val()
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            var rows = $("#numOfRecords").val();
            for (var i = 0; i < data.length; i++) {
                rows = rows - 1;
                var date;
                var invoiceType;
                var companyName;
                if (data[i].Incoming == true) {
                    date = moment(data[i].ReceptionDate).format('DD-MM-YYYY');
                    invoiceType = incoming;
                    companyName = data[i].Sender.Company.Name;
                } else {
                    date = moment(data[i].PrescriptionDate).format('DD-MM-YYYY');
                    invoiceType = outgoing;
                    companyName = data[i].Receiver.Company.Name;
                }

                htmlText += "<tr>" +
                    "<td>" + data[i].RegNumber + "</td>" +
                    "<td>" + data[i].DocNumber + "</td>" +
                    "<td>" + invoiceType + "</td>" +
                    "<td>" + date + "</td>" +
                    "<td>" + companyName + "</td>" +
                    "<td>" + data[i].Sum + "</td>" +
                    "<td>" + data[i].SumNoTax + "</td>" +
                    "<td>" +
                    "<a href='/Invoice/Edit/" + data[i].ID + "'>" + details + "</a>"+
                    "</td>" +
                    "</tr>";
            }
            while (rows > 0) {
                rows = rows - 1;
                htmlText += "<tr>" +
                    "<td></td>" +
                    "<td></td>" +
                    "<td></td>" +
                    "<td></td>" +
                    "<td></td>" +
                    "<td></td>" +
                    "<td></td>" +
                    "<td></td>" +
                    "</tr>";
            }
            $("#invoiceTable tbody").html(htmlText);
        },
        error: function () {
            $("#invoiceTable tbody").html("");
        }
    });
}