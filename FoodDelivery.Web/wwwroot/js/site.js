"use strict";

/* SignalR
-------------------------------------------------- */

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://appHub")
    .configureLogging(signalR.LogLevel.Information)
    .build()

async function start(){
    try{
        await connection.start()
        console.log("Connected.")
    }catch(err){
        console.log(err)
        setTimeout(start, 5000)
    }
}

//start()

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