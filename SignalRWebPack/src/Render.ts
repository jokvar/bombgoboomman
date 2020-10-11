import { GameMap } from "./models/GameMap";
import { GameObjects } from "./models/GameObjects";
import { Player } from "./models/Player";
import { ChatHub } from "./chathub/ChatHub";
export namespace Renderer {
    var ctx = null;

    export class Renderer {
        tileW: number;
        tileH: number;
        mapW: number;
        mapH: number;
        currentSecond: number;
        frameCount: number;
        framesLastSecond: number;
        firstDraw: boolean;

        map: GameMap.Map;
        players: Player.Player;
        bomb: Array<GameObjects.Bomb>;
        powerups: Array<GameObjects.Powerup>;
        explosions: Array<GameObjects.Explosion>;
        messages: Array<ChatHub.Message>);

        constructor() {
            this.tileW = 40;
            this.tileH = 40;
            this.mapW = 15;
            this.mapH = 15;
            this.currentSecond = 0;
            this.frameCount = 0;
            this.framesLastSecond = 0;
            this.firstDraw = false;
        }

        StoreDrawData(
            map: GameMap.Map,
            players: Array<Player.Player>,
            bombs: Array<GameObjects.Bomb>,
            powerups: Array<GameObjects.Powerup>,
            explosions: Array<GameObjects.Explosion>,
            messages: Array<ChatHub.Message>): void {
            if (this.firstDraw == false) {
                this.map = new GameMap.Map();
            }
            console.log("draw");
        }

        drawGame() {
            if (ctx == null) { return; }

            var sec = Math.floor(Date.now() / 1000);

            if (sec != this.currentSecond) {
                this.currentSecond = sec;
                this.framesLastSecond = this.frameCount;
                this.frameCount = 1;
            }
            else { this.frameCount++; }

            let y = 0;
            let x = 0;

            //PAKEISTI VELIAU!!!!sauktukassauktukassauktukas
            while (y < this.mapH) {
                y++;
                while (x < this.mapW) {
                    x++;
                    switch (this.map.GetTile(x, y).passable) {
                        case true:
                            ctx.fillStyle = "#e6ffe6";
                            break;
                        case false:
                            ctx.fillStyle = "#000000";
                    }
                    ctx.fillRect(x * this.tileW, y * this.tileH, this.tileW, this.tileH);
                }
            }

            ctx.fillStyle = "#ff0000";
            ctx.fillText("FPS: " + this.framesLastSecond, 10, 20);

            requestAnimationFrame(this.drawGame);
        }

    }

}