import * as signalRlib from "@microsoft/signalr";
import { GameMap } from "../models/GameMap";
import { GameObjects } from "../models/GameObjects";
import { Player } from "../models/Player";
import { Renderer } from "../ui/Render"
export namespace ChatHub {
    export class Hub {
        connection: signalRlib.HubConnection;
        renderer: Renderer.Renderer;
        start: boolean = false;
        server: Server;
        username: string
        constructor(renderer: Renderer.Renderer) {
            this.connection = new signalRlib.HubConnectionBuilder()
                .withUrl("/chathub")
                .configureLogging(signalRlib.LogLevel.Information)
                .build();
            this.username = new Date().getTime().toString();
            this.server = new Server(this.connection, this.username);
            this.connection.on("StoreDrawData",
                (map: GameMap.Map, players: Array<Player.Player>,
                    bombs: Array<GameObjects.Bomb>, powerups: Array<GameObjects.Powerup>,
                    explosions: Array<GameObjects.Explosion>, messages: Array<Message>) => {
                    //console.log("StoreDrawData");
                    //console.log(map, players, bombs, powerups, explosions, messages);
                    renderer.StoreDrawData(map, players, bombs, powerups, explosions, messages);
            });
            this.connection.on("StartPlaying", () => {
                this.start = true;
            });
            this.connection.on("messageReceived", (username: string, message: Message) => {
                renderer.DisplayMessage(username, message.content, message.class);
            });
        }
    }
    export class Message {
        content: string;
        class: string;
        constructor(content: string) {
            this.content = content;
            this.class = "irrelevant";
        }       
    }
    export class Server {
        connection: signalRlib.HubConnection;
        username: string;
        constructor(connection: signalRlib.HubConnection, username: string) {
            this.connection = connection;
            this.username = username;
        }
        CreateSession(mapName: string): void {
            this.connection.send("CreateSession", mapName);
        }
        JoinSession(roomCode: string): void {
            this.connection.send("JoinSession", roomCode);
        }
        SendInput(input: Player.PlayerAction): void {
            this.connection.send("SendInput", input);
        }
        NewMessage(message: ChatHub.Message): void {
            this.connection.send("NewMessage", message);
        }
    }
}