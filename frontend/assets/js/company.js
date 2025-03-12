document.addEventListener('DOMContentLoaded', function() {
    const myCompanyLink = document.getElementById('myCompanyLink');
    const contentDiv = document.getElementById('content');

    myCompanyLink.addEventListener('click', function(event) {
        event.preventDefault();
        fetchCompanyData();
    });

    function fetchCompanyData() {
        const token = localStorage.getItem('jwtToken');
        fetch('https://localhost:7201/api/company/me', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            if (data.name) {
                displayCompanyInfo(data);
            } else {
                displayCreateCompanyForm();
            }
        })
        .catch(error => {
            console.error('There has been a problem with your fetch operation:', error);
            displayCreateCompanyForm();
        });
    }

    function displayCompanyInfo(company) {
        contentDiv.innerHTML = `
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">${company.name}</h5>
                    <p class="card-text">${company.description}</p>
                    <a href="${company.webSite}" class="card-link">Website</a>
                </div>
            </div>
        `;
    }

    function displayCreateCompanyForm() {
        contentDiv.innerHTML = `
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">Создать компанию</h5>
                    <form id="createCompanyForm">
                        <div class="mb-3">
                            <label for="companyName" class="form-label">Название компании</label>
                            <input type="text" class="form-control" id="companyName" required>
                        </div>
                        <div class="mb-3">
                            <label for="companyDescription" class="form-label">Описание</label>
                            <textarea class="form-control" id="companyDescription" rows="3" required></textarea>
                        </div>
                        <div class="mb-3">
                            <label for="companyWebsite" class="form-label">Веб-сайт</label>
                            <input type="url" class="form-control" id="companyWebsite" required>
                        </div>
                        <button type="submit" class="btn btn-primary">Создать</button>
                    </form>
                </div>
            </div>
        `;

        document.getElementById('createCompanyForm').addEventListener('submit', function(event) {
            event.preventDefault();
            createCompany();
        });
    }

    function createCompany() {
        const token = localStorage.getItem('jwtToken');
        const companyName = document.getElementById('companyName').value.trim();
        const companyDescription = document.getElementById('companyDescription').value.trim();
        const companyWebsite = document.getElementById('companyWebsite').value.trim();
    
        if (!companyName || !companyDescription || !companyWebsite) {
            alert('Все поля обязательны для заполнения!');
            return;
        }
    
        const companyData = {
            name: companyName,
            description: companyDescription,
            webSite: companyWebsite,
            logoPath: "" 
        };
    
        console.log('Отправляемые данные:', companyData); 
    
        fetch('https://localhost:7201/api/company', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(companyData)
        })
        .then(response => {
            if (!response.ok) {
                return response.json().then(err => {
                    throw new Error(err.message || 'Ошибка при создании компании');
                });
            }
            return response.json();
        })
        .then(data => {
            alert(data.message || 'Компания успешно создана!');
            window.location.href = 'index.html'; 
        })
        .catch(error => {
            console.error('Ошибка:', error);
            alert(error.message || 'Произошла ошибка при создании компании');
        });
    }
});