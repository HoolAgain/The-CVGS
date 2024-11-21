// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    //toggle profile pages ----------------------------------------------------------
    const toggleButtonProfile = document.getElementById('toggleProfile');
    const toggleButtonPrefrences = document.getElementById('togglePrefrences');
    const toggleButtonShippingInfo = document.getElementById('toggleShippingInfo');
    const togglePaymentInfo = document.getElementById("togglePaymentInfo");
    const toggleFandF = document.getElementById("toggleFandF");



    const div1 = document.getElementById('AccountMainPage');
    const div2 = document.getElementById('PrefrencesPage');
    const div3 = document.getElementById('ShippingInformationPage');
    const div4 = document.getElementById('PaymentMethodPage');
    const div5 = document.getElementById('FriendsandFamilyPage');

    const activeTab = document.getElementById('activeTab');

    toggleButtonProfile.addEventListener('click', function () {
        div1.classList.remove('hidden');
        div2.classList.add('hidden');
        div3.classList.add('hidden');
        div4.classList.add('hidden');
        div5.classList.add('hidden');
        setActiveTab('Profile');


    });
    toggleButtonPrefrences.addEventListener('click', function () {
        div1.classList.add('hidden');
        div2.classList.remove('hidden');
        div3.classList.add('hidden');
        div4.classList.add('hidden');
        div5.classList.add('hidden');
        setActiveTab('Preferences');

    });
    toggleButtonShippingInfo.addEventListener('click', function () {
        div1.classList.add('hidden');
        div2.classList.add('hidden');
        div3.classList.remove('hidden');
        div4.classList.add('hidden');
        div5.classList.add('hidden');
        setActiveTab('ShippingInfo');

    });
    togglePaymentInfo.addEventListener('click', function () {
        div1.classList.add('hidden');
        div2.classList.add('hidden');
        div3.classList.add('hidden');
        div4.classList.remove('hidden');
        div5.classList.add('hidden');
        setActiveTab('PaymentInfo');

    });

    toggleFandF.addEventListener('click', function () {
        div1.classList.add('hidden');
        div2.classList.add('hidden');
        div3.classList.add('hidden');
        div4.classList.add('hidden');
        div5.classList.remove('hidden');
        setActiveTab('FandF');

    });

    function setActiveTab(tab) {
        if (activeTab) {
            activeTab.value = tab;
        }
        showTab(tab);
    }

    function showTab(tab) {
        div1.classList.toggle('hidden', tab != 'Profile');
        div2.classList.toggle('hidden', tab != 'Preferences');
        div3.classList.toggle('hidden', tab != 'ShippingInfo');
        div4.classList.toggle('hidden', tab != 'PaymentInfo');
        div5.classList.toggle('hidden', tab != 'FandF');
    }

    document.addEventListener("DOMContentLoaded", function () {
        if (activeTab) {
            const currTab = activeTab.value || 'Profile';
            showTab(currTab);
        }
    });
    //-------------------------------------------------------------------------------

    
});


//Show the games info-----------------------------------------------------------
//sends all the informatiuon 
function showGameInfo(gameName, gameInfo, gameImageUrl, gameGenre, gamePrice, gameDeveloper, gamePublisher, gameId, inWishlist, averageRating, randomReview) {
    document.querySelector('#gameInfoCardLabel').innerText = gameName;
    document.querySelector('#gameDescription').innerHTML = gameInfo;
    document.querySelector('#gameImage').src = gameImageUrl;
    document.querySelector('#gameGenre').innerHTML = gameGenre;
    document.querySelector('#gamePrice').innerHTML = "$" + gamePrice;
    document.querySelector('#gameDeveloper').innerHTML = gameDeveloper;
    document.querySelector('#gamePublisher').innerHTML = gamePublisher;

    document.querySelector('#averageRating').innerHTML = "Rating: " + averageRating;
    document.querySelector('#randomReview').innerHTML = "Review: " + randomReview;

    document.querySelector('#gameIdWishlist').value = gameId;
    document.getElementById('gameIdReview').value = gameId;
    document.getElementById('gameIdRating').value = gameId;

    const wishlistForm = document.querySelector('#wishlistForm');
    const wishlistBtn = document.querySelector('#wishlistButton');
    if (inWishlist === 'True')
    {
        wishlistForm.action = "/Game/RemoveFromWishlist";
        wishlistBtn.innerText = "Remove From Wishlist";
    }
    else
    {
        wishlistForm.action = "/Game/AddToWishlist";
        wishlistBtn.innerText = "Add To Wishlist";

    }
    var myModal = new bootstrap.Modal(document.getElementById('gameInfoCard'));
    myModal.show();
    console.log("Average Rating:", averageRating);
    console.log("Random Review:", randomReview);

}


    //Filter for genre------------------------------------------------------------------
    //the button
    function filterByGenre(genre) {
        const games = document.querySelectorAll('.gameDiv');
        games.forEach(game => {
            const gameGenre = game.getAttribute('data-genre');
            if (genre === 'all' || gameGenre === genre) {
                game.style.display = '';
            } else {
                game.style.display = 'none';
            }
        });
    }





//Toggle admin pages-------------------------------------------------------------
document.addEventListener("DOMContentLoaded", function () {
    const toggleAddEventAdminPage = document.getElementById("AddEventAdminPage");
    const toggleReviewReviewsAdminPage = document.getElementById("ReviewReviewsAdminPage");
    const toggleGenerateReportAdminPage = document.getElementById("GenerateReportAdminPage");

    const toggleAddEventBtn = document.getElementById("toggleAddEvent");
    const toggleReviewsBtn = document.getElementById("toggleReviews");
    const toggleReportsBtn = document.getElementById("toggleReports");

    toggleAddEventBtn.addEventListener('click', function () {
        toggleAddEventAdminPage.classList.remove('hidden');
        toggleReviewReviewsAdminPage.classList.add('hidden');
        toggleGenerateReportAdminPage.classList.add('hidden');
    });

    toggleReviewsBtn.addEventListener('click', function () {
        toggleAddEventAdminPage.classList.add('hidden');
        toggleReviewReviewsAdminPage.classList.remove('hidden');
        toggleGenerateReportAdminPage.classList.add('hidden');
    });

    toggleReportsBtn.addEventListener('click', function () {
        toggleAddEventAdminPage.classList.add('hidden');
        toggleReviewReviewsAdminPage.classList.add('hidden');
        toggleGenerateReportAdminPage.classList.remove('hidden');
    })
});



    















