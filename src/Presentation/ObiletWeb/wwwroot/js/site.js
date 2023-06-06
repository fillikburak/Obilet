
$(document).ready(function () {
    var today = new Date();
    var tomorrow = new Date();
    tomorrow.setDate(today.getDate() + 1);

    var formattedToday = formatDate(today);
    var formattedTomorrow = formatDate(tomorrow);

    //set tomorrow default
    $("#datepicker").attr("placeholder", formattedTomorrow);
    $("#datepicker").val(formattedTomorrow);

    //set today button
    $("#setTodayBtn").on("click", function () {
        $("#datepicker").val(formattedToday);
    });

    //set tomorrow button
    $("#setTomorrowBtn").on("click", function () {
        var tomorrow = new Date();
        tomorrow.setDate(tomorrow.getDate() + 1);

        var formattedTomorrow = formatDate(tomorrow);

        $("#datepicker").datepicker("setDate", formattedTomorrow);
    });

    //check the selected value of the source has not the same value of the destination and vice versa
    $('#source, #destination').change(function () {
        var sourceValue = $('#source').val();
        var destinationValue = $('#destination').val();

        if (sourceValue === destinationValue) {
            alert('Kaynak ve hedef aynı şehir olarak seçilemez!');

            $('#source').val($('#source option:first').val());
            $("button[data-id='source']").find(".filter-option-inner-inner").html("İstanbul Avrupa");

            $('#destination').val($('#source option:eq(2)').val());
            $("button[data-id='destination']").find(".filter-option-inner-inner").html("Ankara");
        }
    });

    //swap between source and destination
    $("#swap").on("click", function () {
        var sourceValue = $('#source').val();
        var sourceText = $("button[data-id='source']").find(".filter-option-inner-inner").html();
        var destinationValue = $('#destination').val();
        var destinationText = $("button[data-id='destination']").find(".filter-option-inner-inner").html();

        $('#source').val(destinationValue);
        $("button[data-id='source']").find(".filter-option-inner-inner").html(destinationText);
        $('#destination').val(sourceValue);
        $("button[data-id='destination']").find(".filter-option-inner-inner").html(sourceText);
    });

    //format jquery new date() object to dd/mm/yyyy format
    function formatDate(date) {
        return ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
    }
});

//block before today
$.fn.datepicker.defaults.startDate = new Date();

//set date format
$.fn.datepicker.defaults.format = 'dd/mm/yyyy';

