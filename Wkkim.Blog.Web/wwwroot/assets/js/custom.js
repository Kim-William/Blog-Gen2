// ====== aside navigation bar ======

function open_aside() {
    "use strict";
    const sidepanel = document.getElementById("mySidenav");
    if (sidepanel) {
        sidepanel.style.left = "0";
    } else {
        console.error("Error: Side panel element not found!");
    }
}
function close_aside() {
    "use strict";
    const sidepanel = document.getElementById("mySidenav");
    if (sidepanel) {
        sidepanel.style.left = "-355px";
    } else {
        console.error("Error: Side panel element not found!");
    }
}

// aside page button
let slid = document.getElementById("slid-btn");
slid.onclick = () => {
    let dropdwon = document.getElementById("slid-drop");
    dropdwon.classList.toggle("aside-dropdwon");
}

// aside page button
let slidadmin = document.getElementById("slidadmin-btn");
if (slidadmin != null) {

    slidadmin.onclick = () => {
        let dropdwon = document.getElementById("slidadmin-drop");
        dropdwon.classList.toggle("aside-dropdwon");
    }
} 


// ====== 1.3. Logoipsum section ======
$('.logo_ipsum_slider').slick({
    arrows: false,
    dots: false,
    infinite: true,
    autoplay: true,
    autoplaySpeed: 0,
    speed: 3000,
    slidesToShow: 4,
    cssEase: 'linear',
    responsive: [{
        breakpoint: 1000,
        settings: {
            slidesToShow: 3,
            slidesToScroll: 1,
            infinite: true,
            dots: false,
        }
    },
    {
        breakpoint: 768,
        settings: {
            slidesToShow: 2,
            slidesToScroll: 2,
            infinite: true,
            dots: false,
        }
    },
    {
        breakpoint: 480,
        settings: {
            slidesToShow: 2,
            slidesToScroll: 1,
        }
    },
    {
        breakpoint: 360,
        settings: {
            slidesToShow: 2,
            slidesToScroll: 1
        }
    },
    ]
});




// ====== 1.4. Amenities section ======
$('.amenities-slider').slick({
    arrows: false,
    dots: true,
    infinite: true,
    slidesToShow: 4,
    slidesToScroll: 2,
    responsive: [{
            breakpoint: 1000,
            settings: {
                slidesToShow: 3,
                slidesToScroll: 2,
                infinite: true,
            }
        },
    {
            breakpoint: 768,
            settings: {
                slidesToShow: 3,
                slidesToScroll: 2,
                infinite: true,
            }
        },
        {
            breakpoint: 767,
            settings: {
                slidesToShow: 3,
                slidesToScroll: 1,
            }
        },
        {
            breakpoint: 481,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1,
            }
        },
    ]
});



// ====== 1.5. workspaces section ====== h6 hover

const hoveredDivs = document.querySelectorAll('.banifits h6');
let larg;
hoveredDivs.forEach((hoveredDiv) => {
  hoveredDiv.addEventListener('mouseenter', () => {
       larg = document.getElementById('work-fanifits')
       larg.classList.remove('work-active')
    hoveredDiv.classList.add('work-active');   
  });

  hoveredDiv.addEventListener('mouseleave', () => {
    hoveredDiv.classList.remove('work-active');
         larg = document.getElementById('work-fanifits')
        larg.classList.add('work-active')
  });
});


// ====== 1.11. customers section ====== 

$('.slider_customers').slick({
    arrows: false,
    dots: true,
    infinite: true,
    slidesToShow: 2,
    slidesToScroll: 1,
    responsive: [{
            breakpoint: 1047,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1,
                infinite: true,
                dots: true,
            }
        },
    ]
});



 // 1.14. Subscribe-us section === Subscribe successfully massage

 //const aboutFor = document.getElementById('Subscribe-massage');
 //const aboutMessag = document.getElementById('Succes-box');
 //if (aboutFor !== null){
 //aboutFor.addEventListener('submit', (event) => {
 //    event.preventDefault();
 //    aboutMessag.innerHTML = 'You Subscribe successfully!';
 //    aboutMessag.style.display = 'block';
 //    aboutFor.reset();
 //    setTimeout(() => {
 //      aboutMessag.style.display = 'none';
 //    }, 3000);
 //});
 //}


 // 1.7. find section === form Submit massage

// const aboutFor2 = document.getElementById('Subscribe-massage2');
// const aboutMessag2 = document.getElementById('Succes-box2');
// if (aboutFor2 !== null){
// aboutFor2.addEventListener('submit', (event) => {
//     event.preventDefault();
//     aboutMessag2.innerHTML = 'Form Submit successfully!';
//     aboutMessag2.style.display = 'block';
//     aboutFor2.reset();
//     setTimeout(() => {
//       aboutMessag2.style.display = 'none';
//     }, 3000);
// });
//}


 // 10.1. ask-question section === successfully Message

// const aboutFor3 = document.getElementById('Subscribe-massage3');
// const aboutMessag3 = document.getElementById('Succes-box3');
// if (aboutFor3 !== null){
// aboutFor3.addEventListener('submit', (event) => {
//     event.preventDefault();
//     aboutMessag3.innerHTML = 'Message Send successfully!';
//     aboutMessag3.style.display = 'block';
//     aboutFor3.reset();
//     setTimeout(() => {
//       aboutMessag3.style.display = 'none';
//     }, 3000);
// });
//}


// ====== Number Counter =======

(function (e) {
    "use strict";
    e.fn.counterUp = function (t) {
        const n = e.extend({
            time: 400,
            delay: 10
        }, t);
        return this.each(function () {
            const t = e(this);
            const r = n;
            function incrementValue() {
                const nums = [];
                const duration = r.time / r.delay;
                let value = t.text();
                const hasCommas = /[0-9]+,[0-9]+/.test(value);
                value = value.replace(/,/g, "");
                const isNumber = /^[0-9]+$/.test(value);
                const isFloat = /^[0-9]+\.[0-9]+$/.test(value);
                const decimalPlaces = isFloat ? (value.split(".")[1] || []).length : 0;
                for (let i = duration; i >= 1; i--) {
                    let newValue = parseInt(value / duration * i);
                    if (isFloat) {
                        newValue = parseFloat(value / duration * i).toFixed(decimalPlaces);
                    }
                    if (hasCommas) {
                        while (/(\d+)(\d{3})/.test(newValue.toString())) {
                            newValue = newValue.toString().replace(/(\d+)(\d{3})/, "$1,$2");
                        }
                    }
                    nums.unshift(newValue);
                }
                t.data("counterup-nums", nums);
                t.text("0");
                function updateValue() {
                    t.text(t.data("counterup-nums").shift());
                    if (t.data("counterup-nums").length) {
                        setTimeout(updateValue, r.delay);
                    } else {
                        t.removeData("counterup-nums");
                    }
                }
                t.data("counterup-func", updateValue);
                setTimeout(t.data("counterup-func"), r.delay);
            }
            t.waypoint(incrementValue, {
                offset: "100%",
                triggerOnce: true
            });
        });
    };
})(jQuery);
jQuery(document).ready(function ($) {
    $('.count').counterUp({
        delay: 10,
        time: 1000
    });
});
