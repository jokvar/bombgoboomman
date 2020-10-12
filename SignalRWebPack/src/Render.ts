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
        players: Array<Player.Player>;
        bombs: Array<GameObjects.Bomb>;
        powerups: Array<GameObjects.Powerup>;
        explosions: Array<GameObjects.Explosion>;
        messages: Array<ChatHub.Message>;

        //temp data, delete later
        playerOne: Player.Player;
        playerTwo: Player.Player;
        bomb: GameObjects.Bomb;
        powerup: GameObjects.Powerup;
        explosion: GameObjects.Explosion;

        constructor(canvas: CanvasRenderingContext2D) {
            console.log("Renderer constructor");
            this.mapW = 15;
            this.mapH = 15;
            this.tileW = canvas.canvas.width / this.mapW;
            this.tileH = canvas.canvas.height / this.mapH;
            this.currentSecond = 0;
            this.frameCount = 0;
            this.framesLastSecond = 0;
            this.firstDraw = false;
            this.canvas = canvas;

            //temp data, delete later
            this.map = new GameMap.Map();
            this.playerOne = new Player.Player(3, "#66ff99", false, 1, 1);
            this.playerTwo = new Player.Player(3, "#66ff99", false, 13, 13);
            this.bomb = new GameObjects.Bomb(7, 7, "#0d0d0d", 3, new Date("2019-01-16"), "#0d0d0d");
            this.powerup = new GameObjects.Powerup(8, 8, "#ffff00", GameObjects.Powerup_type.BombDamage, 10, new Date("2019-01-16"));
            this.explosion = new GameObjects.Explosion(9, 9, "#ffffff", 3, 2, 3, new Date("2019-01-16"));
            this.players = [this.playerOne, this.playerTwo];
            this.bombs = [this.bomb];
            this.powerups = [this.powerup];
            this.explosions = [this.explosion];
        }

        StoreDrawData(
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

        rand(max: number) {
            return Math.floor(Math.random() * Math.floor(max));
        }

        drawGame = () => {
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
            while (y < this.mapH) {
                x = 0;
                while (x < this.mapW) {
                    let rand = Math.floor(Math.random() * Math.floor(4));
                    //checking drawing, delete later
                    if (rand == 1) {
                        this.map.GetTile(x, y).texture = "#006600";
                    }
                    if (rand == 2) {
                        this.map.GetTile(x, y).texture = "#ff0000";
                    }
                    if (rand == 3) {
                        this.map.GetTile(x, y).texture = "#003399";
                    }
                    this.canvas.fillStyle = this.map.GetTile(x, y).texture;
                    this.canvas.fillRect(x * this.tileW, y * this.tileH, this.tileW, this.tileH);
                    x++;
                }
                y++;
            }

            for (let p of this.players) {
                this.canvas.fillStyle = p.texture;
                this.canvas.fillRect(p.x * this.tileW, p.y * this.tileH, this.tileW, this.tileH);
            }

            for (let b of this.bombs) {
                console.log("bomb");
                this.canvas.fillStyle = b.texture;
                this.canvas.fillRect(b.x * this.tileW, b.y * this.tileH, this.tileW, this.tileH);
            }

            for (let pow of this.powerups) {
                console.log("powerups");
                this.canvas.fillStyle = pow.texture;
                this.canvas.fillRect(pow.x * this.tileW, pow.y * this.tileH, this.tileW, this.tileH);
            }

            for (let e of this.explosions) {
                console.log("explosions");
                this.canvas.fillStyle = e.texture;
                this.canvas.fillRect(e.x * this.tileW, e.y * this.tileH, this.tileW, this.tileH);
            }
            this.canvas.fillStyle = "#ff0000";
            this.canvas.fillText("FPS: " + this.framesLastSecond, 10, 20);
            console.log("draw dobne");
            requestAnimationFrame(this.drawGame);
        }

    }

}