$('.daterange').daterangepicker();

$("#numOfRecords").change(function () {
    window.location.href = '/Invoice/Index?numOfRecords=' + $("#numOfRecords").val();
});