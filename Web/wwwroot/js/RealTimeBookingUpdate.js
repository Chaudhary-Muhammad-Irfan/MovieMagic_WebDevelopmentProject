"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/bookingHub").build();
connection.start();
connection.on("BookingUpdate", function (movieId, showId, seatIds, isBooked) {
    console.log("yes");
    const mID = document.getElementById("mID").value;
    const sID = document.getElementById("sID").value;
    console.log(mID);
    console.log(sID);
    console.log("Booked IDS");
    console.log(movieId);
    console.log(showId);
    if (mID == movieId && sID == showId) {
        console.log("in condition");
        seatIds.forEach(seatId => {
            console.log("in loop");
            const seatElement = document.querySelector(`[data-number='${seatId}']`);
            if (seatElement) {
                if (isBooked) {
                    console.log("is booked");
                    $(seatElement).fadeOut('slow', function () {
                        seatElement.classList.add('booked');
                        seatElement.classList.remove('selected');
                        seatElement.style.backgroundColor = 'red';
                        seatElement.removeEventListener('click', handleSeatClick);
                        $(seatElement).fadeIn('slow');
                    });
                }
            }
        });
    }
});
function handleSeatClick(event) {
    const seat = event.currentTarget;
    if (!seat.classList.contains('booked')) {
        seat.classList.toggle('selected');
        updateSummary();
    }
}