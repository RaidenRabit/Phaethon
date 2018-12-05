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
    $("#numOfRecords, #companyName, #dateRange, input[name=companyOption], input[name=dateOption], #docNumber").change(function() {
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
            selectedCompany: $('input[name=companyOption]:checked').val(),
            name: $("#companyName").val(),
            selectedDate: $('input[name=dateOption]:checked').val(),
            from: $("#dateRange").data('daterangepicker').startDate.format('DD/MM/YYYY'),
            to: $("#dateRange").data('daterangepicker').endDate.format('DD/MM/YYYY'),
            docNumber: $("#docNumber").val()
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                var PrescriptionDate = moment(data[i].PrescriptionDate).format('DD-MM-YYYY');
                var ReceptionDate = moment(data[i].ReceptionDate).format('DD-MM-YYYY');
                var PaymentDate = moment(data[i].PaymentDate).format('DD-MM-YYYY');
                var invoiceType;
                if (data[i].Incoming == true) {
                    invoiceType = incoming;
                } else {
                    invoiceType = outgoing;
                }
                htmlText += "<tr>" +
                    "<td>" + data[i].DocNumber + "</td>" +
                    "<td>" + invoiceType + "</td>" +
                    "<td>" + PrescriptionDate + "</td>" +
                    "<td>" + ReceptionDate + "</td>" +
                    "<td>" + PaymentDate + "</td>" +
                    "<td>" + data[i].Sender.Company.Name + "</td>" +
                    "<td>" + data[i].Receiver.Company.Name + "</td>" +
                    "<td>" + data[i].Sum + "</td>" +
                    "<td>" +
                    "<a href='/Invoice/Edit/" + data[i].ID + "'>" + details + "</a> |" +
                    "<a data-ajax='true' data-ajax-method='POST' data-ajax-success='window.location.reload()' href='/Invoice/Delete/" + data[i].ID + "'>" + deleteLabel + "</a>" +
                    "</td>" +
                    "</tr>";
            }
            $("#invoiceTable tbody").html(htmlText);
        },
        error: function () {
            $("#invoiceTable tbody").html("");
        }
    });
}