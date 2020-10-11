import "./css/main.css";
import * as signalR from "@microsoft/signalr";
import { ChatHub } from "./chathub/ChatHub";
import { Renderer } from "./Render";

const version = "v0.3.1.1";

window.onload = function () {

    var canvas = (document.getElementById('game') as HTMLCanvasElement).getContext("2d");
    (document.getElementById('version') as HTMLDivElement).textContent = version;
    canvas.font = "bold 10pt sans-serif";
    var renderer: Renderer.Renderer = new Renderer.Renderer(canvas);
    console.log(renderer);
    var chathub: ChatHub.Hub = new ChatHub.Hub(renderer);
    renderer.drawGame(); 
    console.log("requtested frame");
};
