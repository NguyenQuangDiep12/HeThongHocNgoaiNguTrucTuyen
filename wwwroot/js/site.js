// Js dung chung cho toan bo he thong


/** Xu ly phan hien thi dang ky dang nhap va dang xuat*/
const menuButton =
    document.getElementById("userMenuButton");

const dropdown =
    document.getElementById("userDropdown");

if (menuButton && dropdown) {

    menuButton.addEventListener("click", function (e) {

        e.stopPropagation();

        dropdown.classList.toggle("show");
    });

    document.addEventListener("click", function () {

        dropdown.classList.remove("show");
    });
}

// Cat ten nguoi dung gioi han khoang 20 ky tu trong user-info tag
const usernameTag = document.getElementById("user-info-name");
if (usernameTag.textContent.trim() >= 20) {

    let username = usernameTag.textContent;
    let truncatedUser = username.substring(0, 20);

    usernameTag.innerText = truncatedUser + '...';
}