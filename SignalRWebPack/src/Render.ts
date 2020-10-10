import { GameMap } from "./models/GameMap";
import { GameObjects } from "./models/GameObjects";
import { Player } from "./models/Player";
import { ChatHub } from "./chathub/ChatHub";
export namespace Renderer {
    export class Renderer {
        StoreDrawData(map: GameMap.Map,
            players: Array<Player.Player>,
            bombs: Array<GameObjects.Bomb>,
            powerups: Array<GameObjects.Powerup>,
            explosions: Array<GameObjects.Explosion>,
            messages: Array<ChatHub.Message>): void {
            console.log("draw");
        }
    }
}