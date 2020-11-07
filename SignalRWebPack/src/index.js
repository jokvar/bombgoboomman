"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("./css/main.css");
require("./images/wall.jpg");
require("./images/box.jpg");
require("./images/blank.jpg");
require("./images/bomb.jpg");
require("./images/explosion.jpg");
require("./images/player.jpg");
require("./images/playerTwo.jpg");
require("./images/playerThree.jpg");
require("./images/playerFour.jpg");
require("./images/powerup.jpg");
var ChatHub_1 = require("./chathub/ChatHub");
var Render_1 = require("./ui/Render");
var Input_1 = require("./ui/Input");
var version = "v0.21";
window.onload = function () {
    var canvas = document.getElementById('game').getContext("2d");
    document.getElementById('version').textContent = version;
    canvas.font = "bold 10pt sans-serif";
    var renderer = new Render_1.Renderer.Renderer(canvas);
    var chathub = new ChatHub_1.ChatHub.Hub(renderer);
    chathub.connection.start();
    var input = new Input_1.Input.EventListener(chathub.server);
    renderer.drawGame();
    console.log("requtested frame");
};
