window.setTheme = (themeClass) => {
    document.body.classList.remove('rz-dark-theme', 'rz-light-theme');
    document.body.classList.add(themeClass);
};

window.setBodyClass = function (mode) {
    if (mode === 'dark') {
        document.body.classList.add("rz-dark-theme");
    } else {
        document.body.classList.remove("rz-dark-theme");
    }
};
