// Форма регистрации
document.getElementById('registerForm').addEventListener('submit', function (e) {
  e.preventDefault();

  const data = {
      username: document.getElementById('username').value,
      email: document.getElementById('email').value,
      password: document.getElementById('password').value,
      firstName: document.getElementById('firstName').value,
      lastName: document.getElementById('lastName').value,
      roleId: document.getElementById('role').value
  };

  fetch('http://localhost:5199/api/auth/register', {
      method: 'POST',
      headers: {
          'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
  })
  .then(response => response.json())
  .then(result => {
      console.log('Registration successful:', result);
      localStorage.setItem('token', result.token);
  })
  .catch(error => {
      console.error('Error:', error);
  });
});

// Форма авторизации
document.getElementById('loginForm').addEventListener('submit', function (e) {
  e.preventDefault();

  const data = {
      email: document.getElementById('email').value,
      password: document.getElementById('password').value
  };

  fetch('http://localhost:7201/api/auth/login', {
      method: 'POST',
      headers: {
          'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
  })
  .then(response => response.json())
  .then(result => {
      console.log('Login successful:', result);
      localStorage.setItem('token', result.token);
  })
  .catch(error => {
      console.error('Error:', error);
  });
});
