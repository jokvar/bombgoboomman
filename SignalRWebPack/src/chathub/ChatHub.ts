import * as signalRlib from "@microsoft/signalr";
import { GameMap } from "../models/GameMap";
import { GameObjects } from "../models/GameObjects";
import { Player } from "../models/Player";
export namespace ChatHub {
    export class Hub {
        connection: signalRlib.HubConnection;

        constructor() {
            this.connection = new signalRlib.HubConnectionBuilder()
                .withUrl("/chathub")
                .configureLogging(signalRlib.LogLevel.Information)
                .build();
            this.connection.on("DrawSession",
                (map: GameMap.Map, players: Array<Player.Player>, bombs: Array<GameObjects.Bomb>, powerups: Array<GameObjects.Powerup>, explosions: Array<GameObjects.Explosion>, messages: Array<Message>)) => {

            }
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