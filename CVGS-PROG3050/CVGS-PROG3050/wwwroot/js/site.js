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

    if (activeTab) {
        const currTab = activeTab.value || 'Profile';
        showTab(currTab);
    }
    //-------------------------------------------------------------------------------

});


//Show the games info-----------------------------------------------------------
//sends all the informatiuon 
function showGameInfo(gameName, gameInfo, gameImageUrl, gameGenre, gamePrice, gameDeveloper, gamePublisher, gameId, inWishlist, averageRating, randomReview, gameReleaseDate, reviews) {
    document.querySelector('#gameInfoCardLabel').innerText = gameName;
    document.querySelector('#gameDescription').innerHTML = gameInfo;
    document.querySelector('#gameImage').src = gameImageUrl;
    document.querySelector('#gameGenre').innerHTML = gameGenre;
    document.querySelector('#gamePrice').innerHTML = "$" + gamePrice;
    document.querySelector('#gameDeveloper').innerHTML = gameDeveloper;
    document.querySelector('#gamePublisher').innerHTML = gamePublisher;
    document.querySelector('#gameReleaseDate').innerHTML = gameReleaseDate;

    document.querySelector('#averageRating').innerHTML = averageRating;
    document.querySelector('#randomReview').innerHTML = randomReview;

    document.querySelector('#gameIdWishlist').value = gameId;
    document.getElementById('gameIdReview').value = gameId;
    document.getElementById('gameIdRating').value = gameId;
    document.getElementById('gameIdDelete').value = gameId;
    document.getElementById('editGameid').value = gameId;
    document.getElementById('gameIdCart').value = gameId;


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
    const allReviews = document.querySelector('#allReviews');
    allReviews.innerHTML = "";
    fetch(`/Home/GetGameReviews?gameId=${gameId}`)
        .then(response => response.json())
        .then(data => {
            if (data && data.length > 0) {
                data.forEach(review => {
                    const reviewElement = document.createElement('li');
                    reviewElement.classList.add('review-item');
                    reviewElement.innerHTML = `<strong>${review.username}:</strong> ${review.reviewText}`;
                    allReviews.appendChild(reviewElement);
                });
            } else {
                allReviews.innerHTML = "No reviews available";
            }
        })
        .catch(error => {
            console.error("Error fetching reviews:", error);
            allReviews.innerHTML = "Failed to load reviews.";
        });

    // Show the modal
    const myModal = new bootstrap.Modal(document.getElementById('gameInfoCard'));
    myModal.show();
    console.log("Game Name:", gameName);

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



document.addEventListener("DOMContentLoaded", function () {
    const shippingCheckbox = document.getElementById('shipPhysicalCopy');
    const shippingCostElement = document.getElementById('shipping');
    const totalPriceElement = document.getElementById('totalPrice');
    const subtotalElement = document.getElementById('subtotal');
    const taxElement = document.getElementById('tax');

    if (shippingCheckbox && shippingCostElement && totalPriceElement && subtotalElement && taxElement) {
        let subtotal = parseFloat(subtotalElement.textContent.replace(/[^0-9.-]+/g, ""));
        if (isNaN(subtotal)) {
            subtotal = 0; 
        }
        let taxRate = 0.13;
        let shippingCost = 0.00;

        function calculateTotals() {
            let tax = subtotal * taxRate;
            let grandTotal = subtotal + tax + shippingCost;
            taxElement.innerText = `$${tax.toFixed(2)}`;
            shippingCostElement.innerText = `$${shippingCost.toFixed(2)}`;
            totalPriceElement.innerText = `$${grandTotal.toFixed(2)}`;
        }
        shippingCheckbox.addEventListener('change', function () {
            if (shippingCheckbox.checked) {
                shippingCost = 10.00;
            } else {
                shippingCost = 0.00;
            }
            calculateTotals();
        });

        calculateTotals();
    }
});

document.addEventListener("DOMContentLoaded", function () {
    const mailingSameAsShippingCheckbox = document.getElementById("SameAddress");
    const shippingAddressSection = document.getElementById("shippingAddressSection");
    const shippingAddressInput = shippingAddressSection.querySelectorAll("input, select, textarea");

    // Function to toggle shipping address section
    function toggleShippingAddressSection() {
        if (mailingSameAsShippingCheckbox.checked) {
            shippingAddressSection.style.display = "none";
            shippingAddressInputs.forEach(input => {
                input.disabled = true;
            });
        } else {
            shippingAddressSection.style.display = "block";
            shippingAddressInputs.forEach(input => {
                input.disabled = false; 
            });
        }
    }
    mailingSameAsShippingCheckbox.addEventListener("change", toggleShippingAddressSection);
    toggleShippingAddressSection();
});

//Toggle admin pages-------------------------------------------------------------
document.addEventListener("DOMContentLoaded", function () {
    const toggleAddEventAdminPage = document.getElementById("AddEventAdminPage");
    const toggleReviewReviewsAdminPage = document.getElementById("ReviewReviewsAdminPage");
    const toggleGenerateReportAdminPage = document.getElementById("GenerateReportAdminPage");
    const toggleAddGameAdminPage = document.getElementById("AddGameAdminPage");

    const toggleAddEventBtn = document.getElementById("toggleAddEvent");
    const toggleReviewsBtn = document.getElementById("toggleReviews");
    const toggleReportsBtn = document.getElementById("toggleReports");
    const toggleAddGameBtn = document.getElementById("toggleAddGame");

    toggleAddEventBtn.addEventListener('click', function () {
        toggleAddEventAdminPage.classList.remove('hidden');
        toggleReviewReviewsAdminPage.classList.add('hidden');
        toggleGenerateReportAdminPage.classList.add('hidden');
        toggleAddGameAdminPage.classList.add('hidden');
    });

    toggleReviewsBtn.addEventListener('click', function () {
        toggleAddEventAdminPage.classList.add('hidden');
        toggleReviewReviewsAdminPage.classList.remove('hidden');
        toggleGenerateReportAdminPage.classList.add('hidden');
        toggleAddGameAdminPage.classList.add('hidden');
    });

    toggleReportsBtn.addEventListener('click', function () {
        toggleAddEventAdminPage.classList.add('hidden');
        toggleReviewReviewsAdminPage.classList.add('hidden');
        toggleGenerateReportAdminPage.classList.remove('hidden');
        toggleAddGameAdminPage.classList.add('hidden');
    });

    toggleAddGameBtn.addEventListener('click', function () {
        toggleAddEventAdminPage.classList.add('hidden');
        toggleReviewReviewsAdminPage.classList.add('hidden');
        toggleGenerateReportAdminPage.classList.add('hidden');
        toggleAddGameAdminPage.classList.remove('hidden');
    });
});