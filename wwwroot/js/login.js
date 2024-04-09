const uri = '/Login';
const form = document.getElementById('formUser')
const userName = document.getElementById('userName')
const userPassword = document.getElementById('userPassword')
const submitButton = document.getElementById('submitButton');
const login = (req, res) => {
    localStorage.clear();
    const user = {
        Id: 1,
        Name: userName.value.trim(),
        Password: userPassword.value.trim(),
        UserType: 0
    };
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then((token) => {
            if (token.includes("401")) {
                userPassword = "";
                userName = "";
                alert("woooooops!!😮😮😮😮 user not found!");
            }
            else {
                localStorage.setItem("token", JSON.stringify(token));
                userName.value = ''; 
                userPassword.value = '';          
                window.location.href = "../tasks.html";
            }
        })
        .catch(() => alert("not found!!🙄"))
}

form.onsubmit = (event) => {
    event.preventDefault();
    login();
}
