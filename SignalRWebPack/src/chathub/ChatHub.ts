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
        constructor(renderer: Renderer.Renderer) {
            this.connection = new signalRlib.HubConnectionBuilder()
                .withUrl("/chathub")
                .configureLogging(signalRlib.LogLevel.Information)
                .build();    
            this.connection.on("DrawSession",
                (map: GameMap.Map, players: Array<Player.Player>,
                    bombs: Array<GameObjects.Bomb>, powerups: Array<GameObjects.Powerup>,
                    explosions: Array<GameObjects.Explosion>, messages: Array<Message>) => {
                    renderer.StoreDrawData(map, players, bombs, powerups, explosions, messages);
                });
            this.connection.on("StartPlaying", () => {
                this.start = true;
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
}