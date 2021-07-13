"use strict";
var _userId = ""

/* Toastr Options
-------------------------------------------------- */

toastr.options = {
  "closeButton": true,
  "debug": true,
  "newestOnTop": true,
  "progressBar": true,
  "positionClass": "toast-top-right",
  "preventDuplicates": false,
  "showDuration": "300",
  "hideDuration": "1000",
  "timeOut": "50000",
  "extendedTimeOut": "50000",
  "showEasing": "swing",
  "hideEasing": "linear",
  "showMethod": "fadeIn",
  "hideMethod": "fadeOut"
}

/* SignalR
-------------------------------------------------- */

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5003/appHub")
    .configureLogging(signalR.LogLevel.Information)
    .build()

function addToChannel(userId){
    _userId = userId
}

connection.on('UpdateOrders', (message, order) => {
	toastr.success(message, order)
});

async function start(){
    try{
        await connection.start()

        if(_userId.length > 0){
            connection.invoke('AddUserToGroup', _userId).catch(err => console.error(err.toString()))
        }

        console.log("Connected.")
    }catch(err){
        console.log(err)
        setTimeout(start, 5000)
    }
}

connection.onclose(async () => {
    await start()
})

window.addEventListener('load', () => {
    start()
    toastr.success(message)
})

/* Owl Carousel
-------------------------------------------------- */
var owl = $('.owl-carousel')

owl.owlCarousel({
    items:4,
    loop:true,
    margin:10,
    autoplay:true,
    autoplayTimeout:1000,
    autoplayHoverPause:true
});

$('.play').on('click',function(){
    owl.trigger('play.owl.autoplay',[1000])
})

$('.stop').on('click',function(){
    owl.trigger('stop.owl.autoplay')
})

/* System
-------------------------------------------------- */

window.addEventListener('load', () => {
    HideControlsOnLoad(0)
})

var price = Number($("#product-price").val())

$("#product-quantity").change(() => {
    $("#product-total").text(price * Number($("#product-quantity").val()))
})

var wallet = $("#wallet-div")
var reference = $("#ref-div")
var billing = $("#bill-address")

function HideControlsOnLoad(delay){
    wallet.hide(delay)
    reference.hide(delay)
}

$("#tpa").click(() => HideControlsOnLoad(500))

$("#wallet").click(() => {
    HideControlsOnLoad(500)
    wallet.show(500)
})

$("#reference").click(() => {
    HideControlsOnLoad(500)
    reference.show(500)
})

var isSameAddress = true

$("#same-address").click(() => {
    if(isSameAddress){
        billing.hide(500)
        isSameAddress = false
    }else{
        billing.show(500)
        isSameAddress = true
    }
})

function GetExtraData(data){
    
}
