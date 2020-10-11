import { Player } from "./Player";

export namespace GameMap {
    export class Map {
        tiles: Array<Tile>;
        width: number;
        mapData: number[] = [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1,
            0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0,
            1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 2, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1];
        index: number;

        constructor() {
            this.index = 0;
            this.width = 15;
            this.tiles = new Array<Tile>(225);
            this.mapData.forEach((value) => {
                var x = this.index % 15;
                var y = Math.floor(this.index / 15);

                if (value == 0) {
                    let tile = new EmptyTile(x, y, "#ffffff");
                    this.tiles[this.index] = tile;
                }
                if (value == 1) {
                    let tile = new Wall(x, y, "#000000");
                    this.tiles[this.index] = tile;
                }
                if (value == 2) {
                    let tile = new Box(x, y, "#ff661a");
                    this.tiles[this.index] = tile;
                }
                this.index++;
            });
        }

        GetTile(x: number, y: number) {
            let index = y * this.width + x;
            //console.log(x + " " + y);
            //console.log(index);
            //console.log(this.tiles[index]);
            return this.tiles[index];
        }

        UpdateTile(tile: Tile) {
            let index = tile.y * this.width;
            index = index + tile.x;
            this.tiles[index] = tile;
        }
    }
    export class Tile {
        x: number;
        y: number;
        texture: string;
        passable: boolean;
        destructable: boolean;

        constructor(x: number, y: number, texture: string) {
            this.x = x;
            this.y = y;
            this.texture = texture;
        }
    }
    export class Wall extends Tile {
        constructor(x: number, y: number, texture: string) {
            super(x, y, texture);
            this.passable = false;
            this.destructable = false;
        }
    }
    export class Box extends Tile {
        constructor(x: number, y: number, texture: string) {
            super(x, y, texture);
            this.passable = false;
            this.destructable = true;
        }
    }
    export class EmptyTile extends Tile {
        constructor(x: number, y: number, texture: string) {
            super(x, y, texture);
            this.passable = true;
            this.destructable = false;
        }
    }
}