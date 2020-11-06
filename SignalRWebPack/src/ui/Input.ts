import { ChatHub } from "../chathub/ChatHub";
import { Player } from "../models/Player";

export namespace Input {
    export class EventListener {
        canvas: HTMLCanvasElement;
        divMessages: HTMLDivElement;
        tbMessage: HTMLInputElement;
        btnSend: HTMLButtonElement;
        server: ChatHub.Server;
        constructor(server: ChatHub.Server) {
            this.canvas = document.querySelector('#game');
            this.divMessages = document.querySelector("#divMessages");
            this.tbMessage = document.querySelector("#tbMessage");
            this.btnSend = document.querySelector("#btnSend");
            this.server = server;
            this.tbMessage.addEventListener("keyup", (e: KeyboardEvent) => {
                if (e.key === "Enter") {
                    var message = new ChatHub.Message(this.tbMessage.value);
                    this.tbMessage.value = "";
                    server.NewMessage(message);
                }
            });

            document.addEventListener("keydown", (e: KeyboardEvent) => {
                if (e.key === "ArrowRight" || e.key === "Right" || e.keyCode === 39) {
                    var action = new Player.PlayerAction(Player.ActionEnum.Right)
                    server.SendInput(action);
                }
                if (e.key === "ArrowUp" || e.key === "Up" || e.keyCode === 38) {
                    var action = new Player.PlayerAction(Player.ActionEnum.Up)
                    server.SendInput(action);
                }
                if (e.key === "ArrowLeft" || e.key === "Left" || e.keyCode === 37) {
                    var action = new Player.PlayerAction(Player.ActionEnum.Left)
                    server.SendInput(action);
                }
                if (e.key === "ArrowDown" || e.key === "Down" || e.keyCode === 40) {
                    var action = new Player.PlayerAction(Player.ActionEnum.Down)
                    server.SendInput(action);
                }
                if (e.key === " " || e.key === "Space" || e.keyCode === 32) {
                    var action = new Player.PlayerAction(Player.ActionEnum.PlaceBomb)
                    server.SendInput(action);
                }
            });
        }
    }
}