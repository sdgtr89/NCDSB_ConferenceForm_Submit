$(function() {


    function calculateDistance(origin, destination) {
        
        var service = new google.maps.DistanceMatrixService();
        service.getDistanceMatrix(
            {
                origins: [origin],
                destinations: [destination],
                travelMode: google.maps.TravelMode.DRIVING,
                unitSystem: google.maps.UnitSystem.METRIC,
                avoidHighways: false,
                avoidTolls: false
            }, callback);
    }

    function callback(response, status) {
        if (status != google.maps.DistanceMatrixStatus.OK) {
            alert(err);
        } else {
            var origin = response.originAddresses[0];
            var destination = response.destinationAddresses[0];
            var distance = response.rows[0].elements[0].distance;
            var distance_value = distance.value;
            var distance_text = distance.text.substr(0, distance.text.indexOf(' ')); 
            var miles = distance_text.substring(0, distance_text.length - 3);
            $('#Distance').val(distance_text);
        }
    }


    $('#calcDis').click(function () {

        var originGet = $("#StartAddress").val();
        var destinationGet = $("#EndAddress").val();
        var origin = originGet.substr(originGet.indexOf("-") + 1);
        var destination = destinationGet.substr(destinationGet.indexOf("-") + 1);
        var distance_text = calculateDistance(origin, destination);
        
    });
});