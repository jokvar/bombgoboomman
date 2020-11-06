import { GameMap } from "../models/GameMap";
import { GameObjects } from "../models/GameObjects";
import { Player } from "../models/Player";
import { ChatHub } from "../chathub/ChatHub";
import { Renderer } from "../ui/Render";
export namespace oldRenderer
{
    export class oldRenderer
    {
        //old data, used in drawGame method. not used.
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

        
        playerOne: Player.Player;
        playerTwo: Player.Player;
        bomb: GameObjects.Bomb;
        powerup: GameObjects.Powerup;
        explosion: GameObjects.Explosion;

        constructor() {
            
        }

        StoreData(
            map: GameMap.Map,
            players: Array<Player.Player>,
            bombs: Array<GameObjects.Bomb>,
            powerups: Array<GameObjects.Powerup>,
            explosions: Array<GameObjects.Explosion>,
            messages: Array<ChatHub.Message>,
            render: Renderer.Renderer): void {

            //init or update map
            if (render.firstDraw == false) {
                render.map = new GameMap.Map();
                render.firstDraw = true;
            }
            else {
                for (let tile of map.tiles) {
                    render.map.UpdateTile(tile);
                }
            }

            render.players = players;
            render.bombs = bombs;
            render.powerups = powerups;
            render.explosions = explosions;
            render.messages = messages;
        }

        random(max: number) {
            return Math.floor(Math.random() * Math.floor(max));
        }

        drawGame = () => {
            console.log("DrawGame()");
            var sec = Math.floor(Date.now() / 1000);

            if (sec != this.currentSecond) {
                this.currentSecond = sec;
                this.framesLastSecond = this.frameCount;
                this.frameCount = 1;
            }
            else { this.frameCount++; }

            let y = 0;
            let x = 0;
            console.log("fps count");
            while (y < this.mapH) {
                y++;
                while (x < this.mapW) {
                    x++;
                    console.log(this.map);
                    var tile = this.map.GetTile(0, 0);
                    console.log(tile);
                    switch (this.map.GetTile(x, y).passable) {
                        case true:
                            this.canvas.fillStyle = "#e6ffe6";
                            break;
                        case false:
                            this.canvas.fillStyle = "#000000";
                    }
                    this.canvas.fillRect(x * this.tileW, y * this.tileH, this.tileW, this.tileH);
                }
            }

            this.canvas.fillStyle = "#ff0000";
            this.canvas.fillText("FPS: " + this.framesLastSecond, 10, 20);
            console.log("draw dobne");
            requestAnimationFrame(this.drawGame);
        }
    }
}