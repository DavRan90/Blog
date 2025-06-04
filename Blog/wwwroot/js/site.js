// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.getElementById("newElement").onchange = function () { myFunction() };
function myFunction() {
    let element = document.getElementById("selectedElement").value;
    var x = document.getElementById("content");
    if (element == 1) {
        /*x.innerHTML = "<input />";*/
        x.innerHTML = "<input asp-for='Title.Content' />";
        //<input asp-for="Title.Content" />
    }
    else if (element == 2) {
        /*x.innerHTML = "<textarea cols='28' rows='5'/>";*/
        x.innerHTML = "<textarea asp-for='Text.Content' cols='28' rows='5'></textarea>";
        //<textarea asp-for="Text.Content" cols="40" rows="5"></textarea>
    }
    else if (element == 3) {
        /*x.innerHTML = "<input type='file'/>"*/
        x.innerHTML = "<input type='file' asp-for='UploadedImage' />"
        //<input asp-for="UploadedImage" />
    }
    let test = "<a asp-route-addElement='" + element + "' class='btn btn-primary'> Lägg till</a >";
    x.innerHTML += test
    
}

function selectElement() {
    let element = document.getElementById("selectedElement").value;
    var x = document.getElementById("content");
    hideAll();
    if (element == 1) {
        let title = document.getElementById("title");
        title.className = "show";
    }
    else if (element == 2) {
        let text = document.getElementById("text");
        text.className = "show";
    }
    else if (element == 3) {
        let image = document.getElementById("image");
        image.className = "show";
    }
    function hideAll() {
        let title = document.getElementById("title");
        let text = document.getElementById("text");
        let image = document.getElementById("image");
        title.className = "visually-hidden"
        text.className = "visually-hidden"
        image.className = "visually-hidden"
    }
}