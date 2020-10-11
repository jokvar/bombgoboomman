import "./css/main.css";
import * as signalR from "@microsoft/signalr";
import { ChatHub } from "./chathub/ChatHub";
import { Renderer } from "./Render";

window.onload = function () {
    var canvas = (document.getElementById('game') as any).getContext("2d");
    //console.log("got id of canvas")
    //console.log(canvas)
    canvas.font = "bold 10pt sans-serif";
    var renderer: Renderer.Renderer = new Renderer.Renderer(canvas);
    var chathub: ChatHub.Hub = new ChatHub.Hub(renderer);
    renderer.drawGame(); 
    console.log("requtested frame");
};
