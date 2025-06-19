window.authLogin = async function (email, password) {
    const response = await fetch('/api/auth/login', {
        method: 'POST',
        credentials: 'include', 
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ email, password })
    });

    return response.ok;
};

window.authLogout = async function () {
    const response = await fetch('/api/auth/logout', {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ refreshToken: "" }) 
    });

    return response.ok;
};

window.authRefresh = async function () {
    const response = await fetch('/api/auth/refresh', {
        method: 'POST',
        credentials: 'include'
    });

    return response.ok;
};
