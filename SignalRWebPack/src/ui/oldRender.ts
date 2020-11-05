import { GameMap } from "../models/GameMap";
import { GameObjects } from "../models/GameObjects";
import { Player } from "../models/Player";
import { ChatHub } from "../chathub/ChatHub";
export namespace oldRenderer
{
    export class oldRenderer
    {
        tileW: number;
        tileH: number;
        mapW: number;
        mapH: number;
        currentSecond: number;
        frameCount: number;
        framesLastSecond: number;
        firstDraw: boolean;

        public canvas: CanvasRenderingContext2D;

        map: GameMap.Map;
        players: Array<Player.Player>;
        bombs: Array<GameObjects.Bomb>;
        powerups: Array<GameObjects.Powerup>;
        explosions: Array<GameObjects.Explosion>;
        messages: Array<ChatHub.Message>;
        oldRenderer: oldRenderer.oldRenderer;


        divMessages: HTMLDivElement;
        tbMessage: HTMLInputElement;

        //temp data, delete later
        playerOne: Player.Player;
        playerTwo: Player.Player;
        bomb: GameObjects.Bomb;
        powerup: GameObjects.Powerup;
        explosion: GameObjects.Explosion;

        constructor() {
            
        }

        DisplayMessage = (username: string, message: string) => {
            var m = document.createElement("div");
            m.innerHTML =
                "<div class=\"message-author\">" + username + "</div><div>" + message + "</div>";
            this.divMessages.appendChild(m);
            this.divMessages.scrollTop = this.divMessages.scrollHeight;
            this.tbMessage.value = "";
        }

        StoreData(
            map: GameMap.Map,
            players: Array<Player.Player>,
            bombs: Array<GameObjects.Bomb>,
            powerups: Array<GameObjects.Powerup>,
            explosions: Array<GameObjects.Explosion>,
            messages: Array<ChatHub.Message>): void {

            //init or update map
            if (this.firstDraw == false) {
                this.map = new GameMap.Map();
                this.firstDraw = true;
            }
            else {
                for (let tile of map.tiles) {
                    this.map.UpdateTile(tile);
                }
            }

            this.players = players;
            this.bombs = bombs;
            this.powerups = powerups;
            this.explosions = explosions;
            this.messages = messages;
        }

        random(max: number) {
            return Math.floor(Math.random() * Math.floor(max));
        }
    }
}