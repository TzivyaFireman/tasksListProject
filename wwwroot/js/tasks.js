const uri = '/task';
let tasks = [];
let token = localStorage.getItem("token");

const root = document.documentElement
let isDarkMode = false

function setTheme(mode){
    if (mode === "light") {
        root.style.setProperty('--primary', '#f9db5f');
        root.style.setProperty('--secondary', '#243946');
        root.style.setProperty('--higlight', '#9f9da9');
        root.style.setProperty('--text', '#ffffff');
        root.style.setProperty('--oppositeText', 'black')
        document.body.style.backgroundImage =  'linear-gradient(to top right, #f9a743, #f9db5f'
    } else if (mode === "dark") {
        root.style.setProperty('--primary', '#00203f');
        root.style.setProperty('--secondary', '#ADEFD1FF');
        root.style.setProperty('--higlight', '#3a4a66');
        root.style.setProperty('--text', 'black');
        root.style.setProperty('--oppositeText', 'white')
        document.body.style.backgroundImage = 'linear-gradient(to top right, #00203f, #203552'
    }
}

document.getElementById("colourMode").addEventListener('click', () =>{
    isDarkMode = !isDarkMode;
    if (isDarkMode) {
        setTheme("dark");
    } else {
        setTheme("light");
    }
})

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
    document.getElementById('pageContainer').style.filter = 'blur(2px)';
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
    document.getElementById('pageContainer').style.filter = '';
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

        let checkBoxLabel = document.createElement('label')
        checkBoxLabel.setAttribute('class', 'checkBoxContainer')

        let editButton = button.cloneNode(false);
        editButton.setAttribute('class', 'editDeleteBtn')
        editButton.setAttribute('style', 'visibility: hidden')
        editButton.setAttribute('aria-label', 'Edit')
        editButton.innerHTML = `<svg class='editDeleteSvg' xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="size-6">
                                    <path d="M21.731 2.269a2.625 2.625 0 0 0-3.712 0l-1.157 1.157 3.712 3.712 1.157-1.157a2.625 2.625 0 0 0 0-3.712ZM19.513 8.199l-3.712-3.712-12.15 12.15a5.25 5.25 0 0 0-1.32 2.214l-.8 2.685a.75.75 0 0 0 .933.933l2.685-.8a5.25 5.25 0 0 0 2.214-1.32L19.513 8.2Z" />
                                </svg>`
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.setAttribute('class', 'editDeleteBtn')
        deleteButton.setAttribute('style', 'visibility: hidden')
        deleteButton.setAttribute('aria-label', 'Delete')
        deleteButton.innerHTML = `<svg class="editDeleteSvg" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className="size-6">
                                    <path fillRule="evenodd" d="M16.5 4.478v.227a48.816 48.816 0 0 1 3.878.512.75.75 0 1 1-.256 1.478l-.209-.035-1.005 13.07a3 3 0 0 1-2.991 2.77H8.084a3 3 0 0 1-2.991-2.77L4.087 6.66l-.209.035a.75.75 0 0 1-.256-1.478A48.567 48.567 0 0 1 7.5 4.705v-.227c0-1.564 1.213-2.9 2.816-2.951a52.662 52.662 0 0 1 3.369 0c1.603.051 2.815 1.387 2.815 2.951Zm-6.136-1.452a51.196 51.196 0 0 1 3.273 0C14.39 3.05 15 3.684 15 4.478v.113a49.488 49.488 0 0 0-6 0v-.113c0-.794.609-1.428 1.364-1.452Zm-.355 5.945a.75.75 0 1 0-1.5.058l.347 9a.75.75 0 1 0 1.499-.058l-.346-9Zm5.48.058a.75.75 0 1 0-1.498-.058l-.347 9a.75.75 0 0 0 1.5.058l.345-9Z" clipRule="evenodd" />
                                  </svg>`
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        tr.addEventListener('mouseover', () => {deleteButton.setAttribute("style", "visibility: visible", editButton.setAttribute("style", "visibility: visible"))})
        tr.addEventListener('mouseout', () => {deleteButton.setAttribute("style", "visibility: hidden", editButton.setAttribute("style", "visibility: hidden"))})
       
        let td1 = tr.insertCell(0);
        td1.appendChild(checkBoxLabel);
        checkBoxLabel.appendChild(isDoneCheckbox);

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




