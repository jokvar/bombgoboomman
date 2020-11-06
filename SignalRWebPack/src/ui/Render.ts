import { GameMap } from "../models/GameMap";
import { GameObjects } from "../models/GameObjects";
import { Player } from "../models/Player";
import { ChatHub } from "../chathub/ChatHub";
import { oldRenderer } from "../ui/oldRender";
import { ITarget } from "../ui/ITarget";
export namespace Renderer {
    export class Renderer implements ITarget.ITarget {
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
            this.oldRenderer = new oldRenderer.oldRenderer();

            this.divMessages = document.querySelector("#divMessages");
            this.tbMessage = document.querySelector("#tbMessage");

            //temp data, probably delete later?
            //temp data, delete later
            this.map = new GameMap.Map();
            this.playerOne = new Player.Player(3, "player", false, 1, 1);
            this.playerTwo = new Player.Player(3, "playerTwo", false, 13, 13);
            this.bomb = new GameObjects.Bomb(7, 7, "bomb", 3, new Date("2019-01-16"), "#0d0d0d");
            this.powerup = new GameObjects.Powerup(8, 8, "powerup", GameObjects.Powerup_type.BombDamage, 10, new Date("2019-01-16"));
            this.explosion = new GameObjects.Explosion(9, 9, "explosion", 3, 2, 3, new Date("2019-01-16"));
            this.players = [this.playerOne, this.playerTwo];
            this.bombs = [this.bomb];
            this.powerups = [this.powerup];
            this.explosions = [this.explosion];
        }

        DisplayMessage = (username: string, message: string) => {
            var m = document.createElement("div");
            m.innerHTML =
                "<div class=\"message-author\">" + username + "</div><div>" + message + "</div>";
            this.divMessages.appendChild(m);
            this.divMessages.scrollTop = this.divMessages.scrollHeight;
            this.tbMessage.value = "";
        }

        StoreDrawData(
            map: GameMap.Map,
            players: Array<Player.Player>,
            bombs: Array<GameObjects.Bomb>,
            powerups: Array<GameObjects.Powerup>,
            explosions: Array<GameObjects.Explosion>,
            messages: Array<ChatHub.Message>): void {

            this.oldRenderer.StoreData(map, players, bombs, powerups, explosions, messages, this);

            
        }

        rand(max: number) {
            return this.oldRenderer.random(max);
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
                    let texture = this.map.GetTile(x, y).texture;
                    var img = document.getElementById(texture) as HTMLCanvasElement;
                    this.canvas.drawImage(img, x * this.tileW, y * this.tileH);
                    x++;
                }
                y++;
            }

            for (let p of this.players) {
                let t = p.texture;
                var img = document.getElementById(t) as HTMLCanvasElement;
                this.canvas.drawImage(img, p.x * this.tileW, p.y * this.tileH);
            }

            for (let b of this.bombs) {
                var img = document.getElementById("bomb") as HTMLCanvasElement;
                this.canvas.drawImage(img, b.x * this.tileW, b.y * this.tileH);
            }

            for (let pow of this.powerups) {
                var img = document.getElementById("powerup") as HTMLCanvasElement;
                this.canvas.drawImage(img, pow.x * this.tileW, pow.y * this.tileH);
            }

            for (let e of this.explosions) {
                var img = document.getElementById("explosion") as HTMLCanvasElement;
                this.canvas.drawImage(img, e.x * this.tileW, e.y * this.tileH);
            }
            this.canvas.fillStyle = "#ff0000";
            this.canvas.fillText("FPS: " + this.framesLastSecond, 10, 20);
            requestAnimationFrame(this.drawGame);
        }

    }

}