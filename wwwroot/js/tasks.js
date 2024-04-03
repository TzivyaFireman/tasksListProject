const uri = '/task';
let tasks = [];
let token = localStorage.getItem("token");


const moveToUsersDetails = async () => {

    const tokenParts = token.split('.');
    const payload = JSON.parse(atob(tokenParts[1]));
    const userType = payload.type;
    let linkButtonToUsers = document.getElementById('linkButtonToUsers');
    if (userType[1] === "admin") {
        linkButtonToUsers.style.display = 'block';
    }
}
moveToUsersDetails()

const tokenExpired = () => {
    // if (token) {
    // const tokenData = jwt_decode(token);
    // const expirationTime = tokenData.exp * 1000
    alert("פג הסשן של המשתמש");
    window.location.href = '/index.html';
}


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
        .catch(error => {
            console.error('Unable to get items.', error);
            tokenExpired();
        });
}

getItems(token);

function addItem() {
    const addNameTextbox = document.getElementById('add-name');

    const item = {
        isDone: false,
        name: addNameTextbox.value.trim()
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
        })
        .catch(
            error => {
                console.error('Unable to add item.', error);
                tokenExpired();
            });
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: myHeaders,
    })
        .then(() => getItems(token))
        .catch(error => {
            console.error('Unable to delete item.', error);
            tokenExpired();
        });
}

function displayEditForm(id) {
    const item = tasks.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isDone').checked = item.isDone;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isDone: document.getElementById('edit-isDone').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: myHeaders,
        body: JSON.stringify(item)
    })
        .then(() => getItems(token))
        .catch(error => {
            console.error('Unable to update item.', error);
            tokenExpired();
        });

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'task' : 'task options';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = item.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    tasks = data;
}




