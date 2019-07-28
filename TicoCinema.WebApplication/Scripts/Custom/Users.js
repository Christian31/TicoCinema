function GetCantonsByProvinceId() {
    var provinceID = $("#Province").val();

    $.ajax({
        url: "/Users/GetCantonsByProvinceId",
        type: "GET",
        async: false,
        dataType: "JSON",
        data: { provinceId: provinceID },
        contentType: "application/json;",
        success: function (response) {
            var select = $("#Canton");
            select.empty();
            $.each(response, function (index, itemData) {
                select.append($('<option/>', {
                    value: itemData.IdCanton,
                    text: itemData.CantonName
                }));
            });
        },
        error: function (request, error) {
            console.log(error);
        }
    });
};

function GetDistrictsByCantonId() {
    var cantonID = $("#Canton").val();

    $.ajax({
        url: "/Users/GetDistrictsByCantonId",
        type: "GET",
        async: false,
        dataType: "JSON",
        data: { cantonId: cantonID },
        contentType: "application/json;",
        success: function (response) {
            var select = $("#District");
            select.empty();
            $.each(response, function (index, itemData) {
                select.append($('<option/>', {
                    value: itemData.IdDistrict,
                    text: itemData.DistrictName
                }));
            });
        },
        error: function (request, error) {
            console.log(error);
        }
    });
};