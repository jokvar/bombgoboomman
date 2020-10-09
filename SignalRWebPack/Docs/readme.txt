-----
1. client loads page
2. client makes connection
3. client invokes server.RegisterPlayer()
4. server invokes client.DrawMenu(object that details menu shape, images, style)
5. client involes server.CreateSession(mapname) //vvia buttonpress on canvas "create room", having SOMEHOW selected a room
6. server generates room_code, creates a Player, saves it and also saves it as HOST. Server invokes client.DrawSession(map, gameobjects, players, etc, room_code) 
7. client starts forever drawing the map and all related information. when new players join they are also sent. The ui looks pretty much like what it will look like during gameplay
8. once enough players have joined (4 or 3) server invokes client.StartPlaying()
9. client starts recording inputs and sendign them to the server every so often, while still drawing the map, using server.SendInput(inputObjectOrSomething).
10. server keeps periodically invoking client.DrawSession(map, gameobjects, players, etc)
-----
5. client invokes server.JoinSession(room_code), having inputted a room_code
6. server creates a Player, saves it and assigns it to session based on room code. Server checks if there are enough players. Server invokes client.DrawSession(map, gameobjects, players, etc, room_code)
7. client starts forever drawing the map and all related information. when new players join they are also sent. The ui looks pretty much like what it will look like during gameplay
8. once enough players have joined (4 or 3) server invokes client.StartPlaying()
9. client starts recording inputs and sendign them to the server every so often, while still drawing the map, using server.SendInput(inputObjectOrSomething).
10. server keeps periodically invoking client.DrawSession(map, gameobjects, players, etc)

//server
server.RegisterPlayer() {
	server knows a new person has just connected to the game. server will form and send the client a response with all data needed to draw the ui.
}

server.CreateSession(mapname) {
	server knows a client wants to create a new session. server received an identifier for the selected map. server generates a room code, creates a Player and also stores it seperately as Host in the session (for reasons currently unknown, the host is important (its not, the host could DC and the game would continue p much fine))
}

server.JoinSession(room_code) {
	server knows a client want st oconnect to a session. server received the room code. server creates a Player and adds it to the session. //niga
}

server.SendInput(inputObjectOrSomething) {
	a game is in progress. a client has sent the server its input data, containing a direction and a bomb placement flag. both of these can be null/empty, meaning no movement.
	the server will pass this information to the gameloop... which will need to run on a seperate thread lmao yikes aight not too bad. we did this in concurrent programming 1 lmao. this is probably a hint for the singleton design pattern.
	anyway gameloop will process this information and send it back periodically. i can already foresee out of syncs, but they shouldnt be more than a few net frames one way or the other. 
}

//client
client.DrawMenu(object that details menu shape, images, style) {
	client is sent information on how the menu should look, and draws the menu. the menu should have some room code god i hope (google input field in canvas?), and two buttons, create room and join room. god save us all. it can also just be html elements thats fine actually fuck the canvas
}

client.DrawSession(map, gameobjects, players, etc, room_code) {
	client starts constantly redrawing the map. or maybe whenever server updates idk. cause javascript inputs are invoked on new threads, and the server response should, too, be a seperate thread by default i imagine? so the client, after having received this, starts a script that will continue forever, or like maybe until the next time it received this idk doesnt matter. i guess its fine for the client to only draw after receiving? but that might be too slow too, fuck. weeeeew.
}

client.StartPlaying() {
	having received this, the client starts sending inputs to the server. this is the official start of the session.
}

