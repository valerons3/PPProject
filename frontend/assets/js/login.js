document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    if (!form) return;

    form.addEventListener("submit", async function (event) {
        event.preventDefault();

        const email = document.getElementById("email").value.trim();
        const password = document.getElementById("password").value.trim();
        const errorContainer = document.getElementById("error-message");

        if (errorContainer) errorContainer.remove(); 

        const requestBody = { email, password };

        try {
            const response = await fetch("http://localhost:5199/api/auth/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(requestBody),
            });

            const result = await response.json();

            if (!response.ok) {
                showError(result.message || "Ошибка авторизации");
                return;
            }

            localStorage.setItem("jwtToken", result.token);
            window.location.href = "../index.html"; 
        } catch (error) {
            showError("Ошибка сети. Попробуйте позже.");
        }
    });

    function showError(message) {
        const errorDiv = document.createElement("div");
        errorDiv.id = "error-message";
        errorDiv.className = "alert alert-danger mt-3";
        errorDiv.textContent = message;
        document.querySelector(".card-body").appendChild(errorDiv);
    }
});
