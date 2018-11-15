﻿$(function () {
    //creates date range picker
    $('#dateRange').daterangepicker({
        "showDropdowns": true,
        "showWeekNumbers": true,
        "autoApply": true,
        "linkedCalendars": false,
        locale: {
            format: 'DD/MMM/YYYY'
        }
    });

    //gets existing companies
    //$.ajax({
    //    type: "GET",
    //    url: "/Api/Company/GetCompanies",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        var htmlText = "";
    //        for (var i = 0; i < data.length; i++) {
    //            htmlText += "<option value='" + data[i].Name + "'/>";
    //        }
    //        $("#companies").html(htmlText);
    //    },
    //    error: function () {
    //        $("#companies").html("");
    //    }
    //});

    GetJobs();

    //on search option change get corresponding invoices
    $("#numOfRecords, #jobId, #jobName, #dateRange, input[name=jobStatus], input[name=dateOption], #customerName").change(function () {
        GetJobs();
    });
});

//gets invoices
function GetJobs() {
    $.ajax({
        type: "GET",
        url: "/Api/Job/ReadAll" +
            "&jobId=" + $("#jobId").val() +
            "&jobName=" + $("#jobName").val() +
            "?numOfRecords=" + $("#numOfRecords").val() +
            "&jobStatus=" + $('input[name=jobStatus]:checked').val() +
            "&dateOption=" + $('input[name=dateOption]:checked').val() +
            "&from=" + $("#dateRange").data('daterangepicker').startDate.format('DD/MM/YYYY') +
            "&to=" + $("#dateRange").data('daterangepicker').endDate.format('DD/MM/YYYY') +
            "&customerName=" + $("#customerName").val(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
            for (var i = 0; i < data.length; i++) {
                var StartedTime = moment(data[i].PrescriptionDate).format('DD-MM-YYYY');
                var FinishedTime = moment(data[i].ReceptionDate).format('DD-MM-YYYY');
                var CustomerName = data[i].Customer.FamilyName + " " + data[i].Customer.GivenName;
                htmlText += "<tr>" +
                    "<td>" + data[i].ID + "</td>" +
                    "<td>" + data[i].JobName + "</td>" +
                    "<td>" + StartedTime + "</td>" +
                    "<td>" + FinishedTime + "</td>" +
                    "<td>" + data[i].Cost + "</td>" +
                    "<td>" + data[i].Description + "</td>" +
                    "<td>" + CustomerName + "</td>" +
                    "<td>" +
                    "<a href='/Job/Edit/" + data[i].ID + "'>Details</a> |" +
                    "<a data-ajax='true' data-ajax-method='POST' data-ajax-success='window.location.reload()' href='/Job/Delete/1" + data[i].ID + "'>Delete</a>" +
                    "</td>" +
                    "</tr>";
            }
            $("#jobTable tbody").html(htmlText);
        },
        error: function () {
            $("#jobTable tbody").html("");
        }
    });
}