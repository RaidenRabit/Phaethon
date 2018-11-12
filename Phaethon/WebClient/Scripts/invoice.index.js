$('.daterange').daterangepicker({ startDate: '01/01/2000'});
//gets companies
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

//index
//change shown date formats
//add search by number
//fix this js file (organize)

//edit
//if create new doesnt color in the begining
//if remove company than representative stends

//fix the path ptoblem
//use headers to pass info

GetInvoices();

//gets invoices
function GetInvoices()
{
    $.ajax({
        type: "GET",
        url: "/InvoiceApi/GetInvoices" +
            "?numOfRecords=" + $("#numOfRecords").val() +
            "&selectedCompany=" + $('input[name=companyOption]:checked').val() +
            "&name=" + $("#companyName").val() +
            "&selectedDate=" + $('input[name=dateOption]:checked').val() +
            "&from=" + $("#dateRange").data('daterangepicker').startDate.format('DD/MM/YYYY') +
            "&to=" + $("#dateRange").data('daterangepicker').endDate.format('DD/MM/YYYY'),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                htmlText += "<tr>" +
                    "<td>" + data[i].Transport + "</td>" +
                    "<td>" + data[i].DocNumber + "</td>" +
                    "<td>" + data[i].PrescriptionDate + "</td>" +
                    "<td>" + data[i].ReceptionDate + "</td>" +
                    "<td>" + data[i].PaymentDate + "</td>" +
                    "<td>" + data[i].Sender.Company.Name + "</td>" +
                    "<td>" + data[i].Receiver.Company.Name + "</td>" +
                    "<td>" +
                    "<a href='/Invoice/Edit/" + data[i].ID + "'>Details</a> |" +
                    "<a data-ajax='true' data-ajax-method='POST' data-ajax-success='window.location.reload()' href='/Invoice/Delete/1" + data[i].ID + "'>Delete</a>" +
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

$("#numOfRecords, #companyName, #dateRange, input[name=companyOption], input[name=dateOption]").change(function () {
    GetInvoices();
});