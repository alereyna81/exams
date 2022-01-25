const uri_act = 'api/Activity';
const uri_prop = 'api/Property';

function getItems() {
	Promise.all([
		fetch(uri_act),
		fetch(uri_prop)
	]).then(function (responses) {
		// Uso de Promise para llamadas en paralelo - Alejandra Reyna
		return Promise.all(responses.map(function (response) {
			return response.json();
		}));
	}).then(function (data) {
        _displayActivities(data[0]);
        _displayproperties(data[1]);

		console.log(data);
	}).catch(function (error) {
		console.log(error);
	});
}

