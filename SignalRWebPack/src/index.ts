//import "./css/main.css";
import "./images/wall.jpg";
import "./images/box.jpg";
import "./images/blank.jpg";
import "./images/bomb.jpg";
import "./images/explosion.jpg";
import "./images/player.jpg";
import "./images/powerup.jpg";
import { ChatHub } from "./chathub/ChatHub";
import { Renderer } from "./ui/Render";
import { Input } from "./ui/Input";
import 'bootstrap/dist/css/bootstrap.min.css';


const version = "v0.5";

window.onload = function () {
    var canvas = (document.getElementById('game') as HTMLCanvasElement).getContext("2d");
    (document.getElementById('version') as HTMLDivElement).textContent = version;
    canvas.font = "bold 10pt sans-serif";
    var renderer: Renderer.Renderer = new Renderer.Renderer(canvas);
    var chathub: ChatHub.Hub = new ChatHub.Hub(renderer);
    chathub.connection.start();
    var input: Input.EventListener = new Input.EventListener(chathub.server);
    renderer.drawGame(); 
    console.log("requtested frame");
};
