//on load
$(function () {
    //creates date range picker
    $("#dateRange").daterangepicker({
        "showDropdowns": true,
        "showWeekNumbers": true,
        "autoApply": true,
        "linkedCalendars": false,
        "startDate": "01/01/2001",
        locale: {
            format: 'DD/MMM/YYYY'
        }
    });

    GetJobs();
    //on search option change get corresponding invoices
    $("#numOfRecords, #jobId, #jobName, #dateRange, input[name=jobStatus], input[name=dateOption], #customerName, #description").change(function () {
        GetJobs();
    });
    $("#NewJob").click(function(){
        NewJob();
    }); 
});

//gets jobs
function GetJobs() {
    $.ajax({
        type: "GET",
        url: "/Job/GetJobsAjax",
        data: {
            numOfRecords: $("#numOfRecords").val(),
            jobId: $("#jobId").val(),
            jobName: $("#jobName").val(),
            jobStatus: $("input[name=jobStatus]:checked").val(),
            customerName: $("#customerName").val(),
            description: $("#description").val(),
            dateOption: $("input[name=dateOption]:checked").val(),
            from: $("#dateRange").data("daterangepicker").startDate.format("DD/MM/YYYY"),
            to: $("#dateRange").data("daterangepicker").endDate.format("DD/MM/YYYY")
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
                    for (var i = 0; i < data.length; i++) {
                        var StartedTime = moment(data[i].StartedTime).format("DD-MM-YYYY");
                        var FinishedTime = moment(data[i].FinishedTime).format("DD-MM-YYYY");
                        var NotificationTime = moment(data[i].NotificationTime).format("DD-MM-YYYY HH:mm");
                        var CustomerName = data[i].Customer.FamilyName + " " + data[i].Customer.GivenName;
                        statusJob = ["", "Unassigned", "In Progress", "Done"];
                        htmlText += "<tr>" +
                            "<td class=\"jobId\">" +
                            data[i].ID +
                            "</td>" +
                            "<td>" +
                            data[i].JobName +
                            "</td>" +
                            "<td>" +
                            StartedTime +
                            "</td>" +
                            "<td>" +
                            FinishedTime +
                            "</td>" +
                            "<td>" +
                            data[i].Cost +
                            "</td>" +
                            "<td>" +
                            statusJob[data[i].JobStatus] +
                            "</td>" +
                            "<td>" +
                            CustomerName +
                            "</td>" +
                            "<td>" +
                            data[i].Description +
                            "</td>";
                            
                        if (data[i].NotificationTime != null) {
                            htmlText += "<td>" +
                                "<button class=\"fa fa-envelope\" title = \"Sent at: " +
                                NotificationTime +
                                "\" onclick=\"ResendNotification(this)\" style='font-size:36px'></button>" +
                                "</td>";
                        } else {
                            htmlText += "<td> &nbsp; </td>";
                        }

                        htmlText += "<td>" +
                            "<button class=\"btn\"  onclick=\"ReadJob(this)\">" +
                            editLabel +
                            "</td>" +
                            "</tr>";
                    }
            $("#jobTable tbody").html(htmlText);
                }
    });
}

function Edit() {
    $.ajax({
        type: "POST",
        url: "/Job/PostJobAjax",
        data: $("#editJobPartial").serialize(),
        success: function () {
            $("#dialog").html("");
            $("#dialog").dialog("close");
            GetJobs();
        }
    });
};

function ReadJob(obj) {
    var jobId = $(obj).closest("tr").find("td:nth-child(1)").html();
    $.ajax({
        type: "GET",
        url: "/Job/ReadJob",
        data: {
            id: jobId
        },
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function(response) {
            InitializeDialog(response);
        }
    });
};

function ResendNotification(obj) {
    var jobId = $(obj).closest("tr").find("td:nth-child(1)").html();
    $.ajax({
        type: "GET",
        url: "/Job/ResendNotificationAjax",
        data: {
            id: jobId
        },
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function () {
            GetJobs();
        }
    });
};

function NewJob() {
    $.ajax({
        type: "GET",
        url: "/Job/ReadJob",
        data: {
            id: 0
        },
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            InitializeDialog(response);
        }
    });
};

function InitializeDialog(data) {
    var dialog = $("#dialog").dialog({
        autoOpen: false,
        modal: true,
        title: viewDetails,
        width: 1000,
        buttons: {
            cancel : function () {
                $(this).dialog("close");
            },

            save : function () {
                Edit();
            }
        }
    });

    dialog.html(data);
    dialog.dialog("open");

    $(".datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        changeDay: true
    });
};