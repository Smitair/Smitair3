$(document).ready(function () {

    $("#myNavbar a").on('click', function (event) {
        var hash = this.hash;
        $('html, body').animate({
            scrollTop: $(hash).offset().top
        }, 1500, function () {
            window.location.hash = hash;
        });
    });

    $(function () {
        $('.navbar').affix({
            offset: {
                top: $("header").outerHeight(true)
            }
        });
    });

    resize();
    $(window).resize(function () {
        resize();
    });

});

function resize() {
    var heightof;
    var height_browser = $(window).height();
    var height_carousel = $(".carousel-inner").height();
    if (height_browser > height_carousel)
        heightof = height_carousel;
    else heightof = height_browser;
    $(window).scroll(function () {
        if ($(this).scrollTop() > heightof) {
            $(".navbar").css("position", "fixed")
        }
        else {
            $(".navbar").css("position", "absolute")
        }
    });
}