function GetCantonsByProvinceId() {
    var dID = $("#Province").val();

    $.ajax({
        url: "/Users/GetCantonsByProvinceId",
        type: "GET",
        async: false,
        dataType: "JSON",
        data: { provinceId: dID },
        contentType: "application/json;",
        success: function (response) {
            console.log(response);
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
            alert(error);
        }
    });
};

function GetDistrictsByCantonId() {
    var dID = $("#Canton").val();

    $.ajax({
        url: "/Users/GetDistrictsByCantonId",
        type: "GET",
        async: false,
        dataType: "JSON",
        data: { cantonId: dID },
        contentType: "application/json;",
        success: function (response) {
            console.log(response);
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
            alert(error);
        }
    });
};