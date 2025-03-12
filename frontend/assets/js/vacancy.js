document.addEventListener('DOMContentLoaded', function () {
    const vacanciesLink = document.getElementById('vacanciesLink'); 
    const contentDiv = document.getElementById('content');

    vacanciesLink.addEventListener('click', function (event) {
        event.preventDefault();
        fetchCompanyData();
    });

    async function fetchCompanyData() {
        const token = localStorage.getItem('jwtToken');
        try {
            const response = await fetch('https://localhost:7201/api/company/me', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                if (response.status === 404) {
                    const errorData = await response.json();
                    displayCompanyNotFoundMessage(errorData.message);
                } else {
                    throw new Error('Ошибка при получении данных компании');
                }
            } else {
                const company = await response.json();
                if (company.name) {
                    fetchVacancies(company.name); 
                } else {
                    displayCreateCompanyForm();
                }
            }
        } catch (error) {
            console.error('Ошибка:', error);
            displayCompanyNotFoundMessage('Произошла ошибка при загрузке данных компании');
        }
    }

    function displayCompanyNotFoundMessage(message) {
        contentDiv.innerHTML = `
            <div class="alert alert-warning" role="alert">
                <h4 class="alert-heading">${message}</h4>
                <p>Чтобы просматривать и создавать вакансии, сначала создайте компанию.</p>
                <hr>
                <button id="createCompanyBtn" class="btn btn-primary">Создать компанию</button>
            </div>
        `;

        document.getElementById('createCompanyBtn').addEventListener('click', function (event) {
            event.preventDefault();
            displayCreateCompanyForm(); 
        });
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

        document.getElementById('createCompanyForm').addEventListener('submit', function (event) {
            event.preventDefault();
            createCompany();
        });
    }

    async function createCompany() {
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

        try {
            const response = await fetch('https://localhost:7201/api/company', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(companyData)
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Ошибка при создании компании');
            }

            const result = await response.json();
            alert(result.message || 'Компания успешно создана!');
            fetchCompanyData();
        } catch (error) {
            console.error('Ошибка:', error);
            alert(error.message || 'Произошла ошибка при создании компании');
        }
    }

    async function fetchVacancies(companyName) {
        const token = localStorage.getItem('jwtToken');
        try {
            const response = await fetch(`https://localhost:7201/api/vacancy/company?companyName=${encodeURIComponent(companyName)}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                if (response.status === 404) {
                    displayNoVacanciesMessage(); 
                } else {
                    throw new Error('Ошибка при получении вакансий');
                }
            } else {
                const vacancies = await response.json();
                displayVacancies(vacancies); 
            }
        } catch (error) {
            console.error('Ошибка:', error);
            alert('Произошла ошибка при загрузке вакансий');
        }
    }

    function displayVacancies(vacancies) {
        contentDiv.innerHTML = `
            <h2>Вакансии компании</h2>
            <button id="createVacancyBtn" class="btn btn-primary mb-3">Создать вакансию</button>
            <ul class="list-group">
                ${vacancies.map(vacancy => `
                    <li class="list-group-item">
                        <h5>${vacancy.title}</h5>
                        <p>Категория: ${vacancy.categoryName}</p>
                        <p>Описание: ${vacancy.description}</p>
                        <p>Локация: ${vacancy.location}</p>
                        <p>Зарплата: ${vacancy.salary} Р/мес.</p>
                    </li>
                `).join('')}
            </ul>
        `;

        document.getElementById('createVacancyBtn').addEventListener('click', function () {
            displayCreateVacancyForm();
        });
    }

    function displayNoVacanciesMessage() {
        contentDiv.innerHTML = `
            <h2>Вакансии компании</h2>
            <p>Вакансии для вашей компании не найдены.</p>
            <button id="createVacancyBtn" class="btn btn-primary">Создать вакансию</button>
        `;

        document.getElementById('createVacancyBtn').addEventListener('click', function () {
            displayCreateVacancyForm();
        });
    }

    async function displayCreateVacancyForm() {
        const categories = await fetchCategories();
        contentDiv.innerHTML = `
            <h2>Создать вакансию</h2>
            <form id="createVacancyForm">
                <div class="mb-3">
                    <label for="title" class="form-label">Название вакансии</label>
                    <input type="text" class="form-control" id="title" required>
                </div>
                <div class="mb-3">
                    <label for="categoryName" class="form-label">Категория</label>
                    <select class="form-control" id="categoryName" required>
                        ${categories.map(category => `
                            <option value="${category.categoryName}">${category.categoryName}</option>
                        `).join('')}
                    </select>
                </div>
                <div class="mb-3">
                    <label for="description" class="form-label">Описание</label>
                    <textarea class="form-control" id="description" rows="3" required></textarea>
                </div>
                <div class="mb-3">
                    <label for="location" class="form-label">Локация</label>
                    <input type="text" class="form-control" id="location" required>
                </div>
                <div class="mb-3">
                    <label for="salary" class="form-label">Зарплата</label>
                    <input type="number" class="form-control" id="salary" required>
                </div>
                <button type="submit" class="btn btn-primary">Создать</button>
            </form>
        `;

        document.getElementById('createVacancyForm').addEventListener('submit', function (event) {
            event.preventDefault();
            createVacancy();
        });
    }

    async function fetchCategories() {
        const token = localStorage.getItem('jwtToken');
        try {
            const response = await fetch('https://localhost:7201/api/category', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error('Ошибка при получении категорий');
            }

            return await response.json();
        } catch (error) {
            console.error('Ошибка:', error);
            alert('Произошла ошибка при загрузке категорий');
            return [];
        }
    }

    async function createVacancy() {
        const token = localStorage.getItem('jwtToken');
        const title = document.getElementById('title').value.trim();
        const categoryName = document.getElementById('categoryName').value;
        const description = document.getElementById('description').value.trim();
        const location = document.getElementById('location').value.trim();
        const salary = document.getElementById('salary').value.trim();

        if (!title || !categoryName || !description || !location || !salary) {
            alert('Все поля обязательны для заполнения!');
            return;
        }

        const vacancyData = {
            title,
            categoryName,
            description,
            location,
            salary: parseFloat(salary)
        };

        try {
            const response = await fetch('https://localhost:7201/api/vacancy', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(vacancyData)
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Ошибка при создании вакансии');
            }

            alert('Вакансия успешно создана!');
            fetchCompanyData(); 
        } catch (error) {
            console.error('Ошибка:', error);
            alert(error.message || 'Произошла ошибка при создании вакансии');
        }
    }
});