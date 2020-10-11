import "./css/main.css";
import * as signalR from "@microsoft/signalr";
import { ChatHub } from "./chathub/ChatHub";
import { Renderer } from "./Render";

window.onload = function () {
    var canvas = (document.getElementById('game') as any).getContext("2d");
    console.log("got id of canvas")
    console.log(canvas)
    canvas.font = "bold 10pt sans-serif";
    let renderer = new Renderer.Renderer(canvas);
    let chathub = new ChatHub.Hub(renderer);
    requestAnimationFrame(renderer.drawGame);   
    console.log("requtested frame")
};
