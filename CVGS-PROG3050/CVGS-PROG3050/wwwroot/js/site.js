// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//toggle profile pages ----------------------------------------------------------
const toggleButtonProfile = document.getElementById('toggleProfile');
const toggleButtonPrefrences = document.getElementById('togglePrefrences');
const toggleButtonShippingInfo = document.getElementById('toggleShippingInfo');

const div1 = document.getElementById('AccountMainPage');
const div2 = document.getElementById('PrefrencesPage');
const div3 = document.getElementById('ShippingInformationPage');


toggleButtonProfile.addEventListener('click', function () {
    div1.classList.remove('hidden');
    div2.classList.add('hidden');
    div3.classList.add('hidden');
});
toggleButtonPrefrences.addEventListener('click', function () {
    div1.classList.add('hidden');
    div2.classList.remove('hidden');
    div3.classList.add('hidden');
});
toggleButtonShippingInfo.addEventListener('click', function () {
    div1.classList.add('hidden');
    div2.classList.add('hidden');
    div3.classList.remove('hidden');
});
//-------------------------------------------------------------------------------