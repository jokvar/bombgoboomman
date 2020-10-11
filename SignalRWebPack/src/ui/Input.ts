import { ChatHub } from "../chathub/ChatHub";
import { Player } from "../models/Player";

export namespace Input {
    export class EventListener {
        canvas: HTMLCanvasElement;
        divMessages: HTMLDivElement;
        tbMessage: HTMLInputElement;
        btnSend: HTMLButtonElement;
        server: ChatHub.Server;
        username: string
        constructor(server: ChatHub.Server) {
            this.canvas = document.querySelector('#game');
            this.divMessages = document.querySelector("#divMessages");
            this.tbMessage = document.querySelector("#tbMessage");
            this.btnSend = document.querySelector("#btnSend");
            this.server = server;
            this.username = server.username;
            this.tbMessage.addEventListener("keyup", (e: KeyboardEvent) => {
                if (e.key === "Enter") {
                    console.log("eventEnter: " + this.username + "- " + this.tbMessage.value);
                    var message = new ChatHub.Message(this.tbMessage.value);
                    server.NewMessage(this.username, message);
                }
            });
            this.tbMessage.addEventListener("keyup", (e: KeyboardEvent) => {
                if (e.key === "Space") {
                    console.log("arrow right");
                    var action = new Player.PlayerAction(Player.ActionEnum.Right)
                    server.SendInput(action);
                }
            });
        }
    }
}