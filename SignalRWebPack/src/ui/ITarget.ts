import { GameMap } from "../models/GameMap";
import { GameObjects } from "../models/GameObjects";
import { Player } from "../models/Player";
import { ChatHub } from "../chathub/ChatHub";

export namespace ITarget
{
    export interface ITarget {
        DisplayMessage(username: string, message: string);
        StoreDrawData(map: GameMap.Map, players: Array<Player.Player>, bombs: Array<GameObjects.Bomb>, powerups: Array<GameObjects.Powerup>, explosions: Array<GameObjects.Explosion>, messages: Array<ChatHub.Message>);
        drawGame();
        rand(max: number);
    }
}
