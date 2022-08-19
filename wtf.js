function returnXp(defense,health){
	return (defense+0.5 * (health /4.5));
}
function returnProb(prob) {
	let x = [];
	for (let i = 0; i < 10000; i++) x.push(Math.random(prob) * 100 <= Math.abs(100 - prob));
	return [x.filter(e=> e == true).length]
}