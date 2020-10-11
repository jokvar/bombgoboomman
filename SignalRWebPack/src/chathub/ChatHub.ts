import * as signalRlib from "@microsoft/signalr";
import { GameMap } from "../models/GameMap";
import { GameObjects } from "../models/GameObjects";
import { Player } from "../models/Player";
import { Renderer } from "../Render"
export namespace ChatHub {
    export class Hub {
        connection: signalRlib.HubConnection;
        renderer: Renderer.Renderer;
        start: boolean = false;
        server: Server;
        constructor(renderer: Renderer.Renderer) {
            this.connection = new signalRlib.HubConnectionBuilder()
                .withUrl("/chathub")
                .configureLogging(signalRlib.LogLevel.Information)
                .build();
            console.log("hub constructor");
            this.connection.on("StoreDrawData",
                (map: GameMap.Map, players: Array<Player.Player>,
                    bombs: Array<GameObjects.Bomb>, powerups: Array<GameObjects.Powerup>,
                    explosions: Array<GameObjects.Explosion>, messages: Array<Message>) => {
                    console.log("StoreDrawData");
                    renderer.StoreDrawData(map, players, bombs, powerups, explosions, messages);
            });
            this.connection.on("StartPlaying", () => {
                console.log("StartPlaying");
                this.start = true;
            });
            this.connection.on("messageReceived", (username: string, message: string) => {
                console.log(username + ": " + message);
                //not implemented yet
                //renderer.newMessage(username, message);
            });
        }
    }
    export class Message {
        content: string;
        code: number;
        constructor(content: string, code: number) {
            this.content = content;
            this.code = code;
        }
    }
    class Server {
        connection: signalRlib.HubConnection;
        constructor(connection: signalRlib.HubConnection) {
            this.connection = connection;
        }
        CreateSession(mapName: string): void {
            this.connection.send("CreateSession", mapName)
                .then(() => console.log("mapName sent"));
        }
        JoinSession(roomCode: string): void {
            this.connection.send("JoinSession", roomCode)
                .then(() => console.log("roomCode sent"));
        }
        SendInput(input: Player.PlayerAction): void {
            this.connection.send("SendInput", input)
                .then(() => console.log("input sent"));
        }
        //not implemented yet
        //newMessage(mapname: string): void { } 

    }
}