
// xu ly su kien click vao the user-info hien thi va tat
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