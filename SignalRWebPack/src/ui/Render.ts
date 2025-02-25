﻿import { GameMap } from "../models/GameMap";
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
        textures: Array<String>;
        oldRenderer: oldRenderer.oldRenderer;
        
        divMessages: HTMLDivElement;
        tbodyMessages: HTMLTableSectionElement;

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
            this.tbodyMessages = document.querySelector("#tbodyMessages");
            
            this.textures = ["powerup", "powerup_bomb_naked", "powerup_plus"];

            //temp data, delete later
            this.map = new GameMap.Map();
            this.playerOne = new Player.Player(3, "player", false, 1, 1);
            this.playerTwo = new Player.Player(3, "playerTwo", false, 13, 13);
            this.bomb = new GameObjects.Bomb(7, 7, "bomb", 3, new Date("2019-01-16"), "#0d0d0d", this.textures);
            this.powerup = new GameObjects.Powerup(8, 8, "powerup", GameObjects.Powerup_type.BombDamage, 10, new Date("2019-01-16"), this.textures);
            this.explosion = new GameObjects.Explosion(9, 9, "explosion", 3, 2, 3, new Date("2019-01-16"), this.textures);
            this.players = [this.playerOne, this.playerTwo];
            this.bombs = [this.bomb];
            this.powerups = [this.powerup];
            this.explosions = [this.explosion];
        }

        //ResolveMessageType = (messageType: number) => {
        //    switch (messageType) {
        //        case 0:
        //            return "table-info";
        //        case 1:
        //            return "table-warning";
        //        case 2:
        //            return "table-danger";
        //        case 3:
        //            return "table-success";
        //        default:
        //            return "table-secondary";
        //    }
        //}

        DisplayMessage(username: string, content: string, _class: string) : void {
            var row = document.createElement("tr");
            //row.classList.add(this.ResolveMessageType(message.Type));
            row.classList.add(_class);
            row.innerHTML =
                "<td><b>" + username + "</b> : " + content + "</td>";
            this.tbodyMessages.appendChild(row);
            this.divMessages.scrollTop = this.divMessages.scrollHeight;
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
                    var img = document.getElementById(texture) as HTMLImageElement;
                    this.canvas.drawImage(img, x * this.tileW, y * this.tileH);
                    x++;
                }
                y++;
            }

            for (let p of this.players) {
                let t = p.texture;
                var img = document.getElementById(t) as HTMLImageElement;
                this.canvas.drawImage(img, p.x * this.tileW, p.y * this.tileH);
            }

            for (let b of this.bombs) {
                var img = document.getElementById("bomb") as HTMLImageElement;
                this.canvas.drawImage(img, b.x * this.tileW, b.y * this.tileH);
            }

            for (let pow of this.powerups) {
                var background = pow.textures[0];
                var foreground = pow.textures[1];
                var top = pow.textures[2];
                var img = document.getElementById(background as string) as HTMLImageElement;
                this.canvas.drawImage(img, pow.x * this.tileW, pow.y * this.tileH);
                if (foreground != null)
                {
                    var imgaa = document.getElementById(foreground as string) as HTMLImageElement;
                    this.canvas.drawImage(imgaa, pow.x * this.tileW, pow.y * this.tileH);
                }
                if (top != null)
                {
                    var imgaa = document.getElementById(top as string) as HTMLImageElement;
                    this.canvas.drawImage(imgaa, pow.x * this.tileW, pow.y * this.tileH);
                }
                
            }

            for (let e of this.explosions) {
                var img = document.getElementById("explosion") as HTMLImageElement;
                this.canvas.drawImage(img, e.x * this.tileW, e.y * this.tileH);
            }
            this.canvas.fillStyle = "#ff0000";

            if (!this.firstDraw) {
                //<img id="title_screen" src="images/title.png" width="450" height="450" hidden>
                var titleScreen = document.getElementById("title_screen") as HTMLImageElement;
                this.canvas.drawImage(titleScreen, 0, 0);
            }
            this.canvas.fillText("FPS: " + this.framesLastSecond, 10, 20);
            requestAnimationFrame(this.drawGame);
        }

    }

}