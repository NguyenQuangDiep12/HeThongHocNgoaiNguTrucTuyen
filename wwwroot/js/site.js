// Js dung chung cho toan bo he thong


/** Xu ly phan hien thi dang ky dang nhap va dang xuat*/
const menuButton = document.getElementById("userMenuButton");

const dropdown = document.getElementById("userDropdown");

if (menuButton && dropdown) {

    menuButton.addEventListener("click", function (e) {

        e.stopPropagation();

        dropdown.classList.toggle("show");
    });

    document.addEventListener("click", function () {

        dropdown.classList.remove("show");
    });
}
