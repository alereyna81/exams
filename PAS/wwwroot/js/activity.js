
//const uri = 'api/Activity';
let activities = [];


//function getItems() {
//    fetch(uri)
//        .then(response => response.json())
//        .then(data => _displayItems(data))
//        .catch(error => console.error('Error al obtener lista de Actividades.', error));
//}

function addItem() {
    const addNameTextbox = document.getElementById('add-title');
    const activitydateTextbox = document.getElementById('activitydate');
    alert(activitydateTextbox.value.trim());
    var _date = activitydateTextbox.value.trim().substring(0, 10);
    var _time = activitydateTextbox.value.trim().substring(11, 5); 

    const item = {      
        id: 0,
        property_id: parseInt('1', 10),
        schedule: _date, // + 'T' + _time, //new Date(),
        title: addNameTextbox.value.trim(),
        created_at: new Date(),
        updated_at: new Date(),
        status: "Active",
        property: "",
        condition: "",
        survey:""
    };

    fetch(uri_act, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
            activitydateTextbox.value = '';
        })
        .catch(error => console.error('Error al añadir la actividad.', error));
}

function deleteItem(id) { //Sólo cambiar el estatus a 'Inactivo', pero usaré DELETE
    fetch(`${uri_act}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Error al cancelar la actividad.', error));
}

function displayEditForm(id) {
    const item = activities.find(item => item.id === id);

    document.getElementById('title').value = item.title;
    document.getElementById('property_id').value = item.property;
    document.getElementById('id').value = item.id;
    document.getElementById('updateactivitydate').value = item.schedule.substring(0,10);
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('id').value;
    const propertyId = document.getElementById('property_id').value;

    const item = {
        id: parseInt(itemId, 10),
        property_id: propertyId,
        schedule: new Date(),
        title: document.getElementById('title').value.trim(),
        updated_at: new Date(),
        status: "Active"
    };

    fetch(uri_act, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
        })
        .catch(error => console.error('Error al actualizar la actividad.', error));

    closeEdition();

    return false;
}

function closeEdition() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayActivities(data) {
    const tBody = document.getElementById('activities');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(item => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Re-Agendar';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);
        editButton.setAttribute("style", "font-size: smaller;");
        
        if (item.status.trim() == 'Cancelled') {
            editButton.setAttribute("class", "btn btn-secondary");
            editButton.setAttribute("disabled", "true");
        } else {
            editButton.setAttribute("class", "btn btn-success");
        }

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Cancelar';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);
        deleteButton.setAttribute("style", "font-size: smaller;");
        if (item.status.trim() == "Cancelled") {
            deleteButton.setAttribute("class", "btn btn-secondary");
            deleteButton.setAttribute("disabled", "true");
        } else {
            deleteButton.setAttribute("class", "btn btn-danger");
        }

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(item.id);
        td1.appendChild(textNode1);
        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(item.schedule.substring(0,10));
        td2.appendChild(textNode2);
        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(item.title);
        td3.appendChild(textNode3);
        let td4 = tr.insertCell(3);
        let textNode4 = document.createTextNode(item.created_at.substring(0, 10));
        td4.appendChild(textNode4);
        let td5 = tr.insertCell(4);
        let textNode5 = document.createTextNode(item.status);
        td5.appendChild(textNode5);

        let td6 = tr.insertCell(5);
        let textNode6 = document.createTextNode(item.condition);
        td6.appendChild(textNode6);

        let td7 = tr.insertCell(6);
        let textNode7 = document.createTextNode(item.property);
        td7.appendChild(textNode7);

        let td8 = tr.insertCell(7);
        var link = "http://google.com";
        var element = document.createElement("a");
        element.setAttribute("href", link);
        element.innerHTML = "Details...";
        td8.appendChild(element);
  
        let td9 = tr.insertCell(8);
        td9.appendChild(editButton);

        let td10 = tr.insertCell(9);
        td10.appendChild(deleteButton);
    });

    activities = data;
    closeEdition();
}