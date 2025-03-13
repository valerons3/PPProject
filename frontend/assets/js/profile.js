document.addEventListener('DOMContentLoaded', function () {
    const profileLink = document.getElementById('profileLink'); 
    const contentDiv = document.getElementById('content');

    profileLink.addEventListener('click', function (event) {
        event.preventDefault();
        fetchUserInfo();
    });

    async function fetchUserInfo() {
        const token = localStorage.getItem('jwtToken');
        try {
            const response = await fetch('https://localhost:7201/api/users', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Ошибка при получении данных пользователя');
            }

            const user = await response.json();
            displayUserInfo(user); 
        } catch (error) {
            console.error('Ошибка:', error);
            alert(error.message || 'Произошла ошибка при загрузке данных пользователя');
        }
    }

    function displayUserInfo(user) {
        contentDiv.innerHTML = `
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">Информация о пользователе</h5>
                    <p><strong>Имя пользователя:</strong> ${user.username}</p>
                    <p><strong>Email:</strong> ${user.email}</p>
                    <p><strong>Имя:</strong> ${user.firstName}</p>
                    <p><strong>Фамилия:</strong> ${user.lastName}</p>
                    <p><strong>Роль:</strong> ${user.role}</p>
                    <p><strong>Дата регистрации:</strong> ${new Date(user.createdAt).toLocaleDateString()}</p>
                </div>
            </div>
        `;
    }
});