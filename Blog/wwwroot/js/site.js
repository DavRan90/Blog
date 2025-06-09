// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.getElementById("newElement").onchange = function () { myFunction() };

function loadBackgroundColor() {
    var x = document.getElementById("myText");
    x.style.color = "Red";
}

function changeBackgroundColor() {
    let colorpicker = document.getElementById('colorpickerbg');

    setInterval(() => {
        let color = colorpicker.value;
        document.getElementById("page").style.backgroundColor = color;

    }, 200);
}

function changeFontColor() {
    let colorpickerfont = document.getElementById('colorpickerfont');

    setInterval(() => {
        let color = colorpickerfont.value;
        document.getElementById("page").style.color = color;

    }, 200);
}

function preview() {
    let x = document.getElementById("previewBtn");
    let element = document.getElementById('preview');
    let previewElements = document.getElementsByClassName('preview');
    if (x.value == "Preview") {
        x.value = "Edit";
        for (var i = 0; i < previewElements.length; i++) {
            previewElements[i].className += " visually-hidden";
        }
    }
    else {
        x.value = "Preview";
        for (var i = 0; i < previewElements.length; i++) {
            previewElements[i].className += "show";
        }
        /*x.className¨= "btn btn-info float-end"*/
    }

}

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
//function selectMenuItems() {
//    let form = document.getElementById("form");
//    let items = document.getElementById("menuBar").value;
//    for (int i = 1; i < items; i++;)
//    {
//        form.innerHTML = "<input asp-for='Menu.MenuTitles' class='show' />";
//    }
//}

function selectElement(siteId) {
    /*let element = document.getElementById("selectedElement1").value;*/

    // Think this is the one
    let element2 = document.querySelector("select[name='selectedElement" + CSS.escape(siteId) + "']").value;

    /*var x = document.getElementById("content");*/



    //let title = document.getElementById("title");
    //let text = document.getElementById("title");
    //let image = document.getElementById("title");

    //let elements = document.getElementsByClassName(siteId);

    //for (let i = 0; i < elements.length; i++) {
    //    if (elements[i].id == "title") {
    //        title = elements[i];
    //    }
    //    else if (elements[i].id == "text") {
    //        text = elements[i];
    //    }
    //    else if (elements[i].id == "image") {
    //        title = elements[i];
    //    }
    //}

    //hideAll();


    //if (element == 1) {
    //    title.className = "show";
    //}
    //else if (element == 2) {
    //    text.className = "show";
    //}
    //else if (element == 3) {
    //    image.className = "show";
    //}

    let title = document.querySelector("#title" + CSS.escape(siteId));
    let text = document.querySelector("#text" + CSS.escape(siteId));
    let image = document.querySelector("#image" + CSS.escape(siteId));

    let menuTitle1 = document.querySelector("#menu" + CSS.escape(siteId) + "0");
    let menuTitle2 = document.querySelector("#menu" + CSS.escape(siteId) + "1");
    let menuTitle3 = document.querySelector("#menu" + CSS.escape(siteId) + "2");

    /*let title = document.querySelector("input[name='title" + CSS.escape(siteId) + "']");*/
    //let text = document.querySelector("textarea[name='text" + CSS.escape(siteId) + "']");
    //let image = document.querySelector("input[name='image" + CSS.escape(siteId) + "']");
    hideAll();

    if (element2 == 1) {
        title.className = "show";
    }
    else if (element2 == 2) {
        text.className = "show";
    }
    else if (element2 == 3) {
        image.className = "show";
    }
    else if (element2 == 4) {
        menuTitle1.className = "show";
        menuTitle2.className = "show";
        menuTitle3.className = "show";
    }
    function hideAll() {
        title.className = "visually-hidden"
        text.className = "visually-hidden"
        image.className = "visually-hidden"
        menuTitle1.className = "visually-hidden"
        menuTitle2.className = "visually-hidden"
        menuTitle3.className = "visually-hidden"
    }
}