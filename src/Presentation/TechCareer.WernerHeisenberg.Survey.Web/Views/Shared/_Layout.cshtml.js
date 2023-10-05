addEventListener("DOMContentLoaded",function (ev){
    console.log("DOMContentLoaded");
    let mainHeader= document.getElementById("mainHeader");
    let mainContainer = document.getElementById("mainContent");
    mainContainer.style.marginTop= mainHeader.offsetHeight + "px";
});