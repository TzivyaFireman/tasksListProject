const uri = '/user';
let users = [];
let token = localStorage.getItem("token");

alert("hi manager");

var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer " + JSON.parse(token));
myHeaders.append("Content-Type", "application/json");


const getItems = (token) => {
    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };
    fetch(uri, requestOptions)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

getItems(token);

function addItem() {
    const addNameTextbox = document.getElementById('add-userName');
    const addPasswordTextbox = document.getElementById('userPassword');

    const item = {
        Name: addNameTextbox.value.trim(),
        Password: addPasswordTextbox.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: myHeaders,
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems(token);
            addNameTextbox.value = '';
            addPasswordTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: myHeaders,
    })
        .then(() => getItems(token))
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = users.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-password').value = item.password;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isManager').checked = item.isManager;
    document.getElementById('editForm').style.display = 'block';
}

// function updateItem() {
//     const itemId = document.getElementById('edit-id').value;
//     const item = {
//         id: parseInt(itemId, 10),
//         name: document.getElementById('edit-name').value.trim(),
//         password: document.getElementById('edit-password').value.trim(),
//         isManager: document.getElementById('edit-isManager').checked
//     };
//     fetch(`${uri}/${itemId}`, {
//         method: 'PUT',
//         headers: myHeaders,
//         body: JSON.stringify(item)
//     })
//         .then(() => getItems(token))
//         .catch(error => console.error('Unable to update item.', error));

//     closeInput();

//     return false;
// }

// function closeInput() {
//     document.getElementById('editForm').style.display = 'none';
// }

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'user' : 'user options';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isManagerCheckbox = document.createElement('input');
        isManagerCheckbox.type = 'checkbox';
        isManagerCheckbox.disabled = true;
        isManagerCheckbox.checked = item.isManager;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isManagerCheckbox);

        let td2 = tr.insertCell(1);
        let nameNode = document.createTextNode(item.name);
        td2.appendChild(nameNode);

        let td3 = tr.insertCell(2);
        let passwordNode = document.createTextNode(item.password);
        td3.appendChild(passwordNode);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    users = data;
}



