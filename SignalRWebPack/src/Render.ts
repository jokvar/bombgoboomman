import { GameMap } from "./models/GameMap";
import { GameObjects } from "./models/GameObjects";
import { Player } from "./models/Player";
import { ChatHub } from "./chathub/ChatHub";
export namespace Renderer {
    export class Renderer {
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
        players: Player.Player;
        bomb: Array<GameObjects.Bomb>;
        powerups: Array<GameObjects.Powerup>;
        explosions: Array<GameObjects.Explosion>;
        messages: Array<ChatHub.Message>;

        constructor(canvas: CanvasRenderingContext2D) {
            console.log("Renderer constructor");
            this.tileW = 20;
            this.tileH = 20;
            this.mapW = 15;
            this.mapH = 15;
            this.currentSecond = 0;
            this.frameCount = 0;
            this.framesLastSecond = 0;
            this.firstDraw = false;
            this.canvas = canvas;
            this.map = new GameMap.Map();
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
            //console.log("draw");
        }

        drawGame = () => {
            console.log("DrawGame()");
            if (this.canvas == null) { return; }
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
                x = 0;
                while (x < this.mapW) {
                    switch (this.map.GetTile(x, y).passable) {
                        case true:
                            console.log("FALSET");
                            this.canvas.fillStyle = "#ffffff";
                            break;
                        case false:
                            console.log("FALSE");
                            this.canvas.fillStyle = "#000000";
                            break;
                    }
                    this.canvas.fillRect(x * this.tileW, y * this.tileH, this.tileW, this.tileH);
                    x++;
                }
                console.log("MapH - " + this.mapH);
                console.log("YY - " + y);
                y++;
            }

            this.canvas.fillStyle = "#ff0000";
            this.canvas.fillText("FPS: " + this.framesLastSecond, 10, 20);
            console.log("draw dobne");
            requestAnimationFrame(this.drawGame);
        }

    }

}