let properties = [];

function _displayproperties(data) {
    let dropdown = document.getElementById('property-dropdown');
    dropdown.length = 0;

    let defaultOption = document.createElement('option');
    defaultOption.text = 'Seleccione una Propiedad';

    dropdown.add(defaultOption);
    dropdown.selectedIndex = 0;
    let option;

    for (let i = 0; i < data.length; i++) {
        option = document.createElement('option');
        option.text = data[i].title;
        option.value = data[i].id;
        dropdown.add(option);
    }

    properties = data;
}