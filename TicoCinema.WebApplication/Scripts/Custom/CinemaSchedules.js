function GetCinemasAvailable() {
    var beginDate = $("#BeginDatetime").val();
    var finishDate = $("#FinishDatetime").val();
    var beginHour = $("#BeginTime").val();
    var hoursRange = $("#HoursRange").val();
    var movieFormat = $("#MovieFormatId").val();
    var movieID = $("#MovieId").val();

    $.ajax({
        url: "/CinemaSchedules/GetCinemasAvailable",
        type: "GET",
        async: false,
        dataType: "JSON",
        data: { movieId: movieID, movieFormatId: movieFormat, beginDate: beginDate, finishDate: finishDate, beginHour: beginHour, hoursRange: hoursRange},
        contentType: "application/json;",
        success: function (response) {
            var select = $("#CinemaId");
            select.empty();
            $.each(response, function (index, itemData) {
                select.append($('<option/>', {
                    value: itemData.Value,
                    text: itemData.Text
                }));
            });
        },
        error: function (request, error) {
            console.log(request);
        }
    });
};