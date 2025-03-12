document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    form.addEventListener("submit", async function (event) {
        event.preventDefault(); 

        const username = document.getElementById("username").value.trim();
        const email = document.getElementById("email").value.trim();
        const password = document.getElementById("password").value.trim();
        const firstName = document.getElementById("firstName").value.trim();
        const lastName = document.getElementById("lastName").value.trim();
        const roleId = document.getElementById("role").value; 

        const userData = {
            username,
            email,
            password,
            firstName,
            lastName,
            roleId: parseInt(roleId) 
        };

        try {
            const response = await fetch("http://localhost:5199/api/auth/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(userData)
            });

            const result = await response.json();

            if (response.ok) {
                localStorage.setItem("jwtToken", result.token);
                
                window.location.href = "../index.html";
            } else {
                showError(result.message || "Ошибка регистрации");
            }
        } catch (error) {
            showError("Ошибка соединения с сервером");
        }
    });

    function showError(message) {
        let alertBox = document.getElementById("error-message");
        if (!alertBox) {
            alertBox = document.createElement("div");
            alertBox.id = "error-message";
            alertBox.className = "alert alert-danger mt-3";
            form.appendChild(alertBox);
        }
        alertBox.textContent = message;
    }
});