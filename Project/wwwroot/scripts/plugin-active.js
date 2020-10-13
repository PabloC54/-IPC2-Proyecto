$(document).ready(function() {
  
  // smooth scroll
  $(".smooth-scroll").click(function() {
    var target = $(this).attr("href"),
      scrollTo = $(target).offset().top,
      headerHeight = $('#Header').outerHeight();
    console.log(headerHeight);
    $('html, body').animate({ scrollTop: scrollTo - headerHeight });
    $('#nav-icon1').trigger("click");
    $("body").removeClass("menu-active");
  });
  //menu icon animation
  $('#nav-icon1').click(function() {
    $(this).toggleClass('open');
    $("body").toggleClass("menu-active");
  });
});