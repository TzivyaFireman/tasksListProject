const uri = '/Login';
const form = document.getElementById('formUser')
const userName = document.getElementById('userName')
const userPassword = document.getElementById('userPassword')
const submitButton = document.getElementById('submitButton');
const GetUser = (req, res) => {
    localStorage.clear();
    // const itemId = document.getElementById('edit-id').value;
    const user = {
        Id: 1,
        Name: userName.value.trim(),
        Password: userPassword.value.trim(),
        UserType: 0
    };
    alert(user.name);
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
                window.location.href = "../users.html";
            }
        })
        .catch(() => alert("not found!!🙄"))
}

form.onsubmit = (event) => {
    event.preventDefault();
    GetUser();
}

